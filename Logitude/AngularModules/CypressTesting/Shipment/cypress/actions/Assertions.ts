export function ValidateCreatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPostShipmentRequest", 200, resultFile)
}

export function SaveOperationCompletedSuccessfully(){
    cy.intercept('PUT', '/test/api/shipment', (req) => {
        req.reply((response) => {
            assert.equal(response.statusCode, 200);
        })
    })
}