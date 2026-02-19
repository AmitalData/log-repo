
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
    cy.Click(TrailBalanceReportSelectors.SelectReportRun, null);    
    cy.Click(TrailBalanceReportSelectors.RunReportButton, null);
}

export function AssertRunTrailBalanceReport() {
    cy.get(TrailBalanceReportSelectors.TrailBalanceReporScreen).should('be.visible')
}

