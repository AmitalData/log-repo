import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ReportSettingsDetails } from "../../../cypress/models/ReportSettingsDetails";
import * as ReportActions from "../../actions/Actions";
import * as ReportAssertion from "../../actions/Assertion";
import {ExportReportAdvancedDetails} from "../../../cypress/models/ExportReportAdvancedDetails"
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { SendReportDetails } from "../../../cypress/models/SendReportDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import {ReportConstants} from "../../../cypress/constants/ReportConstants"
let reportSettingsDetails: ReportSettingsDetails
let sendReportDetails: SendReportDetails
let exportReportAdvancedDetails:ExportReportAdvancedDetails
//#region Run Report with exception / without exception

Given("the user logged in and navigates to {string} in reports menu", (reportName) => {
    cy.Login();
    ReportActions.NavigatesToReportWorkspaceInReportsMenu(reportName);
});

Given("a report settings with the following details", (dataTable) => {
    reportSettingsDetails = Assists.CreateInstance<ReportSettingsDetails>(dataTable, true);
    ReportActions.ChangeReportsSettings(reportSettingsDetails)
});

When("run report", () => {
    ReportActions.RunReport();
});

Then("the report should run successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.Report, 200)
});

Then("the information messagee with {string} message should appear", (informationMessagee) => {
    BaseAssertion.AssertInformationMessage(informationMessagee)
});

Then("the report should appear", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.Report, 200)
});
//#endregion

//#region Print report
When("print the report", () => {
    ReportActions.PrintReport();
});

Then("the report should print successfully", () => {
ReportAssertion.AssertPrintReport()
});

//#endregion

//#region Save report as excel file advanced/excel file
Given("export advanced settings with the following details", (dataTable) => {
    exportReportAdvancedDetails = Assists.CreateInstance<ExportReportAdvancedDetails>(dataTable, true);
});

When("save report as {string}", (saveType) => {
    ReportActions.SaveReport(saveType,exportReportAdvancedDetails)
});

Then("the excel file should download successfully", () => {
    ReportActions.AssertDownloadFile(ReportConstants.AutomationTestReport)
});

//#endregion

//#region Send report as Excel File/Pdf File
Given("a send report with the following details", (dataTable) => {
    sendReportDetails = Assists.CreateInstance<SendReportDetails>(dataTable, true);
    ReportActions.FillSendReportDetails(sendReportDetails)
});

When("send report", () => {
    ReportActions.SendReport();
});

Then("the report should send successfully", () => {
    ReportAssertion.AssertSendReport()
});

Then("the send message window should disappear", () => {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
});
//#endregion

