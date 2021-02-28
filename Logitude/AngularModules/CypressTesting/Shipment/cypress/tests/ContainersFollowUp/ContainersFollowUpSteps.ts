import * as Actions from "../../actions/Actions";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { PackagesDetails } from "../../models/PackagesDetails";

//#region variables
let shipmentDetails: ShipmentDetails;
let packagesDetails: PackagesDetails[];
//#endregion

//#region Create import ocean FCL shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Add a container
Given("the user add a container with the following details", (dataTable) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    packagesDetails = dataTable.hashes() as PackagesDetails[];
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails, shipmentDetails.ShipmentType)
});
//#endregion

//#region Add main carriage ATD/ATA
Given("edit main carriage leg ATD to {string} at {string}", (date, time) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    Actions.AddMainCarriageATDDateAndTime(date, time);
});

Given("edit main carriage leg ATA to {string} at {string}", (date, time) => {
    Actions.NavigatesToShipmentsWorkspace()
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    Actions.AddMainCarriageATADateAndTime(date, time);
});
//#endregion

When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Then("it's status is {string}", (shipmentStatus) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, shipmentStatus)
    cy.Click(BaseSelectors.Backbutton, null);
});

Then("the container should appear in the {string} view", (containerView) => {
    Actions.NavigatesToAContainersWorkspace()
    Actions.ContainersView(containerView)

    cy.FillLogTextBox(BaseSelectors.SearchField, packagesDetails[0].ContainerNumber);
    cy.DefineRequestWait("GET", "**/ContainerFollowUpViews/getbyfilters?**", RequestAliases.CustomerViews)
    BaseAssertion.AssertStatusCode(RequestAliases.CustomerViews, 200)
    cy.DefineRequestWait("GET", "**/ContainerFollowUpViews/getbyfilters?**", RequestAliases.CustomerViews)
    BaseAssertion.AssertStatusCode(RequestAliases.CustomerViews, 200)
    //cy.Click(BaseSelectors.ListItem, null, false)

    BaseAssertion.AssertElementContain(".cdk-virtual-scroll-content-wrapper", packagesDetails[0].ContainerNumber)
    cy.Click(BaseSelectors.Backbutton, null);

});

Then("the container should not appear in the {string} view", (containerView) => {
    Actions.NavigatesToAContainersWorkspace()
    Actions.ContainersView(containerView)

    cy.FillLogTextBox(BaseSelectors.SearchField, packagesDetails[0].ContainerNumber);
    cy.DefineRequestWait("GET", "**/ContainerFollowUpViews/getbyfilters?**", RequestAliases.CustomerViews)
    BaseAssertion.AssertStatusCode(RequestAliases.CustomerViews, 200)
    cy.DefineRequestWait("GET", "**/ContainerFollowUpViews/getbyfilters?**", RequestAliases.CustomerViews)
    BaseAssertion.AssertStatusCode(RequestAliases.CustomerViews, 200)
    //cy.Click(BaseSelectors.ListItem, null, false)

    BaseAssertion.AssertElementContain(".cdk-virtual-scroll-content-wrapper", packagesDetails[0].ContainerNumber)
    cy.Click(BaseSelectors.Backbutton, null);
});