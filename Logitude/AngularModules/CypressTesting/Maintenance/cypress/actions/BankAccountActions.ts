import { MaintenanceSelectors } from "../selectors/Selectors";
import { BankAccountSelectors } from "../selectors/BankAccountSelectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import { BankAccountDetails } from '../models/BankAccountDetails'
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';
import * as GeneralActions from './BaseActions'

let BankAccountEnglishName = null;
let inActiveBankAccount = false;

function GenerateRandomNumber(numberLength: number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(numberLength)
    return NewRandomCode;
}

export function FillBankAccountDetails(bankAccountDetails: BankAccountDetails) {

    let RandomBankAccountCode = GenerateRandomNumber(BankAccountSelectors.CodeDigitCount);
    let CurrentDateName = GenerateCurrentDatetimeString("_")

    cy.FillLogTextBox(BankAccountSelectors.BankAccountAccountNumber, bankAccountDetails.AccountNumber)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountBankCode, RandomBankAccountCode)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountBranchNumber, bankAccountDetails.BranchNumber)
    cy.FillLogLov(BankAccountSelectors.BankAccountCurrency, bankAccountDetails.Currency, true)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountName, CurrentDateName)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountLocalName, bankAccountDetails.LocalName)
}

export function CreateBankAccount() {
    DefinePostBankAccountRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostBankAccountRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.BankAccounts, RequestAliases.PostBankAccount)
}

export function AssertCreateBankAccount() {
    let intercept = cy.wait("@" + RequestAliases.PostBankAccount);
    intercept.then((interception) => {
        AssertPostBankAccount(interception.response.statusCode, 200, interception.response.body.EnglishName)
    })
}

export function AssertPostBankAccount(responseStatusCode: number, expectedStatusCode: number, bankAccountEnglishName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    BankAccountEnglishName = bankAccountEnglishName
}

export function SearchBankAccount() {
    GeneralActions.Search(BankAccountEnglishName)
}

export function AssertSearchBankAccount() {
    GeneralActions.AssertSearch(BankAccountEnglishName)
}

export function OpenBankAccount() {
    DefineBankAccountsGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineBankAccountsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BankAccountsGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenBankAccount() {
    AssertBankAccountGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertBankAccountGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function FillBankAccountLocalName(LocalName: string) {
    cy.FillLogTextBox(BankAccountSelectors.BankAccountLocalName, LocalName)
}

export function EditBankAccount() {
    DefinePutBankAccountRequest();
    cy.Click(BankAccountSelectors.BankAccountSaveButton, null);
}

function DefinePutBankAccountRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.BankAccounts, RequestAliases.PutBankAccount);
}

export function AssertEditBankAccount() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutBankAccount, 200)
}

export function CloseSaveBankAccount() {
    DefineBankAccountiewGetSingleRequest()
    cy.Click(BankAccountSelectors.BankAccountSaveCloseButton, null);
}

export function AssertCloseSaveBankAccount() {
    AssertBankAccountGetSingle();
}

function DefineBankAccountiewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BankAccountsviewGetSingle, RequestAliases.GetSignle);
}

