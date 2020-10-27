

/*import { browser, by, element } from "protractor"


import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { NewVendor } from "./NewVendorGLaccount";*/

//

import { RandomGenerator } from './RandomGenerator'

import { LoginComp } from "../../login/Login.po";

export class CustomerSpec {

  private login: LoginComp = new LoginComp();
}

describe('CustomerGlAccount Module', function () {

    let R: RandomGenerator = new RandomGenerator();
 

    it(' New Customer GLAccount Was Created', function () {

  
       // cy.get('li[id=GeneralMHMaintenance]', { timeout: 60000 })
        // this was the only way that worked well :/
        var code = R.GenerateRandomNumberACC();


// create new customer 
        cy.get('li[id="GeneralMHCustomers"]').click();
        cy.get('Button[id="NewButton_Customer"]').click();

        cy.get('#Address_Address1').type('Ramallah');
        cy.get('#Address_Address2').type('Nablus');
        cy.get('#Address_ZipCode').type('00970');
        cy.get('#Address_CountryId').type('ps');
        cy.get('#Address_LocalName').type('Test Customer GLAccount' + code);
        cy.get('#textboxdiv_Address_Name').type('Test Customer GLAccount' + code);
        cy.get('#Address_City').type('Nablus');
        cy.get('#Address_CountryId').type('ps');
        cy.get('.DropDownListItem').contains(' State Of Palestine ').click();
        cy.get('#Ok-AddCustomer').click();


      
  
        cy.get('#SearchFieldsId_0_0').type('Test Customer GLAccount' + code,{ force: true });
        cy.get('div[id=ListDataLoaded]').should('exist');
        cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
        cy.get('li[id=CustomerTHGeneral]').click();
        cy.get('#Customer_EnglishName');      
        cy.get('.TextTrimming').contains('Accounting').click(); 
        cy.get('#Activate').click();
		
        cy.get('#GLAccount_ChartOfAccountsId').type('cust');
        cy.get('.DropDownListItem').contains('Customer').click(); 
		cy.get('input[type="checkbox"]').check({ force: true })
       
        cy.get('#Ok-AddGLAccount').click();
       cy.get('#EditBackbutton').click({ force: true });
	   
	   
	   
	   
	   // move to full accounting tab 
	  cy.get('li[id="GeneralMHFullAccounting"]').click();
	  cy.get('#NewInvoice').click();
      cy.get('#NewGeneralInvoice').click();
      cy.get('input[id=ARInvoice_BillToId]').type("Test Customer GLAccountAB36000WD").should("have.value", "Test Customer GLAccountAB36000WD")

      cy.get('ul[id=mydatalist_ARInvoice_BillToId]').contains("Test Customer GLAccountAB36000WD").then(a => {
          a[0].click();
      })
   
      cy.get('input[id=ARInvoice_VatNumber]').type("123");

      cy.get('button[id=ok-addArInvoice]').click();
      cy.get('button[id=Add]').click();
      cy.get('input[id=ARInvoiceLine_ChargesTypeId]').type("air");
      cy.get('ul[id=mydatalist_ARInvoiceLine_ChargesTypeId]').contains("AFT").then(a => {
          a[0].click();
      })      
      cy.get('input[id=ARInvoiceLine_VatTypeId]').clear();
      cy.get('input[id=ARInvoiceLine_VatTypeId]').type('Zero');

      cy.get('ul[id=mydatalist_ARInvoiceLine_VatTypeId]').contains('Zero').then(a => {
          a[0].click();
      })      
      cy.get('input[id= ARInvoiceLine_ForiegnCurrencyId]').type("NIS");
      cy.get('ul[id=mydatalist_ARInvoiceLine_ForiegnCurrencyId]').contains("NIS").then(a => {
          a[0].click();
      })
      cy.get('input[id=ARInvoiceLine_Quantity]').type("10");
      cy.get('input[id=ARInvoiceLine_UnitPrice]').type("10");
      cy.get('#ok-addArInvoiceline').click();
      cy.get('#ARInvoiceBApprove').click();
      cy.contains('Unpaid') 
      



    });
});

