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
Given("the user edit pick expected departure with {string} as a value",(expectedDeparture)=>{
//cy.FillLogTextBox

})
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

