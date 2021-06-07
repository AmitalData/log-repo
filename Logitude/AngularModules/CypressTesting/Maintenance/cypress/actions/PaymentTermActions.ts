import { PaymentTermsSelectors } from "../selectors/PaymentTermsSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import { PaymentTermDetails } from "../models/PaymentTermDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

let PaymentTermCode = null;

export function FillPaymentTermDetails(paymentTermDetails: PaymentTermDetails, codeDigits: number) {
    var RandomCode = Actions.GenerateRandomNumber(codeDigits);
    cy.FillLogTextBox(PaymentTermsSelectors.Code, paymentTermDetails.Code.toLowerCase() == "random" ? RandomCode : paymentTermDetails.Code)
    cy.FillLogTextBox(PaymentTermsSelectors.Name, paymentTermDetails.Name);
    Actions.FillCheckBoxProcess(PaymentTermsSelectors.CurrentMonthCheckBox + BaseSelectors.LastElement, paymentTermDetails.CurrentMonth)
    cy.FillLogTextBox(PaymentTermsSelectors.LocalName, paymentTermDetails.LocalName);
    cy.FillLogLov(PaymentTermsSelectors.FromDate, paymentTermDetails.FromDate, true);
    cy.FillLogTextBox(PaymentTermsSelectors.Days, paymentTermDetails.Days);
    cy.FillLogTextBox(PaymentTermsSelectors.Description, paymentTermDetails.Description);
    cy.FillLogTextBox(PaymentTermsSelectors.LocalDescription, paymentTermDetails.LocalDescription);
}

export function CreatePaymentTerm() {
    DefinePostPaymentTermRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostPaymentTermRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PaymentTerms, RequestAliases.PostPaymentTerm);
}

export function AssertCreatePaymentTerm() {
    AssertPostPaymentTerm()
    Actions.AssertGetByFilters()
}

function AssertPostPaymentTerm() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostPaymentTerm, 200).then((interception) => {
        PaymentTermCode = interception.response.body.Code;
    });
}

export function SearchPaymentTerm(paymentTermCode) {
    DefinePaymentTermViewsGetByFiltersRequest(paymentTermCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, paymentTermCode);
    AssertPaymentTermViewsGetByFilters();
}

function DefinePaymentTermViewsGetByFiltersRequest(PaymentTermCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(PaymentTermCode), RequestAliases.GetFilterSearch);
}

function AssertPaymentTermViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function getPaymentTermCode() {
    return PaymentTermCode
}

export function AssertSearchPaymentTerm(paymentTermCode) {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(paymentTermCode);
    });
}

export function OpenPaymentTerm() {
    DefinePaymentTermGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefinePaymentTermGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.PaymentTermsGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenPaymentTerm() {
    AssertPaymentTermGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertPaymentTermGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditPaymentTermGeneralTab(paymentTermDetails: PaymentTermDetails) {
    cy.FillLogTextBox(PaymentTermsSelectors.Description, paymentTermDetails.Description);
    cy.FillLogTextBox(PaymentTermsSelectors.LocalDescription, paymentTermDetails.LocalDescription);
}

export function FillPaymentTermAccountingTab(paymentTermDetails: PaymentTermDetails) {
    cy.FillLogTextBox(PaymentTermsSelectors.AccountingExternalID, paymentTermDetails.AccountingExternalID);
}

export function UpdatePaymentTerm() {
    DefinePutPaymentTermRequest()
    cy.Click(PaymentTermsSelectors.SaveButton, null)
}

function DefinePutPaymentTermRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PaymentTerms, RequestAliases.PutPaymentTerm);
}

export function AssertUpdatePaymentTerm() {
    AssertPutPaymentTerm()
}

function AssertPutPaymentTerm() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutPaymentTerm, 200);
}

export function CloseSavePaymentTerm() {
    DefinePaymentTermViewGetSingleRequest()
    cy.Click(PaymentTermsSelectors.SaveCloseButton, null);
}

function DefinePaymentTermViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.PaymentTermsviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSavePaymentTerm() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}