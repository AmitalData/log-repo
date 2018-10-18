import {EntityArgs} from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeTemplateList} from '../../../../../../Common/EntityLists/DocumentTypeTemplateList';
import {DocumentOutCopyPM} from '../../../../../../Common/EntityPMs/DocumentOutCopyPM';

export class DocumentOutCopyViewModel {


    public Id: string;
    public Tenant: number;
    public DocumentId: number;
    public DocumentOutId: string;
    public DocumentTypeCopyId: string;
    public DocumentTypeCopyCode: string;
    public DocumentTypeCopyNameWithDocumentTypeName: string;
    public LastPrintedByUserId: string;
    public LastPrintDate: Date;
    public FileSize: number;
    public FileName: string;
    public LastPrintedByUserName: string;
    public IsAttachSelect: boolean;
  
    public Key: string;
    constructor(documentOutCopyPM: DocumentOutCopyPM) {
        this.Id = documentOutCopyPM.Id;
        this.Tenant = documentOutCopyPM.Tenant;

        this.DocumentId = documentOutCopyPM.DocumentId;
        this.DocumentOutId = documentOutCopyPM.DocumentOutId;

        this.DocumentTypeCopyId = documentOutCopyPM.DocumentTypeCopyId;
        this.DocumentTypeCopyNameWithDocumentTypeName = documentOutCopyPM.DocumentTypeCopyNameWithDocumentTypeName;

        this.LastPrintedByUserId = documentOutCopyPM.LastPrintedByUserId;
        this.LastPrintDate = documentOutCopyPM.LastPrintDate;
        this.FileSize = documentOutCopyPM.FileSize;
        this.FileName = documentOutCopyPM.FileName;

        this.LastPrintedByUserName = documentOutCopyPM.LastPrintedByUserName;
        this.IsAttachSelect = documentOutCopyPM.IsAttachSelect;

        console.log(this.Id);
    }
}