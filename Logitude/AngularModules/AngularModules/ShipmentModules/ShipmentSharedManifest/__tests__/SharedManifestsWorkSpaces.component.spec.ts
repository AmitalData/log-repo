import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { SharedManifestsWorkSpaces } from '../Components/SharedManifestsWorkSpaces';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { LastFilter } from '../../../Infrastructure/Utilities/LastFilter';

jest.mock('../../../Controls/Windows/LogitudeWindow', () => ({
    LogitudeWindow: jest.fn().mockImplementation(() => ({
        Title: '',
        Width: 0,
        Height: 0,
        WindowArgs: null,
        Show: jest.fn(),
        IsShowCloseButton: false,
        WindowClosed: { subscribe: jest.fn() },
    })),
}));

const { LogitudeWindow } = require('../../../Controls/Windows/LogitudeWindow');
const LogitudeWindowMock = LogitudeWindow as jest.Mock;

describe('SharedManifestsWorkSpaces', () => {
    const createServiceStub = (overrides: Partial<Record<string, any>> = {}) => ({
        getAgentSharedManifestsWorkspaceSummary: jest.fn().mockReturnValue(of({})),
        GetAgentSharedManifestsForDashBoard: jest.fn().mockReturnValue(of({ Result: [] })),
        ...overrides,
    });

    beforeEach(() => {
        jest.clearAllMocks();
        SessionLocator.SelectedSession = {
            GetChartId: jest.fn().mockReturnValue('CHART-ID'),
            SessionMenuLocation: { viewContainerRef: {} },
            SessionLocation: { viewContainerRef: {} },
            AddMenuReference: jest.fn(),
            SessionMenuReferences: [],
            StartBusyIndicatorLoading: jest.fn(),
            StopBusyIndicator: jest.fn(),
        } as any;
        SessionLocator.DynamicLoader = {
            Load: jest.fn().mockImplementation(async () => ({
                instance: {
                    ComponentRef: {} as any,
                    Run: jest.fn() as any,
                    BackCompleted: { subscribe: (callback: any) => callback(true) } as any,
                } as any,
            })) as any,
        } as any;
        SessionInfo.LoggedUserTenant = 77;
        (global as any).makeAmBarChart = jest.fn();
        (global as any).BarClick = jest.fn();
        (global as any).ResetItem = jest.fn();
        LogitudeWindowMock.mockClear();
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('sets share button visibility when feature permitted', () => {
        const featureSpy = jest
            .spyOn(FeatureLocator, 'HasFeaturePermession')
            .mockImplementation((feature, code) => feature === 'AgentSharedDocument' && code === 'NEW');

        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);

        expect(featureSpy).toHaveBeenCalledWith('AgentSharedDocument', 'NEW');
        expect(component.IsShareDocumentsButtonVisible).toBe(true);
        expect(SessionLocator.SelectedSession.GetChartId).toHaveBeenCalled();
    });

    it('SetPropertyVisibility honors feature codes', () => {
        const allowed = new Set([
            'AgentSharedManifestQ',
            'AirAgentSharedManifestsQ',
            'OceanAgentSharedManifestsQ',
            'InlandAgentSharedManifestsQ',
        ]);
        jest.spyOn(FeatureLocator, 'HasFeaturePermession').mockImplementation((_feature, code) =>
            allowed.has(code)
        );

        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        component.SetPropertyVisibility();

        expect(component.AgentSharedManifestsAllVisibility).toBe(true);
        expect(component.AgentSharedManifestsAirVisibility).toBe(true);
        expect(component.AgentSharedManifestsOceanVisibility).toBe(true);
        expect(component.AgentSharedManifestsInlandVisibility).toBe(true);
        expect(component.AgentSharedManifestsCancelledVisibility).toBe(false);
    });

    it('LoadDataSummary formats counts above threshold', () => {
        const service = createServiceStub({
            getAgentSharedManifestsWorkspaceSummary: jest
                .fn()
                .mockReturnValue(
                    of({
                        HasError: false,
                        Result: {
                            AgentSharedManifestsAirCount: 1500,
                            AgentSharedManifestsOceanCount: 1000,
                            AgentSharedManifestsInlandCount: 999,
                        },
                    })
                ),
        });
        const component = new SharedManifestsWorkSpaces(service as any);

        component.LoadDataSummary();

        expect(component.AgentSharedManifestsAirCount).toBe('1000+');
        expect(component.AgentSharedManifestsOceanCount).toBe('1000');
        expect(component.AgentSharedManifestsInlandCount).toBe('999');
    });

    it('LoadAllData fills filters when none selected', () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        const fillSpy = jest.spyOn(component as any, 'FillTimeRangeFilterList').mockImplementation(() => {});
        const loadBarSpy = jest.spyOn(component as any, 'LoadBarQueries').mockImplementation(() => {});
        const reloadSpy = jest.spyOn(component.ReloadAgentSharedManifestsQueries, 'emit');

        component.LoadAllData();

        expect(service.getAgentSharedManifestsWorkspaceSummary).toHaveBeenCalled();
        expect(reloadSpy).toHaveBeenCalled();
        expect(fillSpy).toHaveBeenCalled();
        expect(loadBarSpy).not.toHaveBeenCalled();
    });

    it('LoadAllData triggers LoadBarQueries when selection exists', () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        const loadBarSpy = jest.spyOn(component as any, 'LoadBarQueries').mockImplementation(() => {});
        jest.spyOn(component as any, 'FillTimeRangeFilterList').mockImplementation(() => {});

        component.SelectedTimeRangeItem = { title: 'current' } as any;
        component.LoadAllData();

        expect(loadBarSpy).toHaveBeenCalled();
    });

    it('FillTimeRangeFilterList populates list and selects week', () => {
        jest.spyOn(LastFilter, 'ActivitymyList').mockReturnValue([
            { lastTitle: 'Today' },
            { lastTitle: 'This Week' },
            { lastTitle: 'This Month' },
            { lastTitle: 'This Year' },
        ] as any);

        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        jest.spyOn(component as any, 'LoadBarQueries').mockImplementation(() => {});

        (component as any).FillTimeRangeFilterList();

        expect(component.TimeRangeFilterList).toHaveLength(4);
        expect(component.SelectedTimeRangeItem.Title).toBe('This Week');
    });

    it('ViewAgentSharedManifestQuery loads list component and refreshes', async () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        component.LoadAllData = jest.fn();

        await component.ViewAgentSharedManifestQuery('Air');

        const loadSpy = SessionLocator.DynamicLoader.Load as jest.Mock;
        const cmpRef = await loadSpy.mock.results[0].value;
        expect(loadSpy).toHaveBeenCalledWith(
            './Infrastructure/Components/ListComponent/ListComponent',
            SessionLocator.SelectedSession.SessionMenuLocation.viewContainerRef
        );
        expect(cmpRef.instance.Run).toHaveBeenCalledWith(
            expect.objectContaining({
                QueryCode: 'AirAgentSharedManifests',
                ObjectTableName: 'AgentSharedManifest',
                BackButtonTitle: 'Shared Manifests',
            })
        );
        expect(SessionLocator.SelectedSession.AddMenuReference).toHaveBeenCalledWith(cmpRef);
        expect(component.LoadAllData).toHaveBeenCalled();
    });

    it('DocumentsPermissionsLinkClick opens permissions component', () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);

        component.DocumentsPermissionsLinkClick();

        const windowInstance = LogitudeWindowMock.mock.results[0].value;
        expect(windowInstance.Show).toHaveBeenCalledWith(
            './InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent'
        );
        expect(windowInstance.Width).toBe(820);
        expect(windowInstance.Height).toBe(520);
    });
});
import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { SharedManifestsWorkSpaces } from '../Components/SharedManifestsWorkSpaces';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { LastFilter } from '../../../Infrastructure/Utilities/LastFilter';

jest.mock('../../../Controls/Windows/LogitudeWindow', () => ({
    LogitudeWindow: jest.fn().mockImplementation(() => ({
        Title: '',
        Width: 0,
        Height: 0,
        WindowArgs: null,
        Show: jest.fn(),
        IsShowCloseButton: false,
        WindowClosed: { subscribe: jest.fn() },
    })),
}));

const { LogitudeWindow } = require('../../../Controls/Windows/LogitudeWindow');
const LogitudeWindowMock = LogitudeWindow as jest.Mock;

describe('SharedManifestsWorkSpaces', () => {
    const createServiceStub = (overrides: Partial<Record<string, any>> = {}) => ({
        getAgentSharedManifestsWorkspaceSummary: jest.fn().mockReturnValue(of({})),
        GetAgentSharedManifestsForDashBoard: jest.fn().mockReturnValue(of({ Result: [] })),
        ...overrides,
    });

    beforeEach(() => {
        jest.clearAllMocks();
        LogitudeWindowMock.mockClear();
        SessionLocator.SelectedSession = {
            GetChartId: jest.fn().mockReturnValue('CHART-ID'),
            SessionMenuLocation: { viewContainerRef: {} },
            SessionLocation: { viewContainerRef: {} },
            AddMenuReference: jest.fn(),
            SessionMenuReferences: [],
            StartBusyIndicatorLoading: jest.fn(),
            StopBusyIndicator: jest.fn(),
        } as any;
        SessionLocator.DynamicLoader = {
            Load: jest.fn().mockImplementation(async () => ({
                instance: {
                    ComponentRef: {} as any,
                    Run: jest.fn() as any,
                    BackCompleted: { subscribe: (callback: any) => callback(true) } as any,
                } as any,
            })) as any,
        } as any;
        SessionInfo.LoggedUserTenant = 77;
        (global as any).makeAmBarChart = jest.fn();
        (global as any).BarClick = jest.fn();
        (global as any).ResetItem = jest.fn();
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('constructor marks share button visible when feature granted', () => {
        const featureSpy = jest
            .spyOn(FeatureLocator, 'HasFeaturePermession')
            .mockImplementation((feature, code) => feature === 'AgentSharedDocument' && code === 'NEW');

        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);

        expect(featureSpy).toHaveBeenCalledWith('AgentSharedDocument', 'NEW');
        expect(component.IsShareDocumentsButtonVisible).toBe(true);
        expect(SessionLocator.SelectedSession.GetChartId).toHaveBeenCalled();
    });

    it('SetPropertyVisibility honours feature permissions', () => {
        const allowed = new Set([
            'AgentSharedManifestQ',
            'AirAgentSharedManifestsQ',
            'OceanAgentSharedManifestsQ',
            'InlandAgentSharedManifestsQ',
        ]);
        jest.spyOn(FeatureLocator, 'HasFeaturePermession').mockImplementation((_feature, code) =>
            allowed.has(code)
        );

        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        component.SetPropertyVisibility();

        expect(component.AgentSharedManifestsAllVisibility).toBe(true);
        expect(component.AgentSharedManifestsAirVisibility).toBe(true);
        expect(component.AgentSharedManifestsOceanVisibility).toBe(true);
        expect(component.AgentSharedManifestsInlandVisibility).toBe(true);
        expect(component.AgentSharedManifestsCancelledVisibility).toBe(false);
    });

    it('LoadDataSummary maps large counts to 1000+', () => {
        const service = createServiceStub({
            getAgentSharedManifestsWorkspaceSummary: jest.fn().mockReturnValue(
            of({
                HasError: false,
                Result: {
                    AgentSharedManifestsAirCount: 1500,
                    AgentSharedManifestsOceanCount: 1000,
                    AgentSharedManifestsInlandCount: 999,
                },
            })
            ),
        });

        const component = new SharedManifestsWorkSpaces(service as any);
        component.LoadDataSummary();

        expect(component.AgentSharedManifestsAirCount).toBe('1000+');
        expect(component.AgentSharedManifestsOceanCount).toBe('1000');
        expect(component.AgentSharedManifestsInlandCount).toBe('999');
    });

    it('LoadAllData fills filters when none selected', () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        const fillSpy = jest.spyOn(component as any, 'FillTimeRangeFilterList').mockImplementation(() => {});
        const loadBarSpy = jest.spyOn(component as any, 'LoadBarQueries').mockImplementation(() => {});
        const reloadSpy = jest.spyOn(component.ReloadAgentSharedManifestsQueries, 'emit');

        component.LoadAllData();

        expect(service.getAgentSharedManifestsWorkspaceSummary).toHaveBeenCalled();
        expect(reloadSpy).toHaveBeenCalled();
        expect(fillSpy).toHaveBeenCalled();
        expect(loadBarSpy).not.toHaveBeenCalled();
    });

    it('LoadAllData calls LoadBarQueries when selection exists', () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        const loadBarSpy = jest.spyOn(component as any, 'LoadBarQueries').mockImplementation(() => {});
        jest.spyOn(component as any, 'FillTimeRangeFilterList').mockImplementation(() => {});

        component.SelectedTimeRangeItem = { title: 'existing' } as any;
        component.LoadAllData();

        expect(loadBarSpy).toHaveBeenCalled();
    });

    it('FillTimeRangeFilterList populates filters and selects second item', () => {
        jest.spyOn(LastFilter, 'ActivitymyList').mockReturnValue([
            { lastTitle: 'Today' },
            { lastTitle: 'This Week' },
            { lastTitle: 'This Month' },
            { lastTitle: 'This Year' },
        ] as any);

        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        jest.spyOn(component as any, 'LoadBarQueries').mockImplementation(() => {});
        component['FillTimeRangeFilterList']();

        expect(component.TimeRangeFilterList).toHaveLength(4);
        expect(component.SelectedTimeRangeItem.Title).toBe('This Week');
    });

    it('ViewAgentSharedManifestQuery loads list component with selected code', async () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);
        component.LoadAllData = jest.fn();
        const loadSpy = SessionLocator.DynamicLoader.Load as jest.Mock;

        component.ViewAgentSharedManifestQuery('Air');

        const cmpRef = await loadSpy.mock.results[0].value;
        expect(loadSpy).toHaveBeenCalledWith(
            './Infrastructure/Components/ListComponent/ListComponent',
            SessionLocator.SelectedSession.SessionMenuLocation.viewContainerRef
        );
        expect(cmpRef.instance.Run).toHaveBeenCalledWith(
            expect.objectContaining({
                QueryCode: 'AirAgentSharedManifests',
                ObjectTableName: 'AgentSharedManifest',
                BackButtonTitle: 'Shared Manifests',
            })
        );
        expect(SessionLocator.SelectedSession.AddMenuReference).toHaveBeenCalledWith(cmpRef);
        expect(component.LoadAllData).toHaveBeenCalled();
    });

    it('DocumentsPermissionsLinkClick opens permissions window', () => {
        const service = createServiceStub();
        const component = new SharedManifestsWorkSpaces(service as any);

        component.DocumentsPermissionsLinkClick();

        const windowInstance = LogitudeWindowMock.mock.results[0].value;
        expect(windowInstance.Show).toHaveBeenCalledWith(
            './InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent'
        );
        expect(windowInstance.Width).toBe(820);
        expect(windowInstance.Height).toBe(520);
    });
});

