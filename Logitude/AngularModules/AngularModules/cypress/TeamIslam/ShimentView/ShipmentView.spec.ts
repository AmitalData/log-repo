/// <reference types="cypress" />
  

  //login
/// <reference types="cypress" />
  
let timeStamp = (new Date()).getTime()

//login


import { LoginComp } from '../../Login/Login.po';

export class ShipmentViewr  {

private login: LoginComp = new LoginComp();

constructor() {
}
}




it('OpenShipmentView', () => {
    cy.visit(Cypress.env("URL"))
    cy.get('#Email').type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password').type(Cypress.env("Password"))
    cy.get('#cmdLogin').click()
    cy.url().should('not.include', '/login')
    
    cy.get('#GeneralMHOperations').click();
    cy.get('#SHIP').click();
    cy.get('#Shipments-O-Q').click()
  })



it('CreateNewView ', () => {
    cy.visit(Cypress.env("URL"))
    cy.get('#Email').type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password').type(Cypress.env("Password"))
    cy.get('#cmdLogin').click()
    cy.url().should('not.include', '/login')
    
    cy.get('#GeneralMHOperations').click()
    cy.get('#SHIP').click()
    cy.get('#Shipments-O-Q').click()
    
    cy.get('#QueryList_0_0', { timeout: 5000 }).click()
    cy.get('#NewViewId_0_0', { timeout: 5000 }).click() 
    cy.get('#NewViewTabchoose', { timeout: 5000 }).click()
    cy.get('#ViewNameId', { timeout: 5000 }).type('CypressTest View')
    cy.get('#NewViewSearchFields_0_0', { timeout: 5000 }).type('first')
    cy.get(".ListBoxItem", { timeout: 5000 }).eq(0).click();
    cy.get("#NewButtonViewAdd", { timeout: 5000 }).click()
    cy.get("#NewButtonViewCreate", { timeout: 5000 }).click()
  })

it('EditShipmentView', () => {
    cy.visit(Cypress.env("URL"))
    cy.get('#Email').type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password').type(Cypress.env("Password"))
    cy.get('#cmdLogin').click()
    cy.url().should('not.include', '/login')
    
    cy.get('#GeneralMHOperations').click()
    cy.get('#SHIP').click()
    cy.get('#Shipments-O-Q').click()
    
    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('#SearchFieldsId_0_0').click({ force: true })
    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('.ActionButtonsParent', { timeout: 5000 }).should('be.visible')
    let lastShipment = cy.get('.ActionButtonsParent').last()
    lastShipment.trigger('mouseover')
    lastShipment.get('.ActionButtons').last().click({ force: true, timeout: 5000 })
    cy.get('#NewViewSearchFields_0_1').type('account')
    cy.get(".ListBoxItem").eq(0).click();
    cy.get("#NewButtonViewAdd").click()
    cy.get("#NewButtonViewCreate").click()
})



it('DeleteShipmentView',()=>{
    cy.visit(Cypress.env("URL"))
    cy.get('#Email').type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password').type(Cypress.env("Password"))
    cy.get('#cmdLogin').click()
    cy.url().should('not.include', '/login')
    
    cy.get('#GeneralMHOperations').click()
    cy.get('#SHIP').click()
    cy.get('#Shipments-O-Q').click()
    
    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('#SearchFieldsId_0_0').click({ force: true })
    cy.get('#QueryList_0_0').click({ force: true })
    cy.get('.ActionButtonsParent', { timeout: 5000 }).should('be.visible')
    let lastShipment = cy.get('.ActionButtonsParent').last()
    lastShipment.trigger('mouseover')
    lastShipment.get('.ActionButtons').first().click({ force: true, timeout: 5000 })
    cy.get('.ConfirmWindow', { timeout: 5000 }).should('be.visible')
    cy.get('#ConfirmWindow_Yes_0').click({ timeout: 5000 })
})
