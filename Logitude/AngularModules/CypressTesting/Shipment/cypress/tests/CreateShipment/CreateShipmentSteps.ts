import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
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

Given("Go to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});

Given("Shipment details",
  (dataTable) => {
   const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
   LevelCode = shipmentDetails.LevelCode;
   DirectionCode = shipmentDetails.DirectionCode;
   TransportModeCode = shipmentDetails.TransportModeCode;
   ShipmentTypeCode = shipmentDetails.ShipmentTypeCode;
   Actions.OpenNewShipmentWizard(LevelCode);
   Actions.FillShipmentDefaultFields(LevelCode, DirectionCode, TransportModeCode, ShipmentTypeCode);
});

When("Click create shipment button", () => {
  Actions.CreateShipment();
});

Then("The create operation completed successfully", () => {
  let resultFile = "CreatedShipmentsData/" + LevelCode + DirectionCode + TransportModeCode + ShipmentTypeCode + ".json";
  Assertions.ValidateCreatedShipment(resultFile);
});