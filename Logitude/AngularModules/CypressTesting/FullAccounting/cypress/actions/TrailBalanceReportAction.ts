import { JournalSelectors } from "../selectors/JournalSelectors";
import { JournalLineActionDetails } from "cypress/models/JournalLineActionDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TrailBalanceReportDetails } from "cypress/models/TrailBalanceReportDetails";
import { TrailBalanceReportSelectors } from "../selectors/TrailBalanceReportSelectors";



export function NavigatesTrailBalanceReportWizerd() {
    
    cy.Click(TrailBalanceReportSelectors.RequestSheet, null)
    cy.Click(TrailBalanceReportSelectors.Reportbtn, null) 
}

export function FillTrailBalanceReportDetails(tailBalanceReportDetails:TrailBalanceReportDetails ) {
  
    const now = new Date();
    cy.FillDate(TrailBalanceReportSelectors.FromDate,now.toLocaleDateString('fr-FR') )
    cy.FillDate(TrailBalanceReportSelectors.ToDate, now.toLocaleDateString('fr-FR'))
    
}

export function RunTrailBalanceReport() {
    cy.Click(TrailBalanceReportSelectors.GreenButton, null);
}

export function AssertRunTrailBalanceReport() {
    cy.get(TrailBalanceReportSelectors.TrailBalanceReporScreen).should('be.visible')
}

