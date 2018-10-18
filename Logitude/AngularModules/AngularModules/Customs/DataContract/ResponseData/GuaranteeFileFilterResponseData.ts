import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';

export class GuaranteeFileFilterResponseData extends INF_MSG_GenericResponseData {

    public DisplayFileNumber: string;
    public StatusName: string;
    public CreditLimit: string;
    public EntityTypeName: string;
    public CustomOfficeNumber: string;
    public CustomOfficeName: string;
    public CreditBalance: string;
    public GuaranteedName: string;
    public EntityNumber: string;
    public AgentExternalID: string;
    public AgentName: string;
    public GuaranteeExecutedAmountAdjusted: string;
    public GuaranteeAmount: string;
    public Validity: string;
    public GuaranteeLettersList: Array<ExternalGuaranteeLettersResult>;
    public CreditTransactionsList: Array<ExternalCreditTransactionsResult>;
    public RequireDocumentsList: Array<RequiredDocumentsResult>;
}

export class ExternalGuaranteeLettersResult {

    public GuaranteeTypeName: string;
    public CertificateID: string;
    public GuaranteeExternalCertificateNumebr: string;
    public GuaranatorName: string;
    public GuaranteeValidityDate: string;
    public CertificateAmount: string;
    public CertificateAllocation: string;
    public AvaliableCertificateAmount: string;
    public GuaranteeStatusName: string;
}

export class ExternalCreditTransactionsResult {

    public CreditTransactionDate: string;
    public CreditTransactionName: string;
    public EntityTypeName: string;
    public EntityNumber: string;
    public CreditTransactionAmount: string;
}

export class RequiredDocumentsResult {

    public DocumentCode: string;
    public DocumentTypeName: string;
    public FileNumber: string;
    public Numeral: string;
    public DisplayFileNumber: string;
    public DocumentID: string;

}