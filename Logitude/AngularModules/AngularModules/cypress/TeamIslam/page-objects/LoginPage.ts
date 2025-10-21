/// <reference types="cypress" />

export class LoginPage {
    static visit() {
        cy.visit(Cypress.env("URL"), { timeout: 15000 });
        cy.get('body', { timeout: 15000 }).should('be.visible');
    }

    static login(email: string = Cypress.env("Email"), password: string = Cypress.env("Password")) {
        cy.get('#Email', { timeout: 15000 }).clear().type(email, { delay: 50 });
        cy.get('#Password', { timeout: 15000 }).clear().type(password);
        cy.get('#cmdLogin', { timeout: 15000 }).click();
        cy.url({ timeout: 20000 }).should('not.include', '/login');
    }

    static loginAndVerify() {
        this.visit();
        this.login();
        // Verify login was successful
        cy.get('#Email').invoke('val').should('eq', Cypress.env("Email"));
    }
}
