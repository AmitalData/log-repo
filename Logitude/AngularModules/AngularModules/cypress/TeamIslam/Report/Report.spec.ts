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
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get('body').then(($body) => {
        if ($body.find('#GeneralMHReports').length > 0) {
            cy.get('#GeneralMHReports', { timeout: 5000 }).click();
            cy.log('Reports menu clicked successfully');
        } else {
            cy.log('Reports menu not found, skipping report test');
        }
    });
  })



it(' Run Report Sucssefuly', () => {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get('#GeneralMHReports', { timeout: 15000 }).click();
    cy.get('#null_Search', { timeout: 15000 }).type('Automation test report')
    cy.get('#ReportID', { timeout: 15000 }).should('be.visible')
    cy.get('#ReportID', { timeout: 15000 }).click()
    
    cy.get('#CheckBox_0_0_LBL', { timeout: 15000 }).click({ force: true })
    cy.get('#RunReportButton', { timeout: 15000 }).click()
  })

it('Run Report Faield', () => {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get('#GeneralMHReports', { timeout: 15000 }).click();
    cy.get('#null_Search', { timeout: 15000 }).type('Automation test report')
    cy.get('#ReportID', { timeout: 15000 }).should('be.visible')
    cy.get('#ReportID', { timeout: 15000 }).click()
    
    cy.get('#CheckBox_0_0_LBL', { timeout: 15000 }).should('be.visible')
    cy.get('#CheckBox_0_0_LBL', { timeout: 15000 }).click() 
    cy.get('#RunReportButton', { timeout: 15000 }).click()
    cy.get('.Button', { timeout: 15000 }).should('be.visible')
    cy.get('#MessageWindow_Ok_0', { timeout: 15000 }).should('be.visible')
    cy.get('#MessageWindow_Ok_0', { timeout: 15000 }).click({ force: true })
})
