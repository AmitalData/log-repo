import {Guid} from '../../Infrastructure/Utilities/Guid';


export class CertificateConnectedItem {


    public Id: string;
    public DeclarationId: string;
    public InvoiceNumber: string;
    public ItemCode: string;
    public ClassificationCode: string;
    public TradeAgreementCode: string;
    public OriginCountryCode: string;
    public TradeAgreementName: string;
    public OriginCountryName: string;
    public CatalogNumber: string;
    public SequenceNumeric: number;
    public InvoiceCounterKey: number;
    public LineNumber: number;
    public ItemCertificateCounterKey: number;
    public ReqConfirmationTypeCode: string;
    public CertificateNumber: string;
    public CertificateExemptionTypeCode: string;
    public ResConfirmationTypeCode: string;
    public AttachmentTypeCode: string;
    public IsSelected: boolean;
}