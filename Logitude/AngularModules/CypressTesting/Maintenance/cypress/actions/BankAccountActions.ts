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

let BankAccountEnglishName = null;
let inActiveBankAccount = false;

function GenerateRandomNumber(NumberLength: number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(NumberLength)
    return NewRandomCode;
}

export function FillBankAccountDetails(bankAccountDetails: BankAccountDetails) {
   
    let RandomBankAccountCode = GenerateRandomNumber(BankAccountSelectors.CodeDigitCount);
    let CurrentDateName = GenerateCurrentDatetimeString("_")

    cy.FillLogTextBox(BankAccountSelectors.BankAccountAccountNumber, bankAccountDetails.AccountNumber)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountBankCode, bankAccountDetails.BankCode.toLowerCase() == "random" ? RandomBankAccountCode : bankAccountDetails.BankCode)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountBranchNumber, bankAccountDetails.BranchNumber)
    cy.FillLogLov(BankAccountSelectors.BankAccountCurrency, bankAccountDetails.Currency, true)
    cy.FillLogTextBox(BankAccountSelectors.BankAccountName, bankAccountDetails.Name.toLowerCase() == "currentdate" ? CurrentDateName : bankAccountDetails.Name)
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
    DefineBankAccountViewsGetByFiltersRequest(BankAccountEnglishName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, BankAccountEnglishName);
    AssertBankAccountViewsGetByFilters();
}

export function DefineBankAccountViewsGetByFiltersRequest(BankAccountEnglishName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(BankAccountEnglishName + "&GetCount=false"), RequestAliases.GetFilterSearch);
}

export function AssertBankAccountViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearchBankAccount() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(BankAccountEnglishName);
    });
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
    AssertPutBankAccount();
}

export function AssertPutBankAccount() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutBankAccount, 200).
        then((interception) => {
            inActiveBankAccount = interception.response.body.InActive;
        });
}

