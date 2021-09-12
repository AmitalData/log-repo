import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as APInvoiceActions from '../../actions/APInvoiceActions';
import * as BankAccountsActions from '../../actions/BankAccountsActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { APInvoiceDetails } from '../../models/APInvoiceDetails';
import { GLAccountsDetails } from '../../models/GLAccountsDetails';
import { BankAccountsDetails } from '../../models/BankAccountsDetails';

import { InvoiceLineDetails } from '../../models/InvoiceLineDetails';
import { GLAccountsSelectors } from "../../selectors/GLAccountsSelectors";

//#region Create new GL Account
Given("the user logged in and navigates to GL Account workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
    cy.Click(GLAccountsSelectors.GLAccountsTab, null)
});

Given("a GL Account with the following details", (dataTable) => {
    BankAccountsActions.NavigatesGLAccountWizerd()
    let gLAccountsDetails = Assists.CreateInstance<GLAccountsDetails>(dataTable, true);
    BankAccountsActions.FillGLAccountDetails(gLAccountsDetails)
});

When("create GL Account", () => {
    BankAccountsActions.CreateGLAccount()
});

Then("the GL Account should create successfully", () => {
    BankAccountsActions.AssertCreateGLAccount()
});
//#endregion

//#region Create new Bank Account
Given("the user navigates Bank Accounts wizerd", () => {
    BankAccountsActions.NavigatesBankAccountWizerd()
});

Given("a Bank Account with the following details", (dataTable) => {
    let bankAccountsDetails = Assists.CreateInstance<BankAccountsDetails>(dataTable, true);
    BankAccountsActions.FillBankAccountDetails(bankAccountsDetails);
});

When("create Bank Account", () => {
    BankAccountsActions.CreateBankAccount();
});

Then("the Bank Account should create successfully", () => {
    BankAccountsActions.AssertCreateBankAccount();
});
//#endregion