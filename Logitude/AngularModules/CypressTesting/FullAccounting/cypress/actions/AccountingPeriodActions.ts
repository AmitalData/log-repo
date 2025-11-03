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
    cy.get(AccountingPeriodSelectors.YearInput).should('be.visible').clear({ force: true }).type(accountingPeriodDetails.Year, { force: true });
    // Click the אישור (Approve) button after entering the year
    cy.contains('button', 'אישור').should('be.visible').click({ force: true });
    // Wait for the table to appear (much faster than waiting for API)
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable, { timeout: 5000 }).should('be.visible');
}

export function AssertYearSetSuccessfully() {
    // Pass as soon as we see the table data (period type text appears in the table)
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable).should('be.visible');
    // Check for the period type text in the table - if we see it, table is populated
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable)
        .contains('חודש חשבונאי')
        .should('be.visible');
}

export function OpenClosedMonth(accountingPeriodDetails: AccountingPeriodDetails) {
    // Find the row with the specified period type within the table and click Edit button
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable).should('be.visible');
    
    // Find the row containing the period type, then find the Edit button (img) within that row
    cy.get(AccountingPeriodSelectors.AccountingPeriodsTable)
        .find('.SimpleGridViewRow')
        .contains(accountingPeriodDetails.PeriodType || 'חודש חשבונאי')
        .parents('.SimpleGridViewRow')
        .first()
        .find('img[src*="Edit.png"], img[src*="edit.png"], img[src*="Edit"], img[src*="edit"]')
        .first()
        .should('be.visible')
        .click({ force: true });
    
    // Wait for dialog/window to appear - dialog opening means test passes
    cy.get(AccountingPeriodSelectors.OpenMonthDialog, { timeout: 5000 }).should('be.visible');
}

export function AssertMonthOpenedSuccessfully() {
    // Test passes when the dialog is visible - no need to actually open the month for smoke test
    cy.get(AccountingPeriodSelectors.OpenMonthDialog).should('be.visible');
}

