import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ActivitiesDetails } from "../../models/ActivitiesDetails";
import * as BaseActions from "../../actions/ActivitiesActions";
import { ActivitiesSelectors } from "../../selectors/ActivitiesSelectors"

//#region Create new task
Given("the user logged in and open Activites in CRM", () => {
    cy.Login();
    BaseActions.NavigatesToActivitiesInCRM();
});

Given("navigate task wizerd and fill the following details", (dataTable) => {
    cy.Click(ActivitiesSelectors.NEWTASK, null)
    let taskDetails = Assists.CreateInstance<ActivitiesDetails>(dataTable, true);
    BaseActions.FillTaskWizardsFields(taskDetails);
});

When("create task", () => {
    BaseActions.CreateActivity();
});

Then("the task should create successfully", () => {
    BaseActions.AssertCreateActivity()
});
//#endregion

//#region Search for the task by subject
When("search task", () => {
    BaseActions.SearchActivity()
});

Then("the task should appear successfully", () => {
    BaseActions.AssertSearchActivity()
});
//#endregion

//#region Open the task
When("open task", () => {
    BaseActions.OpenActivity();
});

Then("the task should open successfully", () => {
    BaseActions.AssertOpenActivity();
});
//#endregion

//#region add new note
Given("add {string} to main note", (note) => {
    BaseActions.AddNewNote(note);
});

When("save task", () => {
    BaseActions.UpdateActivity()
});
//#endregion

//#region copy task
Given("copy the task with new subject", () => {
    BaseActions.NavigatesToCopyActivityWizerd()
    BaseActions.FillSubject("Task_")
});

Then("the copy task should create successfully", () => {
    BaseActions.AssertCreateCopyActivity()
});
//#endregion

//#region mark the task as complete
Given("navigate CRM activity screen", () => {
    cy.get(ActivitiesSelectors.Backbutton).click()
});

When("press on Mark as Complete button", () => {
    BaseActions.MarkAsComplete();
});

Then("the task should update successfully", () => {
    BaseActions.AssertUpdateActivity();
});

Then("the task should appear in My Closed Activites list", () => {
    BaseActions.AssertActivityExistInCorrectList(ActivitiesSelectors.MyClosedActivitiesList);
});
//#endregion

//#region reopen task
When("reopen the task", () => {
    BaseActions.ReOpenActivity()
});

Then("the task should appear in My Open Activites list", () => {
    BaseActions.AssertActivityExistInCorrectList(ActivitiesSelectors.MyOpenActivitiesList);
});
//#endregion

//#region cancel task
When("cancel the task", () => {
    BaseActions.CancelActivity()
});

Then("a red Cancelled label should appear", () => {
    BaseActions.AssertCancelledLabelExists()
});

Then("the task should appear in Cancelled Activites list", () => {
    BaseActions.AssertActivityExistInCorrectList(ActivitiesSelectors.CancelledActivitiesList);
});
//#endregion

//#region mark a task as complete from recent activities list
When("press on Complete button", () => {
    cy.get(ActivitiesSelectors.TaskFilter).click({ force: true })
    BaseActions.CompleteActivityFromRecentActivitesList()
});

Then("the task should get update", () => {
    BaseActions.AssertCompleteActivity()
});
//#endregion