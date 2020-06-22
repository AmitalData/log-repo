/// <reference types="cypress" />
  

  //login
it('Sucessfully Login ', () => {
    cy.visit('https://test.logitudeworld.com/test/')
    cy.get('#Email').click()
    cy.get('#Email').clear()
    cy.get('#Email').type('Raghad@protractor.com')
    cy.get('#Password').click()
    cy.get('#Password').clear()
    cy.get('#Password').type('!RS123Rs')
    cy.get('#cmdLogin').click()
  })



it('Search For Report', () => {
    //cy.wait(10000)
    cy.get('#GeneralMHReports').click();
    cy.get('#null_Search').type('Automation test report')
    cy.get('#ReportID').click()

  })



it(' Run Report Sucssefuly', () => {
   // cy.wait(10000)
    //cy.wait(100)
    cy.get('#CheckBox_0_0_LBL').click()
    cy.get('#RunReportButton').click()

  })

it('Run Report Faield', () => {
    cy.wait(1000)
    cy.get('#CheckBox_0_0_LBL').click()
    cy.get('#RunReportButton').click()
    cy.wait(3000)
    cy.get('#MessageWindow_Ok_0').click()


})