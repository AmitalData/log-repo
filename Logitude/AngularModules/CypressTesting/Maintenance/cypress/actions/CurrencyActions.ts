import { CurrencySelectors } from "../selectors/CurrencySelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { CurrencyDetails } from "../models/CurrencyDetails";
import * as Actions from "./Actions"
import { constants } from "../../../Base/cypress/constants/constants";

export function FillCurrencyDetails(currencyDetails: CurrencyDetails) {
    cy.FillLogLov(CurrencySelectors.Currency, currencyDetails.Currency, true)
    cy.FillLogTextBox(CurrencySelectors.ExchangeRate, currencyDetails.ExchangeRate)
    cy.FillLogTextBox(CurrencySelectors.ExchangeRateDate, currencyDetails.ExchangeRateDate)
}

export function MockCreateCurrency(selector) {
    cy.intercept(RestAPI.GET, selector, [true])
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
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

export function FillCurrencyNotes(notes: string) {
    cy.FillLogTextBox(CurrencySelectors.Notes, notes)
}

export function FillCurrencyAccountingExternalId(externalId) {
    cy.FillLogTextBox(CurrencySelectors.AccountingExternalID, " ")
    cy.FillLogTextBox(CurrencySelectors.AccountingExternalID, externalId);
}

export function EditCurrency() {
    DefinePutCurrencyRequest();
    cy.Click(CurrencySelectors.SaveButton, null);
}

function DefinePutCurrencyRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Currencies, RequestAliases.PutCurrency);
}

export function AssertEditCurrency() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCurrency, 200)
}

export function CloseSaveCurrency() {
    DefineCurrencyViewGetSingleRequest()
    cy.Click(CurrencySelectors.SaveCloseButton, null);
}

export function AssertCloseSaveCurrency() {
    AssertCurrencyGetSingle();
}

function DefineCurrencyViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CurrenciesviewGetSingle, RequestAliases.GetSignle);
}