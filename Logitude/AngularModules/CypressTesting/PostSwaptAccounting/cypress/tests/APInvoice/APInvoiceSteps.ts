import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as APInvoiceActions from '../../actions/APInvoiceActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { APInvoiceDetails } from '../../models/APInvoiceDetails';
import { InvoiceLineDetails } from '../../models/InvoiceLineDetails';

//#region Create new AP Invoice
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
});

Given("an AP Invoice with the following details", (dataTable) => {
    APInvoiceActions.NavigatesAPInvoiceWizerd()
    let aPInvoiceDetails = Assists.CreateInstance<APInvoiceDetails>(dataTable, true);
    APInvoiceActions.FillAPInvoiceDetails(aPInvoiceDetails)
});

// When("create AP Invoice", () => {
//     APInvoiceActions.CreateAPInvoice()
// });

// Then("the AP Invoice should get successfully", () => {
//     APInvoiceActions.AssertCreateAPInvoice()
// });
//#endregion

//#region Add new Invoice Line
Given("Invoice line with the following details", (dataTable) => {
    APInvoiceActions.NavigatesAPInvoiceLineWizerd()
    let invoiceLineDetails = Assists.CreateInstance<InvoiceLineDetails>(dataTable, true);
    APInvoiceActions.FillInvoiceLineDetails(invoiceLineDetails);
});

When("add Invoice Line", () => {
    APInvoiceActions.AddAPInvoiceLine();
});

Then("the Invoice Line should be added successfully", () => {
    APInvoiceActions.AssertAddAPInvoiceLine();
});
//#endregion

//#region Approve the AP Invoice
When("approve the AP Invoice", () => {
    APInvoiceActions.ApproveAPInvoice()
});

Then("the AP Invoice should approve successfully", () => {
    APInvoiceActions.AssertApproveAPInvoice()
});
//#endregion