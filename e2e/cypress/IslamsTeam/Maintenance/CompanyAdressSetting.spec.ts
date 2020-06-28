/// <reference types="cypress" />
  
import { LoginComp } from '../../Login/Login.po';

export class CompanyAddressSetting  {

  private login: LoginComp = new LoginComp();
  
  constructor() {
  }
}



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


