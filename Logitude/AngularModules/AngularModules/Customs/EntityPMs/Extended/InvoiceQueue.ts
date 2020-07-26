 
export class Invoices {
 
    Statuses: StatusData[];
    InvoiceLines: InvoiceLine[];
    IntegratedInvoices: IntegratedInvoice[];
    InvoiceList: InvoiceList[];
    static InvoiceList: any;

}
 
export class StatusData {
    Code: string;
    Name: string;
    Date: string;
    Time: string;
    Comments: string;

}


export class InvoiceLine {
    ServiceCode: string;
    ServiceName: string;
    PayType: string;
    AmountNIS: any;
    AmountForeign: any;
    Currency: string;
}

export class IntegratedInvoice {
    InvoiceNumber: string;
    ForwarderFile: string;
    InvoiceCurrency: string;
    InvoiceAmount: any;
 
}
export class InvoiceList {
    InvoiceBillTo: string;
    InvoiceType: string;
    InvoiceDate: string;
    InvoiceCurrency: string;
}
