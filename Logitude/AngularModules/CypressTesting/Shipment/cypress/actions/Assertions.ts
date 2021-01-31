import { ShipmentDetails } from "../models/ShipmentDetails";

export function ValidateCreatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPostShipmentRequest", 200, resultFile)
}

export function ValidateUpdatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPutShipmentRequest", 200, resultFile)
}
export function ValidateCreatedAPInvoice(InvoiceFile:string){
    cy.AssertResponseStatusCode("WaitPostAPInvoicesRequest", 200, InvoiceFile)
}

export function ValidateUpdatedAPInvoice(resultFile: string){
    cy.AssertResponseStatusCode("WaitPutAPInvoicesRequest", 200, resultFile)
}
export function HouseConnectedToMaster(HouseFile: string , MasterFile:string){
    let houseMasterId :string ;
    let masterID:string ;
    cy.fixture(HouseFile).then((shipment) => {
        houseMasterId = shipment.MasterShipmentDataId
    })
    cy.fixture(MasterFile).then((shipment) => {
        masterID = shipment.Id
    })
    
    assert.equal(houseMasterId,masterID)
}

export function HouseDisconnectedFromMaster(HouseFile: string , MasterFile:string){
    let houseNumber :string ;
    let masterHousesNumbers:string ;
    cy.fixture(HouseFile).then((shipment) => {
        houseNumber = shipment.ShipmentNumber
    })
    cy.fixture(MasterFile).then((shipment) => {
        masterHousesNumbers = shipment.MasterHousesNumbers
    })
    debugger;
    console.log("Master "+ masterHousesNumbers +"House "+ houseNumber)
    assert.notInclude(masterHousesNumbers,houseNumber);
    
}

