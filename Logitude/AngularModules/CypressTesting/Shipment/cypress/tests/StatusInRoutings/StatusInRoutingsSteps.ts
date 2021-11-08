import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PayableDetails } from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

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
Given("the user edit pick window",()=>{
cy.Navigate(ShipmentSelectors.EditPickUp,true)
})

Given("add expected departure with {string} as a value",(expectedDeparture)=>{
cy.FillLogTextBox(ShipmentSelectors.PickUpDeliveryETDDate,expectedDeparture,false)
})

Given("add Actual Departure with {string} as a value",(actualDeparture)=>{
  cy.FillLogTextBox(ShipmentSelectors.PickUpDeliveryATDDate,actualDeparture,false)
}) 

Given("add Actual Arrival with {string} as a value",(actualArrival)=>{
    cy.FillLogTextBox(ShipmentSelectors.PickUpDeliveryATADate,actualArrival,false)
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
  cy.FillLogTextBox(ShipmentSelectors.WarehouseLegExpectedReleaseDate,actualRelease,false)
})

//#endregio

//edit on main carriage window 

Given("the user edit main carriage window",()=>{
cy.Navigate(ShipmentSelectors.EditRoutingMainCarriage,true)
})

Given("add ATD with {string} as a value",(aTD)=>{
cy.FillLogTextBox(ShipmentSelectors.MainCarriageATDDate,aTD,false)
})

Given("add ATA with {string} as a value",(aTA)=>{
  cy.FillLogTextBox(ShipmentSelectors.MainCarriageATADate,aTA,false)
})
//#endregio

//edit on Delivary window 
Given("the user edit Delivary window",()=>{
  cy.Navigate(ShipmentSelectors.EditDelivery,true)
})
  
When("save pickup",(()=>{
    cy.Navigate(ShipmentSelectors.SaveClose,true) 
    //cy.Navigate(ShipmentSelectors.CloseBtn,true) 
    //cy.Navigate(ShipmentSelectors.ConfirmWindowYes+BaseSelectors.LastElement) 
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

