import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { ActivitiesDetails } from "../models/ActivitiesDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ActivitiesSelectors } from "../selectors/ActivitiesSelectors"
import * as ActivitiesActions from "./ActivitiesActions";

export function FillAppointmentWizardsFields(activitiesDetails: ActivitiesDetails) {
    cy.FillLogLov(ActivitiesSelectors.ActivityCustomer, activitiesDetails.Customer, true)
    ActivitiesActions.FillSubject("Appointment_")
    cy.FillLogTextBox(ActivitiesSelectors.ActivityDescription, activitiesDetails.Description)
    cy.FillLogLov(ActivitiesSelectors.ActivityPriorityCode, activitiesDetails.PriorityCode, true)
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