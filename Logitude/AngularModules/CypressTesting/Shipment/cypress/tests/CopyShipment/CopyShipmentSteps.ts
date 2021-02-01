import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";

let ShipmentData: ShipmentDetails;
let ResultFile: string;

Given("the user logged in", () => {
  cy.Login();
});

Given("navigates to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});

Given("a direct shipment with the following details",
  (dataTable) => {
    let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
});

When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
  ResultFile = "CreatedShipmentsData/" + ShipmentData.ShipmentLevel.charAt(0) + ShipmentData.Direction +
  ShipmentData.TransportMode + ((typeof ShipmentData.ShipmentType) === "undefined" || ShipmentData.ShipmentType === null ? "" : ShipmentData.ShipmentType) + ".json";

  Assertions.ValidateCreatedShipment(ResultFile);
});

Given("the user open the direct shipment", () => {
  Actions.OpenShipment(ResultFile)
});

When("copy the shipment", () => {
  cy.Click(Selectors.ShipmentMoreList, null)
  cy.Click(Selectors.CopyShipmentButton,null)
  Actions.CopyShipment(ShipmentData.ShipmentLevel);
});

Then("a shipment copy should create successfully", () => {
  Assertions.ValidateCreatedShipment(null);
});