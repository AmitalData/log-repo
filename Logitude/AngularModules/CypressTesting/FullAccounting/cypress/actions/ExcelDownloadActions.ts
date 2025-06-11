import { ExcelDownloadSelectors } from "../selectors/ExcelDownloadSelectors";
import { ExcelDownloadDetails } from "cypress/models/ExcelDownloadDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

export function NavigatesJournalWorkspace() {
    cy.Click(ExcelDownloadSelectors.JournalTab, null);

}

export function Alljournal() {
    cy.Click(ExcelDownloadSelectors.AllJournal, null);
}

export function Excel() {
    cy.Click(ExcelDownloadSelectors.Excelicon, null);
}





// export function SaveJournal() {
//     cy.DefineRequestWait(RestAPI.POST, URLs.Journals, RequestAliases.PostJournal)
//     cy.Click(JournalSelectors.SaveAsDraftButton, null)
// }

// export function AssertSaveJournal() {
//     BaseAssertion.AssertStatusCode(RequestAliases.PostJournal, 200)
// }

// export function ApproveJournal() {
//     cy.DefineRequestWait(RestAPI.PUT, URLs.Journals, RequestAliases.PutJournal)
//     cy.Click(JournalSelectors.ApproveButton, null)
// }

// export function AssertApproveJournal() {
//     BaseAssertion.AssertStatusCode(RequestAliases.PutJournal, 200)
//     cy.wait(5000)
// }

// export function PrintReport() {
//     cy.DefineWindowOpen(RequestAliases.PrintReportWindowOpen);
//     cy.Click(JournalSelectors.MenuButtons, null);
//     cy.Click(JournalSelectors.JournalPrint, null);
//     cy.Click(JournalSelectors.Printbutton, null,true);
// }

// export function AssertPrintReport() {
//     BaseAssertion.AssertWindowOpen(RequestAliases.PrintReportWindowOpen);
// }