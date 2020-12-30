import * as sh from "../../actions/ShipmentActions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

let LevelCode: string;
let DirectionCode: string;
let TransportModeCode: string;
let ShipmentTypeCode: string;

Given("User logged in successfully", () => {
  cy.Login();
});

Given("Open shipments workspace", () => {
  cy.Click("#GeneralMHOperations", null);
  cy.Click("#SHIP", null);
});

Given("{string} shipment with direction {string} and transport mode {string} and type {string}",
(levelCode, directionCode, transportModeCode, shipmentTypeCode) => {
  LevelCode = levelCode;
  DirectionCode = directionCode;
  TransportModeCode = transportModeCode;
  ShipmentTypeCode = shipmentTypeCode;
  sh.OpenNewShipmentWizard(LevelCode);
  sh.FillShipmentDefaultFields(DirectionCode, TransportModeCode, ShipmentTypeCode);
});

When("Click create shipment", () => {
  sh.CreateShipment();
});

Then("The shipment should created successfully", () => {
  let resultFile = "CreatedShipmentsData/" + LevelCode + DirectionCode + TransportModeCode + ShipmentTypeCode + ".json";
  sh.ValidateCreatedShipment(resultFile);
});