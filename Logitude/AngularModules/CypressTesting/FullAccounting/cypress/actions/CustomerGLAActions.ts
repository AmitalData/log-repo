import { CustomersGLASelectors } from "../selectors/CustomersGLASelectors";
import { CustomersGLADetails } from "cypress/models/CustomersGLADetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let SearchFieldValue = null;

export function GetSearchFieldValue() {
    return SearchFieldValue;
}

export function NavigateCutomersWizerd() {
    cy.Click(CustomersGLASelectors.CustomersTab, null);
    cy.Click(CustomersGLASelectors.NewCustomerButton, null);
}

export function FillCustomerDetails(customersGLADetails: CustomersGLADetails) {
    let currentDateTime = gr.GenerateCurrentDatetimeString("")
    cy.FillLogTextBox(CustomersGLASelectors.CompanyName, currentDateTime)
    cy.FillLogTextBox(CustomersGLASelectors.Phone, customersGLADetails.Phone)
    cy.FillLogTextBox(CustomersGLASelectors.Address1, customersGLADetails.Address)
    cy.FillLogTextBox(CustomersGLASelectors.City, customersGLADetails.City)
    cy.FillLogLov(CustomersGLASelectors.Country, customersGLADetails.Country, true)
}

export function CreateCustomer() {
    cy.DefineRequestWait(RestAPI.POST, URLs.PartnersDomain, RequestAliases.PostCustomer)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateCustomer() {
    let intercept = cy.wait("@" + RequestAliases.PostCustomer);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
        SearchFieldValue = interception.response.body.Address.Name
    })
}

export function OpenCutomer() {
    cy.DefineRequestWait(RestAPI.GET, URLs.CustomerGetSingle, RequestAliases.GetSignle);
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function AssertOpenCutomer() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function NavigateNewAccountWizerd() {
    cy.Click(CustomersGLASelectors.AccountingTab, null)
    cy.Click(CustomersGLASelectors.ActivateHyperLink, null)
}

export function AssertARproveARPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}