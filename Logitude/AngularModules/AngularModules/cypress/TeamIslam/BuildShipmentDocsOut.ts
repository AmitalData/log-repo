/// <reference types="cypress" />
import { LoginComp } from ".././login/Login.po";
import { CreateEditShipment } from './CreateEditShipment';

export class NewAgentScenario {

    private login: LoginComp = new LoginComp();
    private CreateEditShipment: CreateEditShipment = new CreateEditShipment();

    constructor() {
    }
}


it('OpenDocOutTab', function () {

    cy.get('#ShipmentTHDocsOut').click();

});

it('Successfully Printing Document', function () {

    cy.get('#SearchFieldsId_0_1').type('Export Trucking Order');
    cy.get('#ETO-L-DocsOut').click();
    cy.get('#ETO-P-DocsOut').click();
    cy.get('#BusyIndicator_0').should('be.visible')



    cy.get('#BusyIndicator_0').should('be.visible')
    cy.get('#BusyIndicator_0').should('not.be.visible')


    cy.get('#BuildDocumentSucceededDiv').click({ force: true })

    cy.get('#closeButtonId').click()


})

it('Failing Printing Document', function () {

    cy.get('#SearchFieldsId_0_1').clear();

    cy.get('#SearchFieldsId_0_1').type('Failure Test Document');
    cy.get('#FTDT-L-DocsOut').click();
    cy.get('#FTDT-P-DocsOut').click();

    cy.get('#MessageWindow_Ok_0').click()

})