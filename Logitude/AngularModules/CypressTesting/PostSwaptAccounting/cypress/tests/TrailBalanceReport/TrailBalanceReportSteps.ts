import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as APInvoiceActions from '../../actions/APInvoiceActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { APInvoiceDetails } from '../../models/APInvoiceDetails';
import { InvoiceLineDetails } from '../../models/InvoiceLineDetails';
import * as TrailBalanceReportAction from '../../actions/TrailBalanceReportAction';
import { TrailBalanceReportDetails } from '../../models/TrailBalanceReportDetails';

//#region Run TrailBalanceReport

Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    //Actions.NavigatesFullAccounting()
});

Given("run Trail Balance Report with the following details", (dataTable) => {
    TrailBalanceReportAction.NavigatesTrailBalanceReportWizerd()
    let TrailBalanceReportDetails = Assists.CreateInstance<TrailBalanceReportDetails>(dataTable, true);
    TrailBalanceReportAction.FillTrailBalanceReportDetails(TrailBalanceReportDetails)
});

//#region Run Trail Balance Report
When("run Trail Balance Report", () => {
    TrailBalanceReportAction.RunTrailBalanceReport()
});

Then("the Trail Balance Report should get successfully", () => {
    TrailBalanceReportAction.AssertRunTrailBalanceReport()
});
//#endregion