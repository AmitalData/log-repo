import {GenericRequestParams} from './GenericRequestParams';

export class PaymentOrderRequestParams extends GenericRequestParams {

    public AgentID: string;
    public AgentExternalId: string;
    public PaymentID: string;
    public PaymentType: string;
    public PaymentAmount: string;
    public PaymentMethodType: string;
    public ClientId: string;
    public ExternalID: string;
    public CustomFileNo: string;
    public EntityType: string;
    public EntityExternalID: string;
    public PaymentOrderStatus: string;
    public PaymentProcess: string;
    public paymentDateFrom: Date;
    public paymentDateTo: Date;
    public EffectiveDateFrom: Date;
    public EffectiveDateTo: Date;
    public CustomBankId: string;
    public BankID: string;
    public BranchID: string;
    public BankAccount: string;

}