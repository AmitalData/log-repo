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
import { PackagesDetails } from "cypress/models/PackagesDetails";

let shipmentDetails: ShipmentDetails;
let PickupDelivarytData: PickupDelivaryDetails;
let shipmentNumber: string;


//#region create direct shipment
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
//#endregion

//#region create Standalone shipment from pickup routing 
Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelectors.RoutingsTab, null)
  StandaloneAction.AddStandaloneWithPickupFromRouting()
});

Given("the user in the shipment's routings tab add standalone with pickup", () => {
  cy.Click(ShipmentSelectors.CloseBtn, null)
  StandaloneAction.AddStandaloneWithPickupFromRouting()
})

Given("add Standalone Shipment With Pickup leg with the following details", (dataTable) => {
  PickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
  StandaloneAction.CreateDomesticInlandFromRoutingDetails(PickupDelivarytData);
});

When("create shipment", () => {
  cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
  cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, ShipmentConstants.Create)
})

Then("a domestic inland shipment should create", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Then("the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim", () => {
  cy.Click(BaseSelectors.ToggleButtonClass + BaseSelectors.LastElement, null)
  StandaloneAction.AssertShipmenteMenuButtonsEnabled()
})
Then("all other actions should be dim", () => {
  StandaloneAction.AssertShipmenteMenuButtonsDisabled()
})

Then("the pickup will add all fields should be dim in pickup window", () => {
  cy.Navigate(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, true)
  cy.Click(ShipmentSelectors.EditPickUpNumber + BaseSelectors.LastElement, null)
  StandaloneAction.AssertShipmentPickupDelivaryWindowFields(BaseSelectors.BeDisabled)
})

Then("the link of standalon should display", () => {
  BaseAssertion.AssertElementExist(ShipmentSelectors.StandaloneShipmentHyperlink)
})
//#endregion

//#region create Standalone shipment from delivery routing 
Given("the user in the shipment's routings tab", () => {
  StandaloneAction.AddStandaloneWithDeliveryFromRouting()
})
Given("add Standalone Shipment With delivery leg with the following details", (dataTable) => {
  PickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
  StandaloneAction.CreateDomesticInlandFromRoutingDetails(PickupDelivarytData);
});

Then("the delivery will add all fields should be dim in delivery window", () => {
  cy.Navigate(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, true)
  cy.Click(ShipmentSelectors.EditDeliveryNumber + BaseSelectors.LastElement, null)
  StandaloneAction.AssertShipmentPickupDelivaryWindowFields(BaseSelectors.BeDisabled)
})
//#endregion