import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as SupplierAccountActions from '../../actions/SupplierAccountActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { SupplierAccountDetails } from '../../models/SupplierAccountDetails';
import * as SearchActions from '../../actions/SearchActions';

//#region Supplier Account Test
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    SearchActions.NavigateToImportDeclarationsWorkspace();
});

Given("Search for file and enter to Supplier Accounts", (dataTable) => {
    let supplierAccountDetails = Assists.CreateInstance<SupplierAccountDetails>(dataTable, true);
    SupplierAccountActions.FillSearchField(supplierAccountDetails);
});

Given("Fill Supplier Account with the following details", (dataTable) => {
    let supplierAccountDetails = Assists.CreateInstance<SupplierAccountDetails>(dataTable, true);
    SupplierAccountActions.CreateAndFillNewSupplierAccount(supplierAccountDetails);
});

Given("Fill Transport Data with the following details", (dataTable) => {
    let supplierAccountDetails = Assists.CreateInstance<SupplierAccountDetails>(dataTable, true);
    SupplierAccountActions.FillTransportData(supplierAccountDetails);
});

Given("Fill the New Customs Detail Row with the following details", (dataTable) => {
    let supplierAccountDetails = Assists.CreateInstance<SupplierAccountDetails>(dataTable, true);
    SupplierAccountActions.CreateNewCustomsDetailRow(supplierAccountDetails);
    SupplierAccountActions.FillCustomsDetailRow(supplierAccountDetails);
});

When("saving the Supplier Account", () => {
    SupplierAccountActions.SaveSupplierAccount();
});

Then("the Supplier Account should save successfully", () => {
    SupplierAccountActions.AssertSaveSupplierAccount();
});

Given("the user delete the Supplier Account row", () => {
    SupplierAccountActions.DeleteRow();
});

Then("there is no Supplier Account row in the grid", () => {
    SupplierAccountActions.AssertDeleteRow();
});

//#endregion

