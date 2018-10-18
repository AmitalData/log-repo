import {ResponseDataBase} from './ResponseDataBase';

export class TPG_NG_8245_ClaimFilesDetailResponseData extends ResponseDataBase {

    
    public GeneralDetailsData: GeneralDetails;
    public OpenFilesCollapsList: Array<FilesDetails>;
    public CloseFilesCollapsList : Array<FilesDetails>;
    public RefundOrderList: Array<RefundOrder>;
    public RequireDocumentsList: Array<RequireDocuments>;

                                                       
}

export class GeneralDetails {

    public FileStatus: number;
    public StatusName: string;
    public ExternalID: string;
    public ExternalName: number;
    
    public CustomOfficeNumber: number;
    public CustomOfficeName: string;

    public FilingNumber: string;
    public OpenFileCounter: number;
    public CloseFileCounter: string;
    public AgentExternalID: string;
    public AgentName: string;
}

export class FilesDetails {
    public ExternalID: number;
    public ExternalName: string;
    public FileNumber: string;
    public Numeral: string;
    public DisplayFileNumber: number;
    public ClaimEntityType: string;
    public EntityTypeName: string;
    public ClaimEntityID: number;
    public CreateDate: string;
    public CloseDate : string;
    public ClaimAmount : string;                           
    public TotalComponentAmount : string;
    public TotalRefundAmount : number;
    public Status : string;
    public StatusName : string;
}

export class RefundOrder {
    public ExternalID: number;
    public ExternalName: string;
    public PaymentOrderID: string;
    public PaymentProcessType: string;
    public PaymentProcessName: number;
    public AmountSum: string;
    public CreateDate: string;
    public ValidityDateTo: string;
    public PaymentOrderStatus: string;
    public PaymentOrderStatusName: string;
       
}
export class RequireDocuments {
    public DocumentCode: string;
    public TypeName: string;
    public FileNumber: string;
    public Numeral: string;
    public DisplayFileNumber: string;
    public DocumentID: string;
}