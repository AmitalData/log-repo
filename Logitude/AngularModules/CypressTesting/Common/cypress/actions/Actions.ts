import { CommonSelectors } from "../selectors/Selectors"
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion"
import { CustomerDetails } from "../models/CustomerDetails";
import { LoginDetails } from 'cypress/models/LoginDetails';
import { ValidationMessageDetails } from "../../../Base/cypress/models/ValidationMessageDetails";
import * as Assists from "../../../Base/cypress/assists/Assists";
import * as gr from "../../../Base/cypress/actions/GenerateRandoms";
import { Constants } from "../constants/Constants";
//#region Reset password
export function FillChangePasswordPage(CurrentPassword: string, password: string, confirmPassword: string) {
    cy.FillLogTextBox(CommonSelectors.CurrentPassword, CurrentPassword)
    cy.FillLogTextBox(CommonSelectors.Password, password)
    if (confirmPassword != null) {
        cy.FillLogTextBox(CommonSelectors.ConfirmPassword, confirmPassword)
    }
}
//#endregion
//#region Customer actions
export function NavigatesToCustomersWorkspace() {
    cy.Click(BaseSelectors.CustomersMenu, null);
}

export function AddNewCustomer(customerDetails: CustomerDetails) {
    cy.Click(CommonSelectors.NewCustomer, null);
    cy.FillLogTextBox(CommonSelectors.CustomerCompanyName, customerDetails.CompanyName);
    cy.FillLogTextBox(CommonSelectors.CustomerCity, customerDetails.City);
    cy.FillLogLov(CommonSelectors.CustomerCountry, customerDetails.Country, true);
    cy.FillLogLov(CommonSelectors.CustomerState, customerDetails.State, true);
    cy.FillLogTextBox(CommonSelectors.CustomerPhoneNumber,customerDetails.PhoneNumber)
    cy.FillLogTextBox(CommonSelectors.CustomerFaxNumber,customerDetails.FaxNumber)

}

export function CreateCustomer() {
    cy.DefineRequestWait(RestAPI.POST, URLs.PartnersDomain, RequestAliases.PartnersDomainRequest)
    cy.Click(CommonSelectors.AddCustomer, null);
}

export function UpdateCustomer() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Customers, RequestAliases.Customers)
    cy.Click(CommonSelectors.CustomerSave, null)
}

export function OpenCustomer(customerCode: string) {
    cy.FillLogTextBox(BaseSelectors.SearchField, customerCode);
    cy.DefineRequestWait(RestAPI.GET, URLs.CustomerViews, RequestAliases.CustomerViews)
    BaseAssertion.AssertStatusCode(RequestAliases.CustomerViews, 200)
    cy.DefineRequestWait(RestAPI.GET, URLs.CustomerViews, RequestAliases.CustomerViews)
    BaseAssertion.AssertStatusCode(RequestAliases.CustomerViews, 200)
    cy.Click(BaseSelectors.ListItem, null, false)
}
//#endregion
//#region login
export function Login() {
    cy.DefineRequestWait(RestAPI.POST, URLs.Authentication, RequestAliases.Authentication)
    cy.Click(CommonSelectors.LoginButton, null);
}
export function FillLoginDetails(loginDetails: LoginDetails) {
    FillEmail(loginDetails.Email);
    FillPassword(loginDetails.Password);
}
export function FillEmail(Email: string) {
    cy.FillLogTextBox(CommonSelectors.Email, Email);
}
function FillPassword(Password: string) {
    let PasswordToFill = Password.toLowerCase() == Constants.random ? GetRandomPassword() : Password;
    cy.FillLogTextBox(CommonSelectors.Password, PasswordToFill);
}
function GetRandomPassword(){
    return gr.GenerateRandomString(6,true).toString()
}
export function ValidateErrorMessage(Message: string) {
    cy.ValidateElementColor(CommonSelectors.ErrorList, BaseSelectors.RedColor);
    cy.get(CommonSelectors.ErrorList).should("contain.text", Message)
}
//#endregion
//#region Password Reset Request
export function VisitPasswordResetRequestPage() {
    cy.RedirectToLogin();
    ClickForgotYourPassword()
}
function ClickForgotYourPassword(){
    cy.DefineRequestWait(RestAPI.GET, URLs.PasswordResetRequestPage, RequestAliases.PasswordResetRequestPage)
    cy.Click(BaseSelectors.Anchor, CommonSelectors.ForgotYourPasswordLink)
    BaseAssertion.AssertStatusCode(RequestAliases.PasswordResetRequestPage, 200)
}
export function SendPasswordResetRequest() {
    cy.DefineRequestWait(RestAPI.POST, URLs.ResetPassword, RequestAliases.ResetPassword)
    cy.Click(CommonSelectors.SubmitButton, null)
}
export function ValidateMessagesForSendingPasswordResetRequest(dataTable: any) {
    let validationMessageDetailsList = Assists.CreateSet<ValidationMessageDetails>(dataTable);
    for (let i = 0; i < validationMessageDetailsList.length; i++) {
        cy.get(CommonSelectors. ResetPasswordLinkSentMessage).should('contain.text', validationMessageDetailsList[i].Message)
    }
}
//#endregion