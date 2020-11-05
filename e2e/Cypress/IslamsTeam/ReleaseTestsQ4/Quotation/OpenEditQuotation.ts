/// <reference types="cypress" />

import { LoginComp } from "../../../login/Login.po";


export class CreateEditQuote {


  private login: LoginComp = new LoginComp();
  
  private isEditGrid: boolean = false;
  public IsEditGrid(value: boolean = true) {
      this.isEditGrid = value;
      return this;
  }
  
  constructor() {
  }
}


it('Create Quote Successfully', () => {
  cy.get('#GeneralMHQuotes').click({ force: true })
 // cy.get('.DefaultMenuItem:first').click();
  cy.get('#NewQuote').click({ force: true })
  cy.get('#DirectionRadio_0E').click({ force: true })
  cy.get('#TransportModeRadio_0A').click({force:true})
  cy.get('#Quote_ShipperId').type('Customer')
  cy.get(".DropDownListItem").eq(0).should('contain', 'Customer').click();
  cy.get('#Quote_ToPortId').type('IST')
  cy.get('.DropDownListItem:first').click({force:true})
  cy.get('#Quote_FromPortId').type('AAM')
  cy.get('.DropDownListItem:first').click({force:true})
  cy.get('#CreateQuote').click({force:true})
  cy.get('#BusyIndicator_0').should('not.be.visible')
  
})

it('Search For Specific Quote', () => {

    cy.get('#CreatedQuote').click()
    cy.get('#LogGrid_0_0row0').click({force:true})

})



it('Add Quote Charges ', () => {


  
    cy.get('#QuoteTHCharges').click({force:true})
    cy.get('#AddCharges').click()
    cy.get('#QuoteCharge_ChargesTypeId').type('Tax')
    cy.get('.DropDownListItem:first').click({force:true})
    cy.get('#QuoteCharge_CostMinAmount').type('40')
    cy.get('#QuoteCharge_CostMaxAmount').type('400')
    cy.get('#QuoteCharge_SaleMinAmount').type('80')
    cy.get('#QuoteCharge_SaleMaxAmount').type('800')
    cy.get('#CostPrice').type('850')
    cy.get('#OKAddCharges').click({force:true})

    cy.get('#AddCharges').click()
    cy.get('#QuoteCharge_ChargesTypeId').type('DELV')
    cy.get('.DropDownListItem:first').click({force:true})
    cy.get('#QuoteCharge_CostMinAmount').type('22')
    cy.get('#QuoteCharge_CostMaxAmount').type('200')
    cy.get('#QuoteCharge_SaleMinAmount').type('44')
    cy.get('#QuoteCharge_SaleMaxAmount').type('400')
    cy.get('#CostPrice').type('325')
    cy.get('#CheckBox_0_127_LBL').click({force:true})
    cy.get('#Quote-Save').click({force:true})
    cy.get('#BusyIndicator_0').should('not.be.visible')


})

it('Open Quotation Successfully', () => {

  cy.get('#QuoteBQuotation').click({force:true})
  cy.get('#BusyIndicator_0').should('not.be.visible')

})

it('Edit Quotation', () => {
  cy.wait(3000)
  cy.get('#EdiitTempalte').should('be.visible')
  cy.get('#EdiitTempalte').click({force:true})
  cy.get('#EditId').click({force:true})
 // cy.get('#BusyIndicator_0').should('not.be.visible')
 // cy.get('.BusyIndicatorControlInner').should('not.be.visible')
 cy.wait(3000)
 cy.get('#EditSection2').click()
  //cy.get('.fr-view').click()

})



