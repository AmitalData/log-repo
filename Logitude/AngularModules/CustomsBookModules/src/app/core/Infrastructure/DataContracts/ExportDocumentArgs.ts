

import {Injectable} from '@angular/core';

@Injectable()

export class ExportDocumentArgs {
    public DocumentTypeId: string;
    public DocumentTypeTemplateId: string;
    public Tenant: number;
    public CurrentDocumentOutId: string;
    public CurrentDocumentTypeCode: string;
    public DocumentTypeCopyId: string;
    public EntityId: string;
    public IsDisplayOnly: boolean;
    public LoggedContactId: string;
    public ObjectTableId: string;
    public PageNumber: number;
    public ReportKey: string;
    public RequestMethodType: string;
    public ChildEntityId: string;
    public ProcessType: string;
    public ChildObjectTableId: string;
    public LoggedContactName: string;
    public AccountingCurrencyId: string;
    public DocumentTypeCopyIdsList: string[];
    public DocumentTypeName: string;
    public ObjectTableName: string;
    public DocumentTemplateEditorTool: string;

    


}

