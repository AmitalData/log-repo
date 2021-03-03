import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";

let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let EventNote ; 

//#region  Create Direct Shipment
Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details",
  (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
  });

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  });
});
//#endregion

Given("the user open the shipment", () => {
  Actions.OpenShipment(shipmentNumber);
});

When("cancel the shipment with {string} Note", (note) => {
  EventNote = note
  Actions.CancelShipment(note);
});

Then("the shipment should cancel successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  Actions.ValidateCancelIconExist(true);
  Actions.ValidateShipmentEventActions(ShipmentSelectors.EventsTab,EventNote);
  Actions.ValidateShipmentFields(true);
}); 

When("reactive the shipment with {string} Note",(note)=>{
  EventNote = note
  Actions.ReactiveShipment(note);
})

Then("the shipment should reactive successfully",()=>{
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  Actions.ValidateCancelIconExist(false);
  Actions.ValidateShipmentEventActions(ShipmentSelectors.EventsTab,EventNote);
  Actions.ValidateShipmentFields(false);

})
