import { SupplierInvoicePM } from "Customs/EntityPMs/SupplierInvoicePM";
import { GenericRequestParams } from "./GenericRequestParams";
import { SupplierInvioceItemCertificatPM } from "Customs/EntityPMs/SupplierInvioceItemCertificatPM";
import { SupplierInvioceExportDefaultList } from "Customs/EntityLists/SupplierInvioceExportDefaultList";
import { SupplierInvioceExportDefaultPM } from "Customs/EntityPMs/SupplierInvioceExportDefaultPM";

export class MultiUpdateOcrParams extends GenericRequestParams {
    
    public SupplierInvioceItemCertificats:SupplierInvioceItemCertificat[];
    public DeclarationId:string;
    public SupplierInvoiceList:String;
    public SupplierInvioceExportDefault:SupplierInvioceExportDefaultPM;

}

export class SupplierInvioceItemCertificat{
    
	 
    private invoiceCounterKey: number;
    public get InvoiceCounterKey() { return this.invoiceCounterKey; }
    public set InvoiceCounterKey(newValue: number) { if (this.invoiceCounterKey != newValue) { this.invoiceCounterKey = newValue; } }
       
	 
    private lineNumber: number;
    public get LineNumber() { return this.lineNumber; }
    public set LineNumber(newValue: number) { if (this.lineNumber != newValue) { this.lineNumber = newValue;  } }
       
	 
    private itemCertificateCounterKey: number;
    public get ItemCertificateCounterKey() { return this.itemCertificateCounterKey; }
    public set ItemCertificateCounterKey(newValue: number) { if (this.itemCertificateCounterKey != newValue) { this.itemCertificateCounterKey = newValue;  } }
       
	 
    private certificateNumber: string;
    public get CertificateNumber() { return this.certificateNumber; }
    public set CertificateNumber(newValue: string) { if (this.certificateNumber != newValue) { this.certificateNumber = newValue;  } }
       
	 
    
	 
    private reqConfirmationTypeCode: string;
    public get ReqConfirmationTypeCode() { return this.reqConfirmationTypeCode; }
    public set ReqConfirmationTypeCode(newValue: string) { if (this.reqConfirmationTypeCode != newValue) { this.reqConfirmationTypeCode = newValue;  } }
       
	 
    private certificateExemptionTypeCode: string;
    public get CertificateExemptionTypeCode() { return this.certificateExemptionTypeCode; }
    public set CertificateExemptionTypeCode(newValue: string) { if (this.certificateExemptionTypeCode != newValue) { this.certificateExemptionTypeCode = newValue; } }
       
	 
    private attachmentTypeCode: string;
    public get AttachmentTypeCode() { return this.attachmentTypeCode; }
    public set AttachmentTypeCode(newValue: string) { if (this.attachmentTypeCode != newValue) { this.attachmentTypeCode = newValue;  } }
       
	 
    private resConfirmationTypeCode: string;
    public get ResConfirmationTypeCode() { return this.resConfirmationTypeCode; }
    public set ResConfirmationTypeCode(newValue: string) { if (this.resConfirmationTypeCode != newValue) { this.resConfirmationTypeCode = newValue;  } }
       
	 
    private customsAttachmentID: string;
    public get CustomsAttachmentID() { return this.customsAttachmentID; }
    public set CustomsAttachmentID(newValue: string) { if (this.customsAttachmentID != newValue) { this.customsAttachmentID = newValue; } }
       
	 
    private reqConfirmationTypeName: string;
    public get ReqConfirmationTypeName() { return this.reqConfirmationTypeName; }
    public set ReqConfirmationTypeName(newValue: string) { if (this.reqConfirmationTypeName != newValue) { this.reqConfirmationTypeName = newValue; } }
       
	 
    private certificateExemptionTypeName: string;
    public get CertificateExemptionTypeName() { return this.certificateExemptionTypeName; }
    public set CertificateExemptionTypeName(newValue: string) { if (this.certificateExemptionTypeName != newValue) { this.certificateExemptionTypeName = newValue;  } }
       
	 
    private attachmentTypeName: string;
    public get AttachmentTypeName() { return this.attachmentTypeName; }
    public set AttachmentTypeName(newValue: string) { if (this.attachmentTypeName != newValue) { this.attachmentTypeName = newValue;  } }
       
	 
    private resConfirmationTypeName: string;
    public get ResConfirmationTypeName() { return this.resConfirmationTypeName; }
    public set ResConfirmationTypeName(newValue: string) { if (this.resConfirmationTypeName != newValue) { this.resConfirmationTypeName = newValue;  } }
       
	 
    private sequenceNumeric: number;
    public get SequenceNumeric() { return this.sequenceNumeric; }
    public set SequenceNumeric(newValue: number) { if (this.sequenceNumeric != newValue) { this.sequenceNumeric = newValue; } }
       
	 
    private externalCertificatCode: string;
    public get ExternalCertificatCode() { return this.externalCertificatCode; }
    public set ExternalCertificatCode(newValue: string) { if (this.externalCertificatCode != newValue) { this.externalCertificatCode = newValue;  } }
       
	 
    private externalRequestTypeCode: string;
    public get ExternalRequestTypeCode() { return this.externalRequestTypeCode; }
    public set ExternalRequestTypeCode(newValue: string) { if (this.externalRequestTypeCode != newValue) { this.externalRequestTypeCode = newValue; } }
       
	 
    private approvalRequestNumber: string;
    public get ApprovalRequestNumber() { return this.approvalRequestNumber; }
    public set ApprovalRequestNumber(newValue: string) { if (this.approvalRequestNumber != newValue) { this.approvalRequestNumber = newValue;  } }
       


}

