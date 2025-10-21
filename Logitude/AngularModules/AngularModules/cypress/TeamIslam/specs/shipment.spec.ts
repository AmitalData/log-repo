/// <reference types="cypress" />
import { LoginPage } from '../page-objects/LoginPage';
import { ShipmentPage } from '../page-objects/ShipmentPage';

describe('Shipment Tests', () => {
    beforeEach(() => {
        LoginPage.loginAndVerify();
    });

    it('Should navigate to Docs In tab', () => {
        ShipmentPage.navigateToDocsIn();
        cy.get('#ShipmentTHDocsIn').should('be.visible');
    });

    it('Should navigate to Docs Out tab', () => {
        ShipmentPage.navigateToDocsOut();
        cy.get('#ShipmentTHDocsOut').should('be.visible');
    });

    it('Should create a new shipment', () => {
        ShipmentPage.navigateToShipments();
        ShipmentPage.createNewShipment();
        ShipmentPage.fillShipmentDetails('TestShipperExport1', 'TestConsigneeExport1', 'CreateShipmentFromCypress');
        ShipmentPage.saveShipment();
    });
});
