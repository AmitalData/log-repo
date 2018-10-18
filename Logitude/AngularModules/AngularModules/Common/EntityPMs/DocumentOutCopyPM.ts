

import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class DocumentOutCopyPM {


    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    public IsDirty: boolean;


    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private documentId : number;
    public get DocumentId() { return this.documentId; }
    public set DocumentId(newValue: number) { this.documentId = newValue; this.MarkAsDirty(); }

    private documentOutId : string;
    public get DocumentOutId() { return this.documentOutId; }
    public set DocumentOutId(newValue: string) { this.documentOutId = newValue; this.MarkAsDirty(); }

    private documentTypeCopyId : string;
    public get DocumentTypeCopyId() { return this.documentTypeCopyId; }
    public set DocumentTypeCopyId(newValue: string) { this.documentTypeCopyId = newValue; this.MarkAsDirty(); }


    private docoumentTypeCopyName : string;
    public get DocoumentTypeCopyName() { return this.docoumentTypeCopyName; }
    public set DocoumentTypeCopyName(newValue: string) { this.docoumentTypeCopyName = newValue; this.MarkAsDirty(); }

    private documentTypeCopyNameWithDocumentTypeName : string;
    public get DocumentTypeCopyNameWithDocumentTypeName() { return this.documentTypeCopyNameWithDocumentTypeName; }
    public set DocumentTypeCopyNameWithDocumentTypeName(newValue: string) { this.documentTypeCopyNameWithDocumentTypeName = newValue; this.MarkAsDirty(); }
    
    private lastPrintedByUserId : string;
    public get LastPrintedByUserId() { return this.lastPrintedByUserId; }
    public set LastPrintedByUserId(newValue: string) { this.lastPrintedByUserId = newValue; this.MarkAsDirty(); }


    private lastPrintDate : Date;
    public get LastPrintDate() { return this.lastPrintDate; }
    public set LastPrintDate(newValue: Date) { this.lastPrintDate = newValue; this.MarkAsDirty(); }

    private fileSize: number;
    public get FileSize() { return this.fileSize; }
    public set FileSize(newValue: number) { this.fileSize = newValue; this.MarkAsDirty(); }

    private fileName : string;
    public get FileName() { return this.fileName; }
    public set FileName(newValue: string) { this.fileName = newValue; this.MarkAsDirty(); }

    private lastPrintedByUserName : string;
    public get LastPrintedByUserName() { return this.lastPrintedByUserName; }
    public set LastPrintedByUserName(newValue: string) { this.lastPrintedByUserName = newValue; this.MarkAsDirty(); }

    private isAttachSelect: boolean;
    public get IsAttachSelect() { return this.isAttachSelect; }
    public set IsAttachSelect(newValue: boolean) { this.isAttachSelect = newValue; this.MarkAsDirty(); }
    
    private calculatedFileName: string;
    public get CalculatedFileName() { return this.calculatedFileName; }
    public set CalculatedFileName(newValue: string) { this.calculatedFileName = newValue; this.MarkAsDirty(); }

    MarkAsDirty() {
        this.IsDirty = true;
    }
}