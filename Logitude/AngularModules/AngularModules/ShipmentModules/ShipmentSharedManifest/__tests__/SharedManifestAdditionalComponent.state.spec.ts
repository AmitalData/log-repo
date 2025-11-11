import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { buildComponent } from './helpers/component-builder';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentPickUpDeliverySL } from '../../../Common/DataContracts/ShipmentPickUpDeliverySL';
import { createServiceResponse } from './helpers/service-stubs';

const createAgentData = (overrides: Partial<Record<string, any>> = {}) => ({
    IncotermCode: null,
    ValueOfGoods: null,
    DescriptionOfGoods: null,
    IsDangerous: false,
    MoveTypeCode: null,
    ValueOfGoodsCurrencyCode: null,
    MainHarmonize: null,
    CarrierCode: null,
    CarrierId: null,
    CarrierAddedManually: false,
    IncotermId: null,
    IncotermAddedManually: false,
    MoveTypeId: null,
    MoveTypeAddedManually: false,
    ValueOfGoodsCurrencyId: null,
    ValueOfGoodsCurrencyAddedManually: false,
    Transshipment1CarrierAddedManually: false,
    Transshipment1VesselAddedManually: false,
    MainCarriageVesselAddedManually: false,
    InterlineAddedManually: false,
    ...overrides,
});

describe('SharedManifestAdditionalComponent (State/Indicators)', () => {
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

    describe('UI toggle handlers', () => {
        it('toggles other partner and pickup/delivery visibility flags', () => {
            const { component } = buildComponent();

            component.HideOtherPartnersArea = false;
            component.HidePickupDetailsArea = false;
            component.HideDeliveryDetailsArea = false;

            component.ButtonHideOtherPartnersAreaClicked();
            component.imgHidePickupDetailsAreaClicked();
            component.imgHideDeliveryDetailsAreaClicked();

            expect(component.HideOtherPartnersArea).toBe(true);
            expect(component.HidePickupDetailsArea).toBe(true);
            expect(component.HideDeliveryDetailsArea).toBe(true);

            component.ButtonHideOtherPartnersAreaClicked();
            component.imgHidePickupDetailsAreaClicked();
            component.imgHideDeliveryDetailsAreaClicked();

            expect(component.HideOtherPartnersArea).toBe(false);
            expect(component.HidePickupDetailsArea).toBe(false);
            expect(component.HideDeliveryDetailsArea).toBe(false);
        });

        it('toggles general, package, additional, and agent sections', () => {
            const { component } = buildComponent();

            component.HideGeneralDetailsArea = false;
            component.HidePackageTypeArea = false;
            component.HideAddtionalArea = false;
            component.HideAgentSideDataArea = false;

            component.imgHideGeneralDetailsAreaClicked();
            component.imgHidePackageTypeAreaClicked();
            component.imgHideAddtionalClicked();
            component.imgHideAgentSideDataAreaClicked();

            expect(component.HideGeneralDetailsArea).toBe(true);
            expect(component.HidePackageTypeArea).toBe(true);
            expect(component.HideAddtionalArea).toBe(true);
            expect(component.HideAgentSideDataArea).toBe(true);

            component.imgHideGeneralDetailsAreaClicked();
            component.imgHidePackageTypeAreaClicked();
            component.imgHideAddtionalClicked();
            component.imgHideAgentSideDataAreaClicked();

            expect(component.HideGeneralDetailsArea).toBe(false);
            expect(component.HidePackageTypeArea).toBe(false);
            expect(component.HideAddtionalArea).toBe(false);
            expect(component.HideAgentSideDataArea).toBe(false);
        });
    });

    describe('BuildOurSide', () => {
        it('populates carrier data and keeps routing leg visible when manifest provides details', () => {
            const { component } = buildComponent();
            SessionLocator.TenantPM = {
                AllowAgentInCustomersLOV: true,
                AgentId: 'TENANT-AG',
                AddressId: 'ADDR-1'
            } as any;
            component.ManifestSL = {
                TransportModeId: 'O',
                ShipmentLevelCode: 'M',
                MainCarriageVesselCode: 'MAIN',
                MainCarriageVesselId: 'MV-1',
                InterlineCode: 'IL',
                InterlineId: 'IL-1',
                Transshipment1CarrierCode: 'CARR1',
                Transshipment1CarrierId: 'CAR-1',
                Transshipment1VesselCode: 'VES1',
                Transshipment1VesselId: 'VES-1',
                Transshipment2CarrierCode: null,
                Transshipment2VesselCode: null,
                Transshipment3CarrierCode: null,
                Transshipment3VesselCode: null,
                Transshipment1FromPortCode: 'P1'
            } as any;
            component.AgentSideData = createAgentData({
                IncotermCode: 'CPT',
                ValueOfGoods: 1200,
                DescriptionOfGoods: 'goods',
                IsDangerous: true,
                MoveTypeCode: 'MOVE',
                ValueOfGoodsCurrencyCode: 'USD',
                MainHarmonize: 'HS',
                CarrierCode: 'CARR',
                CarrierId: 'MAIN-CARRIER',
                IncotermId: 'INC-ID',
                MoveTypeId: 'MOVE-ID',
                ValueOfGoodsCurrencyId: 'CUR-ID'
            }) as any;
            component.EntityPM.TransportModeId = 'O';
            component.EntityPM.ShipmentLevelCode = 'M';
            component.EntityPM.Transshipment1FromPortCode = 'PORT-1';
            component.EntityPM.Transshipment2FromPortCode = null;
            component.EntityPM.Transshipment3FromPortCode = null;

            component.BuildOurSide();

            expect(component.CardDependencyProperty1).toBe('CS,AG');
            expect(component.IsHideIncoterm).toBe(false);
            expect(component.MainCarriageCarrierId).toBe('MAIN-CARRIER');
            expect(component.Transshipment1CarrierId).toBe('CAR-1');
            expect(component.Transshipment1VesselId).toBe('VES-1');
            expect(component.IsHideRoutingLeg1).toBe(false);
            expect(component.IsHideRoutingLeg2).toBe(true);
            expect(component.IsHideRoutingLeg3).toBe(true);
            expect(component.IsNoTransShipmentsDetailsFound).toBe(false);
            expect(component.HideTransShipmentsDetailsArea).toBe(false);
        });

        it('hides routing information and marks no transshipments for house shipments', () => {
            const { component } = buildComponent();
            component.HouseEntity = {} as any;
            component.ManifestSL = {
                TransportModeId: 'O',
                ShipmentLevelCode: 'H'
            } as any;
            component.AgentSideData = createAgentData() as any;
            component.EntityPM.TransportModeId = 'O';
            component.EntityPM.ShipmentLevelCode = 'H';
            component.EntityPM.Transshipment1FromPortCode = null;
            component.EntityPM.Transshipment2FromPortCode = null;
            component.EntityPM.Transshipment3FromPortCode = null;

            component.BuildOurSide();

            expect(component.IsHouseShipment).toBe(true);
            expect(component.IsHideCarrier).toBe(true);
            expect(component.IsHideRoutingLeg1).toBe(true);
            expect(component.IsHideRoutingLeg2).toBe(true);
            expect(component.IsHideRoutingLeg3).toBe(true);
            expect(component.IsNoTransShipmentsDetailsFound).toBe(true);
            expect(component.HideTransShipmentsDetailsArea).toBe(true);
        });

        it('sets consol shipment defaults when shipment level is consol', () => {
            const { component } = buildComponent();
            SessionLocator.TenantPM = {
                AllowAgentInCustomersLOV: false,
                AgentId: 'TEN-AG',
                AddressId: 'TEN-ADDR'
            } as any;
            component.ManifestSL = {
                TransportModeId: 'I',
                ShipmentLevelCode: 'C'
            } as any;
            component.AgentSideData = createAgentData() as any;
            component.CurrentEntity.AgentId = 'AG-123';
            component.EntityPM.TransportModeId = 'I';
            component.EntityPM.ShipmentLevelCode = 'C';
            component.EntityPM.Transshipment1FromPortCode = null;
            component.EntityPM.Transshipment2FromPortCode = null;
            component.EntityPM.Transshipment3FromPortCode = null;

            component.BuildOurSide();

            expect(component.IsConsolShipment).toBe(true);
            expect(component.IsShowTransShipmentsDetails).toBe(true);
            expect(component.LableCreateButton).toBe('Create Master');
            expect(component.ConsigneeId).toBe('TEN-AG');
            expect(component.ConsigneeAddressId).toBe('TEN-ADDR');
            expect(component.CardDependencyProperty1).toBe('CS');
        });
    });

    describe('BuildOurPickUpDeliverySide', () => {
        it('flags missing pickup details and stops the busy indicator when no pickup data is provided', () => {
            const portListResponse = createServiceResponse([]);
            const portListMockFn = jest.fn().mockReturnValue(of(portListResponse));
            const countryListMockFn = jest.fn().mockReturnValue(of(createServiceResponse([])));
            const otherServices = {
                myPortListService: {
                    instance: { getAllFromCache: portListMockFn },
                    spies: { getAllFromCache: portListMockFn },
                },
                countryListService: {
                    instance: { getAllFromCache: countryListMockFn },
                    spies: { getAllFromCache: countryListMockFn },
                },
            };

            const { component } = buildComponent({
                manifestSL: { ShipmentPickUp: null, ShipmentDelivery: null } as any,
                currentEntity: { SharedManifestTranslations: [] } as any,
                otherServices,
            });

            component.AgentSideData = createAgentData() as any;
            component.OurSideShipmentPickUp = new ShipmentPickUpDeliverySL();
            component.OurSideShipmentDelivery = new ShipmentPickUpDeliverySL();

            component.BuildOurPickUpDeliverySide('PICK');

            expect(component.IsNoPickupDetailsFound).toBe(true);
            expect(component.HidePickupDetailsArea).toBe(true);
        });
    });
});

