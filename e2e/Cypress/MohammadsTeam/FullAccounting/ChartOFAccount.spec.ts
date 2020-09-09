
import { RandomGenerator } from './RandomGenerator'



import { LoginComp } from "../../login/Login.po";

export class ChartSpec {

  private login: LoginComp = new LoginComp();
}

describe('ChartOfAccount Module', function () {


  let R: RandomGenerator = new RandomGenerator();


  it('Chart Of Account Success', function () {  
    var chartOfAccountNo = R.GenerateRandomNumber();



    //create new chart of account 
    cy.get('li[id=GeneralMHMaintenance]').click();
    cy.get('input[id=null_Search]').type('chart')       
    cy.get('#MaintenanceItemMTCA').click();
    cy.get('#NewButton_ChartOfAccount').click();
    cy.get('input[id=ChartOfAccount_EnglishName]').type('Customer' + chartOfAccountNo);
    cy.get('input[id=ChartOfAccount_LocalName]').type('Customer' + chartOfAccountNo);
    cy.get('input[id=ChartOfAccount_Code]').type(chartOfAccountNo);
    cy.get('input[id=ChartOfAccount_TypeCode').type('Customer');
    cy.get('.DropDownListItem').contains('Customers').click();
    cy.get('#ok-AddChartOfAccount').click({ force: true })


    // edit chartofaccount 
    cy.get('input[id=SearchFieldsId_0_0]').should('be.visible').then( a=> {
    cy.get('input[id=SearchFieldsId_0_0]').type(chartOfAccountNo,{ force: true });
    cy.get('div[id=ListDataLoaded]').then( a=> {
          cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
      })});
    
    cy.get('input[id=ChartOfAccount_EnglishName]').type('English Name Modified');
    cy.get('input[id=ChartOfAccount_LocalName]').type('Local Name Modified');
    cy.get('#ChartOfAccount-SaveClose').click();





  });

});