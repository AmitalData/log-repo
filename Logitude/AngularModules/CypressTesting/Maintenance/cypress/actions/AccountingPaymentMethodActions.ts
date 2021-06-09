import { AccountingPaymentMethodSelectors } from "../selectors/AccountingPaymentMethodSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import * as GeneralActions from "./GeneralActions";
import { AccountingPaymentMethodDetails } from "../models/AccountingPaymentMethodDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';

let SearchFieldValue = null

export function FillAccountingPaymentMethodDetails(AccountingPaymentMethodDetails: AccountingPaymentMethodDetails, codeDigits: number) {
    cy.FillLogTextBox(AccountingPaymentMethodSelectors.Name, AccountingPaymentMethodDetails.Name)
    cy.FillLogTextBox(AccountingPaymentMethodSelectors.Code, GenerateRandomNumberAndString(codeDigits));
    Actions.FillCheckBoxProcess(AccountingPaymentMethodSelectors.ARCheckBox, AccountingPaymentMethodDetails.ARCheckBox)
    Actions.FillCheckBoxProcess(AccountingPaymentMethodSelectors.APCheckBox, AccountingPaymentMethodDetails.APCheckBox)
}

export function CreateAccountingPaymentMethod() {
    DefinePostAccountingPaymentMethodRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostAccountingPaymentMethodRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AccountingPaymentMethods, RequestAliases.PostAccountingPaymentMethod);
}

export function AssertCreateAccountingPaymentMethod() {
    AssertPostAccountingPaymentMethod()
    Actions.AssertGetByFilters()
}

function AssertPostAccountingPaymentMethod() {
    let intercept = cy.wait("@" + RequestAliases.PostAccountingPaymentMethod);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        if (statusCode === 400) {
            ReCreateAccountingPaymentMethod();
        }
        else {
            assert.equal(statusCode, 200)
            SearchFieldValue = interception.response.body.Code
        }
    })
}

function ReCreateAccountingPaymentMethod() {
    let RandomCode = Actions.GenerateRandomNumber(4);
    cy.FillLogTextBox(AccountingPaymentMethodSelectors.Code, RandomCode)
    CreateAccountingPaymentMethod();
    AssertCreateAccountingPaymentMethod();
}

export function SearchAccountingPaymentMethod() {
    GeneralActions.Search(SearchFieldValue)
}

export function AssertSearchAccountingPaymentMethod() {
    GeneralActions.AssertSearch(SearchFieldValue);
}

export function OpenAccountingPaymentMethod() {
    DefineAccountingPaymentMethodGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineAccountingPaymentMethodGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.AccountingPaymentMethodsGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenAccountingPaymentMethod() {
    AssertAccountingPaymentMethodGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertAccountingPaymentMethodGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditAccountingPaymentMethodGeneralTab(AccountingPaymentMethodDetails: AccountingPaymentMethodDetails) {
    Actions.FillInputCheckBoxProcess(AccountingPaymentMethodSelectors.InActiveCheckBox, AccountingPaymentMethodDetails.InactiveCheckBox)
    Actions.FillInputCheckBoxProcess(AccountingPaymentMethodSelectors.APCheckBox, AccountingPaymentMethodDetails.APCheckBox)
}

export function FillAccountingPaymentMethodAccountingTab(AccountingPaymentMethodDetails: AccountingPaymentMethodDetails) {
    cy.FillLogTextBox(AccountingPaymentMethodSelectors.AccountingARExternalID, AccountingPaymentMethodDetails.ARExternalID);
    cy.FillLogTextBox(AccountingPaymentMethodSelectors.AccountingAPExternalID, AccountingPaymentMethodDetails.APExternalID);
}

export function UpdateAccountingPaymentMethod() {
    DefinePutAccountingPaymentMethodRequest()
    cy.Click(AccountingPaymentMethodSelectors.SaveButton, null)
}

function DefinePutAccountingPaymentMethodRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.AccountingPaymentMethods, RequestAliases.PutAccountingPaymentMethod);
}

export function AssertUpdateAccountingPaymentMethod() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutAccountingPaymentMethod, 200);
}

export function CloseSaveAccountingPaymentMethod() {
    DefineAccountingPaymentMethodViewGetSingleRequest()
    cy.Click(AccountingPaymentMethodSelectors.SaveCloseButton, null);
}

function DefineAccountingPaymentMethodViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.AccountingPaymentMethodsviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveAccountingPaymentMethod() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}