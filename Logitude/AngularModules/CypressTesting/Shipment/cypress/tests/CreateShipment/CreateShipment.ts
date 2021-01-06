import * as shipmentActions from "../../actions/Actions";
import * as shipmentAssertions from "../../actions/Assertions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";

let LevelCode: string;
let DirectionCode: string;
let TransportModeCode: string;
let ShipmentTypeCode: string;

Given("User logged in", () => {
  cy.Login();
});

Given("He is in shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});

Given("A shipment details",
  (dataTable) => {
   const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
   LevelCode = shipmentDetails.LevelCode;
   DirectionCode = shipmentDetails.DirectionCode;
   TransportModeCode = shipmentDetails.TransportModeCode;
   ShipmentTypeCode = shipmentDetails.ShipmentTypeCode;
   shipmentActions.OpenNewShipmentWizard(LevelCode);
   shipmentActions.FillShipmentDefaultFields(DirectionCode, TransportModeCode, ShipmentTypeCode);
});

When("He Click create shipment button", () => {
  shipmentActions.CreateShipment();
});

Then("The create operation complete successfully", () => {
  let resultFile = "CreatedShipmentsData/" + LevelCode + DirectionCode + TransportModeCode + ShipmentTypeCode + ".json";
  shipmentAssertions.ValidateCreatedShipment(resultFile);
});