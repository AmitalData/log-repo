import * as Actions from "../../actions/Actions"
import * as Assertion from "../../actions/Assertion"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { AMANACStatusDetails } from "cypress/models/AMANACStatusDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PackagesDetails } from "../../models/PackagesDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region Variables
let shipmentDetails: ShipmentDetails;
let amanacStatusDetails: AMANACStatusDetails;
let packagesDetails: PackagesDetails[];
//#endregion

//#region AMANAC Setup
Given("the user logged in and navigates to {string} in maintenance menu", (maintenanceSearchValue) => {
    cy.Login()
    Actions.NavigatesTocustomSettingsInMaintenance(maintenanceSearchValue);
});

When("set local customs interface to {string}", (localCustomsInterfaceValue) => {
    Actions.UpdateLocalCustomsInterface(localCustomsInterfaceValue);
});

Then("the AMANAC workspace should appear in operations menu", () => {
    cy.Click(BaseSelectors.OperationsMenu, null);
    BaseAssertion.AssertElementExist(ShipmentSelectors.AMANACTab);
});
//#endregion

//#region Create direct export air shipment
Given("the user in shipment workspace", () => {
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
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

//#region Marked as (not)blocked for transfer
When("the user marke the shipment {string} for transfer in {string} view", (MarkAs, AMANACView) => {
    Actions.NavigatesToAMANACWorkspace();
    Actions.AMANACView(shipmentDetails.TransportMode, AMANACView);
    Actions.AMANACMarkeShipmentAs(MarkAs, shipmentDetails.ShipmentNumber);
});

Then("the shipment should appear in the {string} view in the AMANAC workspace", (AMANACView) => {
    Actions.AMANACView(shipmentDetails.TransportMode, AMANACView);
    //Actions.SearchAShipmentInNullSearch(shipmentDetails.ShipmentNumber);
    BaseAssertion.AssertElementContain(ShipmentSelectors.AMANACShipmentNumber(shipmentDetails.ShipmentNumber), shipmentDetails.ShipmentNumber);
    cy.Click(ShipmentSelectors.CloseAMANACView, null);
});

Then("should not appear in the {string} view in the AMANAC workspace", (AMANACView) => {
    Actions.AMANACView(shipmentDetails.TransportMode, AMANACView);
    //Actions.SearchAShipmentInNullSearch(shipmentDetails.ShipmentNumber);
    BaseAssertion.AssertElementNotExist(ShipmentSelectors.AMANACShipmentNumber(shipmentDetails.ShipmentNumber));
    cy.Click(ShipmentSelectors.CloseAMANACView, null);
});

Then("AMANAC and customs transmissions statuses should be {string}", (AMANACstatus) => {
    Actions.NavigatesToShipmentsWorkspace();
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Assertion.AssertAMANAandCustomsTransmissionsStatuses(AMANACstatus);
    cy.Click(BaseSelectors.Backbutton, null);
});
//#endregion

//#region New transfer before fill all mandatory fields
When("the user export the shipment in {string} view", (AMANACView) => {
    Actions.NavigatesToAMANACWorkspace();
    Actions.AMANACView(shipmentDetails.TransportMode, AMANACView);
    Actions.AMANACExportAShipment(shipmentDetails.ShipmentNumber);
});

Then("a validation message {string} should appear", (ValidationMssage) => {
    BaseAssertion.AssertElementContain(ShipmentSelectors.ValidationMsg, ValidationMssage);
    cy.Click(ShipmentSelectors.CloseExportingScreen, null)
    cy.Click(ShipmentSelectors.CloseAMANACView, null)
});
//#endregion 

//#region Fill/Update mandatory fields
Given("the user add {string} as shipping line", (ShippingLine) => {
    Actions.NavigatesToShipmentsWorkspace()
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.FillShippingLineInOrdersTab(ShippingLine);
});

Given("add package with the following details", (dataTable) => {
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails, shipmentDetails.ShipmentType)
});

Given("edit main carriage leg with {string} as voyage no and {string} as vessel", (VoyageNo, Vessel) => {
    Actions.FillVoyageNoVesselInRoutingsTab(VoyageNo, Vessel);
});

Given("the user edit package with {string} as GrossWeight", (GrossWeight) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.EditPackageGrossWeightPackagesTab(GrossWeight);
});

When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    cy.Click(BaseSelectors.Backbutton, null);
});
//#endregion

//#region New Transfer After fill/update mandatory fields
Then("AMANAC and customs transmissions statuses should be as following", (dataTable) => {
    amanacStatusDetails = Assists.CreateInstance<AMANACStatusDetails>(dataTable, true);
    Actions.NavigatesToShipmentsWorkspace()
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Assertion.AssertAMANAandCustomsTransmissionsStatusDetails(amanacStatusDetails);
    cy.Click(BaseSelectors.Backbutton, null);
});
//#endregion

//#region Retransfer
When("the user retransfer the shipment", () => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.Retransfer();
    cy.Click(BaseSelectors.Backbutton, null);
});
//#endregion

