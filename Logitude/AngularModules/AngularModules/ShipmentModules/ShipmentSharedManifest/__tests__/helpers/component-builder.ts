import { SharedManifestAdditionalComponent } from '../../Components/SharedManifestAdditionalComponent';
import { SharedAgentManifestService } from '../../../../Shipment/Services/Others/SharedAgentManifestService';
import { AgentSharedManifestPMService } from '../../../../Common/Services/StandardPMs/AgentSharedManifestPMService';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { AgentSharedManifestPM } from '../../../../Common/EntityPMs/AgentSharedManifestPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { of } from 'rxjs';
import { configureSessionLocator, SessionLocatorMocks } from './session-mocks';
import {
    createAddressListServiceMock,
    createCardListServiceMock,
    createListGetSingleMock,
    createNoopServiceMock,
    createServiceResponse,
    MockWithSpies
} from './service-stubs';

export interface LocationDirectiveStub {
    Code: string;
    viewContainerRef: any;
}

export interface ComponentBuilderOverrides {
    sharedAgentManifestService?: Partial<SharedAgentManifestService>;
    agentSharedManifestPMService?: Partial<AgentSharedManifestPMService>;
    entityPMService?: Partial<EntityPMService>;
    cardListServiceMock?: MockWithSpies<any>;
    addressListServiceMock?: MockWithSpies<any>;
    otherServices?: Record<string, MockWithSpies<any>>;
    currentEntity?: AgentSharedManifestPM;
    shipmentPM?: ShipmentPM;
    agentSharedManifestList?: any;
    manifestSL?: any;
    houseEntity?: any;
    sessionMocks?: Partial<SessionLocatorMocks>;
    locations?: Array<Pick<LocationDirectiveStub, 'Code' | 'viewContainerRef'>>;
}

export interface ComponentBuilderResult {
    component: SharedManifestAdditionalComponent;
    sessionMocks: SessionLocatorMocks;
    serviceMocks: {
        cardListService: MockWithSpies<any>;
        addressListService: MockWithSpies<any>;
        [key: string]: MockWithSpies<any>;
    };
}

export interface LocationDirectiveStub {
    Code: string;
    viewContainerRef: any;
}

const createLocationCollection = (items: Array<Pick<LocationDirectiveStub, 'Code' | 'viewContainerRef'>>) => ({
    toArray: () => items
});

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
    myShipmentPMService: createNoopServiceMock,
    _sharedAgentManifestService: createNoopServiceMock,
    _agentSharedManifestPMService: createNoopServiceMock,
    entityPMService: createNoopServiceMock
};

export const buildComponent = (overrides?: ComponentBuilderOverrides): ComponentBuilderResult => {
    const sessionMocks = configureSessionLocator(overrides?.sessionMocks);

    const sharedAgentManifestService = overrides?.sharedAgentManifestService || {};
    const agentSharedManifestPMService = overrides?.agentSharedManifestPMService || {};
    const entityPMService = overrides?.entityPMService || {};

    const component = new SharedManifestAdditionalComponent(
        sharedAgentManifestService as SharedAgentManifestService,
        agentSharedManifestPMService as AgentSharedManifestPMService,
        entityPMService as EntityPMService
    );

    const cardListService = overrides?.cardListServiceMock || createCardListServiceMock();
    const addressListService = overrides?.addressListServiceMock || createAddressListServiceMock();

    component['myCardListService'] = cardListService.instance;
    component['myAddressListService'] = addressListService.instance;

    const additionalServiceMocks: Record<string, MockWithSpies<any>> = {};

    Object.entries(defaultServiceFactories).forEach(([propertyName, factory]) => {
        if (!component[propertyName]) {
            const mock = factory();
            component[propertyName] = mock.instance;
            additionalServiceMocks[propertyName] = mock;
        }
    });

    if (overrides?.otherServices) {
        Object.entries(overrides.otherServices).forEach(([propertyName, mock]) => {
            component[propertyName] = mock.instance;
            additionalServiceMocks[propertyName] = mock;
        });
    }

    component['CurrentSession'] = sessionMocks.session as any;
    component.EntityPM = overrides?.shipmentPM || new ShipmentPM();
    component.CurrentEntity = overrides?.currentEntity || new AgentSharedManifestPM();
    component.ManifestSL = overrides?.manifestSL || component.ManifestSL || {};
    component.HouseEntity = overrides?.houseEntity || component.HouseEntity || null;
    component.AgentSharedManifestList = overrides?.agentSharedManifestList || [];

    component.AllLocations = createLocationCollection(overrides?.locations || []) as any;

    return {
        component,
        sessionMocks,
        serviceMocks: {
            cardListService,
            addressListService,
            ...additionalServiceMocks
        }
    };
};

export const createSuccessfulResponse = (result?: any): ServiceResponse => createServiceResponse(result, false);

export const observableOf = <T>(value: T) => of(value);

