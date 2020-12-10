export function OpenNewShipmentWizard(type: string){
    cy.Click("#HelperNotesButton_0_0", null)
    cy.Click(".LogitudeToggleButtonItem", type)
}

export function FillShipmentDefaultFields(directionCode: string, transportModCode: string, shipmentTypeCode: string = null){
    cy.ClickRadio("#DirectionRadio_0" + directionCode)
    cy.ClickRadio("#TransportModeRadio_0" + transportModCode)

    if(shipmentTypeCode){
        cy.ClickRadio("#ShipmentTypeRadio_0" + shipmentTypeCode)
    }

    if(directionCode === "D" && transportModCode === "I"){
        cy.SelectLogLovRandomElement("#Shipment_ShipperId", false, 5)
        cy.SelectLogLovRandomElement("#Shipment_ConsigneeId", false, 5)
    }else{
        if(directionCode === "I"){
            cy.SelectLogLovRandomElement("#Shipment_ConsigneeId", false, 5)
        }else{
            cy.SelectLogLovRandomElement("#Shipment_ShipperId", false, 5)
        }

        cy.SelectLogLovRandomElement("#Shipment_MainCarriageFromPortId", false, 5)
        cy.SelectLogLovRandomElement("#Shipment_MainCarriageToPortId", false, 5)
    }
}

export function SaveShipment(resultFile:string) {
    cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", null)
}