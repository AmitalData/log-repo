import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { CurrencyRateRunInBatchDetails } from "cypress/models/CurrencyRateRunInBatchDetails";
import { CurrencyRateRunInBatchSelectors } from '../selectors/CurrencyRateRunInBatchSelectors';


export function NavigatesCurrencyRatesWizerd() {

    cy.Click(BaseExportSelectors.CustomsRequest, null)
    cy.Click(CurrencyRateRunInBatchSelectors.RequestSheet, null)
}

export function FillCurrencyRatesDetails(CurrencyRateRunInBatchDetails: CurrencyRateRunInBatchDetails) {

    const now = new Date();
    cy.FillDate(CurrencyRateRunInBatchSelectors.FromDate, now.toLocaleDateString('fr-FR'));
    cy.FillDate(CurrencyRateRunInBatchSelectors.ToDate, now.toLocaleDateString('fr-FR'));
    cy.FillLogLov(CurrencyRateRunInBatchSelectors.CurrencyTypeId, CurrencyRateRunInBatchDetails.CurrencyTypeId, true);
    cy.Click(CurrencyRateRunInBatchSelectors.SendOptions, null);
    cy.Click(CurrencyRateRunInBatchSelectors.RunInBatch, null);
    cy.Click(CurrencyRateRunInBatchSelectors.Approve, null);
    cy.Click(CurrencyRateRunInBatchSelectors.Cancel, null);
    
    // Wait for processing to complete with shorter timeout
    cy.wait(3000);
    cy.Click(CurrencyRateRunInBatchSelectors.RequestsSheets, null);
    cy.Click(CurrencyRateRunInBatchSelectors.Search, null);

}

export function FillRequestSheets(currencyRateRunInBatchDetails: CurrencyRateRunInBatchDetails) {
    // Wait for data to load with shorter timeout
    cy.wait(10000);
    cy.FillLogLov(CurrencyRateRunInBatchSelectors.ManageCustomsRequests, currencyRateRunInBatchDetails.ManageCustomsRequests, true);
    cy.get(CurrencyRateRunInBatchSelectors.RequestStatus).then($status => {
        if ($status.length) {
            cy.get(CurrencyRateRunInBatchSelectors.RequestStatus).click({ force: true });
        } else {
            cy.get(CurrencyRateRunInBatchSelectors.RequestStatus1).click({ force: true });
        }
    });
    
    // Wait for CheckAll checkbox to be visible and clickable with shorter timeout
    cy.get(CurrencyRateRunInBatchSelectors.CheckAll, { timeout: 10000 })
        .should('be.visible')
        .click({ force: true });
    
    cy.get(CurrencyRateRunInBatchSelectors.Reference).click();

}


export function CheckStatusRequest() {

    // Use more robust selector for grid rows with shorter timeout
    cy.get('[id*="LogGrid"][id*="rowtemplate"]:first', { timeout: 10000 }).then(($elements) => {
        // בדיקת מספר האלמנטים
        expect($elements.length).to.be.greaterThan(0);

        // בדיקת טקסט בתוך האלמנט הראשון
        expect($elements.first().text()).not.to.include('בקשה נכשלה');
    });

}








