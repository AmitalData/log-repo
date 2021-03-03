import * as Actions from '../../actions/Actions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { PackagesDetails } from '../../models/PackagesDetails';

//#region variables
let shipmentDetails: ShipmentDetails;
let packagesDetails: PackagesDetails[];
//#endregion

//#region Create import ocean FCL shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login();
    Actions.NavigatesToShipmentsWorkspace();
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
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails, shipmentDetails.ShipmentType);
});
//#endregion

//#region Add main carriage ATD/ATA
Given("edit main carriage leg ATD to {string} at {string}", (date, time) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null);
    Actions.AddMainCarriageATDDateAndTime(date, time);
});

Given("edit main carriage leg ATA to {string} at {string}", (date, time) => {
    Actions.NavigatesToShipmentsWorkspace()
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    Actions.AddMainCarriageATADateAndTime(date, time);
});
//#endregion

//#region Add/Edit delivery/empty container return follow up
Given("the user add a delivery follow up with {string} at {string} as actual departure", (date, time) => {
    Actions.NavigatesToShipmentsWorkspace();
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.AddDeliveryFollowUpActualDepartureDateAndTime(date, time, packagesDetails[0].ContainerNumber);
});

Given("the user edit a delivery follow up with {string} at {string} as actual arrival", (date, time) => {
    Actions.NavigatesToShipmentsWorkspace();
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.AddDeliveryActualArrivalDateAndTime(date, time, packagesDetails[0].ContainerNumber);
});

Given("the user add an empty container return follow up with {string} at {string} as actual departure", (date, time) => {
    Actions.NavigatesToShipmentsWorkspace();
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.AddContainerReturnFollowUpActualDepartureDateAndTime(date, time, packagesDetails[0].ContainerNumber);
});

Given("the user edit an empty container return follow up with {string} at {string} as actual arrival", (date, time) => {
    Actions.NavigatesToShipmentsWorkspace();
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.AddContainerReturnActualArrivalDateAndTime(date, time, packagesDetails[0].ContainerNumber);
});
//#endregion

//#region Update 
When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
});

When("update follow up", () => {
    Actions.UpdateFollowUp();
});
//#endregion

//#region Assertions 
Then("the follow up should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    cy.Click(BaseSelectors.Backbutton, null);
});

Then("the shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Then("it's status is {string}", (shipmentStatus) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, shipmentStatus)
    cy.Click(BaseSelectors.Backbutton, null);
});

Then("the container should appear in the {string} view", (containerView) => {
    Actions.NavigatesToAContainersWorkspace();
    Actions.ContainersView(containerView);
    Actions.SearchAContainer(packagesDetails[0].ContainerNumber);
    BaseAssertion.AssertElementContain(ShipmentSelectors.GridFitstRow(), packagesDetails[0].ContainerNumber);
    cy.Click(BaseSelectors.Backbutton, null);
});

Then("the container should not appear in the {string} view", (containerView) => {
    Actions.NavigatesToAContainersWorkspace()
    Actions.ContainersView(containerView)
    Actions.SearchAContainer(packagesDetails[0].ContainerNumber);
    BaseAssertion.AssertElementNotExist(ShipmentSelectors.GridFitstRow())
    cy.Click(BaseSelectors.Backbutton, null);
});
//#endregion