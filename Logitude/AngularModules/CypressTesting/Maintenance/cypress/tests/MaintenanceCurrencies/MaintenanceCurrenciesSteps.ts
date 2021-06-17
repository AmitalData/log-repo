import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as CurrencyActions from "../../actions/CurrencyActions";
import { CurrencySelectors } from "../../../cypress/selectors/CurrencySelectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CurrencyDetails } from "../../../cypress/models/CurrencyDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { Constants } from "../../constants/Constants";

let currencyDetails = null

//#region Search for the currency by code 
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCurrency)
});

Given("get currency code", () => {
    CurrencyActions.SelectFirstCurrency()
});

When("search currency", () => {
    CurrencyActions.SearchCurrency()
});

Then("the currency should appear successfully", () => {
    CurrencyActions.AssertSearchCurrency()
});

When("open currency", () => {
    CurrencyActions.OpenCurrency();
});
Then("the currency should open successfully", () => {
    CurrencyActions.AssertOpenCurrency();
});

//#endregion

//#region Edit the currency
Given("the user fill the following currency details", (dataTable) => {
    currencyDetails = Assists.CreateInstance<CurrencyDetails>(dataTable, true);
    let LocalName = currencyDetails.LocalName
    CurrencyActions.FillCurrencyLocalName(LocalName)
});

Given("the user active or inactive currency", () => {
    CurrencyActions.ChangeInactiveCheckBoxValue(CurrencySelectors.InActiveCurrencyCheckBox)
});

Given("fill the following currency Accounting External ID", (dataTable) => {
    let currencyDetails = Assists.CreateInstance<CurrencyDetails>(dataTable, true);
    cy.Navigate(CurrencySelectors.AccountingTab);
    CurrencyActions.FillCurrencyAccountingTab(currencyDetails)
});

When("edit currency", () => {
    CurrencyActions.EditCurrency();
});

Then("the currency should update successfully", () => {
    CurrencyActions.AssertEditCurrency();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    CurrencyActions.AssertEventTab(eventDetailsList)
});

When("save and close currency", () => {
    CurrencyActions.CloseSaveCurrency();
});

Then("the currency should close successfully", () => {
    CurrencyActions.AssertCloseSaveCurrency();
});

Given("a currency with the following details", (dataTable) => {
    currencyDetails = Assists.CreateInstance<CurrencyDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.Currency);
    CurrencyActions.FillCurrencyDetails(currencyDetails) 
});

When("create currency", () => {
    CurrencyActions.CreateExitingCurrency();
});

Then("the currency should not create successfully", () => {
    CurrencyActions.AssertFaildCreateCurrency();
});
