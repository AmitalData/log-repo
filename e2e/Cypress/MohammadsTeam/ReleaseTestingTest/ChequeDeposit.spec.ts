



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
    
      cy.get('li[id=FACS]').click();
      cy.get('button[id=NewARPayment]').click();
      cy.get('input[id=ARPayment_BillToId]').type("ARpayment Reco");
      cy.get('ul[id=mydatalist_ARPayment_BillToId]').contains("ARpayment Reco").then(a => {
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

      
     
      
  
      
      cy.get('input[id=date_ARPayment_ValueDate]').type("1/9");
      cy.get('input[id=ARPayment_Account]').type("1655");
      cy.get('input[id=ARPayment_ChequeOrPaymentRef]').type("12352");
      
      cy.get('input[id=ARPayment_BankBranch]').type("7845");
      cy.get('input[id=ARPayment_Bank]').type("69582");
      
      cy.get('button[id=ARPaymentBApprove]').click();
      cy.contains('Approved') 
      cy.log('ARPayment Is Approved')
     // cy.get('#Refresh').click(); 
      cy.get('div[id=EditBackbutton]').click();
      

      // create the cheque deposit  
    cy.get('li[id=FABNKS]').click();
    cy.get('button[id=NEWDEPOSIT]').click();
    cy.get('input[id=date_BankDeposit_AccountingDate]').type("1/9/2020");
    cy.get('input[id=BankDeposit_CashBookId]').type("cheque");
    cy.get('ul[id=mydatalist_BankDeposit_CashBookId]').contains("cheque").then(a => {
        a[0].click();
    })
      cy.get('input[id=BankDeposit_DepositBankAccountId]').type("BankNO822998ZA");
    cy.get('ul[id=mydatalist_BankDeposit_DepositBankAccountId]').contains("Bank").then(a => {
        a[0].click();
    })
    cy.get('button[id=CREATEDEPOSIT]').click();
    
    
    cy.get('label[id=CheckBox_0_0_LBL]').click();
   
    // cy.get('input[id=CheckBox_0_1_LBL]').click();
    cy.get('button[id=BankDepositBApprove]').click();
 
      cy.contains('Today');
      cy.get('div[id=EditBackbutton]').click();

      cy.get('#BANKSQUIERY').click();
    //  cy.get('input[id=SearchFieldsId_0_1]').type("BankNO822998ZA");
      
      cy.get('input[id=SearchFieldsId_0_1]').should('be.visible').then(a => {
          cy.get('input[id=SearchFieldsId_0_1]').type("BankNO822998ZA", { force: true });
          cy.get('div[id=ListDataLoaded]').then(a => {
              cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
          })
      });
      cy.get('li[id=BankAccountTHBankPages]').click();
      cy.get('input[id=date_ReconcileExternalPage_FromDate]').type('1/1')  
      cy.get('#row0col0').click();
      const closeBalance = cy.get('input[id=textboxdiv_ReconcileExternalPage_CloseBalance]')
      cy.log(closeBalance+'');
      //cy.get('#Add').click();
      
      
    

    

    
    
    

    });
      
 
});



