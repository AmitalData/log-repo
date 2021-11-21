import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region variables
let ShipmentData: ShipmentDetails;
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
//#endregion

//#region Create direct export air shipment Given steps
Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  ShipmentData = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
  Actions.FillShipmentWizardsFields(ShipmentData);
});
//#endregion

//#region Update routing tab given steps

Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Navigate(ShipmentSelectors.RoutingsTab);
});

Given("add a warehouse with {string} as a terminal", (warehouseLegTerminal) => {
  Actions.AddWarehouseLegPickups(warehouseLegTerminal)

});

Given("the user add new pickup", () => {
  Actions.FillPickupRouting()
});

Given("add delivery with {string} as a partner routing", (partner) => {
  Actions.FillDeliveryRouting(partner)
});

//#endregio

//edit on pickup window 
Given("the user edit pickup window",()=>{
cy.Navigate(ShipmentSelectors.EditPickUp,true)
})

Given("add expected departure with {string} as a value and Notes {string}",(expectedDeparture,noteExpetedPickUp)=>{
cy.FillDate(ShipmentSelectors.PickUpDeliveryETDDate,expectedDeparture)
cy.FillLogTextBox(ShipmentSelectors.PickUpDeliverynote,noteExpetedPickUp,false)
})

Given("add Actual Departure with {string} as a value and Notes {string}",(actualDeparture,noteactualDeparture)=>{
  cy.FillDate(ShipmentSelectors.PickUpDeliveryATDDate,actualDeparture)
  cy.FillLogTextBox(ShipmentSelectors.PickUpDeliverynote,noteactualDeparture,false)
}) 

Given("add Actual Arrival with {string} as a value and Notes {string}",(actualArrival,noteactualArrival)=>{
    cy.FillDate(ShipmentSelectors.PickUpDeliveryATADate,actualArrival)
    cy.FillLogTextBox(ShipmentSelectors.PickUpDeliverynote,noteactualArrival,false)
}) 

 //#endregio

//edit on Warehouse window 
Given("the user edit Warehouse window",()=>{
  cy.Navigate(ShipmentSelectors.EditWarehouseLegPickups,true)
})
  
Given("add Actual Entry with {string} as a value",(actualEntry)=>{
  cy.FillLogTextBox(ShipmentSelectors.WarehouseLegActualEntryDate,actualEntry,false)
})

Given("add Actual Release with {string} as a value",(actualRelease)=>{
  cy.FillLogTextBox(ShipmentSelectors.WarehouseLegActualReleaseDate,actualRelease,false)
})

//#endregio

//edit on main carriage window 

Given("the user edit main carriage window",()=>{
cy.Navigate(ShipmentSelectors.EditRoutingMainCarriage,true)
})

Given("add ATD with {string} as a value",(aTD)=>{
cy.FillDate(ShipmentSelectors.MainCarriageATDDate,aTD)
})

Given("add ATA with {string} as a value",(aTA)=>{
  cy.FillDate(ShipmentSelectors.MainCarriageATADate,aTA)
})

//#endregio

//edit on Delivary window 
Given("the user edit Delivary window",()=>{
  cy.Navigate(ShipmentSelectors.EditDelivery,true)
})
  
When("save pickup",(()=>{
  Actions.SavePickUpDlivery()
}))

When("save Warehouse",(()=>{
  Actions.SaveWaerehouse()
}))  

When("save main carriage",(()=>{
  Actions.SaveMainCarriage()
}))   

When("save Delivary",(()=>{
  Actions.SavePickUpDlivery()
}))

When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

When("update shipment", () => {
  Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the status value should be {string}", (statusValue) => {
  BaseAssertion.AssertElementContain(ShipmentSelectors.RoutingStatus, statusValue)
});

//#endregion

When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

When("update shipment", () => {
  Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});


Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  })
});

