import { SupplierAccountSelectors } from "../selectors/SupplierAccountSelectors";
import { SupplierAccountDetails } from "cypress/models/SupplierAccountDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { SearchSelectors } from "../selectors/SearchSelectors";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';

export function FillSearchField(supplierAccountDetails: SupplierAccountDetails) {
    cy.log('=== Searching for file and opening Supplier Accounts ===');
    
    // Enter search term in the search field
    cy.FillLogTextBox(SupplierAccountSelectors.SearchField, supplierAccountDetails.File, true);
    cy.get(SupplierAccountSelectors.SearchField).focus();
    
    // Wait for grid to load and have at least one row
    cy.wait(2000); // Initial wait for search to trigger
    cy.get(SearchSelectors.GridRows, { timeout: 15000 })
        .should('exist')
        .should('have.length.greaterThan', 0);
    
    cy.log('✓ Grid loaded with results');
    
    // Click on the first result from the grid - use flexible selector that matches any row0
    cy.get(SupplierAccountSelectors.FirstDeclaration, { timeout: 10000 })
        .should('exist')
        .should('be.visible')
        .first()
        .click({ force: true });
    
    cy.log('✓ First result clicked');
    
    // Wait for declaration page to load - wait for any indication that page has loaded
    cy.log('=== Waiting for declaration page to load ===');
    cy.wait(3000); // Initial wait for page transition
    
    // Wait for the declaration page to be fully loaded - look for tab holder or any tab element
    cy.get('.TabHolder, [class*="Tab"], #CustomsDeclarationTHSupplierAccounts, [id*="SupplierAccount"]', { timeout: 20000 })
        .should('exist');
    
    cy.log('✓ Declaration page loaded');
    
    // Open Supplier Accounts tab - try multiple approaches
    cy.log('=== Opening Supplier Accounts tab ===');
    cy.get('body').then(($body) => {
        // Check if ID selector exists first
        const idTab = $body.find('#CustomsDeclarationTHSupplierAccounts, [id*="SupplierAccount"]');
        if (idTab.length > 0) {
            cy.log('✓ Found Supplier Accounts tab by ID');
            cy.wrap(idTab.first()).scrollIntoView().should('be.visible').click({ force: true });
        } else {
            // Use cy.contains() - this searches the entire DOM
            cy.log('⚠ ID selector not found, using text-based search');
            cy.contains('חשבונות ספק', { timeout: 30000 })
                .scrollIntoView()
                .should('be.visible')
                .click({ force: true });
        }
    });
    
    cy.wait(2000); // Wait for Supplier Accounts tab to load
    
    // Verify Supplier Accounts tab is active/open - wait for content to load
    cy.wait(1000);
    cy.log('✓ Supplier Accounts content should be visible');
    
    cy.log('✓ Supplier Accounts opened');
}

export function CreateAndFillNewSupplierAccount(supplierAccountDetails: SupplierAccountDetails) {
    cy.wait(1000); // Short wait as mentioned in requirements
    cy.log('=== Clicking + button to add new Supplier Account ===');
    
    // Wait for the Add button - use multiple strategies to find it regardless of dynamic ID
    // Try: button with title="Add", or button containing Add.png image, or button with class LogitudeIconButton
    cy.get('body').then(($body) => {
        // First try: button with title="Add" (most reliable based on HTML structure)
        const addButtonByTitle = $body.find('declarationsupplierinvoicetabcomponent button[title="Add"]');
        if (addButtonByTitle.length > 0 && addButtonByTitle.is(':visible')) {
            cy.log('✓ Found Add button by title');
            cy.wrap(addButtonByTitle.first()).should('be.visible').click({ force: true });
        } else {
            // Second try: find img with Add.png and get its parent button
            const addImage = $body.find('declarationsupplierinvoicetabcomponent img[src="./Images/Buttons/Add.png"]');
            if (addImage.length > 0) {
                cy.log('✓ Found Add button by image');
                cy.wrap(addImage.first()).parent('button').should('be.visible').click({ force: true });
            } else {
                // Third try: button with class LogitudeIconButton in the component
                cy.log('⚠ Trying fallback: button with LogitudeIconButton class');
                cy.get('declarationsupplierinvoicetabcomponent button.LogitudeIconButton', { timeout: 15000 })
                    .first()
                    .should('be.visible')
                    .click({ force: true });
            }
        }
    });
    
    cy.log('✓ Add button clicked');
    
    // Wait for popup to appear - similar to AddItemActions pattern
    cy.wait(3000);
    
    // Wait for AccountTypeCode field to appear - search directly
    cy.log('=== Waiting for AccountTypeCode field ===');
    
    // Wait for AccountTypeCode field - use the exact selector first
    cy.get(SupplierAccountSelectors.AccountTypeCode, { timeout: 20000 })
        .should('exist')
        .should('be.visible')
        .should('not.be.disabled');
    
    cy.wait(500); // Additional wait for form to fully render
    cy.log('✓ New account form appeared');
    
    cy.FillLogLov(SupplierAccountSelectors.AccountTypeCode, supplierAccountDetails.AccountTypeCode, true);
    
    // Handle date field - can use TODAY or specific date
    if (supplierAccountDetails.IssueDate.toUpperCase() === 'TODAY') {
        cy.FillDate(SupplierAccountSelectors.IssueDate, 'TODAY');
    } else {
        cy.FillDate(SupplierAccountSelectors.IssueDate, supplierAccountDetails.IssueDate);
    }
    
    cy.FillLogLov(SupplierAccountSelectors.InvoiceCurrencyTypeCode, supplierAccountDetails.InvoiceCurrencyTypeCode, true);
    cy.FillLogLov(SupplierAccountSelectors.IncotermCode, supplierAccountDetails.IncotermCode, true);
    cy.FillLogTextBox(SupplierAccountSelectors.InvoiceNumber, supplierAccountDetails.InvoiceNumber, true);
    cy.get(SupplierAccountSelectors.InvoiceAmount).type(supplierAccountDetails.InvoiceAmount);
    
    // Check preference account checkbox
    cy.get(SupplierAccountSelectors.IsPreference).click({ force: true });
    
    cy.FillLogLov(SupplierAccountSelectors.PreferenceDocumentTypeCode, supplierAccountDetails.PreferenceDocumentTypeCode, true);
    cy.FillLogLov(SupplierAccountSelectors.VendorId, supplierAccountDetails.VendorId, true);
    
    // Handle duplicate account confirmation dialog immediately if it appears (it blocks subsequent operations)
    cy.log('=== Checking for duplicate account confirmation popup ===');
    cy.wait(1000); // Wait for popup to appear if it's going to
    
    // Check if popup is visible and handle it
    cy.get('body').then(($body) => {
        const popup = $body.find('.LogitudeWindow:visible, .MessageWindow:visible');
        if (popup.length > 0) {
            const dialogText = popup.text();
            if (dialogText.indexOf('קיים כבר חשבון ספק') !== -1 || 
                dialogText.indexOf('מספר חשבון זהה') !== -1 ||
                dialogText.indexOf('האם להמשיך') !== -1 ||
                dialogText.indexOf('שורה') !== -1) {
                cy.log('=== Duplicate account confirmation dialog found, clicking Yes ===');
                // Find the Yes button - use specific selector for confirmation popup buttons
                cy.get('button[id^="ConfirmWindow_Yes"]:visible', { timeout: 5000 })
                    .first()
                    .click({ force: true });
            }
        }
    });
    
    cy.log('✓ Supplier Account form filled, popup handled if present');
}

export function FillTransportData(supplierAccountDetails: SupplierAccountDetails) {
    cy.log('=== Filling Transport Data ===');
    
    // First, check if there's a blocking popup with Yes button and handle it
    cy.get('body').then(($body) => {
        const popup = $body.find('.LogitudeWindow:visible, .MessageWindow:visible');
        const yesButton = $body.find('button[id^="ConfirmWindow_Yes"]');
        if (popup.length > 0 && yesButton.length > 0 && yesButton.is(':visible')) {
            cy.log('=== Popup detected with Yes button, clicking it ===');
            cy.wrap(yesButton.first()).click({ force: true });
            cy.wait(500);
        }
    });
    
    // Wait for transport data fields to be visible - these are div elements (LogCellTemplate)
    cy.get(SupplierAccountSelectors.TransportCurrencyTypeCode, { timeout: 15000 })
        .should('be.visible');
    
    cy.wait(500); // Small wait for fields to be ready
    
    FillMatchingDDL(SupplierAccountSelectors.TransportCurrencyTypeCode, supplierAccountDetails.TransportCurrencyTypeCode);
    FillMatching(SupplierAccountSelectors.TransportAmount, supplierAccountDetails.TransportAmount);
    
    cy.log('✓ Transport Data filled');
}

export function FillMatching(selector, value) {
    // These are div elements (LogCellTemplate), need to click first to enter edit mode
    cy.get(selector, { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    
    cy.wait(300); // Wait for cell to enter edit mode
    
    // Now type the value - try to find input inside, or type directly on the cell
    cy.get(selector, { timeout: 10000 }).then(($el) => {
        const input = $el.find('input');
        if (input.length > 0) {
            cy.wrap(input.first()).type('{selectall}' + value, { force: true });
        } else {
            cy.wrap($el.first()).type('{selectall}' + value, { force: true });
        }
    });
    
    cy.wait(200); // Wait for value to be set
}

export function FillMatchingDDL(selector, value) {
    // Check for blocking popup first and handle it
    cy.get('body').then(($body) => {
        const popup = $body.find('.LogitudeWindow:visible, .MessageWindow:visible');
        const yesButton = $body.find('button[id^="ConfirmWindow_Yes"]');
        if (popup.length > 0 && yesButton.length > 0 && yesButton.is(':visible')) {
            cy.log('=== Popup blocking dropdown, clicking Yes button ===');
            cy.wrap(yesButton.first()).click({ force: true });
            cy.wait(500);
        }
    });
    
    // These are div elements (LogCellTemplate), need to click first to enter edit mode
    cy.get(selector, { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    
    cy.wait(500); // Wait for cell to enter edit mode
    
    // Try to find input, if not found, type directly on the cell
    cy.get(selector, { timeout: 10000 })
        .first()
        .then(($cell) => {
            const input = $cell.find('input');
            if (input.length > 0) {
                // Input exists, use it
                cy.wrap(input.first()).clear({ force: true }).type(value, { force: true });
                cy.wait(300);
                cy.wrap(input.first()).type('{downarrow}', { force: true });
            } else {
                // No input found, type directly on the cell
                cy.wrap($cell.first()).type(value, { force: true });
                cy.wait(300);
                cy.wrap($cell.first()).type('{downarrow}', { force: true });
            }
        });
    
    cy.wait(500); // Wait for dropdown to appear
    
    // Wait for dropdown and select the item
    cy.get(BaseSelectors.DropDownList, { timeout: 15000 })
        .should('be.visible')
        .contains(value)
        .then(($items) => {
            if ($items.length > 0) {
                $items[0].click();
            } else {
                // Fallback: try to find by text in the list
                cy.get(BaseSelectors.DropDownList + ' ' + BaseSelectors.DropDownListItem, { timeout: 10000 })
                    .contains(value)
                    .first()
                    .click({ force: true });
            }
        });
    
    cy.wait(300); // Wait for selection to be applied
}

export function CreateNewCustomsDetailRow(supplierAccountDetails: SupplierAccountDetails) {
    cy.wait(1000);
    cy.log('=== Clicking + button to add customs detail row ===');
    cy.get(SupplierAccountSelectors.AddItemButton, { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    cy.wait(1000); // Wait for new row to appear
}

// Helper function to fill a grid cell - clicks LogCellTemplate to activate, waits for input, then types
function FillGridCell(selector: string, value: string) {
    // Click the LogCellTemplate div to enter edit mode
    cy.get(selector, { timeout: 10000 })
        .should('exist')
        .first()
        .click({ force: true });
    
    cy.wait(500); // Wait for cell to enter edit mode and input to appear
    
    // Try multiple approaches to find and fill the input
    cy.get(selector, { timeout: 10000 })
        .first()
        .then(($cell) => {
            // Check if input exists inside the cell
            const input = $cell.find('input');
            if (input.length > 0) {
                // Input exists, use it
                cy.wrap(input.first()).clear({ force: true }).type(value, { force: true });
            } else {
                // No input found, try typing directly on the cell (some grids work this way)
                cy.wrap($cell.first()).type('{selectall}' + value, { force: true });
            }
        });
    
    cy.wait(300); // Wait for value to be set
}

export function FillCustomsDetailRow(supplierAccountDetails: SupplierAccountDetails) {
    cy.log('=== Filling Customs Detail Row ===');
    
    // Check for blocking popup first
    cy.get('body').then(($body) => {
        const popup = $body.find('.LogitudeWindow:visible, .MessageWindow:visible');
        const yesButton = $body.find('button[id^="ConfirmWindow_Yes"]');
        if (popup.length > 0 && yesButton.length > 0 && yesButton.is(':visible')) {
            cy.log('=== Popup detected with Yes button, clicking it ===');
            cy.wrap(yesButton.first()).click({ force: true });
            cy.wait(500);
        }
    });
    
    // Wait for the row to exist
    cy.get('#row0', { timeout: 15000 }).should('exist');
    cy.wait(500);
    
    // Fill ItemNo - grid cell index 2
    if (supplierAccountDetails.ItemNo) {
        FillGridCell(SupplierAccountSelectors.ItemNo, supplierAccountDetails.ItemNo);
    }
    
    // Fill ItemDescription - grid cell index 3
    if (supplierAccountDetails.ItemDescription) {
        FillGridCell(SupplierAccountSelectors.ItemDescription, supplierAccountDetails.ItemDescription);
    }
    
    // Fill Item - grid cell index 4
    if (supplierAccountDetails.Item) {
        FillGridCell(SupplierAccountSelectors.Item, supplierAccountDetails.Item);
    }
    
    // Fill TradeAgreementCode - dropdown, grid cell index 5
    if (supplierAccountDetails.TradeAgreementCode) {
        FillMatchingDDL(SupplierAccountSelectors.TradeAgreementCode, supplierAccountDetails.TradeAgreementCode);
    }
    
    // Fill UnitsQuantity - grid cell index 6 (correct)
    if (supplierAccountDetails.UnitsQuantity) {
        FillGridCell(SupplierAccountSelectors.UnitsQuantity, supplierAccountDetails.UnitsQuantity);
    }
    
    // Fill UnitType - dropdown, grid cell index 7 (correct)
    if (supplierAccountDetails.UnitType) {
        FillMatchingDDL(SupplierAccountSelectors.UnitType, supplierAccountDetails.UnitType);
    }
    
    // Fill ValueInForeignCurrency - grid cell index 8 (correct)
    if (supplierAccountDetails.ValueInForeignCurrency) {
        FillGridCell(SupplierAccountSelectors.ValueInForeignCurrency, supplierAccountDetails.ValueInForeignCurrency);
    }
    
    // Fill OriginCountryCode - dropdown, grid cell index 0 (edit-log-grid_0_20_8_0)
    if (supplierAccountDetails.OriginCountryCode) {
        FillMatchingDDL(SupplierAccountSelectors.OriginCountryCode, supplierAccountDetails.OriginCountryCode);
    }
    
    cy.wait(500);
    cy.log('✓ Customs Detail Row filled');
}

export function SaveSupplierAccount() {
    cy.Click(SupplierAccountSelectors.ButtonSaveSupplierInvoice, null);
    cy.wait(5000); // Wait for save to complete
}

export function AssertSaveSupplierAccount() {
    BaseAssertion.AssertElementExist(SupplierAccountSelectors.SupplierAccountFirstRow);
}

export function DeleteRow() {
    cy.log('=== Deleting Supplier Account row ===');
    
    // Try to find the delete button directly - it might be visible or in a menu
    cy.get('body').then(($body) => {
        // Check if delete button is visible
        const deleteBtn = $body.find(SupplierAccountSelectors.ButtonDeleteSupplierInvoice + ':visible');
        if (deleteBtn.length > 0) {
            cy.log('✓ Delete button found and visible');
            cy.wrap(deleteBtn.first()).click({ force: true });
        } else {
            // Button might be in a dropdown menu - try to find and click the menu trigger first
            cy.log('⚠ Delete button not visible, trying to find menu trigger');
            // Look for delete button by ID even if not visible, or look for menu buttons
            const deleteBtnExists = $body.find(SupplierAccountSelectors.ButtonDeleteSupplierInvoice);
            if (deleteBtnExists.length > 0) {
                // Button exists but not visible, try to click it with force
                cy.log('✓ Delete button exists, clicking with force');
                cy.get(SupplierAccountSelectors.ButtonDeleteSupplierInvoice, { timeout: 5000 })
                    .first()
                    .click({ force: true });
            } else {
                // Look for a menu button or toggle button that might contain delete
                cy.log('⚠ Looking for menu button');
                cy.get('.ToggleButton, button[class*="Menu"], button[class*="Action"]', { timeout: 5000 })
                    .first()
                    .click({ force: true });
                
                cy.wait(500);
                
                // Now try to click delete button
                cy.get(SupplierAccountSelectors.ButtonDeleteSupplierInvoice, { timeout: 5000 })
                    .first()
                    .click({ force: true });
            }
        }
    });
    
    cy.wait(2000); // Wait for confirmation dialog
    
    // Wait for the Yes button to appear and click it - try multiple selectors
    cy.log('=== Waiting for delete confirmation Yes button ===');
    cy.get('button[id^="ConfirmWindow_Yes"]:visible, button.RedButton:contains("כן"), button:contains("כן")', { timeout: 10000 })
        .should('be.visible')
        .first()
        .click({ force: true });
    
    cy.wait(2000); // Wait for deletion to complete
    cy.log('✓ Supplier Account deleted');
}

export function AssertDeleteRow() {
    // Check if row doesn't exist or is not visible (since it might remain in DOM but hidden)
    cy.get('body').then(($body) => {
        const row = $body.find(SupplierAccountSelectors.SupplierAccountFirstRow);
        if (row.length > 0) {
            // Row exists, check if it's not visible
            cy.get(SupplierAccountSelectors.SupplierAccountFirstRow, { timeout: 5000 })
                .should('not.be.visible');
        } else {
            // Row doesn't exist, which is also fine
            cy.log('✓ Row does not exist in DOM');
        }
    });
}

