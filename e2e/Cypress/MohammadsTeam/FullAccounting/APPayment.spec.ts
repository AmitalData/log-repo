



//import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';

import { LoginComp } from "../../login/Login.po";
export class ARPAyemntSpec {

  private login: LoginComp = new LoginComp();
}
describe('New APPayment ', () => {

 
//let l: Login= new Login();
let R: CreateRandom= new CreateRandom();


  it('New APPayment Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
          
      cy.get('li[id=FAVND]').click();
      cy.get('button[id=NewAPPayment]').click();
      cy.get('input[id=APPayment_VendorId]').type("Test Vendor GLAccount1542").should("have.value", "Test Vendor GLAccount1542")
      cy.get('ul[id=mydatalist_APPayment_VendorId]').contains("Test Vendor GLAccount").then(a => {
          a[0].click();
      })
      cy.get('input[id=APPayment_AccountingPaymentMethodId]').type("cash")       
      cy.get('ul[id=mydatalist_APPayment_AccountingPaymentMethodId]').contains("Cash").then(a => {
          a[0].click();
      })
      cy.get('input[id=APPayment_AmountInPaymentCurrency]').type("1200")
  
      cy.get('input[id=APPayment_PaymentCurrencyId]').type("NIS")
      cy.get('ul[id=mydatalist_APPayment_PaymentCurrencyId]').contains("NIS").then(a => {
          a[0].click();
      })
      cy.get('button[id=APPaymentBApprove]').click();          
      cy.contains('Approved') 
      cy.log('APPayment Is Approved')
      

    });
      
 
});



