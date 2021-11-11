import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PayableDetails } from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as MaintenanceActions from "../../../../Maintenance/cypress/actions/Actions";
import { MaintenanceSelectors } from "../../../../Maintenance/cypress/selectors/Selectors";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import { URLs } from "../../constants/URLs";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
//#endregion

//#region Update Accounting System
Given("the user logged in and navigates to {string} in maintenance menu", (maintenanceItem) => {
  cy.Login();
  MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItem, MaintenanceSelectors.MaintenanceItemSystemDefaults)
});

Given("{string} as volume unit", (volumeUnit) => {
  cy.SelectDropDownListItem2(BaseSelectors.TenantVolumeUnitCode, volumeUnit)
});

Given("{string} as Gross and Chargeable Weight Unit", (weightUnit) => {
  cy.SelectDropDownListItem2(BaseSelectors.TenantGrossWeightUnitCode, weightUnit)
  cy.SelectDropDownListItem2(BaseSelectors.TenantChargeableWeightUnitCode, weightUnit)
});

When("update system defaults", () => {
  cy.DefineRequestWait(RestAPI.PUT, URLs.Tenants, RequestAliases.Tenants);
  cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
});

Then("the system defaults should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Tenants, 200);
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  Actions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  })
});
//#endregion

//#region Change shipment ratio
Given("open the shipment and navigates packages tab", () => {
  Actions.OpenShipment(shipmentNumber)
  cy.Click(ShipmentSelectors.PackagesTab_Number + BaseSelectors.LastElement, null)
});

Given("the user change the shipment ratio to {string}", (ratioValue) => {
  cy.Click(BaseSelectors.SettingsButton, null)
  cy.FillLogTextBox(ShipmentSelectors.ShipmentRatio, ratioValue)
});
//#endregion

//#region Update packages tab
Given("the user add a package with the following details", (dataTable) => {
  let packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});
//#endregion

//#region Add Payables
Given("the user navigates to payable wizerd", () => {
  cy.Click(ShipmentSelectors.PayablesTab, null)
  cy.Click(ShipmentSelectors.AddNewPayableLine, null)
});

Given("a payable with {string} as a charges type", (chargesType) => {
  cy.FillLogLov(ShipmentSelectors.ShipmentPayableChargesType, chargesType, true);
});

Then("the Quantity should should has {string} as a value", (quantityValue) => {
  BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentPayableQuantity, quantityValue)
});
//#endregion

//#region Change Gross Weight Unit Code
Given("the user navigates to packages tab", () => {
  cy.Click(BaseSelectors.Button + BaseSelectors.LastElement, BaseSelectors.ContainsCancel)
  cy.Click(ShipmentSelectors.PackagesTab, null)
});

Given("a Gross Weight with {string} as a value", (grossWeightUnit) => {
  cy.SelectDropDownListItem2(ShipmentSelectors.ShipmentGrossWeightUnitCode, grossWeightUnit)
});
//#endregion

//#region update shipment step
When("update shipment", () => {
  Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion

//#region update shipment assert step
Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

