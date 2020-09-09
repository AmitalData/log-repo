/*import { browser, by, element } from "protractor"


import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { NewVendor } from "./NewVendorGLaccount";*/



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


      
      // edit teh customer 
        cy.get('#SearchFieldsId_0_0').type('Test Customer GLAccount' + code,{ force: true });
        cy.get('div[id=ListDataLoaded]').should('exist');
        cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
        cy.get('li[id=CustomerTHGeneral]').click();
        cy.get('#Customer_EnglishName');      
        cy.get('.TextTrimming').contains('Accounting').click(); 
        cy.get('#Activate').click();
        cy.get('#GLAccount_ChartOfAccountsId').type('cust');
        cy.get('.DropDownListItem').contains('Customer').click(); 
        cy.get('#GLAccount_CurrencyId').type('Nis');
        cy.get('.DropDownListItem').contains(' NIS ').click(); 
        cy.get('#Ok-AddGLAccount').click();
        cy.get('td').contains('Dispaly transactions').click();





    });
});
