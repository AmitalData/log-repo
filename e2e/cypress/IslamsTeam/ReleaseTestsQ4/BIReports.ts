/// <reference types="cypress" />

import { LoginComp } from "../../login/Login.po";

    export class NewBIReportScenario {

        private login: LoginComp = new LoginComp();
    
     
    }



    it('CreateBIReportFolder', function () {

        cy.get('#GeneralMHMaintenance').click()
        cy.get('#null_Search').click();
    });


    it('CreateBIReport', function () {


        cy.get('#null_Search').type("Agent");
        cy.get('#MaintenanceItemMTAG').click();
    });

    it('RunBIReport', function () {


        cy.get('#null_Search').type("Agent");
        cy.get('#MaintenanceItemMTAG').click();
    });

    it('EditBIReport', function () {
        this.agentName = 'Ahmad Company-' + Math.random();

        cy.get('#NewButton_Agent').click();
        cy.get('#Address_Name').type(this.agentName);
        cy.get('#Address_City').type('Warsaw');
        cy.get('#Address_CountryId').type("Poland");
        cy.get('.DropDownListItem:first').click();
        cy.get('#OkButtonId').click();
    });



