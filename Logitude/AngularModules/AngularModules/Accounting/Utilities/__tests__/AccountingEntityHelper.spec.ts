import { expect } from '@jest/globals';
import { AccountingEntityHelper, AccountingEntityValues } from '../AccountingEntityHelper';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

describe('AccountingEntityHelper', () => {
    beforeEach(() => {
        SessionLocator.SelectedSession = {
            SessionLocation: { viewContainerRef: {} }
        } as any;
        SessionLocator.DynamicLoader = {
            Load: jest.fn()
        } as any;
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('maps source type codes to icons', () => {
        expect(AccountingEntityHelper.getEntityIcon(AccountingEntityValues.Journal)).toBe('JR');
        expect(AccountingEntityHelper.getEntityIcon(AccountingEntityValues.ARInvoice)).toBe('IN');
        expect(AccountingEntityHelper.getEntityIcon(AccountingEntityValues.APPayment)).toBe('PY');
        expect(AccountingEntityHelper.getEntityIcon(AccountingEntityValues.TaxReport)).toBe('TR');
        expect(AccountingEntityHelper.getEntityIcon('UNKNOWN')).toBe('');
    });

    it('maps partner type codes to object table names', () => {
        expect(AccountingEntityHelper.GetPartnerTypeObjectTableName('AG')).toBe('Agent');
        expect(AccountingEntityHelper.GetPartnerTypeObjectTableName('CS')).toBe('Customer');
        expect(AccountingEntityHelper.GetPartnerTypeObjectTableName('VD')).toBe('Vendor');
        expect(AccountingEntityHelper.GetPartnerTypeObjectTableName('ZZ')).toBeUndefined();
    });

    it('resolves object table names for accounting entities', () => {
        expect(AccountingEntityHelper.getEntityObjectTableName(AccountingEntityValues.ARInvoice)).toBe('ARInvoice');
        expect(AccountingEntityHelper.getEntityObjectTableName(AccountingEntityValues.ChequeDeposit)).toBe('BankDeposit');
        expect(AccountingEntityHelper.getEntityObjectTableName(AccountingEntityValues.BankAdjustment)).toBe('ExternalReconciliation');
        expect(AccountingEntityHelper.getEntityObjectTableName('UNKNOWN')).toBe('Journal');
    });

    it('opens partner cards using dynamic loader', async () => {
        const runMock = jest.fn();
        (SessionLocator.DynamicLoader.Load as jest.Mock).mockResolvedValue({
            instance: {
                ComponentRef: null,
                Run: runMock,
                BackCompleted: { subscribe: jest.fn() }
            }
        });

        AccountingEntityHelper.OpenCard('CARD-1', 'Others', 'TAB-1');
        await Promise.resolve();

        expect(SessionLocator.DynamicLoader.Load).toHaveBeenCalledWith(
            './Infrastructure/Components/EditComponent/EditComponent',
            SessionLocator.SelectedSession.SessionLocation.viewContainerRef
        );
        expect(runMock).toHaveBeenCalledWith({
            EntityId: 'CARD-1',
            ObjectTableName: 'Vendor',
            SelectedTabCode: 'TAB-1'
        });
    });

    it('does not load when card id is empty', () => {
        AccountingEntityHelper.OpenCard('', 'Customer', 'TAB-1');
        expect(SessionLocator.DynamicLoader.Load).not.toHaveBeenCalled();
    });
});
