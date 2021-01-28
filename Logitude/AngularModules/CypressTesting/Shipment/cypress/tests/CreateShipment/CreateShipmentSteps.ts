import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";

let _ShipmentDetails: ShipmentDetails;

Given("the user logged in", () => {
  cy.Login();
});

Given("navigate to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
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

Then("the shipment should create successfully", () => {
  let resultFile = "CreatedShipmentsData/" + _ShipmentDetails.ShipmentLevel + _ShipmentDetails.Direction +
  _ShipmentDetails.TransportMode +
  ((typeof _ShipmentDetails.ShipmentType) === "undefined" || _ShipmentDetails.ShipmentType === null ? "" : _ShipmentDetails.ShipmentType) + ".json";

  Assertions.ValidateCreatedShipment(resultFile);
});