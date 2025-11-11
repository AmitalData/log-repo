import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

export interface DynamicLoaderMock {
    Load: jest.Mock<Promise<any>, [string, any]>;
}

export interface WindowMock {
    StopBusyIndicator: jest.Mock<void, []>;
    Close?: jest.Mock<void, [any?]>;
}

export interface SessionMock {
    CurrentWindow: WindowMock;
    StartBusyIndicator: jest.Mock<void, [string?]>;
    StopBusyIndicator: jest.Mock<void, []>;
    StartBusyIndicatorLoading?: jest.Mock<void, [string?]>;
}

export interface SessionLocatorMocks {
    dynamicLoader: DynamicLoaderMock;
    session: SessionMock;
}

const createDynamicComponentInstance = () => ({
    Run: jest.fn(),
    LabelWidth: undefined,
    LoadCompleted: {
        subscribe: jest.fn()
    }
});

export const createDynamicLoaderMock = (): DynamicLoaderMock => ({
    Load: jest.fn().mockImplementation(() =>
        Promise.resolve({
            instance: createDynamicComponentInstance()
        })
    )
});

export const createSessionMock = (): SessionMock => ({
    CurrentWindow: {
        StopBusyIndicator: jest.fn(),
        Close: jest.fn()
    },
    StartBusyIndicator: jest.fn(),
    StopBusyIndicator: jest.fn(),
    StartBusyIndicatorLoading: jest.fn()
});

export const configureSessionLocator = (overrides?: Partial<SessionLocatorMocks>): SessionLocatorMocks => {
    const dynamicLoader = overrides?.dynamicLoader || createDynamicLoaderMock();
    const session = overrides?.session || createSessionMock();

    SessionLocator.DynamicLoader = dynamicLoader as unknown as any;
    SessionLocator.SelectedSession = session as unknown as any;

    return { dynamicLoader, session };
};

