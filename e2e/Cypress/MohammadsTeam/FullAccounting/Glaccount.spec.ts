

import { RandomGenerator } from './RandomGenerator'

import { LoginComp } from "../../login/Login.po";

export class GLASpec {

  private login: LoginComp = new LoginComp();
}
describe('GLAccount Module', function () {
 

    let R: RandomGenerator = new RandomGenerator();




    

  it(' New GLAccount Was Created And Updated', function () {
   

   // cy.get('li[id=PAR]',{timeout: 60000})
  
   cy.get('li[id="GeneralMHFullAccounting"]').click();
    cy.get('#FAGLAccouts').click();
   

  
    var GlaccountNumber = R.RandomNum();
    var name = 'My Auto GLAccount';

  
//Create GLaccount 
    cy.get('Button[id=NewGLAccount]').click({ force:true });
    cy.get('#GLAccount_ChartOfAccountsTypeCode').type('Revenues');
    cy.get('.DropDownListItem').contains('Revenues').click();
    cy.get('#GLAccount_ChartOfAccountsId').type('Rev');
    cy.get('.DropDownListItem').contains('Rev').click();
    cy.get('#GLAccount_LocalName').type(name+GlaccountNumber);
    cy.get('#Ok-AddGLAccount').click();




  //Edit glaccount  
    cy.get('#CardGLAccount_Search').type(name +GlaccountNumber);
    cy.get('.ListBoxItem').contains(name +GlaccountNumber).click();
    cy.get('#GLAccountTHGeneral').click();
    cy.get('#GLAccount_LocalName').type('Updated Local Name');
    cy.get('#GLAccount_EnglishName').type('Updated English Name');
    cy.get('#GLAccount-Save').click();



  });
});
