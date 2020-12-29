/// <reference types="cypress" />


//login
/// <reference types="cypress" />

let timeStamp = (new Date()).getTime()

//login


import { LoginComp } from '../../../Login/Login.po';

export class ShipmentViewr {

    private login: LoginComp = new LoginComp();

    constructor() {
    }
}




it('OpenShipmentView', () => {
    cy.wait(10000)
    cy.get('#GeneralMHOperations').click();
    cy.wait(100)
    cy.get('#SHIP').click();
    cy.get('#Shipments-O-Q').click()

})



it('CreateNewView ', () => {
   cy.wait(200)
    cy.get('#QueryList_0_0').should('be.visible')
    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('#NewViewId_0_0').click()
    cy.get('#NewViewTabchoose').click()
    cy.get('#ViewNameId').type('CypressTest View')
    cy.get('#NewViewSearchFields_0_0').type('first')
    cy.get(".ListBoxItem").eq(0).click();
    cy.get("#NewButtonViewAdd").click()
    cy.get("#NewButtonViewCreate").click()
   // cy.get('#BusyIndicator_0').should('not.be.visible')
})

it('EditShipmentView', () => {
  cy.wait(1000)

    //cy.get('#QueryList_0_0').should('be.visible')

    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('#SearchFieldsId_0_0').click({ force: true })
    cy.wait(100)
    cy.get('#QueryList_0_0').click({ force: true })
    cy.wait(100)
    cy.get('.ActionButtonsParent').should('be.visible')
    let lastShipment = cy.get('.ActionButtonsParent').last()
    lastShipment.trigger('mouseover')
    lastShipment.get('.ActionButtons').last().click({ force: true })
    cy.get('#NewViewSearchFields_0_1').type('account')
    cy.get(".ListBoxItem").eq(0).click();
    cy.wait(1000)
    cy.get("#NewButtonViewAdd").click()
    cy.get("#NewButtonViewCreate").click()
    cy.get('#BusyIndicator_0').should('not.be.visible')


})

it('DeleteShipmentView', () => {
    //cy.wait(3000)

    //cy.get('#QueryList_0_0').click({ force: true })
    //cy.get('#SearchFieldsId_0_0').click({ force: true })
    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('.ActionButtonsParent').should('be.visible')
    let lastShipment = cy.get('.ActionButtonsParent').last()
    lastShipment.trigger('mouseover')
    lastShipment.get('.ActionButtons').first().click({ force: true })
    cy.get('.ConfirmWindow').should('be.visible')
    cy.get('#ConfirmWindow_Yes_0').click()


})

