import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BankAccountsActions from '../../actions/BankAccountsActions';
import * as GLAccountsActions from '../../actions/GLAccountsActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { GLAccountsDetails } from '../../models/GLAccountsDetails';
import { BankAccountsDetails } from '../../models/BankAccountsDetails';
import { GLAccountsSelectors } from "../../selectors/GLAccountsSelectors";

let currentDateTime = BankAccountsActions.GetCurrentDateTime()

//#region Create new GL Account
Given("the user logged in and navigates to GL Account workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
    cy.Click(GLAccountsSelectors.GLAccountsTab, null)
});

Given("a GL Account with the following details", (dataTable) => {
    GLAccountsActions.NavigatesGLAccountWizerd()
    let gLAccountsDetails = Assists.CreateInstance<GLAccountsDetails>(dataTable, true);
    GLAccountsActions.FillGLAccountDetails(gLAccountsDetails, currentDateTime)
});

When("create GL Account", () => {
    GLAccountsActions.CreateGLAccount()
});

Then("the GL Account should create successfully", () => {
    GLAccountsActions.AssertCreateGLAccount()
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