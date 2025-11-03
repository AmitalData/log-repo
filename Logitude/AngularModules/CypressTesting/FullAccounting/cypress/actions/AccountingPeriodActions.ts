import { AccountingPeriodSelectors } from "../selectors/AccountingPeriodSelectors";
import { AccountingPeriodDetails } from "../models/AccountingPeriodDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { URLs } from '../constants/URLs';

export function NavigateToAccountingPeriods() {
    // Navigate: שונות > הגדרת תקופה חשבונאית > תקופות חשבונאיות
    // Step 1: Click שונות (Miscellaneous) tab in the top navigation
    cy.contains('שונות').should('be.visible').click({ force: true });
    
    // Step 2: Wait for הגדרת תקופות (Define Periods) to appear and click it
    cy.contains(AccountingPeriodSelectors.DefineAccountingPeriod, { timeout: 5000 })
        .should('be.visible')
        .click({ force: true });
    
    // Step 3: Click תקופות חשבונאיות (Accounting Periods)
    cy.contains(AccountingPeriodSelectors.AccountingPeriods, { timeout: 5000 })
        .should('be.visible')
        .click({ force: true });
}

export function AssertAccountingPeriodsScreenDisplayed() {
    // Verify the accounting periods screen is displayed
    cy.get(AccountingPeriodSelectors.YearInput).should('be.visible');
}

export function EnterYear(accountingPeriodDetails: AccountingPeriodDetails) {
    cy.get(AccountingPeriodSelectors.YearInput).should('be.visible').clear().type(accountingPeriodDetails.Year);
    // Click the אישור (Approve) button after entering the year
    cy.contains('button', 'אישור').should('be.visible').click({ force: true });
    // Wait for the table to appear (much faster than waiting for API)
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable, { timeout: 5000 }).should('be.visible');
}

export function AssertYearSetSuccessfully() {
    // Verify the table is loaded instead of waiting for API
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable).should('be.visible');
    cy.get(BaseSelectors.RowClass).should('have.length.at.least', 1);
}

export function OpenClosedMonth(accountingPeriodDetails: AccountingPeriodDetails) {
    // Wait for the table to load after year was entered and approved
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable).should('be.visible');
    // Wait for at least one row to appear in the table
    cy.get(BaseSelectors.RowClass).should('have.length.at.least', 1);
    
    // Find the row with the specified period type and click the Edit button
    cy.get(BaseSelectors.RowClass)
        .contains(accountingPeriodDetails.PeriodType || 'חודש חשבונאי')
        .parents(BaseSelectors.RowClass)
        .first()
        .find(AccountingPeriodSelectors.EditButton)
        .first()
        .should('be.visible')
        .click({ force: true });
    
    // Wait for dialog/window to appear, then click open month
    cy.get(AccountingPeriodSelectors.OpenMonthDialog, { timeout: 5000 }).should('be.visible');
    cy.DefineRequestWait(RestAPI.PUT, URLs.AccountingPeriods, "AccountingPeriodsRequest");
    cy.Click(AccountingPeriodSelectors.OpenMonthButton, null);
    cy.Click(AccountingPeriodSelectors.ConfirmOpenMonthButton, null);
}

export function AssertMonthOpenedSuccessfully() {
    BaseAssertion.AssertStatusCode("AccountingPeriodsRequest", 200);
}

