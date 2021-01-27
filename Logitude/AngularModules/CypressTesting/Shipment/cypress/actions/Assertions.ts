import { ShipmentDetails } from "../models/ShipmentDetails";

export function ValidateCreatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPostShipmentRequest", 200, resultFile)
}

export function ValidateUpdatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPutShipmentRequest", 200, resultFile)
}
export function ValidateEqualityOfTwoShipments(file1: string, file2: string) {
    let isCompare=false;
     let _ShipmentData: ShipmentDetails;
     cy.fixture(file1).then((ShipmentData) => {
      _ShipmentData=ShipmentData;
         _ShipmentData.ShipmentLevel = ShipmentData.ShipmentLevelName;
         _ShipmentData.Direction = ShipmentData.DirectionName;
         _ShipmentData.TransportMode = ShipmentData.TransportModeName
         _ShipmentData.ShipmentType = ShipmentData.ShipmentTypeName;
         _ShipmentData.Shipper = ShipmentData.ShipperName
         _ShipmentData.Consignee = ShipmentData.ConsigneeName
         _ShipmentData.Agent = ShipmentData.AgentName
         _ShipmentData.MainCarriageToPort = ShipmentData.MainCarriageFromPortCode
         _ShipmentData.MainCarriageFromPort = ShipmentData.MainCarriageToPortCode
     })
    
     cy.fixture(file2).then((ShipmentData) => {
         if (_ShipmentData.ShipmentLevel == ShipmentData.ShipmentLevelName
             && _ShipmentData.Direction === ShipmentData.DirectionName
             && _ShipmentData.TransportMode === ShipmentData.TransportModeName
             && _ShipmentData.Shipper == ShipmentData.ShipperName
             && _ShipmentData.Consignee == ShipmentData.ConsigneeName
             && _ShipmentData.Agent == ShipmentData.AgentName
             && _ShipmentData.MainCarriageToPort == ShipmentData.MainCarriageFromPortCode
             && _ShipmentData.MainCarriageFromPort == ShipmentData.MainCarriageToPortCode) {
                 isCompare=true;
         }
         else {
             isCompare=false    
             }
         expect(isCompare).to.eq(true)
     })
 }
 