import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BankAccountsActions from '../../actions/BankAccountsActions';
import * as GLAccountsActions from '../../actions/GLAccountsActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { GLAccountsDetails } from '../../models/GLAccountsDetails';
import { GLAccountsSelectors } from "../../selectors/GLAccountsSelectors";

let currentDateTime = BankAccountsActions.GetCurrentDateTime()
let gLAccountsDetails = null

//#region Create new GL Account
Given("the user logged in and navigates to GL Account workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
    cy.Click(GLAccountsSelectors.GLAccountsTab, null)
});

Given("a GL Account with the following details", (dataTable) => {
    GLAccountsActions.NavigatesGLAccountWizerd()
    gLAccountsDetails = Assists.CreateInstance<GLAccountsDetails>(dataTable, true);
    GLAccountsActions.FillGLAccountDetails(gLAccountsDetails, currentDateTime)
});

When("create GL Account", () => {
    GLAccountsActions.CreateGLAccount()
});

Then("the GL Account should create successfully", () => {
    GLAccountsActions.AssertCreateGLAccount()
});
//#endregion

//#region Search for the GL Account by name
When("search GL Account", () => {
    GLAccountsActions.Search(gLAccountsDetails.LocalName + currentDateTime)
});

Then("the GL Account should appear successfully", () => {
    GLAccountsActions.AssertSearch()
});
//#endregion

//#region Open the GL Account
When("open GL Account", () => {
    GLAccountsActions.OpenGLAccounts();
});

Then("the GL Account should open successfully", () => {
    Actions.AssertGetSingle();
});
//#endregion

//#region Edit the GL Account
Given("fill {string} as EnglishName", (englishName) => {
    cy.Click(GLAccountsSelectors.GeneralTab, null)
    GLAccountsActions.FillEnglishName(englishName)
});

When("save GL Account", () => {
    GLAccountsActions.SaveGLAccounts();
});

Then("the GL Account should update successfully", () => {
    GLAccountsActions.ASsertSaveGLAccounts();
});
//#endregion