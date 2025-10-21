/// <reference types="cypress" />
import { LoginPage } from '../page-objects/LoginPage';

describe('Login Tests', () => {
    it('Should visit the application', () => {
        LoginPage.visit();
        cy.url().should('include', Cypress.env("URL"));
    });

    it('Should login successfully', () => {
        LoginPage.loginAndVerify();
    });
});
