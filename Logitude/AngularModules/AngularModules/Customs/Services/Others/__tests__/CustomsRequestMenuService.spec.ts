(global as any).window = { TextCodesCache: [], TextCodes: [] };

import { expect, jest } from '@jest/globals';
import { CustomsRequestMenuService } from '../CustomsRequestMenuService';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

jest.mock('../../../../Infrastructure/Utilities/FeatureLocator');
jest.mock('../../../../Infrastructure/Utilities/TextCodeTranslator', () => ({
    TextCodeTranslator: {
        Translate: jest.fn((key: string) => key),
    },
}));
jest.mock('../../../../Infrastructure/Tools', () => ({
    AppTool: {
        IsNullOrEmpty: jest.fn((value: any) => value === null || value === undefined || value === ''),
        GetLogitudeURL: jest.fn().mockReturnValue('http://test'),
    },
}));

const createdLogitudeWindows: any[] = [];
const createWindowInstance = () => ({
    Width: 0,
    Height: 0,
    ShowCloseButton: false,
    Title: '',
    WindowArgs: null as any,
    ComponentLoaded: { subscribe: jest.fn() },
    WindowClosed: { subscribe: jest.fn() },
    Show: jest.fn(),
});

jest.mock('../../../../Controls/Windows/LogitudeWindow', () => {
    function MockLogitudeWindow(this: any) {
        Object.assign(this, createWindowInstance());
        createdLogitudeWindows.push(this);
    }
    return {
        __esModule: true,
        LogitudeWindow: MockLogitudeWindow,
        default: MockLogitudeWindow,
    };
});

const confirmWindowInstances: any[] = [];
const confirmWindowFactory = jest.fn().mockImplementation(() => {
    const instance = {
        Show: jest.fn(),
        Yes: true,
        WindowClosed: {
            subscribe: jest.fn((cb: any) => cb(true)),
        },
    };
    confirmWindowInstances.push(instance);
    return instance;
});

jest.mock('../../../../Controls/Windows/ConfirmWindow', () => ({
    __esModule: true,
    ConfirmWindow: function ConfirmWindowMock(this: any) {
        return confirmWindowFactory();
    },
}));

const defaultStepDocuments = [
    { DocumentData: JSON.stringify({ request: 'data' }) },
    { DocumentData: JSON.stringify({ response: 'data' }) },
];
let documentStepsResult = [...defaultStepDocuments];
let requestParamStepsResult = [...defaultStepDocuments];

function MockCommunicationLogStepListService(this: any) {
    this.GetCommunicationLogStepsDocumentDataBystringStepFilter = jest.fn().mockReturnValue({
        subscribe: (callback: any) =>
            callback({
                Result: documentStepsResult,
            }),
    });
    this.getCommunicationLogStepsRequestParamResponseData = jest.fn().mockReturnValue({
        subscribe: (callback: any) =>
            callback({
                Result: requestParamStepsResult,
            }),
    });
}

jest.mock('../../../../Common/Services/ExtendedLists/CommunicationLogStepListService', () => ({
    __esModule: true,
    CommunicationLogStepListService: MockCommunicationLogStepListService,
    default: MockCommunicationLogStepListService,
}));

jest.mock('../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging', () => ({
    BaseRequestsSheetMassaging: class {
        public IsViewChildCustomMessageWrapperComponentInit = true;
        public MyCustomsMenuItem: any;
        public MyCommunicationLogId: any;
        public CustomRequestContentIsDisable = false;
        public CustomResponseContentIsDisable = false;
        public CustomSendOptionsButtonIsDisable = false;
        MassageDisplay = jest.fn();
        DisposeMyState = jest.fn();
        SetMenuArg = jest.fn();
    },
}));

jest.mock('../../../../Infrastructure/Utilities/DownloadManager', () => ({
    DownloadManager: {
        DownloadPage: jest.fn(),
    },
}));

describe('CustomsRequestMenuService', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        createdLogitudeWindows.length = 0;
        (global as any).window.TextCodesCache = [];
        (global as any).window.TextCodes = [];
        documentStepsResult = [...defaultStepDocuments];
        requestParamStepsResult = [...defaultStepDocuments];
        confirmWindowInstances.length = 0;
        SessionLocator.LoggedUserPM = { DontShowLocal: false } as any;
        SessionLocator.SelectedSession = {
            StartBusyIndicator: jest.fn(),
            StopBusyIndicator: jest.fn(),
        } as any;
        SessionLocator.Tenant = 77;
        (FeatureLocator.HasFeaturePermession as jest.Mock).mockReturnValue(true);
        (AppTool.IsNullOrEmpty as jest.Mock).mockImplementation((value: any) => value === null || value === undefined || value === '');
    });

    it('builds default customs menu when permissions allow', () => {
        const service = new CustomsRequestMenuService(false);

        expect(service.CustomsRequestMenuItems.length).toBeGreaterThan(0);
        expect(
            service.CustomsRequestMenuItems.some(item => item.ScreenName === 'DeclarationStatusQuery')
        ).toBe(true);
    });

    it('builds reports list when constructor flag is true', () => {
        const service = new CustomsRequestMenuService(true);

        expect(service.CustomsRequestMenuItems.length).toBeGreaterThan(0);
    });

    it('ShowModalByIdAndIntreface warns when id missing', () => {
        const warnSpy = jest.spyOn(console, 'warn').mockImplementation(() => undefined);
        const service = new CustomsRequestMenuService(false);
        const showSpy = jest.spyOn(service as any, 'ShowModal');

        service.ShowModalByIdAndIntreface('', 'CODE', 'desc');

        expect(warnSpy).toHaveBeenCalledWith(expect.stringContaining('id is must'));
        expect(showSpy).not.toHaveBeenCalled();
        warnSpy.mockRestore();
    });

    it('ShowModalByIdAndIntreface warns when interface code missing', () => {
        const warnSpy = jest.spyOn(console, 'warn').mockImplementation(() => undefined);
        const service = new CustomsRequestMenuService(false);

        service.ShowModalByIdAndIntreface('123', '', 'desc');

        expect(warnSpy).toHaveBeenCalledWith(expect.stringContaining('InterfaceTypeCode is must'));
        warnSpy.mockRestore();
    });

    it('ShowModalByIdAndIntreface falls back to default when interface not found', () => {
        const service = new CustomsRequestMenuService(false);
        const showDefaultSpy = jest.spyOn(service as any, 'ShowModalDefault').mockImplementation(() => undefined);
        const showSpy = jest.spyOn(service as any, 'ShowModal');

        service.ShowModalByIdAndIntreface('LOG-1', 'UNKNOWN', 'desc');

        expect(showDefaultSpy).toHaveBeenCalledWith('LOG-1', 'UNKNOWN', 'desc');
        expect(showSpy).not.toHaveBeenCalled();
    });

    it('ShowModalByIdAndIntreface loads matching item', () => {
        const service = new CustomsRequestMenuService(false);
        const target = service.CustomsRequestMenuItems.find(item => !(AppTool.IsNullOrEmpty as jest.Mock)(item.MainInterfaceCode));
        const showSpy = jest.spyOn(service as any, 'ShowModal').mockImplementation(() => undefined);

        service.ShowModalByIdAndIntreface('LOG-2', target!.MainInterfaceCode!, 'desc');

        expect(showSpy).toHaveBeenCalledWith(target, 'LOG-2', null);
    });

    it('ShowModalAsEditMenuAction warns when interface code missing', () => {
        const warnSpy = jest.spyOn(console, 'warn').mockImplementation(() => undefined);
        const service = new CustomsRequestMenuService(false);

        service.ShowModalAsEditMenuAction('', {});

        expect(warnSpy).toHaveBeenCalledWith(expect.stringContaining('InterfaceTypeCode is must'));
        warnSpy.mockRestore();
    });

    it('ShowModalAsEditMenuAction invokes ShowModal with menu argument', () => {
        const service = new CustomsRequestMenuService(false);
        const target = service.CustomsRequestMenuItems.find(item => !(AppTool.IsNullOrEmpty as jest.Mock)(item.MainInterfaceCode));
        const showSpy = jest.spyOn(service as any, 'ShowModal').mockImplementation(() => undefined);

        service.ShowModalAsEditMenuAction(target!.MainInterfaceCode!, { foo: 'bar' });

        expect(showSpy).toHaveBeenCalledWith(target, null, { foo: 'bar' });
    });

    it('ShowModal returns immediately when URL is empty', () => {
        const service = new CustomsRequestMenuService(false);

        service.ShowModal({ URLContent: '', WindowWidth: 100, WindowHeight: 100 } as any, 'ID', null);

        expect(createdLogitudeWindows.length).toBe(0);
    });

    it('ShowModal propagates menu argument through SetMenuArg when provided', () => {
        const service = new CustomsRequestMenuService(false);
        const target = service.CustomsRequestMenuItems.find(item => !(AppTool.IsNullOrEmpty as jest.Mock)(item.URLContent));
        const menuArg = { foo: 'bar' };

        service.ShowModal(target as any, null, menuArg);

        expect(createdLogitudeWindows.length).toBeGreaterThan(0);
        const windowInstance = createdLogitudeWindows[createdLogitudeWindows.length - 1];
        const componentLoaded = windowInstance.ComponentLoaded.subscribe as jest.Mock;
        expect(componentLoaded).toHaveBeenCalled();
        const callback = componentLoaded.mock.calls[0][0];
        const fakeComponent = {
            IsViewChildCustomMessageWrapperComponentInit: true,
            MyCustomsMenuItem: null,
            CustomRequestContentIsDisable: false,
            CustomResponseContentIsDisable: false,
            CustomSendOptionsButtonIsDisable: false,
            MassageDisplay: jest.fn(),
            DisposeMyState: jest.fn(),
            SetMenuArg: jest.fn(),
        };
        callback(fakeComponent);

        expect(fakeComponent.SetMenuArg).toHaveBeenCalledWith(menuArg);
        expect(windowInstance.Show).toHaveBeenCalledWith(target!.URLContent);
        const windowClosed = windowInstance.WindowClosed.subscribe as jest.Mock;
        expect(windowClosed).toHaveBeenCalled();
    });

    it('ViewXMLClicked downloads by security id when provided', () => {
        const service = new CustomsRequestMenuService(false);

        service.ViewXMLClicked({ DocumentId: 'DOC', SecurityId: 'SEC' } as any);

        expect(DownloadManager.DownloadPage).toHaveBeenCalledWith('', 'SEC');
    });

    it('ViewXMLClicked downloads by document id when security id missing', () => {
        const service = new CustomsRequestMenuService(false);

        service.ViewXMLClicked({ DocumentId: 'DOC' } as any);

        expect(DownloadManager.DownloadPage).toHaveBeenCalledWith('DOC', null);
    });

    it('ShowModalDefault populates window args and shows component', () => {
        const service = new CustomsRequestMenuService(false);
        const startSpy = SessionLocator.SelectedSession.StartBusyIndicator as jest.Mock;
        const stopSpy = SessionLocator.SelectedSession.StopBusyIndicator as jest.Mock;

        service.ShowModalDefault('LOG-3', 'CODE', 'Request');

        expect(startSpy).toHaveBeenCalled();
        expect(stopSpy).toHaveBeenCalled();
        const windows = [...createdLogitudeWindows];
        expect(windows.length).toBeGreaterThan(0);
        const windowInstance = windows[windows.length - 1];
        expect(windowInstance.Title).toBe('Request');
        expect(windowInstance.WindowArgs.AnalyzeMessage).toEqual({ response: 'data' });
        expect(windowInstance.WindowArgs.CustomResponse).toEqual({ request: 'data' });
        expect(windowInstance.Show).toHaveBeenCalledWith('./CustomsModules/CustomsControls/Components/ObjectViewerComponent');
    });

    it('ShowModalDefault prompts for download when payload flagged as huge', () => {
        documentStepsResult = [
            {
                DocumentData: '(item.DocumentData.Length * sizeof(Char) > sizeOf250KB)',
                SecurityId: 'SEC-HUGE',
                DocumentId: 'DOC-HUGE',
            } as any,
            { DocumentData: JSON.stringify({ response: 'data' }) },
        ];
        const service = new CustomsRequestMenuService(false);
        const viewSpy = jest.spyOn(service, 'ViewXMLClicked').mockImplementation(() => undefined);

        service.ShowModalDefault('LOG-4', 'CODE', 'Large');

        expect(confirmWindowFactory).toHaveBeenCalled();
        expect(confirmWindowInstances.length).toBeGreaterThan(0);
        expect(viewSpy).toHaveBeenCalledWith(expect.objectContaining({ SecurityId: 'SEC-HUGE' }));
        expect(createdLogitudeWindows.length).toBeGreaterThan(0);
        const lastWindow = createdLogitudeWindows[createdLogitudeWindows.length - 1];
        expect(lastWindow.Show).not.toHaveBeenCalled();
    });
});

