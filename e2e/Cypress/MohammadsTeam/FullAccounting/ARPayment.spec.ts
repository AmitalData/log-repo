



//import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';

import { LoginComp } from "../../login/Login.po";
export class ARPAyemntSpec {

  private login: LoginComp = new LoginComp();
}
describe('New ARPayment ', () => {

 
//let l: Login= new Login();
let R: CreateRandom= new CreateRandom();



  it('New ARPayment Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
    
      cy.get('li[id=FACS]').click();
      cy.get('button[id=NewARPayment]').click();
      cy.get('input[id=ARPayment_BillToId]').type("Test Customer GLAccountXU805920XV").should("have.value", "Test Customer GLAccountXU805920XV")
      cy.get('ul[id=mydatalist_ARPayment_BillToId]').contains("Test Customer GLAccountXU805920XV").then(a => {
          a[0].click();
      })
      cy.get('input[id=ARPayment_AmountInPaymentCurrency]').type("1000").should("have.value","1000")
      cy.get('input[id=ARPayment_AccountingPaymentMethodId]').type("Cash").should("have.value","Cash")
      cy.get('ul[id=mydatalist_ARPayment_AccountingPaymentMethodId]').contains("Cash").then(a => {
          a[0].click();
      })
      cy.get('input[id=ARPayment_BranchId]').type("Main Office").should("have.value","Main Office")
      cy.get('ul[id=mydatalist_ARPayment_BranchId]').contains("Main Office").then(a => {
          a[0].click();
      })
      cy.get('button[id=ok-AddARPayment]').click();
      cy.get('#ARPaymentSpinner').should("not.be.visible");
      cy.get('button[id=ARPaymentBApprove]').click();
      cy.contains('Approved') 
      cy.log('ARPayment Is Approved')
      
    });
      
 
});



