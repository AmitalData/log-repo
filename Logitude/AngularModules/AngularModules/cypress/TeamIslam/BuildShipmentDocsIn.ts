// @ts-nocheck
/// <reference types="cypress" />

import { LoginComp } from ".././login/Login.po";
import { CreateEditShipment } from './CreateEditShipment';
export class NewAgentScenario {

    private login: LoginComp = new LoginComp();
    private CreateEditShipment: CreateEditShipment = new CreateEditShipment();

    constructor() {
    }
}

describe('BuildShipmentDocsIn Tests', () => {
    it('OpenDocsInTab', function () {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get('#GeneralMHOperations', { timeout: 15000 }).click()
    cy.get('#SHIP', { timeout: 15000 }).click()
    cy.get('#Shipments-O-Q', { timeout: 15000 }).click()
    cy.get('#ShipmentTHDocsIn', { timeout: 15000 }).click();
});

it('Successfully Upload File', function () {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get('#GeneralMHOperations', { timeout: 5000 }).click()
    cy.get('#SHIP', { timeout: 5000 }).click()
    cy.get('#Shipments-O-Q', { timeout: 5000 }).click()
    cy.get('#ShipmentTHDocsIn', { timeout: 5000 }).click()
    
    cy.get('#SearchFieldsId_0_1', { timeout: 5000 }).type('General Message');
    cy.get('#row0', { timeout: 5000 }).click();
    cy.get('#UploadDocumentdbtn', { multiple: true, timeout: 5000 }).click();
    const fileName = 'dummy.pdf'
    cy.fixture('dummy.pdf').then(function (fileContent) {
        cy.get('input.upload', { timeout: 5000 }).attachFile({ fileContent, fileName, mimetype: 'application/pdf' })
        cy.get('#FileUploadedSuccessfully', { timeout: 10000 }).should('be.visible')
        cy.get('.RedButton', { timeout: 5000 }).click()
    })
})
})
