import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ActivitiesDetails } from "../../models/ActivitiesDetails";
import * as ActivitiesActions from "../../actions/ActivitiesActions";
import { ActivitiesSelectors } from "../../selectors/ActivitiesSelectors"

//#region Create new appointment
Given("the user logged in and open Activites in CRM", () => {
    cy.Login();
    ActivitiesActions.NavigatesToActivitiesInCRM();
});

Given("navigate appointment wizerd and fill the following details", (dataTable) => {
    ActivitiesActions.NavigatesToAppointmentWizerd();
    let appointmentDetails = Assists.CreateInstance<ActivitiesDetails>(dataTable, true);
    ActivitiesActions.FillAppointmentWizardsFields(appointmentDetails);
});

When("create appointment", () => {
    ActivitiesActions.CreateActivity();
});

Then("the appointment should create successfully", () => {
    ActivitiesActions.AssertCreateActivity()
});
//#endregion

//#region Search for the appointment by subject
When("search appointment", () => {
    ActivitiesActions.SearchActivity()
});

Then("the appointment should appear successfully", () => {
    ActivitiesActions.AssertSearchActivity()
});
//#endregion

//#region Open the appointment
When("open appointment", () => {
    ActivitiesActions.OpenActivity();
});

Then("the appointment should open successfully", () => {
    ActivitiesActions.AssertOpenActivity();
});
//#endregion

//#region add new note
Given("add {string} to main note", (note) => {
    ActivitiesActions.AddNewNote(note);
});

When("save appointment", () => {
    ActivitiesActions.UpdateActivity()
});
//#endregion

//#region copy appointment
Given("copy the appointment with new subject", () => {
    ActivitiesActions.NavigatesToCopyActivityWizerd()
    ActivitiesActions.FillSubject("Appointment_")
});

Then("the copy appointment should create successfully", () => {
    ActivitiesActions.AssertCreateCopyActivity()
});
//#endregion

//#region mark the appointment as complete
Given("navigate CRM activity screen", () => {
    cy.get(ActivitiesSelectors.Backbutton).click()
});

When("press on Mark as Complete button", () => {
    ActivitiesActions.MarkAsComplete();
});

Then("the appointment should update successfully", () => {
    ActivitiesActions.AssertUpdateActivity();
});

Then("the appointment should appear in My Closed Activites list", () => {
    ActivitiesActions.AssertActivityExistInCorrectList(ActivitiesSelectors.MyClosedActivitiesList);
});
//#endregion

//#region reopen appointment
When("reopen the appointment", () => {
    ActivitiesActions.ReOpenActivity()
});

Then("the appointment should appear in My Open Activites list", () => {
    ActivitiesActions.AssertActivityExistInCorrectList(ActivitiesSelectors.MyOpenActivitiesList);
});
//#endregion

//#region cancel appointment
When("cancel the appointment", () => {
    ActivitiesActions.CancelActivity()
});

Then("a red Cancelled label should appear", () => {
    ActivitiesActions.AssertCancelledLabelExists()
});

Then("the appointment should appear in Cancelled Activites list", () => {
    ActivitiesActions.AssertActivityExistInCorrectList(ActivitiesSelectors.CancelledActivitiesList);
});
//#endregion

//#region mark a appointment as complete from recent activities list
When("press on Complete button", () => {
    cy.get(ActivitiesSelectors.AppointmentFilter).click({ force: true })
    ActivitiesActions.CompleteAppointmentFromRecentActivitesList()
});

Then("the appointment should put complete successfully", () => {
    ActivitiesActions.AssertPutCompleteActivity();
});
//#endregion