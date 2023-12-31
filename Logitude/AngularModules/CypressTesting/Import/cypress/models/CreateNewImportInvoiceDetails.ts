export interface CreateNewImportInvoiceDetails {

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

    VendorId: string,

    PreferenceDocumentTypeCode: string, 
    DutyRegimeProtocolCode: string, 
    ExportModificationCurrency: string, 

    FreightAmountCurrencyTypeCode: string, 
    FreightAmount: string, 


    EditButtonInvoiceItemNo: string,
    AddItemButtonItemNo: string,
    ItemNo: string,   
    ItemDescription: string, 
    Item: string, 
    TradeAgreementCode: string, 
    ProtocolCode: string, 
    UnitsQuantity: string, 
    UnitType: string, 
    ValueInForeignCurrency: string, 
    OriginCountryCode: string, 
    Edit: string, 
    ProcessTypeCode: string, 
    DeleteButton: string,

}