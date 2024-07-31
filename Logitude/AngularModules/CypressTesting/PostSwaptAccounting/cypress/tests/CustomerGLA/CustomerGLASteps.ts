import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CustomerGLAActions from '../../actions/CardActions';
import * as GLAccountsActions from '../../actions/GLAccountsActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CardDetails } from '../../models/CardDetails';
import { GLAccountsDetails } from '../../models/GLAccountsDetails';
import * as Actions from '../../actions/Actions';
import * as gr from '../../../../Base/cypress/actions/GenerateRandoms';
import { Constants } from "../../constants/Constants";

let currentDateTime = gr.GenerateCurrentDatetimeString("")
let searchFieldValue = null

//#region Create new customer
Given("the user logged in and navigates to Customers workspace", () => {
    cy.Login();
    CustomerGLAActions.NavigateCutomersWizerd()
});

Given("a customer with the following details", (dataTable) => {
    let customersGLADetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    CustomerGLAActions.FillCardDetails(customersGLADetails)
});

When("create customer", () => {
    CustomerGLAActions.CreateCard()
});

Then("the customer should create successfully", () => {
    CustomerGLAActions.AssertCreateCard(Constants.Customer)
});
//#endregion

//#region Search for the Customer by name
When("search customer", () => {
    searchFieldValue = CustomerGLAActions.getCardCode()
    Actions.Search(searchFieldValue)
});

Then("the customer should appear successfully", () => {
    Actions.AssertSearch(searchFieldValue)
});
//#endregion

//#region Open the customer
When("open the customer", () => {
    CustomerGLAActions.OpenCard(Constants.Customer);
});

Then("the customer should open successfully", () => {
    Actions.AssertGetSingle();
});
//#endregion

//#region Activate the customer in accounting system
Given("navigates new account wizerd inside the customer", () => {
    CustomerGLAActions.NavigateGLAccountWizerdFromCustomer()
});

Given("a GL Account with the following details", (dataTable) => {
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

//#region Connect Split by Currency Accounts
Given("navigate split by currency accounts", () => {
    CustomerGLAActions.NavigateSplitByCurrencyWizerd()
});

Given("fill the {string} as currency value", (currency) => {
    GLAccountsActions.FillCurrency(currency)
});
//#endregion