/// <reference types="cypress" />
  
  let timeStamp = (new Date()).getTime()

  //login

  
  import { LoginComp } from '../../../Login/Login.po';

export class CreateNewShipper  {

  private login: LoginComp = new LoginComp();
  
  constructor() {
  }
}




it('Create New Shipper', () => {
   // cy.wait(10000)
   // cy.wait(100)
    cy.get("#GeneralMHMaintenance").click()
    cy.get('#null_Search').type('Shipper')
    cy.get('#MaintenanceItemMTCL').should('be.visible')
    cy.get('#MaintenanceItemMTCL').click({ force: true })
    cy.get('#NewButton_Customer').click({ force: true })

    cy.get('#Address_Name').type("CypressShipper" + timeStamp )
    cy.get('#Address_CountryId').type('Italy')
    cy.get(".DropDownListItem").eq(0).should('contain', 'Italy').click();
    cy.get('#Address_City').type('Florence')
    cy.get('#Ok-AddCustomer').click()

  })

it('Search For Shipper', () => {

   // cy.get('#BusyIndicator_0').should('not.be.visible')
    cy.wait(100)
   cy.get('#SearchFieldsId_0_0').should('be.visible')
   cy.get('#SearchFieldsId_0_0').type("CypressShipper" + timeStamp)
   cy.get('#BusyIndicator_0').should('not.be.visible')
   cy.get("#LogGrid_0_0row0").click({ force: true })

})

 it('Edit Shipper', () => {
     cy.wait(100)
     cy.get('#CustomerTHGeneral').click({ force: true })
   //  cy.wait(1000)
     cy.get('#Customer_LocalName').type('Test Company 123')
     cy.wait(1000)
     cy.get("#Customer-Save").click()

  })



