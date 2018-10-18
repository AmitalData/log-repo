import { ResponseDataBase } from './ResponseDataBase';

export class DeficitFilesDetailResponseData extends ResponseDataBase {
    public FileStatus: string;
    public StatusName: string;
    public ExternalID: string;
    public ExternalName: string;
    public CustomOfficeNumber: string;
    public CustomOfficeName: string;
    public FilingNumber: string;
    public OpenFileCounter: string;
    public CloseFileCounter: string;
    public AgentExternalID: string;
    public AgentName: string;


    public OpenFilesList: ExternalFilesDetailsResult[];
    public CloseFileList: ExternalFilesDetailsResult[];
    public PaymentOrderList: ExternalPaymentOrderResult[];
    public RequireDocumentsList: RequireDocumentsResult[];

}
export class RequireDocumentsResult {
    public DocumentCode: string;
    public DocumentTypeName: string;
    public FileNumber: string;
    public Numeral: string;
    public DisplayFileNumber: string;
    public DocumentID: string;
    
}

export class ExternalPaymentOrderResult {
    public ExternalID: string;
    public ExternalName: string;


    public PaymentID: string;
    public PaymentProcessType: string;
    public PaymentProcessName: string;
    public AmountSum: number;
    public ValidityDateTo: string;
    public CreateDate: string;
    public PaymentOrderPayDate: string;
    public PaymentOrderStatus: string;
    public PaymentOrderStatusName: string;
}
export class ExternalFilesDetailsResult {
    public ExternalID: string;
    public ExternalName: string;
    public FileNumber: string;


    public Numeral: string;
    public DisplayFileNumber: string;
    public DeficitEntityType: string;
    public EntityTypeName: string;
    public DeficitEntityID: string;
    public ProductionDate: string;

    public UnpaidBalance: string;
    public EstimatedBalance: string;
    public EstimatedDate: string;
    public Status: string;
    public StatusName: string;
    public TotalComponentAmount: string;
    public TotalRefundAmount: string;
    public CloseDate: string;
    public SecondaryStatus: string;

}