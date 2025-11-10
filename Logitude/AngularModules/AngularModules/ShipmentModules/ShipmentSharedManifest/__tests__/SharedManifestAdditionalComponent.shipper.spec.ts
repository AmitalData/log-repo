import { expect } from '@jest/globals';
import { buildComponent } from './helpers/component-builder';
import { createAddressListServiceMock, createCardListServiceMock } from './helpers/service-stubs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SharedManifestTranslationPM } from '../../../Common/EntityPMs/SharedManifestTranslationPM';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';
import { HouseSL } from '../../../Common/DataContracts/HouseSL';

describe('SharedManifestAdditionalComponent (Shipper)', () => {
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

        it('prefers house entity references when available', () => {
            const cardList = {
                Code: 'CARD-03',
                PartnerTypeId: 'PARTNER',
                PrimaryContactId: 'CONTACT',
                EnglishName: 'House Shipper',
                Notes: 'House',
                MainAddressId: 'ADDR-6',
                PickAddressId: 'ADDR-7',
                SalesmanUserId: 'SALESMAN-H'
            } as any;

            const houseEntity = {
                Shipper: { Code: 'CARD-03' },
                ShipperReference1: 'HOUSE-REF-1',
                ShipperReference2: 'HOUSE-REF-2'
            } as unknown as HouseSL;

            const { component } = buildComponent({
                cardListServiceMock: createCardListServiceMock(cardList),
                addressListServiceMock: createAddressListServiceMock(null),
                houseEntity,
                manifestSL: {} as ManifestSL
            });

            component.IsConsolShipment = false;
            component.ShipperId = 'CARD-03';

            expect(component.EntityPM.ShipperReference1).toBe('HOUSE-REF-1');
            expect(component.EntityPM.ShipperReference2).toBe('HOUSE-REF-2');
        });

        it('falls back to manifest references when house does not match', () => {
            const cardList = {
                Code: 'CARD-04',
                PartnerTypeId: 'PARTNER',
                PrimaryContactId: 'CONTACT',
                EnglishName: 'Manifest Shipper',
                Notes: 'Manifest',
                MainAddressId: 'ADDR-8',
                PickAddressId: 'ADDR-9',
                SalesmanUserId: 'SALESMAN-M'
            } as any;

            const manifest = {
                Shipper: { Code: 'CARD-04' },
                ShipperReference1: 'MANIFEST-REF-1',
                ShipperReference2: 'MANIFEST-REF-2'
            } as ManifestSL;

            const { component } = buildComponent({
                cardListServiceMock: createCardListServiceMock(cardList),
                addressListServiceMock: createAddressListServiceMock(null),
                manifestSL: manifest
            });

            component.IsConsolShipment = false;
            component.HouseEntity = null;
            component.ShipperId = 'CARD-04';

            expect(component.EntityPM.ShipperReference1).toBe('MANIFEST-REF-1');
            expect(component.EntityPM.ShipperReference2).toBe('MANIFEST-REF-2');
        });
    });
});

