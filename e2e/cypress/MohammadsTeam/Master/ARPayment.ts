
export class ARPayment {
    constructor() {



    }
    CreateNewARPayment(BillToName: string) {
        {

            
            cy.get('li[id=FACS]').click();
            cy.get('button[id=NewARPayment]').click();
            cy.get('input[id=ARPayment_BillToId]').type(BillToName).should("have.value", BillToName)
            cy.get('ul[id=mydatalist_ARPayment_BillToId]').contains(BillToName).then(a => {
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
            
        }

    }
  
}
 



    