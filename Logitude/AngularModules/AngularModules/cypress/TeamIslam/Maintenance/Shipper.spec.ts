/// <reference types="cypress" />
  
  let timeStamp = (new Date()).getTime()

  //login
  it('Sucessfully Login ', () => {
    //  cy.visit('https://test.logitudeworld.com/test/')
      cy.visit('http://localhost:4200')
      cy.get('#Email').click()
      cy.get('#Email').clear()
      //cy.get('#Email').type('Raghad@protractor.com')
      cy.get('#Email').type('angular@fnarsoft.com')
      cy.get('#Password').click()
      cy.get('#Password').clear()
      //cy.get('#Password').type('!RS123Rs')
      cy.get('#Password').type('1')
      cy.get('#cmdLogin').click()
  
    })




it('Create New Shipper', () => {
   // cy.wait(10000)
   // cy.wait(100)
    cy.get("#GeneralMHMaintenance").click()
    cy.get('#null_Search').type('Shipper')
    cy.get('#MaintenanceItemMTCL').click()
    cy.get('#NewButton_Customer').click()

    cy.get('#Address_Name').type("CypressShipper" + timeStamp )
    cy.get('#Address_CountryId').type('Italy')
    cy.get(".DropDownListItem").eq(0).should('contain', 'Italy').click();
    cy.get('#Address_City').type('Florence')
    cy.get('#Ok-AddCustomer').click()

  })

it('Search For Shipper', () => {
   cy.get('#SearchFieldsId_0_0').type("CypressShipper" + timeStamp)
   cy.get("#LogGrid_0_0row0").click()

})

 it('Edit Shipper', () => {
   //  cy.wait(100)
     cy.get('#CustomerTHGeneral').click()
   //  cy.wait(1000)
     cy.get('#Customer_LocalName').type('Test Company 123')
     cy.get("#Customer-Save").click()

  })



