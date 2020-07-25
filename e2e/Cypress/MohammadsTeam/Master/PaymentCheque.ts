
export class PaymentCheque {
    constructor() {



    }
    CreateNewPaymentCheque(BillToName: string) {
        {

            
          
            cy.get('button[id=NEWPaymentCheque]').click();
            cy.get('input[id=PaymentCheque_PayToGLAccountId]').type(BillToName).should("have.value", BillToName)
            cy.get('ul[id=mydatalist_PaymentCheque_PayToGLAccountId]').contains(BillToName).then(a => {
                a[0].click();
            })
            cy.get('input[id=PaymentCheque_BankAccountId]').type('Bank');
            cy.get('ul[id=mydatalist_PaymentCheque_BankAccountId]').contains('Bank').then(a => {
                a[0].click();
            })
            cy.get('input[id=PaymentCheque_LocalAmount]').type('1200');
            cy.contains('Ok').click();           
           cy.contains('Draft');
            
          
            
    
            
        }

    }
  
}
 



    