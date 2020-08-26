
export class APInvoice {
    constructor() {



    }
    CreateNewAPInvoice(Vendor: string, InvoiceNumber: string) {
        {


            cy.get('li[id=FACS]').click();
            cy.get('li[id=FAVND]').click();
            cy.get('button[id=NewAPInvoice]').click();
            cy.get('input[id=APInvoice_VendorId]').type(Vendor)
            cy.get('ul[id=mydatalist_APInvoice_VendorId]').contains(Vendor).then(a => {
                a[0].click();
            })
            cy.get('input[id=APInvoice_InvoiceNumber]').type(InvoiceNumber,{ force: true });
            cy.get('input[id=APInvoice_AmountInInvoiceCurrency]').type('10000').should("have.value", '10000')
            cy.get('input[id=APInvoice_InvoiceCurrencyId]').type('NIS').should("have.value", 'NIS')

            cy.get('ul[id=mydatalist_APInvoice_InvoiceCurrencyId]').contains('NIS').then(a => {
                a[0].click();
            })
            cy.get('input[id=date_APInvoice_InvoiceDate]').type("1/7/2020")
            cy.get('input[id=APInvoice_VATNumber]').type('123456789').should("have.value", '123456789')

            cy.get('button[id=Ok-AddAPInvoice]').click();
            cy.get('button[id=AddInvoiceLine]').click();
            cy.get('input[id=APInvoiceLine_ChargesTypeId]').type('Air Freight').should("have.value", 'Air Freight')

            cy.get('ul[id=mydatalist_APInvoiceLine_ChargesTypeId]').contains('Air Freight').then(a => {
                a[0].click();
            })
            cy.get('input[id=APInvoiceLine_VatTypeId]').clear();
            cy.get('input[id=APInvoiceLine_VatTypeId]').type('Zero');

            cy.get('ul[id=mydatalist_APInvoiceLine_VatTypeId]').contains('Zero').then(a => {
                a[0].click();
            })
            cy.get('input[id=APInvoiceLine_VatPercentage]').type('0')
            cy.get('input[id=APInvoiceLine_InvoiceCurrencyAmount]').type('10000')


            cy.get('button[id=Ok-AddAPInvoiceLine]').click();
            cy.get('button[id=APInvoiceBApprove]').click();
            cy.contains('Approved')
            cy.log('ARPayment Is Approved')

        }

    }

} 