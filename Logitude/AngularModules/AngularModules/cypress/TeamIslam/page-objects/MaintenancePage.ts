/// <reference types="cypress" />

export class MaintenancePage {
    static navigateToMaintenance() {
        cy.get("#GeneralMHMaintenance", { timeout: 15000 }).click();
    }

    static searchForItem(searchTerm: string) {
        cy.get('#null_Search', { timeout: 15000 }).type(searchTerm);
    }

    static navigateToShippers() {
        this.navigateToMaintenance();
        this.searchForItem('Shipper');
        cy.get('#MaintenanceItemMTCL', { timeout: 15000 }).click();
    }

    static navigateToUsers() {
        this.navigateToMaintenance();
        this.searchForItem('user');
        cy.get('#MaintenanceItemMTUS', { timeout: 15000 }).click();
    }

    static navigateToCompanyAddress() {
        this.navigateToMaintenance();
        this.searchForItem('company address setting');
        cy.get('#MaintenanceItemCOAD', { timeout: 15000 }).click();
    }
}
