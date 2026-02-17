import {Output, EventEmitter, Component, OnInit, ChangeDetectorRef, AfterViewInit}  from '@angular/core';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {DocumentCopiesViewModel} from './DocsOut/ViewModel/DocumentCopiesViewModel';
import {DocumentTypeTemplateViewModel} from './DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SendHtmlDocumentFilter} from './DocsOut/Filters/SendHtmlDocumentFilter';
import {FroalaEditorSetting} from './DocsOut/FroalaEditorSetting';
import {DocumentOutPMService} from '../../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocumentOutCopyViewModel} from './DocsOut/ViewModel/DocumentOutCopyViewModel';
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DocsOutDataViewModel} from './DocsOut/ViewModel/DocsOutDataViewModel';
import {AttachmentsList} from './DocsOut/Filters/AttachmentsList';
import {DocumentTypeListService} from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityPartner} from '../../../../Infrastructure/DataContracts/EntityPartner';
import {DocumentTypeTemplateListExtendedService} from '../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import {DocumentTypeTemplateFilter} from './DocsOut/Filters/DocumentTypeTemplateFilter';
import {HtmlEditorService} from '../../../../Common/Services/DocumentServices/HtmlEditorService';
import {CommunicationAttachmentExtendedPMService} from '../../../../Common/Services/ExtendedPMs/CommunicationAttachmentExtendedPMService';
import {CommunicationLogExtendedPMService} from '../../../../Common/Services/ExtendedPMs/CommunicationLogExtendedPMService';
import {DocumentTypeTemplateList} from '../../../../Common/EntityLists/DocumentTypeTemplateList';
import {DocumentTypeList} from '../../../../Common/EntityLists/DocumentTypeList'
import {DocumentOutCopyPM} from '../../../../Common/EntityPMs/DocumentOutCopyPM';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {CommunicationAttachmentPM} from '../../../../Common/EntityPMs/CommunicationAttachmentPM';
import {DocumentOutPM} from '../../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentsFilingPM} from '../../../../Common/EntityPMs/DocumentsFilingPM';
import {CommunicationLogPMViewModel} from './DocsOut/ViewModel/CommunicationLogPMViewModel';
import {AttachmentDocment} from './DocsOut/ViewModel/AttachmentDocment';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {DocumentExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import {DocumentPM} from '../../../../Common/EntityPMs/DocumentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
declare var System: any;
declare var window: any;
declare var htmlComponentProparitiesTrue, GetPlainTextFromHtml, htmlComponentProparitiesFalse: any;

@Component({
    moduleId: module.id,
    selector: 'SendDocument',
    templateUrl: './SendControl.html',
    providers: [CommunicationLogExtendedPMService, CommunicationAttachmentExtendedPMService, DocumentOutPMService, ServiceArgs, DocumentTypeTemplatePMService, DocumentTypeTemplateListExtendedService, HtmlEditorService, DocumentsFilingExtendedPMService, DocumentExtendedService, DocumentTypePMExtendedService, DocumentTypeListService],
    inputs: ['selectedInternalDocument']
})


export class SendDocumentComponent implements OnInit, AfterViewInit {

    PartnersObslist: EntityPartner[];
    DocumentTypeId: string;
    DocumentOutId: string;
    IsDisableAddTemplateFromLibrary: boolean = false;
    @Output() OnCloseAttachmentDocsInEvent: EventEmitter<any> = new EventEmitter();
    @Output() OnCloseSendToContactsEvent: EventEmitter<any> = new EventEmitter();
    @Output() OnCloseSharedWithAgentsEvent: EventEmitter<any> = new EventEmitter();

    SelectedInternalDocument: DocsOutDataViewModel;
    EntityId: string;
    public HtmlEditorData: string;
    public ReportTemplates: DocumentTypeTemplateViewModel[];
    DocumenttypetemplateLists: DocumentTypeTemplateViewModel[];
    public SelectedDocumentTypeTemplateViewModel: DocumentTypeTemplateViewModel;
    public DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
    IsShowEditHtml: boolean = false;
    public AreaAttachmentHeight: any;
    public AreaAttachmentMargin: any;
    public AreaAttachmentWidth: string = "600px";
    public CommunicationAttachmentPMs: CommunicationAttachmentPM[];


    public CommunicationLogs: CommunicationLogPMViewModel[];

    AttrTitleShowTemplateList: string = "Expand";
    documentInPMs: DocumentsFilingPM[];
    public ComponentSendTokey: string = "";
    ShowBusyIndicator: boolean;
    public PageType: string = "Send";
    IsShowTemplateList: boolean = false;
    RefreshTemplateId: string;
    DocumentOutCopyId: string;
    Tenant: number;
    Subject: string = null;
    ToEmail: string = "";
    Bcc: string = "";
    Cc: string = "";
    IsSend: boolean;
    SelectId: string;
    CurrentDocument: DocumentOutPM;
    CurrentDocumentType: DocumentTypePM;
    IsEnableLinkAttachExternal: boolean = true;
    ObjecttableName: string;
    public DocumentOutLists: DocumentOutPM[];
    public froalaEditorSetting: FroalaEditorSetting;
    ShowInactiveCheckBoxKey: string;
    public SelectedItemFromMenu: DocumentTypeTemplateViewModel;
    IsShowCcBox: boolean = false;
    IsShowBccBox: boolean = false;
    IsHideBccCcLinkArea: boolean = false;
    public AttachmentsLists: AttachmentsList[];
    ObjectTableId: string;

    DocumentCopiesList: DocumentOutCopyViewModel[];
    IsShowAttachmentList: boolean;
    IsCheckedInActive: boolean = false;
    IsRefreshFrolaEditor: boolean;

    IsEnableLinkDocOout: boolean = true;
    IsEnableLinkDocsSharedWithAgents: boolean = true;
    IsEnableLinkDocIn: boolean = true;

    table: Element;
    table2: Element;
    div: Element;
    tr: Element;
    tr2: Element;
    td: Element;
    tbdy: Element;
    tbdy2: Element;
    CountTd = 1;
    documentPM: DocumentPM;
    Count = 0;
    Title: string;
    IsSendEditMode: boolean = true;
    AttachmentListId: string;
    EventRefreshName: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    From: string = "";
    ReplyTo: string = "";
    IsShowLinkDocsSharedWithAgents: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _communicationLogExtendedPMService: CommunicationLogExtendedPMService, public _communicationAttachmentExtendedPMService: CommunicationAttachmentExtendedPMService, public _documentOutPMService: DocumentOutPMService, public _documentExtendedService: DocumentExtendedService, public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService, public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService, public _htmlEditorService: HtmlEditorService, public _documentTypePMService: DocumentTypePMExtendedService, private cd: ChangeDetectorRef, public _documentTypeListService: DocumentTypeListService) {

        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();

        }


        if (FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "UPDATE") && FeatureLocator.HasFeaturePermession("DocumentType", "HTMLEMAIL")) {
            this.IsShowEditHtml = true;
        }


        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.PageType = "Send";
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.ShowInactiveCheckBoxKey = Guid.newGuid();

        this.AttachmentListId = Guid.newGuid();
        this.AttachmentsLists = new Array<AttachmentsList>();
        this.DocumentTypeTemplatePMLists = [];


    }

    ngOnInit(


    ) {



    }

    ngAfterViewInit(


    ) {

        if (this.SelectedInternalDocument && this.IsSendEditMode && this.SelectedInternalDocument.TemplateType.toUpperCase() != "M") {

            var copy = this.SelectedInternalDocument.CurrentDocument.DocumentOutCopies.filter(d => d.Id == this.SelectedInternalDocument.documentOutCopyId)[0];
            if (copy) {
                var attachment = new AttachmentsList();
                attachment.Tenant = this.SelectedInternalDocument.CurrentDocument.Tenant;

                var documentType: any = this.SelectedInternalDocument.DocumentType;

                if (documentType) {
                    var attachmentName = documentType.Name;
                    if (attachmentName != copy.DocoumentTypeCopyName) {
                        attachmentName = attachmentName + "-" + copy.DocoumentTypeCopyName;
                    }
                }


                attachment.DocumentTypeCopyNameWithDocumentTypeName = attachmentName;
                attachment.FileSize = this.SelectedInternalDocument.CurrentDocument.FileSize;
                attachment.ShowRemoveLink = true;
                attachment.Id = copy.Id;

                var attachmentsList = new Array<AttachmentsList>();
                attachmentsList.push(attachment);

                this.AttachmentsLists = attachmentsList;
                this.BliudAttachmentList(attachmentsList, false, false);
            }
        }

        if (this.SelectedInternalDocument.AttachmentsLists) {
            this.BliudAttachmentList(this.SelectedInternalDocument.AttachmentsLists, false, false);
        }

    }


    ChildEntityId: string;
    ChildObjectTableId: string;
    SetDataContext(dataContext: DocsOutDataViewModel) {
        this.SelectedInternalDocument = dataContext;
        if (this.SelectedInternalDocument.ModeSendDocument == "preview") {
            this.IsSendEditMode = false;
            this.froalaEditorSetting.IsDisableEdit = true;
        }

        if (this.IsSendEditMode) {
            this.froalaEditorSetting.Height = this.SelectedInternalDocument.WindowHeight - 210;
            this.froalaEditorSetting.IsDisableEdit = false;
        }
       
        if (!dataContext.DocumentTypePM && !AppTool.IsNullOrEmpty(dataContext.Id)) {
            this.CurrentSession.StartBusyIndicator("Loading...");
            this._documentTypePMService.GetSinglePMWithOutInclude(dataContext.Id, SessionLocator.Tenant).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    dataContext.DocumentTypePM = pmResponse.Result;
                }
                this.CurrentSession.StopBusyIndicator();
                this.Start(dataContext);
            });

        }
        else this.Start(dataContext);



    }


    Start(item: DocsOutDataViewModel) {
        this.SelectedInternalDocument = item;

        this.EntityId = this.SelectedInternalDocument.EntityId;
        this.DocumentOutCopyId = this.SelectedInternalDocument.documentOutCopyId;

        this.CurrentDocument = this.SelectedInternalDocument.CurrentDocument;
        this.CurrentDocumentType = this.SelectedInternalDocument.DocumentTypePM;
        this.ObjectTableId = this.SelectedInternalDocument.CurrentObjectTableId;
        this.EventRefreshName = this.SelectedInternalDocument.EventRefreshName;
        if (this.CurrentDocumentType) {
            this.DocumentTypeId = this.CurrentDocumentType.Id;
        }
        if (this.CurrentDocument) {
            this.DocumentOutId = this.CurrentDocument.Id;
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedInternalDocument.ToSpecificeEmail)) {
            this.ToEmail = this.SelectedInternalDocument.ToSpecificeEmail;
        }


        this.ChildEntityId = this.SelectedInternalDocument.ChildEntityId ? this.SelectedInternalDocument.ChildEntityId : "";
        this.ChildObjectTableId = this.SelectedInternalDocument.ChildObjectTableId ? this.SelectedInternalDocument.ChildObjectTableId : "";


        if (AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            if (this.SelectedInternalDocument && this.SelectedInternalDocument.DocumentTypePM) {
                var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];
                var tableId = this.SelectedInternalDocument.DocumentTypePM.ObjectTableId;
                if (tableId != this.ObjectTableId) {
                    this.ChildObjectTableId = tableId;
                }
            }
        }


        this.Tenant = SessionLocator.Tenant;

        var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];

        this.ObjecttableName = table.Name;

        if (this.IsSendEditMode) {


            if (this.ObjecttableName == "Shipment") {
                if (FeatureLocator.HasFeaturePermession("AgentSharedDocument", "DOCSSHAREDVIAEMAIL")) {

                    if (FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
                        if (this.SelectedInternalDocument.EntityPM && this.SelectedInternalDocument.EntityPM.DirectionId == "E" && (this.SelectedInternalDocument.EntityPM.ShipmentLevelCode == "C" || this.SelectedInternalDocument.EntityPM.ShipmentLevelCode == "D")) {
                            this.IsShowLinkDocsSharedWithAgents = true;
                        }
                    }


                }
            }

            if (!AppTool.IsNullOrEmpty(this.SelectedInternalDocument.Subject)) {
                this.Subject = this.SelectedInternalDocument.Subject;
            }
            else {
                this.Subject = this.CurrentDocument.DocumentTypeSubject != null ? this.CurrentDocument.DocumentTypeSubject : this.CurrentDocument.DocumentTypeName;
            }

            this.LoadDocumentTypeTemplates(null);
        }
        else {

            this.ViewCommunicationLog();
        }
    }


    ViewCommunicationLog() {
        if (this.SelectedInternalDocument.SelectedCommunicationLogViewMode) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            this.Cc = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.CC;
            this.ToEmail = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.To;
            this.Subject = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Subject;
            this.Bcc = this.SelectedInternalDocument.SelectedCommunicationLogViewMode.CurrentEntityPm.BCC;
            var froalaheight = this.SelectedInternalDocument.WindowHeight - 140;

            if (this.Cc) {
                froalaheight -= 22;
                this.IsShowCcBox = true;
            }

            if (this.Bcc) {
                froalaheight -= 22;
                this.IsShowBccBox = true;
            }


            this.IsHideBccCcLinkArea = true;
            this.froalaEditorSetting.IsDisableEdit = true;
            this.froalaEditorSetting.Height = froalaheight;

            this._htmlEditorService.getSentMessageHtmlBody(this.SelectedInternalDocument.SelectedCommunicationLogViewMode.CurrentEntityPm.DocumentId, SessionInfo.LoggedUserTenant).subscribe(res => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult);

                        this.ReloadFroalaEditor();
                    }

                    this._communicationAttachmentExtendedPMService.getCommunicationAttachmentsByCommunicationLogId(this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Id, SessionInfo.LoggedUserTenant).subscribe(res => {

                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                this.CommunicationAttachmentPMs = myResult
                                var attachmentsLogList = new Array<AttachmentsList>();
                                if (this.CommunicationAttachmentPMs && this.CommunicationAttachmentPMs.length > 0) {
                                    var logAttachments = this.CommunicationAttachmentPMs.filter(a => a.CommunicationLogId == this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Id);
                                    // IsViewGeneralAttachment

                                    var numberOfAttachment: number = logAttachments.length;
                                    var count: number = 0;
                                    if (this.SelectedInternalDocument.IsViewGeneralAttachment) {

                                        logAttachments.forEach((attachment) => {
                                            this._documentExtendedService.GetDocumentById(attachment.DocumentId, attachment.Tenant).subscribe(res => {
                                                var pmResponse: ServiceResponse = res;
                                                count += 1;
                                                if (!pmResponse.HasError) {
                                                    var myResult = pmResponse.Result;
                                                    if (myResult) {
                                                        var document: any = myResult;

                                                        var fileName: string = !AppTool.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName : document.FileName;
                                                        var attachmentlog = this.GetAttachmentList(fileName, document.Id, document.FileSize, attachment.Tenant);
                                                        if (attachmentlog) {
                                                            attachmentsLogList.push(attachmentlog);
                                                        }


                                                        if (count == numberOfAttachment) {
                                                            this.AttachmentsLists = attachmentsLogList;
                                                            if (attachmentsLogList) {
                                                                this.BliudAttachmentList(attachmentsLogList);

                                                            }
                                                        }
                                                    }

                                                }

                                                if (count == numberOfAttachment) {
                                                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                                }



                                            });
                                        });

                                    }
                                    else {
                                        //start region

                                        var documentOutCopies = new Array<DocumentOutCopyPM>();

                                        if (this.SelectedInternalDocument.DocsOutTabComponent) {
                                            this.documentInPMs = this.SelectedInternalDocument.DocsOutTabComponent.DocumentInPMs;
                                        }

                                        if (this.SelectedInternalDocument.DocsOutItemsList) {
                                            this.SelectedInternalDocument.DocsOutItemsList.forEach((dataView) => {
                                                if (dataView.CurrentDocument != null) {
                                                    if (dataView.CurrentDocument.DocumentOutCopies != null) {

                                                        dataView.CurrentDocument.DocumentOutCopies.forEach((copy) => {
                                                            documentOutCopies.push(copy);
                                                        });

                                                    }
                                                }
                                            });
                                        }



                                        logAttachments.forEach((attachment) => {
                                            if (attachment.CommunicationLogId == this.SelectedInternalDocument.SelectedCommunicationLogViewMode.Id) {

                                                var copy = documentOutCopies.filter(d => d.Id == attachment.DocumentId)[0];

                                                var attachmentlog = null;
                                                if (copy != null) {
                                                    if (copy.DocoumentTypeCopyName != this.SelectedInternalDocument.CurrentDocument.DocumentTypeName) {
                                                        attachmentlog = this.GetAttachmentList(copy.DocumentTypeCopyNameWithDocumentTypeName, attachment.DocumentId, copy.FileSize, copy.Tenant);
                                                    }
                                                    else {
                                                        attachmentlog = this.GetAttachmentList(copy.DocoumentTypeCopyName, attachment.DocumentId, copy.FileSize, copy.Tenant);
                                                    }
                                                }
                                                else {
                                                    var documentIn = null;
                                                    if (this.documentInPMs) {
                                                        documentIn = this.documentInPMs.filter(d => d.DocumentId == attachment.DocumentId)[0];
                                                    }

                                                    if (documentIn != null) {
                                                        attachmentlog = this.GetAttachmentList(documentIn.DocumentTypeName, attachment.DocumentId, documentIn.FileSize, documentIn.Tenant);
                                                    }
                                                    else if (this.SelectedInternalDocument.DocsOutTabComponent && this.SelectedInternalDocument.DocsOutTabComponent.DocumentOuts) {

                                                        var documentout = this.SelectedInternalDocument.DocsOutTabComponent.DocumentOuts.filter(d => d.Id == attachment.DocumentId)[0];
                                                        if (documentout != null) {
                                                            attachmentlog = this.GetAttachmentList(documentout.DocumentTypeName, attachment.DocumentId, documentout.FileSize, documentout.Tenant);
                                                        }

                                                    }

                                                }

                                                if (attachmentlog) {
                                                    attachmentsLogList.push(attachmentlog);
                                                }

                                            }

                                        });

                                        this.AttachmentsLists = attachmentsLogList;
                                        if (attachmentsLogList) {
                                            this.BliudAttachmentList(attachmentsLogList);
                                        }

                                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                    //end region 
                                    }

                                }
                                else {
                                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                }

                            }

                        }

                        else {
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }


                    });



                }
                else {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                        this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                    }
                }


                this.ShowBusyIndicator = false;
            });


            //getSentMessageHtmlBody

        }

    }
    //  (copy.DocoumentTypeCopyName, attachment.DocumentId, copy.FileSize, SelectedInternalDocument.Tenant, false);
    GetAttachmentList(name: string, documentId: string, fileSize: any, tenant: number) {

        var attachmentlog = new AttachmentsList();
        attachmentlog.Tenant = tenant;
        attachmentlog.FileSize = fileSize;
        attachmentlog.ShowRemoveLink = false;
        attachmentlog.Id = documentId;
        attachmentlog.DocumentTypeCopyNameWithDocumentTypeName = name;
        return attachmentlog;
    }

    LoadHtmlTemplateData(templateId: string) {


        if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedDocumentTypeTemplateViewModel.HtmlData);
            this.From = this.SelectedDocumentTypeTemplateViewModel.From;
            this.ReplyTo = this.SelectedDocumentTypeTemplateViewModel.ReplyTo;
            this.Subject = this.SelectedDocumentTypeTemplateViewModel.TemplateSubject;
            this.Cc = this.SelectedDocumentTypeTemplateViewModel.TemplateCc;

            if (!AppTool.IsNullOrEmpty(this.Cc)) this.AddCcClick();

            this.ReloadFroalaEditor();
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            var docoutId = this.DocumentOutId;
            if (templateId) docoutId = "";

            if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.Entity) {
                this.From = !AppTool.IsNullOrEmpty(this.SelectedDocumentTypeTemplateViewModel.Entity.From) ? this.SelectedDocumentTypeTemplateViewModel.Entity.From : "";
                this.ReplyTo = !AppTool.IsNullOrEmpty(this.SelectedDocumentTypeTemplateViewModel.Entity.ReplyTo) ? this.SelectedDocumentTypeTemplateViewModel.Entity.ReplyTo : "";
                this.Cc = !AppTool.IsNullOrEmpty(this.SelectedDocumentTypeTemplateViewModel.Entity.CC) ? this.SelectedDocumentTypeTemplateViewModel.Entity.CC : "";

            }



            this._htmlEditorService.getEditorHtmlData(docoutId, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, true, templateId, "", "", this.From, this.ReplyTo, this.Cc).subscribe(res => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {

                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                        if (this.SelectedDocumentTypeTemplateViewModel != null) {
                            this.SelectedDocumentTypeTemplateViewModel.HtmlData = myResult.Htmlstring;
                            this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                            this.SelectedDocumentTypeTemplateViewModel.From = !AppTool.IsNullOrEmpty(myResult.From) ? myResult.From : "";
                            this.SelectedDocumentTypeTemplateViewModel.ReplyTo = !AppTool.IsNullOrEmpty(myResult.ReplyTo) ? myResult.ReplyTo : "";
                            this.SelectedDocumentTypeTemplateViewModel.TemplateCc = !AppTool.IsNullOrEmpty(myResult.Cc) ? myResult.Cc : "";

                            if (!AppTool.IsNullOrEmpty(myResult.Subject)) {
                                this.SelectedDocumentTypeTemplateViewModel.TemplateSubject = myResult.Subject;
                            }
                            else {
                                this.SelectedDocumentTypeTemplateViewModel.TemplateSubject = this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject != null ? this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject : this.SelectedInternalDocument.CurrentDocument.DocumentTypeName;
                            }


                        }

                        this.From = !AppTool.IsNullOrEmpty(myResult.From) ? myResult.From : "";
                        this.ReplyTo = !AppTool.IsNullOrEmpty(myResult.ReplyTo) ? myResult.ReplyTo : "";
                        this.Cc = !AppTool.IsNullOrEmpty(myResult.Cc) ? myResult.Cc : "";
                        if (!AppTool.IsNullOrEmpty(this.Cc)) this.AddCcClick();

                        if (!AppTool.IsNullOrEmpty(myResult.Subject)) {
                            this.Subject = myResult.Subject;
                        }
                        else {
                            this.Subject = this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject != null ? this.SelectedInternalDocument.CurrentDocument.DocumentTypeSubject : this.SelectedInternalDocument.CurrentDocument.DocumentTypeName;
                        }



                        this.ReloadFroalaEditor();

                    }

                }
                else {
                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                        this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                    }
                }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.ShowBusyIndicator = false;
            });
        }

    }

    LoadDocumentTypeTemplates(selectId: string) {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.SelectId = selectId;
        this.ReportTemplates = new Array<DocumentTypeTemplateViewModel>();
        this.DocumenttypetemplateLists = new Array<DocumentTypeTemplateViewModel>();
        this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.CurrentDocumentType.Id, this.CurrentDocumentType.Tenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var selectId = this.SelectId ? this.SelectId : this.CurrentDocument.EmailTemplateId;
                    myResult.forEach((item) => {

                        if (item.DocumentTypeId == this.DocumentTypeId && item.TemplateType == "M") {
                            if (!item.InActive || item.IsDefault || item.Id == selectId) {
                                this.ReportTemplates.push(new DocumentTypeTemplateViewModel(item));
                            }
                            this.DocumenttypetemplateLists.push(new DocumentTypeTemplateViewModel(item));
                        }
                    });

                    this.Title = "Templates (" + this.ReportTemplates.length + ")";

                    if (this.ReportTemplates.length > 0) {

                        if (selectId) {
                            this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(r => r.Id == selectId)[0];

                        }

                        if (!this.SelectedDocumentTypeTemplateViewModel) {

                            this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates[0];
                        }
                    }


                    if (this.SelectedDocumentTypeTemplateViewModel != null) {
                        this.Subject = this.SelectedDocumentTypeTemplateViewModel.Subject;
                        this.CurrentDocument.EmailTemplateId = this.SelectedDocumentTypeTemplateViewModel.Id;
                        this.LoadHtmlTemplateData(this.SelectedDocumentTypeTemplateViewModel.Id);



                    }



                }

            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }





        });
    }

    OnSelectTemplateChange(selectedItem: DocumentTypeTemplateViewModel) {


        if (selectedItem != this.SelectedDocumentTypeTemplateViewModel) {
            this.SelectedDocumentTypeTemplateViewModel = selectedItem;
            this.CurrentDocument.EmailTemplateId = selectedItem.Id;
            this.LoadHtmlTemplateData(selectedItem.Id);
        }



    }

    ShowHideTemplateList() {
        if (this.IsShowTemplateList) {
            this.IsShowTemplateList = false;
            this.AttrTitleShowTemplateList = "Expand";
        }
        else {
            this.IsShowTemplateList = true;
            this.AttrTitleShowTemplateList = "Hide";
        }


        var element = document.getElementById(this.AttachmentListId);

        this.ComputeAttachmentListWidth();
        if (this.AttachmentsLists.length > 4) {
            element.setAttribute("style", "height:60px;margin-left:5px;overflow-y:scroll;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
        else {
            element.setAttribute("style", "height:auto;margin-left:5px;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }


    }

    public CheckboxClick() {


        if (this.IsCheckedInActive) {
            this.IsCheckedInActive = false;
            this.RefreshDocumentTemplateList(false);
        }
        else {
            this.IsCheckedInActive = true;
            this.RefreshDocumentTemplateList(true);
        }


    }


    RefreshDocumentTemplateList(isactive: boolean) {
        //ReportTemplates
        // DocumenttypetemplateLists
        if (isactive) {
            this.ReportTemplates = this.DocumenttypetemplateLists;
        }
        else {

            this.ReportTemplates = this.DocumenttypetemplateLists.filter(d => d.InActive == false || d.IsDefault == true || (this.SelectedDocumentTypeTemplateViewModel && this.SelectedDocumentTypeTemplateViewModel.Id == d.Id));


        }
        this.Title = "Templates (" + this.ReportTemplates.length + ")";

    }

    CheckIsValidEmails(mailsList: string) {

        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;



        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach((item) => {

                if (item) {
                    if (!EMAIL_REGEXP1.test(item)) {
                        IsOk = false;
                        return;
                    } else if (!EMAIL_REGEXP2.test(item)) {
                        IsOk = false;
                        return;
                    }
                }

            });
        }
        if (IsOk) {


        }


        return IsOk;
    }


    SendDocumentHtml() {

        ServiceLocator.SendTotangoUserActivity(this.ObjecttableName, "SendDocByEmail");

        var filter = new SendHtmlDocumentFilter();

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Sending...");
        filter.HtmlString = this.froalaEditorSetting.froalaEditorComponent.getHtml();

        var html = filter.HtmlString;

        var start = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body>";
        var end = "</body></html>";


        filter.HtmlString = start + filter.HtmlString + end;


        filter.InternalDocumentId = this.DocumentOutId;
        filter.ToEmail = this.ToEmail;
        filter.Tenant = SessionInfo.LoggedUserTenant;
        filter.UserId = SessionInfo.LoggedUserId;
        filter.ObjectTableId = this.ObjectTableId;
        filter.EntityReference = "";
        filter.EntityId = this.EntityId;
        filter.Subject = this.Subject;
        filter.Cc = this.Cc;
        filter.Bcc = this.Bcc;
        filter.Attachments = "";
        filter.ExportQuotationsToIntegratedSystem = this.SelectedInternalDocument.ExportQuotationsToIntegratedSystem;
        filter.ObjectTableName = this.ObjecttableName;
        if (this.SelectedInternalDocument.IsCrm) {
            filter.EventTypeCode = this.SelectedInternalDocument.EventTypeCode;
            filter.CustomerId = this.SelectedInternalDocument.EntityPM ? this.SelectedInternalDocument.EntityPM.CustomerId : "";
            filter.DocumentTypeCode = this.SelectedInternalDocument.DocumentTypePM.Code;
            filter.IsCRM = this.SelectedInternalDocument.IsCrm;

            if (this.SelectedInternalDocument.ObjectTableName == "Customer") {
                filter.CustomerId = this.SelectedInternalDocument.EntityPM ? this.SelectedInternalDocument.EntityPM.Id : "";
                filter.EntityId = "";
            }

            var div = document.createElement("div");
            div.innerHTML = html;
            document.body.appendChild(div);
            filter.HtmlPlainString = GetPlainTextFromHtml(div);
            document.body.removeChild(div);
        }



        filter.From = !AppTool.IsNullOrEmpty(this.From) ? this.From : "";
        filter.ReplyTo = !AppTool.IsNullOrEmpty(this.ReplyTo) ? this.ReplyTo : "";

        if (!filter.ToEmail) {
            this.ShowMessage("Please specify at least one recepient", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }


        if (!this.CheckIsValidEmails(filter.ToEmail)) {

            this.ShowMessage("Some of To e- mails are Invalid", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }


        if (filter.Cc != null && !this.CheckIsValidEmails(filter.Cc)) {
            this.ShowMessage("Some of Cc e-mails are Invalid", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }

        if (filter.Bcc != null && !this.CheckIsValidEmails(filter.Bcc)) {
            this.ShowMessage("Some of Bcc e-mails are Invalid", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }

        var Byte = 1024;
        var totalsize = 0;
        this.AttachmentsLists.forEach((item) => {

            filter.Attachments += item.Id + ",";

            if (item.FileSize != null) {
                totalsize += item.FileSize / (Byte * Byte);
            }
        });


        if (totalsize > 15) {
            this.ShowMessage("The maximum size of documents you can attach is 15 MB. Please send the documents in separated emails", "Attachment Limit");
            //this.ShowMessage("The file you are trying to send exceeds the 15 MB attachment limit.", "Attachment Limit");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }


        this._htmlEditorService.sendDocumentHtml(filter).subscribe(res => {
            var response: ServiceResponse = res;

            if (!response.HasError) {

                this._communicationLogExtendedPMService.getCommunicationLogPMsByEntityIdAndDocumentOutId(this.EntityId, this.CurrentDocument.Id, this.CurrentDocument.Tenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;

                        this.CommunicationLogs = new Array<CommunicationLogPMViewModel>();
                        myResult.forEach((item) => {
                            this.CommunicationLogs.push(new CommunicationLogPMViewModel(item));
                        });
                        this.SelectedInternalDocument.CommunicationLogObsList = this.CommunicationLogs.filter(d => d.CurrentEntityPm.DocumentOutId == this.SelectedInternalDocument.CurrentDocument.Id);

                        this._documentOutPMService.getSingleDocumentOutPM(this.SelectedInternalDocument.CurrentDocument.Id, this.SelectedInternalDocument.CurrentDocument.Tenant).subscribe(res => {

                            var pmResponse: ServiceResponse = res;
                            if (!pmResponse.HasError) {
                                var updated = pmResponse.Result;
                                if (updated) {
                                    this.SelectedInternalDocument.CurrentDocument = updated;
                                    this.SelectedInternalDocument.Issued = true;

                                    this.SelectedInternalDocument.IssuedByUserName = this.SelectedInternalDocument.CurrentDocument.IssuedByUserName = updated.IssuedByUserName;
                                    this.SelectedInternalDocument.IssuedDate = this.SelectedInternalDocument.CurrentDocument.IssuedDate = updated.IssuedDate;
                                }
                            }

                            if (this.SelectedInternalDocument.CommunicationLogObsList && this.SelectedInternalDocument.CommunicationLogObsList.length > 0) {

                                this.SelectedInternalDocument.HasTree = true;
                                this.SelectedInternalDocument.SetCommunicationLogListHeight();
                            }


                            if (!AppTool.IsNullOrEmpty(this.EventRefreshName)) {
                                this.CurrentSession.FireEvent(this.EventRefreshName);
                            }


                            this.CurrentSession.CurrentWindow.Close("SendEnd");

                        });
                    }
                    else {


                        this.CloseButtonClicked();

                    }


                });
            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                    this.ShowMessage(response.ErrorsArray[0], "Logitude Message");
                }
            }


        });




    }

    CloseButtonClicked() {

        //this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe(res => {
        //});


        this.CurrentSession.CloseCurrentWindow();
    }




    ShowAttachDocsOut() {
        this.IsEnableLinkDocOout = false;
        this.DocTypeLists = [];
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = this.CurrentDocument.Tenant;

        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.DocTypeLists = myResult;
                this.AttachDocsOut();
            }

        });



    }





    currentDocTypeList: DocumentTypeList;
    DocTypeLists: DocumentTypeList[];
    IsShowAtachmentDocOut: boolean;
    AttachDocsOut() {
        this.IsShowAtachmentDocOut = false;
        this.DocumentCopiesList = new Array<DocumentOutCopyViewModel>();

        //if (this.SelectedInternalDocument.PageRequestSendComponent == "DocOut") {
        //    if (this.SelectedInternalDocument.DocsOutItemsList) {
        //        var DocumentviewmodelList = this.SelectedInternalDocument.DocsOutItemsList;
        //        this.SelectedInternalDocument.DocsOutItemsList.forEach((item) => {
        //            if (item.CurrentDocument != null && item.CurrentDocument.DocumentOutCopies) {
        //                item.CurrentDocument.DocumentOutCopies.forEach((copy) => {
        //                    this.currentDocTypeList = this.DocTypeLists.filter(d=> d.Id == item.CurrentDocument.DocumentTypeId)[0];

        //                    if (this.currentDocTypeList) {

        //                        if (!(this.currentDocTypeList.IsDocumentOneTimePrintLimited == true && this.currentDocTypeList.LimitedPrintCopyId == copy.DocumentTypeCopyId && !AppTool.IsNullOrEmpty(copy.LastPrintedByUserId))) {
        //                            this.DocumentCopiesList.push(new DocumentOutCopyViewModel(copy));
        //                        }
        //                    }

        //                });
        //            }

        //        });
        //    }
        //    this.ViewAttachDocsOut(this.DocumentCopiesList);

        //}


        //else {
        this.DocumentOutLists = new Array<DocumentOutPM>();
        var childEntityId: string = this.ChildEntityId;
        var mychildObjectTable = window.ObjectTables.filter(d => d.Id == this.ChildObjectTableId)[0];
        var childObjectTableName: string = "";
        if (mychildObjectTable) {
            if (mychildObjectTable.Name == "ARInvoice" || mychildObjectTable.Name == "APInvoice") childEntityId = "";
        }

        var table: any = "";

        if (AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            childEntityId = "";
        }

        this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, childEntityId, this.ObjectTableId, SessionInfo.LoggedUserTenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.DocumentOutLists = myResult;
                    this.DocumentOutLists.forEach((documentout) => {
                        if (documentout.DocumentOutCopies) {
                            documentout.DocumentOutCopies.forEach((copy) => {
                                this.currentDocTypeList = this.DocTypeLists.filter(d => d.Id == documentout.DocumentTypeId)[0];

                                if (this.currentDocTypeList) {

                                    if (!(this.currentDocTypeList.IsDocumentOneTimePrintLimited == true && this.currentDocTypeList.LimitedPrintCopyId == copy.DocumentTypeCopyId && !AppTool.IsNullOrEmpty(copy.LastPrintedByUserId))) {
                                        this.DocumentCopiesList.push(new DocumentOutCopyViewModel(copy));
                                    }
                                }

                            });
                        }

                    });


                }

            }

            this.ViewAttachDocsOut(this.DocumentCopiesList);

        });
        //}



    }

    ViewAttachDocsOut(items: any) {
        if (items.length > 0) {


            var logWindow = new LogitudeWindow();
            logWindow.Title = "Attach Internal Document";
            logWindow.Width = 800;
            logWindow.Height = 500;
            logWindow.DataContext = this;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachDocsOutComponent");

        }
        else {
            this.ShowMessage("No Internal documents found");
            this.IsEnableLinkDocOout = true;

        }

    }




    IsCloseAttachmentUploader: boolean;
    ShowAttachExternal() {
        this.IsCloseAttachmentUploader = false;
        this.IsEnableLinkAttachExternal = false;

        var windowArgs: any = {};
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.RequsetPageName = "SendControl";
        windowArgs.TiggerViewModel = this;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 360;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.IsEnableLinkAttachExternal = true;
        });
    }



    OnUploadComplete(uploader: any) {

        this.IsEnableLinkAttachExternal = true;

        if (uploader.IsUploadDone && uploader.CurrentDocument) {

            var item = new AttachmentsList();
            item.Id = uploader.CurrentDocument.DocumentId;
            item.Tenant = uploader.CurrentDocument.Tenant;
            item.FileSize = uploader.CurrentDocument.FileSize;
            item.DocumentTypeCopyNameWithDocumentTypeName = uploader.CurrentDocument.DocumentTypeName;
            item.FileExtension = uploader.CurrentDocument.FileExtension;
            item.ShowRemoveLink = true;
            var attachmentsLists = this.AttachmentsLists;
            if (!attachmentsLists) attachmentsLists = new Array<AttachmentsList>();
            attachmentsLists = attachmentsLists.filter(d => d.Id != item.Id);
            attachmentsLists.push(item);
            this.BliudAttachmentList(attachmentsLists);

        }
    }


    IsCloseAttachmentDocsIn: boolean;
    ShowAttachDocsIn() {

        this.IsEnableLinkDocIn = false;
        var childEntityId: string = this.ChildEntityId;
        var mychildObjectTable = window.ObjectTables.filter(d => d.Id == this.ChildObjectTableId)[0];
        var childObjectTableName: string = "";
        if (mychildObjectTable) {
            if (mychildObjectTable.Name == "ARInvoice" || mychildObjectTable.Name == "APInvoice") childEntityId = "";
        }

        this._documentsFilingExtendedPMService.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(this.EntityId, childEntityId, this.ObjectTableId, "I", this.CurrentDocumentType.Tenant, true).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentInPMs = myResult;
                    if (this.documentInPMs.length > 0) {

                        this.IsCloseAttachmentDocsIn = false;
                        this.OnCloseAttachmentDocsInEvent.subscribe(($event: any) => {

                            if (!this.IsCloseAttachmentDocsIn && $event) {
                                this.IsCloseAttachmentDocsIn = true;
                                this.IsEnableLinkDocIn = true;
                                this.BliudAttachmentList($event);

                                if (this.AttachmentsLists.length != null && this.AttachmentsLists.length > 0) {
                                    this.IsShowAttachmentList = true;

                                }
                                this.ReloadFroalaEditor();

                            }


                        });


                        var windowArgs: any = {};
                        windowArgs.DocumentsFilingList = this.documentInPMs;
                        windowArgs.OnCloseAttachmentDocsInEvent = this.OnCloseAttachmentDocsInEvent;

                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 800;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = "Attach Docs In";
                        logitudeWindow.WindowArgs = windowArgs;
                        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentDocsInComponent");
                        logitudeWindow.WindowClosed.subscribe(($event: any) => {
                            this.IsEnableLinkDocIn = true;
                        });


                    }
                    else {

                        this.ShowMessage("No Docs In found");
                        this.IsEnableLinkDocIn = true;
                    }
                }

            }
            else this.IsEnableLinkDocIn = true;

        });




    }

    IsCloseAttachDocsShareWithAgents: boolean;
    ShowDocsSharedWithAgentsAttachment() {

        if (this.IsEnableLinkDocsSharedWithAgents) {
            this.IsEnableLinkDocsSharedWithAgents = false;

            this.IsCloseAttachDocsShareWithAgents = false;
            this.OnCloseSharedWithAgentsEvent.subscribe(($event: any) => {

                if (!this.IsCloseAttachDocsShareWithAgents && $event) {
                    this.IsCloseAttachDocsShareWithAgents = true;
                    this.IsEnableLinkDocsSharedWithAgents = true;
                    this.BliudAttachmentList($event);

                    if (this.AttachmentsLists.length != null && this.AttachmentsLists.length > 0) {
                        this.IsShowAttachmentList = true;

                    }
                    this.ReloadFroalaEditor();

                }


            });

            var windowArgs: any = {};
            windowArgs.OnCloseSharedWithAgentsEvent = this.OnCloseSharedWithAgentsEvent;
            windowArgs.Mode = "Attachment";
            windowArgs.EntityPM = this.SelectedInternalDocument != null ? this.SelectedInternalDocument.EntityPM : null;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = "Attach Documents Shared with Agents";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.IsEnableLinkDocsSharedWithAgents = true;
            });


        }


    }

    BliudAttachmentList(attachmentsLists: any, removeHeightList: boolean = true, isChangeHeightFroalaEditor: boolean = true) {
        if (removeHeightList) {
            this.RemoveHeightAttachmentsListsFromWindow(this);
        }
        if (attachmentsLists.length > 0) {

            attachmentsLists.forEach((item) => {
                this.AttachmentsLists = this.AttachmentsLists.filter(d => d.Id != item.Id);
                this.AttachmentsLists.push(item);
            });

            this.AttachmentsLists = this.AttachmentsLists.reverse();
            this.CreateAttachmentList(isChangeHeightFroalaEditor);

        }




    }

    public AddBccClick() {

        if (!this.IsShowBccBox) {

            this.IsShowBccBox = true;
            if (this.IsShowCcBox) {
                this.IsHideBccCcLinkArea = true;
                this.froalaEditorSetting.Height -= 11;
            }
            else {
                this.froalaEditorSetting.Height -= 26;
            }

            this.ReloadFroalaEditor();
        }
    }
    public AddCcClick() {

        if (!this.IsShowCcBox) {
            this.IsShowCcBox = true;

            if (this.IsShowBccBox) {
                this.IsHideBccCcLinkArea = true;
                this.froalaEditorSetting.Height -= 11;
            } else {
                this.froalaEditorSetting.Height -= 26;
            }

            this.ReloadFroalaEditor();
        }
    }

    RemoveHeightCcAndBCcFromWindow() {
        if (this.IsShowBccBox && this.IsShowCcBox) this.froalaEditorSetting.Height += 37;
        else if (this.IsShowCcBox) this.froalaEditorSetting.Height += 26;
        else if (this.IsShowBccBox) this.froalaEditorSetting.Height += 26;

        this.IsShowBccBox = false;
        this.IsShowCcBox = false;
        this.ReloadFroalaEditor();
    }
    RemoveHeightAttachmentsListsFromWindow(sendControl: SendDocumentComponent) {

        if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 2 && sendControl.AttachmentsLists.length < 5) {
            sendControl.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 22;
        }

        else if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 4) {
            this.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 45;
        }
    }

    public CreateAttachmentList(isChangeHeightFroalaEditor: boolean = true) {


        if (this.AttachmentsLists.length > 2 && this.AttachmentsLists.length < 5) {
            if (this.IsSendEditMode) this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 22;

            else this.froalaEditorSetting.Height = this.froalaEditorSetting.Height - 35;

        }

        else if (this.AttachmentsLists.length > 4) {
            if (this.IsSendEditMode) this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 45;

            else this.froalaEditorSetting.Height = this.SelectedInternalDocument.WindowHeight - 70;


        }

        if (isChangeHeightFroalaEditor) this.ReloadFroalaEditor();


        var element = document.getElementById(this.AttachmentListId);

        element.innerHTML = "";
        this.table = document.createElement("table");
        this.tbdy = document.createElement('tbody');
        this.tbdy2 = document.createElement('tbody');
        this.tr = document.createElement("tr");


        this.tr.setAttribute("style", "height:22px;vertical-align:top;");
        this.td = document.createElement("td");
        this.div = document.createElement("div"); //Create left div
        this.div.setAttribute("style", "width:auto");

        this.table2 = document.createElement("table");
        this.tr2 = document.createElement("tr");

        this.order = 0;
        this.Count = 0;
        this.AttachmentsLists.forEach((item) => {

            this.AddAttachment(item);
        });

        var element = document.getElementById(this.AttachmentListId);

        this.ComputeAttachmentListWidth();
        if (this.AttachmentsLists.length > 4) {
            element.setAttribute("style", "height:65px;margin-left:5px;vertical-align:top;overflow-y:scroll;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
        else {
            element.setAttribute("style", "height:auto;margin-left:5px;vertical-align:top;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        }
        this.table2.appendChild(this.tbdy2);

        this.div.appendChild(this.table2);


        this.td.appendChild(this.div);

        this.tr.appendChild(this.td);

        this.tbdy.appendChild(this.tr);

        this.table.appendChild(this.tbdy);


        element.appendChild(this.table);




        //  
    }
    order: number = 0;
    public AddAttachment(item: AttachmentsList) {


        var att = new AttachmentDocment(item.DocumentTypeCopyNameWithDocumentTypeName, item.FileSize, item.Id, this.order++);


        this._documentExtendedService.GetDocumentById(item.Id, item.Tenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentPM = myResult;
                    att.FileSize = this.documentPM.FileSize;
                    att.FileName = !AppTool.IsNullOrEmpty(this.documentPM.CalculatedFileName) ? this.documentPM.CalculatedFileName : item.DocumentTypeCopyNameWithDocumentTypeName;
                    this.CreateAttachment(att, this.documentPM, item.ShowRemoveLink, this);
                }

            }

        });

    }

    private CreateAttachment(att: AttachmentDocment, documentPM: DocumentPM, showRemoveLink: boolean, SendControl: SendDocumentComponent) {


        this.Count += 1;

        var name = att.FileName;

        name += "." + documentPM.Extension

        if (!SendControl.IsSendEditMode) name += " , ";


        var cellText = document.createTextNode(name);

        var div = document.createElement("div");
        div.setAttribute("style", "width:auto;text-align:left;font-weight:bold;height:22px;");

        div.appendChild(cellText);
        if (SendControl.IsSendEditMode) {
            var a = document.createElement('a');
            var linkText = document.createTextNode("[Remove],");
            a.appendChild(linkText);
            a.title = " [Remove]";
            a.id = att.DocumentId;
            a.setAttribute("style", "width:auto;margin-left:5px");
            a.onclick = function () {

                SendControl.RemoveHeightAttachmentsListsFromWindow(SendControl);

                SendControl.AttachmentsLists = SendControl.AttachmentsLists.filter(d => d.Id != a.id);
                if (SendControl.AttachmentsLists != null && SendControl.AttachmentsLists.length > 0) {
                    htmlComponentProparitiesTrue(a);

                    SendControl.CreateAttachmentList();
                }
                else {

                    var element = document.getElementById(SendControl.AttachmentListId);
                    element.innerHTML = "";
                    htmlComponentProparitiesFalse(a);

                    //this.IsShowAttachmentList = false;
                }

            };
            div.appendChild(a);
        }


        if (this.CountTd == 2) {
            var td = document.createElement("td");
            td.setAttribute("style", "width:auto;text-align:left;");

            td.appendChild(div);
            var tdspace = document.createElement("td");
            tdspace.setAttribute("style", "width:10px;text-align:left;");

            this.tr2.appendChild(tdspace);
            this.tr2.appendChild(td);

            this.tbdy2.appendChild(this.tr2);

            this.tr2 = document.createElement("tr");
            this.tr2.setAttribute("style", "height:22px;vertical-align:top;");

            this.CountTd = 1;
        }
        else {
            var td = document.createElement("td");
            td.setAttribute("style", "width:30px;text-align:left;");
            td.appendChild(div);
            this.tr2.appendChild(td);
            if (this.Count == this.AttachmentsLists.length) {

                this.tbdy2.appendChild(this.tr2);
                this.tr2 = document.createElement("tr");
                this.tr2.setAttribute("style", "height:22px;vertical-align:top;");

                this.CountTd = 1;
            }
            else {
                this.CountTd = this.CountTd + 1;
            }
        }
        this.IsShowAttachmentList = true;

    }

    ShowHtmlDocumentPreview(item: DocumentTypeTemplateViewModel) {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Send";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = item.Tenant;
        windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.EntityId = this.EntityId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;

        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;

        var logWindow = new LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Html Template";


        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {

            this.RefreshTemplateId = $event;
            if (this.RefreshTemplateId) {
                if (!this.SelectedDocumentTypeTemplateViewModel) this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];

                if (this.SelectedDocumentTypeTemplateViewModel) {
                    if (this.SelectedDocumentTypeTemplateViewModel.Id != this.RefreshTemplateId) this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];
                }

                if (this.SelectedDocumentTypeTemplateViewModel) {
                    var documentTemplate: DocumentTypeTemplatePM = this.DocumentTypeTemplatePMLists.filter(d => d.Id == this.SelectedDocumentTypeTemplateViewModel.Id)[0];
                    if (documentTemplate) {

                        this.SelectedDocumentTypeTemplateViewModel.Entity = documentTemplate;
                        this.SelectedDocumentTypeTemplateViewModel.From = documentTemplate.From;
                        this.SelectedDocumentTypeTemplateViewModel.ReplyTo = documentTemplate.ReplyTo;
                        this.SelectedDocumentTypeTemplateViewModel.Subject = documentTemplate.Subject;
                        this.SelectedDocumentTypeTemplateViewModel.TemplateTechnologyCode = documentTemplate.TemplateTechnologyCode;
                    }

                    this.ShowBusyIndicator = true;
                    this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
                    this.LoadHtmlTemplateData(this.RefreshTemplateId);

                } else {

                    this.LoadDocumentTypeTemplates(this.RefreshTemplateId);

                }

            }
        });


    }

    SetTemplateAsDeflut(selectitem: DocumentTypeTemplateViewModel) {

        if (!selectitem.InActive) {

            if (selectitem != null) {

                if (!this.IsTemplateDefualt(selectitem)) {


                    if (!this.IsSend) {
                        if (this.CurrentDocumentType.TemplateFormatCode == "P" && selectitem.TemplateType == "P") {
                            this.CurrentDocumentType.DocumentTypeDefaultReportTemplateId = selectitem.Id;
                            this.CurrentDocumentType.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                        }
                        else if (this.CurrentDocumentType.TemplateFormatCode == "M" && selectitem.TemplateType == "M") {

                            this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId = selectitem.Id;
                            this.CurrentDocumentType.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                        }
                    }
                    else {

                        if (selectitem.TemplateType == "M") {
                            this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId = selectitem.Id;
                            this.CurrentDocumentType.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                        }

                    }


                    this.ReportTemplates.forEach((template) => {
                        if (template.Id != selectitem.Id) {
                            template.IsDefault = false;

                        }
                        else {
                            template.IsDefault = true;

                        }
                    });


                    this._documentTypePMService.putDocumentType(this.CurrentDocumentType).subscribe(res => {
                    });

                }
            }
        }
        else {

            this.ShowMessage("Please note that you can't set an inactive template as default");
        }


        //////End////
    }


    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }


    SetTemplateAsInactive(selectitem: DocumentTypeTemplateViewModel) {

        if (selectitem != null) {


            if (!this.IsTemplateDefualt(selectitem)) {

                if (!selectitem.InActive) {
                    selectitem.InActive = true;
                    selectitem.LableSetactive = "Mark as active";


                }
                else {
                    selectitem.InActive = false;
                    selectitem.LableSetactive = "Mark as inactive";
                }


                var item = this.DocumentTypeTemplatePMLists.filter(d => d.Id == selectitem.Id)[0];


                if (item) {
                    item.InActive = selectitem.InActive;
                    this.UpdateDocumentTypeTemplate(item);
                }

                else {

                    this.documentTypeTemplatePMService.get(selectitem.Id).subscribe(res => {
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                myResult.InActive = selectitem.InActive;
                                this.DocumentTypeTemplatePMLists.push(myResult);
                                this.UpdateDocumentTypeTemplate(myResult);
                            }
                        }
                    });
                }



            }



            else {
                this.ShowMessage("Please note that you can't Inactive the default template, please change the default template first");


            }

        }
    }

    IsTemplateDefualt(selectitem: DocumentTypeTemplateViewModel) {


        var IsDefualt = false;

        if (selectitem.TemplateType == "P") {
            if (selectitem.Id == this.CurrentDocumentType.DocumentTypeDefaultReportTemplateId) {
                IsDefualt = true;
            }
        }
        else
            if (selectitem.TemplateType == "M") {
                if (selectitem.Id == this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId) {
                    IsDefualt = true;
                }
            }

        return IsDefualt;

    }


    UpdateDocumentTypeTemplate(item: any) {

        this.documentTypeTemplatePMService.update(item).subscribe(myResult => {

        });
    }

    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }

    }





    IsCloseSendToContact: boolean = false;

    IsOpenWidnow: boolean = false;
    OpenEmailsBox() {

        if (!this.IsOpenWidnow) {
            this.IsOpenWidnow = true;
            if (!this.PartnersObslist) {

                var childEntityId: string = !AppTool.IsNullOrEmpty(this.SelectedInternalDocument.ChildEntityId) ? this.SelectedInternalDocument.ChildEntityId : "";
                var childObjectTableName: string = !AppTool.IsNullOrEmpty(this.SelectedInternalDocument.ChildObjectTableName) ? this.SelectedInternalDocument.ChildObjectTableName : "";

                this._documentOutPMService.GetEntityPartners(this.EntityId, this.ObjecttableName, childEntityId, childObjectTableName).subscribe(res => {
                    var pmResponse: ServiceResponse = res;

                    if (!pmResponse.HasError) {
                        this.PartnersObslist = [];
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            myResult.forEach((item) => {
                                this.PartnersObslist.push(new EntityPartner(item.PartnerType, item.PartnerId, item.IsUser));
                            });
                        }

                        this.ShowSendToEmail();

                    } else {
                        this.IsOpenWidnow = false;
                    }


                });
            }
            else {

                this.ShowSendToEmail();
            }




        }





    }

    ShowSendToEmail() {


        this.IsCloseSendToContact = false;
        this.OnCloseSendToContactsEvent.subscribe(($event: any) => {
            if (!this.IsCloseSendToContact && $event) {
                this.IsCloseSendToContact = true;

                this.ToEmail = "";
                this.Cc = "";
                this.Bcc = "";

                this.RemoveHeightCcAndBCcFromWindow();
                this.IsHideBccCcLinkArea = false;
                if ($event.ToEmailLists && $event.ToEmailLists.length > 0) {
                    $event.ToEmailLists.forEach((item) => { this.ToEmail += item + ";"; });
                }

                if ($event.CcEmailLists && $event.CcEmailLists.length > 0) {
                    $event.CcEmailLists.forEach((item) => { this.Cc += item + ";"; });
                    if (!this.IsShowCcBox) this.AddCcClick();
                } else this.IsShowCcBox = false;


                if ($event.BccEmailLists && $event.BccEmailLists.length > 0) {
                    $event.BccEmailLists.forEach((item) => { this.Bcc += item + ";"; });
                    if (!this.IsShowBccBox) this.AddBccClick();
                } else this.IsShowBccBox = false;


                if (!this.Bcc && !this.Cc) {
                    this.IsShowBccBox = false;
                    this.IsShowCcBox = false;
                    this.IsHideBccCcLinkArea = false;
                    this.ReloadFroalaEditor();
                }

            }


        });
        var windowArgs: any = {};
        windowArgs.PartnersObslist = this.PartnersObslist;
        windowArgs.ToEmail = this.ToEmail;
        windowArgs.Cc = this.Cc;
        windowArgs.Bcc = this.Bcc;
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Contacts List";
        logWindow.Width = window.innerWidth - 100;
        logWindow.Height = window.innerHeight - 100;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.IsOpenWidnow = false;
        });


    }

    OnWindowClosed() {

        this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();

    }

    ComputeAttachmentListWidth() {

        if (!this.IsShowTemplateList || window.innerWidth > 1200) {

            var width = window.innerWidth - 400;
            if (width > 700) {
                this.AreaAttachmentWidth = "600px"
            }
            else {
                this.AreaAttachmentWidth = (width + "px");
            }

        }
        else {

            var width = window.innerWidth - 700;
            if (width > 700) {
                this.AreaAttachmentWidth = "600px"
            }
            else {
                this.AreaAttachmentWidth = width + "px";
            }


        }

    }


    AddTemplateFromLibrary() {

        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(response => {

            this.IsDisableAddTemplateFromLibrary = true;

            var logWindow = new LogitudeWindow();
            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.ObjectTableId = this.ObjectTableId;
            windowArgs.EntityId = this.EntityId;
            logWindow.Title = TextCodeTranslator.Translate("DocumentTypeTemplate.O.NewHTMLTemplate");

            if (this.CurrentDocumentType) {
                windowArgs.CurrentDocumentType = this.CurrentDocumentType;
            }

            if (this.SelectedInternalDocument) {
                windowArgs.ChildEntityId = this.ChildEntityId;
            }

            windowArgs.ChildObjectTableId = ""; //this.ChildObjectTableId;
            windowArgs.PageRequest = "Send";

            logWindow.Width = 1000;
            logWindow.Height = 550;

            logWindow.WindowArgs = windowArgs;

            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeTemplateFromLibraryComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.IsDisableAddTemplateFromLibrary = false;
            });
        });

    }


}



