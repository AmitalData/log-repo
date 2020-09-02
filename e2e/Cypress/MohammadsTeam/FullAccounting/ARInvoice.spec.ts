
//import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';

import { LoginComp } from "../../login/Login.po";
export class ARPAyemntSpec {

  private login: LoginComp = new LoginComp();
}
describe('New ARPayment ', () => {

 
//let l: Login= new Login();
let R: CreateRandom= new CreateRandom();



  it('New ARInvoice Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
 
      cy.get('li[id=FACS]').click();
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



