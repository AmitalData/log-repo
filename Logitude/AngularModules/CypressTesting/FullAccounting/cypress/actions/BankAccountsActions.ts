import { BankAccountsSelectors } from "../selectors/BankAccountsSelectors";
import { GLAccountsSelectors } from "../selectors/GLAccountsSelectors";
import { GLAccountsDetails } from "cypress/models/GLAccountsDetails";
import { BankAccountsDetails } from "cypress/models/BankAccountsDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let currentDateTime = gr.GenerateCurrentDatetimeString("")

export function NavigatesGLAccountWizerd() {
    cy.Click(GLAccountsSelectors.NewGLAccountButton, null)
}

export function FillGLAccountDetails(gLAccountsDetails: GLAccountsDetails) {
    cy.FillLogLov(GLAccountsSelectors.ChartOfAccountsType, gLAccountsDetails.ChartOfAccountsType, true)
    cy.FillLogLov(GLAccountsSelectors.ChartOfAccounts, gLAccountsDetails.ChartOfAccounts, true)
    cy.FillLogTextBox(GLAccountsSelectors.LocalName, gLAccountsDetails.LocalName + currentDateTime)
    cy.FillLogTextBox(GLAccountsSelectors.EnglishName, gLAccountsDetails.EnglishName + currentDateTime)
    cy.FillLogLov(GLAccountsSelectors.Currency, gLAccountsDetails.Currency, true)
    cy.FillLogLov(GLAccountsSelectors.RevenueExpenseType, gLAccountsDetails.RevenueExpenseType, true)
}

export function CreateGLAccount() {
    cy.DefineRequestWait(RestAPI.POST, URLs.GLAccounts, RequestAliases.PostGLAccounts)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateGLAccount() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.PostGLAccounts, 200)
}

export function NavigatesBankAccountWizerd() {
    cy.Click("#FABNKS", null)
    cy.Click(BankAccountsSelectors.NEWBANKButton, null)
}

export function FillBankAccountDetails(bankAccountsDetails: BankAccountsDetails) {
    let accountNumber = gr.GenerateRandomNumberAndString(14)
    cy.FillLogLov(BankAccountsSelectors.BankCode, bankAccountsDetails.BankCode, true)
    cy.FillLogTextBox(BankAccountsSelectors.BranchCode, bankAccountsDetails.BranchCode)
    cy.FillLogTextBox(BankAccountsSelectors.AccountNumber, accountNumber)
    cy.FillLogTextBox(BankAccountsSelectors.LocalBanchName, bankAccountsDetails.LocalBankName)
    cy.FillLogTextBox(BankAccountsSelectors.EnglishBankName, bankAccountsDetails.EnglishBankName)
    cy.FillLogLov(BankAccountsSelectors.Currency, bankAccountsDetails.Currency, true)
    cy.FillLogLov(BankAccountsSelectors.GLAccount, bankAccountsDetails.GLAccount + currentDateTime, true)
    cy.FillLogLov(BankAccountsSelectors.DefferedGLAccount, bankAccountsDetails.DefferedGLAccount + currentDateTime, true)
    cy.FillLogLov(BankAccountsSelectors.TransferGLAcccount, bankAccountsDetails.TransferGLAcccount + currentDateTime, true)
}

export function CreateBankAccount() {
    cy.DefineRequestWait(RestAPI.POST, URLs.Bankaccounts, RequestAliases.PostBankAccount)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateBankAccount() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.PostBankAccount, 200)
}