module.exports = {
    preset: 'jest-preset-angular',
    setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
    globalSetup: 'jest-preset-angular/global-setup',
    testMatch: ['**/__tests__/**/*.spec.ts'],
    testEnvironment: 'jsdom',
    moduleFileExtensions: ['ts', 'html', 'js', 'json', 'mjs'],
    transform: {
        '^.+\\.(ts|mjs|html)$': 'ts-jest'
    },
    globals: {
        'ts-jest': {
            tsconfig: '<rootDir>/tsconfig.jest.json',
            diagnostics: {
                warnOnly: true
            }
        }
    },
    transformIgnorePatterns: [
        'node_modules/(?!.*\\.mjs$)'
    ],
    moduleNameMapper: {
        '\\.(css|scss|sass|less)$': '<rootDir>/__mocks__/styleMock.js',
        '\\.(jpg|jpeg|png|gif|svg)$': '<rootDir>/__mocks__/fileMock.js',
        '^environments/(.*)$': '<rootDir>/environments/$1',
        '^Infrastructure/(.*)$': '<rootDir>/Infrastructure/$1',
        '^Common/(.*)$': '<rootDir>/Common/$1',
        '^Controls/(.*)$': '<rootDir>/Controls/$1',
        '^ShipmentModules/(.*)$': '<rootDir>/ShipmentModules/$1',
        '^Shipment/(.*)$': '<rootDir>/Shipment/$1',
        '^Customs/(.*)$': '<rootDir>/Customs/$1',
        '^CommonModules/(.*)$': '<rootDir>/CommonModules/$1',
        '^InfrastructureModules/(.*)$': '<rootDir>/InfrastructureModules/$1',
        '^Invoice/(.*)$': '<rootDir>/Invoice/$1',
        '^Accounting/(.*)$': '<rootDir>/Accounting/$1'
    },
    collectCoverageFrom: [
        'ShipmentModules/ShipmentSharedManifest/Components/SharedManifestEditAgentComponent.ts',
        'ShipmentModules/ShipmentSharedManifest/Components/SharedManifestComponent.ts',
        'ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent.ts',
        'ShipmentModules/ShipmentSharedManifest/Components/SharedManifestsWorkSpaces.ts',
        'Shipment/Services/Others/SharedAgentManifestService.ts',
        'Accounting/Services/ModulesService.ts',
        'Customs/Services/Others/CustomsRequestMenuService.ts',
        'Customs/Services/Others/CourierMasterService.ts',
        'Customs/Services/Others/GITITEMCacheService.ts',
        'Customs/Services/Others/CacheCourierPendingReasonService.ts',
        'Customs/Services/WebServices/PendingWebService.ts',
        'Customs/Services/Others/MultiCertificatesService.ts',
        'CRM/Services/TicketCorrespondencesService.ts',
        'CRM/Services/InsertCorrespondenceService.ts',
        'CRM/Services/CRMDomainService.ts'
    ],
    coverageDirectory: '<rootDir>/coverage-jest',
    reporters: [
        'default'
    ],
    roots: ['<rootDir>']
};

