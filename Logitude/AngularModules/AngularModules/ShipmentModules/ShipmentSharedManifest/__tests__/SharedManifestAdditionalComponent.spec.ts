import { expect } from '@jest/globals';
import { buildComponent } from './helpers/component-builder';
import { createAddressListServiceMock, createCardListServiceMock } from './helpers/service-stubs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SharedManifestTranslationPM } from '../../../Common/EntityPMs/SharedManifestTranslationPM';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';

const flushPromises = () => new Promise(resolve => setTimeout(resolve, 0));

describe('SharedManifestAdditionalComponent (Jest)', () => {
    beforeEach(() => {
        SessionInfo.LoggedUserTenant = 777;
    });

    afterEach(() => {
        SessionInfo.LoggedUserTenant = undefined as unknown as number;
        jest.clearAllMocks();
    });

    describe('InsertAndUpdateTransLation', () => {
        it('creates a new translation when none exists', () => {
            const { component } = buildComponent();

            component.InsertAndUpdateTransLation('AG01', 'Shipment', 'CODE-1');

            expect(component.CurrentEntity.SharedManifestTranslations).toHaveLength(1);
            const translation = component.CurrentEntity.SharedManifestTranslations[0];
            expect(translation.AgentCode).toBe('AG01');
            expect(translation.ObjectTableName).toBe('Shipment');
            expect(translation.MyCode).toBe('CODE-1');
            expect(translation.ChangeSetOp).toBe('Insert');
            expect(translation.Tenant).toBe(SessionInfo.LoggedUserTenant);
        });

        it('updates an existing translation when present', () => {
            const existingTranslation = new SharedManifestTranslationPM();
            existingTranslation.AgentCode = 'AG02';
            existingTranslation.ObjectTableName = 'Shipment';
            existingTranslation.MyCode = 'OLD';
            existingTranslation.ChangeSetOp = 'Insert';

            const currentEntity = new AgentSharedManifestPM();
            currentEntity.SharedManifestTranslations = [existingTranslation];

            const { component } = buildComponent({ currentEntity });

            component.InsertAndUpdateTransLation('AG02', 'Shipment', 'NEW-CODE');

            expect(component.CurrentEntity.SharedManifestTranslations).toHaveLength(1);
            const translation = component.CurrentEntity.SharedManifestTranslations[0];
            expect(translation.MyCode).toBe('NEW-CODE');
            expect(translation.ChangeSetOp).toBe('Update');
        });
    });

    describe('StopBusyIndicator', () => {
        it('stops busy indicator and hides sections when all data loaded', () => {
            const { component, sessionMocks } = buildComponent();

            component.IsLoadedCarrierTranslation = true;
            component.IsLoadedIncotermTranslation = true;
            component.IsLoadedShipperTranslation = true;
            component.IsLoadedConsigneeTranslation = true;
            component.IsLoadedShiperDefaultValues = true;
            component.IsLoadedConsigneeDefaultValues = true;
            component.IsLoadedTransshipment1CarrierTranslation = true;
            component.IsLoadedTransshipment2CarrierTranslation = true;
            component.IsLoadedTransshipment3CarrierTranslation = true;
            component.IsLoadedMainCarriageVesselTranslation = true;
            component.IsLoadedTransshipment1VesselTranslation = true;
            component.IsLoadedTransshipment2VesselTranslation = true;
            component.IsLoadedTransshipment3VesselTranslation = true;
            component.IsLoadedMainCarriageInterlineTranslation = true;
            component.IsLoadedNotify1IdTranslation = true;
            component.IsLoadedNotify1DefaultValuesTranslation = true;
            component.IsLoadedPortsTranslation = true;
            component.IsLoadedPackageTranslation = true;
            component.IsLoadedMoveTypeTranslation = true;
            component.IsLoadedValueOfGoodsCurrencyTranslation = true;
            component.IsLoadedFromPickUpTranslation = true;
            component.IsLoadedToPickUpTranslation = true;
            component.IsLoadedFromDeliveryTranslation = true;
            component.IsLoadedToDeliveryTranslation = true;

            component.IsHaveFromPickUpTranslation = true;
            component.IsHaveToPickUpTranslation = true;
            component.IsHaveFromDeliveryTranslation = true;
            component.IsHaveToDeliveryTranslation = true;

            component.EntityPM.IncotermId = 'INC';
            component.EntityPM.MoveTypeId = 'MOVE';
            component.EntityPM.ValueOfGoodsCurrencyId = 'CUR';
            component.EntityPM.Transshipment1CarrierId = 'C1';
            component.EntityPM.Transshipment2CarrierId = 'C2';
            component.EntityPM.Transshipment3CarrierId = 'C3';
            component.EntityPM.Transshipment1VesselId = 'V1';
            component.EntityPM.Transshipment2VesselId = 'V2';
            component.EntityPM.Transshipment3VesselId = 'V3';

            component.StopBusyIndicator();

            expect(sessionMocks.session.CurrentWindow.StopBusyIndicator).toHaveBeenCalledTimes(1);
            expect(component.HideGeneralDetailsArea).toBe(true);
            expect(component.HideTransShipmentsDetailsArea).toBe(true);
            expect(component.HidePickupDetailsArea).toBe(true);
            expect(component.HideDeliveryDetailsArea).toBe(true);
        });
    });

    describe('SetSalesman', () => {
        it('uses shipper salesman when available', () => {
            const { component } = buildComponent();
            component.EntityPM.CreatedByUserId = 'CREATOR';
            (component as any).myShipperSalesmanId = 'SHIPPER-USER';

            component.SetSalesman();

            expect(component.EntityPM.SalesmanUserId).toBe('SHIPPER-USER');
        });

        it('falls back to creator when shipper salesman missing', () => {
            const { component } = buildComponent();
            component.EntityPM.CreatedByUserId = 'CREATOR';
            (component as any).myShipperSalesmanId = null;

            component.SetSalesman();

            expect(component.EntityPM.SalesmanUserId).toBe('CREATOR');
        });
    });

    describe('ShipperId setter', () => {
        it('populates shipper details from service', () => {
            const cardList = {
                Code: 'CARD-01',
                PartnerTypeId: 'PARTNER',
                PrimaryContactId: 'CONTACT',
                EnglishName: 'Test Shipper',
                Notes: 'Notes',
                MainAddressId: 'ADDR-1',
                PickAddressId: 'ADDR-2',
                KnownConsignor: 'KC001',
                KCExpirationDate: new Date('2025-01-01'),
                SalesmanUserId: 'SALESMAN-1'
            } as any;

            const addressList = {
                Id: 'ADDR-1',
                Street: 'Test'
            } as any;

            const cardListServiceMock = createCardListServiceMock(cardList);
            const addressListServiceMock = createAddressListServiceMock(addressList);

            const { component, serviceMocks } = buildComponent({
                cardListServiceMock,
                addressListServiceMock
            });

            component.ManifestSL = {} as ManifestSL;
            component.EntityPM.CreatedByUserId = 'CREATOR';

            component.ShipperId = 'CARD-01';

            expect(serviceMocks.cardListService.spies.getSingle).toHaveBeenCalledWith('CARD-01');
            expect(serviceMocks.addressListService.spies.getSingle).toHaveBeenCalledWith('ADDR-1');
            expect(component['ShipperCode']).toBe('CARD-01');
            expect(component.EntityPM.ShipperName).toBe('Test Shipper');
            expect(component.EntityPM.ShipperReference1).toBeUndefined();
            expect(component.ShipperAddressList).toEqual(addressList);
            expect(component.EntityPM.SalesmanUserId).toBe('SALESMAN-1');
        });

        it('clears shipper fields when id becomes empty', () => {
            const cardList = {
                Code: 'CARD-02',
                PartnerTypeId: 'PARTNER',
                PrimaryContactId: 'CONTACT',
                EnglishName: 'Another Shipper',
                Notes: 'Notes',
                MainAddressId: 'ADDR-3',
                PickAddressId: 'ADDR-4',
                KnownConsignor: 'KC002',
                KCExpirationDate: new Date('2026-01-01'),
                SalesmanUserId: 'SALESMAN-2'
            } as any;

            const cardListServiceMock = createCardListServiceMock(cardList);
            const addressListServiceMock = createAddressListServiceMock(null);

            const { component } = buildComponent({
                cardListServiceMock,
                addressListServiceMock
            });

            component.ShipperId = 'CARD-02';
            expect(component.EntityPM.ShipperName).toBe('Another Shipper');

            component.ShipperId = null;

            expect(component['ShipperCode']).toBeNull();
            expect(component.EntityPM.ShipperName).toBeNull();
            expect(component.ShipperAddressList).toBeNull();
            expect(component['ShipperPartnerTypeId']).toBeNull();
            expect(component['myShipperSalesmanId']).toBeNull();
        });
    });

    describe('LoadChildComponent', () => {
        it('invokes dynamic loader for additional and header screens', async () => {
            const { component, sessionMocks } = buildComponent({
                locations: [
                    { Code: 'GECO', viewContainerRef: {} },
                    { Code: 'SHCO', viewContainerRef: {} }
                ],
                agentSharedManifestList: []
            });

            component.ManifestSL = {} as ManifestSL;
            component.HouseEntity = null;
            component.IsLoadAdditionalScreen = true;
            component.IsLoadSharedManifestheaderScreen = false;

            component.LoadChildComponent();
            await flushPromises();

            expect(sessionMocks.dynamicLoader.Load).toHaveBeenCalledTimes(2);
            expect(sessionMocks.dynamicLoader.Load).toHaveBeenNthCalledWith(
                1,
                './Infrastructure/GenericComponents/GeneratedComponent',
                expect.any(Object)
            );
            expect(sessionMocks.dynamicLoader.Load).toHaveBeenNthCalledWith(
                2,
                './ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent',
                expect.any(Object)
            );

            const generatedComponentRef = await sessionMocks.dynamicLoader.Load.mock.results[0].value;
            expect(generatedComponentRef.instance.Run).toHaveBeenCalledWith(
                component.EntityPM,
                'Shipment',
                'SharedManifestAdditionalScreen'
            );

            const headerComponentRef = await sessionMocks.dynamicLoader.Load.mock.results[1].value;
            expect(headerComponentRef.instance.Run).toHaveBeenCalledWith(
                component.CurrentEntity,
                component.AgentSharedManifestList
            );

            expect(component.IsLoadAdditionalScreen).toBe(false);
            expect(component.IsLoadSharedManifestheaderScreen).toBe(true);
        });
    });
});

