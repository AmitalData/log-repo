export function ValidateCreatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPostShipmentRequest", 200, resultFile)
}