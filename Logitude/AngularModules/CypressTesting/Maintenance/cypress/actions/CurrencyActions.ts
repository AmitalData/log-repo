import { CurrencySelectors } from "../selectors/CurrencySelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { CurrencyDetails } from "../models/CurrencyDetails";
import * as GeneralActions from './GeneralActions'
import { EventTypeDetails } from "../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../Base/cypress/actions/Actions"

let CurrencyCode = null;
var IsActiveCurrency = null;

/*
export function CreateExitingCurrency() {

    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
}
*/
export function FillSearchFeild(currencyDetails: CurrencyDetails) {
    CurrencyCode = currencyDetails.Code
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

export function ChangeInactiveCheckBoxValue(InActivateSelector: string) {
    cy.get(InActivateSelector).then($InActiveStatesCheckBox => {
        if ($InActiveStatesCheckBox.is(':checked')) {
            IsActiveCurrency = true
            cy.get(InActivateSelector).uncheck({ force: true })
        }
        else {
            IsActiveCurrency = false
            cy.get(InActivateSelector).check({ force: true })
        }
    })
}

export function AssertEventTab(eventDetailsList: EventTypeDetails[]) {
    if (IsActiveCurrency) {
        eventDetailsList[0].Notes = 'Currency Activated'
    }
    else {
        eventDetailsList[0].Notes = 'Currency Inactivated'
    }
    BaseActions.ValidateEventsTab(eventDetailsList, CurrencySelectors.CurrencyEventsTab);
}

export function AssertCloseSaveCurrency() {
    AssertCurrencyGetSingle();
}

function DefineCurrencyViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CurrenciesviewGetSingle, RequestAliases.GetSignle);
}
/*
export function AssertFaildCreateCurrency() {
    cy.get('.ValidationSummary').should('contain', 'This currency already exists')
}
*/