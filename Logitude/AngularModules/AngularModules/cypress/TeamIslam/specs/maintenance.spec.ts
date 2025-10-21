/// <reference types="cypress" />
import { LoginPage } from '../page-objects/LoginPage';
import { MaintenancePage } from '../page-objects/MaintenancePage';

describe('Maintenance Tests', () => {
    beforeEach(() => {
        LoginPage.loginAndVerify();
    });

    it('Should navigate to Shippers', () => {
        MaintenancePage.navigateToShippers();
        cy.get('#MaintenanceItemMTCL').should('be.visible');
    });

    it('Should navigate to Users', () => {
        MaintenancePage.navigateToUsers();
        cy.get('#MaintenanceItemMTUS').should('be.visible');
    });

    it('Should navigate to Company Address Settings', () => {
        MaintenancePage.navigateToCompanyAddress();
        cy.get('#MaintenanceItemCOAD').should('be.visible');
    });
});
