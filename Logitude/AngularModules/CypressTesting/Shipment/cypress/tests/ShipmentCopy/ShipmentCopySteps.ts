import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PackagesDetails } from "cypress/models/PackagesDetails";

let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  Actions.FillShipmentWizardsFields(shipmentDetails);
  Actions.FillShipmentCustomFields(shipmentDetails)
});

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});
Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  })
});

//#region Add Packages
Given("the user open the shipment and navigate to packages workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a package with the following details", (dataTable) => {
  let packageDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
  Actions.FillPackageTab(shipmentDetails.TransportMode, packageDetailsList, shipmentDetails.ShipmentType);
});

When("save shipment", () => {
  Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the direct shipment should save successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Copy Direct export air shipment
When("copy the shipment", () => {
  Actions.CopyShipment(shipmentDetails.ShipmentLevel);
});

Then("a shipment copy should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Assert packages tab
When("navigates packages tab", () => {
  cy.Navigate(ShipmentSelectors.ShipmentPackagesTab)
});

Then("the direct shipment should should has the following package details", (dataTable) => {
  let packageDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
  Actions.ValidatePackageDetails(ShipmentSelectors.ShipmentPackagesTab, packageDetailsList[0], ShipmentSelectors.ShipmentGrossWeight, true)
});
//#endregion

//#region Assert routing tab
When("navigates routing tab", () => {
  cy.Navigate(ShipmentSelectors.RoutingTab_1)
});

Then("the pick up has {string} as to port value", (value) => {
  Actions.AssertShipmentRoutingPickUpToPortValue(value)
});

Then("the delivery has {string} as from port value", (value) => {
  Actions.AssertShipmentRoutingDeliveryFromPortValue(value)
});
//#endregion