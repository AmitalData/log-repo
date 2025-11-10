import { expect } from '@jest/globals';
import { buildComponent } from './helpers/component-builder';
import { createAddressListServiceMock, createCardListServiceMock } from './helpers/service-stubs';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';
import { HouseSL } from '../../../Common/DataContracts/HouseSL';

describe('SharedManifestAdditionalComponent (Consignee)', () => {
    describe('ConsigneeId setter', () => {
        it('populates consignee details and keeps house references', () => {
            const consigneeCard = {
                Code: 'CON-1',
                PartnerTypeId: 'PARTNER',
                PrimaryContactId: 'CON-CONTACT',
                EnglishName: 'Consignee Name',
                Notes: 'Consignee Notes',
                MainAddressId: 'ADDR-CON',
                PickAddressId: 'ADDR-PICK',
                SalesmanUserId: 'SALESMAN-CON'
            } as any;

            const houseEntity = {
                Consignee: { Code: 'CON-1' },
                ConsigneeReference1: 'HOUSE-C1',
                ConsigneeReference2: 'HOUSE-C2'
            } as unknown as HouseSL;

            const { component, serviceMocks } = buildComponent({
                cardListServiceMock: createCardListServiceMock(consigneeCard),
                addressListServiceMock: createAddressListServiceMock({ Id: 'ADDR-CON' }),
                houseEntity,
                manifestSL: {} as ManifestSL
            });

            component.IsConsolShipment = false;
            component.ConsigneeId = 'CON-1';

            expect(serviceMocks.cardListService.spies.getSingle).toHaveBeenCalledWith('CON-1');
            expect(serviceMocks.addressListService.spies.getSingle).toHaveBeenCalledWith('ADDR-CON');
            expect(component.EntityPM.ConsigneeName).toBe('Consignee Name');
            expect(component.EntityPM.ConsigneeReference1).toBe('HOUSE-C1');
            expect(component.EntityPM.ConsigneeReference2).toBe('HOUSE-C2');
            expect(component.EntityPM.ConsigneeMainAddressId).toBe('ADDR-CON');
        });

        it('falls back to manifest references when house mismatch', () => {
            const consigneeCard = {
                Code: 'CON-2',
                PartnerTypeId: 'PARTNER',
                PrimaryContactId: 'CON-CONTACT',
                EnglishName: 'Consignee Name',
                Notes: 'Consignee Notes',
                MainAddressId: 'ADDR-CON2',
                PickAddressId: 'ADDR-PICK2',
                SalesmanUserId: 'SALESMAN-CON2'
            } as any;

            const manifest = {
                Consignee: { Code: 'CON-2' },
                ConsigneeReference1: 'MANIFEST-C1',
                ConsigneeReference2: 'MANIFEST-C2'
            } as ManifestSL;

            const { component } = buildComponent({
                cardListServiceMock: createCardListServiceMock(consigneeCard),
                addressListServiceMock: createAddressListServiceMock({ Id: 'ADDR-CON2' }),
                manifestSL: manifest
            });

            component.IsConsolShipment = false;
            component.HouseEntity = null;

            component.ConsigneeId = 'CON-2';

            expect(component.EntityPM.ConsigneeReference1).toBe('MANIFEST-C1');
            expect(component.EntityPM.ConsigneeReference2).toBe('MANIFEST-C2');
        });
    });
});

