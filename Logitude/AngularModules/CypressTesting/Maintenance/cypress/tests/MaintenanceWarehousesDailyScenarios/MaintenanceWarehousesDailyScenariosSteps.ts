import * as Actions from "../../actions/Actions";
import * as WarehouseActions from "../../actions/WarehouseActions";
import * as GeneralActions from "../../actions/BaseActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { WarehousesSelectors } from "../../selectors/WarehousesSelectors";
import { CardDetails } from "cypress/models/CardDetails";
import { ContactDetails } from "../../models/ContactDetails";
import { CardGeneralTabDetails } from "cypress/models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "cypress/models/CardBillingTabDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { Urls } from "../../constants/Urls";


//#region Add warehouse code with lenght more than 5
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, WarehousesSelectors.MaintenanceItem)
});

When("add {string} as warehouse code", (warehouseCode) => {
    Actions.OpenNewWizard("Warehouse");
    WarehouseActions.FillWarehouseCode(warehouseCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Create new warehouse
Given("a warehouse with the following details", (dataTable) => {
    let warehouseDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    WarehouseActions.FillWarehouseDetails(warehouseDetails);
});

Given("a warehouse contact with the following details", (dataTable) => {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    WarehouseActions.FillWarehouseContactDetails(contactDetails)
});

When("create warehouse", () => {
    GeneralActions.MockCreate(Urls.Warehouses);
});

Then("the warehouse should create successfully", () => {
    GeneralActions.AssertMockCreate();
});
//#endregion

//#region Search for the warehouse
When("search for {string} warehouse", (searchFieldValue) => {
    Actions.SearchCardByValue(searchFieldValue)
});

Then("the {string} warehouse should appear successfully", (searchFieldValue) => {
    Actions.AssertSearchCard(searchFieldValue)
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

//#region Add warehouse terminal code with lenght more than 25
When("add {string} as warehouse terminal code", (warehouseTerminalCode) => {
    WarehouseActions.FillWarehouseTerminalCode(warehouseTerminalCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
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