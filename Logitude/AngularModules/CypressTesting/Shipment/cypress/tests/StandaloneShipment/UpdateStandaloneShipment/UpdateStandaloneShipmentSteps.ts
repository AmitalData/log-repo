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
import { StandaloneRoutingDetails } from "../../../models/StandaloneRoutingDetails";
let shipmentDetails: ShipmentDetails;
let PickupDelivarytData: PickupDelivaryDetails;
let shipmentNumber: string;
let StandaloneShipmenNumber: string;

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

//#region create Standalone shipment
Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
    Actions.OpenShipment(shipmentNumber);
    StandaloneAction.OpenRoutingTabAddPickUp()
});

Given("add a new pickup leg with the following details", (dataTable) => {
    PickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
    StandaloneAction.FillPickUpDelivaryDetails(PickupDelivarytData);
});

Given("save the pickup", () => {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(BaseSelectors.SaveButton + BaseSelectors.LastElement, null)
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

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
    cy.Click(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, null, true)
    StandaloneAction.AssertShipmentPickupDelivaryWindowFields(BaseSelectors.BeDisabled)
})

Then("the link of standalon should display", () => {
    BaseAssertion.AssertElementExist(ShipmentSelectors.StandaloneShipmentHyperlink)
})
//#endregion

//#region Update Standalone
Given("the user in the standalone shipment routings tab", () => {
    cy.Click(BaseSelectors.HyperlinkButtonControl, StandaloneShipmenNumber)
    cy.Click(ShipmentSelectors.RoutingsTab_Number + BaseSelectors.LastElement, null)
})

Given("edit main carriage leg with the following details", (dataTable) => {
    let standaloneroutingdetails = Assists.CreateInstance<StandaloneRoutingDetails>(dataTable, true);
    StandaloneAction.FillStandaloneRoutingDetails(standaloneroutingdetails)
})

When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton_Number + BaseSelectors.LastElement)
});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    cy.Click(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, null, true)
});

Then("the pickup window should update successfully with the following details", (dataTable) => {
    let standaloneroutingdetails = Assists.CreateInstance<StandaloneRoutingDetails>(dataTable, true);
    StandaloneAction.AsseratAllFieldsInPickUpWindowField(standaloneroutingdetails)
})
//#endregion