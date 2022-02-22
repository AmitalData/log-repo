import {Component, OnInit, ChangeDetectorRef, OnDestroy, AfterViewInit}  from '@angular/core';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {DocumentTypeTemplateViewModel} from './DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SendHtmlDocumentFilter} from './DocsOut/Filters/SendHtmlDocumentFilter';
import {FroalaEditorSetting} from './DocsOut/FroalaEditorSetting';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {FroalaEditorComponent} from '../../../../Infrastructure/Components/FroalaEditorComponent/FroalaEditorComponent';
import {DocumentTypeTemplateFilter} from './DocsOut/Filters/DocumentTypeTemplateFilter';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
declare var System: any;
declare var window: any;
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {HtmlEditorService} from '../../../../Common/Services/DocumentServices/HtmlEditorService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ReportsTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService';
declare var insertAtSubject, StringToBase64, querySelection, resultToUnitArray, Base64ToString: any;

@Component({
    
    selector: 'HtmlDocumentPreview',
    templateUrl: './HtmlDocumentPreviewComponent.html',
    providers: [DocumentTypeTemplatePMExtendedService, DocumentTypeTemplatePMService, HtmlEditorService]
})

export class HtmlDocumentPreviewComponent implements OnInit, AfterViewInit {
    public ScreenHeight: any;
    public ScreenWidth: any;
    IsShowButtonSaveAs: boolean;
    public HtmlEditorData: string;
    froalaEditorSetting: FroalaEditorSetting;
    public Subject: string;
    public From: string;
    public ReplyTo: string;
    DocumentTypeCode: string;
    DocumentTemplateFileId: string = Guid.NewRandomString();
    IsShowFromInputBox: boolean;
    IsShowReplyToInputBox: boolean;
    IsShowCCInputBox: boolean;
    IsShowBCCInputBox: boolean;
    IsShowToInputBox: boolean;

    IsShowUploadAndDownloadButtons: boolean = false;
    HtmlTemplateEditor: string;
    public SignatureContext: string[];
    TemplateId: string;
    Tenant: number;
    PageType: string;
    IsShowAreaDataField: boolean = true;
    IsPreviewMode: boolean = false;
    LableSaveButton: string = "Save";
    IsShowButtonCancel: boolean = true;
    ObjectTableId: string;
    objecttypeField: string;
    InSertDataFieldType: string;
    IsShowFroalaEditor: boolean;

    EntityId: string = "";
    ChildEntityId: string = "";
    ChildObjectTableId: string = "";
    IsShowHeaderAndFooterButton: boolean = false;
    SubjectId: string;
    FromId: string;
    ReplyToId: string;
    CCId: string;
    BCCId: string;
    TOId: string;



    CC: string;
    BCC: string;
    To: string;

    Mode: string = "Preview";
    ObjectType: string = "PM";
    public TemplatePMLists: any[];
    IsFillData: boolean = false;
    public template: any;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private documentTypeTemplatePMService: DocumentTypeTemplatePMService;


    TemplateHeaderHtml: any;
    TemplateHeaderHeight: number;
    TemplateFooterHtml: any;
    TemplateFooterHeight: number;

    RequsetPageName: string;
    public AutomationId: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService, private cd: ChangeDetectorRef, public _htmlEditorService: HtmlEditorService) {
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();

        }
     
    }

    ngOnInit(


    ) {

    }
    IsOpenHeaderAndFooter: boolean = false;
    OldDataTemplateByte: any = null;
    ngAfterViewInit() {
        
    }


    DontShowToField: boolean = false;
    public DataViewModel: any;
    SetWindowArgs(args: any) {



        this.froalaEditorSetting = new FroalaEditorSetting();
        this.DataViewModel = args.DataViewModel;
        this.PageType = args.PageType;
        this.TemplateId = args.TemplateId;
        this.Tenant = args.Tenant;
        this.EntityId = args.EntityId;
        this.ObjectTableId = args.ObjectTableId ? args.ObjectTableId : "";
        this.ChildEntityId = args.ChildEntityId ? args.ChildEntityId : "";
        this.ChildObjectTableId = args.ChildObjectTableId ? args.ChildObjectTableId : "";
        this.DontShowToField = args.DontShowToField;
        this.RequsetPageName = args.RequsetPageName;
        this.AutomationId = args.AutomationId;

        if (args.ObjectType) this.ObjectType = args.ObjectType;
       
        this.TemplatePMLists = args.DocumentTypeTemplatePMLists;
        if (this.TemplatePMLists) {

            if (this.ObjectType == "DocumentTypeTemplateViewModel") {

                var docViewModel = this.TemplatePMLists.filter(d=> d.Id == this.TemplateId)[0];
                if (docViewModel) {
                    this.template = docViewModel.Entity;
                }
            } 
            else {

                this.template = this.TemplatePMLists.filter(d=> d.Id == this.TemplateId)[0];
            }


        }
        else this.TemplatePMLists = [];




        if (this.TemplateId) this.Run(args);

    }





    IsShowAttachmentLinks: boolean = false;


    Run(args: any) {



        this.froalaEditorSetting.PageType = "HtmlDocumentPreview";

        this.froalaEditorSetting.Id = Guid.newGuid();
        this.SubjectId = Guid.newGuid();
        this.FromId = Guid.newGuid();
        this.ReplyToId = Guid.newGuid();
        this.CCId = Guid.newGuid();
        this.BCCId = Guid.newGuid();
        this.TOId = Guid.newGuid();


        this.IsShowButtonSaveAs = true;
        this.froalaEditorSetting.IsDisableEdit = false;
   
 
        if (this.PageType) {
              // Preview
            if (this.PageType == "Preview") {
                this.froalaEditorSetting.Height = window.innerHeight - 190;
                this.froalaEditorSetting.IsDisableEdit = true;
                this.IsShowButtonSaveAs = false;
                this.IsShowButtonCancel = false;
                this.IsShowAreaDataField = false;
                this.IsPreviewMode = true;
                this.froalaEditorSetting.IsDisableEdit = true;

                this.LableSaveButton = "Close";

            }

            // Edit 
            else if (this.PageType == "Maintenance") {
                this.froalaEditorSetting.Height = window.innerHeight - 310;
                this.IsShowButtonSaveAs = false;
                this.Mode = "Edit";

                this.IsShowUploadAndDownloadButtons = true;
            }
            else if (this.PageType == "Send" || this.PageType == "ManageTemplate") {
                this.froalaEditorSetting.Height = window.innerHeight - 310;
                if (this.PageType == "ManageTemplate") this.IsShowHeaderAndFooterButton = true;
          
                this.Mode = "Edit";
                this.IsShowUploadAndDownloadButtons = true;
            }

                 // Signature

            else if (this.PageType == "Signature") {
                this.IsShowAreaDataField = false;
                this.IsShowButtonSaveAs = false;
                this.froalaEditorSetting.Height = window.innerHeight - 240;

            }

            //Report Template
            else if (this.PageType == "ReportTemplate") {
                this.IsShowAreaDataField = true;
                this.IsShowUploadAndDownloadButtons = true;
                if (args.ReportComponentArea == "Maintenance") {
                    this.IsShowButtonSaveAs = false;
                }
                this.froalaEditorSetting.Height = window.innerHeight - 310;

            }
        }
       
        this.froalaEditorSetting.Height = this.froalaEditorSetting.Height - 20;


        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");



        if (this.IsPreviewMode) {
            this.LoadHtmlTemplateData();
        }

        else {
            if (this.template) {
                this.FillData();
            }
            else {

                if (this.PageType == "ReportTemplate") {

                    this.template = args.ReportTemplatePM;
                    if (this.template) {
                        this.FillProp();
                        if (args.IsNewEntity) {
                            if (this.template.TemplateData && this.template.TemplateData.length > 0) {
                                var htmlBody = Base64ToString(this.template.TemplateData);

                                this.froalaEditorSetting.HtmlString = htmlBody;
                                if (this.froalaEditorSetting.froalaEditorComponent) {
                                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlBody);
                                    this.ReloadFroalaEditor();
                                }
                            }

                            this.CurrentSession.CurrentWindow.StopBusyIndicator();

                        } else this.LoadReportTemplateDate();               

                    } else this.CurrentSession.CurrentWindow.StopBusyIndicator();
                  
                }

                else  if (this.PageType == "Signature") {
                    this.LoadSignatureData();

                }
                else {


                    this._documentTypeTemplatePMExtendedService.GetSingleDocumentTypeTemplate(this.TemplateId, this.Tenant).subscribe((res:any) => {

                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                            
                                this.template = myResult;

                               if (this.TemplatePMLists) {
                                   var template = this.TemplatePMLists.filter(d => d.Id == this.template.Id)[0];
                                    if (!template) {

                                        if (this.ObjectType == "DocumentTypeTemplateViewModel") {
                                            this.TemplatePMLists.push(new DocumentTypeTemplateViewModel(this.template));
                                        }
                                        else this.TemplatePMLists.push(this.template);
                                    
                                    }
                                }

                                this.FillData();
                            }
                        }
                    });
                }
            }


        }
    }



    LoadReportTemplateDate() {

        var reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
        reportsTemplatePMExtendedService.GetMessageReportsTemplateBodyByReportTemplateIdAndVersion(this.template.Id, this.template.CurrentVersion).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                var htmlBody: string = "";

                if (myResult && myResult.length>0) {
                     htmlBody = Base64ToString(myResult);

                    this.froalaEditorSetting.HtmlString = htmlBody;
                    if (this.froalaEditorSetting.froalaEditorComponent) {
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlBody);
                        this.ReloadFroalaEditor();
                    }
                }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }

        });
    }

  

    LoadSignatureData() {

        this._documentTypeTemplatePMExtendedService.GetTemplateBodyhtmlOrJsonByDocumentTemplateId(this.TemplateId, this.Tenant, true, this.PageType).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    if (!myResult) myResult = "";
                    if (this.froalaEditorSetting.froalaEditorComponent) {
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult);

                        this.ReloadFroalaEditor();
                    }
                }
            }





            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    }


    IsShowSaveAsButtonOnly: boolean = false;
    FillData() {

        if (this.template) {

            if (this.RequsetPageName == "Automation") {

                if (!this.template.AutomationId) {
                    this.IsShowSaveAsButtonOnly = true;
                } 

            }



            if (this.template.TemplateType == "M") {
                if (this.PageType == "Send" || this.PageType == "ManageTemplate" || this.PageType =="Maintenance") {
                    this.IsShowAttachmentLinks = true;
                }
            }



            this.TemplateHeaderHtml = this.template.TemplateHeaderHtml;
            this.TemplateFooterHtml = this.template.TemplateFooterHtml;
            this.TemplateFooterHeight = this.template.TemplateFooterHeight;
            this.TemplateHeaderHeight = this.template.TemplateHeaderHeight;


            if (this.template && AppTool.IsNullOrEmpty(this.ObjectTableId)) {
                this.ObjectTableId = this.template.ObjectTableId;
            }

            if (this.PageType == "Maintenance" || this.PageType == "Send" || this.PageType == "ManageTemplate") {
                if (this.template.TemplateType == "P") this.IsShowHeaderAndFooterButton = true;
               else {
                    if ((this.template.TemplateHeaderHtml || this.template.TemplateFooterHtml)) this.IsShowHeaderAndFooterButton = true;
                }
            }
           
            this.DocumentTypeCode = this.template.DocumentTypeCode;

            var htmlBody: string = "";
            if (this.template.TemplateBodyHtml) {
        
                htmlBody = Base64ToString(this.template.TemplateBodyHtml);
            } else htmlBody = "";

            this.froalaEditorSetting.HtmlString = htmlBody;


      

            if (this.froalaEditorSetting.froalaEditorComponent) {
                this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlBody);
            }
            else {
                this.IsFillData = true;
            }

            this.Subject = this.template.Subject;
       
            if (this.Mode == "Edit") {

                this.FillProp();



            }


            this.ReloadFroalaEditor();


            if (this.froalaEditorSetting.froalaEditorComponent) {
                this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());

            }
            else {
                this.OldDataTemplateByte = StringToBase64(htmlBody);
            }


            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }

    }

    FillProp() {
        if (this.template) {
            this.Subject = !AppTool.IsNullOrEmpty(this.template.Subject) ? this.template.Subject : "";   
            this.From = !AppTool.IsNullOrEmpty(this.template.From) ? this.template.From : ""; 
            this.ReplyTo = !AppTool.IsNullOrEmpty(this.template.ReplyTo) ? this.template.ReplyTo : ""; 
            this.CC = !AppTool.IsNullOrEmpty(this.template.CC) ? this.template.CC : "";
            this.BCC = !AppTool.IsNullOrEmpty(this.template.BCC) ? this.template.BCC : "";
            this.To = !AppTool.IsNullOrEmpty(this.template.To) ? this.template.To : "";




            if (this.From) {
                this.IsShowFromInputBox = true;
                this.froalaEditorSetting.Height -= 30;

            }

            if (this.ReplyTo) {
                this.IsShowReplyToInputBox = true;
                this.froalaEditorSetting.Height -= 30;
            }

            if (this.CC) {
                this.IsShowCCInputBox = true;
                this.froalaEditorSetting.Height -= 30;

            }

            if (this.BCC) {
                this.IsShowBCCInputBox = true;
                this.froalaEditorSetting.Height -= 30;

            }

            if (this.To) {
                this.IsShowToInputBox = true;
                this.froalaEditorSetting.Height -= 30;

            }


            
        }
    }

    LoadHtmlTemplateData() {

        this._htmlEditorService.getEditorHtmlData("", this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, false, this.TemplateId, this.Subject).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                    this.ReloadFroalaEditor();
                    this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                }
            }

            this.CurrentSession.CurrentWindow.StopBusyIndicator();

        });
    }

    CloseButtonClicked() {
       this.CurrentSession.CurrentWindow.StopBusyIndicator();
       this.DestroyfroalaEditor();
        var id = "";
        if (this.template) {
            id = this.template.Id;
        }
        this.CurrentSession.CurrentWindow.Close(id);

    }


    CancelButtonClicked() {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.DestroyfroalaEditor();
        this.CurrentSession.CurrentWindow.Close("");


    }


    DestroyfroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
        }

    }



    SaveReportTemplateData() {

        var reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();

        this.template.TemplateData = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
        reportsTemplatePMExtendedService.SaveReportTemplateMessageBody(this.template).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.template = myResult;

            } else {

                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }

            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.CloseButtonClicked();
        });


    }





    SaveSignatureData() {


        var filter = new DocumentTypeTemplateFilter();
        filter.Id = this.TemplateId;
        filter.Tenant = this.Tenant;
        filter.Body = this.froalaEditorSetting.froalaEditorComponent.getHtml();
        filter.TemplateType = "HTML";
        filter.Subject = this.Subject;
        filter.Processtype = this.PageType;

        this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe((res:any) => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.CloseButtonClicked();
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }

          

        });
    }

     AddCCLinkClick() {

        this.IsShowCCInputBox = true;
        this.froalaEditorSetting.Height -=30;
        this.ReloadFroalaEditor();
    }


     AddBCCLinkClick() {
         this.IsShowBCCInputBox = true;
         this.froalaEditorSetting.Height -= 30;
         this.ReloadFroalaEditor();
     }


    AddToLinkClick() {
        this.IsShowToInputBox = true;
        this.froalaEditorSetting.Height -= 30;
        this.ReloadFroalaEditor();
    }





    AddReplyToLinkClick() {

        this.IsShowReplyToInputBox = true;
      
        this.froalaEditorSetting.Height -=  30;
        this.ReloadFroalaEditor();
    }


    AddFromLinkClick() {

        this.IsShowFromInputBox = true;
     
        this.froalaEditorSetting.Height -= this.IsShowReplyToInputBox ? 15 : 30;
        this.ReloadFroalaEditor();
    }

    SaveButtonClicked() {
        if (this.PageType != "Preview") {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            if (this.PageType == "Signature") {
                this.SaveSignatureData();
            }
         
            else {

                if (!this.CheckIsValidEmail(this.From)) {

                    this.ShowMessage("From email is Invalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }

                if (!this.CheckIsValidEmail(this.ReplyTo)) {

                    this.ShowMessage("Reply-to email is iInvalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }

                if (!this.CheckIsValidEmails(this.CC)) {

                    this.ShowMessage("Some of Cc e-mails are Invalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }


                if (!this.CheckIsValidEmails(this.BCC)) {

                    this.ShowMessage("Some of Bcc e-mails are Invalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }

                if (!this.CheckIsValidEmails(this.To)) {

                    this.ShowMessage("Some of To e-mails are Invalid", "Logitude Message");
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    return;
                }


                this.template.Subject = this.Subject;
                this.template.From = this.From;
                this.template.ReplyTo = this.ReplyTo;
                this.template.CC = this.CC;
                this.template.BCC = this.BCC;
                this.template.To = this.To;

                if (this.PageType == "ReportTemplate") {

                    this.SaveReportTemplateData();
                }
                else {

                    this.template.TemplateBodyHtml = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                    this.template.TemplateTechnologyCode = "AG";
                    this.template.TemplateHeaderHtml = this.TemplateHeaderHtml;
                    this.template.TemplateHeaderHeight = this.TemplateHeaderHeight;
                    this.template.TemplateFooterHtml = this.TemplateFooterHtml;
                    this.template.TemplateFooterHeight = this.TemplateFooterHeight;

                    this.documentTypeTemplatePMService.update(this.template).subscribe((res:any) => {

                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            var template = myResult;

                            if (this.ObjectType == "DocumentTypeTemplateViewModel") {
                                template = new DocumentTypeTemplateViewModel(myResult);
                            }

                            if (this.TemplatePMLists) {

                                this.TemplatePMLists = this.TemplatePMLists.filter(d => d.Id != this.TemplateId);
                                this.TemplatePMLists.push(template);
                            }

                            this.CloseButtonClicked();


                        }
                        else {

                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                            }
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }


                    });


                }
    
            }


        }
        else {
            this.CloseButtonClicked();
        }


    }



    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }

    }



    SavaAsButtonClicked() {


        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 150;
        logWindow.DataContext = this;
        logWindow.Title = "Save as Template";
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SaveAsTemplateComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {

            if ($event) {

                this.CurrentSession.CurrentWindow.Close($event);
                
            }
        

        });
                            

    }


    AddDataField(type: string) {

         var tableName = "";
        var tableId: string = !AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId;
        var table = window.ObjectTables.filter(d => d.Id == tableId)[0];
        if (table) tableName = table.Name;

        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe((response:any) => {

            if (table) {
                this._entityResourceService.getEntityResourceByTableName(tableName).subscribe((response:any) => {
                    this.ViewDataField(type, this.objecttypeField, tableId);
                });
            }
            else this.ViewDataField(type, "", tableId);

        });

        
    }



    ViewHeaderAndFooter(type: string) {
        var windowArgs: any = {};
        this.IsOpenHeaderAndFooter = true;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.DocumentTypeTemplatePM = this.template;
        windowArgs.PageRequse = this.PageType;
        windowArgs.DataViewModel = this;
        windowArgs.Mode = "Edit";
        windowArgs.PageType = type;
        var logWindow = new LogitudeWindow();
        logWindow.Width = window.innerWidth- 200;
        logWindow.Height = 325;

        logWindow.Title = type;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HeaderAndFooterComponent');


    }
    

    ViewDataField(type: string,  objectTypeField:string , tableId:string) {

        var windowArgs: any = {};
        windowArgs.ObjectTableId = tableId;
        windowArgs.ObjectTypeField = objectTypeField;
        windowArgs.InSertDataFieldType = type;
        windowArgs.DocumentTypeCode = this.DocumentTypeCode;
        
        if (this.PageType == "Signature") windowArgs.ObjectTableId = null;
        this.InSertDataFieldType = type;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
  
        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {

            if ($event) {

                if (type == "Subject") {
                    this.Subject = insertAtSubject(this.SubjectId, $event);
                }
                else if (type == "From") {
                    this.From = $event;
                }
                else if (type == "ReplyTo") {
                    this.ReplyTo = $event;
                }
                else if (type == "CC") {
                    if (this.CC && $event) this.CC += ";";
                    this.CC += $event;
                }
                else if (type == "BCC") {
                    if (this.BCC && $event) this.BCC += ";";
                    this.BCC += $event;
                }
                else if (type == "To") {
                    if (this.To && $event) this.To += ";";
                    this.To += $event;
                }


                else if (type == "FroalaEditor") {
                    this.froalaEditorSetting.froalaEditorComponent.InSertHtml($event);
                    this.ReloadFroalaEditor();
                }
            }

        });
    }

    IsDownLoadButtonClick: boolean = false;

    DownloadButtonClicked() {

        if (!this.IsDownLoadButtonClick) {
            this.IsDownLoadButtonClick = true;
            var templateByte: any = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());

            if (this.PageType != "ReportTemplate") {
                this.template.TemplateBodyHtml = templateByte;
                this.documentTypeTemplatePMService.update(this.template).subscribe((res:any) => {
                    this.IsOpenHeaderAndFooter = false;
                    this.IsDownLoadButtonClick = false;
                    this.OldDataTemplateByte = templateByte;
                    this.DownloadTemplate();
                });
            } else {

                var reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
                this.template.TemplateData = templateByte;
                reportsTemplatePMExtendedService.SaveReportTemplateMessageBody(this.template).subscribe((res:any) => {
                    this.IsOpenHeaderAndFooter = false;
                    this.IsDownLoadButtonClick = false;
                    this.OldDataTemplateByte = templateByte;
                    this.DownloadTemplate();
                });

            }








            //if (templateByte != this.OldDataTemplateByte || this.IsOpenHeaderAndFooter) {
            //    var confirmWindow: ConfirmWindow = new ConfirmWindow();
            //    confirmWindow.Width = 400;
            //    confirmWindow.Show("You have unsaved changes, Please save your work first!");
            //    confirmWindow.YesButtonText = "Save";
            //    confirmWindow.NoButtonText = "Cancel";

            //    confirmWindow.WindowClosed.subscribe((event: any) => {
            //        if (confirmWindow.Yes) {
            //            this.CurrentSession.StartBusyIndicatorSaving();

            //            if (this.PageType != "ReportTemplate") {
            //                this.template.TemplateBodyHtml = templateByte;
            //                this.documentTypeTemplatePMService.update(this.template).subscribe((res:any) => {
            //                    this.IsOpenHeaderAndFooter = false;
            //                    this.IsDownLoadButtonClick = false;
            //                    this.OldDataTemplateByte = templateByte;
            //                    this.CurrentSession.StopBusyIndicator();
            //                });
            //            }

            //            else {
            //                var reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
            //                this.template.TemplateData = templateByte;
            //                reportsTemplatePMExtendedService.SaveReportTemplateMessageBody(this.template).subscribe((res:any) => {
            //                    this.IsOpenHeaderAndFooter = false;
            //                    this.IsDownLoadButtonClick = false;
            //                    this.OldDataTemplateByte = templateByte;
            //                    this.CurrentSession.StopBusyIndicator();
            //                });
            //            }
            //        }

            //        if (confirmWindow.No) {
            //            this.IsDownLoadButtonClick = false;
            //        }


            //    });
            //}
            //else {
            //    this.DownloadTemplate();
            //}

        }
    }

    DownloadTemplate() {
        if (this.template) {

            var fileName = this.template.Description;
            var id = this.template.Id;
            var token = ServiceHelper.GetLDocumentDownloadToken();

            

            var url: string = ServiceHelper.GetLogitudeURL() + "WebPages/DownloadTemplateDocumentPage.aspx?id=" + id + "&tempId=" + token + "&fileName=" + fileName + "&type=" + this.PageType;
            window.open(url);
        }
        this.IsDownLoadButtonClick = false;
    }

    UploadButtonClicked() {

        document.getElementById(this.DocumentTemplateFileId).click();

    }

    UpLoadTemplateFileMethod(event: any) {

        var file = querySelection(this.DocumentTemplateFileId);
    

        if (file) {
            this.ArrayBufferToBase64(file, this);
        }



    }


    ArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            if (viewmodel && binary) {
                viewmodel._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(window.btoa(binary)).subscribe((res:any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var htmltemplate: any = myResult;
                            if (htmltemplate) {

                                htmltemplate.HeaderHtml = !AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                htmltemplate.FooterHtml = !AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";

                                var html: any = htmltemplate.BodyHtml;
                                if (viewmodel.PageType == "ReportTemplate") {
                                    html = htmltemplate.HeaderHtml + html + htmltemplate.FooterHtml;
                                }

                                if (viewmodel.froalaEditorSetting.froalaEditorComponent) {
                                    viewmodel.froalaEditorSetting.froalaEditorComponent.SetHtml(html);
                                    viewmodel.ReloadFroalaEditor();

                                }

                                if (viewmodel.PageType != "ReportTemplate") {
                                    if (viewmodel.PageType == "Maintenance" || viewmodel.PageType == "Send" || viewmodel.PageType == "ManageTemplate") {
                                        if ((!AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) || !AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml))) viewmodel.IsShowHeaderAndFooterButton = true;
                                    }

                                    viewmodel.TemplateHeaderHtml = StringToBase64(htmltemplate.HeaderHtml);
                                    viewmodel.TemplateFooterHtml = StringToBase64(htmltemplate.FooterHtml);
                                    viewmodel.TemplateHeaderHeight = htmltemplate.HeaderHeight;
                                    viewmodel.TemplateFooterHeight = htmltemplate.FooterHeight;
                                }

                            }
                        }
                    }

                });
            
            }


        };

        reader.onerror = function (e) {

        };
        reader.readAsArrayBuffer(file);

    }


    CheckIsValidEmail(email: string) {

        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if (email) {
            if (email.charAt(0) != "[" || email.charAt(email.length - 1) != "]" || email.indexOf("][") > -1) {
                if (email) {
                    if (!EMAIL_REGEXP1.test(email)) {
                        IsOk = false;
                        return;
                    } else if (!EMAIL_REGEXP2.test(email)) {
                        IsOk = false;
                        return;
                    }
                }
            }
        }

        return IsOk;
    }

    CheckIsValidEmails(mailsList: string) {

        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9'._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach((item) => {
                if (item){
                    if (item.charAt(0) != "[" || item.charAt(item.length - 1) != "]" || item.indexOf("][") > -1) {
                        if (item) {
                            if (!EMAIL_REGEXP1.test(item)) {
                                IsOk = false;
                                return;
                            } else if (!EMAIL_REGEXP2.test(item)) {
                                IsOk = false;
                                return;
                            }
                        }
                    }
                }

            });
        }
        if (IsOk) {


        }


        return IsOk;
    }


    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }


    public SelectDefultAttachments() {

        var windowArgs: any = {};
        var tableName: string = "";
        var tableId: string = !AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId;
        windowArgs.ObjectTableId = tableId;
        windowArgs.DocumentTypeTemplatePM = this.template;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Available Documents";
        logWindow.Width = 800;
        logWindow.Height = 600;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentDefultAttachmentsComponent");

    }

    AddDefultExternalAttachments() {
        // here
        var windowArgs: any = {};
        var tableName: string = "";
        var tableId: string = !AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId;
        windowArgs.ObjectTableId = tableId;
        windowArgs.DocumentTypeTemplatePM = this.template;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Attach External Documents";
        logWindow.Width = 600;
        logWindow.Height = 500;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentDefaultExternalAttachmentsComponent");
    }

}





