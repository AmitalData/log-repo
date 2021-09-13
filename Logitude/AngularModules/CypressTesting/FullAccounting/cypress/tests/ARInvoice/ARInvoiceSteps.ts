import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ARInvoiceActions from '../../actions/ARInvoiceActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ARInvoiceDetails } from '../../models/ARInvoiceDetails';
import { InvoiceLineDetails } from '../../models/InvoiceLineDetails';

//#region Create new AR Invoice
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
});

Given("an AR Invoice with the following details", (dataTable) => {
    ARInvoiceActions.NavigatesARInvoiceWizerd()
    let ARInvoiceDetails = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
    ARInvoiceActions.FillARInvoiceDetails(ARInvoiceDetails)
});

When("create AR Invoice", () => {
    ARInvoiceActions.CreateARInvoice()
});

Then("the AR Invoice should get successfully", () => {
    ARInvoiceActions.AssertCreateARInvoice()
});
//#endregion

//#region Add new Invoice Line
Given("Invoice line with the following details", (dataTable) => {
    ARInvoiceActions.NavigatesARInvoiceLineWizerd()
    let invoiceLineDetails = Assists.CreateInstance<InvoiceLineDetails>(dataTable, true);
    ARInvoiceActions.FillInvoiceLineDetails(invoiceLineDetails);
});

When("add Invoice Line", () => {
    ARInvoiceActions.AddARInvoiceLine();
});

Then("the Invoice Line should be added successfully", () => {
    ARInvoiceActions.AssertAddARInvoiceLine()
});
//#endregion

//#region ARprove the AR Invoice
When("ARprove the AR Invoice", () => {
    ARInvoiceActions.ApproveARInvoice()
});

Then("the AR Invoice should ARprove successfully", () => {
    ARInvoiceActions.AssertApproveARInvoice()
});
//#endregion