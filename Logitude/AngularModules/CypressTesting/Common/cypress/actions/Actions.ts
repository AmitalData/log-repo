import { CommonSelectors } from "../selectors/Selectors"
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion"
import { CustomerDetails } from "../models/CustomerDetails";

//#region Reset password
export function FillChangePasswordPage(CurrentPassword:string,password: string , confirmPassword:string) {
    cy.FillLogTextBox(CommonSelectors.CurrentPassword ,CurrentPassword)
    cy.FillLogTextBox(CommonSelectors.Password , password)
    if(confirmPassword!=null){
        cy.FillLogTextBox(CommonSelectors.ConfirmPassword ,confirmPassword)
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

