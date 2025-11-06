/**
 * Import Action Actions
 * Handles actions for opening Declaration Form from Import Declarations workspace
 */

import { BaseExportSelectors } from '../selectors/BaseExportSelectors';
import { ImportActionSelectors } from '../selectors/ImportActionSelectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { NavigateToImportDeclarationsWorkspace as NavigateToImportDeclarationsWorkspaceFromSearch } from './SearchActions';

/**
 * Navigate to Import Declarations workspace
 * Reuses existing navigation function
 */
export function NavigateToImportDeclarationsWorkspace() {
    NavigateToImportDeclarationsWorkspaceFromSearch();
}

/**
 * Select the first result from the grid
 */
export function SelectFirstResult() {
    cy.log('=== Selecting first result from grid ===');
    
    // Wait for grid to load and have at least one row
    cy.get(ImportActionSelectors.GridRows, { timeout: 15000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    // Click on the first row
    cy.get(ImportActionSelectors.FirstRow, { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    
    cy.wait(1000); // Wait for row selection to register
    cy.log('✓ First result selected');
}

/**
 * Click on "Forms" dropdown button (top left) - "טפסים"
 */
export function ClickFormsMenu() {
    cy.log('=== Clicking Forms dropdown (טפסים) ===');
    
    // Click the Forms dropdown button at top left
    cy.get(ImportActionSelectors.FormsDropdown, { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    
    // Wait for dropdown menu to appear - increased timeout to handle slower rendering
    // Check for QueryLink elements or DeclarationForm menu item with longer timeout
    cy.wait(1500); // Initial wait for dropdown to start opening
    
    // Try QueryLink first, if not found, try DeclarationForm directly
    cy.get(BaseSelectors.QueryLink + ', ' + ImportActionSelectors.DeclarationForm, { timeout: 20000 })
        .should('be.visible')
        .first();
    
    cy.wait(500); // Additional small wait for menu to fully render
    cy.log('✓ Forms dropdown opened');
}

/**
 * Click on "Declaration Form" menu item - "טופס הצהרה"
 * Note: Specifically targets "טופס הצהרה" and NOT "בקשה לטופס הצהרה"
 */
export function ClickDeclarationForm() {
    cy.log('=== Clicking Declaration Form (טופס הצהרה) ===');
    
    // Wait for the menu item to be visible in the dropdown
    // Find elements containing "טופס הצהרה" and filter to exclude "בקשה"
    cy.get(ImportActionSelectors.DeclarationForm, { timeout: 10000 })
        .should('be.visible')
        .contains('טופס הצהרה')
        .then(($elements) => {
            // Filter to find the one that contains "טופס הצהרה" but NOT "בקשה"
            const exactMatch = Array.from($elements).find(el => {
                const text = Cypress.$(el).text().trim();
                return text.includes('טופס הצהרה') && !text.includes('בקשה');
            });
            if (exactMatch) {
                cy.wrap(exactMatch).click({ force: true });
            } else {
                // If filtering didn't work, just click the first match
                cy.wrap($elements.first()).click({ force: true });
            }
        });
    
    cy.log('✓ Declaration Form clicked');
}

/**
 * Wait for popup dialog to appear
 */
export function WaitForPopup() {
    cy.log('=== Waiting for popup dialog ===');
    
    cy.get(ImportActionSelectors.PopupDialog, { timeout: 15000 })
        .should('be.visible');
    
    cy.log('✓ Popup dialog appeared');
}

/**
 * Click OK/Confirm button in popup
 */
export function ClickPopupConfirm() {
    cy.log('=== Clicking OK/Confirm in popup ===');
    
    cy.get(ImportActionSelectors.PopupConfirmButton, { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    
    cy.wait(1000); // Wait for popup to close
    cy.log('✓ Popup confirmed');
}

/**
 * Verify that the action completed successfully
 */
export function VerifyActionSuccess() {
    cy.log('=== Verifying action success ===');
    
    // Verify popup is closed (should not exist or not be visible)
    cy.get('body').then(($body) => {
        if ($body.find(ImportActionSelectors.PopupDialog).length > 0) {
            cy.get(ImportActionSelectors.PopupDialog).should('not.be.visible');
        } else {
            // Popup doesn't exist, which is also success
            cy.log('✓ Popup closed successfully');
        }
    });
    
    // Verify we're still on the Import Declarations workspace
    cy.get(ImportActionSelectors.GridRows, { timeout: 10000 })
        .should('exist');
    
    cy.log('✓ Action completed successfully');
}

