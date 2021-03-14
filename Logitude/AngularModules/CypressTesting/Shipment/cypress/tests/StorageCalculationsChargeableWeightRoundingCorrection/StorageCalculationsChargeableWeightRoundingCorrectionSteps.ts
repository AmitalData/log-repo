import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PackagesDetails } from "cypress/models/PackagesDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as AccountingActions from "../../../../Accounting/cypress/actions/Actions"
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { WarehouseStorage } from "cypress/models/WarehouseStorage";
import { ReceivableDetails } from "cypress/models/ReceivableDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";
import { ARInvoiceDetails } from "../../../../Accounting/cypress/models/ARInvoiceDetails";

//#region variables
let shipmentDetails: ShipmentDetails;
//#endregion

//#region Set up a warehouse with storage charges
Given("the user logged in and navigate to warehouse workspace", () => {
    cy.Login();
    BaseActions.NavigatesToWarehouse();
});

Given("open {string} warehouse", (warehouseName) => {
    BaseActions.OpenWarehouse(warehouseName);
});

Given("the following storage details for {string} type", (type, dataTable) => {
    let warehouseDetails = Assists.CreateInstance<WarehouseStorage>(dataTable, false);
    cy.FillLogLov(BaseSelectors.WarehouseTypeCode, type, true);
    BaseActions.FillWarehouseStorageDetails(warehouseDetails);
});

Given("the following {string} weight details", (transportMode, dataTable) => {
    let warehouseDetails = Assists.CreateInstance<WarehouseStorage>(dataTable, false);
    BaseActions.FillWarehouseStorageWeightDetails(warehouseDetails, transportMode);
});

Given("the following pricing defaults lines", (dataTable) => {
    let warehousePricingList = Assists.CreateSet<WarehouseStorage>(dataTable);
    BaseActions.FillWarehouseStoragePricing(warehousePricingList);
});

When("update warehouse", () => {
    BaseActions.UpdateWarehouse();
});

Then("the warehouse should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWarehouses, 200);
});
//#endregion

//#region Create direct import ocean shipment
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

//#region Add a container with chargeable weight
Given("the user open the shipment and navigate to packages tab", () => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click(ShipmentSelectors.PackagesTab, null)
});

Given("a container with the following details", (dataTable) => {
    let packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails, shipmentDetails.ShipmentType);
});

When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
});

Then("the shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Add/Update the warehouse with dates for storage calculation
Given("the user in the shipment's rounting tab", () => {
    cy.Navigate(ShipmentSelectors.RoutingsTab);
});

Given("a warehouse leg with {string} as terminal", (warehouseName) => {
    cy.Click(ShipmentSelectors.AddWarehouse, null);
    cy.FillLogLov(ShipmentSelectors.WarehouseLeg, warehouseName, true);
});

Given("{string} as actual entry and expected release after {string} days", (actualEntry, daysToAddForExpectedRelease) => {
    cy.FillDate(ShipmentSelectors.WarehouseLegActualEntryDate, actualEntry)
    cy.FillDate(ShipmentSelectors.WarehouseLegExpectedReleaseDate, BaseActions.AddDaysToTodayDate(parseInt(daysToAddForExpectedRelease, 10)))
});

Given("edit the expected release date to be after {string} days", (daysToAddForExpectedRelease) => {
    cy.Click(ShipmentSelectors.EditWarehouseLeg, null)
    cy.FillDate(ShipmentSelectors.WarehouseLegExpectedReleaseDate, BaseActions.AddDaysToTodayDate(parseInt(daysToAddForExpectedRelease, 10)))
});

When("calculate storage", () => {
    Actions.CalculateStorage();
});

Then("storage fee should be {string}", (expectedStorageFeeValue) => {
    BaseAssertion.AssertElementTextEqual(ShipmentSelectors.StorageFeeResult, expectedStorageFeeValue)
});

Then("storage pricing should have weight {string} with the following amounts", (expectedWeight, dataTable) => {
    let AmountsList = Assists.CreateSet<WarehouseStorage>(dataTable);
    Actions.ValidateStoragePricing(AmountsList, expectedWeight);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, BaseSelectors.ContainsOK);
});

Then("a receivables line with the following details should appear", (dataTable) => {
    cy.Click(ShipmentSelectors.ReceivablesTab, null);
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);

    let receivableDetails = Assists.CreateInstance<ReceivableDetails>(dataTable, false);
    BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol(BaseSelectors.ColNo1, BaseSelectors.RowNo0) + BaseSelectors.LastElement, receivableDetails.ChargesType, BaseSelectors.DivElement)
    BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol(BaseSelectors.ColNo8, BaseSelectors.RowNo0) + BaseSelectors.LastElement, receivableDetails.Amount, BaseSelectors.DivElement)
});

Then("a second receivables line with the following details should appear", (dataTable) => {
    cy.Click(ShipmentSelectors.ReceivablesTab, null);
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);

    let receivableDetails = Assists.CreateInstance<ReceivableDetails>(dataTable, false);
    BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol(BaseSelectors.ColNo1, BaseSelectors.RowNo1) + BaseSelectors.LastElement, receivableDetails.ChargesType, BaseSelectors.DivElement)
    BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol(BaseSelectors.ColNo8, BaseSelectors.RowNo1) + BaseSelectors.LastElement, receivableDetails.Amount, BaseSelectors.DivElement)
});
//#endregion 

//#region Create an ARInvoice
Given("an ARInvoice with the following details", (dataTable) => {
    const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(ARInvoiceData)
});

When("create invoice", () => {
    AccountingActions.CreateARInvoice()
});

Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
    cy.BackButton(BaseSelectors.ContainsShipment + shipmentDetails.ShipmentNumber);
});
//#endregion