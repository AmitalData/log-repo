import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { OverviewDetails } from "../../models/OverviewDetails";
import * as BaseActions from "../../actions/OverviewActions";
import { OverviewSelectors } from "../../selectors/OverviewSelectors"

//#region Create new task
Given("the user logged in and open Overview in CRM", () => {
    cy.Login();
    BaseActions.NavigatesToOverviewInCRM();
});

Given("navigate task wizerd and fill the following details", (dataTable) => {
    BaseActions.NavigatesToTaskInOverview()
    let taskDetails = Assists.CreateInstance<OverviewDetails>(dataTable, true);
    BaseActions.FillTaskWizardsFields(taskDetails);
});

When("create task", () => {
    BaseActions.CreateActivity();
});

Then("the task should create successfully", () => {
    BaseActions.AssertCreateActivity()
});
//#endregion


//#region mark a task as complete from Upcoming Activities list

When("press on Complete button", () => {
    BaseActions.CompleteActivityFromRecentActivitesList()
});

Then("the task should get update", () => {
    BaseActions.AssertCompleteActivity()
});
//#endregionS

//#region go into the task from Daily Spotlight list

When("press on redirect button from today column", () => {
    BaseActions.GoInsideTaskFromDailySpotlightList()
});

Then("the task should opend", () => {
    BaseActions.AssertTodayActivity()
});
//#endregion


//#region Create new post 

Given("navigate post and fill the following details", (dataTable) => {

    cy.get(OverviewSelectors.PostsArea).eq(1).type("TestPost")
  
});

When("create post", () => {
    BaseActions.CreatePost()
});

Then("the post should create successfully", () => {
    BaseActions.AssertCreatePost()
});
//#endregion

//#region Create new post 

Given("navigate message and fill the following details", (dataTable) => {
    
    cy.get(OverviewSelectors.OverviewTab).eq(2).click()
  
});

When("create new message", () => {
    BaseActions.CreateNewMessage()
});

Then("the message should create successfully", () => {
    BaseActions.AssertCreateNewMessage()
});
//#endregion

