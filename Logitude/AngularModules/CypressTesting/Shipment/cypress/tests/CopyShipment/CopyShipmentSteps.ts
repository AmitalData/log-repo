import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
let _ShipmentDetails: ShipmentDetails;
let ShipmentFile;
let CopiedShipmentFile;
Given("user has logged", () => {
  cy.Login();
});
And("the user has gone to the shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});

Given("the user fills the required shipment details with valid data",
  (dataTable) => {
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    _ShipmentDetails = shipmentDetails;
    Actions.OpenNewShipmentWizard(_ShipmentDetails.ShipmentLevel);
    Actions.FillShipmentDefaultFields(_ShipmentDetails);
  });
When("the user Clicks on create shipment button", () => {
  Actions.CreateShipment(_ShipmentDetails.ShipmentLevel);
});
Then("the create operation completed successfully", () => {
  ShipmentFile = "CreatedShipmentsData/" + _ShipmentDetails.ShipmentLevel + _ShipmentDetails.Direction +
    _ShipmentDetails.TransportMode +
    ((typeof _ShipmentDetails.ShipmentType) === "undefined" || _ShipmentDetails.ShipmentType === null ? "" : _ShipmentDetails.ShipmentType) + ".json";
  Assertions.ValidateCreatedShipment(ShipmentFile);
});


Given("the user searches for the required shipment using quick search", () => {
  Actions.OpenShipment(ShipmentFile)
})
And("the user clicks on copy shipment", () => {
  cy.Click(Selectors.MenuButtons, null)
  cy.Click(Selectors.MenuButtons_CopyShipment,null)
});
When("the users clicks on Copy", () => {
  Actions.CreateShipment(_ShipmentDetails.ShipmentLevel);
});
Then("the copy operation completed successfully", () => {
  CopiedShipmentFile = "CreatedShipmentsData/" + _ShipmentDetails.ShipmentLevel + _ShipmentDetails.Direction +
    _ShipmentDetails.TransportMode +
    ((typeof _ShipmentDetails.ShipmentType) === "undefined" || _ShipmentDetails.ShipmentType === null ? "" : _ShipmentDetails.ShipmentType) + "1" + ".json";
  Assertions.ValidateCreatedShipment(CopiedShipmentFile);
  Assertions.ValidateEqualityOfTwoShipments(ShipmentFile, CopiedShipmentFile)
});