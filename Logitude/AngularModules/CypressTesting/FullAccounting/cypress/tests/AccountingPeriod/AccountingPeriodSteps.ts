import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as AccountingPeriodActions from '../../actions/AccountingPeriodActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { AccountingPeriodDetails } from '../../models/AccountingPeriodDetails';

//#region Navigate to Accounting Periods
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting();
});

When("navigate to accounting periods", () => {
    AccountingPeriodActions.NavigateToAccountingPeriods();
});

Then("the accounting periods screen should be displayed", () => {
    AccountingPeriodActions.AssertAccountingPeriodsScreenDisplayed();
});
//#endregion

//#region Enter year
Given("the accounting periods screen is displayed", () => {
    AccountingPeriodActions.AssertAccountingPeriodsScreenDisplayed();
});

Given("a year with the following details", (dataTable) => {
    let accountingPeriodDetails = Assists.CreateInstance<AccountingPeriodDetails>(dataTable, true);
    cy.wrap(accountingPeriodDetails).as('yearDetails');
});

When("enter year", () => {
    cy.get('@yearDetails').then((accountingPeriodDetails: any) => {
        AccountingPeriodActions.EnterYear(accountingPeriodDetails);
    });
});

Then("the year should be set successfully", () => {
    AccountingPeriodActions.AssertYearSetSuccessfully();
});
//#endregion

//#region Open closed month
Given("the accounting periods screen is displayed with year", () => {
    AccountingPeriodActions.AssertAccountingPeriodsScreenDisplayed();
});

Given("a closed month with the following details", (dataTable) => {
    let accountingPeriodDetails = Assists.CreateInstance<AccountingPeriodDetails>(dataTable, true);
    cy.wrap(accountingPeriodDetails).as('monthDetails');
});

When("open closed month", () => {
    cy.get('@monthDetails').then((accountingPeriodDetails: any) => {
        AccountingPeriodActions.OpenClosedMonth(accountingPeriodDetails);
    });
});

Then("the month should be opened successfully", () => {
    AccountingPeriodActions.AssertMonthOpenedSuccessfully();
});
//#endregion

