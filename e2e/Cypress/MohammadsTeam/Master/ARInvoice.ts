
export class ARInvoice {
    constructor() {



    }
    CreateNewARInvoice(BillToName: string) {
        {

            
            cy.get('li[id=FACS]').click();
            cy.get('#NewInvoice').click();
            cy.get('#NewGeneralInvoice').click();
            cy.get('input[id=ARInvoice_BillToId]').type(BillToName).should("have.value", BillToName)

            cy.get('ul[id=mydatalist_ARInvoice_BillToId]').contains(BillToName).then(a => {
                a[0].click();
            })
            cy.get('input[id=ARInvoice_InvoiceCurrencyId]').type("NIS").should("have.value","NIS");
            cy.get('ul[id=mydatalist_ARInvoice_InvoiceCurrencyId]').contains("NIS").then(a => {
                a[0].click();
            }) 
            cy.get('button[id=ok-addArInvoice]').click();
            cy.get('button[id=Add]').click();
            cy.get('input[id=ARInvoiceLine_ChargesTypeId]').type("air");
            cy.get('ul[id=mydatalist_ARInvoiceLine_ChargesTypeId]').contains("AFT").then(a => {
                a[0].click();
            })           
            cy.get('input[id= ARInvoiceLine_ForiegnCurrencyId]').type("NIS").should("have.value","NIS");
            cy.get('ul[id=mydatalist_ARInvoiceLine_ForiegnCurrencyId]').contains("NIS").then(a => {
                a[0].click();
            })
            cy.get('input[id=ARInvoiceLine_Quantity]').type("10");
            cy.get('input[id=ARInvoiceLine_UnitPrice]').type("10");
            cy.get('#ok-addArInvoiceline').click();
            cy.get('#ARInvoiceBApprove').click();
            cy.contains('Unpaid') 
           
            
        }

    }
  
}
 



    