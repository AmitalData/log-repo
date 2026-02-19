/**
 * Regression Test Step Definitions for Bug 117820 - Tenant Parameter Validation
 * Focuses on the core issue: verifying tenant parameters in network requests
 */

import { Given, When, Then, And } from 'cypress-cucumber-preprocessor/steps';
import { BaseExportSelectors } from '../../selectors/BaseExportSelectors';

// Network request storage
let capturedRequests: any[] = [];
const expectedTenant = 106;

Given("the user is logged in to the system", () => {
    cy.Login();
});

And("network monitoring captures declaration API requests only", () => {
    cy.log('🔍 Starting network monitoring for declaration endpoints');
    
    // Reset requests array
    capturedRequests = [];
    
    // Intercept all API calls
    cy.intercept('**/api/**', (req) => {
        // ONLY test endpoints that request declarations and views (business logic data)
        const declarationEndpoints = [
            '/Declaration/',              // All declaration operations
            '/ExportStorage/',            // Export storage declarations
            '/CustomsRequest/',           // Customs requests
            '/Consignment/',              // Cargo/consignment data
            '/DeclarationItem/',          // Declaration items
            '/SupplierInvoice/',          // Supplier invoices for declarations
            'SendDeclarationCancellation', // The specific bug endpoint!
            'Closure',                    // Declaration closure
            'referencestatusviews',       // Reference status views - NEEDS TENANT
            'GetLastTableUpdateDate',     // Last update date - NEEDS TENANT
            'declarationviews',           // Declaration views - NEEDS TENANT
            'ngMetaData',                 // Metadata - NEEDS TENANT
        ];
        
        // Only capture if URL contains declaration-related endpoints
        const isDeclarationEndpoint = declarationEndpoints.some(endpoint => 
            req.url.toLowerCase().includes(endpoint.toLowerCase())
        );
        
        if (isDeclarationEndpoint) {
            capturedRequests.push({
                url: req.url,
                method: req.method,
                body: req.body
            });
            // Note: Cannot use cy.log() inside intercept callback in Cypress 6.5.0
            console.log(`📡 Captured Declaration Request: ${req.method} ${req.url}`);
        }
    }).as('apiCalls');
});

When("the user navigates to Export workspace and loads data", () => {
    cy.log('Navigating to Export workspace');
    cy.Click(BaseExportSelectors.ExportDeclaration, null, true);
    cy.wait(10000); // Slow data pull
});

And("the user opens the first Export declaration", () => {
    cy.log('Opening first Export declaration');
    cy.get('.Row', { timeout: 15000 }).should('have.length.greaterThan', 0);
    cy.wait(2000); // Let data settle
    cy.get('.Row').eq(0).should('be.visible').click();
    cy.wait(10000); // Slow data pull
});

Then("all captured declaration requests should include tenant parameter in URL or body", () => {
    cy.log(`🔍 Validating ${capturedRequests.length} captured declaration requests`);
    
    cy.wrap(null).then(() => {
        const requestsWithInvalidTenant: any[] = [];
        
        capturedRequests.forEach((req) => {
            // Check if URL has tenant parameter
            const hasTenantParam = req.url.includes('tenant=') || req.url.includes('Tenant=');
            
            if (hasTenantParam) {
                // Extract tenant value from URL
                const tenantMatch = req.url.match(/[&?]tenant=([^&]*)/i);
                const tenantValue = tenantMatch ? tenantMatch[1] : null;
                
                // Verify tenant has a valid value (not empty, not null, not undefined)
                if (!tenantValue || tenantValue === '' || tenantValue === 'null' || tenantValue === 'undefined' || tenantValue === '0') {
                    requestsWithInvalidTenant.push({
                        ...req,
                        issue: `tenant parameter is empty or invalid: "${tenantValue}"`
                    });
                }
            }
            // If URL doesn't have tenant param, check body
            else if (req.body && (req.body.tenant || req.body.Tenant)) {
                const tenantValue = req.body.tenant || req.body.Tenant;
                if (!tenantValue || tenantValue === 0) {
                    requestsWithInvalidTenant.push({
                        ...req,
                        issue: `tenant in body is empty or invalid: "${tenantValue}"`
                    });
                }
            }
            // Otherwise, it's OK - not all endpoints need tenant
        });
        
        if (requestsWithInvalidTenant.length > 0) {
            cy.log(`❌ Found ${requestsWithInvalidTenant.length} requests with invalid tenant:`);
            
            // Build detailed error message with URLs
            const invalidUrls = requestsWithInvalidTenant.map((req, index) => 
                `\n  ${index + 1}. ${req.method} ${req.url}\n      Issue: ${req.issue}`
            ).join('');
            
            requestsWithInvalidTenant.forEach((req, index) => {
                cy.log(`${index + 1}. ${req.method} ${req.url} - ${req.issue}`);
                console.log(`Request ${index + 1}:`, req);
            });
            
            // Fail with descriptive message showing which URLs
            throw new Error(`${requestsWithInvalidTenant.length} requests have invalid tenant:${invalidUrls}`);
        }
        
        cy.log(`✅ All ${capturedRequests.length} declaration requests have valid tenant parameters`);
    });
});

And("all tenant parameters should have value {int}", (expectedTenantValue: number) => {
    cy.log(`🔍 Validating tenant value is ${expectedTenantValue}`);
    
    cy.wrap(null).then(() => {
        const requestsWithWrongTenant: any[] = [];
        
        capturedRequests.forEach((req) => {
            let tenantValue: number | null = null;
            
            // Extract tenant from URL
            const urlMatch = req.url.match(/[&?]tenant=(\d+)/i);
            if (urlMatch) {
                tenantValue = parseInt(urlMatch[1]);
            }
            
            // Extract from body
            if (req.body && (req.body.tenant || req.body.Tenant)) {
                tenantValue = req.body.tenant || req.body.Tenant;
            }
            
            if (tenantValue !== null && tenantValue !== expectedTenantValue) {
                requestsWithWrongTenant.push({ ...req, foundTenant: tenantValue });
            }
        });
        
        if (requestsWithWrongTenant.length > 0) {
            cy.log(`❌ Found ${requestsWithWrongTenant.length} requests with wrong tenant:`);
            requestsWithWrongTenant.forEach((req, index) => {
                cy.log(`${index + 1}. Expected: ${expectedTenantValue}, Got: ${req.foundTenant} - ${req.url}`);
            });
        }
        
        expect(requestsWithWrongTenant.length,
            `All requests should have tenant = ${expectedTenantValue}`
        ).to.equal(0);
        
        cy.log(`✅ All requests have correct tenant value: ${expectedTenantValue}`);
    });
});

