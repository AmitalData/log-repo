import 'jest-preset-angular/setup-jest';

Object.defineProperty(window, 'matchMedia', {
    writable: true,
    value: (query: any) => ({
        matches: false,
        media: query,
        onchange: null,
        addListener: jest.fn(),
        removeListener: jest.fn(),
        addEventListener: jest.fn(),
        removeEventListener: jest.fn(),
        dispatchEvent: jest.fn()
    })
});

Object.defineProperty(window, 'scrollTo', {
    writable: true,
    value: jest.fn()
});

global['SessionLocator'] = global['SessionLocator'] || {};

