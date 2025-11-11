import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { SharedManifestComponent } from '../Components/SharedManifestComponent';
import {
    createAgentSharedManifestList,
    createAgentSharedManifestPM,
    createManifestSL,
    createHouseSL
} from './fixtures/shipment-fixtures';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

const createServiceResponse = <T>(result: T): ServiceResponse => {
    const response = new ServiceResponse();
    response.Result = result;
    return response;
};

const createSessionMock = () => {
    const windowStub = {
        StartBusyIndicator: jest.fn(),
        StopBusyIndicator: jest.fn()
    };

    return {
        StartBusyIndicatorLoading: jest.fn(),
        StartBusyIndicator: jest.fn(),
        StopBusyIndicator: jest.fn(),
        CurrentWindow: windowStub,
        SessionLocation: { viewContainerRef: {} },
        CloseCurrentWindow: jest.fn()
    };
};

const createComponent = () => {
    const session = createSessionMock();
    SessionLocator.SelectedSession = session as any;
    SessionLocator.DynamicLoader = {
        Load: jest.fn(() => Promise.resolve({ instance: { Run: jest.fn() } as any }))
    } as any;
    SessionInfo.LoggedUserTenant = 77;

    const sharedService = {
        getAgentSharedManifesRefShipmentListsByIds: jest.fn().mockReturnValue(of(createServiceResponse([]))),
        GetCheckIfAnyShipmentHaveMasterNumber: jest.fn().mockReturnValue(of(createServiceResponse(null))),
        get: jest.fn()
    };

    const entityResourceService = {
        getEntityResourceByTableName: jest.fn()
    };

    const pmService = {
        update: jest.fn().mockReturnValue(of(createServiceResponse(null)))
    };

    const component = new SharedManifestComponent(
        sharedService as any,
        entityResourceService as any,
        pmService as any
    );

    return {
        component,
        services: {
            sharedService,
            entityResourceService,
            pmService
        },
        session
    };
};

describe('SharedManifestComponent', () => {
    beforeEach(() => {
        jest.spyOn(ServiceLocator, 'SendTotangoUserActivity').mockImplementation(() => undefined);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('RefreshSharedManifiestoStatus enables edit when manifest entity exists', () => {
        const { component } = createComponent();
        component.ManifestSL = createManifestSL({ EntityId: 'SHIP-1' });
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'WAIT' });

        component.RefreshSharedManifiestoStatus();

        expect(component.IsShowEditButon).toBe(true);
        expect(component.IsEnableCreateHouse).toBe(true);
        expect(component.ButtonChangeStatusLable).toBe('Cancel Manifest');
        expect(component.ShowAreaButton).toBe(true);
    });

    it('RefreshSharedManifiestoStatus shows reactivate when status cancelled', () => {
        const { component } = createComponent();
        component.ManifestSL = createManifestSL({ EntityId: null });
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'CANC' });

        component.RefreshSharedManifiestoStatus();

        expect(component.ButtonChangeStatusLable).toBe('reactivate');
        expect(component.IsEnableCreateMasterButton).toBe(false);
        expect(component.WidthButtonStatusChange).toBe('70px');
    });

    it('UpDateAgentSharedManifest updates status and triggers refresh', () => {
        const { component, services, session } = createComponent();
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'WAIT' });
        jest.spyOn(component, 'RefreshSharedManifiestoStatus').mockImplementation(() => undefined);

        component.UpDateAgentSharedManifest('COMP');

        expect(session.CurrentWindow.StartBusyIndicator).toHaveBeenCalledWith('Saving....');
        expect(services.pmService.update).toHaveBeenCalled();
        expect(component.RefreshSharedManifiestoStatus).toHaveBeenCalled();
        expect(component.CurrentEntity.StatusCode).toBe('COMP');
    });

    it('MarkASCompleted sets status to COMP and updates manifest', () => {
        const { component, services } = createComponent();
        const spy = jest.spyOn(component, 'UpDateAgentSharedManifest').mockImplementation(() => undefined);
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'WAIT' });

        component.MarkASCompleted();

        expect(spy).toHaveBeenCalledWith('COMP');
        expect(component.CurrentEntity.StatusCode).toBe('COMP');
        expect(services.pmService.update).not.toHaveBeenCalled();
    });

    it('ChangeStatusAgentSharedManifest toggles status and logs activity', () => {
        const { component } = createComponent();
        const updateSpy = jest.spyOn(component, 'UpDateAgentSharedManifest').mockImplementation(() => undefined);
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'WAIT' });

        component.ChangeStatusAgentSharedManifest();

        expect(updateSpy).toHaveBeenCalledWith('CANC');
        expect(ServiceLocator.SendTotangoUserActivity).toHaveBeenCalledWith(
            'Agents Shared Logistics',
            'Decline Shared Manifests'
        );
    });

    it('ChangeStatusAgentSharedManifest reactivates cancelled manifests without logging decline', () => {
        const { component } = createComponent();
        const updateSpy = jest.spyOn(component, 'UpDateAgentSharedManifest').mockImplementation(() => undefined);
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'CANC' });

        component.ChangeStatusAgentSharedManifest();

        expect(updateSpy).toHaveBeenCalledWith('WAIT');
        expect(ServiceLocator.SendTotangoUserActivity).not.toHaveBeenCalled();
    });

    it('SetWindowArgs loads data, maps houses, and updates labels', () => {
        const { component, services, session } = createComponent();
        const manifest = createManifestSL({
            SharedManifestRef: 'REF-001',
            Houses: [createHouseSL()],
            EntityId: null,
            MasterNumber: 'MAWB-1',
            LongMaster: 'LM-1'
        });
        const currentEntity = createAgentSharedManifestPM({
            ManifestSL: manifest,
            StatusCode: 'WAIT'
        });

        services.entityResourceService.getEntityResourceByTableName.mockReturnValue(of({}));
        services.sharedService.get.mockReturnValue(of({ HasError: false, Result: currentEntity }));
        services.sharedService.getAgentSharedManifesRefShipmentListsByIds.mockReturnValue(
            of(
                createServiceResponse([
                    { AgentSharedManifestRef: currentEntity.Id, ShipmentId: 'MASTER-1' },
                    { AgentSharedManifestRef: `${currentEntity.Id}/1`, ShipmentId: 'HOUSE-1' }
                ])
            )
        );

        component.SetWindowArgs({ CurrentEntity: createAgentSharedManifestList({ Id: 'LIST' }) });

        expect(session.StartBusyIndicator).toHaveBeenCalledWith('Loading...');
        expect(services.sharedService.get).toHaveBeenCalled();
        expect(services.sharedService.GetCheckIfAnyShipmentHaveMasterNumber).toHaveBeenCalledWith(
            'MAWB-1',
            'LM-1',
            SessionInfo.LoggedUserTenant
        );
        expect(component.ManifestSL.EntityId).toBe('MASTER-1');
        expect(component.HousesList[0].EntityId).toBe('HOUSE-1');
        expect(component.LableHouseArea).toBe('1 of 1 houses created');
        expect(component.IsShowMarkASCompleted).toBe(false);
        expect(session.StopBusyIndicator).toHaveBeenCalled();
    });

    it('SetWindowArgs respects cancelled manifests', () => {
        const { component, services } = createComponent();
        const manifest = createManifestSL({
            SharedManifestRef: 'REF-002',
            ShipmentLevelCode: 'D',
            TransportModeId: 'A',
            LongMaster: 'LM',
            MasterNumber: 'MN'
        } as any);
        const currentEntity = createAgentSharedManifestPM({
            ManifestSL: manifest,
            CancelledBySenderAgent: true
        });

        services.entityResourceService.getEntityResourceByTableName.mockReturnValue(of({}));
        services.sharedService.get.mockReturnValue(of({ HasError: false, Result: currentEntity }));

        component.SetWindowArgs({ CurrentEntity: createAgentSharedManifestList({ Id: 'LIST-2' }) });

        expect(component.ValidationWarningsList).toContain(
            'The manifest was cancelled by the sender.You are not allowed to reactivate it.'
        );
        expect(component.IsDisableEdit).toBe(true);
        expect(component.MessageNoHouseFound).toBe('This is a direct shipment');
    });

    it('CheckifShipmentCreateOrNotAndEnableEdit handles missing houses and marks completion', () => {
        const { component, services } = createComponent();
        const manifest = createManifestSL({
            Houses: [],
            SharedManifestRef: 'REF-EMPTY',
            EntityId: null
        });
        component.CurrentEntity = createAgentSharedManifestPM({ Id: 'REF-EMPTY', ManifestSL: manifest });
        component.ManifestSL = manifest;
        services.sharedService.getAgentSharedManifesRefShipmentListsByIds.mockReturnValue(
            of(createServiceResponse([{ AgentSharedManifestRef: 'REF-EMPTY', ShipmentId: 'MASTER-ONLY' }]))
        );

        component.CheckifShipmentCreateOrNotAndEnableEdit();

        expect(component.IsNoHouses).toBe(true);
        expect(component.ManifestSL.EntityId).toBe('MASTER-ONLY');
        expect(component.IsShowMarkASCompleted).toBe(false);
    });

    it('UpDateAgentSharedManifest stops the busy indicator even when update fails', () => {
        const { component, services, session } = createComponent();
        services.pmService.update.mockReturnValue(of({ HasError: true }));
        const refreshSpy = jest.spyOn(component, 'RefreshSharedManifiestoStatus').mockImplementation(() => undefined);
        component.CurrentEntity = createAgentSharedManifestPM({ StatusCode: 'WAIT' });

        component.UpDateAgentSharedManifest('WAIT');

        expect(session.CurrentWindow.StopBusyIndicator).toHaveBeenCalled();
        expect(component.isEntityChange).toBe(true);
        expect(refreshSpy).toHaveBeenCalled();
    });
});

