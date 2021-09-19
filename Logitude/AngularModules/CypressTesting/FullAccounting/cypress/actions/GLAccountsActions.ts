import { GLAccountsSelectors } from "../selectors/GLAccountsSelectors";
import { GLAccountsDetails } from "cypress/models/GLAccountsDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as Actions from './Actions';

export function NavigatesGLAccountWizerd() {
    cy.Click(GLAccountsSelectors.NewGLAccountButton, null)
}

export function FillGLAccountDetails(gLAccountsDetails: GLAccountsDetails, currentDateTime) {
    cy.FillLogLov(GLAccountsSelectors.ChartOfAccountsType, gLAccountsDetails.ChartOfAccountsType, true)
    cy.FillLogLov(GLAccountsSelectors.ChartOfAccounts, gLAccountsDetails.ChartOfAccounts, true)
    cy.FillLogTextBox(GLAccountsSelectors.LocalName, gLAccountsDetails.LocalName + currentDateTime)
    FillEnglishName(gLAccountsDetails.EnglishName + currentDateTime)
    FillCurrency(gLAccountsDetails.Currency)
    Actions.FillCheckBoxProcess(GLAccountsSelectors.MultiCurrencyCheckBox, gLAccountsDetails.MultiCurrencyCheckBox)
    cy.FillLogLov(GLAccountsSelectors.RevenueExpenseType, gLAccountsDetails.RevenueExpenseType, true)
}

export function FillEnglishName(englishName) {
    cy.FillLogTextBox(GLAccountsSelectors.EnglishName, englishName)
}

export function FillCurrency(currency) {
    cy.FillLogLov(GLAccountsSelectors.Currency, currency, true)
}

export function CreateGLAccount() {
    cy.DefineRequestWait(RestAPI.POST, URLs.GLAccounts, RequestAliases.PostGLAccounts)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateGLAccount() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostGLAccounts, 200)
}

export function Search(searchFieldValue) {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetQuickSearch(searchFieldValue), RequestAliases.GetQuickSearch);
    cy.get(GLAccountsSelectors.SearchBox).type(searchFieldValue);
}

export function AssertSearch() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuickSearch, 200);
}

export function OpenGLAccounts() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GLAccountGetSingle, RequestAliases.GetSignle);
    cy.get(GLAccountsSelectors.ListBoxItem).eq(0).click();
}

export function SaveGLAccounts() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.GLAccounts, RequestAliases.PutGLAccounts);
    cy.Click(GLAccountsSelectors.SaveButton, null)
}

export function ASsertSaveGLAccounts() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutGLAccounts, 200)
}