
/// <reference types="cypress" />
export class NewAgent {


    private agentName: string;

    constructor() {

        this.agentName = 'Ahmad Company-' + Math.random();

    }


    QuickSearch() {



        cy.get('#GeneralMHMaintenance').click()
        cy.get('#null_Search').click();




        //cy.wait('@GeneralMHMaintenance').get().should('be.visible'),
        // if (cy.get('#GeneralMHMaintenance').visible) cy.get('#GeneralMHMaintenance').click();
        //cy.get('#GeneralMHMaintenance',{timeout:50000}).click().should('be.visible'),




        //this.helper.WaitBusyIndicator();
    }

    SearchAgentTab() {

        cy.get('#null_Search2').type("Agent");
        cy.get('#MaintenanceItemMTAG').click();


    }

    CreateNewAgent() {
        cy.get('#NewButton_Agent').click();
        cy.get('#Address_Name').type(this.agentName);
        cy.get('#Address_City').type('Warsaw');
        cy.get('#Address_CountryId').type("Poland");
        cy.get('.DropDownListItem:first').click();
        cy.get('#OkButtonId').click();
    }


}
