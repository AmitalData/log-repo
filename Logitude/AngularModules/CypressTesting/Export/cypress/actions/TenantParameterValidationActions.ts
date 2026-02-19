/**
 * Actions for Bug 117820 - Tenant Parameter Validation Testing
 * Contains functions for network monitoring and tenant parameter validation
 */

import { TenantParameterValidationSelectors } from '../selectors/TenantParameterValidationSelectors';
import {
    NetworkRequest,
    TenantValidationResult,
    NetworkMonitoringConfig,
    DEFAULT_EXCLUDED_ENDPOINTS,
    CRITICAL_EXPORT_ENDPOINTS,
    shouldExcludeEndpoint,
    isCriticalExportEndpoint,
    validateNetworkRequest,
    formatRequestForReport
} from '../models/TenantParameterValidationDetails';

// Global network monitoring configuration
let monitoringConfig: NetworkMonitoringConfig = {
    isActive: false,
    capturedRequests: [],
    expectedTenant: 0,
    excludedEndpoints: DEFAULT_EXCLUDED_ENDPOINTS,
    startTime: new Date()
};

/**
 * Starts network request monitoring with interception
 * Captures all API requests for tenant validation
 */
export function StartNetworkMonitoring() {
    cy.log('🔍 Starting network request monitoring for tenant validation');
    
    // Get expected tenant from session (assumes user is logged in)
    cy.window().then((win: any) => {
        if (win.SessionLocator && win.SessionLocator.Tenant) {
            monitoringConfig.expectedTenant = win.SessionLocator.Tenant;
            cy.log(`Expected Tenant: ${monitoringConfig.expectedTenant}`);
        } else {
            cy.log('⚠️  Warning: Could not determine expected tenant from SessionLocator');
        }
    });
    
    // Reset captured requests
    monitoringConfig.capturedRequests = [];
    monitoringConfig.isActive = true;
    monitoringConfig.startTime = new Date();
    
    // Intercept all API requests (Cypress 6.5.0 syntax)
    cy.intercept('**/api/**', (req) => {
        // Skip excluded endpoints (infrastructure/metadata that don't need tenant)
        if (shouldExcludeEndpoint(req.url)) {
            return; // Let it pass through without validation
        }
        
        // Create network request object
        const networkRequest: NetworkRequest = {
            url: req.url,
            method: req.method,
            body: req.body,
            headers: req.headers,
            timestamp: new Date(),
            hasTenantParameter: false,
            tenantValue: null
        };
        
        // Validate the request for tenant parameter
        const validatedRequest = validateNetworkRequest(networkRequest);
        
        // Store the request
        monitoringConfig.capturedRequests.push(validatedRequest);
        
        // Log critical Export endpoints (use console.log, not cy.log inside intercept)
        if (isCriticalExportEndpoint(req.url)) {
            console.log(`🎯 Critical Export Endpoint: ${req.method} ${req.url} - Tenant: ${validatedRequest.hasTenantParameter ? '✓' : '✗'}`);
        }
        
        // Let request continue (no req.continue() in Cypress 6.5.0)
    }).as('apiRequests');
    
    cy.log('✓ Network monitoring enabled');
}

/**
 * Stops network request monitoring
 */
export function StopNetworkMonitoring() {
    cy.log('🛑 Stopping network request monitoring');
    monitoringConfig.isActive = false;
}

/**
 * Resets network request monitoring (clears captured requests)
 */
export function ResetRequestMonitoring() {
    cy.log('🔄 Resetting network request monitoring');
    monitoringConfig.capturedRequests = [];
    monitoringConfig.startTime = new Date();
}

/**
 * Navigates to Export workspace
 */
export function NavigateToExportWorkspace() {
    cy.log('Navigating to Export workspace');
    cy.Click(TenantParameterValidationSelectors.ExportWorkspaceMenu, null, true);
    cy.wait(2000);
}

/**
 * Opens an existing Export declaration (first one in list)
 */
export function OpenExistingExportDeclaration() {
    cy.log('Opening existing Export declaration');
    
    // Wait for rows to appear in the grid
    cy.get('.Row', { timeout: 15000 }).should('have.length.greaterThan', 0);
    cy.wait(1000);
    
    // Click first row in the grid using .Row class
    cy.get('.Row').eq(0).should('be.visible').click();
    
    cy.wait(2000);
}

/**
 * Opens Export Cancellation dialog
 */
export function OpenExportCancellationDialog() {
    cy.log('Opening Export Cancellation dialog');
    
    // Click cancellation button (may be in menu)
    cy.get('body').then(($body) => {
        if ($body.find(TenantParameterValidationSelectors.ExportCancellationButton).length > 0) {
            cy.Click(TenantParameterValidationSelectors.ExportCancellationButton, null);
        } else {
            // Try to find in context menu or action buttons
            cy.get(TenantParameterValidationSelectors.DeclarationMenuButtons)
                .find('button')
                .contains(/ביטול|Cancel/i)
                .click();
        }
    });
    
    cy.wait(2000);
    
    // Verify dialog opened
    cy.get(TenantParameterValidationSelectors.ExportCancellationDialog, { timeout: 5000 })
        .should('exist');
}

/**
 * Opens Export Storage Declaration
 */
export function OpenExportStorageDeclaration() {
    cy.log('Opening Export Storage Declaration');
    
    cy.get('body').then(($body) => {
        if ($body.find(TenantParameterValidationSelectors.ExportStorageDeclarationMenu).length > 0) {
            cy.Click(TenantParameterValidationSelectors.ExportStorageDeclarationMenu, null);
        } else {
            // Navigate through menu
            cy.get(TenantParameterValidationSelectors.DeclarationMenuButtons)
                .find('button')
                .contains(/אחסנה|Storage/i)
                .click();
        }
    });
    
    cy.wait(2000);
}

/**
 * Loads Export Storage Declaration data
 */
export function LoadExportStorageDeclarationData() {
    cy.log('Loading Export Storage Declaration data');
    
    // Wait for grid to be visible
    cy.get(TenantParameterValidationSelectors.StorageDeclarationGrid, { timeout: 10000 })
        .should('be.visible');
    
    cy.wait(1000);
}

/**
 * Loads Export declarations list
 */
export function LoadExportDeclarationsList() {
    cy.log('Loading Export declarations list');
    
    // Navigate to Export workspace
    NavigateToExportWorkspace();
    
    // Wait for data to load
    cy.wait(3000);
}

/**
 * Applies filters to Export declarations list
 */
export function ApplyCustomFilters() {
    cy.log('Applying custom filters to Export declarations list');
    
    // Enter search text (if search input exists)
    cy.get('body').then(($body) => {
        if ($body.find(TenantParameterValidationSelectors.SearchInput).length > 0) {
            cy.get(TenantParameterValidationSelectors.SearchInput).type('TEST');
            cy.wait(500);
        }
    });
    
    // Click apply filters (if button exists)
    cy.get('body').then(($body) => {
        if ($body.find(TenantParameterValidationSelectors.ApplyFiltersButton).length > 0) {
            cy.Click(TenantParameterValidationSelectors.ApplyFiltersButton, null);
            cy.wait(1000);
        }
    });
}

/**
 * Refreshes Export declarations list
 * Note: Avoid cy.reload() as it logs user out
 */
export function RefreshExportDeclarationsList() {
    cy.log('Refreshing Export declarations list (re-navigating)');
    
    // Navigate away and back
    cy.get('body').click();
    cy.wait(500);
    cy.Click(TenantParameterValidationSelectors.ExportWorkspaceMenu, null, true);
    cy.wait(2000);
}

/**
 * Initiates Declaration Closure operation
 */
export function InitiateDeclarationClosure() {
    cy.log('Initiating Declaration Closure operation');
    
    cy.get('body').then(($body) => {
        if ($body.find(TenantParameterValidationSelectors.DeclarationClosureButton).length > 0) {
            cy.Click(TenantParameterValidationSelectors.DeclarationClosureButton, null);
        } else {
            // Try operational closure
            if ($body.find(TenantParameterValidationSelectors.OperationalClosureButton).length > 0) {
                cy.Click(TenantParameterValidationSelectors.OperationalClosureButton, null);
            }
        }
    });
    
    cy.wait(2000);
}

/**
 * Performs multiple Export operations as specified in data table
 */
export function PerformMultipleExportOperations(operations: any[]) {
    cy.log('Performing multiple Export operations');
    
    operations.forEach((op, index) => {
        const operation = op.Operation || op.operation;
        cy.log(`Operation ${index + 1}: ${operation}`);
        
        switch (operation) {
            case 'Load Export list':
                LoadExportDeclarationsList();
                break;
            case 'Open Export declaration':
                OpenExistingExportDeclaration();
                break;
            case 'View Cancellation dialog':
                OpenExportCancellationDialog();
                break;
            case 'Load Storage declarations':
                OpenExportStorageDeclaration();
                LoadExportStorageDeclarationData();
                break;
            case 'Apply custom filters':
                ApplyCustomFilters();
                break;
            default:
                cy.log(`⚠️  Unknown operation: ${operation}`);
        }
        
        cy.wait(1000);
    });
}

/**
 * Validates that all captured API requests include tenant parameter
 * @returns Validation result with details
 */
export function ValidateAllRequestsHaveTenant(): Cypress.Chainable<TenantValidationResult> {
    cy.log('🔍 Validating all captured requests for tenant parameter');
    
    return cy.wrap(null).then(() => {
        const requestsWithoutTenant = monitoringConfig.capturedRequests.filter(
            req => !req.hasTenantParameter
        );
        
        const result: TenantValidationResult = {
            totalRequests: monitoringConfig.capturedRequests.length,
            requestsWithTenant: monitoringConfig.capturedRequests.length - requestsWithoutTenant.length,
            requestsMissingTenant: requestsWithoutTenant,
            isValid: requestsWithoutTenant.length === 0,
            expectedTenantValue: monitoringConfig.expectedTenant
        };
        
        cy.log(`Total API Requests: ${result.totalRequests}`);
        cy.log(`Requests With Tenant: ${result.requestsWithTenant}`);
        cy.log(`Requests Missing Tenant: ${result.requestsMissingTenant.length}`);
        
        if (!result.isValid) {
            cy.log(`❌ Found ${result.requestsMissingTenant.length} requests missing tenant parameter:`);
            result.requestsMissingTenant.forEach((req, index) => {
                cy.log(`${index + 1}. ${formatRequestForReport(req)}`);
            });
        } else {
            cy.log('✅ All API requests include tenant parameter');
        }
        
        return result;
    });
}

/**
 * Validates that tenant parameter matches the expected user tenant
 */
export function ValidateTenantMatchesUser() {
    cy.log('🔍 Validating tenant parameter matches logged user tenant');
    
    ValidateAllRequestsHaveTenant().then((result) => {
        if (!result.isValid) {
            throw new Error(`${result.requestsMissingTenant.length} requests missing tenant parameter`);
        }
        
        // Check if tenant values match expected
        const incorrectTenantRequests = monitoringConfig.capturedRequests.filter(
            req => req.hasTenantParameter && req.tenantValue !== monitoringConfig.expectedTenant
        );
        
        if (incorrectTenantRequests.length > 0) {
            cy.log(`❌ Found ${incorrectTenantRequests.length} requests with incorrect tenant value:`);
            incorrectTenantRequests.forEach((req, index) => {
                cy.log(`${index + 1}. Expected: ${monitoringConfig.expectedTenant}, Got: ${req.tenantValue}`);
                cy.log(`   ${req.method} ${req.url}`);
            });
            throw new Error(`${incorrectTenantRequests.length} requests have incorrect tenant value`);
        }
        
        cy.log(`✅ All requests have correct tenant value: ${monitoringConfig.expectedTenant}`);
    });
}

/**
 * Asserts that no requests are missing tenant filtering
 */
export function AssertNoRequestsMissingTenant() {
    cy.log('✓ Asserting no requests missing tenant parameter');
    
    ValidateAllRequestsHaveTenant().then((result) => {
        expect(result.isValid, 'All requests should include tenant parameter').to.be.true;
        expect(result.requestsMissingTenant.length, 'No requests should be missing tenant').to.equal(0);
    });
}

/**
 * Asserts that closure request includes tenant in body
 */
export function AssertClosureRequestHasTenantInBody() {
    cy.log('✓ Asserting closure request includes tenant in body');
    
    const closureRequests = monitoringConfig.capturedRequests.filter(req =>
        req.url.toLowerCase().includes('closure') && req.method === 'POST'
    );
    
    if (closureRequests.length === 0) {
        cy.log('⚠️  No closure requests found');
        return;
    }
    
    closureRequests.forEach((req) => {
        expect(req.hasTenantParameter, `Closure request should have tenant: ${req.url}`).to.be.true;
        expect(req.tenantLocation, 'Tenant should be in body or URL').to.be.oneOf(['body', 'url']);
        cy.log(`✅ Closure request has tenant: ${req.url}`);
    });
}

/**
 * Asserts that no tenant parameter is null or undefined
 */
export function AssertNoNullOrUndefinedTenant() {
    cy.log('✓ Asserting no tenant parameter is null or undefined');
    
    const nullTenantRequests = monitoringConfig.capturedRequests.filter(req =>
        req.hasTenantParameter && (req.tenantValue === null || req.tenantValue === undefined || req.tenantValue === 0)
    );
    
    if (nullTenantRequests.length > 0) {
        cy.log(`❌ Found ${nullTenantRequests.length} requests with null/undefined tenant:`);
        nullTenantRequests.forEach((req, index) => {
            cy.log(`${index + 1}. ${req.method} ${req.url}`);
        });
        throw new Error(`${nullTenantRequests.length} requests have null/undefined tenant`);
    }
    
    cy.log('✅ No requests have null or undefined tenant');
}

/**
 * Gets requests that are missing tenant parameter
 */
export function GetRequestsMissingTenant(): NetworkRequest[] {
    return monitoringConfig.capturedRequests.filter(req => !req.hasTenantParameter);
}

/**
 * Gets the current network monitoring configuration
 */
export function GetMonitoringConfig(): NetworkMonitoringConfig {
    return monitoringConfig;
}

/**
 * Gets count of captured requests
 */
export function GetCapturedRequestsCount(): number {
    return monitoringConfig.capturedRequests.length;
}

