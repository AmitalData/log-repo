



import  { CreateRandom } from './CreateRandom';

import { LoginComp } from "../../login/Login.po";
export class APInvoiceSpec {

  private login: LoginComp = new LoginComp();
}
describe('New APInvoice ', () => {

 

let R: CreateRandom= new CreateRandom();



  it('New APInvoice Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();

    

            cy.get('li[id=FACS]').click();
            cy.get('li[id=FAVND]').click();
            cy.get('button[id=NewAPInvoice]').click();

            cy.get('input[id=APInvoice_InvoiceNumber]').type(str,{ force: true });
            cy.get('input[id=APInvoice_AmountInInvoiceCurrency]').type('10000').should("have.value", '10000')
            cy.get('input[id=APInvoice_InvoiceCurrencyId]').type('NIS').should("have.value", 'NIS')

            cy.get('ul[id=mydatalist_APInvoice_InvoiceCurrencyId]').contains('NIS').then(a => {
                a[0].click();
            })
            cy.get('input[id=date_APInvoice_InvoiceDate]').type("1/12/2020")

            cy.get('input[id=APInvoice_VATNumber]').type('123456789').should("have.value", '123456789')



            cy.get('#APInvoice_VendorId').click({force:true}).type('{downarrow}').type("Test Vendor GLAccount");
            cy.get('ul[id= mydatalist_APInvoice_VendorId]').contains("Test Vendor GLAccount").then(a => {
               a[0].click();
            })
           
          
            

        

            cy.get('button[id=Ok-AddAPInvoice]').click();
            cy.get('button[id=AddInvoiceLine]').click();
            cy.get('input[id=APInvoiceLine_ChargesTypeId]').type('Air Freight').should("have.value", 'Air Freight')

            cy.get('ul[id=mydatalist_APInvoiceLine_ChargesTypeId]').contains('Air Freight').then(a => {
                a[0].click();
            })
           
            cy.get('input[id=APInvoiceLine_VatPercentage]').type('0')
            cy.get('input[id=APInvoiceLine_InvoiceCurrencyAmount]').type('10000')
           

            cy.get('button[id=Ok-AddAPInvoiceLine]').click();
            cy.get('button[id=APInvoiceBApprove]').click();
            cy.contains('Approved')
            cy.log('APinvoice Is Approved')

        
      
    });
      
 
});



