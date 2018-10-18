import {CertificateConnectedItem} from './CertificateConnectedItem';



export class CertificateTicket {

   
    public Id: string;
    public AttachmentTypeCode: string;
    public CertificateNumber: string;
    public ResConfirmationTypeCode: string;
    public CertificateExemptionTypeCode: string;
    public ReqConfirmationTypeCode: string;
    public ResConfirmationTypeName: string;
    public ReqConfirmationTypeName: string
    public AttachmentTypeName: string
    public CertificateExemptionTypeName: string;
    public CustomsAttachmentId: string;
    public ExternalCertificatCode: string;
    public InvoiceNumber: string;
    public DeclarationId: string;
    public oldAttachment: string;
    public oldCertificateNumber: string;
    public oldResConfirmation: string;
    public oldCertificateExempt: string;
    public SelectedItems: CertificateConnectedItem[];
    public IsAllSelected: boolean;
    public ConnectedItemsKeys: string;
    public ExcludedItemsKeys: string;

}