/// <reference types="cypress" />

export class ShipmentPage {
    static navigateToShipments() {
        cy.get('#GeneralMHOperations', { timeout: 15000 }).click();
        cy.get('#SHIP', { timeout: 15000 }).click();
        cy.get('#Shipments-O-Q', { timeout: 15000 }).click();
    }

    static navigateToDocsIn() {
        this.navigateToShipments();
        cy.get('#ShipmentTHDocsIn', { timeout: 15000 }).click();
    }

    static navigateToDocsOut() {
        this.navigateToShipments();
        cy.get('#ShipmentTHDocsOut', { timeout: 15000 }).click();
    }

    static createNewShipment() {
        cy.get('#NEWDIRECT', { timeout: 15000 }).click();
        cy.get('#DirectionRadio_0E', { timeout: 5000 }).click({ force: true });
        cy.get('#TransportModeRadio_0A', { timeout: 5000 }).click({ force: true });
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
