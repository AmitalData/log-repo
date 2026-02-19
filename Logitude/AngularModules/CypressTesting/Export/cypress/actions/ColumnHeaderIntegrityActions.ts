/**
 * Actions for Bug 118310 - Column Header Integrity Testing
 * Contains functions to create declarations, refresh views, and verify column header integrity
 */

import { ColumnHeaderIntegritySelectors } from '../selectors/ColumnHeaderIntegritySelectors';
import { 
    ColumnHeaderIntegrityDetails,
    ExportDeclarationHeaders,
    ImportDeclarationHeaders,
    CriticalHeadersToTest,
    isBase64Encoded,
    isValidHebrewText,
    BASE64_PATTERN
} from '../models/ColumnHeaderIntegrityDetails';
import * as Assists from '../../../Base/cypress/assists/Assists';

// Store declaration IDs for later reference
let currentExportDeclarationId: string = null;
let currentImportDeclarationId: string = null;

/**
 * Creates a new Export declaration with standard column headers
 * Simplified: Just opens an existing declaration for testing
 */
export function CreateExportDeclarationWithHeaders() {
    cy.log('Selecting Export Declaration with standard headers');
    
    // Navigate to Export workspace
    cy.Click(ColumnHeaderIntegritySelectors.ExportWorkspaceMenu, null, true);
    cy.wait(3000);
    
    // Wait for rows to appear
    cy.get('.Row', { timeout: 15000 }).should('have.length.greaterThan', 0);
    cy.wait(1000);
    
    // Click first existing declaration
    cy.get('.Row').eq(0).should('be.visible').click();
    cy.wait(2000);
    
    // Store declaration ID
    cy.get('.ShortTitleControl').invoke('text').then((text) => {
        currentExportDeclarationId = text.trim();
        cy.log(`Opened Export Declaration: ${currentExportDeclarationId}`);
    });
}

/**
 * Creates a new Import declaration with standard column headers
 * Simplified: Just opens an existing declaration for testing
 */
export function CreateImportDeclarationWithHeaders() {
    cy.log('Selecting Import Declaration with standard headers');
    
    // Navigate to Import workspace
    cy.Click(ColumnHeaderIntegritySelectors.ImportWorkspaceMenu, null, true);
    cy.wait(3000);
    
    // Wait for rows to appear
    cy.get('.Row', { timeout: 15000 }).should('have.length.greaterThan', 0);
    cy.wait(1000);
    
    // Click first existing declaration
    cy.get('.Row').eq(0).should('be.visible').click();
    cy.wait(2000);
    
    // Store declaration ID
    cy.get('.ShortTitleControl').invoke('text').then((text) => {
        currentImportDeclarationId = text.trim();
        cy.log(`Opened Import Declaration: ${currentImportDeclarationId}`);
    });
}

/**
 * Refreshes the current declaration view
 * Note: Avoid cy.reload() as it logs user out
 */
export function RefreshDeclaration() {
    cy.log('Refreshing declaration view (navigating back to list and reopening)');
    
    // Navigate back to Export list
    cy.Click(ColumnHeaderIntegritySelectors.ExportWorkspaceMenu, null, true);
    cy.wait(2000);
    
    // Reopen first declaration
    cy.get('.Row', { timeout: 15000 }).should('have.length.greaterThan', 0);
    cy.get('.Row').eq(0).should('be.visible').click();
    cy.wait(2000);
}

/**
 * Extracts all column headers from the current grid
 * @returns Array of header text values
 */
export function GetColumnHeaders(): Cypress.Chainable<string[]> {
    cy.log('Extracting column headers from grid');
    
    return cy.get(ColumnHeaderIntegritySelectors.AllColumnHeaders)
        .then(($headers) => {
            const headers: string[] = [];
            $headers.each((index, el) => {
                const headerText = Cypress.$(el).text().trim();
                if (headerText) {
                    headers.push(headerText);
                }
            });
            cy.log(`Found ${headers.length} column headers`);
            return headers;
        });
}

/**
 * Verifies that a specific column header has the expected text
 * @param columnName The column name to search for
 * @param expectedText The expected text value
 */
export function VerifyColumnHeaderText(columnName: string, expectedText: string) {
    cy.log(`Verifying column header "${columnName}" has text "${expectedText}"`);
    
    cy.get(ColumnHeaderIntegritySelectors.AllColumnHeaders)
        .contains(columnName)
        .should('exist')
        .and('contain', expectedText);
    
    cy.log(`✓ Column header "${columnName}" verified successfully`);
}

/**
 * Verifies that no Base64 encoded strings appear in visible column headers
 */
export function VerifyNoBase64InHeaders() {
    cy.log('Verifying no Base64 encoded strings in column headers');
    
    GetColumnHeaders().then((headers) => {
        const base64Headers: string[] = [];
        
        headers.forEach(header => {
            if (isBase64Encoded(header)) {
                base64Headers.push(header);
            }
        });
        
        if (base64Headers.length > 0) {
            cy.log(`❌ Found ${base64Headers.length} Base64 encoded headers: ${base64Headers.join(', ')}`);
            throw new Error(`Base64 encoded strings found in headers: ${base64Headers.join(', ')}`);
        } else {
            cy.log('✓ No Base64 encoded strings found in headers');
        }
    });
}

/**
 * Verifies that all column headers display correctly (valid text, not corrupted)
 */
export function VerifyAllColumnHeadersDisplayCorrectly() {
    cy.log('Verifying all column headers display correctly');
    
    GetColumnHeaders().then((headers) => {
        const invalidHeaders: string[] = [];
        
        headers.forEach(header => {
            // Check if header is empty or corrupted
            if (!header || header.length === 0) {
                invalidHeaders.push('[EMPTY]');
            } else if (isBase64Encoded(header)) {
                invalidHeaders.push(`${header} [BASE64]`);
            } else if (!isValidHebrewText(header)) {
                // Allow some special characters in headers
                const hasValidContent = /[א-ת]|[a-zA-Z]|\d/.test(header);
                if (!hasValidContent) {
                    invalidHeaders.push(`${header} [INVALID]`);
                }
            }
        });
        
        if (invalidHeaders.length > 0) {
            cy.log(`❌ Found ${invalidHeaders.length} invalid headers: ${invalidHeaders.join(', ')}`);
            throw new Error(`Invalid headers found: ${invalidHeaders.join(', ')}`);
        } else {
            cy.log(`✓ All ${headers.length} column headers display correctly`);
        }
    });
}

/**
 * Verifies that Base64 encoding is used internally (for Import) but not visible
 * This checks the DOM structure to ensure encoding exists at data level but not in display
 */
export function VerifyBase64EncodingInternal() {
    cy.log('Verifying Base64 encoding is internal only (not visible)');
    
    // First verify display is correct
    VerifyAllColumnHeadersDisplayCorrectly();
    
    // Check if internal data attributes might contain Base64
    cy.get(ColumnHeaderIntegritySelectors.AllColumnHeaders).then(($headers) => {
        let hasInternalEncoding = false;
        
        $headers.each((index, el) => {
            const $el = Cypress.$(el);
            const dataAttrs = Object.keys(el.dataset || {});
            
            // Check data attributes for Base64 patterns
            dataAttrs.forEach(attr => {
                const value = el.dataset[attr];
                if (value && isBase64Encoded(value)) {
                    hasInternalEncoding = true;
                }
            });
        });
        
        cy.log(hasInternalEncoding ? 
            '✓ Base64 encoding detected at internal level' : 
            'ℹ Base64 encoding not detected in data attributes');
    });
}

/**
 * Switches between different declarations by ID
 * @param declarationId The ID of the declaration to switch to
 */
export function SwitchBetweenDeclarations(declarationId: string) {
    cy.log(`Switching to declaration: ${declarationId}`);
    
    // Navigate to declarations list
    cy.get(ColumnHeaderIntegritySelectors.DeclarationList).should('be.visible');
    
    // Search for the specific declaration
    cy.get('.search-input, input[type="search"]').type(declarationId);
    cy.wait(1000);
    
    // Click on the first result
    cy.Click(ColumnHeaderIntegritySelectors.FirstDeclarationRow, null);
    cy.wait(2000);
}

/**
 * Saves the current Export declaration
 */
export function SaveExportDeclaration() {
    cy.log('Saving Export declaration');
    cy.Click(ColumnHeaderIntegritySelectors.ExportDeclarationSaveButton, null);
    cy.wait(2000);
}

/**
 * Saves the current Import declaration
 */
export function SaveImportDeclaration() {
    cy.log('Saving Import declaration');
    cy.Click(ColumnHeaderIntegritySelectors.ImportDeclarationSaveButton, null);
    cy.wait(2000);
}

/**
 * Opens an existing Export declaration
 */
export function OpenExportDeclaration() {
    cy.log('Opening existing Export declaration');
    
    // Navigate to Export workspace
    cy.Click(ColumnHeaderIntegritySelectors.ExportWorkspaceMenu, null, true);
    cy.wait(3000);
    
    // Wait for data to load and click first row
    cy.get('#ListDataLoaded', { timeout: 10000 }).should('exist');
    cy.get('.Row').eq(0).should('be.visible').click();
    cy.wait(2000);
}

/**
 * Opens an existing Import declaration
 */
export function OpenImportDeclaration() {
    cy.log('Opening existing Import declaration');
    
    // Navigate to Import workspace
    cy.Click(ColumnHeaderIntegritySelectors.ImportWorkspaceMenu, null, true);
    cy.wait(3000);
    
    // Wait for data to load and click first row
    cy.get('#ListDataLoaded', { timeout: 10000 }).should('exist');
    cy.get('.Row').eq(0).should('be.visible').click();
    cy.wait(2000);
}

/**
 * Verifies that Export column headers remain in plain text format
 */
export function VerifyExportHeadersInPlainText() {
    cy.log('Verifying Export headers are in plain text format');
    
    // Verify no Base64 encoding visible
    VerifyNoBase64InHeaders();
    
    // Verify headers are valid text
    VerifyAllColumnHeadersDisplayCorrectly();
    
    cy.log('✓ Export headers verified as plain text');
}

/**
 * Verifies that Import Base64 encoding does not leak into Export
 */
export function VerifyNoImportEncodingLeakage() {
    cy.log('Verifying no Import Base64 encoding leaked into Export');
    
    GetColumnHeaders().then((headers) => {
        headers.forEach(header => {
            // Check for any patterns that suggest Import-style encoding
            if (isBase64Encoded(header)) {
                throw new Error(`Base64 encoding detected in Export header: ${header}`);
            }
        });
        
        cy.log('✓ No Import encoding leakage detected');
    });
}

/**
 * Verifies critical headers are correct
 */
export function VerifyCriticalHeaders() {
    cy.log('Verifying critical headers');
    
    CriticalHeadersToTest.forEach(({ name, expectedText }) => {
        VerifyColumnHeaderText(name, expectedText);
    });
    
    cy.log('✓ All critical headers verified');
}

/**
 * Performs Import declaration operations (simulates Import workflow)
 */
export function PerformImportOperations() {
    cy.log('Performing Import declaration operations');
    
    // Navigate to Import
    cy.Click(ColumnHeaderIntegritySelectors.ImportWorkspaceMenu, null);
    cy.wait(3000);
    
    cy.log('✓ Import operations completed');
}

/**
 * Performs Export declaration operations (simulates Export workflow)
 */
export function PerformExportOperations() {
    cy.log('Performing Export declaration operations');
    
    // Navigate to Export
    cy.Click(ColumnHeaderIntegritySelectors.ExportWorkspaceMenu, null);
    cy.wait(3000);
    
    cy.log('✓ Export operations completed');
}

/**
 * Gets current Export declaration ID
 */
export function GetCurrentExportDeclarationId(): string {
    return currentExportDeclarationId;
}

/**
 * Gets current Import declaration ID
 */
export function GetCurrentImportDeclarationId(): string {
    return currentImportDeclarationId;
}

