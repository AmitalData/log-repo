import * as Actions from "../../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { DelivaryDeteails } from "../../../models/DelivaryDeteails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"

let ShipmentData: ShipmentDetails;
let DelivarytData: DelivaryDeteails;
let shipmentNumber: string;
Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  ShipmentData = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
  Actions.FillShipmentWizardsFields(ShipmentData);
});

When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Navigate(ShipmentSelectors.RoutingsTab);
});

Given("add a new Delivery leg with the following details", (dataTable) => {
  DelivarytData = Assists.CreateInstance<DelivaryDeteails>(dataTable, true);
  Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
  Actions.FillShipmentWizardsFields(ShipmentData);
});







































 



