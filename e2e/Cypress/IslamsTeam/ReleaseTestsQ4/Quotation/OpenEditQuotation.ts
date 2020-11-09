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
    cy.get('#BusyIndicator_0').should('not.be.visible')

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
    cy.get('#OKAddCharges').click({force:true})
    //cy.get('#CheckBox_0_127_LBL').click({force:true})
    cy.get('#Quote-Save').click({force:true})
    cy.get('#BusyIndicator_0').should('not.be.visible')


})

it('Open Quotation Successfully', () => {
  cy.get('#AddCharges').should('be.visible')
  cy.get('#QuoteBQuotation').click({force:true})
  cy.get('#BusyIndicator_0').should('not.be.visible')

})

it('Edit Quotation', () => {
  //cy.wait(3000)
  cy.get('.RefreshButton').should('be.visible')
  cy.get('#EdiitTempalte').click({force:true})
})

it('Edit Quote Introduction', () => {

  cy.get('#EditId').should('be.visible')
  cy.get('#EditId').click({force:true})
  cy.get('#EditSection2').should('be.visible')
 cy.get('#EditSection2').click({force:true})
 cy.get('#AddDataField').click({force:true})
 cy.get('#SystemData').click({force:true})
 cy.get('#Expand3').click({force:true})
 //cy.get('.iwanttohover').eq(0).click();
 cy.get('#OkButton').click({force:true})
 cy.get('#Savee').click({force:true})
})

it('Pricing Packages Setting', () => {

 cy.get('#Setting_3').should('be.visible')
 cy.get('#Setting_3').click()
 cy.get('#ShowTitle').click({force:true})
 cy.get('#SaveButton').click({force:true})

})


it('Add New Section', () => {

  cy.get('#MainAdd').should('be.visible')
  cy.get('#MainAdd').click()
  cy.get('#AddSection').click({force:true})
  cy.get('#DisplayName').should('be.visible')
  cy.get('#DisplayName').type('Automated Test')
  cy.get('#Savee').click({force:true})
  cy.get('#Save').click({force:true})
 })

 it('Send Quotation To Customer', () => {

  cy.get('#SendOption').should('be.visible')
  cy.get('#SendOption').click()
  cy.get('#SendToCustomer').click({force:true})
  //cy.get('#EmailSearchTextBox_TextArea_0_0').should('be.visible')
  cy.get('#SendMessagebtn').click({force:true})
 // cy.get('#Savee').click({force:true})
 })

 