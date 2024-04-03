import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ActivitiesDetails } from "../../models/ActivitiesDetails";
import * as BaseActions from "../../actions/ActivitiesActions";
import { ActivitiesSelectors } from "../../selectors/ActivitiesSelectors"

//#region Create new phone call
Given("the user logged in and open Activites in CRM", () => {
    cy.Login();
    BaseActions.NavigatesToActivitiesInCRM();
});

Given("navigate phone call wizerd and fill the following details", (dataTable) => {
    cy.Click(ActivitiesSelectors.NEWPHONECALL, null)
    let phoneCallDetails = Assists.CreateInstance<ActivitiesDetails>(dataTable, true);
    BaseActions.FillPhoneCallWizardsFields(phoneCallDetails);
});

When("create phone call", () => {
    BaseActions.CreateActivity();
});

Then("the phone call should create successfully", () => {
    BaseActions.AssertCreateActivity()
});
//#endregion

//#region Search for the phone call by subject
When("search phone call", () => {
    BaseActions.SearchActivity()
});

Then("the phone call should appear successfully", () => {
    BaseActions.AssertSearchActivity()
});
//#endregion

//#region Open the phone call
When("open phone call", () => {
    BaseActions.OpenActivity();
});

Then("the phone call should open successfully", () => {
    BaseActions.AssertOpenActivity();
});
//#endregion

//#region add new note
Given("add {string} to main note", (note) => {
    BaseActions.AddNewNote(note);
});

When("save phone call", () => {
    BaseActions.UpdateActivity()
});
//#endregion

//#region copy phone call
Given("copy the phone call with new subject", () => {
    BaseActions.NavigatesToCopyActivityWizerd()
    BaseActions.FillSubject("PhoneCall_")
});

Then("the copy phone call should create successfully", () => {
    BaseActions.AssertCreateCopyActivity()
});
//#endregion

//#region mark the phone call as complete
Given("navigate CRM activity screen", () => {
    cy.get(ActivitiesSelectors.Backbutton).click()
});

When("press on Mark as Complete button", () => {
    BaseActions.MarkAsComplete();
});

Then("the phone call should update successfully", () => {
    BaseActions.AssertUpdateActivity();
});

Then("the phone call should appear in My Closed Activites list", () => {
    BaseActions.AssertActivityExistInCorrectList(ActivitiesSelectors.MyClosedActivitiesList);
});
//#endregion

//#region reopen phone call
When("reopen the phone call", () => {
    BaseActions.ReOpenActivity()
});

Then("the phone call should appear in My Open Activites list", () => {
    BaseActions.AssertActivityExistInCorrectList(ActivitiesSelectors.MyOpenActivitiesList);
});
//#endregion

//#region cancel phone call
When("cancel the phone call", () => {
    BaseActions.CancelActivity()
});

Then("a red Cancelled label should appear", () => {
    BaseActions.AssertCancelledLabelExists()
});

Then("the phone call should appear in Cancelled Activites list", () => {
    BaseActions.AssertActivityExistInCorrectList(ActivitiesSelectors.CancelledActivitiesList);
});
//#endregion

//#region mark a phone call as complete from recent activities list
When("press on Complete button", () => {
    cy.get(ActivitiesSelectors.PhoneCallFilter).click({ force: true })
    BaseActions.CompleteActivityFromRecentActivitesList()
});

Then("the phone call should get update", () => {
    BaseActions.AssertCompleteActivity()
});
//#endregion