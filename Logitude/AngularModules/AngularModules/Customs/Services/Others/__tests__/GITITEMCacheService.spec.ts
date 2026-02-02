import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

const confirmWindowInstances: ConfirmWindowStub[] = [];

class ConfirmWindowStub {
    public Yes = false;
    public No = false;
    public Show = jest.fn();
    private closedHandler: ((event: any) => void) | null = null;
    public WindowClosed = {
        subscribe: (handler: (event: any) => void) => {
            this.closedHandler = handler;
        }
    };
    constructor() {
        confirmWindowInstances.push(this);
    }
    emitClose() {
        if (this.closedHandler) {
            this.closedHandler({});
        }
    }
}

describe('GITITEMCacheService', () => {
    beforeEach(() => {
        jest.resetModules();
        confirmWindowInstances.length = 0;
        SessionLocator.Tenant = 'TENANT-1' as any;
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('returns voidDoNothing when item details match cache', async () => {
        const { service, OnItemCodeAddResult } = createService();
        const result = await service.OnItemCodeAdd(
            {
                ClassificationCode: 'CLS',
                ItemDescription: 'DESC',
                InvoiceQuantityType: 'UNIT',
                OriginCountryCode: 'IL'
            },
            {
                ClassificationCode: 'CLS',
                ItemDescription: 'DESC',
                InvoiceQuantityType: 'UNIT',
                OriginCountryCode: 'IL'
            }
        );

        expect(result).toBe(OnItemCodeAddResult.voidDoNothing);
        expect(confirmWindowInstances.length).toBe(0);
    });

    it('returns OverwriteRowFromDB when confirmation is accepted', async () => {
        const { service, OnItemCodeAddResult } = createService();
        const promise = service.OnItemCodeAdd(
            { ClassificationCode: 'CLS', ItemDescription: 'NEW' },
            { ClassificationCode: 'CLS', ItemDescription: 'OLD' }
        );

        const confirm = getLatestConfirmWindow();
        confirm.Yes = true;
        confirm.emitClose();

        await expect(promise).resolves.toBe(OnItemCodeAddResult.OverwriteRowFromDB);
    });

    it('returns AddTaskToUpdateDB when confirmation is declined', async () => {
        const { service, OnItemCodeAddResult } = createService();
        const promise = service.OnItemCodeAdd(
            { ClassificationCode: 'CLS', ItemDescription: 'NEW' },
            { ClassificationCode: 'CLS', ItemDescription: 'OLD' }
        );

        const confirm = getLatestConfirmWindow();
        confirm.No = true;
        confirm.emitClose();

        await expect(promise).resolves.toBe(OnItemCodeAddResult.AddTaskToUpdateDB);
    });
});

function createService() {
    jest.doMock('../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService', () => ({
        CustomsSettingExtendedListService: jest.fn().mockImplementation(() => ({
            GetDefault: jest.fn().mockReturnValue(of({ HasError: false, Result: { DefaultValue: 'N' } }))
        }))
    }));

    jest.doMock('../../../../Controls/Windows/ConfirmWindow', () => ({
        ConfirmWindow: ConfirmWindowStub
    }));

    const module = require('../GITITEMCacheService');
    (module.GITITEMCacheService as any)._instance = undefined;
    return {
        service: module.GITITEMCacheService.Instance,
        OnItemCodeAddResult: module.OnItemCodeAddResult
    };
}

function getLatestConfirmWindow(): ConfirmWindowStub {
    const instance = confirmWindowInstances.pop();
    if (!instance) {
        throw new Error('Expected confirm window to be created');
    }
    return instance;
}
