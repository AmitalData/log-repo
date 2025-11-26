// Jest projects configuration for module-scoped runs while keeping shared base config.
const base = {
	preset: 'jest-preset-angular',
	setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
	globalSetup: 'jest-preset-angular/global-setup',
	testEnvironment: 'jsdom',
	
	// Performance: Order matters - check TypeScript first
	moduleFileExtensions: ['ts', 'html', 'js', 'json', 'mjs'],
	
	// Performance: Optimize transform with isolatedModules (skips type checking)
	// Note: SWC not compatible with jest-preset-angular, using ts-jest with isolatedModules
	transform: {
		'^.+\\.(ts|mjs|html)$': 'ts-jest'
	},
	globals: {
		'ts-jest': {
			tsconfig: '<rootDir>/tsconfig.jest.json',
			diagnostics: { warnOnly: true },
			isolatedModules: true
		}
	},
	
	// Performance: Skip more node_modules transformations
	transformIgnorePatterns: [
		'node_modules/(?!.*\\.mjs$|@angular|@azure|@microsoft|ag-grid|primeng|ng-zorro)'
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
	
	// Performance: Enable caching explicitly
	cache: true,
	cacheDirectory: '<rootDir>/.jest-cache',
	
	// Performance: Clean mocks between tests for isolation
	clearMocks: true,
	restoreMocks: true,
	
	// Performance: Optimized timeout (reduced from 10000ms for faster feedback)
	testTimeout: 5000,
	
	// Performance: Optimize module resolution
	moduleDirectories: ['node_modules', '<rootDir>'],
	
	// Performance: Only collect coverage from source files, not tests
	collectCoverageFrom: [
		'**/*.ts',
		'!**/*.spec.ts',
		'!**/*.d.ts',
		'!**/__tests__/**',
		'!**/node_modules/**',
		'!**/dist/**',
		'!**/coverage/**'
	],
	coveragePathIgnorePatterns: [
		'/node_modules/',
		'/dist/',
		'/coverage/',
		'\\.spec\\.ts$',
		'\\.d\\.ts$'
	],
	
	reporters: ['default'],
	maxWorkers: 4  // Optimized: 4 workers showed best performance (233s vs 318s with 6)
};

function project(displayName, relDir) {
	return {
		...base,
		displayName,
		testMatch: [`<rootDir>/${relDir}/**/__tests__/**/*.spec.ts`],
		roots: [`<rootDir>/${relDir}`],
		coverageDirectory: `<rootDir>/coverage-jest/${displayName}`
	};
}

module.exports = {
	projects: [
		project('shipment-modules', 'ShipmentModules'),
		project('shipment', 'Shipment'),
		project('accounting', 'Accounting'),
		project('customs', 'Customs'),
		project('crm', 'CRM'),
		project('common-modules', 'CommonModules'),
		project('infrastructure-modules', 'InfrastructureModules'),
		project('invoice', 'Invoice'),
		project('controls', 'Controls'),
		project('common', 'Common')
	],
	// Keep a top-level project to allow running everything if needed (optional):
	// ...project('all', '')
};
