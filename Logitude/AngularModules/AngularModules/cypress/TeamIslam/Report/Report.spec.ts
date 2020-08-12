/// <reference types="cypress" />
  

  //login
/// <reference types="cypress" />
  
let timeStamp = (new Date()).getTime()

//login


import { LoginComp } from '../../Login/Login.po';

export class RunReport {

private login: LoginComp = new LoginComp();

constructor() {
}
}




it('Search For Report', () => {
    //cy.wait(10000)
    cy.get('#GeneralMHReports').click();
    cy.get('#null_Search').type('Automation test report')
    cy.get('#ReportID').should('be.visible')
    cy.get('#ReportID').click()

  })



it(' Run Report Sucssefuly', () => {
   // cy.wait(10000)
    //cy.wait(100)
    cy.get('#CheckBox_0_0_LBL').click({ force: true })
    cy.get('#RunReportButton').click()

  })

it('Run Report Faield', () => {
    cy.wait(1000)
    cy.get('#CheckBox_0_0_LBL').should('be.visible')
    cy.get('#CheckBox_0_0_LBL').click() 
    cy.get('#RunReportButton').click()
    cy.wait(5000)
    cy.get('.Button').should('be.visible')
    cy.get('#MessageWindow_Ok_0').should('be.visible')
    cy.get('#MessageWindow_Ok_0').click({ force: true })


})
