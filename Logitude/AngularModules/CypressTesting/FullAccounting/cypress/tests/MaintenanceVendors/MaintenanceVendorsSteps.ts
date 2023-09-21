import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { CardDetails } from "../../../cypress/models/CardDetails";
import { CardSelectors } from "../../selectors/CardSelectors";
import { Constants } from "../../constants/Constants";
import * as VendorActions from '../../actions/CardActions';
import * as GLAccountsActions from '../../actions/GLAccountsActions';
import { GLAccountsDetails } from '../../models/GLAccountsDetails';
import * as gr from '../../../../Base/cypress/Actions/GenerateRandoms';

let currentDateTime = gr.GenerateCurrentDatetimeString("")

let searchFieldValue = null;

//#region Create new vendor
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, CardSelectors.VendorMaintenanceItem)
});

Given("a vendor with the following details", (dataTable) => {
    let vendorDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    Actions.OpenNewWizard("Vendor");
    VendorActions.FillCardDetails(vendorDetails)
});

When("create vendor", () => {
    VendorActions.CreateCard();
});

Then("the vendor should create successfully", () => {
    VendorActions.AssertCreateCard(Constants.Vendor)
});

//#endregion

//#region Search for the vendor by code
When("search vendor", () => {
    searchFieldValue = VendorActions.getCardCode()
    Actions.Search(searchFieldValue);
});

Then("the vendor should appear successfully", () => {
    Actions.AssertSearch(searchFieldValue);
});

//#endregion

//#region Open the vendor
When("open vendor", () => {
    VendorActions.OpenCard(Constants.Vendor);
});

Then("the vendor should open successfully", () => {
    Actions.AssertGetSingle();
});
//#endregion

//#region Activate the vendor in accounting system
Given("navigates new account wizerd inside the customer", () => {
    VendorActions.NavigateGLAccountWizerdFromVendor()
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