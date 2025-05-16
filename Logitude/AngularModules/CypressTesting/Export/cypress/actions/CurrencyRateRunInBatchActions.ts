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
    cy.FillDate(CurrencyRateRunInBatchSelectors.FromDate,now.toLocaleDateString('fr-FR') );
    cy.FillDate(CurrencyRateRunInBatchSelectors.ToDate, now.toLocaleDateString('fr-FR'));
    cy.FillLogLov(CurrencyRateRunInBatchSelectors.CurrencyTypeId, CurrencyRateRunInBatchDetails.CurrencyTypeId, true);
    cy.Click(CurrencyRateRunInBatchSelectors.SendOptions,null);
    cy.Click(CurrencyRateRunInBatchSelectors.RunInBatch,null);
    cy.Click(CurrencyRateRunInBatchSelectors.Approve,null);
    cy.Click(CurrencyRateRunInBatchSelectors.Cancel,null);
    cy.wait(200000);
    cy.Click(CurrencyRateRunInBatchSelectors.RequestsSheets,null);
    cy.Click(CurrencyRateRunInBatchSelectors.Search, null);

}

export function FillRequestSheets(currencyRateRunInBatchDetails: CurrencyRateRunInBatchDetails) {
    cy.wait(20000);

       cy.FillLogLov(CurrencyRateRunInBatchSelectors.ManageCustomsRequests,currencyRateRunInBatchDetails.ManageCustomsRequests,true);
    // cy.get(CurrencyRateRunInBatchSelectors.ManageCustomsRequests).type(currencyRateRunInBatchDetails.ManageCustomsRequests);
    cy.Click(CurrencyRateRunInBatchSelectors.RequestStatus,null);
    cy.get(CurrencyRateRunInBatchSelectors.CheckAll).click({force: true});
    cy.get(CurrencyRateRunInBatchSelectors.Reference).click();
    
}


export function CheckStatusRequest() {
    
    cy.get('#LogGrid_0_12rowtemplate0').then(($elements) => {
        // בדיקת מספר האלמנטים
        expect($elements.length).to.be.greaterThan(0);
      
        // בדיקת טקסט בתוך האלמנט הראשון
        expect($elements.first().text()).to.contain('תשובה נותחה');
      });

}
   



  



