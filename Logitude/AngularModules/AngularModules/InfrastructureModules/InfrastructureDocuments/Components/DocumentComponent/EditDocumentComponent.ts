import {Component, OnInit, ChangeDetectorRef, EventEmitter, Output}  from '@angular/core';
import {ViewChild, ViewContainerRef} from '@angular/core';
import {DocumentOutPMService} from '../../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {StimulsoftArg} from './StimulsoftArg';
import {HtmlEditorService} from '../../../../Common/Services/DocumentServices/HtmlEditorService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {DocumentTypeTemplateListExtendedService} from '../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import {DocumentTypeTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ExportDocumentArgs} from '../../../../Infrastructure/DataContracts/ExportDocumentArgs';
import {FroalaEditorFilters} from './DocsOut/Filters/FroalaEditorFilters';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {DocumentCustomFieldsComponent} from './DocumentCustomFieldsComponent';
import {FroalaEditorSetting} from './DocsOut/FroalaEditorSetting';
import {DocumentTypeTemplateViewModel} from './DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {DocumentCustomFieldsArgs} from './DocsOut/Filters/DocumentCustomFieldsArgs';
import {ExportDocumentService} from '../../../../Common/Services/DocumentServices/ExportDocumentService';
import {DocumentTypeTemplateFilter} from './DocsOut/Filters/DocumentTypeTemplateFilter';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {DocumentOutPM} from '../../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeCustomFieldPM} from '../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityPMService} from '../../../../Infrastructure/Services/EntityPMService';
declare var System: any;
declare var window: any;
declare var insertAtSubject, StringToBase64, querySelection, resultToUnitArray, Base64ToString: any;

@Component({
    
    selector: 'EditDocumentComponent',
    templateUrl: './EditDocumentView.html',
    providers: [HtmlEditorService, DocumentTypeTemplatePMService, DocumentTypeTemplateListExtendedService, DocumentTypeTemplatePMExtendedService, ExportDocumentService, DocumentTypePMExtendedService, DocumentOutPMService]
})


export class EditDocumentComponent implements OnInit {
    IsDisableAddTemplateFromLibrary: boolean = false;
    public DocumentTypeTemplateId: string = null;
    IsDisplayToggleButtonMenu: boolean = false;
    WindowHeight: number;
    WindowWidth: number;
    DocumentTemplateEditorTool: string;
    DocumentTypeId: string;
    DocumentTypeCustomFieldLists: DocumentTypeCustomFieldPM[];
    DocumenttypetemplateLists: DocumentTypeTemplateViewModel[];
    XamlDocumentId: string;
    TemplateFormatCode: string;
    CurrentDocument: DocumentOutPM;
    DocumentTypePM: DocumentTypePM;
    public IsShowFroalaEditor: boolean = false;
    public IsShowTemplateList: boolean;
    IsCheckedInActive: boolean;
    public ReportTemplates: DocumentTypeTemplateViewModel[];
    ObjectTableId: string;
    Subject: string;
    ShowInactiveCheckBoxKey: string;
    DocumentTemplateFileId: string = Guid.NewRandomString();
    public DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
    HeaderHtml: string = "";
    FooterHtml: string = "";
    BodyHtml: string = "";
    HeaderHeight: number;
    FooterHeight: number;
    IsEditManageTemplate: boolean = false;
    @Output() OnHeaderAndFooterCompleteEvent: EventEmitter<any> = new EventEmitter();
    public SelectedDocumentTypeTemplateViewModel: DocumentTypeTemplateViewModel;

    TitleList: string;
    froalaEditorSetting: FroalaEditorSetting;
    stimulsoftArg: StimulsoftArg;
    PopupEditDocumentPreviceScreenWidth: string;
    SubjectId: string;
    RefreshTemplateId: string;

    IsDisplayOnly: boolean = true;
    Tenant: number;
    CurrentDocumentOutId: string = null;
    EntityId: string;
    DocumenttypeCode: string = null;
    DocumentTypeCopyId: string = "";

    IsEditStimul: boolean;
    IsManageStimul: boolean;
    IsAdditionalPrintingStimula: boolean;
    ModePage: string;
    PageType: string;
    IsEditHtml: boolean;
    IsManageHtml: boolean;

    IsShowStimulReportView: boolean;
    //IsStimul: boolean;

    IsShowSaveAndCancelButton: boolean;
    DocumentCustomFieldsArgs: DocumentCustomFieldsArgs;
    AdditionalPrintingAFieldsreaHeight: string;
    OldHtml: string = "";
    IsOpenHeaderAndFooter: boolean = false;

    IsShowEditHtml: boolean = false;
    IsShowEditStimual: boolean;
    IsShowNoEditAllow: boolean;
    public documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    private entityPMService: EntityPMService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypePMService: DocumentTypePMExtendedService, public _exportDocumentService: ExportDocumentService, public _htmlEditorService: HtmlEditorService, public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService, public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService, private cd: ChangeDetectorRef, public _documentOutPMService: DocumentOutPMService) {
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();
        }

        this.ShowInactiveCheckBoxKey = Guid.newGuid();
        this.entityPMService = new EntityPMService();

    }

    ngOnInit() {

        this.DocumentTypeTemplatePMLists = new Array<DocumentTypeTemplatePM>();
        this.IsCheckedInActive = false;


    }
    IsSystemAdditionalPrintingFields: boolean;
    PrintingFieldsScreenCode: string;
    DataViewModel: any;
    Run() {
     
        this.SubjectId = Guid.newGuid();
        if (this.CurrentDocument && this.DocumentTemplateEditorTool) {
            if (this.DocumentTemplateEditorTool == "S") {

                this.stimulsoftArg = new StimulsoftArg();
                this.stimulsoftArg.Tenant = SessionInfo.LoggedUserTenant;
                this.stimulsoftArg.NumberOfPage = 1;
                this.stimulsoftArg.DocumenttypetemplateId = this.DocumentTypeTemplateId;
                this.stimulsoftArg.EditDocumentComponent = this;
                this.stimulsoftArg.TypePage = this.ModePage;
                this.IsShowStimulReportView = true;
                this.stimulsoftArg.ShowStimulFooter = true;
                if (this.ModePage != "Preview") {
                    this.stimulsoftArg.ShowStimulHeader = true;
                    this.stimulsoftArg.IsShowShiftToolbar = true;
                    this.IsShowSaveAndCancelButton = true;

                }


            }

            if (this.DocumentTemplateEditorTool == "S" && this.PageType == "EditDocument") {

                this.stimulsoftArg.ScreenWidth = this.WindowWidth - 40;
                if (this.ModePage == "StimaulEdit") {
                    this.stimulsoftArg.ScreenHeight = this.WindowHeight - 100;
                    this.IsDisplayOnly = false;
                }
                else if (this.ModePage == "AWBWizardEdit") {
                    this.stimulsoftArg.ScreenHeight = this.WindowHeight - 120;
                    this.IsShowNoEditAllow = true;
                }
                else if (this.ModePage == "Preview") {
                    this.stimulsoftArg.ScreenHeight = this.WindowHeight - 70;
                }


                this.IsEditStimul = true;


            }
            else if (this.DocumentTemplateEditorTool == "S" && this.PageType == "ManageTemplate") {
                this.stimulsoftArg.ScreenHeight = this.WindowHeight - 135;
                this.stimulsoftArg.ScreenWidth = this.WindowWidth - 390;
                this.stimulsoftArg.ShowStimulHeader = false;
                this.IsShowTemplateList = true;
                this.IsManageStimul = true;
                var FeatureName = "";
                if (this.DocumentTemplateEditorTool == "S") FeatureName = "MRTPDFPRINT";
                else FeatureName = "RICHTEXTPRINT";


                if (this.IsManageStimul && FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "UPDATE") && FeatureLocator.HasFeaturePermession("DocumentType", FeatureName)) {
                    this.IsShowEditStimual = true;
                }

            }

            else if (this.DocumentTemplateEditorTool == "S" && this.PageType == "AdditionalPrintingFields") {

                this.IsSystemAdditionalPrintingFields = this.DocumentTypePM.IsSystemAdditionalPrintingFields;
                this.PrintingFieldsScreenCode = this.DocumentTypePM.PrintingFieldsScreenCode;

                this.stimulsoftArg.ScreenHeight = this.WindowHeight - 105;
                this.stimulsoftArg.ScreenWidth = this.WindowWidth - 390;
                this.DocumentCustomFieldsArgs = new DocumentCustomFieldsArgs();
                this.DocumentCustomFieldsArgs.editDocumentComponent = this;
                this.DocumentCustomFieldsArgs.EditCustomField = true;
                this.DocumentCustomFieldsArgs.Tenant = this.Tenant;
                this.DocumentCustomFieldsArgs.DocumentTypeId = this.DocumentTypeId;
                this.DocumentCustomFieldsArgs.ObjectTableId = this.ObjectTableId;
                this.DocumentCustomFieldsArgs.DocumentTypeCustomFieldLists = this.DocumentTypeCustomFieldLists;
                this.DocumentCustomFieldsArgs.EntityId = this.EntityId;
                this.AdditionalPrintingAFieldsreaHeight = (this.stimulsoftArg.ScreenHeight - 0).toString() + "px";
           

                if (!this.IsSystemAdditionalPrintingFields) {
                    this.IsAdditionalPrintingStimula = true;
                } else {
                    if (!AppTool.IsNullOrEmpty(this.PrintingFieldsScreenCode) && this.DataViewModel) {
                        this.DocumentCustomFieldsArgs.ScreenCode = this.PrintingFieldsScreenCode;
                        this.DocumentCustomFieldsArgs.EntityPM = this.DataViewModel.DataContext.EntityPM;
                        this.DocumentCustomFieldsArgs.ObjectTableName = this.DataViewModel.DataContext.ObjectTableName;
                        this.IsAdditionalPrintingStimula = true;
                    }
                }




 
            }
            else if (this.DocumentTemplateEditorTool == "R") {
                this.IsShowSaveAndCancelButton = true;
                this.froalaEditorSetting = new FroalaEditorSetting();
                this.froalaEditorSetting.Id = Guid.newGuid();
                this.froalaEditorSetting.PageType = "Edit";
                this.IsShowFroalaEditor = true;
                if (this.PageType == "EditDocument") {
                    this.IsShowTemplateList = false;
                    this.froalaEditorSetting.Height = this.WindowHeight - 210;
                    this.IsEditHtml = true;
                    this.froalaEditorSetting.IsDisableEdit = false;
                    //this.froalaEditorSetting.RemovePageBreak = true;
                }
                else if (this.PageType == "ManageTemplate") {
                    this.froalaEditorSetting.IsDisableEdit = true;
                    this.froalaEditorSetting.Height = this.WindowHeight - 130; //window.innerHeight - 300;
                    this.IsShowTemplateList = true;
                    this.IsManageHtml = true;
                    if (FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "UPDATE") && FeatureLocator.HasFeaturePermession("DocumentType", "HTMLEMAIL")) {
                        this.IsShowEditHtml = true;
                    }


                }
            }


            if (this.IsShowTemplateList) {
                this.LoadDocumentTypeTemplates(null);
            }


            if (this.IsManageHtml || this.IsEditHtml) {
                if (this.XamlDocumentId) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
                    this._exportDocumentService.DownloadFileFromServer(this.XamlDocumentId, this.Tenant).subscribe((res:any) => {
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {

              
                                this.HeaderHtml = myResult[0];
                                this.BodyHtml = myResult[1];
                                this.FooterHtml = myResult[2];
                                this.HeaderHeight = myResult[3];
                                this.FooterHeight = myResult[4];
                          
                                if (!this.IsEditHtml) {
                                    this.BodyHtml = this.HeaderHtml + this.BodyHtml + this.FooterHtml;
                                }
                             
                                this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.BodyHtml);
                                this.OldHtml = this.froalaEditorSetting.froalaEditorComponent.getHtml();
                                this.ReloadFroalaEditor();

                                if (this.IsEditHtml) {
                                 
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 230;
                                    logWindow.Height = 85;
                                    logWindow.Title = "";
                                    logWindow.IsHideWindowMargin = true;
                                    logWindow.IsHideHeader = true;
                                    logWindow.DataContext = "";
                                    logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SimplogInfoPopupComponent');

                                     logWindow.WindowClosed.subscribe(($event: any) => {
                                        if ($event == "Regenerate") {
                                            this.LoadHtmlTemplateData(null, "Edit");
                                        }
                                      

                                    });

                                }
                               
                         
                            }
                        }


                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    });





                }
                else {
                    this.LoadHtmlTemplateData(null);
                }

            }

            else if (this.IsManageStimul || this.IsEditStimul || this.IsAdditionalPrintingStimula) {

                this.LoadstimulData(null, this.IsDisplayOnly, true, this.stimulsoftArg.NumberOfPage, "GenerateReport", "");


            }

        }
        else this.IsShowSaveAndCancelButton = true;

    }


    LoadHtmlTemplateData(templateId: string, mode: string = null) {

        if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedDocumentTypeTemplateViewModel.HtmlData);
            this.ReloadFroalaEditor();

           

        }
        else {

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            
            var docoutId = this.CurrentDocumentOutId;
            if (templateId) docoutId = "";
            this._htmlEditorService.getEditorHtmlData(docoutId, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, false, templateId, "", mode).subscribe((res:any) => {
                var htmlresult = "";

                var pmResponse: ServiceResponse = res;

                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        htmlresult = myResult.Htmlstring;
          

                        this.Subject = myResult.Subject;
                        if (mode == "Edit") {
                            this.HeaderHeight = myResult.HeaderHeight;
                            this.HeaderHtml = myResult.HeaderHtml;
                            this.FooterHeight = myResult.FooterHeight;
                            this.FooterHtml = myResult.FooterHtml;
                        }
                       
                        if (this.SelectedDocumentTypeTemplateViewModel != null) {
                            this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                            this.SelectedDocumentTypeTemplateViewModel.HtmlData = htmlresult;
                        }
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlresult);
                        this.ReloadFroalaEditor();

                    }
                }


                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });

        }
    }


    CloseButtonClicked() {

        var isClose = true;
        if (this.IsAdditionalPrintingStimula) {

            if (this.DocumentCustomFieldsArgs && this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent && this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent.HasError) {
                isClose = false;
                this.entityPMService.getSingle(this.DocumentCustomFieldsArgs.ObjectTableName, this.EntityId).then((response: any) => {
                    response.subscribe((res) => {
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {

                            this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent.CustomFieldLists.forEach(field => {
                                this.DocumentCustomFieldsArgs.EntityPM[field.FieldName] = pmResponse.Result[field.FieldName];

                            });
                     
                            this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent.HasError = false;
                            this.DocumentCustomFieldsArgs.EntityPM.IsDirty = false;
         
                        }

                        this.DataViewModel.LoadDocumentCustomFields();

                        if (this.CurrentSession.CurrentWindow) {
                            this.CurrentSession.CurrentWindow.Close("");
                        }

                    });
                });

            }

          else  this.DataViewModel.LoadDocumentCustomFields();
        }

        if (this.IsEditHtml || this.IsManageHtml) this.DestroyfroalaEditor();



        if (isClose) {
            this.CurrentSession.CurrentWindow.Close("");
        }


    }


    CancelButtonClicked() {

        this.DataViewModel.IsRefreshPrintConrol = false;
        this.CloseButtonClicked();

    }

    LoadstimulData(documenttypetemplateId: string, isDisplayOnly: boolean, isloadingtemplate: boolean, pagenumber: number, processType: string, reportKey: string, messageIndicator: string = null, iscloseWindow: boolean = false) {
        

        if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
            this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
        }

            if (messageIndicator) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(messageIndicator);
            }
            else this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");


            if (!documenttypetemplateId) {
                documenttypetemplateId = this.DocumentTypeTemplateId
                this.stimulsoftArg.DocumenttypetemplateId = documenttypetemplateId;
            }


            var exportDocumentArgs = new ExportDocumentArgs();
            exportDocumentArgs.DocumentTypeTemplateId = documenttypetemplateId;
            exportDocumentArgs.IsDisplayOnly = isDisplayOnly;
            exportDocumentArgs.PageNumber = pagenumber;
            exportDocumentArgs.RequestMethodType = processType;
            exportDocumentArgs.ReportKey = reportKey;
            exportDocumentArgs.Tenant = this.Tenant;
            exportDocumentArgs.CurrentDocumentOutId = this.CurrentDocumentOutId;
            exportDocumentArgs.CurrentDocumentTypeCode = this.DocumenttypeCode;
            exportDocumentArgs.DocumentTypeCopyId = this.DocumentTypeCopyId;
            exportDocumentArgs.EntityId = this.EntityId;
            exportDocumentArgs.ObjectTableId = this.ObjectTableId;
            exportDocumentArgs.LoggedContactId = SessionInfo.LoggedUserId;
            exportDocumentArgs.ChildEntityId = this.ChildEntityId;
            exportDocumentArgs.ChildObjectTableId = this.ChildObjectTableId;
            exportDocumentArgs.LoggedContactName = SessionLocator.LoggedUserPM.EnglishName;
            exportDocumentArgs.AccountingCurrencyId = SessionLocator.TenantPM.CurrencyId;


            this._exportDocumentService.PostReportStimulsoftViewer(exportDocumentArgs).subscribe((res:any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {

                        this.stimulsoftArg.BuildStimulReportResult = myResult;
                        if (this.SelectedDocumentTypeTemplateViewModel != null && pagenumber == 1) {
                            this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                            this.SelectedDocumentTypeTemplateViewModel.StimulData = myResult;
                        }

                        if (iscloseWindow) {

                            this.CloseButtonClicked();
                        }
                        else {
                            this.ReloadStimulsoftViewer();
                        }

                    }
                }


            });
       
    }



    private LoadCompletedEvent: any = null;
    IsCloseViewHeaderAndFooter: boolean = false;
    ViewHeaderAndFooter(editType: string) {
        this.IsOpenHeaderAndFooter = true;
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.Mode = "Edit";
        windowArgs.PageType = logWindow.Title = editType;
        windowArgs.HtmlString = editType == "Header" ? this.HeaderHtml : this.FooterHtml;
        windowArgs.OnHeaderAndFooterCompleteEvent = this.OnHeaderAndFooterCompleteEvent;
        windowArgs.PageRequse = "ManageDocument";
        windowArgs.HeightValue = editType == "Header" ? this.HeaderHeight : this.FooterHeight;
        this.IsCloseViewHeaderAndFooter = false;
        logWindow.Width = window.innerWidth - 200;
        logWindow.Height = 325;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HeaderAndFooterComponent');

        if (!this.LoadCompletedEvent) {
            this.LoadCompletedEvent = this.OnHeaderAndFooterCompleteEvent.subscribe(($event: any) => {

                if ($event.PageType == "Header") {
                    this.HeaderHtml = $event.HtmlString;
                    this.HeaderHeight = $event.HeightValue;

                }

                else if ($event.PageType == "Footer") {
                    this.FooterHtml = $event.HtmlString;
                    this.FooterHeight = $event.HeightValue;

                }

                if (this.LoadCompletedEvent) {
                    this.LoadCompletedEvent.unsubscribe();
                    this.LoadCompletedEvent = null;
                }




            });
        }

    }
    

    LoadDocumentTypeTemplates(selectId: string) {
        this.ReportTemplates = new Array<DocumentTypeTemplateViewModel>();
        this.DocumenttypetemplateLists = new Array<DocumentTypeTemplateViewModel>();
        this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.DocumentTypeId, this.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    myResult.forEach((item) => {

                        if (this.DocumentTemplateEditorTool == "R") {
                            if (item.DocumentTypeId == this.DocumentTypeId && item.TemplateType == "P" && this.TemplateFormatCode == "P" && item.EditorTool == "R") {

                                var template = new DocumentTypeTemplateViewModel(item);
                                if (!item.InActive || item.IsDefault || item.Id == this.DocumentTypeTemplateId) {
                                    this.ReportTemplates.push(template);
                                }
                                this.DocumenttypetemplateLists.push(template);
                            }
                        }
                        else {
                            if (this.DocumentTemplateEditorTool == "S") {

                                if (item.EditorTool == this.DocumentTemplateEditorTool && item.DocumentTypeId == this.DocumentTypeId && item.TemplateType == "P") {

                                    var template = new DocumentTypeTemplateViewModel(item);

                                    if (!item.InActive || item.IsDefault || (item.Id == this.DocumentTypeTemplateId)) {
                                        this.ReportTemplates.push(template);
                                    }
                                    this.DocumenttypetemplateLists.push(template);
                                }

                            }
                        }


                    });

                    var count = this.ReportTemplates ? this.ReportTemplates.length.toString() : "0";
                    this.TitleList = "Templates " + "( " + count + " )";
                    if (this.ReportTemplates && this.ReportTemplates.length > 0) {


                        if (!selectId) {
                            this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(r => r.Id == this.DocumentTypeTemplateId)[0];
                        }
                        else {

                            this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(r => r.Id == selectId)[0];

                        }
                    }

             
                }

            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0]);
                }
            }


        });
    }


    OnSelectTemplateChange(selectedItem: DocumentTypeTemplateViewModel) {
        this.IsEditManageTemplate = true;
        if (selectedItem != this.SelectedDocumentTypeTemplateViewModel) {
            this.CurrentDocument.DocumentTemplateId = selectedItem.Id;
            this.SelectedDocumentTypeTemplateViewModel = selectedItem;
            if (this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
                if (this.IsManageHtml) {
                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedDocumentTypeTemplateViewModel.HtmlData);
                    this.ReloadFroalaEditor();
                } else if (this.IsManageStimul) {
                    this.stimulsoftArg.NumberOfPage = 1;
                    this.stimulsoftArg.DocumenttypetemplateId = selectedItem.Id;
                    this.stimulsoftArg.BuildStimulReportResult = this.SelectedDocumentTypeTemplateViewModel.StimulData;
                    this.ReloadStimulsoftViewer();
                }
            }

            else {
                if (this.IsManageHtml) {
                    this.HtmlDocumentTemplateSelectedChange(selectedItem);
                }
                else if (this.IsManageStimul) {
                    this.StimulSoftDocumentTemplateSelectedChange(selectedItem);
                }
            }

        }

    }



    HtmlDocumentTemplateSelectedChange(selectedItem: any) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._htmlEditorService.getEditorHtmlData("", this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, false, selectedItem.Id, "").subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.Subject = myResult.Subject;
                    this.SelectedDocumentTypeTemplateViewModel.HtmlData = myResult.Htmlstring;
                    this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(myResult.Htmlstring);
                    this.ReloadFroalaEditor();
                }
            }

            this.CurrentSession.CurrentWindow.StopBusyIndicator();

        });
    }

    StimulSoftDocumentTemplateSelectedChange(selectedItem:any) {

        var editableFieldsBody: string = this.CurrentDocument.EditableFields ? Base64ToString(this.CurrentDocument.EditableFields) : null;
        if (editableFieldsBody && editableFieldsBody.indexOf("<Items isList='true' count='0' />") == -1) {
            var confirmWindow: ConfirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Do you want to lose the data you have entered manually to your edited template?");
            confirmWindow.YesButtonText = "Yes";
            confirmWindow.NoButtonText = "No";
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CurrentDocument.EditableFields = null;
                    this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                    this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe((res:any) => {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.ReportTemplates.filter(d => d.IsLoad == true).forEach((item) => { item.IsLoad = false; });
                        this.LoadDocumentTemplateStimulSoftData(selectedItem);
                    });
                }
                else this.LoadDocumentTemplateStimulSoftData(selectedItem);
            });
        } else this.LoadDocumentTemplateStimulSoftData(selectedItem);

    }






    LoadDocumentTemplateStimulSoftData(selectedItem:any) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.stimulsoftArg.NumberOfPage = 1;
        this.stimulsoftArg.DocumenttypetemplateId = selectedItem.Id;
        this.stimulsoftArg.ReportKey = "";

        var exportDocumentArgs = new ExportDocumentArgs();
        exportDocumentArgs.DocumentTypeTemplateId = selectedItem.Id;
        exportDocumentArgs.IsDisplayOnly = true;
        exportDocumentArgs.PageNumber = this.stimulsoftArg.NumberOfPage;
        exportDocumentArgs.RequestMethodType = "GenerateReport";
        exportDocumentArgs.ReportKey = "";
        exportDocumentArgs.Tenant = this.Tenant;
        exportDocumentArgs.CurrentDocumentOutId = this.CurrentDocumentOutId;
        exportDocumentArgs.CurrentDocumentTypeCode = this.DocumenttypeCode;
        exportDocumentArgs.DocumentTypeCopyId = this.DocumentTypeCopyId;
        exportDocumentArgs.EntityId = this.EntityId;
        exportDocumentArgs.ObjectTableId = this.ObjectTableId;
        exportDocumentArgs.LoggedContactId = SessionInfo.LoggedUserId;
        exportDocumentArgs.ChildEntityId = this.ChildEntityId;
        exportDocumentArgs.ChildObjectTableId = this.ChildObjectTableId;
        exportDocumentArgs.LoggedContactName = SessionLocator.LoggedUserPM.EnglishName;
        exportDocumentArgs.AccountingCurrencyId = SessionLocator.TenantPM.CurrencyId;

        this._exportDocumentService.PostReportStimulsoftViewer(exportDocumentArgs).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.SelectedDocumentTypeTemplateViewModel.StimulData = myResult;
                    this.stimulsoftArg.BuildStimulReportResult = myResult;
                    this.ReloadStimulsoftViewer();
                    this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                }

            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0]);
                }
            }





        });

    }





    public ValidationErrorsList: string[];
     @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
     SaveButtonClicked() {
         var m = this.viewContainerRef;
        var item = null;
        var isNewTemplate = false;
        if (this.DataViewModel && this.ReportTemplates && this.CurrentDocument) {
            this.DataViewModel.DocumentTypeTemplateLists = this.ReportTemplates.filter(d => d.InActive == false || d.Id == this.CurrentDocument.DocumentTemplateId || d.Id == this.CurrentDocument.EmailTemplateId);
        }


        if (this.DataViewModel && this.DataViewModel.DocumentTypeTemplateLists) {
            item = this.DataViewModel.DocumentTypeTemplateLists.filter(d => d.Id == this.CurrentDocument.DocumentTemplateId)[0];
 
        }

        if (this.IsEditHtml || this.IsManageHtml) {
            if (this.IsManageHtml) {
                if (!item) {
                    isNewTemplate = true;
                    item = this.DocumenttypetemplateLists.filter(d=> d.Id == this.CurrentDocument.DocumentTemplateId)[0];
                }
        
            }

            if (item) {

                var html = this.froalaEditorSetting.froalaEditorComponent.getHtml();

                if (this.IsEditHtml) {
                    item.HtmlResolve = "<header>" + "<height>" + "<div style='display:none'>" + this.HeaderHeight + "</div></height>" + this.HeaderHtml + "</header>" + this.froalaEditorSetting.froalaEditorComponent.getHtml() + "<footer>" + "<height>" + "<div style='display:none'>" + this.FooterHeight + "</div></height>" + this.FooterHtml + "</footer>";
                }
                item.Subject = this.Subject;
                item.TemplateHeaderHeight = this.HeaderHeight;
                item.TemplateFooterHeight = this.FooterHeight;
                item.TemplateTechnologyCode = "AG";
                if (isNewTemplate == true) this.DataViewModel.DocumentTypeTemplateLists.push(item);
                this.DataViewModel.CurrentDocumentTypeTemplateList = item;

                if (this.IsManageHtml) {
                    item.HtmlResolve = "";
                }
            }
       
            this.DataViewModel.IsRefreshPrintConrol = true;

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe((res:any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CloseButtonClicked();
            });
         

        }
        
        else if (this.IsEditStimul || this.IsManageStimul) {

            if (this.ModePage != "StimaulEdit") {
         
                if (item) {
                    this.DataViewModel.CurrentDocumentTypeTemplateList = item;
                    this.DataViewModel.IsRefreshPrintConrol = true;
                }
                

                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe((res:any) => {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift()) {
                        this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
                    }
                    else this.CloseButtonClicked();
                });

           
            }
            else {
           
                this.SaveEditFeild();

            }

        }

        else if (this.IsAdditionalPrintingStimula) {
        
            if (this.DataViewModel && this.DocumentCustomFieldsArgs && this.DocumentCustomFieldsArgs.IsEditCustomField) this.DataViewModel.IsRefreshPrintConrol = true;

            if (this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift()) {
                this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
            }
            else if (!this.DocumentCustomFieldsArgs.IsChangeCustomField) {
                this.CloseButtonClicked();
            }



       }


       if (this.DocumentTypePM.IsDirty) {
         this._documentTypePMService.putDocumentType(this.DocumentTypePM).subscribe((res:any) => {
           var pmResponse: ServiceResponse = res;
           if (!pmResponse.HasError) {
             this.DocumentTypePM.IsDirty = false;
           }

         });
       }


    }



    SaveEditFeild() {

        if (this.stimulsoftArg.IsReset) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._exportDocumentService.GetResetEditableFields(this.CurrentDocumentOutId).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    this.stimulsoftArg.IsReset = false;
                    if (this.DataViewModel) {
                        this.DataViewModel.IsRefreshPrintConrol = true;
                    }

                    if (this.CurrentDocument) this.CurrentDocument.EditableFields = pmResponse.Result;

                    this.SaveStimualField();

                }
            });

        }
        else {
            this.SaveStimualField();
        }

      
    }
 
    SaveStimualField() {
        var filter = new DocumentTypeTemplateFilter();
        filter.Tenant = this.Tenant;
        filter.DocumentOutId = this.CurrentDocumentOutId;
        filter.Body = "";
        filter.Processtype = "SaveEditFields";
        filter.PageIndex = this.stimulsoftArg ? (this.stimulsoftArg.NumberOfPage - 1) : 0;
        filter.ReportKey = this.stimulsoftArg ? this.stimulsoftArg.ReportKey : "";
        filter.EditableFieldLists = [];
        if (this.stimulsoftArg.StimulsoftViewerComponent.EditableField != null && this.stimulsoftArg.StimulsoftViewerComponent.EditableField.length > 0) {

            this.stimulsoftArg.StimulsoftViewerComponent.EditableField.filter(d => d.Status == "Change").forEach((field) => {

                if (this.stimulsoftArg.StimulsoftViewerComponent.IsEditableFieldChanged(field)) {
                    filter.EditableFieldLists.push(field);
                    // filter.Body += Field.FieldName + "^" + Field.NewValue + "*" + Field.PageFieldIndex;
                }


            });
            if (filter.EditableFieldLists && filter.EditableFieldLists.length > 0) {

                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

                this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe((res:any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {

                            if (this.CurrentDocument && myResult) this.CurrentDocument.EditableFields = myResult;
                            if (this.DataViewModel) this.DataViewModel.IsRefreshPrintConrol = true;

                        }
                    }

                    if (this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift()) this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
                    else {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CloseButtonClicked();
                    }
                });
            }
            else {
                if (this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift()) this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
                else this.CloseButtonClicked();
            }
        }


    }


    ShowDesignStimul(item: DocumentTypeTemplateViewModel) {

        var title = "";
        if (item != null) {

            title = "Edit Print Template";

        }
        else {
            title = "Edit Document";
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1600;
        logWindow.Height = 850;
        logWindow.Title = title;


        logWindow.DataContext = this.stimulsoftArg;
        logWindow.Show("./Infrastructure/Components/StimulsoftComponent/StimulsoftDesignerComponent");

    }



    ShowHtmlDocumentPreview(item: DocumentTypeTemplateViewModel) {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = this.PageType;
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = this.Tenant;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;

        var logWindow = new LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Html Template";
        

        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.IsEditManageTemplate = true;
            this.RefreshTemplateId = $event;
            if (this.RefreshTemplateId) {
                if (!this.SelectedDocumentTypeTemplateViewModel) {
                    this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];
                }
                if (this.SelectedDocumentTypeTemplateViewModel) {
                    if (this.SelectedDocumentTypeTemplateViewModel.Id != this.RefreshTemplateId) this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];
                }

                if (this.SelectedDocumentTypeTemplateViewModel) {
                    this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
                    this.LoadHtmlTemplateData(this.RefreshTemplateId);
                } else {
                    this.LoadDocumentTypeTemplates(this.RefreshTemplateId);
                    this.LoadHtmlTemplateData(this.RefreshTemplateId);
                }

            }
        });


    }



    ShowEditStimaul(item: DocumentTypeTemplateViewModel) {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = item.Entity.Tenant;
        
        windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
        windowArgs.EntityId = "";
        windowArgs.ChildEntityId = "";
        windowArgs.ChildObjectTableId = "";

        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var logWindow = new LogitudeWindow();

        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Print Template";
         
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        window.designerClosed = false;

        logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");

        logWindow.WindowClosed.subscribe(($event: any) => {

            this.RefreshTemplateId = $event;
            if (this.RefreshTemplateId) {
                if (this.SelectedDocumentTypeTemplateViewModel.Id != this.RefreshTemplateId) {
                    this.SelectedDocumentTypeTemplateViewModel = this.ReportTemplates.filter(d => d.Id == this.RefreshTemplateId)[0];
                }

                this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
                this.stimulsoftArg.NumberOfPage = 1;
                this.stimulsoftArg.ReportKey = "";
                this.stimulsoftArg.DocumenttypetemplateId = this.RefreshTemplateId;
                this.LoadstimulData(this.RefreshTemplateId, this.IsDisplayOnly, true, this.stimulsoftArg.NumberOfPage, "GenerateReport", "");
               
            }
        });


    }

    SetTemplateAsDeflut(selectitem: DocumentTypeTemplateViewModel) {

        if (!selectitem.InActive) {

            if (selectitem != null) {

                if (!this.IsTemplateDefualt(selectitem)) {


                    if (this.TemplateFormatCode == "P") {


                        this.DocumentTypePM.DocumentTypeDefaultReportTemplateId = selectitem.Id;
                        this.DocumentTypePM.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                    }
                    else if (this.DocumentTypePM.TemplateFormatCode == "M") {

                        this.DocumentTypePM.DocumentTypeDefaultHTMLTemplateId = selectitem.Id;
                        this.DocumentTypePM.DocumentTypeDefaultEditorTool = selectitem.EditorTool;
                    }

                    this.ReportTemplates.forEach((template) => {
                        if (template.Id != selectitem.Id) {
                            template.IsDefault = false;

                        }
                        else {
                            template.IsDefault = true;

                        }
                    });



                }
            }
        }
        else {
            this.ShowMessage("Please note that you can't set an inactive template as default");
        }
    

        //////End////
    }

    public ShowMessage(message: string) {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
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


                var item = this.DocumentTypeTemplatePMLists.filter(d=> d.Id == selectitem.Id)[0];


                if (item) {
                    item.InActive = selectitem.InActive;
                    this.UpdateDocumentTypeTemplate(item);
                }

                else {

                    this.documentTypeTemplatePMService.get(selectitem.Id).subscribe((res:any) => {

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



    InSertDataFieldType: string;
    AddDataField(type: string) {
    
            var tableName = "";
            var table = window.ObjectTables.filter(d=> d.Id == this.ObjectTableId)[0];

            if (table) tableName = table.Name;


            var windowArgs: any = {};
            windowArgs.ObjectTableId = this.ObjectTableId;
            windowArgs.ObjectTypeField = "";
            windowArgs.InSertDataFieldType = this.InSertDataFieldType = type;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 600;
            // logWindow.DataContext = this;
            logWindow.Title = "Insert Data Field";

            this._entityResourceService.getEntityResourceByTableName(tableName).subscribe((response:any) => {
                this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe((response:any) => {
                    logWindow.WindowArgs = windowArgs;
                    logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                    logWindow.WindowClosed.subscribe(($event: any) => {

                        if ($event) {
                            if (type == "Subject") {

                                this.Subject = insertAtSubject(this.SubjectId, $event);
                            }
                            else if (type == "FroalaEditor") {
                                this.froalaEditorSetting.froalaEditorComponent.InSertHtml($event);
                                this.ReloadFroalaEditor();
                            }
                        }

                    });
                });
            });
        
    }


    UpdateDocumentTypeTemplate(item: any) {

        this.documentTypeTemplatePMService.update(item).subscribe((myResult:any)=> {

        });
    }



    EditTemplate(selectitem: DocumentTypeTemplateViewModel) {

    }

    IsTemplateDefualt(selectitem: DocumentTypeTemplateViewModel) {


        var IsDefualt = false;

        if (selectitem.TemplateType == "P") {

            if (selectitem.Id == this.DocumentTypePM.DocumentTypeDefaultReportTemplateId) {
                IsDefualt = true;
            }
        }
        else
            if (selectitem.TemplateType == "M") {
                if (selectitem.Id == this.DocumentTypePM.DocumentTypeDefaultHTMLTemplateId) {
                    IsDefualt = true;
                }
            }

        return IsDefualt;

    }



    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
           // this.cd.detectChanges();
        }

    }

    public ReloadStimulsoftViewer() {

        if (this.stimulsoftArg && this.stimulsoftArg.StimulsoftViewerComponent) {
            this.stimulsoftArg.StimulsoftViewerComponent.SetStimualData();
            this.cd.detectChanges();
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

        if (isactive) {
            this.ReportTemplates = this.DocumenttypetemplateLists;
        }
        else {
            this.ReportTemplates = this.DocumenttypetemplateLists.filter(d => d.InActive == false || d.IsDefault == true || (this.SelectedDocumentTypeTemplateViewModel && this.SelectedDocumentTypeTemplateViewModel.Id == d.Id));
        }


    }

    DestroyfroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
        }

    }

    public ChildEntityId: string;
    public ChildObjectTableId: string;

    SetWindowArgs(args: any) {
      
        this.DataViewModel = args.DataViewModel;
        this.PageType = args.PageType;
        this.ModePage = args.ModePage ? args.ModePage : "";
        this.WindowHeight = args.WindowHeight;
        this.WindowWidth = args.WindowWidth;
        this.CurrentDocument = args.CurrentDocument;
        this.DocumentTypePM = args.DocumentTypePM;
        this.Subject = args.Subject ? args.Subject : "";
        this.EntityId = args.EntityId;
        this.DocumentTypeCopyId = args.DocumentTypeCopyId ? args.DocumentTypeCopyId:"";
        this.DocumentTypeCustomFieldLists = args.DocumentTypeCustomFieldLists;


        this.ChildEntityId = args.ChildEntityId ? args.ChildEntityId : "";
        this.ChildObjectTableId = args.ChildObjectTableId ? args.ChildObjectTableId : "";
        this.ObjectTableId = args.ObjectTableId ? args.ObjectTableId : "";

        if (this.DocumentTypePM) {

            if (AppTool.IsNullOrEmpty(this.ObjectTableId)) {
                this.ObjectTableId = this.DocumentTypePM.ObjectTableId;
            }         
    
            this.DocumenttypeCode = this.DocumentTypePM.Code;
            this.Tenant = this.DocumentTypePM.Tenant;
            this.DocumentTypeId = this.DocumentTypePM.Id;
            this.TemplateFormatCode = this.DocumentTypePM.TemplateFormatCode;
        }
        if (this.CurrentDocument) {
            this.CurrentDocumentOutId = this.CurrentDocument.Id;
            this.DocumentTemplateEditorTool = this.CurrentDocument.DocumentTemplateEditorTool;
            this.XamlDocumentId = this.CurrentDocument.XamlDocumentId;
            this.DocumentTypeTemplateId = this.CurrentDocument.DocumentTemplateId;
        }

        this.Run();
    }

    AddTemplateFromLibrary() {

        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe((response:any) => {
         this.IsDisableAddTemplateFromLibrary = true;
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.EntityId = this.EntityId;

        if (this.DocumentTypePM) {
            if (this.DocumentTypePM.TemplateFormatCode == "P" && this.DocumentTypePM.DocumentTypeDefaultEditorTool == "S") {
                logWindow.Title = TextCodeTranslator.Translate("DocumentTypeTemplate.O.NewPrintTemplate");
            }
            else logWindow.Title = TextCodeTranslator.Translate("DocumentTypeTemplate.O.NewHTMLTemplate");

            windowArgs.CurrentDocumentType = this.DocumentTypePM;
        }
        windowArgs.ChildEntityId = this.ChildEntityId;

        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.PageRequest = "Edit";
        windowArgs.DocumentTemplateEditorTool = this.DocumentTemplateEditorTool;


        logWindow.Width = 1000;
        logWindow.Height = 550;

        logWindow.WindowArgs = windowArgs;

        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeTemplateFromLibraryComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.IsDisableAddTemplateFromLibrary = false;
        });
        });

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

            if (viewmodel) {

                viewmodel._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(window.btoa(binary)).subscribe((res:any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var htmltemplate: any = myResult;
                            if (htmltemplate) {

                                htmltemplate.HeaderHtml = !AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                htmltemplate.FooterHtml = !AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";
                                htmltemplate.BodyHtml = !AppTool.IsNullOrEmpty(htmltemplate.BodyHtml) ? htmltemplate.BodyHtml : "";

                                viewmodel.BodyHtml = htmltemplate.BodyHtml;
                                viewmodel.HeaderHtml = htmltemplate.HeaderHtml;
                                viewmodel.FooterHtml = htmltemplate.FooterHtml;
                                viewmodel.HeaderHeight = htmltemplate.HeaderHeight;
                                viewmodel.FooterHeight = htmltemplate.FooterHeight;
                                if (viewmodel.froalaEditorSetting.froalaEditorComponent) {
                                    viewmodel.froalaEditorSetting.froalaEditorComponent.SetHtml(htmltemplate.BodyHtml);
                                    viewmodel.ReloadFroalaEditor();

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


    IsDownLoadButtonClick: boolean = false;
    DownloadButtonClicked() {
        if (!this.IsDownLoadButtonClick) {
            this.IsDownLoadButtonClick = true;

            if (this.IsOpenHeaderAndFooter || (this.OldHtml != this.froalaEditorSetting.froalaEditorComponent.getHtml())) {

                var confirmWindow: ConfirmWindow = new ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Show("You have unsaved changes, Please save your work first!");
                confirmWindow.YesButtonText = "Save";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.CurrentSession.StartBusyIndicatorSaving();
                        var documentTypeCopyId: string = "";
                        if (this.DataViewModel && this.DataViewModel.DocumentTypeload && this.DataViewModel.DocumentTypeload.DocumentTypeCopies[0]) {
                            documentTypeCopyId = this.DataViewModel.DocumentTypeload.DocumentTypeCopies[0].Id;
                        }
                        var filter = new FroalaEditorFilters();
                        filter.DocumentOutId = this.CurrentDocument.Id;
                        filter.DocumentTypeCopyId = documentTypeCopyId;
                        filter.Tenant = SessionInfo.LoggedUserTenant;
                        filter.HtmlString = "<header>" + "<height>" + "<div style='display:none'>" + this.HeaderHeight + "</div></height>" + this.HeaderHtml + "</header>" + this.froalaEditorSetting.froalaEditorComponent.getHtml() + "<footer>" + "<height>" + "<div style='display:none'>" + this.FooterHeight + "</div></height>" + this.FooterHtml + "</footer>";
                        filter.HeaderHeight = this.HeaderHeight;
                        filter.FooterHeight = this.FooterHeight;
                        filter.EntityId = this.EntityId;
                        filter.ChildEntityId = this.ChildEntityId;
                        filter.DocumentTypeId = this.DocumentTypePM ? this.DocumentTypePM.Id : "";
                        this._htmlEditorService.saveEditedReportToServer(filter).subscribe((res:any) => {
                            this.CurrentSession.StopBusyIndicator();

                            this.IsOpenHeaderAndFooter = false;
                            this.OldHtml = this.froalaEditorSetting.froalaEditorComponent.getHtml();
                            this.DownLoadFile();
                        });
                    }
                });

                if (confirmWindow.No) {
                    this.IsDownLoadButtonClick = false;
                }

            } else {
                this.DownLoadFile();
            }

        }
    }


    DownLoadFile() {
        if (this.CurrentDocument && !AppTool.IsNullOrEmpty(this.CurrentDocument.XamlDocumentId)) {
            var fileName = this.CurrentDocument.DocumentTypeName;
            var id = this.CurrentDocument.XamlDocumentId;
            var token = ServiceHelper.GetLDocumentDownloadToken();
            var url: string = ServiceHelper.GetLogitudeURL() + "WebPages/DownloadTemplateDocumentPage.aspx?id=" + id + "&tempId=" + token + "&fileName=" + fileName +  "&XamlDocumentId=" + this.CurrentDocument.XamlDocumentId;
            window.open(url);
        }
        this.IsDownLoadButtonClick = false;

    }


}

