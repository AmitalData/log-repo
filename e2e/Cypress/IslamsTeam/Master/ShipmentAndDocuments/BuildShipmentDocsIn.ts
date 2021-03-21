// @ts-nocheck
/// <reference types="cypress" />

import { LoginComp } from "../../../login/Login.po";
import { CreateEditShipment } from './CreateEditShipment';
export class NewAgentScenario {

    private login: LoginComp = new LoginComp();
    private CreateEditShipment: CreateEditShipment = new CreateEditShipment();

    constructor() {
    }
}


it('OpenDocsInTab', function () {

    cy.get('#ShipmentTHDocsIn').click();

});

it('Successfully Upload File', function () {
    //cy.get('#BusyIndicator_0').click();

    //cy.get('#SearchFieldsId_0_1').click();
    cy.get('#SearchFieldsId_0_1').type('General Message');
    cy.wait(2000)
    cy.get('#row0').click();
    cy.get('#UploadDocumentdbtn', { multiple: true }).click();
    //cy.get('.Button').click()

    
    const fileName = 'Logitude.jpg'
    cy.fixture('Logitude.jpg').then(function (fileContent) {
        cy.get('input.upload').attachFile({ fileContent, fileName, mimetype: 'application/pdf',encoding: 'base64'})
        cy.get('#FileUploadedSuccessfully').should('be.visible')
        cy.get('.RedButton').click()

    })

    

})
