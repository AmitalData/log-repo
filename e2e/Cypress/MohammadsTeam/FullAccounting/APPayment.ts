
export class APPayment {
    constructor() {



    }
    CreateNewAPPayment(BillToName: string) {
        {

            
            cy.get('li[id=FAVND]').click();
            cy.get('button[id=NewAPPayment]').click();
            cy.get('input[id=APPayment_VendorId]').type(BillToName).should("have.value", BillToName)
            cy.get('ul[id=mydatalist_APPayment_VendorId]').contains(BillToName).then(a => {
                a[0].click();
            })
            cy.get('input[id=APPayment_AccountingPaymentMethodId]').type("Cash")       
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
            
        }

    }
  
}
 



    