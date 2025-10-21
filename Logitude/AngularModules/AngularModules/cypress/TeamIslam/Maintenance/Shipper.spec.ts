/// <reference types="cypress" />
  
  let timeStamp = (new Date()).getTime()

  //login

  
import { LoginComp } from '../../Login/Login.po';

export class CreateNewShipper  {

  private login: LoginComp = new LoginComp();
  
  constructor() {
  }
}




it('Create New Shipper', () => {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get("#GeneralMHMaintenance", { timeout: 15000 }).click()
    cy.get('#null_Search', { timeout: 15000 }).type('Shipper')
    cy.get('#MaintenanceItemMTCL', { timeout: 15000 }).click()
    cy.get('#NewButton_Customer', { timeout: 15000 }).click()

    cy.get('#Address_Name', { timeout: 15000 }).type("CypressShipper" + timeStamp )
    cy.get('#Address_CountryId', { timeout: 15000 }).type('Italy')
    cy.get(".DropDownListItem", { timeout: 15000 }).eq(0).should('contain', 'Italy').click();
    cy.get('#Address_City', { timeout: 15000 }).type('Florence')
    
    // Check for validation errors before proceeding
    cy.get('body').then(($body) => {
        if ($body.find('.error, .validation-error, [class*="error"]').length > 0) {
            cy.log('Validation errors detected, test should fail');
            cy.get('.error, .validation-error, [class*="error"]').should('not.exist');
        }
    });
    
    // Ensure no error messages are visible
    cy.get('body').should('not.contain', 'Country: name. The');
    cy.get('body').should('not.contain', 'error');
    
    cy.get('#Ok-AddCustomer', { timeout: 15000 }).click()
  })

it('Search For Shipper', () => {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get("#GeneralMHMaintenance", { timeout: 15000 }).click()
    cy.get('#null_Search', { timeout: 15000 }).type('Shipper')
    cy.get('#MaintenanceItemMTCL', { timeout: 15000 }).click()
    
    cy.get('#SearchFieldsId_0_0', { timeout: 15000 }).type("CypressShipper" + timeStamp)
    cy.get('#BusyIndicator_0', { timeout: 15000 }).should('not.be.visible')
    cy.get("#LogGrid_0_0row0", { timeout: 15000 }).click()
})

 it('Edit Shipper', () => {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get("#GeneralMHMaintenance", { timeout: 15000 }).click()
    cy.get('#null_Search', { timeout: 15000 }).type('Shipper')
    cy.get('#MaintenanceItemMTCL', { timeout: 15000 }).click()
    cy.get("#LogGrid_0_0row0", { timeout: 15000 }).click()
    
    cy.get('#CustomerTHGeneral', { timeout: 15000 }).click()
    cy.get('#Customer_LocalName', { timeout: 15000 }).type('Test Company 123')
    cy.get("#Customer-Save", { timeout: 15000 }).click()
  })



