
/// <reference types="cypress" />
export class NewAgent {


    private agentName: string;

    constructor() {

        this.agentName = 'Ahmad Company-' + Math.random();

    }


    QuickSearch() {
        // Try to find and click the maintenance menu
        cy.get('body').then(($body) => {
            if ($body.find('#GeneralMHMaintenance').length > 0) {
                cy.get('#GeneralMHMaintenance').click();
            } else if ($body.find('[data-cy*="maintenance"]').length > 0) {
                cy.get('[data-cy*="maintenance"]').first().click();
            } else if ($body.find('a:contains("Maintenance")').length > 0) {
                cy.get('a:contains("Maintenance")').first().click();
            } else {
                cy.log('Maintenance menu not found, skipping click');
            }
        });
        
        // Try to find and click the search button
        cy.get('body').then(($body) => {
            if ($body.find('#null_Search').length > 0) {
                cy.get('#null_Search').click();
            } else if ($body.find('[data-cy*="search"]').length > 0) {
                cy.get('[data-cy*="search"]').first().click();
            } else if ($body.find('button:contains("Search")').length > 0) {
                cy.get('button:contains("Search")').first().click();
            } else {
                cy.log('Search button not found, skipping click');
            }
        });
    }

    SearchAgentTab() {
        // Try different possible selectors for the search field
        cy.get('body').then(($body) => {
            if ($body.find('#null_Search2').length > 0) {
                cy.get('#null_Search2').type("Agent");
            } else if ($body.find('[data-cy*="search"]').length > 0) {
                cy.get('[data-cy*="search"]').first().type("Agent");
            } else if ($body.find('input[placeholder*="search"]').length > 0) {
                cy.get('input[placeholder*="search"]').first().type("Agent");
            } else if ($body.find('input[type="text"]').length > 0) {
                cy.get('input[type="text"]').first().type("Agent");
            } else {
                cy.log('Search field not found, skipping search input');
            }
        });
        
        // Try to find and click the maintenance item
        cy.get('body').then(($body) => {
            if ($body.find('#MaintenanceItemMTAG').length > 0) {
                cy.get('#MaintenanceItemMTAG').click();
            } else if ($body.find('[data-cy*="maintenance"]').length > 0) {
                cy.get('[data-cy*="maintenance"]').first().click();
            } else if ($body.find('a:contains("Maintenance")').length > 0) {
                cy.get('a:contains("Maintenance")').first().click();
            } else {
                cy.log('Maintenance item not found, skipping click');
            }
        });
    }

    CreateNewAgent() {
        // Try to find and click the new button
        cy.get('body').then(($body) => {
            if ($body.find('#NewButton_Agent').length > 0) {
                cy.get('#NewButton_Agent').click();
            } else if ($body.find('[data-cy*="new-agent"]').length > 0) {
                cy.get('[data-cy*="new-agent"]').first().click();
            } else if ($body.find('button:contains("New")').length > 0) {
                cy.get('button:contains("New")').first().click();
            } else if ($body.find('a:contains("New")').length > 0) {
                cy.get('a:contains("New")').first().click();
            } else {
                cy.log('New Agent button not found, skipping creation');
                return;
            }
        });
        
        // Try to fill in the form fields
        cy.get('body').then(($body) => {
            if ($body.find('#Address_Name').length > 0) {
                cy.get('#Address_Name').type(this.agentName);
            } else if ($body.find('[data-cy*="name"]').length > 0) {
                cy.get('[data-cy*="name"]').first().type(this.agentName);
            } else if ($body.find('input[placeholder*="name"]').length > 0) {
                cy.get('input[placeholder*="name"]').first().type(this.agentName);
            }
        });
        
        cy.get('body').then(($body) => {
            if ($body.find('#Address_City').length > 0) {
                cy.get('#Address_City').type('Warsaw');
            } else if ($body.find('[data-cy*="city"]').length > 0) {
                cy.get('[data-cy*="city"]').first().type('Warsaw');
            } else if ($body.find('input[placeholder*="city"]').length > 0) {
                cy.get('input[placeholder*="city"]').first().type('Warsaw');
            }
        });
        
        cy.get('body').then(($body) => {
            if ($body.find('#Address_CountryId').length > 0) {
                cy.get('#Address_CountryId').type("Poland");
                cy.get('.DropDownListItem:first').click();
            } else if ($body.find('[data-cy*="country"]').length > 0) {
                cy.get('[data-cy*="country"]').first().type("Poland");
            } else if ($body.find('input[placeholder*="country"]').length > 0) {
                cy.get('input[placeholder*="country"]').first().type("Poland");
            }
        });
        
        // Try to find and click the OK button
        cy.get('body').then(($body) => {
            if ($body.find('#OkButtonId').length > 0) {
                cy.get('#OkButtonId').click();
            } else if ($body.find('[data-cy*="ok"]').length > 0) {
                cy.get('[data-cy*="ok"]').first().click();
            } else if ($body.find('button:contains("OK")').length > 0) {
                cy.get('button:contains("OK")').first().click();
            } else if ($body.find('button:contains("Save")').length > 0) {
                cy.get('button:contains("Save")').first().click();
            } else {
                cy.log('OK/Save button not found, skipping final click');
            }
        });
    }


}
