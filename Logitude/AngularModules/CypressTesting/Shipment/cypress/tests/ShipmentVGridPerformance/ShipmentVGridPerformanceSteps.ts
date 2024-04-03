import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PayableDetails } from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region variables
let shipmentDetails: ShipmentDetails;
let packagesDetails: PackagesDetails[];
let containerDetailsList
//#endregion

//#region Create direct export air shipment Given steps
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
   
   
});
//#endregion
Given("the user open the {string} shipment and navigate to packages workspace", (ShipmentNumber) => {
    Actions.OpenShipment(ShipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});
//#region Update general tab given step
Given("the user fill {string} as ValueOfGoods and {string} as a MoveType", (ValueOfGoods, MoveType) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.FillGeneralTab(ValueOfGoods, MoveType)
});
//#endregion

//#region Update orders tab given step
Given("the user add order package with the following details", (dataTable) => {
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillOrdersTab(packagesDetails)
});

Given("the user delete the first order package", () => {
    Actions.DeleteOrderPackage()
});
//#endregion

//#region Generate packages from order packages
Given("the user generate packages from order packages", () => {
    Actions.GenerateFromOrderPackage()
});
//#endregion

//#region Update packages tab given step
Given("the user add package with the following details", (dataTable) => {
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});

Given("the user delete the package in the packages tabs", () => {
    Actions.DeleteShipmentPackages()
});
//#endregion

//#region Update receivables tab given step
Given("the user fill receivables with the following details", (dataTable) => {
    const ReceivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
    Actions.FillReceivablesTab(ReceivableData)
});
//#endregion

//#region Update routing tab given steps
Given("add transshipments with {string} as via port and {string} as airline", (viaPort, airline) => {
    Actions.AddTransshipments(viaPort, airline)
});

Given("the user add new pickup", () => {
    Actions.FillPickupRouting()
});

Given("add delivery with {string} as a partner routing", (partner) => {
    Actions.FillDeliveryRouting(partner)
});

Given("add pre carriage and on carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting(shipmentDetails.TransportMode, fromPort, toPort)
    Actions.FillOnCarriageRouting(shipmentDetails.TransportMode, fromPort, toPort)
});

Given("add a warehouse with {string} as a terminal", (warehouseLegTerminal) => {
    Actions.AddWarehouseLegPickups(warehouseLegTerminal)
});
//#endregion

//#region  Update payables tab given step
Given("the user add payables with the following details", (dataTable) => {
    const PayableData = Assists.CreateInstance<PayableDetails>(dataTable);
    Actions.FillPayablesTab(PayableData)
});
//#endregion

//#region Actions steps
When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion

//#region Assert steps
Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Then("the direct should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Add Containers


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