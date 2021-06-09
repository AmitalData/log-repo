import { CurrencySelectors } from "../selectors/CurrencySelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { CurrencyDetails } from "../models/CurrencyDetails";
import * as GeneralActions from './GeneralActions'
import { GenerateRandomNumber } from '../../../Base/cypress/actions/GenerateRandoms';

let CurrencyCode = null;
let CurrencyCodeCount = null;
let RandomCurrency = null
let inActiveBranch = false;

export function FillCurrencyDetails(currencyDetails: CurrencyDetails) {
    if (currencyDetails.Currency == 'random') {
        SelectCurrencyName()
    }
    else {
        cy.FillLogLov(CurrencySelectors.CurrencyName, CurrencyCode, true)
    }
    
    cy.FillLogTextBox(CurrencySelectors.CurrencyExchangeRate, currencyDetails.Rate)
}

function SelectCurrencyName() {
    cy.get('#searchicon_Tenant_CurrencyId').click().then(() => {
        cy.get('.cdk-virtual-scroll-content-wrapper').last().find('.tooltip').as('CurrencuListCount').then((listing) => {

            CurrencyCodeCount = Cypress.$(listing).length;
            RandomCurrency = GenerateRandomNumber(CurrencySelectors.MinRandomNumber, CurrencyCodeCount)
            cy.get('@CurrencuListCount').eq(RandomCurrency).click()
        })
    })
}

export function CreateCurrency() {
    DefinePostCurrencyRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostCurrencyRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetTenatCurrencies, RequestAliases.PostCurrency)
}

export function AssertErrorMessage() {
    cy.get('.ValidationSummary').should('exist')
}

export function AssertCreateCurrency() {

    let intercept = cy.wait("@" + RequestAliases.PostCurrency);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateCurrency();
        }
        else {
            AssertPostCurrency(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreateCurrency() {
    CreateCurrency();
    AssertCreateCurrency();
}

export function CreateExitingCurrency() {

    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
}

export function AssertPostCurrency(responseStatusCode: number, expectedStatusCode: number, currencyCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    CurrencyCode = currencyCode
}

export function SearchCurrency() {
    GeneralActions.Search(CurrencyCode)
}

export function AssertSearchCurrency() {
    GeneralActions.AssertSearch(CurrencyCode)
}

export function OpenCurrency() {
    DefineCurrenciesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineCurrenciesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CurrenciesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenCurrency() {
    AssertCurrencyGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertCurrencyGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function FillCurrencyLocalName(LocalName: string) {
    cy.FillLogTextBox(CurrencySelectors.CurrencyLocalName, LocalName)
}

export function FillCurrencyAccountingTab(currencyDetails: CurrencyDetails) {
    cy.FillLogTextBox(CurrencySelectors.AccountingExternalID, currencyDetails.AccountingExternalID);
}

export function EditCurrency() {
    DefinePutCurrencyRequest();
    cy.Click(CurrencySelectors.CurrencySaveButton, null);
}

function DefinePutCurrencyRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Currencies, RequestAliases.PutCurrency);
}

export function AssertEditCurrency() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCurrency, 200)
}

export function CloseSaveCurrency() {
    DefineCurrencyViewGetSingleRequest()
    cy.Click(CurrencySelectors.CurrencySaveCloseButton, null);
}

export function AssertCloseSaveCurrency() {
    AssertCurrencyGetSingle();
}

function DefineCurrencyViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CurrenciesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertFaildCreateCurrency() {
    cy.get('.ValidationSummary').should('contain', 'This currency already exists')
}