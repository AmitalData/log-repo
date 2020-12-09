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
        cy.SelectLogLovFirstElement("#Shipment_ShipperId", false)
        cy.SelectLogLovFirstElement("#Shipment_ConsigneeId", false)
    }else{
        if(directionCode === "I"){
            cy.SelectLogLovFirstElement("#Shipment_ConsigneeId", false)
        }else{
            cy.SelectLogLovFirstElement("#Shipment_ShipperId", false)
        }

        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId", false)
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId", false)
    }
}

export function SaveShipment(resultFile:string) {
    cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", null)
}