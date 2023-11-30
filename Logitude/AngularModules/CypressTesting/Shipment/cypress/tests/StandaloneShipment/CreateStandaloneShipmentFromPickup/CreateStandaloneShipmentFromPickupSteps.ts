import * as Actions from "../../../actions/Actions";
import * as StandaloneAction from "../../../actions/StandaloneAction";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { PickupDelivaryDetails } from "../../../models/PickupDelivaryDetails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"
import { RestAPI } from '../../../../../Base/cypress/constants/RestAPI';
import { URLs } from '../../../constants/URLs';
import { BaseSelectors } from '../../../../../Base/cypress/selectors/BaseSelectors';
import { ShipmentConstants } from "../../../constants/constants";

let shipmentDetails: ShipmentDetails;
let PickupDelivarytData: PickupDelivaryDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  Actions.FillShipmentWizardsFields(shipmentDetails);
  cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, true)
});

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber
  })
});

Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelectors.RoutingsTab, null)
  cy.Click(ShipmentSelectors.AddPickUp, null)
  cy.Click(BaseSelectors.Button, ShipmentConstants.AddPickUp)
});

Given("unchecked the FullResponsibility", () => {
  cy.Click(ShipmentSelectors.FullResponsibilityCheckBox, null)
})

Given("the user in the shipment's routong tab", () => {
  cy.Click(ShipmentSelectors.CloseBtn, null)
  cy.Click(ShipmentSelectors.RoutingToggle+BaseSelectors.LastElement, null)
  cy.Click(ShipmentSelectors.PickUp+BaseSelectors.LastElement, null)
  cy.Click(BaseSelectors.Button, ShipmentConstants.AddPickUp)
})

Given("add a new pickup leg with the following details", (dataTable) => {
  PickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
  StandaloneAction.FillPickUpDelivaryDetails(PickupDelivarytData);
});

Given("save the pickup", () => {
  cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
  cy.Click(BaseSelectors.SaveButton + BaseSelectors.LastElement, null)
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

When("click create Standalone Shipment", () => {
  cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, ShipmentConstants.CreateStandaloneShipment)
})

When("create standalone shipment", () => {
  cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, ShipmentConstants.CreateStandaloneShipment)
  StandaloneAction.CreateStandaloneShipment()
})

Then("a domestic inland shipment should create", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentStandaloneRequest, 200);
});

Then("the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim", () => {
  cy.Click(BaseSelectors.ToggleButtonClass + BaseSelectors.LastElement, null)
  StandaloneAction.AssertShipmenteMenuButtonsEnabled()
})

Then("all other actions should be dim", () => {
  StandaloneAction.AssertShipmenteMenuButtonsDisabled()
})

Then("all fields should be dim in pickup window", () => {
  cy.Navigate(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, true)
  StandaloneAction.AssertShipmentPickupDelivaryWindowFields(BaseSelectors.BeDisabled)
})

Then("the link of standalon should display", () => {
  BaseAssertion.AssertElementExist(ShipmentSelectors.StandaloneShipmentHyperlink)
})

Then("a validation message with {string} error should appear", (validationMessage) => {
  BaseAssertion.AssertElementContain(BaseSelectors.SingleError, validationMessage)
});

Then("the Create Standalone Shipment button Should be dim", () => {
  BaseAssertion.AssertElementNotVisible(ShipmentSelectors.Printbutton)
})