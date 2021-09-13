import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CustomerGLAActions from '../../actions/CustomerGLAActions';
import * as GLAccountsActions from '../../actions/GLAccountsActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CustomersGLADetails } from '../../models/CustomersGLADetails';
import { GLAccountsDetails } from '../../models/GLAccountsDetails';
import * as Actions from '../../actions/Actions';
import * as gr from '../../../../Base/cypress/actions/GenerateRandoms';

let currentDateTime = gr.GenerateCurrentDatetimeString("")
let searchFieldValue = null

//#region Create new customer
Given("the user logged in and navigates to Customers workspace", () => {
    cy.Login();
    CustomerGLAActions.NavigateCutomersWizerd()
});

Given("a customer with the following details", (dataTable) => {
    let customersGLADetails = Assists.CreateInstance<CustomersGLADetails>(dataTable, true);
    CustomerGLAActions.FillCustomerDetails(customersGLADetails)
});

When("create customer", () => {
    CustomerGLAActions.CreateCustomer()
});

Then("the customer should create successfully", () => {
    CustomerGLAActions.AssertCreateCustomer()
});
//#endregion

//#region Search for the Customer by name
When("search customer", () => {
    searchFieldValue = CustomerGLAActions.GetSearchFieldValue()
    Actions.Search(searchFieldValue)
});

Then("the customer should appear successfully", () => {
    Actions.AssertSearch(searchFieldValue)
});
//#endregion

//#region Open the customer
When("open the customer", () => {
    CustomerGLAActions.OpenCutomer();
});

Then("the customer should open successfully", () => {
    CustomerGLAActions.AssertOpenCutomer();
});
//#endregion

//#region Activate the customer in accounting system
Given("navigates new account wizerd inside the customer", () => {
    CustomerGLAActions.NavigateNewAccountWizerd()
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