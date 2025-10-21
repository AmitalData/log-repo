/// <reference types="cypress" />
  
import { LoginComp } from '../../Login/Login.po';

export class CreateNewUser {

  private login: LoginComp = new LoginComp();
  
  constructor() {
  }
}

it('Create New User', () => {
    cy.visit(Cypress.env("URL"), { timeout: 15000 })
    cy.get('body', { timeout: 15000 }).should('be.visible')
    cy.get('#Email', { timeout: 15000 }).type(Cypress.env("Email"), { delay: 50 })
    cy.get('#Password', { timeout: 15000 }).type(Cypress.env("Password"))
    cy.get('#cmdLogin', { timeout: 15000 }).click()
    cy.url({ timeout: 20000 }).should('not.include', '/login')
    
    cy.get("#GeneralMHMaintenance", { timeout: 15000 }).click()
    cy.get('#null_Search', { timeout: 15000 }).type('user')
    cy.get('#MaintenanceItemMTUS', { timeout: 15000 }).click()
    cy.get('#NewUserId', { timeout: 15000 }).click()
    let timeStamp = (new Date()).getTime()
    cy.get('#User_Email', { timeout: 15000 }).type("Cypress14" + timeStamp + "@mail.com")
    cy.get('#PasswordId', { timeout: 15000 }).type('123')
    cy.get('#retypePass', { timeout: 15000 }).type('123')
    cy.get('#User_EnglishName', { timeout: 15000 }).type("TestCypress" + timeStamp) 
    cy.get('#User_DepartmentId', { timeout: 15000 }).type("Management") 
    cy.get(".DropDownListItem", { timeout: 15000 }).eq(0).should('contain', 'Management').click();
    cy.get('#User_BranchId', { timeout: 15000 }).type("main")
    cy.get(".DropDownListItem", { timeout: 15000 }).eq(0).should('contain', 'Main Office').click();
    cy.get('#User_Notes', { timeout: 15000 }).type("This is Test")
    cy.get('#row11', { timeout: 15000 }).click()
    
    // Check for validation errors before proceeding
    cy.get('body').then(($body) => {
        if ($body.find('.error, .validation-error, [class*="error"]').length > 0) {
            cy.log('Validation errors detected, test should fail');
            cy.get('.error, .validation-error, [class*="error"]').should('not.exist');
        }
    });
    
    // Ensure no error messages are visible
    cy.get('body').should('not.contain', 'error');
    
    cy.get('#CheckBox_0_0_LBL', { timeout: 15000 }).click()
    cy.get('#OKIdButton', { timeout: 15000 }).click()

    })

it('Search For User', () => {
      cy.get('#User_Search').type('testcypress')
      cy.get(".ListBoxItem").eq(0).click()

    })
  
  
    it('Edit User', () => {
      cy.get('#User_Notes').type('Test cypress')
      cy.get("#User-Save").click({ force: true })

    })
  



