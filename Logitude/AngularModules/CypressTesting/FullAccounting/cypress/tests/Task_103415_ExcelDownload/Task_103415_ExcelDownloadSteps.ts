import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ExcelDownloadActions from '../../actions/ExcelDownloadActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ExcelDownloadDetails } from '../../models/ExcelDownloadDetails';
import { ExcelDownloadSelectors } from '../../selectors/ExcelDownloadSelectors';

//#region Excel Download
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
});

Given("navigate journal workspace", () => {
    ExcelDownloadActions.NavigatesJournalWorkspace()
});

Given("enter to All journal",  () => {
    ExcelDownloadActions.Alljournal()
});

Given("click on the Excel button",  () => {
    ExcelDownloadActions.Excel()
});




// When("save as draft", () => {
//     JournalActions.SaveJournal()
// });

// Then("the journal should create successfully", () => {
//     JournalActions.AssertSaveJournal()
// });
// //#endregion

// //#region Approve the Journal
// When("approve the journal", () => {
//     JournalActions.ApproveJournal()
// });

// Then("the journal should approve successfully", () => {
//     JournalActions.AssertApproveJournal()
// });
// //#endregion

//#region Print report
// When("print the report", () => {
//     JournalActions.PrintReport();
// });

// Then("the report should print successfully", () => {
//     JournalActions.AssertPrintReport()
// });
//#endregion