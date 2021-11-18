import { UsersSelectors } from "../selectors/UsersSelectors";
import { UserDetails } from "../models/UserDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null

export function NavigateUserWizerd() {
    cy.Click(UsersSelectors.NewUserButton, null)
}

export function FillUserDetails(userDetails: UserDetails) {
    searchFieldValue = gr.GenerateCurrentDatetimeString("_")
    cy.FillLogTextBox(UsersSelectors.Email, searchFieldValue + userDetails.Email)
    cy.FillLogTextBox(UsersSelectors.Password, userDetails.Password);
    cy.FillLogTextBox(UsersSelectors.ReTypePassword, userDetails.ReTypePassword);
    cy.FillLogTextBox(UsersSelectors.Name, userDetails.Name);
    cy.FillLogLov(UsersSelectors.Depatment, userDetails.Department, true);
    cy.FillLogLov(UsersSelectors.Branch, userDetails.Branch, true);
    cy.FillLogTextBox(UsersSelectors.Notes, userDetails.Notes);
    cy.get(BaseSelectors.CheckBoxLabel).eq(1).click()
}

export function CreateUser() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Users, RequestAliases.PostUser);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateUser() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostUser, 200)
}

export function SearchUser() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetQuickSearch(searchFieldValue), RequestAliases.GetFilterSearch);
    cy.FillLogTextBox(UsersSelectors.SearchTextboxInput, searchFieldValue);
}

export function AssertSearchUser() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function OpenUser() {
    cy.DefineRequestWait(RestAPI.GET, Urls.UsersGetSingle, RequestAliases.GetSignle);
    cy.get(BaseSelectors.QuickSearchDropDown)
        .within(() => {
            cy.get('ul > li').eq(0).click({ force: true });
        });
}

export function AssertOpenUser() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function FillNotes(notes) {
    cy.FillLogTextBox(UsersSelectors.Notes, notes);
}

export function UpdateUser() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Users, RequestAliases.PutUser);
    cy.Click(UsersSelectors.SaveButton, null)
}

export function AssertUpdateUser() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutUser, 200);
}

export function getSearchFieldValue() {
    return searchFieldValue
}