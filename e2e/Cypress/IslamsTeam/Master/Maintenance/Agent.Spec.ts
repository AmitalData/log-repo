/// <reference types="cypress" />

import { LoginComp } from "../../../login/Login.po";

    export class NewAgentScenario {

        private login: LoginComp = new LoginComp();
    
     
    }



    it('QuickSearch', function () {

        cy.get('#GeneralMHMaintenance').click()
        cy.get('#null_Search').click();
    });


    it('SearchAgentTab', function () {


        cy.get('#null_Search').type("Agent");
        cy.get('#MaintenanceItemMTAG').click();
    });


    it('CreateNewAgent', function () {
        this.agentName = 'Ahmad Company-' + Math.random();

        cy.get('#NewButton_Agent').click();
        cy.get('#Address_Name').type(this.agentName);
        cy.get('#Address_City').type('Warsaw');
        cy.get('#Address_CountryId').type("Poland");
        cy.get('.DropDownListItem:first').click();
        cy.get('#OkButtonId').click();
    });



