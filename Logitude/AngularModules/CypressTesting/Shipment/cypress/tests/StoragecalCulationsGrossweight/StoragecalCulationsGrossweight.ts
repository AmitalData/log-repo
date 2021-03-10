import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as AccountingActions from "../../../../Accounting/cypress/actions/Actions"
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { WarehouseStorage } from "cypress/models/WarehouseStorage";
import { BaseURLs } from "../../../../Base/cypress/constants/URLs";
import { ReceivableDetails } from "cypress/models/ReceivableDetails";
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";
import { AccountingURLs } from "../../../../Accounting/cypress/constants/URLs";
import { URLs } from "../../../../Shipment/cypress/constants/URLs";

let shipmentDetails: ShipmentDetails;

//#region Edit Warehouse
Given("the user logged in and navigate to warehouse workspace", () => {
    cy.Login();
    BaseActions.NavigatesToWarehouse();
});

Given("open warehouse with {string} as name", (warehouseName) => {
    BaseActions.OpenWarehouseWithName(warehouseName);
});

Given("fill with the following details for {string} Type", (type, dataTable) => {
    let warehouseDetails = dataTable.hashes()[0] as WarehouseStorage;
    cy.FillLogLov(BaseSelectors.WarehouseTypeCode, type, true);
    BaseActions.FillWarehouseStorageDetails(warehouseDetails);
});

Given("{string} weight details as following", (transportMode, dataTable) => {
    let warehouseDetails = dataTable.hashes()[0] as WarehouseStorage;
    BaseActions.FillWarehouseStorageWeightDetails(warehouseDetails, transportMode)
});

Given("pricing defaults line as following", (dataTable) => {
    let warehousePricingList = dataTable.hashes() as WarehouseStorage[];
    BaseActions.FillWarehouseStoragePricing(warehousePricingList)

});

When("update warehouse", () => {
    cy.Click(BaseSelectors.RedButton, "Ok")
    cy.DefineRequestWait(RestAPI.PUT, BaseURLs.Warehouses, RequestAliases.PutWarehouses)
    cy.Click(BaseSelectors.WarehouseSaveCloseBtn, null)
});

Then("the warehouse should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWarehouses, 200)
});
//#endregion

//#region Create direct import air shipment
Given("the user in shipment workspace", () => {
    cy.Login();
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

//#region Add Packages
Given("the user open the shipment and navigate to packages workspace", () => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a package with the following details", (dataTable) => {
    let packageDetailsList = dataTable.hashes() as PackagesDetails[];
    Actions.FillPackageTab(shipmentDetails.TransportMode, packageDetailsList, shipmentDetails.ShipmentType);
});

When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Add Warehouse Leg And Check The Calculation
Given("the user in the shipment's rounting tab", () => {
    cy.Navigate(ShipmentSelectors.RoutingsTab);
});

Given("add new warehouse leg with {string} as Termina", (warehouseName) => {
    cy.Click(ShipmentSelectors.AddWarehouse, null);
    cy.FillLogLov(ShipmentSelectors.WarehouseLegId, warehouseName, true);
});

Given("fill {string} as actual release and {string} days ago date as actual entry", (actualRelease, daysToSub) => {
    cy.FillDate(ShipmentSelectors.WarehouseLegActualEntryDate, BaseActions.SubstractDaysFromDate(daysToSub))
    cy.FillDate(ShipmentSelectors.WarehouseLegActualReleaseDate, actualRelease)
});

When("calculate storage", () => {
    Actions.StorageCalculationsButton();
});

Then("the Storage Fee should be {string}", (expectedStorageFeeValue) => {
    BaseAssertion.AssertElementTextEqual(ShipmentSelectors.StorageFeeResult, expectedStorageFeeValue)
});

Then("Storage pricing should have weight {string} and Amount as following", (expectedWeight, dataTable) => {
    let AmountList = dataTable.hashes() as WarehouseStorage[];
    Actions.ValidateStoragePricing(AmountList, expectedWeight);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, BaseSelectors.ContainsOK);
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion 

//#region  add 
Given("the user in the shipment's receivables tab", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
    Actions.OpenShipment("IA1009")
    cy.Navigate(ShipmentSelectors.ReceivablesTab);
});

Given("receivables containts line with the following details", (dataTable) => {
    let receivableDetails = dataTable.hashes()[0] as ReceivableDetails;
    BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol("1", "0"), receivableDetails.ChargesType, BaseSelectors.DivElement)
    BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol("8", "0"), receivableDetails.Amount, BaseSelectors.DivElement)
});

When("add new invoice with {string} vat type and number", (vat) => {
    AccountingActions.CreateARInvoiceGeneratedFromRoutingLeg(vat);
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(AccountingSelectors.ARInvoiceApproveButton, null)
});

Then("the invoice should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200)
});
//#endregion