export interface APInvoiceDetails {
    Vendor: string,
    InvoiceNumber: string,
    InvoiceAmount: number,
    InvoiceCurrency: string,
    InvoiceExchangeRate:number,
    InvoiceDate: string,
    PaymentTerms:string,
    DueDate: string,
    VATType:string,
    VatNo:string,
    Branch:string,
}