"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//import { Login } from './Login';
var CreateRandom_1 = require("./CreateRandom");
var Login_po_1 = require("../../login/Login.po");
var ChequeDepositSpec = /** @class */ (function () {
    function ChequeDepositSpec() {
        this.login = new Login_po_1.LoginComp();
    }
    return ChequeDepositSpec;
}());
exports.ChequeDepositSpec = ChequeDepositSpec;
describe('New Deposit ', function () {
    //let l: Login= new Login();
    var R = new CreateRandom_1.CreateRandom();
    it('New Cheque Deposit was Created Successfully', function () {
        var str = R.createrandomnum();
        cy.get('li[id=GeneralMHMaintenance]').click();
        cy.get('li[id=PAR]');
        cy.get('li[id=GeneralMHFullAccounting]').click();
        /* cy.get('li[id=FACS]').click();
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
         cy.get('div[id=EditBackbutton]').click();*/
        cy.get('li[id=FABNKS]').click();
        cy.get('#BANKSQUIERY').click();
        //  cy.get('input[id=SearchFieldsId_0_1]').type("BankNO822998ZA");
        cy.get('input[id=SearchFieldsId_0_0]').should('be.visible').then(function (a) {
            cy.get('input[id=SearchFieldsId_0_0]').type('usd bank', { force: true });
            cy.get('div[id=ListDataLoaded]').then(function (a) {
                cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
            });
        });
        var var2 = '';
        var openBalance;
        var closeBalance = 0;
        var close = 0;
        cy.get('li[id=BankAccountTHBankPages]').click();
        cy.get('input[id=date_ReconcileExternalPage_FromDate]').type('1/1');
        cy.get('input[id=SearchFieldsId_0_1]').click();
        cy.get('#LogGrid_0_1row0').click({ force: true });
        cy.get('input[id=ReconcileExternalPage_CloseBalance]')
            .then(function (elem) {
            cy.get('button[id=Close]').click();
            // elem is the underlying Javascript object targeted by the .get() command.
            openBalance = (Cypress.$(elem).val());
            cy.log(openBalance);
            closeBalance = parseFloat(openBalance.split(",").join("")) + 1000;
            cy.log(closeBalance + '');
            //  xyz2=Number(decimal);
            //close=Number(xyz)+ 1000;
            var2 = String(close);
            cy.get('#Add').click();
            cy.get('input[id=date_ReconcileExternalPage_FromDate_2]').type('28/10');
            cy.get('input[id=date_ReconcileExternalPage_ToDate_2]').type('28/10');
            cy.get('input[id=ReconcileExternalPage_StartBalance]').type(openBalance);
            // cy.wait(30000);
            cy.get('input[id=ReconcileExternalPage_CloseBalance]').type(closeBalance + '');
            // cy.wait(30000);
        });
        cy.get('#Add_1').click();
        //id = "edit-log-grid_0_00_1_0" 
        cy.get('div[id=edit-log-grid_0_00_5_0]').type('By Cypress');
        cy.get('#edit-log-grid_0_00_3_0').type('1000');
        cy.get('div[id=edit-log-grid_0_00_4_0]').type('By Cypress');
        cy.get('div[id=edit-log-grid_0_00_1_0]').type('28/10/2020');
        //  cy.get('div[id=edit-log-grid_0_10_3_0]').type('1000');
        cy.get('button[id=Approve]').click();
        cy.get('button[id=BankAccountBReconcile]').click();
    });
});
//# sourceMappingURL=ChequeDeposit.spec.js.map