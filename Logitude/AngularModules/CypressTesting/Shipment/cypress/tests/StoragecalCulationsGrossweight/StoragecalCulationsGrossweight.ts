import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as AccountingActions from "../../../../Accounting/cypress/actions/Actions"
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { WarehouseStorage } from "cypress/models/WarehouseStorage";
import { ReceivableDetails } from "cypress/models/ReceivableDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";
import { ARInvoiceDetails } from "../../../../Accounting/cypress/models/ARInvoiceDetails";


let shipmentDetails: ShipmentDetails;

//#region Edit Warehouse
Given("the user logged in and navigate to warehouse workspace", () => {
    cy.Login();
    BaseActions.NavigatesToWarehouse();
});

Given("open warehouse with {string} warehouse", (warehouseName) => {
    BaseActions.OpenWarehouse(warehouseName);
});

Given("fill with the following storage details for {string} Type", (type, dataTable) => {
    let warehouseDetails = Assists.CreateInstance<WarehouseStorage>(dataTable, true);
    cy.FillLogLov(BaseSelectors.WarehouseTypeCode, type, true);
    BaseActions.FillWarehouseStorageDetails(warehouseDetails);
});

Given("{string} weight details as following", (transportMode, dataTable) => {
    let warehouseDetails = Assists.CreateInstance<WarehouseStorage>(dataTable, true);
    BaseActions.FillWarehouseStorageWeightDetails(warehouseDetails, transportMode)
});

Given("pricing defaults lines as following", (dataTable) => {
    let warehousePricingList = Assists.CreateSet<WarehouseStorage>(dataTable);
    BaseActions.FillWarehouseStoragePricing(warehousePricingList)
});

When("update warehouse", () => {
    BaseActions.UpdateWarehouse()
});

Then("the warehouse should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWarehouses, 200)
});
//#endregion

//#region Create direct import air shipment
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

//#region Add Packages
Given("the user open the shipment and navigate to packages workspace", () => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a package with the following details", (dataTable) => {
    let packageDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, packageDetailsList, shipmentDetails.ShipmentType);
});

When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the direct shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Add Warehouse Leg And Check The Calculation
Given("the user in the shipment's rounting tab", () => {
    cy.Navigate(ShipmentSelectors.RoutingsTab);
});

Given("a warehouse leg with {string} as terminal", (warehouseName) => {
    cy.Click(ShipmentSelectors.AddWarehouse, null);
    cy.FillLogLov(ShipmentSelectors.WarehouseLeg, warehouseName, true);
});

Given("fill {string} as actual release and {string} days ago date as actual entry", (actualRelease, daysToSub) => {
    cy.FillDate(ShipmentSelectors.WarehouseLegActualEntryDate, BaseActions.SubstractDaysFromDate(daysToSub))
    cy.FillDate(ShipmentSelectors.WarehouseLegActualReleaseDate, actualRelease)
});

When("calculate storage", () => {
    Actions.CalculateStorage();
});

Then("the Storage Fee should be {string}", (expectedStorageFeeValue) => {
    // BaseAssertion.AssertElementTextEqual(ShipmentSelectors.StorageFeeResult, expectedStorageFeeValue)
    Actions.AssertStorageFee(expectedStorageFeeValue);
});

Then("Storage pricing should have weight {string} and Amount as following", (expectedWeight, dataTable) => {
    let AmountList = Assists.CreateSet<WarehouseStorage>(dataTable);
    Actions.ValidateStoragePricing(AmountList, expectedWeight);
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