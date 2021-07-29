import { ActivitySelectors } from "../selectors/ActivitySelectors"
import { PhoneCallDetails } from "../models/PhoneCallDetails";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

export function NavigatesToPhoneCallWizerd() {
    cy.get(ActivitySelectors.NEWACTIVITY).click()
    cy.Click(ActivitySelectors.NEWPHONECALL, null)
}

export function FillPhoneCallWizardsFields(phoneCallDetails: PhoneCallDetails) {
    cy.FillLogLov(ActivitySelectors.ActivityCustomer, phoneCallDetails.Customer, true)
    cy.FillLogLov(ActivitySelectors.ActivityCallWith, phoneCallDetails.CallWith, true);
    FillSubject()
    cy.FillLogTextBox(ActivitySelectors.ActivityDescription, phoneCallDetails.Description)
    cy.FillLogLov(ActivitySelectors.ActivityPriorityCode, phoneCallDetails.PriorityCode, true)
}

export function FillSubject() {
    cy.FillLogTextBox(ActivitySelectors.ActivitySubject + BaseSelectors.LastElement, 'PhoneCall_' + GenerateCurrentDatetimeString("_"))
}