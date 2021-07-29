import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ActivitySelectors } from "../selectors/ActivitySelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Constants } from "../constants/Constants";

let searchFieldValue = null

export function NavigatesToActivitiesInCRM() {
    cy.Click(ActivitySelectors.CRM, null)
    cy.Click(ActivitySelectors.Activities, null)
}

export function CreateActivity() {
    DefinePostActivityRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostActivityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Activity, RequestAliases.PostPhoneCall);
}

export function AssertCreateActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostPhoneCall, 200).then((interception) => {
        searchFieldValue = interception.response.body.Subject
    });
}

export function SearchActivity() {
    DefineViewsGetByFiltersRequest();
    cy.FillLogTextBox(ActivitySelectors.SearchTextboxInput, searchFieldValue);
}

function DefineViewsGetByFiltersRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(searchFieldValue), RequestAliases.GetFilterSearch);
}

export function AssertSearchActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function OpenActivity() {
    DefineActivityGetSingleRequest();
    cy.get('.LogitudeQuickSearchTextBox')
        .within(() => {
            cy.get('ul > li').eq(0).click({ force: true });
        });
}

function DefineActivityGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ActivityGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenActivity() {
    AssertActivityGetSingle();
    BaseAssertion.AssertElementExist(ActivitySelectors.GeneralEditScreen)
}

function AssertActivityGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function AddNewNote(note) {
    DefinePutPhoneCallRequest()
    cy.get(ActivitySelectors.HelperNotesButton).click()
    cy.get(ActivitySelectors.HelperNotes).type(note)
}

export function UpdateActivity() {
    DefinePutPhoneCallRequest()
    cy.Click(ActivitySelectors.SaveButton, null)
}

export function MarkAsComplete() {
    DefinePutPhoneCallRequest()
    cy.get(ActivitySelectors.MarkAsComplete).click()
}

function DefinePutPhoneCallRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Activity, RequestAliases.PutPhoneCall)
}

export function AssertUpdateActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutPhoneCall, 200)
}

export function AssertActivityExistInCorrectList(queryLink: string) {
    cy.get(ActivitySelectors.Backbutton).click()
    cy.get(queryLink).click()
    DefineViewsGetByFiltersRequest();
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, searchFieldValue);
    AssertSearchActivity()
    cy.get(ActivitySelectors.Backbutton).click()
}

export function NavigatesToCopyActivityWizerd() {
    cy.get(ActivitySelectors.MenuButtons).click()
    cy.Click(ActivitySelectors.CopyActivity, null)
}

export function AssertCreateCopyActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostPhoneCall, 200)
}

export function ReOpenActivity() {
    DefinePutPhoneCallRequest()
    cy.get(ActivitySelectors.MenuButtons).click()
    cy.Click(ActivitySelectors.ReOpenActivity, null)
}

export function CancelActivity() {
    DefinePutPhoneCallRequest()
    cy.get(ActivitySelectors.MenuButtons).click()
    cy.Click(ActivitySelectors.CancelActivity, null)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCancelledLabelExists() {
    BaseAssertion.AssertElementExist(ActivitySelectors.CancelledLabel)
    BaseAssertion.AssertElementTextEqual(ActivitySelectors.CancelledLabel, Constants.Cancelled)
}

export function CompleteActivityFromRecentActivitesList() {
    DefineCompleteActivityRequest();
    cy.get(ActivitySelectors.RecentEntityItem).eq(0).find(ActivitySelectors.Button).click({ force: true });
}

function DefineCompleteActivityRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetCompleteActivity, RequestAliases.GetCompleteActivity);
}

export function AssertCompleteActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetCompleteActivity, 200);
}