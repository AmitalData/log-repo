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
    cy.FillLogTextBox(UsersSelectors.UserEmail, searchFieldValue + userDetails.Email)
    cy.FillLogTextBox(UsersSelectors.UserPassword, userDetails.Password);
    cy.FillLogTextBox(UsersSelectors.ReTypeUserPassword, userDetails.ReTypePassword);
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

export function Login(password) {
    let mode = Cypress.env("Mode");
    if (mode.toLowerCase() === "development") {
        cy.fixture("Login.json").then(loginData => {
            let url = loginData.url;
            CompleteLoginProcess(url, password);
        });
    }
    else {
        let url = Cypress.env("Url");
        CompleteLoginProcess(url, password);
    }
}

function CompleteLoginProcess(url, password) {
    cy.visit(url)
    cy.FillLogTextBox(UsersSelectors.Email, searchFieldValue + "@mail.com")
    cy.FillLogTextBox(UsersSelectors.Password, password)
    cy.get(UsersSelectors.cmdLogin).click()
}

export function ResetPassword(oldPassword, newPassword) {
    cy.FillLogTextBox(UsersSelectors.CurrentPassword, oldPassword)
    cy.FillLogTextBox(UsersSelectors.Password, newPassword)
    cy.FillLogTextBox(UsersSelectors.ConfirmNewPassword, newPassword)
    cy.get(UsersSelectors.Submit).click()
}