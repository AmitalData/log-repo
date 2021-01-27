import { ShipmentDetails } from "../models/ShipmentDetails";

export function ValidateCreatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPostShipmentRequest", 200, resultFile)
}

export function ValidateUpdatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPutShipmentRequest", 200, resultFile)
}
