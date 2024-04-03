import * as Actions from "../../../actions/Actions";
import * as StandaloneAction from "../../../actions/StandaloneAction";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { PickupDelivaryDetails } from "../../../models/PickupDelivaryDetails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"
import { BaseSelectors } from '../../../../../Base/cypress/selectors/BaseSelectors';
import { ShipmentConstants } from "../../../constants/constants";
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { RestAPI } from '../../../../../Base/cypress/constants/RestAPI';
import { URLs } from '../../../constants/URLs';

let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let containerDetailsList

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

//#region Add Packages
Given("the user open the shipment and navigate to packages workspace", () => {
    Actions.OpenShipment(shipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a container with the following details", (dataTable) => {
    containerDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, containerDetailsList, shipmentDetails.ShipmentType);
});

When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Create Standalone
Given("navigate to RoutingsTab workspace", () => {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.AddPickUp, null)
    cy.Click(BaseSelectors.Button, ShipmentConstants.AddPickUp)
});

Given("add a new pickup leg with the following details", (dataTable) => {
    let PickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
    StandaloneAction.FillPickUpDelivaryDetails(PickupDelivarytData);
});

Given("the user in the pickup packages select the container", () => {
    cy.Click(ShipmentSelectors.PickUpDeliveryPackages, null)
    cy.Click(ShipmentSelectors.AddContainerFromPickup, null, true)
    cy.get(ShipmentSelectors.LogitudeCheckBox).eq(1).click();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
})

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
    cy.Navigate(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, true)
    cy.Click(ShipmentSelectors.ShipmentPickupDelivaryMain, null)
    StandaloneAction.AssertShipmentPickupDelivaryWindowFields(BaseSelectors.BeDisabled)
})

Then("the link of standalon should display", () => {
    BaseAssertion.AssertElementExist(ShipmentSelectors.StandaloneShipmentHyperlink)
})
//#endregion

//#region Delete the container from standalone shipment
Given("the user in the standalone shipment Packages tab", () => {
    cy.Click(BaseSelectors.HyperlinkButtonControl + BaseSelectors.LastElement, null)
    cy.Click(ShipmentSelectors.PackagesTab_Number + BaseSelectors.LastElement, null)
})
Given("click delete button", () => {
    cy.Click(BaseSelectors.DeleteButton + BaseSelectors.LastElement, null)
    cy.Click(BaseSelectors.ConfirmWindowButton + BaseSelectors.LastElement, null)
})

When("save Standalone shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton_Number + BaseSelectors.LastElement)
});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
Then("the container delete from pickup leg packages tab",()=>{
    cy.Navigate(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, true)
    cy.Click(ShipmentSelectors.PickUpDeliveryPackages, null)
    BaseAssertion.AssertElementNotExist(BaseSelectors.RowsInPickUpPackages)
})

Then("in forwarder shipment the container appear",()=>{
    cy.Click(ShipmentSelectors.CloseBtn, null)
    cy.Navigate(ShipmentSelectors.PackagesTab);
    BaseAssertion.AssertElementExist(BaseSelectors.Row0)   
})
//#endregion