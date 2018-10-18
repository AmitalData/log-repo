

import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {DocumentOutCopyPM} from '../../Common/EntityPMs/DocumentOutCopyPM';
export class DocumentOutPM {


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

    private entityId: string;
    public get EntityId() { return this.entityId; }
    public set EntityId(newValue: string) { this.entityId = newValue; this.MarkAsDirty(); }

    private securityId: string;
    public get SecurityId() { return this.securityId; }
    public set SecurityId(newValue: string) { this.securityId = newValue; this.MarkAsDirty(); }

    private isChangeIssuedDate: boolean;
    public get IsChangeIssuedDate() { return this.isChangeIssuedDate; }
    public set IsChangeIssuedDate(newValue: boolean) { this.isChangeIssuedDate = newValue; this.MarkAsDirty(); }


    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; this.MarkAsDirty(); }


    private issuedByUserId: string;
    public get IssuedByUserId() { return this.issuedByUserId; }
    public set IssuedByUserId(newValue: string) { this.issuedByUserId = newValue; this.MarkAsDirty(); }

    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) { this.notes = newValue; this.MarkAsDirty(); }

    

    private documentTypeId: string;
    public get DocumentTypeId() { return this.documentTypeId; }
    public set DocumentTypeId(newValue: string) { this.documentTypeId = newValue; this.MarkAsDirty(); }


    private issued: boolean;
    public get Issued() { return this.issued; }
    public set Issued(newValue: boolean) { this.issued = newValue; this.MarkAsDirty(); }

    private issuedByUserName: string;
    public get IssuedByUserName() { return this.issuedByUserName; }
    public set IssuedByUserName(newValue: string) { this.issuedByUserName = newValue; this.MarkAsDirty(); }

    private childEntityId: string;
    public get ChildEntityId() { return this.childEntityId; }
    public set ChildEntityId(newValue: string) { this.childEntityId = newValue; this.MarkAsDirty(); }

    private childEntityReference: string;
    public get ChildEntityReference() { return this.childEntityReference; }
    public set ChildEntityReference(newValue: string) { this.childEntityReference = newValue; this.MarkAsDirty(); }
    
    private documentTypeName: string;
    public get DocumentTypeName() { return this.documentTypeName; }
    public set DocumentTypeName(newValue: string) { this.documentTypeName = newValue; this.MarkAsDirty(); }


    private documentTypeCode: string;
    public get DocumentTypeCode() { return this.documentTypeCode; }
    public set DocumentTypeCode(newValue: string) { this.documentTypeCode = newValue; this.MarkAsDirty(); }


    private issuedDate: Date;
    public get IssuedDate() { return this.issuedDate; }
    public set IssuedDate(newValue: Date) { this.issuedDate = newValue; this.MarkAsDirty(); }
    
    private documentTemplateEditorTool: string;
    public get DocumentTemplateEditorTool() { return this.documentTemplateEditorTool; }
    public set DocumentTemplateEditorTool(newValue: string) { this.documentTemplateEditorTool = newValue; this.MarkAsDirty(); }
    
    private documentTypeDefaultEditorTool: string;
    public get DocumentTypeDefaultEditorTool() { return this.documentTypeDefaultEditorTool; }
    public set DocumentTypeDefaultEditorTool(newValue: string) { this.documentTypeDefaultEditorTool = newValue; this.MarkAsDirty(); }
    

    private followUpId: string;
    public get FollowUpId() { return this.followUpId; }
    public set FollowUpId(newValue: string) { this.followUpId = newValue; this.MarkAsDirty(); }

    private documentOutCopies: DocumentOutCopyPM[];
    public get DocumentOutCopies() { return this.documentOutCopies; }
    public set DocumentOutCopies(newValue: DocumentOutCopyPM[]) { this.documentOutCopies = newValue; this.MarkAsDirty(); }
    
    private followUpCount: number;
    public get FollowUpCount() { return this.followUpCount; }
    public set FollowUpCount(newValue: number) { this.followUpCount = newValue; this.MarkAsDirty(); }
    

    private extension: string;
    public get Extension() { return this.extension; }
    public set Extension(newValue: string) { this.extension = newValue; this.MarkAsDirty(); }

    private reportTemplate: any;
    public get ReportTemplate() { return this.reportTemplate; }
    public set ReportTemplate(newValue: any) { this.reportTemplate = newValue; this.MarkAsDirty(); }
    
    private hasFollowUp: boolean;
    public get HasFollowUp() { return this.hasFollowUp; }
    public set HasFollowUp(newValue: boolean) { this.hasFollowUp = newValue; this.MarkAsDirty(); }

    private hTMLTemplate: any;
    public get HTMLTemplate() { return this.hTMLTemplate; }
    public set HTMLTemplate(newValue: any) { this.hTMLTemplate = newValue; this.MarkAsDirty(); }
    
    private templateType: string;
    public get TemplateType() { return this.templateType; }
    public set TemplateType(newValue: string) { this.templateType = newValue; this.MarkAsDirty(); }
    
    private documentTypeSubject: string;
    public get DocumentTypeSubject() { return this.documentTypeSubject; }
    public set DocumentTypeSubject(newValue: string) { this.documentTypeSubject = newValue; this.MarkAsDirty(); }
    
    private editableFields: any;
    public get EditableFields() { return this.editableFields; }
    public set EditableFields(newValue: any) { this.editableFields = newValue; this.MarkAsDirty(); }
    
    private documentTemplateId: string;
    public get DocumentTemplateId() { return this.documentTemplateId; }
    public set DocumentTemplateId(newValue: string) { this.documentTemplateId = newValue; this.MarkAsDirty(); }
    

    private emailTemplateId :string;
    public get EmailTemplateId() { return this.emailTemplateId; }
    public set EmailTemplateId(newValue: string) { this.emailTemplateId = newValue; this.MarkAsDirty(); }
    
    private xamlDocumentId :string;
    public get XamlDocumentId() { return this.xamlDocumentId; }
    public set XamlDocumentId(newValue: string) { this.xamlDocumentId = newValue; this.MarkAsDirty(); }
    
    private documentTypeObjectTableId: string;
    public get DocumentTypeObjectTableId() { return this.documentTypeObjectTableId; }
    public set DocumentTypeObjectTableId(newValue: string) { this.documentTypeObjectTableId = newValue; this.MarkAsDirty(); }
    
    private needsRebuild: boolean;
    public get NeedsRebuild() { return this.needsRebuild; }
    public set NeedsRebuild(newValue: boolean) { this.needsRebuild = newValue; this.MarkAsDirty(); }
    
    private isBlobExist: boolean;
    public get IsBlobExist() { return this.isBlobExist; }
    public set IsBlobExist(newValue: boolean) { this.isBlobExist = newValue; this.MarkAsDirty(); }
    
    private fileSize: number;
    public get FileSize() { return this.fileSize; }
    public set FileSize(newValue: number) { this.fileSize = newValue; this.MarkAsDirty(); }
    
    private fileName: string;
    public get FileName() { return this.fileName; }
    public set FileName(newValue: string) { this.fileName = newValue; this.MarkAsDirty(); }



    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; this.MarkAsDirty(); }

    MarkAsDirty() {
        this.IsDirty = true;
    }
}