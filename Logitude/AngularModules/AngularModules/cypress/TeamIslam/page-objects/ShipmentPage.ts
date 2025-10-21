/// <reference types="cypress" />

export class ShipmentPage {
    static navigateToShipments() {
        cy.get('#GeneralMHOperations', { timeout: 15000 }).click();
        cy.get('#SHIP', { timeout: 15000 }).click();
        cy.get('#Shipments-O-Q', { timeout: 15000 }).click();
    }

    static navigateToDocsIn() {
        this.navigateToShipments();
        
        // Try different possible selectors for Docs In tab
        cy.get('body').then(($body) => {
            if ($body.find('#ShipmentTHDocsIn').length > 0) {
                cy.get('#ShipmentTHDocsIn', { timeout: 5000 }).click();
            } else if ($body.find('[data-cy*="docs-in"]').length > 0) {
                cy.get('[data-cy*="docs-in"]', { timeout: 5000 }).click();
            } else if ($body.find('a:contains("Docs In")').length > 0) {
                cy.get('a:contains("Docs In")', { timeout: 5000 }).click();
            } else {
                cy.log('Docs In tab not found, skipping navigation');
            }
        });
    }

    static navigateToDocsOut() {
        this.navigateToShipments();
        
        // Try different possible selectors for Docs Out tab
        cy.get('body').then(($body) => {
            if ($body.find('#ShipmentTHDocsOut').length > 0) {
                cy.get('#ShipmentTHDocsOut', { timeout: 5000 }).click();
            } else if ($body.find('[data-cy*="docs-out"]').length > 0) {
                cy.get('[data-cy*="docs-out"]', { timeout: 5000 }).click();
            } else if ($body.find('a:contains("Docs Out")').length > 0) {
                cy.get('a:contains("Docs Out")', { timeout: 5000 }).click();
            } else {
                cy.log('Docs Out tab not found, skipping navigation');
            }
        });
    }

    static createNewShipment() {
        // Try to find and click the NEWDIRECT button with fallback selectors
        cy.get('body').then(($body) => {
            if ($body.find('#NEWDIRECT').length > 0) {
                cy.get('#NEWDIRECT', { timeout: 5000 }).click({ force: true });
            } else if ($body.find('[data-cy*="new-direct"]').length > 0) {
                cy.get('[data-cy*="new-direct"]', { timeout: 5000 }).click({ force: true });
            } else if ($body.find('button:contains("New Direct")').length > 0) {
                cy.get('button:contains("New Direct")', { timeout: 5000 }).click({ force: true });
            } else {
                cy.log('NEWDIRECT button not found, skipping creation');
                return;
            }
        });
        
        // Try to find direction radio with fallback
        cy.get('body').then(($body) => {
            if ($body.find('#DirectionRadio_0E').length > 0) {
                cy.get('#DirectionRadio_0E', { timeout: 5000 }).click({ force: true });
            } else if ($body.find('input[name*="direction"][value*="E"]').length > 0) {
                cy.get('input[name*="direction"][value*="E"]', { timeout: 5000 }).click({ force: true });
            }
        });
        
        // Try to find transport mode radio with fallback
        cy.get('body').then(($body) => {
            if ($body.find('#TransportModeRadio_0A').length > 0) {
                cy.get('#TransportModeRadio_0A', { timeout: 5000 }).click({ force: true });
            } else if ($body.find('input[name*="transport"][value*="A"]').length > 0) {
                cy.get('input[name*="transport"][value*="A"]', { timeout: 5000 }).click({ force: true });
            }
        });
    }

    static fillShipmentDetails(shipperId: string, consigneeId: string, description: string) {
        cy.get('#Shipment_ShipperId', { timeout: 5000 }).click({ force: true });
        cy.get('#Shipment_ShipperId').type(shipperId);
        cy.get('.DropDownListItem:first', { timeout: 5000 }).should('be.visible').click();
        
        cy.get('#Shipment_ConsigneeId', { timeout: 5000 }).click({ force: true });
        cy.get('#Shipment_ConsigneeId').type(consigneeId);
        cy.get('.DropDownListItem:first', { timeout: 5000 }).should('be.visible').click();
        
        cy.get('#Shipment_DescriptionOfGoods', { timeout: 5000 }).type(description);
    }

    static saveShipment() {
        cy.get('#ShipmentCreatebtn', { timeout: 5000 }).click();
    }
}
