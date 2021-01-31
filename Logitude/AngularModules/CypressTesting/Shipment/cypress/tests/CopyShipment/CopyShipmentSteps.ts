import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
let ShipmentData: ShipmentDetails;
let ShipmentFile;
let CopiedShipmentFile;
Given("the user logged in", () => {
  cy.Login();
});
And("navigates to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});

Given("a direct shipment with the following details",
  (dataTable) => {
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentDefaultFields(ShipmentData);
  });
When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});
Then("the shipment should create successfully", () => {
  ShipmentFile = "CreatedShipmentsData/" + ShipmentData.ShipmentLevel + ShipmentData.Direction +
    ShipmentData.TransportMode +
    ((typeof ShipmentData.ShipmentType) === "undefined" || ShipmentData.ShipmentType === null ? "" : ShipmentData.ShipmentType) + ".json";
  Assertions.ValidateCreatedShipment(ShipmentFile);
});


Given("the user open the direct shipment", () => {
  Actions.OpenShipment(ShipmentFile)
})
When("copy the shipment", () => {
  Actions.CopyShipment(ShipmentData.ShipmentLevel);
});
Then("a shipment copy should create successfully", () => {
  CopiedShipmentFile = "CreatedShipmentsData/" + ShipmentData.ShipmentLevel + ShipmentData.Direction +
    ShipmentData.TransportMode +
    ((typeof ShipmentData.ShipmentType) === "undefined" || ShipmentData.ShipmentType === null ? "" : ShipmentData.ShipmentType) + "1" + ".json";
  Assertions.ValidateCreatedShipment(CopiedShipmentFile);
});