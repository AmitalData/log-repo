
/// <reference types="cypress"/>
export class Glaccount {


  constructor() {


  }

  CreateNewGLAccount(LocalName) {




    cy.get('Button[id=NewGLAccount]').click();
    cy.get('#GLAccount_ChartOfAccountsTypeCode').type('Revenues');
    cy.get('.DropDownListItem').contains('Revenues').click();
    cy.get('#GLAccount_ChartOfAccountsId').type('Rev');
    cy.get('.DropDownListItem').contains('Rev').click();
    cy.get('#GLAccount_LocalName').type(LocalName);
    cy.get('#Ok-AddGLAccount').click();
  //  cy.get('#BusyIndicator_0').should('not.be.visible');
  //  cy.get(".LogitudeWindow").should('not.be.visible');

  }

  EditGLAccount(DisplayNumber) { 
 

    cy.get('#CardGLAccount_Search').type(DisplayNumber);
    cy.get('.ListBoxItem').contains(DisplayNumber).click();
    cy.get('#GLAccountTHGeneral').click();
    cy.get('#GLAccount_LocalName').type('Updated Local Name');
    cy.get('#GLAccount_EnglishName').type('Updated English Name');
    cy.get('#GLAccount-Save').click();
   

  }
}
