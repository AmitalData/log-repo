import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

let _ShipmentDetails: ShipmentDetails;

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

Given("a direct shipment with the following details",
  (dataTable) => {
   const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
   _ShipmentDetails = shipmentDetails;
   Actions.OpenNewShipmentWizard(_ShipmentDetails.ShipmentLevel);
   Actions.FillShipmentDefaultFields(_ShipmentDetails);
});

When("create shipment", () => {
  Actions.CreateShipment(_ShipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200);
});