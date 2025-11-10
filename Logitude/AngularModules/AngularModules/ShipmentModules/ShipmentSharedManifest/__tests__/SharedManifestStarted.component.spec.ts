import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { SharedManifestStarted } from '../Components/SharedManifestStarted';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

describe('SharedManifestStarted', () => {
    const createServiceResponse = <T>(result: T, hasError = false, errors: string[] = []) => {
        const response = new ServiceResponse();
        response.HasError = hasError;
        response.Result = result;
        response.ErrorsArray = errors;
        return response;
    };

    const createSession = () => ({
        StartBusyIndicator: jest.fn(),
        StartBusyIndicatorLoading: jest.fn(),
        StopBusyIndicator: jest.fn(),
        CloseCurrentWindow: jest.fn(),
        CurrentEditComponent: {
            EntityPM: null,
            SaveChanges: jest.fn(),
            SaveCompleted: { subscribe: jest.fn() },
        },
        CurrentWindow: { Title: '' },
    });

    const createShipment = (overrides: Partial<any> = {}) => ({
        Id: 'SHIP-1',
        AgentId: 'AG-1',
        ConsigneeId: 'CN-1',
        TransportModeId: 'O',
        ShipmentLevelCode: 'H',
        IsManifestSentToAgent: false,
        Tenant: 5,
        ...overrides,
    });

    let sessionMock: ReturnType<typeof createSession>;
    let manifestService: any;
    let keyService: any;

    beforeEach(() => {
        sessionMock = createSession();
        SessionLocator.SelectedSession = sessionMock as any;
        SessionLocator.TenantPM = { AllowAgentInCustomersLOV: false } as any;
        SessionInfo.LoggedUserTenant = 5;
        jest.spyOn(ServiceLocator, 'SendTotangoUserActivity').mockImplementation(() => undefined);
        manifestService = {
            ShareAgentManifest: jest.fn(),
            GetCheckIfMasterShipmentHaveHouseWithOtherAgent: jest.fn(),
        };
        keyService = {
            GetSingleByAgentId: jest.fn(),
        };
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('SetWindowArgs records validation errors when mandatory fields missing', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        const shipment = createShipment({ AgentId: null, ConsigneeId: null, TransportModeId: 'A', Master: null });

        component.SetWindowArgs({ EntityPM: shipment, FromSharedManifestEditAgentComponent: false });

        expect(component.ValidationErrorsList).toEqual([
            'Please define shipment agent in the Partners tab',
            'The consignee partner is missing',
            'The master number is missing',
        ]);
        expect(component.HasError).toBe(true);
        expect(component.IsEnableButtonSharedManifest).toBe(false);
    });

    it('SetWindowArgs enables sharing when agent status is active', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        const shipment = createShipment();
        component.UpdateAgent = jest.fn(); // prevent actual window logic

        keyService.GetSingleByAgentId.mockReturnValue(of(createServiceResponse({ StatusCode: 'A' })));

        component.SetWindowArgs({ EntityPM: shipment, FromSharedManifestEditAgentComponent: false });

        expect(sessionMock.StartBusyIndicator).toHaveBeenCalledWith('Loading...');
        expect(sessionMock.StopBusyIndicator).toHaveBeenCalled();
        expect(component.IsEnableButtonSharedManifest).toBe(true);
        expect(component.HasError).toBe(false);
    });

    it('SetWindowArgs handles agent wait status as error', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        const shipment = createShipment();

        keyService.GetSingleByAgentId.mockReturnValue(of(createServiceResponse({ StatusCode: 'W' })));

        component.SetWindowArgs({ EntityPM: shipment, FromSharedManifestEditAgentComponent: false });

        expect(component.HasError).toBe(true);
        expect(component.ValidationErrorsList).toContain(
            "Waiting for the Agent’s approval to enable sharing"
        );
        expect(component.IsEnableButtonSharedManifest).toBe(false);
    });

    it('SetWindowArgs handles inactive agent status', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        const shipment = createShipment();

        keyService.GetSingleByAgentId.mockReturnValue(of(createServiceResponse({ StatusCode: 'I' })));

        component.SetWindowArgs({ EntityPM: shipment, FromSharedManifestEditAgentComponent: false });

        expect(component.HasError).toBe(true);
        expect(component.ValidationErrorsList).toContain(
            'Please connect with the agent from the agent’s shared logistics tab'
        );
    });

    it('StartSharingManifest success path updates state and logs activity', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        component.EntityPM = createShipment() as any;
        manifestService.ShareAgentManifest.mockReturnValue(of(createServiceResponse({}, false)));

        component.StartSharingManifest();

        expect(sessionMock.StartBusyIndicator).toHaveBeenCalledWith('Sharing Manifest...');
        expect(sessionMock.StopBusyIndicator).toHaveBeenCalled();
        expect(component.IsEnableButtonSharedManifest).toBe(false);
        expect(component.IsSuccessfullySharedManifest).toBe(true);
        expect(ServiceLocator.SendTotangoUserActivity).toHaveBeenCalledWith(
            'Agents Shared Logistics',
            'Share Manifests'
        );
    });

    it('StartSharingManifest error path collects validation errors', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        component.EntityPM = createShipment() as any;
        manifestService.ShareAgentManifest.mockReturnValue(
            of(createServiceResponse(null, true, ['Failed share']))
        );

        component.StartSharingManifest(true);

        expect(component.HasError).toBe(true);
        expect(component.ValidationErrorsList).toContain('Failed share');
        expect(ServiceLocator.SendTotangoUserActivity).not.toHaveBeenCalled();
    });

    it('SharingManifesButtonClick triggers save when entity is dirty', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        const shipment = createShipment({ IsDirty: true });
        sessionMock.CurrentEditComponent.SaveChanges.mockImplementation(() => {});
        component.EntityPM = shipment as any;

        component.SharingManifesButtonClick();

        expect(sessionMock.CurrentEditComponent.SaveChanges).toHaveBeenCalled();
        expect(component.isSharingManifesRequested).toBe(true);
    });

    it('SharingManifesButtonClick starts sharing immediately when not dirty', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        const shipment = createShipment({ IsDirty: false });
        component.EntityPM = shipment as any;
        const startSpy = jest.spyOn(component, 'StartSharingManifest').mockImplementation(() => {});

        component.SharingManifesButtonClick();

        expect(startSpy).toHaveBeenCalled();
        expect(startSpy.mock.calls[0][0]).toBeUndefined();
    });

    it('UpdateAgent shows update area when invoked from edit component', () => {
        const component = new SharedManifestStarted(manifestService, keyService);
        component.FromSharedManifestEditAgentComponent = true;

        component.UpdateAgent();

        expect(component.IsShowUpdateAgentArea).toBe(true);
        expect(sessionMock.CurrentWindow.Title).toBe('Share Manifest with Updated Agent');
    });
});

