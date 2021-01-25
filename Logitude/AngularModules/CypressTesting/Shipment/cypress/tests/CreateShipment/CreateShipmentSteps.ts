import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";

let _ShipmentDetails: ShipmentDetails;

Given("User logged in", () => {
  cy.Login();
});

Given("Go to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});

Given("Shipment details",
  (dataTable) => {
   const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
   _ShipmentDetails = shipmentDetails;
   Actions.OpenNewShipmentWizard(_ShipmentDetails.ShipmentLevel);
   Actions.FillShipmentDefaultFields(_ShipmentDetails);
});

When("Click create shipment button", () => {
  Actions.CreateShipment(_ShipmentDetails.ShipmentLevel);
});

Then("The create operation completed successfully", () => {
  let resultFile = "CreatedShipmentsData/" + _ShipmentDetails.ShipmentLevel + _ShipmentDetails.Direction +
  _ShipmentDetails.TransportMode +
  ((typeof _ShipmentDetails.ShipmentType) === "undefined" || _ShipmentDetails.ShipmentType === null ? "" : _ShipmentDetails.ShipmentType) + ".json";

  Assertions.ValidateCreatedShipment(resultFile);
});