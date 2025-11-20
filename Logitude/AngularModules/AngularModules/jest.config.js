// Jest projects configuration for module-scoped runs while keeping shared base config.
const base = {
	preset: 'jest-preset-angular',
	setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
	globalSetup: 'jest-preset-angular/global-setup',
	testEnvironment: 'jsdom',
	moduleFileExtensions: ['ts', 'html', 'js', 'json', 'mjs'],
	transform: {
		'^.+\\.(ts|mjs|html)$': 'ts-jest'
	},
	globals: {
		'ts-jest': {
			tsconfig: '<rootDir>/tsconfig.jest.json',
			diagnostics: { warnOnly: true }
		}
	},
	transformIgnorePatterns: ['node_modules/(?!.*\\.mjs$)'],
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
	reporters: ['default']
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
