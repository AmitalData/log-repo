import { expect } from '@jest/globals';
import { buildComponent } from './helpers/component-builder';

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
});

