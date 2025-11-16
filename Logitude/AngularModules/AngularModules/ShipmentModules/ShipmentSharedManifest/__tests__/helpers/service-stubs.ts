import { of } from 'rxjs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { AddressList } from '../../../../Common/EntityLists/AddressList';

export interface MockWithSpies<T> {
    instance: T;
    spies: Record<string, jest.Mock>;
}

export const createServiceResponse = <T>(result: T, hasError = false): ServiceResponse => {
    const response = new ServiceResponse();
    response.Result = result;
    response.HasError = hasError;
    response.ErrorsArray = hasError ? ['error'] : [];
    return response;
};

const createMock = <T extends Record<string, unknown>>(factory: () => Record<string, jest.Mock>): MockWithSpies<T> => {
    const spies = factory();
    return {
        instance: spies as unknown as T,
        spies
    };
};

export const createCardListServiceMock = (cardList?: Partial<CardList>) =>
    createMock<any>(() => ({
        getSingle: jest.fn().mockReturnValue(of(createServiceResponse(cardList || null)))
    }));

export const createAddressListServiceMock = (addressList?: Partial<AddressList>) =>
    createMock<any>(() => ({
        getSingle: jest.fn().mockReturnValue(of(createServiceResponse(addressList || null)))
    }));

export const createNoopServiceMock = () =>
    createMock<any>(() => ({} as Record<string, jest.Mock>));

export const createListGetSingleMock = () =>
    createMock<any>(() => ({
        getSingle: jest.fn().mockReturnValue(of(createServiceResponse(null)))
    }));

export const createShipmentPMServiceMock = (response?: ServiceResponse) =>
    createMock<any>(() => ({
        insert: jest.fn().mockReturnValue(
            of(response || createServiceResponse({ Id: 'SHIP-ID' }))
        )
    }));

export const createAgentSharedManifestPMServiceMock = (response?: ServiceResponse) =>
    createMock<any>(() => ({
        update: jest.fn().mockReturnValue(
            of(response || createServiceResponse({}))
        )
    }));

const defaultServiceFactories: Record<string, () => MockWithSpies<any>> = {
    myIncotermListService: createListGetSingleMock,
    myAirlineListService: createListGetSingleMock,
    myPartnersDomainService: createListGetSingleMock,
    myPortListService: createListGetSingleMock,
    countryListService: createListGetSingleMock,
    myVesselListService: createListGetSingleMock,
    myMoveTypeListService: createListGetSingleMock,
    myCurrencyListService: createListGetSingleMock,
    myPackageTypeService: createListGetSingleMock,
    myShipmentPMService: () => createShipmentPMServiceMock(),
    _sharedAgentManifestService: createNoopServiceMock,
    _agentSharedManifestPMService: () => createAgentSharedManifestPMServiceMock(),
    entityPMService: createNoopServiceMock
};

