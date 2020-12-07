declare namespace Cypress {
    interface Chainable {
        NavigateToShipmentsWorkSpace(): Chainable<Element> 
        OpenNew(): Chainable<Element> 
        FillShipmentDirection(directionCode:string): Chainable<Element> 
        FillShipmentTransportMod(transportModCode:string): Chainable<Element> 
        FillShipmentType(shipmentTypeCode:string): Chainable<Element> 
        OpenNewShipmentWizard(type:string): Chainable<Element> 
        FillShipmentDefaultFields(directionCode:string,transportModCode:string,shipmentTypeCode?:string): Chainable<Element> 


    } 
    
}

Cypress.Commands.add("NavigateToShipmentsWorkSpace", () => {
    cy.Login();   
    cy.Click("#GeneralMHOperations");
    cy.Click("#SHIP"); 
})
Cypress.Commands.add("FillShipmentDirection", (directionCode) => {
    cy.ClickRadio("#DirectionRadio_0" + directionCode);
})
Cypress.Commands.add("FillShipmentTransportMod", (transportModCode) => {
    cy.ClickRadio("#TransportModeRadio_0" + transportModCode);
})
Cypress.Commands.add("FillShipmentType", (shipmentTypeCode) => {
    cy.ClickRadio("#ShipmentTypeRadio_0" + shipmentTypeCode);
})
Cypress.Commands.add("OpenNewShipmentWizard", (type) => {
    cy.Click("#HelperNotesButton_0_0")
        cy.Click(".LogitudeToggleButtonItem", type)
})
Cypress.Commands.add("FillShipmentDefaultFields", (directionCode,transportModCode,shipmentTypeCode = null) => {
    cy.FillShipmentDirection(directionCode);
    cy.FillShipmentTransportMod(transportModCode);
    if(shipmentTypeCode){
    cy.FillShipmentType(shipmentTypeCode);
    }
    cy.SelectLogLovFirstElement("#Shipment_CustomerId")
    cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
    cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
})