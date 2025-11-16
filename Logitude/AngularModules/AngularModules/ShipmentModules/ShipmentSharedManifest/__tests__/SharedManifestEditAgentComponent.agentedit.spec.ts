import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { SharedManifestEditAgentComponent } from '../Components/SharedManifestEditAgentComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

const createServiceResponse = <T>(result: T, hasError = false): ServiceResponse => {
    const response = new ServiceResponse();
    response.Result = result;
    response.HasError = hasError;
    response.ErrorsArray = hasError ? ['error'] : [];
    return response;
};

const createSessionStub = () => {
    const saveCompleted = { subscribe: jest.fn() };

    return {
        StartBusyIndicatorLoading: jest.fn(),
        StartBusyIndicator: jest.fn(),
        StopBusyIndicator: jest.fn(),
        CurrentWindow: {
            StartBusyIndicator: jest.fn(),
            StopBusyIndicator: jest.fn()
        },
        CurrentEditComponent: {
            SaveCompleted: saveCompleted,
            EntityPM: null,
            SaveChanges: jest.fn()
        },
        CloseCurrentWindow: jest.fn(),
        CloseCurrentWindowEmit: jest.fn(),
        FireEvent: jest.fn()
    };
};

const createShipmentPM = () =>
    ({
        Id: 'SHIP-1',
        AgentId: 'AG1',
        AgentName: 'Agent',
        AgentAddressId: 'ADDR1',
        AgentContactId: 'CONT1',
        AgentNote: 'Note',
        Tenant: 12,
        ShipmentLevelCode: 'H',
        CustomerId: 'C1',
        CustomerName: 'Cust',
        CustomerReference1: 'CR1',
        CustomerReference2: 'CR2',
        CustomerAddressId: 'CADDR',
        CustomerContactId: 'CCON',
        ShipmentCustomerTypeCode: 'TYPE',
        IsDirty: true
    } as any);

const createPartnerItem = (overrides: Partial<any> = {}) => {
    return {
        PartnerId: 'AG1',
        Reference1: 'ref1',
        Reference2: 'ref2',
        Name: 'Agent Name',
        AddressId: 'ADDR1',
        ContactId: 'CONT1',
        IsCustomer: false,
        SetUIProperties: jest.fn(),
        fatherComponent: {
            _agentSharedLogisticsKeyPMService: {
                GetSingleByAgentId: jest.fn().mockReturnValue(of(createServiceResponse({ StatusCode: 'A' })))
            },
            _sharedAgentManifestService: {
                GetCheckIfMasterShipmentHaveHouseWithOtherAgent: jest
                    .fn()
                    .mockReturnValue(of(createServiceResponse(false)))
            }
        },
        EntityPM: createShipmentPM(),
        IsDisableNextButton: false,
        IsReseting: false,
        ...overrides
    };
};

describe('SharedManifestEditAgentComponent', () => {
beforeEach(() => {
        jest.spyOn(TextCodeTranslator, 'Translate').mockImplementation(key => {
            if (key === 'General.M.FieldIsRequired') {
                return '%FieldName is required';
            }
            return key;
        });
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    const setup = (overrides: Partial<any> = {}) => {
        const sharedService = {
            GetIsAgentSharedManifests: jest.fn().mockReturnValue(of(createServiceResponse(false)))
        };
        const session = createSessionStub();
        SessionLocator.SelectedSession = session as any;

        const component = new SharedManifestEditAgentComponent(sharedService as any);
        const partnerItem = createPartnerItem(overrides);
        partnerItem.EntityPM = session.CurrentEditComponent.EntityPM = partnerItem.EntityPM || createShipmentPM();

        component.SetDataContext(partnerItem as any);

        return {
            component,
            sharedService,
            partnerItem,
            session
        };
    };

    it('validates required partner before saving', () => {
        const { component, partnerItem } = setup({ PartnerId: null });

        component.ValidationErrorsList = [];
        component.OkButtonClicked();

        expect(component.ValidationErrorsList.some(msg => msg.includes('Shipment.S.Partners.Name'))).toBe(true);
        expect(partnerItem.fatherComponent._agentSharedLogisticsKeyPMService.GetSingleByAgentId).not.toHaveBeenCalled();
    });

    it('adds validation when references exceed max length', () => {
        const longRef = 'x'.repeat(60);
        const { component } = setup({ Reference1: longRef, Reference2: longRef, PartnerId: 'AG1' });

        component.OkButtonClicked();

        expect(component.ValidationErrorsList).toContain(
            'Shipment.S.Partners.Reference1 max length is 50'
        );
        expect(component.ValidationErrorsList).toContain(
            'Shipment.S.Partners.Reference2 max length is 50'
        );
    });

    it('sets validation when agent key status waiting', () => {
        const keyResponse = of(createServiceResponse({ StatusCode: 'W' }));
        const { component, partnerItem } = setup({
            PartnerId: 'AG1',
            fatherComponent: {
                _agentSharedLogisticsKeyPMService: {
                    GetSingleByAgentId: jest.fn().mockReturnValue(keyResponse)
                },
                _sharedAgentManifestService: {
                    GetCheckIfMasterShipmentHaveHouseWithOtherAgent: jest
                        .fn()
                        .mockReturnValue(of(createServiceResponse(false)))
                }
            }
        });

        component.OkButtonClicked();

        expect(component.ValidationErrorsList).toContain(
            'Waiting for the Agent’s approval to enable sharing'
        );
        expect(partnerItem.fatherComponent._agentSharedLogisticsKeyPMService.GetSingleByAgentId).toHaveBeenCalled();
    });

    it('CompleteSave clears customer fields when my customer and consol', () => {
        const shipment = createShipmentPM();
        shipment.ShipmentLevelCode = 'C';
        const { component } = setup({
            PartnerId: 'AG1',
            IsCustomer: true,
            EntityPM: shipment
        });

        component.ValidationErrorsList = [];
        component.CompleteSave();

        expect(component.EntityPM.CustomerId).toBeNull();
        expect(component.EntityPM.CustomerName).toBeNull();
        expect(component.IsSaveShipment).toBe(true);
        expect(component['CurrentSession'].CurrentEditComponent.SaveChanges).toHaveBeenCalled();
    });

    it('CompleteSave copies partner data when not customer', () => {
        const { component } = setup({ IsCustomer: false });

        (component.DataContext as any).PartnerId = 'AG123';
        (component.DataContext as any).Name = 'Updated Agent';
        (component.DataContext as any).AddressId = 'ADDR2';
        (component.DataContext as any).ContactId = 'CONT2';
        (component.DataContext as any).Reference1 = 'REF-A';
        (component.DataContext as any).Reference2 = 'REF-B';

        component.EntityPM.IsDirty = false;
        component.ValidationErrorsList = [];
        expect(component.DataContext.PartnerId).toBe('AG123');
        expect(component.EntityPM.CustomerId).toBe('C1');
        component['isMyCustomer'] = false;
        component.CompleteSave();

        expect(component['CurrentSession'].CloseCurrentWindowEmit).toHaveBeenCalledWith('OK');
        expect(component['CurrentSession'].CurrentEditComponent.SaveChanges).not.toHaveBeenCalled();
        expect(component.EntityPM.CustomerId).toBe('C1');
    });
});

