import * as Actions from "../../actions/Actions";
import * as WarehouseActions from "../../actions/WarehouseActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { WarehousesSelectors } from "../../selectors/WarehousesSelectors";
import { CardDetails } from "cypress/models/CardDetails";
import { ContactDetails } from "../../models/ContactDetails";
import { CardGeneralTabDetails } from "cypress/models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "cypress/models/CardBillingTabDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";

let warehouseDetails: CardDetails
let code = null
//#region Create new warehouse
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, WarehousesSelectors.MaintenanceItem)
});

Given("a warehouse with the following details", (dataTable) => {
    warehouseDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    Actions.OpenNewWizard("Warehouse");
    WarehouseActions.FillWarehouseDetails(warehouseDetails);
});

Given("a warehouse contact with the following details", (dataTable) => {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    WarehouseActions.FillWarehouseContactDetails(contactDetails)
});

When("create warehouse", () => {
    WarehouseActions.CreateWarehouse();
});

Then("the warehouse should create successfully", () => {
    WarehouseActions.AssertCreateWarehouse();
});
//#endregion

//#region Search for the warehouse
When("search warehouse", () => {
    WarehouseActions.SearchWarehouse()
});

Then("the warehouse should appear successfully", () => {
    WarehouseActions.AssertSearchWarehouse()
});
//#endregion

//#region Open the warehouse
When("open warehouse", () => {
    WarehouseActions.OpenWarehouse();
});

Then("the warehouse should open successfully", () => {
    WarehouseActions.AssertOpenWarehouse();
});
//#endregion

//#region Edit the warehouse
Given("the user fill the following warehouse details", (dataTable) => {
    let warehouseDetails = Assists.CreateInstance<CardGeneralTabDetails>(dataTable, true);
    WarehouseActions.FillWarehouseGeneralTab(warehouseDetails)
});

Given("fill the following warehouse Billing details", (dataTable) => {
    let warehouseBillingDetails = Assists.CreateInstance<CardBillingTabDetails>(dataTable, true);
    cy.Navigate(WarehousesSelectors.BillingTab);
    WarehouseActions.FillWarehouseBillingTab(warehouseBillingDetails);
});

When("save warehouse", () => {
    WarehouseActions.UpdateWarehouse()
});

Then("the warehouse should update successfully", () => {
    WarehouseActions.AssertUpdateWarehouse()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, WarehousesSelectors.EventsTab);
});
//#endregion

//#region Save and close the warehouse
When("save and close warehouse", () => {
    WarehouseActions.CloseSaveWarehouse();
});

Then("the warehouse should close successfully", () => {
    WarehouseActions.AssertCloseSaveWarehouse();
});
 //#endregion