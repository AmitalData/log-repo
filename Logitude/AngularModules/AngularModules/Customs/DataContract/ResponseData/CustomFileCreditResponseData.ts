import { ResponseDataBase } from './ResponseDataBase';

export 
    class CustomFileCreditResponseData extends ResponseDataBase {
    public ApplicationID: string;
    public CreditStatus: string;
    public BankCode: string;
    public PaymentDate: string;
    public PaymentTime: string;
    public PaymentDateTime: Date;
    public BillingTaxAmount: string;
    public IsTRansGove: boolean
    public IsReTRansGove: boolean
}