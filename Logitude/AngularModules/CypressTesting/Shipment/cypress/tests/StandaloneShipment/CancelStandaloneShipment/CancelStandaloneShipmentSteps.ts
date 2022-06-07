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
let pickupDelivarytData: PickupDelivaryDetails;
let shipmentNumber: string;
let standaloneShipmentNumber: string;

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

Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelectors.RoutingsTab, null)
  cy.Click(ShipmentSelectors.AddPickUp, null)
});

Given("add a new pickup leg with the following details", (dataTable) => {
  cy.Click(BaseSelectors.Button, ShipmentConstants.AddPickUp)
  pickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
  StandaloneAction.FillPickUpDelivaryDetails(pickupDelivarytData);
});

Given("save the pickup", () => {
  cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
  cy.Click(BaseSelectors.SaveButton + BaseSelectors.LastElement, null)
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Given("the user in standalone shipment", () => {
  cy.Click(BaseSelectors.HyperlinkButtonControl+BaseSelectors.LastElement, standaloneShipmentNumber, true)
})

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("cancel the standalone shipment with {string} Note", (note) => {
  Actions.CancelShipment(note);
});

When("create standalone shipment", () => {
  cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, ShipmentConstants.CreateStandaloneShipment)
  StandaloneAction.CreateStandaloneShipment()
})

Then("the shipment should cancel successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  Actions.ValidateCancelIconExist(true);
});

Then("the shipment should not connected with pickup", () => {
  cy.Click(ShipmentSelectors.ConnectionsTab + BaseSelectors.LastElement, null, true);
  cy.Navigate(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, true)
})

Then("all fiellds in pickup should not be dim", () => {
  StandaloneAction.AssertShipmentPickupDelivaryWindowFields(BaseSelectors.NotBeDisabled)
})

Then("the shipment should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber
  })
});

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