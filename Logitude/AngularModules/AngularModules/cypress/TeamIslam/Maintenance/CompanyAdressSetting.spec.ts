/// <reference types="cypress" />
  

  //login
  it('Sucessfully Login ', () => {
      cy.visit('https://test.logitudeworld.com/test/')
     // cy.visit('http://localhost:4200')
      cy.get('#Email').click()
      cy.get('#Email').clear()
      cy.get('#Email').type('Raghad@protractor.com')
     // cy.get('#Email').type('angular@fnarsoft.com')
      cy.get('#Password').click()
      cy.get('#Password').clear()
      cy.get('#Password').type('!RS123Rs')
     // cy.get('#Password').type('1')
      cy.get('#cmdLogin').click()
  
    })


it('Srearch Company Address Setting ', () => {
    //cy.wait(10000)
    cy.get("#GeneralMHMaintenance").click()
    cy.get('#null_Search').type('company address setting')
    cy.get('#MaintenanceItemCOAD').click()

  })

it('Edit Copmnay Address Setting', () => {
    cy.get('#Address_Address2').type('Ramallah')
    cy.get('#Address_ZipCode').type('99988')
   // cy.wait(100)
    cy.get('#OkButton').click()



})

