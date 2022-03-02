import {Component, OnInit, ChangeDetectorRef, AfterViewInit, EventEmitter, Output}  from '@angular/core';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SendHtmlDocumentFilter} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/SendHtmlDocumentFilter';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {DocumentOutPMService} from '../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocumentOutCopyViewModel} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentOutCopyViewModel';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {DocumentTypeListExtendedService} from '../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AttachmentsList} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {EntityPartner} from '../../../Infrastructure/DataContracts/EntityPartner';
import {HtmlEditorService} from '../../../Common/Services/DocumentServices/HtmlEditorService';
import {DocumentTypeList} from '../../../Common/EntityLists/DocumentTypeList'
import {DocumentsFilingPM} from '../../../Common/EntityPMs/DocumentsFilingPM';
import {DocumentOutPM} from '../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {AttachmentDocment} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/AttachmentDocment';
import {DocumentExtendedService} from '../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import {DocumentPM} from '../../../Common/EntityPMs/DocumentPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
declare var System: any;
declare var window: any;
declare var htmlComponentProparitiesTrue, htmlComponentProparitiesFalse: any;
import {ReportPMService} from '../../../Common/Services/StandardPMs/ReportPMService';
import {ReportsTemplatePMService} from '../../../Common/Services/StandardPMs/ReportsTemplatePMService';
import {ReportsTemplatePMExtendedService} from '../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService';

@Component({
    
    selector: 'GeneralSendControl',
    templateUrl: './GeneralSendComponent.html',
    providers: [ DocumentOutPMService, DocumentTypeListExtendedService, HtmlEditorService, DocumentsFilingExtendedPMService, DocumentExtendedService],

})


export class GeneralSendComponent implements OnInit, AfterViewInit {
    IsShowAddReportTemplateButton: boolean = false;
     IsUserFromReport: boolean = false;
     ChildObjectTableId: string;
     ChildEntityId: string;
     DocumentFilingId: string;
     ChildEntityReference: string;
     EntityReference: string;
     EntityId: string;
     ObjecttableName: string;
     ObjectTableId: string;
     Subject: string = null;
     ToEmail: string = "";
     From: string;
     ReplyTo: string;
     public AttachmentsLists: AttachmentsList[];
 //    DocumentTypeLists: DocumentTypeList[];
     documentInPMs: DocumentsFilingPM[];
     public froalaEditorSetting: FroalaEditorSetting;
     @Output() OnCloseAttachmentDocsInEvent: EventEmitter<any> = new EventEmitter();
    @Output() OnCloseSendToContactsEvent: EventEmitter<any> = new EventEmitter();
    DocumentTypeId: string;
    DocumentOutId: string;
    Bcc: string = "";
    Cc: string = "";
    AttachmentListId: string;
    IsShowCcBox: boolean = false;
    IsShowBccBox: boolean = false;
    IsHideBccCcLinkArea: boolean = false;
    IsEnableLinkAttachExternal: boolean = true;
    IsEnableLinkDocOout: boolean = true;
    IsEnableLinkDocIn: boolean = true;
    public HtmlEditorData: string;
    public AreaAttachmentWidth: string = "600px";
    CurrentDocumentOut: DocumentOutPM;
    CurrentDocumentType: DocumentTypePM;
    public DocumentOutLists: DocumentOutPM[];
    DocumentCopiesList: DocumentOutCopyViewModel[];
    IsShowAttachmentList: boolean;
    WindowHeight: number;
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
    IsShowTemplateList: boolean = false;
    Title: string;
    SelectedTemplate: TemplateClassData;
    ReportTemplates: TemplateClassData[];
    AllReportTemplates: TemplateClassData[];
    IsShowTemplateArea: boolean = false;
    EntityPM: any;



    reportPMService: ReportPMService;
    reportsTemplatePMService: ReportsTemplatePMService;
    reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService;
    ShowInactiveCheckBoxKey: string;
    IsCheckedInActive: boolean = false;
    AttrTitleShowTemplateList: string = "Expand";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor( public _documentOutPMService: DocumentOutPMService, public _documentTypeListExtendedService: DocumentTypeListExtendedService, public _documentExtendedService: DocumentExtendedService, public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService, public _htmlEditorService: HtmlEditorService,  private cd: ChangeDetectorRef) {
     
        this.AttachmentListId = Guid.newGuid();
        this.AttachmentsLists = new Array<AttachmentsList>();

        this.ShowInactiveCheckBoxKey = Guid.newGuid();
    }

    ngOnInit(


    ) {


    }

    ngAfterViewInit(


    ) {

        if (this.AttachmentsLists) {
            this.BliudAttachmentList(this.AttachmentsLists, false);
        }
      
    }


    GetAttachmentList(name: string, documentId: string, fileSize: any, tenant: number) {

        var attachmentlog = new AttachmentsList();
        attachmentlog.Tenant = tenant;
        attachmentlog.FileSize = fileSize;
        attachmentlog.ShowRemoveLink = false;
        attachmentlog.Id = documentId;
        attachmentlog.DocumentTypeCopyNameWithDocumentTypeName = name;

        return attachmentlog;
    }



    CheckIsValidEmails(mailsList: string) {
        var EMAIL_REGEXP = /^[A-Za-z0-9'._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;
        var IsOk = true;

        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach((item) => {

                if (item) {
                    if (!EMAIL_REGEXP.test(item)) {
                        IsOk = false;
                        return;
                    }
                }

            });
        }
        return IsOk;
    }


    SendDocumentHtml() {

        ServiceLocator.SendTotangoUserActivity(this.ObjecttableName, "SendDocByEmail");



        var filter = new SendHtmlDocumentFilter();

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Sending...");
        filter.HtmlString = this.froalaEditorSetting.froalaEditorComponent.getHtml();

        filter.InternalDocumentId = this.DocumentOutId ? this.DocumentOutId : null;
        filter.ExternalDocumentId = this.DocumentFilingId ? this.DocumentFilingId : null;
        filter.ReplyTo = this.ReplyTo ? this.ReplyTo : ""; 
        filter.From = this.From ? this.From : ""; 
        filter.ToEmail = this.ToEmail ? this.ToEmail : ""; 
        filter.Tenant = SessionInfo.LoggedUserTenant;
        filter.UserId = SessionInfo.LoggedUserId;
        filter.ObjectTableId = this.ObjectTableId;
        filter.EntityReference = this.EntityReference ? this.EntityReference : "";  
        filter.EntityId = this.EntityId;
        filter.Subject = this.Subject ? this.Subject : ""; 
        filter.Cc = this.Cc ? this.Cc : ""; 
        filter.Bcc = this.Bcc ? this.Bcc : ""; 
        filter.Attachments = "";
        filter.ChildObjectTableId = this.ChildObjectTableId;
        filter.ChildEntityId = this.ChildEntityId;


        if (!filter.ToEmail) {
            this.ShowMessage("Please specify at least one recepient", "Logitude Message");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }


        if (!this.CheckIsValidEmails(filter.ToEmail)) {

            this.ShowMessage("some of To e- mails are Invalid", "Logitude Message");
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

        if (this.AttachmentsLists) {
            this.AttachmentsLists.forEach((item) => {

                filter.Attachments += item.Id + ",";

                if (item.FileSize != null) {
                    totalsize += item.FileSize / (Byte * Byte);
                }
            });
        }

        if (totalsize > 20) {
            this.ShowMessage("The maximum size of documents you can attach is 20 MB. Please send the documents in separated emails", "Attachment Limit");
         //   this.ShowMessage("The file you are trying to send exceeds the 15 MB attachment limit.", "Attachment Limit");
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            return;
        }


        this._htmlEditorService.sendDocumentHtml(filter).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
                this.CloseButtonClicked();
            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }
        });



    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    ShowAttachDocsOut() {
        this.IsEnableLinkDocOout = false;
        if (!this.DocTypeLists) {

            this._documentTypeListExtendedService.getDocumentTypesListByObjectTableAndTenant(SessionInfo.LoggedUserTenant, this.ObjectTableId).subscribe((res:any) => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.DocTypeLists = myResult;
                        this.AttachDocsOut(this.DocTypeLists);
                    }
                    else this.IsEnableLinkDocOout = true;

                }


            });
        }
        else {
            this.AttachDocsOut(this.DocTypeLists);
        }
    }


    
    currentDocTypeList: DocumentTypeList;
    DocTypeLists: DocumentTypeList[];
    IsShowAtachmentDocOut: boolean;
    AttachDocsOut(DocTypeLists: DocumentTypeList[]) {

        this.IsShowAtachmentDocOut = false;
        this.DocumentCopiesList = new Array<DocumentOutCopyViewModel>();
        
        this.DocumentOutLists = new Array<DocumentOutPM>();
        this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, this.ObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.DocumentOutLists = myResult;
                        this.DocumentOutLists.forEach((documentout) => {
                            if (documentout.DocumentOutCopies) {
                                documentout.DocumentOutCopies.forEach((copy) => {
                                    this.currentDocTypeList = this.DocTypeLists.filter(d=> d.Id == documentout.DocumentTypeId)[0];
                                    if (this.currentDocTypeList && this.currentDocTypeList.IsDocumentOneTimePrintLimited == false && this.currentDocTypeList.LimitedPrintCopyId != copy.DocumentTypeCopyId && copy.LastPrintedByUserId == null) {
                                        this.DocumentCopiesList.push(new DocumentOutCopyViewModel(copy));
                                    }

                                });
                            }

                        });


                    }

                }

                this.ViewAttachDocsOut(this.DocumentCopiesList);

            });


     



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
            attachmentsLists = attachmentsLists.filter(d=> d.Id != item.Id);
            attachmentsLists.push(item);
            this.BliudAttachmentList(attachmentsLists);

        }
    }






    IsCloseAttachmentDocsIn: boolean;
    ShowAttachDocsIn() {

        this.IsEnableLinkDocIn = false;

        this._documentsFilingExtendedPMService.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, this.ObjectTableId, "I", SessionInfo.LoggedUserTenant, true).subscribe((res:any) => {
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





    BliudAttachmentList(attachmentsLists: AttachmentsList[], removeHeightList: boolean = true) {
        if (removeHeightList) {
            this.RemoveHeightAttachmentsListsFromWindow(this);
        }
        if (attachmentsLists.length > 0) {

            attachmentsLists.forEach((item) => {
                var attach = this.AttachmentsLists.filter(d=> d.Id == item.Id)[0]
                if (attach == null) {

                    this.AttachmentsLists.push(item);

                }


            });

            this.AttachmentsLists = this.AttachmentsLists.reverse();



            this.CreateAttachmentList();



        }
        else {



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

    RemoveHeightAttachmentsListsFromWindow(sendControl: GeneralSendComponent) {

        if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 2 && sendControl.AttachmentsLists.length < 5) {
            sendControl.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 22;
        }

        else if (sendControl.AttachmentsLists && sendControl.AttachmentsLists.length > 4) {
            this.froalaEditorSetting.Height = sendControl.froalaEditorSetting.Height += 45;
        }
    }

    public CreateAttachmentList() {


        if (this.AttachmentsLists.length > 2 && this.AttachmentsLists.length < 5) {
             this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 22;

        }

        else if (this.AttachmentsLists.length > 4) {
            this.froalaEditorSetting.Height = this.froalaEditorSetting.Height -= 45;
        }


        this.ReloadFroalaEditor();

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


        this._documentExtendedService.GetDocumentById(item.Id, item.Tenant).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentPM = myResult;
                    att.FileSize = this.documentPM.FileSize;
                    att.FileName = !AppTool.IsNullOrEmpty(this.documentPM.CalculatedFileName) ? this.documentPM.CalculatedFileName : this.documentPM.FileName
                    this.CreateAttachment(att,  this.documentPM, item.ShowRemoveLink, this);
                }

            }

        });

    }

    private CreateAttachment(att: AttachmentDocment, documentPM: DocumentPM, showRemoveLink: boolean, SendControl: GeneralSendComponent) {


        this.Count += 1;

        var name = att.FileName;

        name += "." + documentPM.Extension

        var cellText = document.createTextNode(name);

        var div = document.createElement("div");
        div.setAttribute("style", "width:auto;text-align:left;font-weight:bold;height:22px;");

        div.appendChild(cellText);
  
            var a = document.createElement('a');
            var linkText = document.createTextNode("[Remove],");
            a.appendChild(linkText);
            a.title = " [Remove]";
            a.id = att.DocumentId;
            a.setAttribute("style", "width:auto;margin-left:5px");
            a.onclick = function () {

                SendControl.RemoveHeightAttachmentsListsFromWindow(SendControl);

                SendControl.AttachmentsLists = SendControl.AttachmentsLists.filter(d=> d.Id != a.id);
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


    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }



    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }

    }



    PartnersObslist: EntityPartner[];
    IsCloseSendToContact: boolean = false;

    IsOpenWidnow: boolean = false;
    OpenEmailsBox() {

        if (!this.IsOpenWidnow) {
            this.IsOpenWidnow = true;
            if (!this.PartnersObslist) {
                this._documentOutPMService.GetEntityPartners(this.EntityId, this.ObjecttableName).subscribe((res:any) => {
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
        windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;
        windowArgs.IsUserFromReport = this.IsUserFromReport;
        
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

        if (window.innerWidth > 1200) {
            this.AreaAttachmentWidth = "600px"
        }
    

    }

    IsHideAttachmentLinkArea: boolean = false;
    SetWindowArgs(args: any) {
        this.WindowHeight = args.WindowHeight;
        this.AttachmentsLists = args.Attachments;
        this.ChildObjectTableId = args.ChildObjectTableId;
        this.ChildEntityId = args.ChildEntityId;
        this.ChildEntityReference = args.ChildEntityReference;
        this.EntityReference = args.EntityReference;
        this.IsUserFromReport = args.IsUserFromReport;
        
     
        this.DocumentFilingId = args.DocumentFilingId;
        this.EntityId = args.EntityId;
        this.ObjecttableName = args.ObjecttableName;
        this.ObjectTableId = args.ObjectTableId;
        this.Subject = args.Subject;
        this.ReplyTo = args.ReplyTo;
        this.From = args.From;
        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.PageType = "Send";
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.Height = this.WindowHeight - 203;

        if (this.EntityReference == "StimualReport") {

            if (FeatureLocator.HasFeaturePermession("ReportsTemplate", "REPORTTEMPLATEMESSAGE")) this.IsShowAddReportTemplateButton = true;

            this.IsHideAttachmentLinkArea = true;
            this.EntityReference = "";
            this.PartnersObslist = args.PartnersObslist;

            this.reportsTemplatePMService = new ReportsTemplatePMService();
            this.reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
            this.reportPMService = new ReportPMService();
            this.IsShowTemplateArea = true;

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            this.reportPMService.get(this.EntityId).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    this.EntityPM = pmResponse.Result;
                    this.LoadTemplateLists(null);
                }
            });

        }

    }



  //Template List Area
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

    OnSelectTemplateChange(selectedItem: any) {
        if (selectedItem != this.SelectedTemplate) {
            this.SelectedTemplate = selectedItem;
            this.LoadHtmlTemplateData(selectedItem.Id, selectedItem.CurrentVersion);
        }
    }

    LoadHtmlTemplateData(id: string , version:number) {


        if (this.SelectedTemplate != null && this.SelectedTemplate.IsLoad) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedTemplate.HtmlData);

            this.From =  !AppTool.IsNullOrEmpty(this.SelectedTemplate.From) ? this.SelectedTemplate.From : "";
            this.ReplyTo = !AppTool.IsNullOrEmpty(this.SelectedTemplate.ReplyTo) ? this.SelectedTemplate.ReplyTo : "";
            this.Cc = !AppTool.IsNullOrEmpty(this.SelectedTemplate.Cc) ? this.SelectedTemplate.Cc : "";
            this.Subject = !AppTool.IsNullOrEmpty(this.SelectedTemplate.Subject) ? this.SelectedTemplate.Subject : "";

            if (!AppTool.IsNullOrEmpty(this.Cc)) this.AddCcClick();
            this.ReloadFroalaEditor();
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");


            this.From = !AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.From) ? this.SelectedTemplate.EntityPM.From : "";
            this.ReplyTo = !AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.ReplyTo) ? this.SelectedTemplate.EntityPM.ReplyTo : "";
            this.Cc = !AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.CC) ? this.SelectedTemplate.EntityPM.CC : "";
            this.Subject = !AppTool.IsNullOrEmpty(this.SelectedTemplate.EntityPM.Subject) ? this.SelectedTemplate.EntityPM.Subject : "";


            this.reportsTemplatePMExtendedService.GetReportTemplateEditorHtmlData(id, version, SessionInfo.LoggedUserId, this.Subject, this.From, this.ReplyTo, this.Cc).subscribe((res:any) => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {

                        this.From = this.SelectedTemplate.From = !AppTool.IsNullOrEmpty(myResult.From) ? myResult.From : "";
                        this.ReplyTo = this.SelectedTemplate.ReplyTo =!AppTool.IsNullOrEmpty(myResult.ReplyTo) ? myResult.ReplyTo : "";
                        this.Cc = this.SelectedTemplate.Cc = !AppTool.IsNullOrEmpty(myResult.Cc) ? myResult.Cc : "";
                        if (!AppTool.IsNullOrEmpty(this.Cc)) this.AddCcClick();

                        if (!AppTool.IsNullOrEmpty(myResult.Subject)) {
                            this.Subject = this.SelectedTemplate.Subject = myResult.Subject;
                        }
                        else {
                            this.Subject = this.SelectedTemplate.Subject= this.SelectedTemplate.Description;
                        }

                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                        if (this.SelectedTemplate != null) {
                            this.SelectedTemplate.HtmlData = myResult.Htmlstring;
                            this.SelectedTemplate.IsLoad = true;
                        }

                        this.ReloadFroalaEditor();

                    }

                }

                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
        }

    }

    SelectId: string;

    LoadTemplateLists(selectId: string) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.SelectId = selectId;
        this.ReportTemplates = new Array<TemplateClassData>();
        this.AllReportTemplates = new Array<TemplateClassData>();
        this.reportsTemplatePMExtendedService.GetReportsTemplatePMsByReportId(this.EntityPM.Id , "M").subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach((item) => {

                        if (!item.InActive || item.IsDefault) {
                            this.ReportTemplates.push(new TemplateClassData(item, this.EntityPM));
                        }
                        this.AllReportTemplates.push(new TemplateClassData(item, this.EntityPM));
                       
                    });

                    this.Title = "Templates (" + this.ReportTemplates.length + ")";

                    if (this.ReportTemplates.length > 0) {
                        if (this.SelectId) {
                            this.SelectedTemplate = this.ReportTemplates.filter(r => r.Id == this.SelectId)[0];
                        }
                        else this.SelectedTemplate = this.ReportTemplates.filter(r => r.Id == this.EntityPM.DefaultMessageTemplateId)[0];
                        if (!this.SelectedTemplate) this.SelectedTemplate = this.ReportTemplates[0];
                    }
                    if (this.SelectedTemplate != null) {
                        this.Subject = this.SelectedTemplate.Description;
                        this.LoadHtmlTemplateData(this.SelectedTemplate.Id, this.SelectedTemplate.CurrentVersion);
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

    SetTemplateAsDeflut(selectitem: any) {
        if (!selectitem.InActive) {
            if (selectitem != null) {
                if (!this.IsTemplateDefualt(selectitem)) {
                    this.EntityPM.DefaultMessageTemplateId = selectitem.Id; 
                    this.UpdateReportPM();
                }
            }
        }
        else {
            this.ShowMessage("Please note that you can't set an inactive template as default");
        }
    }

    SetTemplateAsInactive(selectitem: any) {
        if (selectitem != null) {
            if (!this.IsTemplateDefualt(selectitem)) {
                if (!selectitem.InActive) selectitem.InActive = true;
                else selectitem.InActive = false;
                var item = this.AllReportTemplates.filter(d => d.Id == selectitem.Id)[0];
                if (item) {
                    item.InActive = selectitem.InActive;
                    this.UpdateReportTemplatePM(item.EntityPM);
                }
                else {
                    this.reportsTemplatePMService.get(selectitem.Id).subscribe((res:any) => {
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                myResult.InActive = selectitem.InActive;
                                this.UpdateReportTemplatePM(myResult);
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

    RefreshTemplateId: string = "";

    EditTemplate(item: TemplateClassData, isNew: boolean = false) {

        var windowArgs: any = {};
            var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "ReportTemplate";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = item.EntityPM.Tenant;
        windowArgs.ObjectType = "ReportsTemplatePM";
        windowArgs.ReportTemplatePM = item.EntityPM;
        windowArgs.IsNewEntity = isNew;

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
                if (!this.SelectedTemplate) this.SelectedTemplate = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];

                if (this.SelectedTemplate) {
                    if (this.SelectedTemplate.Id != this.RefreshTemplateId) this.SelectedTemplate = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];
                }

                if (this.SelectedTemplate) {
             
                    this.SelectedTemplate.IsLoad = false;
                    this.LoadHtmlTemplateData(this.SelectedTemplate.Id, this.SelectedTemplate.CurrentVersion);

                } else {

                    this.LoadTemplateLists(this.RefreshTemplateId);

                }

            }
        });



    }

    public CheckboxClick() {

        if (this.IsCheckedInActive) {
            this.IsCheckedInActive = false;
            this.RefreshTemplateList(false);
        }
        else {
            this.IsCheckedInActive = true;
            this.RefreshTemplateList(true);
        }


    }

    RefreshTemplateList(isactive: boolean) {

        if (isactive) {
            this.ReportTemplates = this.AllReportTemplates;
        }
        else {
            this.ReportTemplates = this.AllReportTemplates.filter(d => d.InActive == false);
        }

        this.Title = "Templates (" + this.ReportTemplates.length + ")";

    }

    IsTemplateDefualt(selectitem: any) {

        var IsDefualt = false;
        if (selectitem.Id == this.EntityPM.DefaultMessageTemplateId) {
            IsDefualt = true;
        }

        return IsDefualt;

    }

    UpdateReportPM() {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.reportPMService.update(this.EntityPM).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    this.EntityPM = result;
                }
            }
            
        });
    }

    UpdateReportTemplatePM(item:any) {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.reportsTemplatePMService.update(item).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    item = result;
                }
            }

        });
    }


    AddReportTemplateButtonClicked() {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.TemplateType = "M";
        windowArgs.Area = "GeneralSendComponent";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.Title =  "New Message Template";
        logWindow.WindowArgs = windowArgs;

        logWindow.Show("./Report/Components/NewReportsTemplateComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
           
        });

    }


    BuildViewModel(item: any) {
      return  new TemplateClassData(item, this.EntityPM);
    }

}

export class TemplateClassData {

    ReportPM: any;
    EntityPM: any;
    Id: string;
    IsLoad: boolean = false;
    HtmlData: string = "";
    CurrentVersion: number;
    Subject: string = "";
    Cc: string = "";
    ReplyTo: string = "";
    From: string = "";
    


    public get Description() {
        var description = "";
        if (this.EntityPM) {
            description = this.EntityPM.Description;
        }
        return description;
    }
    public set Description(newValue: string) {
        if (this.EntityPM.description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    public get InActive() {
        var inActive = false;
        if (this.EntityPM) {
            inActive = this.EntityPM.InActive;
        }
        return inActive;
    }
    public set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }


    public get LableSetactive() {
        var lableSetactive = "Mark as inactive";
        if (this.EntityPM) {
            if (this.EntityPM.InActive) {
                lableSetactive = "Mark as active";
            }              
        }

        return lableSetactive;
    }


    public get IsDefault() {
        var isDefault = false;;
        if (this.EntityPM) {
            if (this.EntityPM.Id == this.ReportPM.DefaultMessageTemplateId) {
                isDefault = true;
            }
        }

        return isDefault;
    }

    constructor(public entityPM: any , report:any) {
        this.ReportPM = report;
        this.EntityPM = entityPM;
        this.Id = entityPM.Id;
        this.CurrentVersion = this.EntityPM.CurrentVersion;
    }
}

