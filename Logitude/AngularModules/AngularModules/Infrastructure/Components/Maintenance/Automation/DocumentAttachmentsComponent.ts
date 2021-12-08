import { Component, OnInit } from '@angular/core';
import { DocumentTypeListService } from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import { ApiQueryFilters } from '../../../DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Utilities/SessionInfo';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { DocumentTypeList } from '../../../../Common/EntityLists/DocumentTypeList';
import { ObservableCollection } from '../../../Utilities/ObservableCollection';
import { SessionLocator } from '../../../Utilities/SessionLocator';
import { DocumentTypeCopyList } from '../../../../Common/EntityLists/DocumentTypeCopyList';
import { DocumentTypeListExtendedService } from '../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import { AutomationOnUpdateDocument } from '../../../DataContracts/AutomationOnUpdateDocument';

@Component({
    selector: 'DocumentAttachmentsComponent',
    templateUrl: './DocumentAttachmentsComponent.html',
})

export class DocumentAttachmentsComponent implements OnInit {
    public DocumentTypeCopyLists: DocumentTypeCopyList[];
    public DocumentTypeLists: DocumentTypeList[];
    DataContext: any;
    DocOutAttachmentLists: DocumentAttachmentItem[] = [];
    DocInAttachmentLists: DocumentAttachmentItem[] = [];
    DocumentAttachments: OnUpdateDocumentTypeAttachment[] = [];

    ObjectTableId: string;
    private CurrentSession = SessionLocator.SelectedSession;

    IsLoadDocumentInDocument: boolean = false;
    IsLoadDocumentOutDocument: boolean = false;
    IsReady: boolean = false;
    public DocOutItemsSource: ObservableCollection;
    public DocInItemsSource: ObservableCollection;

    constructor() {
        this.DocOutAttachmentLists = [];
        this.DocInAttachmentLists = [];
        this.DocOutItemsSource = new ObservableCollection([]);
        this.DocInItemsSource = new ObservableCollection([]);
    }

    ngOnInit() {
    }

    public automationOnUpdateDocument: AutomationOnUpdateDocument;
    SetWindowArgs(args: any) {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.automationOnUpdateDocument = args.AutomationOnUpdateDocument;
        if (this.automationOnUpdateDocument.DocumentTypeLists)
            this.DocumentAttachments = this.automationOnUpdateDocument.DocumentTypeLists;
        else
            this.DocumentAttachments = [];
        this.ObjectTableId = args.ObjectTableId;
        this.LoadData();
    }

    public LoadData() {
        this.LoadDocumentOut();
        this.LoadDocumentIn();
    }

    LoadDocumentOut() {
        var _documentTypeListService: DocumentTypeListExtendedService = new DocumentTypeListExtendedService();
        _documentTypeListService.GetDocumentTypeCopyLists(this.ObjectTableId).subscribe((response: any) => {
            this.HandleDocumentsOutLoaded(response);
        });
    }

    IsNotDocumentOutFound: boolean = false;
    IsNotDocumentInFound: boolean = false;

    HandleDocumentsOutLoaded(pmResponse: ServiceResponse) {
        if (!pmResponse.HasError) this.FillDocOutAttachmentLists(pmResponse);

        if (this.DocOutAttachmentLists.length == 0) this.IsNotDocumentOutFound = true;
        this.DocOutItemsSource.Clear();
        this.DocOutItemsSource.AppendCollection(this.DocOutAttachmentLists);
        this.IsLoadDocumentOutDocument = true;
        this.LoadComplete();
    }

    FillDocOutAttachmentLists(pmResponse: ServiceResponse) {
        let documentTypeCopyLists: DocumentTypeCopyList[] = pmResponse.Result;
        documentTypeCopyLists.forEach((copy) => {
            let item = new DocumentAttachmentItem(copy.DocumentTypeId, "DocOut", copy.Name, copy.Id);
            if (this.DocumentAttachments.filter(d => d.DocumentTypeCopyId == item.DocumentTypeCopyId && d.Type == "DocOut")[0]) item.IsChecked = true;
            this.DocOutAttachmentLists.push(item);
        });
    }

    LoadDocumentIn() {
        let _documentTypeListService: DocumentTypeListService = new DocumentTypeListService();
        let apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
        _documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((response: any) => {
            let pmResponse: ServiceResponse = response;
            this.HandleDocumentsInLoaded(pmResponse);
        });
    }

    HandleDocumentsInLoaded(pmResponse: ServiceResponse) {
        if (!pmResponse.HasError) this.FillDocInAttachmentLists(pmResponse);
        if (this.DocInAttachmentLists.length == 0) this.IsNotDocumentInFound = true;
        this.DocInItemsSource.Clear();
        this.DocInItemsSource.AppendCollection(this.DocInAttachmentLists);
        this.IsLoadDocumentInDocument = true;
        this.LoadComplete();
    }

    FillDocInAttachmentLists(pmResponse: ServiceResponse) {
        let documentTypeList: DocumentTypeList[] = pmResponse.Result;
        documentTypeList.filter(d => d.ObjectTableId == this.ObjectTableId && d.IsDocIn == true).forEach((doc) => {
            let item = new DocumentAttachmentItem(doc.Id, "DocIn", doc.Name);
            if (this.DocumentAttachments.filter(d => d.DocumentTypeId == item.DocumentTypeId && d.Type == "DocIn")[0]) item.IsChecked = true;       
            this.DocInAttachmentLists.push(item);
        });
    }

    LoadComplete() {
        if (!this.IsLoadDocumentOutDocument || !this.IsLoadDocumentInDocument) return;
        this.CurrentSession.StopBusyIndicator();
        this.IsReady = true;
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    GetDocumentAttachment(doc: DocumentAttachmentItem) {
        var item = new OnUpdateDocumentTypeAttachment();
        item.DocumentTypeId = doc.DocumentTypeId;
        item.DocumentTypeCopyId = doc.DocumentTypeCopyId;
        item.Type = doc.Type;
        item.DocumentTypeName = doc.DocumentTypeName;
        return item;
    }

    SaveButtonClicked() {
        this.automationOnUpdateDocument.IsChanged = true;
        this.automationOnUpdateDocument.DocumentTypeLists = this.BuildDocumentAttachmentLists();
        this.CurrentSession.CurrentWindow.Close("OK");
    }

    BuildDocumentAttachmentLists() {
        var documentAttachments = new Array<OnUpdateDocumentTypeAttachment>();
        this.DocOutAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
            documentAttachments.push(this.GetDocumentAttachment(doc));
        });
        this.DocInAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
            documentAttachments.push(this.GetDocumentAttachment(doc));
        });
        return documentAttachments;
    }
}

export class DocumentAttachmentItem {
    public DocumentTypeId: string;
    public DocumentTypeName: string;
    public DocumentTypeCopyId: string;
    public Type: string;
    public IsChecked: boolean;
    constructor(id: string,  type: string, name: string , copyId:string = null) {
        this.DocumentTypeId = id;
        this.DocumentTypeCopyId = copyId;
        this.Type = type;
        this.DocumentTypeName = name;
    }
}

export class OnUpdateDocumentTypeAttachment {
    public DocumentTypeId: string;
    public DocumentTypeName: string;
    public DocumentTypeCopyId: string;
    public Type: string;
}
