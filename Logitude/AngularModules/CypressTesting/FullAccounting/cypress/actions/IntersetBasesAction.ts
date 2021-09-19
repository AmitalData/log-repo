import { InterestBasesSelectors } from "../selectors/InterestBasesSelectors";
import { InterestBasesDetails } from "cypress/models/InterestBasesDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';

export function NavigatesInterestWizerd() {
    cy.Click(InterestBasesSelectors.InterestTab, null)
    cy.Click(InterestBasesSelectors.NewInterestBaseButton, null)
}

export function FillInterestBasesDetails(interestBasesDetails: InterestBasesDetails) {
    cy.FillLogTextBox(InterestBasesSelectors.Code, GenerateRandomNumberAndString(4));
    cy.FillLogTextBox(InterestBasesSelectors.EnglishName, interestBasesDetails.EnglishName)
    cy.FillLogTextBox(InterestBasesSelectors.LocalName, interestBasesDetails.LocalName)
    cy.FillLogTextBox(InterestBasesSelectors.Description, interestBasesDetails.Description)
}

export function CreateInterestBases() {
    cy.DefineRequestWait(RestAPI.POST, URLs.InterestBases, RequestAliases.PostInterestBases)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateInterestBases() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.PostInterestBases, 200)
}

export function AddInterestPeriods(interestBasesDetails: InterestBasesDetails) {
    cy.Click(InterestBasesSelectors.AddInterestPeriodsButton, null)
    cy.FillLogTextBox(InterestBasesSelectors.InterestBaseStartDate, interestBasesDetails.InterestBaseStartDate)
    cy.FillLogTextBox(InterestBasesSelectors.InterestBaseRate, interestBasesDetails.InterestBaseRate)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function SaveInterestBases() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.InterestBases, RequestAliases.PutInterestBases);
    cy.Click(InterestBasesSelectors.SaveButton, null)
}

export function ASsertSaveInterestBases() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutInterestBases, 200)
}