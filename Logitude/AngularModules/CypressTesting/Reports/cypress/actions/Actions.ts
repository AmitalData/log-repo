
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as BaseActions from "../../../Base/cypress/actions/Actions";

import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { ReportsSelectors } from "../selectors/Selectors";
import { ReportsUrls } from "../constants/ReportsUrls";
import { ReportSettingsDetails } from "../../cypress/models/ReportSettingsDetails";
import { ExportReportAdvancedDetails } from "../../cypress/models/ExportReportAdvancedDetails"
import { SendReportDetails } from "../../cypress/models/SendReportDetails";
import { constants } from "../../../Base/cypress/constants/constants"
import {ReportConstants} from "../../cypress/constants/ReportConstants"

let IsException = null;

export function NavigatesToReportWorkspaceInReportsMenu(reportName: string) {
  SearchReport(reportName)
  OpenReport(reportName)
}
function SearchReport(reportName: string) {
  NavigatesToReportsMenu();
  cy.get(BaseSelectors.NullSearch).type(reportName)
}
function NavigatesToReportsMenu() {
  cy.Click(ReportsSelectors.ReportsMenu, null, true);
}
function OpenReport(reportName: string) {
  cy.DefineRequestWait(RestAPI.GET, ReportsUrls.ReportsTemplate, RequestAliases.ReportsTemplate)
  cy.Click(ReportsSelectors.ReportId, reportName, true)
  BaseAssertion.AssertStatusCode(RequestAliases.ReportsTemplate, 200)
}

export function ChangeReportsSettings(isException:string) {
  IsException = isException;
  if (isException.toUpperCase() == constants.YES) {
    cy.get(BaseSelectors.CheckboxInput).check({ force: true })
  }
  else if (isException.toUpperCase() == constants.NO) {
    cy.get(BaseSelectors.CheckboxInput).uncheck({ force: true })
  }
}
export function RunReport() {
  cy.DefineRequestWait(RestAPI.PUT, ReportsUrls.Report, RequestAliases.Report)
  if (IsException.toUpperCase() == constants.NO) {
    cy.DefineRequestWait(RestAPI.PUT, ReportsUrls.Report, RequestAliases.Report)
  }
  cy.Click(ReportsSelectors.RunReportButton, null);
}

export function PrintReport() {
  cy.DefineWindowOpen(RequestAliases.PrintReportWindowOpen);
  cy.Click(BaseSelectors.Button, BaseSelectors.ContainPrint);
}

export function SaveReport(saveType: string, exportReportAdvancedDetails: ExportReportAdvancedDetails) {
  cy.Click(BaseSelectors.ToggleButtonClass, BaseSelectors.ContainSave);
  if (saveType.toUpperCase() == constants.ExcelFile) {
    cy.Click(ReportsSelectors.SaveExcelFileButton, null)
  }
  else {
    SaveAdvancedExcelFile(exportReportAdvancedDetails)
  }
}
function SaveAdvancedExcelFile(exportReportAdvancedDetails: ExportReportAdvancedDetails) {
  cy.Click(ReportsSelectors.SaveAdvancedExcelFileButton, null)
  FillExportReportAdvancedDetails(exportReportAdvancedDetails)
  cy.Click(BaseSelectors.RedButton, null, true)
}
function FillExportReportAdvancedDetails(exportReportAdvancedDetails: ExportReportAdvancedDetails) {
  FillExportReportAdvancedCheckBox(ReportConstants.ExportDataOnly, exportReportAdvancedDetails.ExportDataOnly)
  FillExportReportAdvancedCheckBox(ReportConstants.ExportObjectFormatting, exportReportAdvancedDetails.ExportObjectFormatting)
  FillExportReportAdvancedCheckBox(ReportConstants.UseOnePageHeaderAndFooter, exportReportAdvancedDetails.UseOnePageHeaderAndFooter)
}
function FillExportReportAdvancedCheckBox(ContainexportReportAdvanceData: string, exportReportAdvanceData: string) {
  if (exportReportAdvanceData.toUpperCase() == constants.YES) {
    cy.contains(ContainexportReportAdvanceData).siblings(BaseSelectors.td).find(BaseSelectors.input).check({ force: true })
  }
  else {
    cy.contains(ContainexportReportAdvanceData).siblings(BaseSelectors.td).find(BaseSelectors.input).uncheck({ force: true })
  }
}
export function FillSendReportDetails(sendReportDetails: SendReportDetails) {
  cy.Click(BaseSelectors.ToggleButtonClass, BaseSelectors.ContainSend)
  cy.Click(BaseSelectors.button, sendReportDetails.SendType)
  if (sendReportDetails.SendTo.toUpperCase() == constants.LoggedInUser) {
    BaseActions.FillLoggedInUserEmail(BaseSelectors.EmailSearchTextBox)
  }
  else {
    cy.get(BaseSelectors.EmailSearchTextBox).type(sendReportDetails.SendTo + '{downarrow}{enter}')
  }
}

export function SendReport() {
  cy.DefineRequestWait(RestAPI.POST, BaseURLs.PostSendhtmlDocument, RequestAliases.SendReport)
  cy.Click(ReportsSelectors.SendMessageIcon, null)
}

export function AssertDownloadFile(fileName: string) {
  cy.readFile('cypress/downloads/' + fileName).should('exist')
}