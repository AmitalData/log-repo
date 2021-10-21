import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as JournalActions from '../../actions/JournalActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { JournalLineActionDetails } from '../../models/JournalLineActionDetails';

//#region Create new Journal
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
});

Given("navigate journal workspace", () => {
    JournalActions.NavigatesJournalWorkspace()
});

Given("a journal line action with the following details", (dataTable) => {
    let journalLineActionDetails = Assists.CreateInstance<JournalLineActionDetails>(dataTable, true);
    JournalActions.FillLineActionDetails(journalLineActionDetails)
});

When("save as draft", () => {
    JournalActions.SaveJournal()
});

Then("the journal should create successfully", () => {
    JournalActions.AssertSaveJournal()
});
//#endregion

//#region Approve the Journal
When("approve the journal", () => {
    JournalActions.ApproveJournal()
});

Then("the journal should approve successfully", () => {
    JournalActions.AssertApproveJournal()
});
//#endregion

//#region Print report
When("print the report", () => {
    JournalActions.PrintReport();
});

Then("the report should print successfully", () => {
    JournalActions.AssertPrintReport()
});
//#endregion