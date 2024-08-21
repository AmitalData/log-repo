import * as Actions from "../../actions/Actions";
import * as ChartOfAccountsActions from "../../actions/ChartOfAccountsActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ChartOfAccountsSelectors } from "../../selectors/ChartOfAccountsSelectors";
import { ChartOfAccountsDetails } from "cypress/models/ChartOfAccountsDetails";

let searchFieldValue = null

//#region Create new business unit
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, ChartOfAccountsSelectors.MaintenanceItem)
});

Given("a Chart of Account with the following details", (dataTable) => {
    let chartOfAccountsDetails = Assists.CreateInstance<ChartOfAccountsDetails>(dataTable, true);
    Actions.OpenNewWizard("ChartOfAccount");
    ChartOfAccountsActions.FillChartOfAccountDetails(chartOfAccountsDetails, 5);
});

When("create Chart of Account", () => {
    ChartOfAccountsActions.CreateChartOfAccount();
});

Then("the Chart of Account should create successfully", () => {
    ChartOfAccountsActions.AssertCreateChartOfAccount();
});
//#endregion

//#region Search for the Chart of Account
When("search Chart of Account", () => {
    searchFieldValue = ChartOfAccountsActions.GetSearchFieldValue()
    Actions.Search(searchFieldValue)
});

Then("the Chart of Account should appear successfully", () => {
    Actions.AssertSearch(searchFieldValue);
});
//#endregion

//#region Open the Chart of Account
When("open Chart of Account", () => {
    ChartOfAccountsActions.OpenChartOfAccount();
});

Then("the Chart of Account should open successfully", () => {
    Actions.AssertGetSingle();
});
//#endregion

//#region Edit the Chart of Account
Given("the user edit the Chart of Account", () => {
    ChartOfAccountsActions.EditChartOfAccount()
});

When("save Chart of Account", () => {
    ChartOfAccountsActions.SaveChartOfAccount()
});

Then("the Chart of Account should update successfully", () => {
    ChartOfAccountsActions.AssertSaveChartOfAccount()
});
//#endregion