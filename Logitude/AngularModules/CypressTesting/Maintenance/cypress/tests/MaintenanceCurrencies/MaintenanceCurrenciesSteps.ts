import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as CurrencyActions from "../../actions/CurrencyActions";
import * as Actions from "../../actions/Actions";
import * as GeneralActions from "../../actions/GeneralActions";
import { CurrencySelectors } from "../../../cypress/selectors/CurrencySelectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CurrencyDetails } from "../../../cypress/models/CurrencyDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Urls } from "../../constants/Urls";

//#region create currency already exists
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, CurrencySelectors.MaintenanceItem)
});

Given("navigate currency wizard", () => {
    Actions.OpenNewWizard("Currency");
});

Given("fill the following currency details", (dataTable) => {
    let currencyDetails = Assists.CreateInstance<CurrencyDetails>(dataTable, true)
    CurrencyActions.FillCurrencyDetails(currencyDetails)
});

When("create currency", () => {
    CurrencyActions.MockCreateCurrency(Urls.GetCopyCurrencyToTenant)
});

Then("this validation message error {string} should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region create currency
Then("the currency should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the currency
When("search for {string} currency", (searchFieldValue) => {
    GeneralActions.Search(searchFieldValue)
});

Then("the {string} currency should appear successfully", (searchFieldValue) => {
    GeneralActions.AssertSearch(searchFieldValue)
});
//#endregion

//#region open the currency
When("open currency", () => {
    CurrencyActions.OpenCurrency()
});
Then("the currency should open successfully", () => {
    CurrencyActions.AssertOpenCurrency()
});
//#endregion

//#region Edit the currency
Given("fill {string} as notes currency", (localName) => {
    CurrencyActions.FillCurrencyNotes(localName)
});

Given("the user Check the InActive Currency CheckBox", () => {
    CurrencyActions.CheckInActiveCurrencyCheckBox()
});

Given("fill {string} as Accounting External ID", (externalId) => {
    cy.Navigate(CurrencySelectors.AccountingTab)
    CurrencyActions.FillCurrencyAccountingExternalId(externalId)
});

When("edit currency", () => {
    CurrencyActions.EditCurrency();
});

Then("the currency should update successfully", () => {
    CurrencyActions.AssertEditCurrency();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, CurrencySelectors.EventsTab);
});
//#endregion

//#region save and close currency 
When("save and close currency", () => {
    CurrencyActions.CloseSaveCurrency();
});

Then("the currency should close successfully", () => {
    CurrencyActions.AssertCloseSaveCurrency();
});
//#endregion