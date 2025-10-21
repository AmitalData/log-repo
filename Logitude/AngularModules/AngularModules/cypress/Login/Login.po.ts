/// <reference types="cypress" />

export class LoginComp {
    
    static login() {
        cy.visit(Cypress.env("URL"))
        cy.get('#Email').clear().type(Cypress.env("Email"), { delay: 50 })
        cy.get('#Email').invoke('val').should('eq', Cypress.env("Email"))
        cy.get('#Password').clear().type(Cypress.env("Password"))
        cy.get('#cmdLogin').click()
        cy.url().should('not.include', '/login')
    }
}