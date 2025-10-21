/// <reference types="cypress" />

describe('Simple Test', () => {
    it('Should visit the application', () => {
        cy.visit(Cypress.env("URL"), { timeout: 30000 })
        cy.get('body', { timeout: 30000 }).should('be.visible')
        cy.log('Application loaded successfully')
    })
    
    it('Should login successfully', () => {
        cy.visit(Cypress.env("URL"), { timeout: 30000 })
        cy.get('body', { timeout: 30000 }).should('be.visible')
        cy.get('#Email', { timeout: 30000 }).should('be.visible').type(Cypress.env("Email"), { delay: 50 })
        cy.get('#Password', { timeout: 30000 }).should('be.visible').type(Cypress.env("Password"))
        cy.get('#cmdLogin', { timeout: 30000 }).should('be.visible').click()
        cy.url({ timeout: 30000 }).should('not.include', '/login')
        cy.log('Login successful')
    })
})
