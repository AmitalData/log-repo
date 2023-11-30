import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { CurrencyRatesDetails } from "cypress/models/CurrencyRatesDetails";
import { CurrencyRatesSelectors } from '../selectors/CurrencyRatesSelectors';


export function NavigatesCurrencyRatesWizerd() {
    
    cy.Click(BaseExportSelectors.CustomsRequest, null)
    cy.Click(CurrencyRatesSelectors.RequestSheet, null) 
}

export function FillCurrencyRatesDetails(CurrencyRatesDetails: CurrencyRatesDetails) {
  
    const now = new Date();
    cy.FillDate(CurrencyRatesSelectors.FromDate,now.toLocaleDateString('fr-FR') )
    cy.FillDate(CurrencyRatesSelectors.ToDate, now.toLocaleDateString('fr-FR'))
    cy.FillLogLov(CurrencyRatesSelectors.CurrencyTypeId, CurrencyRatesDetails.CurrencyTypeId, true);
}

export function SendToCustom() {
    
    cy.Click(CurrencyRatesSelectors.CustomSendOptionsComponent_3, null);
}

export function FoundCurrencyRate()
{
    //cy.DefineRequestWait(RestAPI.POST, URLs.APInvoices, RequestAliases.APInvoicesRequest)
    //console.log(RestAPI.POST);
    //console.log( URLs.APInvoices);
    

    BaseAssertion.AssertElementExist(CurrencyRatesSelectors.CurrencyRateFirstRow);
    //todo:ORIT
    //BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200)
}

