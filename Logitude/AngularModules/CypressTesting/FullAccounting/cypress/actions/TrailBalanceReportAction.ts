
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
    cy.Click(TrailBalanceReportSelectors.SelectReportRun, null, true);    
    cy.wait(1000); // Wait for report button to become available
    // Wait for the green button to exist and be visible, then click it
    cy.get('.greenButton, .GreenButton', { timeout: 10000 }).first().should('exist').click({ force: true });
}

export function AssertRunTrailBalanceReport() {
    cy.get(TrailBalanceReportSelectors.TrailBalanceReporScreen).should('be.visible')
}

