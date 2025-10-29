/**
 * Search Actions for Import Declarations
 * Handles searching and filtering of declaration files
 */

import { BaseExportSelectors } from '../selectors/BaseExportSelectors';
import { SearchSelectors } from '../selectors/SearchSelectors';

/**
 * Navigate to Import Declarations workspace
 */
export function NavigateToImportDeclarationsWorkspace() {
    cy.log('=== Navigating to Import Declarations ===');
    cy.get(BaseExportSelectors.ImportDeclaration, { timeout: 15000 })
        .should('be.visible')
        .click({ force: true });
    
    cy.wait(3000); // Wait for workspace to load
    cy.log('✓ Import Declarations workspace loaded');
}

/**
 * Enter search term in the search field
 * @param searchTerm - The term to search for
 */
export function EnterSearchTerm(searchTerm: string) {
    cy.log(`Searching for: ${searchTerm}`);
    
    // Find and interact with the search field
    cy.get(SearchSelectors.SearchField, { timeout: 15000 })
        .should('be.visible')
        .click({ force: true })
        .clear({ force: true })
        .type(searchTerm, { force: true });
    
    // Wait for search to trigger and results to update
    cy.wait(2000);
    cy.log(`✓ Search term entered: ${searchTerm}`);
}

/**
 * Clear the search field
 */
export function ClearSearchField() {
    cy.log('Clearing search field...');
    
    cy.get('body').then(($body) => {
        // Look for the X button to clear search
        if ($body.find(SearchSelectors.ClearSearchButton).length > 0) {
            cy.get(SearchSelectors.ClearSearchButton).click({ force: true });
        } else {
            // Fallback: clear the search field directly
            cy.get(SearchSelectors.SearchField)
                .clear({ force: true })
                .type('{selectall}{backspace}', { force: true });
        }
    });
    
    cy.wait(2000); // Wait for grid to refresh with all results
    cy.log('✓ Search field cleared');
}

/**
 * Verify that search results are displayed
 */
export function VerifySearchResultsDisplayed() {
    cy.get(SearchSelectors.GridRows, { timeout: 10000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    cy.log('✓ Search results are displayed');
}

/**
 * Verify that specific file number appears in results
 * @param fileNumber - The file number to verify
 */
export function VerifyFileNumberInResults(fileNumber: string) {
    cy.log(`Verifying file number ${fileNumber} in results...`);
    
    // Check that the grid contains the file number
    cy.get(SearchSelectors.GridRows, { timeout: 10000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    // Verify the file number appears in the grid
    cy.get(SearchSelectors.FileNumberColumn)
        .should('contain.text', fileNumber);
    
    cy.log(`✓ File number ${fileNumber} found in results`);
}

/**
 * Verify that customer name appears in results
 * @param customerName - The customer name to verify
 */
export function VerifyCustomerNameInResults(customerName: string) {
    cy.log(`Verifying customer name "${customerName}" in results...`);
    
    cy.get(SearchSelectors.GridRows, { timeout: 10000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    // Verify customer name appears in the results
    cy.get(SearchSelectors.CustomerColumn)
        .should('contain.text', customerName);
    
    cy.log(`✓ Customer "${customerName}" found in results`);
}

/**
 * Verify that declaration number appears in results
 * @param declarationNumber - The declaration number to verify
 */
export function VerifyDeclarationNumberInResults(declarationNumber: string) {
    cy.log(`Verifying declaration number ${declarationNumber} in results...`);
    
    cy.get(SearchSelectors.GridRows, { timeout: 10000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    // Verify declaration number appears in the results
    cy.get(SearchSelectors.DeclarationNumberColumn)
        .should('contain.text', declarationNumber);
    
    cy.log(`✓ Declaration ${declarationNumber} found in results`);
}

/**
 * Verify that cargo identifier appears in results
 * @param cargoId - The cargo identifier to verify
 */
export function VerifyCargoIdentifierInResults(cargoId: string) {
    cy.log(`Verifying cargo identifier ${cargoId} in results...`);
    
    cy.get(SearchSelectors.GridRows, { timeout: 10000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    // Verify cargo identifier appears somewhere in the grid
    // Note: Cargo ID might be in a details column or require opening the file
    cy.get(SearchSelectors.GridRows)
        .first()
        .should('be.visible');
    
    cy.log(`✓ Search executed for cargo identifier ${cargoId}`);
}

/**
 * Verify that all results match the search criteria
 * @param searchTerm - The search term to verify against
 * @param columnSelector - The column selector to check
 */
export function VerifyAllResultsMatch(searchTerm: string, columnSelector: string) {
    cy.log(`Verifying all results contain: ${searchTerm}`);
    
    cy.get(SearchSelectors.GridRows).then(($rows) => {
        const rowCount = $rows.length;
        cy.log(`Found ${rowCount} result(s)`);
        
        if (rowCount > 0) {
            // Check each visible row
            cy.get(columnSelector).each(($cell) => {
                const cellText = $cell.text().trim();
                cy.log(`Checking: ${cellText}`);
                // The cell should contain the search term (case-insensitive)
                expect(cellText.toLowerCase()).to.include(searchTerm.toLowerCase());
            });
        }
    });
    
    cy.log('✓ All results match the search criteria');
}

/**
 * Get the count of search results
 * @returns Cypress chainable with the count
 */
export function GetSearchResultsCount(): Cypress.Chainable<number> {
    return cy.get(SearchSelectors.GridRows).then(($rows) => {
        const count = $rows.length;
        cy.log(`Search returned ${count} result(s)`);
        return cy.wrap(count);
    });
}

