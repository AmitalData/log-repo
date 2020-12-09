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
        cy.SelectLogLovFirstElement("#Shipment_ShipperId")
        cy.SelectLogLovFirstElement("#Shipment_ConsigneeId")
    }else{
        if(directionCode === "I"){
            cy.SelectLogLovFirstElement("#Shipment_ConsigneeId")
        }else{
            cy.SelectLogLovFirstElement("#Shipment_ShipperId")
        }

        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
    }
}

export function SaveShipment(resultFile:string) {
    cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", null)
}