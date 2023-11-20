export interface CreateNewExportInvoiceDetails {

    File: string,
    AccountTypeCode: string,   
    IssueDate: string, 
    ExporterNumber: string, 
    InvoiceCurrencyTypeCode: string, 
    IncotermCode: string, 
    BuyerName: string, 
    BuyerAddress: string, 
    PartyRelationshipCode: string, 
    BuyerCountryCode: string, 
    BuyerRoleCode: string, 
    InvoiceNumber: string, 
    InvoiceAmount: string, 
    CheckBox: string,

    PreferenceDocumentTypeCode: string, 
    DutyRegimeProtocolCode: string, 
    ExportModificationCurrency: string, 


    InsuranceCurrencyTypeCode: string, 
    InsuranceAmount: string, 
    TransportCurrencyTypeCode: string, 
    TransportAmount: string, 
    ExpenseCurrencyTypeCode : string, 
    ExpenseAmount: string, 
    
}