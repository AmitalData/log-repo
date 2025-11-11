import { expect } from '@jest/globals';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { buildComponent, createSuccessfulResponse } from './helpers/component-builder';
import { createAgentSharedManifestPMServiceMock, createShipmentPMServiceMock } from './helpers/service-stubs';

jest.mock('../../../Controls/Windows/MessageWindow', () => ({
    MessageWindow: jest.fn().mockImplementation(() => ({
        Width: 0,
        Height: 0,
        Title: '',
        Show: jest.fn(),
        InjectWindowComponent: jest.fn(),
    })),
}));

jest.mock('../../../Shipment/Validators/ShipmentValidator', () => ({
    ShipmentValidator: jest.fn().mockImplementation(() => ({
        Validate: jest.fn().mockReturnValue([])
    }))
}));

describe('SharedManifestAdditionalComponent OnCreate', () => {
    beforeEach(() => {
        jest.spyOn(TextCodeTranslator, 'Translate').mockImplementation(key => {
            if (key === 'General.M.FieldIsRequired') {
                return '%FieldName required';
            }
            return key;
        });
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    const createBaseComponent = () => {
        const { component, sessionMocks } = buildComponent({
            otherServices: {
                myShipmentPMService: createShipmentPMServiceMock(createSuccessfulResponse({ Id: 'SHIP-123' })),
                _agentSharedManifestPMService: createAgentSharedManifestPMServiceMock(createSuccessfulResponse({}))
            }
        });

        SessionLocator.TenantPM = { IsDocumentsArchive: false } as any;
        SessionInfo.LoggedUserTenant = 7;

        (global as any).window = (global as any).window || {};
        (global as any).window.ObjectTables = [
            { Name: 'Shipment', Id: 'SHIPMENT' },
            { Name: 'Master', Id: 'MASTER' }
        ];
        (global as any).window.ObjectFields = [];

        (component as any).CurrentEntity = {
            SharedManifestTranslations: [],
            AgentReference: null
        } as any;
        (component as any).AgentSideData = {
            Notify1: null,
            Consignee: null,
            Shipper: null,
            PackageTypes: []
        } as any;
        (component as any).ManifestSL = {} as any;
        (component as any).ValidationErrorsList = [];

        return { component, sessionMocks };
    };

    it('collects validation errors and stops busy indicator when mandatory data missing', () => {
        const { component, sessionMocks } = createBaseComponent();
        component.EntityPM.ShipmentLevelCode = 'C';
        component.ShipperId = null;
        (component as any).ConsigneeId = null;

        component.OnCreate();

        expect(sessionMocks.session.StartBusyIndicator).toHaveBeenCalledWith('Creating...');
        expect(sessionMocks.session.StopBusyIndicator).toHaveBeenCalled();
        expect(component.ValidationErrorsList).toContain('Shipment.F.ShipperId required');
        expect(component.ValidationErrorsList).toContain('Shipment.F.ConsigneeId required');
    });

    it('adds translations for manually added codes before saving', () => {
        const { component, sessionMocks } = createBaseComponent();
        jest.spyOn(ServiceLocator, 'SendTotangoUserActivity').mockImplementation(() => undefined);

        component.ShipperId = 'SHIPPER';
        (component as any).ConsigneeId = null;
        component.EntityPM.ShipmentLevelCode = 'M';
        component.EntityPM.IncotermId = 'INCID';
        (component as any).IncotermCode = 'INC-CODE';
        (component as any).AgentSideData = {
            IncotermCode: 'INC-AGENT',
            IncotermAddedManually: true,
            CarrierCode: 'CAR-001',
            CarrierAddedManually: true,
            MoveTypeCode: null,
            ValueOfGoodsCurrencyCode: null,
            Notify1: null
        } as any;
        (component as any).ManifestSL = {
            CarrierAddedManually: true
        } as any;
        (component as any).MainCarriageCarrierCode = 'CARRIER-CODE';
        component.EntityPM.MainCarriageCarrierId = 'CARRIER-ID';
        component.EntityPM.MoveTypeId = 'MOVE';
        component.EntityPM.ValueOfGoodsCurrencyId = 'CUR';
        component.EntityPM.Notify1Id = 'N1';
        (component as any).IsHideTransshipment1Carrier = true;
        (component as any).IsHideTransshipment2Carrier = true;
        (component as any).IsHideTransshipment3Carrier = true;
        (component as any).IsHideMainCarriageVessel = true;
        (component as any).IsHideTransshipment1Vessel = true;
        (component as any).IsHideTransshipment2Vessel = true;
        (component as any).IsHideTransshipment3Vessel = true;
        (component as any).IsHideMainInterline = true;

        const translationSpy = jest.spyOn(component, 'InsertAndUpdateTransLation');

        component.OnCreate();

        expect(sessionMocks.session.StartBusyIndicator).toHaveBeenCalledWith('Creating...');
        expect(translationSpy).toHaveBeenCalledWith('INC-AGENT', 'Incoterm', 'INC-CODE');
        expect(translationSpy).toHaveBeenCalledWith('CAR-001', 'Carrier', 'CARRIER-CODE');
        expect(((component as any).CurrentEntity.SharedManifestTranslations as any[]).length).toBeGreaterThan(0);
    });
});
