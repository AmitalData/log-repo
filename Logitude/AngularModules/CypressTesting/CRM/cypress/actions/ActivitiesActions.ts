import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { ActivitiesDetails } from "../models/ActivitiesDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ActivitiesSelectors } from "../selectors/ActivitiesSelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Constants } from "../constants/Constants";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null

export function NavigatesToActivitiesInCRM() {
    cy.Click(ActivitiesSelectors.CRM, null)
    cy.Click(ActivitiesSelectors.Activities, null)
    cy.get(ActivitiesSelectors.NEWACTIVITY).click()
}

export function NavigatesToPhoneCallWizerd() {
    cy.Click(ActivitiesSelectors.NEWPHONECALL, null)
}

export function NavigatesToTaskWizerd() {
    cy.Click(ActivitiesSelectors.NEWTASK, null)
}

export function NavigatesToAppointmentWizerd() {
    cy.Click(ActivitiesSelectors.NEWAPPOINTMENT, null)
}

export function FillPhoneCallWizardsFields(activitiesDetails: ActivitiesDetails) {
    cy.FillLogLov(ActivitiesSelectors.ActivityCustomer, activitiesDetails.Customer, true)
    cy.FillLogLov(ActivitiesSelectors.ActivityCallWith, activitiesDetails.CallWith, true);
    FillSubject("PhoneCall_")
    cy.FillLogTextBox(ActivitiesSelectors.ActivityDescription, activitiesDetails.Description)
    cy.FillLogLov(ActivitiesSelectors.ActivityPriorityCode, activitiesDetails.PriorityCode, true)
}

export function FillTaskWizardsFields(activitiesDetails: ActivitiesDetails) {
    FillSubject("Task_")
    cy.FillLogTextBox(ActivitiesSelectors.ActivityDescription, activitiesDetails.Description)
    cy.FillLogLov(ActivitiesSelectors.ActivityPriorityCode, activitiesDetails.PriorityCode, true)
}

export function FillAppointmentWizardsFields(activitiesDetails: ActivitiesDetails) {
    cy.FillLogLov(ActivitiesSelectors.ActivityCustomer, activitiesDetails.Customer, true)
    FillSubject("Appointment_")
    cy.FillLogTextBox(ActivitiesSelectors.ActivityDescription, activitiesDetails.Description)
    cy.FillLogLov(ActivitiesSelectors.ActivityPriorityCode, activitiesDetails.PriorityCode, true)
}

export function FillSubject(activityType: string) {
    cy.FillLogTextBox(ActivitiesSelectors.ActivitySubject + BaseSelectors.LastElement, activityType + GenerateCurrentDatetimeString("_"))
}

export function CreateActivity() {
    DefinePostActivityRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostActivityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Activity, RequestAliases.PostActivity);
}

export function AssertCreateActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostActivity, 200).then((interception) => {
        searchFieldValue = interception.response.body.Subject
    });
}

export function SearchActivity() {
    DefineViewsGetByFiltersRequest();
    cy.FillLogTextBox(ActivitiesSelectors.SearchTextboxInput, searchFieldValue);
}

function DefineViewsGetByFiltersRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(searchFieldValue), RequestAliases.GetFilterSearch);
}

export function AssertSearchActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function OpenActivity() {
    DefineActivityGetSingleRequest();
    cy.get(ActivitiesSelectors.QuickSearchTextBox)
        .within(() => {
            cy.get('ul > li').eq(0).click({ force: true });
        });
}

function DefineActivityGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ActivityGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenActivity() {
    AssertActivityGetSingle();
    BaseAssertion.AssertElementExist(ActivitiesSelectors.GeneralEditScreen)
}

function AssertActivityGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function AddNewNote(note) {
    DefinePutActivityRequest()
    cy.get(ActivitiesSelectors.HelperNotesButton).click()
    cy.get(ActivitiesSelectors.HelperNotes).type(note)
}

export function UpdateActivity() {
    DefinePutActivityRequest()
    cy.Click(ActivitiesSelectors.SaveButton, null)
}

export function MarkAsComplete() {
    DefinePutActivityRequest()
    cy.get(ActivitiesSelectors.MarkAsComplete).click()
}

function DefinePutActivityRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Activity, RequestAliases.PutActivity)
}

export function AssertUpdateActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutActivity, 200)
}

export function AssertActivityExistInCorrectList(queryLink: string) {
    cy.get(ActivitiesSelectors.Backbutton).click()
    cy.get(queryLink).click()
    DefineViewsGetByFiltersRequest();
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, searchFieldValue);
    AssertSearchActivity()
    cy.get(ActivitiesSelectors.Backbutton).click()
}

export function NavigatesToCopyActivityWizerd() {
    cy.get(ActivitiesSelectors.MenuButtons).click()
    cy.Click(ActivitiesSelectors.CopyActivity, null)
}

export function AssertCreateCopyActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostActivity, 200)
}

export function ReOpenActivity() {
    DefinePutActivityRequest()
    cy.get(ActivitiesSelectors.MenuButtons).click()
    cy.Click(ActivitiesSelectors.ReOpenActivity, null)
}

export function CancelActivity() {
    DefinePutActivityRequest()
    cy.get(ActivitiesSelectors.MenuButtons).click()
    cy.Click(ActivitiesSelectors.CancelActivity, null)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCancelledLabelExists() {
    BaseAssertion.AssertElementExist(ActivitiesSelectors.CancelledLabel)
    BaseAssertion.AssertElementTextEqual(ActivitiesSelectors.CancelledLabel, Constants.Cancelled)
}

export function CompleteActivityFromRecentActivitesList() {
    cy.wait(1000)
    DefineCompleteActivityRequest();
    cy.get(ActivitiesSelectors.RecentEntityItem).eq(0).find(ActivitiesSelectors.Button).click({ force: true });
}

function DefineCompleteActivityRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetCompleteActivity, RequestAliases.GetCompleteActivity);
}

export function AssertCompleteActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetCompleteActivity, 200);
}

export function CompleteAppointmentFromRecentActivitesList() {
    cy.wait(1000)
    DefinePutCompleteActivityRequest()
    cy.get(ActivitiesSelectors.RecentEntityItem).eq(0).find(ActivitiesSelectors.AppointmentCompleteButton).click({ force: true });
    cy.get(ActivitiesSelectors.MettingSummaryRedButton).filter(":visible").click()
}

function DefinePutCompleteActivityRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutCompleteActivity, RequestAliases.PutCompleteActivity)
}

export function AssertPutCompleteActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCompleteActivity, 200);
}