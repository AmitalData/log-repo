/// <reference path="../entitypms/documentoutpm.ts" />

import {DocumentOutPM} from '../EntityPMs/DocumentOutPM';
import {DocumentsFilingPM} from '../EntityPMs/DocumentsFilingPM';
export class ShipmentShareDocumentsData {
 
    public ShareDocuments: ShareDocument[];
    public IncludedShareDocuments: ShareDocument[];
    
    public ShipmentNumber: string;
    public  EntityId : string;
    public  AgentSharedManifestRef : string;
    public  ShipmentLevelCode : string;
    public  ShipmentLevelName: string;
    public  AgentId: string;
    public  TenantAgent : number;


}

export class ShareDocument {
    public DocumentTypeName: string;
    public DocumentTypeCode: string;
    public IsReady: boolean;
    public LastUpdateDate: Date;
    public LastShareDate: Date;
    public SecurityId: string;
    public EntityId: string;
    public FileName: string;
    public DirectionCode: string;
    public Included: boolean;
    public ShipmentNumber: string;
    public DocumentId: string;
    public DocumentsFilingId: string;
    public Extension: string;
    public FileSize: number;
    public VisibleBliudDocumentButton: boolean = false;
    public VisibleViewButton: boolean = false;
    public VisibleUploadButton: boolean = false;
    public TemplateFormatCode: string;

    public DocumentTypeId: string;
    public DocumentsFilingPM: DocumentsFilingPM;
    public DocumentOutPM: DocumentOutPM;
    public DocumentTypeCopyId: string;
    public ActionButtonLabel: string;
    public DocumentOutId: string;

}

