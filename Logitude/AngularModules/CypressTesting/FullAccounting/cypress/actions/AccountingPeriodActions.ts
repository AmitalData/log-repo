import { AccountingPeriodSelectors } from "../selectors/AccountingPeriodSelectors";
import { AccountingPeriodDetails } from "../models/AccountingPeriodDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { URLs } from '../constants/URLs';

export function NavigateToAccountingPeriods() {
    // Navigate: Click שונות (Miscellaneous) tab, then click תקופות חשבונאיות from right sidebar
    cy.Click(AccountingPeriodSelectors.MiscellaneousMenu, null, true);
    cy.wait(1000);
    // Click on תקופות חשבונאיות (Accounting Periods) from the right sidebar menu
    cy.Click(BaseSelectors.QueryLink, AccountingPeriodSelectors.AccountingPeriods);
}

export function AssertAccountingPeriodsScreenDisplayed() {
    // Verify the accounting periods screen is displayed
    cy.get(AccountingPeriodSelectors.YearInput).should('be.visible');
}

export function EnterYear(accountingPeriodDetails: AccountingPeriodDetails) {
    cy.DefineRequestWait(RestAPI.GET, URLs.EntityResourceAccountingPeriod, RequestAliases.EntityResourceAccountingPeriod);
    cy.get(AccountingPeriodSelectors.YearInput).clear().type(accountingPeriodDetails.Year);
    cy.Click(AccountingPeriodSelectors.ApproveYearButton, null);
}

export function AssertYearSetSuccessfully() {
    BaseAssertion.AssertStatusCode(RequestAliases.EntityResourceAccountingPeriod, 200);
}

export function OpenClosedMonth(accountingPeriodDetails: AccountingPeriodDetails) {
    // Find the row with the specified period type
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable).should('be.visible');
    cy.get(BaseSelectors.RowClass)
        .contains(accountingPeriodDetails.PeriodType || 'חודש חשבונאי')
        .parents(BaseSelectors.RowClass)
        .find(AccountingPeriodSelectors.EditButton)
        .first()
        .click({ force: true });
    
    // Wait for dialog/window to appear, then click open month
    cy.get(AccountingPeriodSelectors.OpenMonthDialog, { timeout: 10000 }).should('be.visible');
    cy.DefineRequestWait(RestAPI.PUT, URLs.AccountingPeriods, "AccountingPeriodsRequest");
    cy.Click(AccountingPeriodSelectors.OpenMonthButton, null);
    cy.Click(AccountingPeriodSelectors.ConfirmOpenMonthButton, null);
}

export function AssertMonthOpenedSuccessfully() {
    BaseAssertion.AssertStatusCode("AccountingPeriodsRequest", 200);
}

