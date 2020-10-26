



//import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';

import { LoginComp } from "../../login/Login.po";
export class ChequeDepositSpec {

  private login: LoginComp = new LoginComp();
}
describe('New Deposit ', () => {

 
//let l: Login= new Login();
let R: CreateRandom= new CreateRandom();



  it('New Cheque Deposit was Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
    
      /*cy.get('li[id=FACS]').click();
      cy.get('button[id=NewARPayment]').click();
      cy.get('input[id=ARPayment_BillToId]').type("Test Customer GLAccountAB36000WD").should("have.value", "Test Customer GLAccountAB36000WD")
      cy.get('ul[id=mydatalist_ARPayment_BillToId]').contains("Test Customer GLAccountAB36000WD").then(a => {
          a[0].click();
      })
      cy.get('input[id=ARPayment_AmountInPaymentCurrency]').type("1000").should("have.value","1000")
      cy.get('input[id=ARPayment_AccountingPaymentMethodId]').type("Cheque").should("have.value","Cheque")
      cy.get('ul[id=mydatalist_ARPayment_AccountingPaymentMethodId]').contains("Cheque").then(a => {
          a[0].click();
      })
      cy.get('input[id=ARPayment_BranchId]').type("Main Office").should("have.value","Main Office")
      cy.get('ul[id=mydatalist_ARPayment_BranchId]').contains("Main Office").then(a => {
          a[0].click();
      })
      cy.get('button[id=ok-AddARPayment]').click();
      cy.get('#ARPaymentSpinner').should("not.be.visible");

      cy.get('input[id=date_ARPayment_ValueDate]').type("1/9");
      cy.get('input[id=ARPayment_Account]').type("1655");
      cy.get('input[id=ARPayment_ChequeOrPaymentRef]').type("12352");
      
      cy.get('input[id=ARPayment_BankBranch]').type("7845");
      cy.get('input[id=ARPayment_Bank]').type("69582");
      
      cy.get('button[id=ARPaymentBApprove]').click();
      cy.contains('Approved') 
      cy.log('ARPayment Is Approved')
      cy.get('#Back').click();*/
      


      // create the cheque deposit  
    cy.get('li[id=FABNKS]').click();
    cy.get('li[id=NEWDEPOSIT]').click();
    cy.get('input[id=date_BankDeposit_AccountingDate]').type("1/9/2020");
    cy.get('input[id=BankDeposit_CashBookId]').type("cheque");
    cy.get('ul[id=LogLovDropDown-BankDeposit_CashBookId]').contains("cheque").then(a => {
        a[0].click();
    })
    cy.get('input[id=BankDeposit_DepositBankAccountId]').type("Bank");
    cy.get('ul[id=mydatalist_BankDeposit_DepositBankAccountId]').contains("Bank").then(a => {
        a[0].click();
    })
    cy.get('input[id=CheckBox_0_1_LBL]').click();
    cy.get('input[id=BankDepositBApprove]').click();
    cy.contains('Today');
    

    

    
    
    

    });
      
 
});



