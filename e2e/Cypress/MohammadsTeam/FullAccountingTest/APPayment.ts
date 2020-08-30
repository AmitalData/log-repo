
export class APPayment {
    constructor() {



    }
    CreateNewAPPayment(BillToName: string) {
        {

            
            cy.get('li[id=FAVND]').click();
            cy.get('button[id=NewAPPayment]').click();

           /* cy.get('#searchicon_APPayment_VendorId').click();
            
            cy.get('input[id=SearchFieldsId_0_1]').type(BillToName).should("have.value", BillToName)
            cy.get('div[id=row0col0').click({ force: true });

       
            
            

         /*   cy.get('input[id=APPayment_VendorId]').type(BillToName).should("have.value", BillToName);


           cy.get('.DropDownListItem:first').should('be.visible')
            cy.get('.DropDownListItem:first').click()*/
            

           // cy.get('ul[id=mydatalist_APPayment_VendorId]').contains(BillToName).then(a => {
              //  a[0].click();
        //    });
        
            cy.get('input[id=APPayment_AccountingPaymentMethodId]').type("Cash")       
            cy.get('ul[id=mydatalist_APPayment_AccountingPaymentMethodId]').contains("Cash").then(a => {
                a[0].click();
            })
            cy.get('input[id=APPayment_AmountInPaymentCurrency]').type("1200")
        
         
            cy.get('input[id=APPayment_VendorId]').type(BillToName).should("have.value", BillToName);


           cy.get('.DropDownListItem:first').should('be.visible')
            cy.get('.DropDownListItem:first').click()

            cy.get('input[id=APPayment_PaymentCurrencyId]').type("NIS").then (a => {
                cy.get('ul[id=mydatalist_APPayment_PaymentCurrencyId]').contains("NIS").then(a => {
                    a[0].click();
                })});
            cy.get('button[id=APPaymentBApprove]').click();          
            cy.contains('Approved') 
            cy.log('APPayment Is Approved')
            
        }

    }
  
}
 



    