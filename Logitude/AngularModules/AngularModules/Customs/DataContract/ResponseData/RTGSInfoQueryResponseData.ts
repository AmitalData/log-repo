import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class RTGSInfoQueryResponseData extends INF_MSG_GenericResponseData {

    public ActiveInd: string;
    public CreationDate: string;
    public GeneralOutputRemarks: string;
    public QueryDate: string;
    public QueryTime: string;
    public RTGSCurrentBalance: string;
    public RTGSDeposits: string;
    public RTGSRefund: string;
    public RTGSUsed: string;

    public TransactionsList: Array<TransactionResult>;

}

export class TransactionResult {

    public EntityID: string;
    public EntityType: string;
    public PaymentDate: string;
    public PaymentID: string;
    public PaymentStatus: string;
    public RTGSBalance: string;
    public TransactionAmount: string;
    public TransactionType: string;
    public UpdateUser: string;
}
