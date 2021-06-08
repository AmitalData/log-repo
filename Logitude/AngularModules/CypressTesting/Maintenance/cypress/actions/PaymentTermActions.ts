import { PaymentTermsSelectors } from "../selectors/PaymentTermsSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import * as GeneralActions from "./GeneralActions";
import { PaymentTermDetails } from "../models/PaymentTermDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let SearchFieldValue = null

export function FillPaymentTermDetails(paymentTermDetails: PaymentTermDetails, codeDigits: number) {
    cy.FillLogTextBox(PaymentTermsSelectors.Name, gr.GenerateCurrentDatetimeString("_"))
    cy.FillLogTextBox(PaymentTermsSelectors.LocalName, paymentTermDetails.LocalName);
    cy.FillLogTextBox(PaymentTermsSelectors.Code, gr.GenerateRandomNumberAndString(codeDigits));
    Actions.FillCheckBoxProcess(PaymentTermsSelectors.CurrentMonthCheckBox + BaseSelectors.LastElement, paymentTermDetails.CurrentMonth)
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
    let intercept = cy.wait("@" + RequestAliases.PostPaymentTerm);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        if (statusCode === 400) {
            ReCreatePaymentTerm();
        }
        else {
            assert.equal(statusCode, 200)
            SearchFieldValue = interception.response.body.EnglishName
            cy.log(SearchFieldValue)
        }
    })
}

function ReCreatePaymentTerm() {
    let RandomCode = Actions.GenerateRandomNumber(4);
    cy.FillLogTextBox(PaymentTermsSelectors.Code, RandomCode)
    CreatePaymentTerm();
    AssertCreatePaymentTerm();
}

export function SearchPaymentTerm() {
    GeneralActions.Search(SearchFieldValue)
}

export function AssertSearchPaymentTerm() {
    GeneralActions.AssertSearch(SearchFieldValue);
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
    cy.ClickCheckBox(PaymentTermsSelectors.InActiveCheckBox)
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