
export class AllInvoices {

    Statuses: StatusData[];
    InvoiceLines: InvoiceLine[];
    IntegratedInvoices: IntegratedInvoice[];
    Invoices: Invoice[];
    Messages: MessagesData[];
    GeneralDetails: GeneralDetails;
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
    Wip: string;
    LineNumber: string;
    ExcludedLine: boolean;
}

export class IntegratedInvoice {
    InvoiceNumber: string;
    ForwarderFile: string;
    InvoiceCurrency: string;
    InvoiceAmount: any;

}
export class Invoice {
    InvoiceBillTo: string;
    InvoiceBillToCard: string;
    InvoiceType: string;
    InvoiceDate: string;
    InvoiceCurrency: string;
    InvoiceTypeCode: string;
    InvoiceAmount: any;
}
export class MessagesData {
    W: string;
    E: string;
}

export class GeneralDetails {
    Forwarder: string;
    TypeOfDelivery: string;
}
