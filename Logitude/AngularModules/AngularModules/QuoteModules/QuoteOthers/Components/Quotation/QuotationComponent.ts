import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../../Quote/EntityPMs/QuoteTemplatePM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {QuoteTemplateSectionViewModel} from '../../../QuoteTemplates/Components/EditQuoteTemplateComponent';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../../Common/Args';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {QuoteTemplateExtendedPMService} from '../../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {QuoteTemplatePMService} from '../../../../Quote/Services/StandardPMs/QuoteTemplatePMService';
import {ProductTypeList} from '../../../../Common/EntityLists/ProductTypeList';
import {QuoteTemplateSettingPM} from '../../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteTemplateSettingPMService} from '../../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {QuoteTemplateSectionExtendedPMService} from '../../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService';
import {QuoteTemplateSectionPM} from '../../../../Quote/EntityPMs/QuoteTemplateSectionPM';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {QuoteStageListService} from '../../../../Quote/Services/StandardLists/QuoteStageListService';
import {QuoteStageList} from '../../../../Quote/EntityLists/QuoteStageList';
import {QuoteTemplateList} from '../../../../Quote/EntityLists/QuoteTemplateList';
import {QuoteDocumentVersionPM} from '../../../../Quote/EntityPMs/QuoteDocumentVersionPM';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {QuotePMService} from '../../../../Quote/Services/StandardPMs/QuotePMService';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {FroalaEditorSetting} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {GeneralEmailSender} from '../../../../Infrastructure/Helpers/GeneralEmailSender';
import {AttachmentsList} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import {DocumentTypeListService} from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentTypeList} from '../../../../Common/EntityLists/DocumentTypeList';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
declare var attachmentUploader, ResultAsArray: any;
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    selector: 'QuotationComponent',
    moduleId: module.id,
    templateUrl: './QuotationComponent.html',
})
export class QuotationComponent extends BaseComponent implements OnInit {

    public IsDataReady: boolean = false;
    private QuotePM: QuotePM;
    quoteTemplatePMService: QuoteTemplatePMService;
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    quoteTemplateExtendedPMService: QuoteTemplateExtendedPMService;
    quoteTemplateSectionExtendedPMService: QuoteTemplateSectionExtendedPMService;
    myQuoteStageListService: QuoteStageListService;
    quotePMService: QuotePMService;
    public QuoteTemplateSectionLists: QuoteTemplateSectionViewModel[] = [];
    public ReportVersions: QuoteDocumentVersionPM[] = [];
    UploadFileId: string;
    public IsFileUploadedManually: boolean = false;
    public _documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
    _documentTypeListService: DocumentTypeListService = new DocumentTypeListService();
    IsDisableEditQuoteTemplateButton: boolean = false;
    IsShowFromLibraryLink: boolean = false;
    PreviewPdfId: string;
    IsShowPreviewPDF: boolean = true;
    IsReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.Listen();
        this.PreviewPdfId = Guid.newGuid();
        this.UploadFileId = Guid.NewRandomString();
        this.LoadService();

        if (!FeatureLocator.HasFeaturePermession("QuoteTemplate", "UPDATE")) this.IsDisableEditQuoteTemplateButton = true;
        if (FeatureLocator.HasFeaturePermession("QuoteTemplate", "FROMLIBRARY")) {
            this.IsShowFromLibraryLink = true;
        }


    }


    ngOnInit() {

    }

    private SendToCustomerEvent: any = null;
    Listen() {
        if (!this.SendToCustomerEvent) {
            this.SendToCustomerEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "SendToCustomerCompleted") {

                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
                    this.currentDocumentVersion.IsSent = true;
                    this.currentDocumentVersion.SendDate = DateTool.GetCurrentDateTimeAsUtc();

                    var myCreateStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTCR")[0];
                    var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];

                    //this.QuotePM.ActionType = "SentToCustomerFromQuotation";

                    if (this.QuotePM.StageId == myDraftStage.Id || this.QuotePM.StageId == myCreateStage.Id) {
                        this.QuotePM.ActionType = "SetAsSentToCustomer";
                    }
                    //this.CurrentSession.CurrentEditComponent.EntityPM.ActionType = "SetAsSentToCustomer";

                    this.quotePMService.update(this.QuotePM).subscribe(response => {

                        this.CurrentSession.CurrentWindow.StopBusyIndicator();

      

                        if (myDraftStage != null) {
                            if (this.QuotePM.StageId != myDraftStage.Id && this.QuotePM.StageId != myCreateStage.Id) {

                                this.IsEditEnabled = false;
                                this.IsUploadEnabled = false;
                                this.IsEditButtonEnabled = false;
                                this.IsQuoteSent = true;
                            }
                        }
                        else {
                            this.BuildDocumentVersion();
                        }

                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                    });
              
                }
            });
        }

    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SendToCustomerEvent);
        

    }

    LoadService() {

        this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService();
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplatePMService = new QuoteTemplatePMService();
        this.myQuoteStageListService = new QuoteStageListService();
        this.quotePMService = new QuotePMService();

    }


    public froalaEditorSetting: FroalaEditorSetting;
    QuotationWindow: any;
    QuotationTitle: string;
    SetWindowArgs(args: any) {

        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteTemplate").subscribe(response => {
            this.IsReady = true;
 
        this.QuotePM = args.QuotePM;
        this.QuotationWindow = args.QuotationWindow;
        this.QuotationTitle = args.QuotationWindow ? args.QuotationWindow.Title : "";
        this.LoadQuoteCustomerEmail();
        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.IsDisableEdit = true;
        this.froalaEditorSetting.HtmlString = "";
        this.froalaEditorSetting.Height = (this.CurrentSession.CurrentWindow.Height - 100);
        this.HeightPdf = (this.CurrentSession.CurrentWindow.Height - 100);

        this.LoadData();

        });
    }

    public IsQuoteSent: boolean = false;
    public IsUploadEnabled: boolean = false;
    public IsEditEnabled: boolean = true;
    public IsPrintButtonEnabled: boolean = true;
    public IsEditButtonEnabled: boolean = true;
    public IsSendButtonEnabled: boolean = true;

    public HasNoTmplates: boolean = false;


    private allStages: QuoteStageList[] = [];
    public QuotationTemplates: QuoteTemplatePM[] = [];

    public TemplateListTitle: string;


    private selectedMode: string = "";
    public get SelectedMode() {

        return this.selectedMode;

    }
    public set SelectedMode(newValue: string) {
        if (this.selectedMode != newValue) {
            this.selectedMode = newValue;

        }
    }

    SelectedModeClicked(mode) {
        this.SelectedMode = mode;

        if (this.SelectedMode === "Generate" && this.currentDocumentVersion != null && this.currentDocumentVersion.VersionType == "U") {
            if (this.QuotationTemplates.length > 0) {
                if (this.selectedQuoteTemplate == null) {
                    if (!AppTool.IsNullOrEmpty(this.QuotePM.QuoteTemplateId)) {
                        this.SelectedQuoteTemplate = this.QuotationTemplates.filter(r => r.Id == this.QuotePM.QuoteTemplateId)[0];
                    }
                    else {

                        if (!AppTool.IsNullOrEmpty(this.QuotationDefaultTemplateId)) {
                            this.SelectedQuoteTemplate = this.QuotationTemplates.filter(t => t.Id == this.QuotationDefaultTemplateId)[0];
                        }
                        else this.SelectedQuoteTemplate = this.QuotationTemplates.filter(t => t.IsDefault == true)[0];
                       
                    }

                    if (this.SelectedQuoteTemplate == null) {
                        this.SelectedQuoteTemplate = this.QuotationTemplates[0];

                    }
                }


                // Show prompt
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Title = "Generate Document File";
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.Width = 500;
                confirmWindow.Show("Do you want to overwrite the uploaded file?");


                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.currentDocumentVersion.VersionType = "G";
                        this.currentDocumentVersion.VersionTypeName = "Generated";
                        this.currentDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("Quote.Quotation.S.Generated");
                        this.UpdateCurrentVersion();

                    } else if (confirmWindow.No) {
                        this.SelectedMode = "Upload";
                    }
                });

            }
            else {
                var window = new MessageWindow();
                window.Show("There is not templates to generate");

            }
        }
    }
    DocumentTypesList: DocumentTypeList[] = [];
    LoadData() {
        //this.QuotePM.StageId

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator.Tenant
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.DocumentTypesList = pmResponse.Result;

            }

            this.allStages = [];
            this.myQuoteStageListService.getAllFromCache().subscribe((resp: any) => {
                if (!resp.HasError) {
                    this.allStages = resp.Result;
                }
                var myCreateStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTCR")[0];
                var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];

                if (myDraftStage != null) {
                    if (this.QuotePM.StageId != myDraftStage.Id && this.QuotePM.StageId != myCreateStage.Id) {
                        this.IsEditEnabled = false;
                        this.IsUploadEnabled = false;
                        this.IsEditButtonEnabled = false;
                        this.IsQuoteSent = true;
                    }
                }

                this.LoadProductType();
           
            });
        });

    }
    LoadTemplates(templateId:string = null) {
        this.quoteTemplateExtendedPMService.GetQuoteTemplateListsByQuoteTemplateTypeAndTenant(this.QuotePM.QuoteTypeCode, SessionLocator.Tenant).subscribe(response => {

            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!response.HasError) {
                this.QuotationTemplates = response.Result;

                this.TemplateListTitle = TextCodeTranslator.Translate("Quote.Quotation.S.Templates") + "(" + this.QuotationTemplates.length + ")";

                //this.QuotationTemplates.length

                if (!AppTool.IsNullOrEmpty(templateId)) {
                    this.SelectedQuoteTemplate = this.QuotationTemplates.filter(t => t.Id == templateId)[0];
                }


              else  if (!AppTool.IsNullOrEmpty(this.QuotePM.QuoteTemplateId)) {
                    this.SelectedQuoteTemplate = this.QuotationTemplates.filter(r => r.Id == this.QuotePM.QuoteTemplateId)[0];
                }
                else {
                
                    if (!AppTool.IsNullOrEmpty(this.QuotationDefaultTemplateId)) {
                        this.SelectedQuoteTemplate = this.QuotationTemplates.filter(t => t.Id == this.QuotationDefaultTemplateId)[0];
                    }
                    else this.SelectedQuoteTemplate = this.QuotationTemplates.filter(t => t.IsDefault == true)[0];

                    
                }

              
                    

                if (this.SelectedQuoteTemplate == null) {
                    this.SelectedQuoteTemplate = this.QuotationTemplates[0];

                }



                if (this.QuotationTemplates.length == 0) {
                    this.IsPrintButtonEnabled = false;
                    this.IsEditButtonEnabled = false;

                    this.IsSendButtonEnabled = false;
                    //lstViewOption.IsEnabled = false;
                    //pdfViewer.Visibility = Visibility.Collapsed;
                    //blkMessage.Visibility = System.Windows.Visibility.Visible;
                    this.HasNoTmplates = true;

                }
            }
            else {
                //ToDo: handle error
            }

        });
    }

    QuotationDefaultTemplateId: string;
    LoadProductType() {

        if (!AppTool.IsNullOrEmpty(this.QuotePM.ProductCode)) {
            var _productTypeListService: ProductTypeListService = new ProductTypeListService();
            _productTypeListService.getSingleFromCache(this.QuotePM.ProductCode).subscribe(result => {
                var productsList: ProductTypeList = result.Result;
                if (productsList) {
                    this.QuotationDefaultTemplateId = productsList.QuotationDefaultTemplateId;
                }

                this.LoadTemplates();

            });
        } else this.LoadTemplates();

       
    }


    public IsShowTemplateList: boolean = false;
    public AttrTitleShowTemplateList: string = TextCodeTranslator.Translate("Quote.Quotation.B.Expand")  ;
    ShowHideTemplateList() {
        if (this.IsShowTemplateList) {
            this.IsShowTemplateList = false;
            this.AttrTitleShowTemplateList = TextCodeTranslator.Translate("Quote.Quotation.B.Expand");
        }
        else {
            this.IsShowTemplateList = true;
            this.AttrTitleShowTemplateList = TextCodeTranslator.Translate("Quote.Quotation.B.Hide");
        }


        //var element = document.getElementById(this.AttachmentListId);

        //this.ComputeAttachmentListWidth();
        //if (this.AttachmentsLists.length > 4) {
        //    element.setAttribute("style", "height:60px;margin-left:5px;overflow-y:scroll;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        //}
        //else {
        //    element.setAttribute("style", "height:auto;margin-left:5px;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        //}


    }




     //this.SelectedQuoteTemplate
    private selectedQuoteTemplate: QuoteTemplatePM;
    public get SelectedQuoteTemplate() { return this.selectedQuoteTemplate; }
    public set SelectedQuoteTemplate(newValue: QuoteTemplatePM) {
        if (this.selectedQuoteTemplate != newValue) {
            this.selectedQuoteTemplate = newValue;
            var title = this.QuotationTitle + (" (" + this.SelectedQuoteTemplate.Name + ")");
           if (this.QuotationWindow && this.QuotationWindow.Title != title) {
               this.QuotationWindow.Title = title;
            }


            this.PreviewQuoteTemplatePdfReport();
        }


    }



    public HeightPdf: number;
    public IFrameURI: string = "";
    PreviewQuoteTemplatePdfReport(isGenerate: boolean = false) {

        var defultQuoteTemplateId = !AppTool.IsNullOrEmpty(this.QuotePM.QuoteTemplateId) ? this.QuotePM.QuoteTemplateId : "";
        var quotationSections = !AppTool.IsNullOrEmpty(this.QuotePM.QuotationSections) ? this.QuotePM.QuotationSections : "";

        if (isGenerate) defultQuoteTemplateId = "";
        this.quoteTemplateExtendedPMService.GetTemplateSectionsByQuoteTemplateIdAndQuoteId(this.selectedQuoteTemplate.Id, this.QuotePM.Id, SessionLocator.Tenant, defultQuoteTemplateId, quotationSections).subscribe(response => {
            if (!response.HasError) {
                this.selectedQuoteTemplate.TemplateSections = response.Result;
                this.BuildingQuoteTemplateSectionsAndVersions(isGenerate);
            }
        });



    }

    currentDocumentVersion: QuoteDocumentVersionPM;
    BuildingQuoteTemplateSectionsAndVersions(isGenerate: boolean = false) {

        this.QuoteTemplateSectionLists = [];

        var PricingType: string = "";
        if (this.QuotePM != null) {
            if (this.QuotePM.TransportModeId == "A" || this.QuotePM.ShipmentTypeId == "LCL" || this.QuotePM.ShipmentTypeId == "LCLD" || this.QuotePM.ShipmentTypeId == "LTL") {
                PricingType = "P";
            }
            else if (this.QuotePM.ShipmentTypeId == "FCL" || this.QuotePM.ShipmentTypeId == "FTL" || this.QuotePM.ShipmentTypeId == "FCLD") {
                PricingType = "C";
            }
        }

        if (this.SelectedQuoteTemplate.TemplateSections) {
            for (var k in this.SelectedQuoteTemplate.TemplateSections) {
                var item = this.SelectedQuoteTemplate.TemplateSections[k];
                //if (!item.IsCancel) {
                    if (!AppTool.IsNullOrEmpty(PricingType)) {
                        if ((PricingType == "P" && item.QuoteTemplateSectionTypeCode === "PC")
                            || (PricingType == "C" && item.QuoteTemplateSectionTypeCode === "PP")) {
                            continue;
                        }

                    }

                    this.QuoteTemplateSectionLists.push(new QuoteTemplateSectionViewModel(item, this))
               // }
            }

            if (this.QuoteTemplateSectionLists && this.QuoteTemplateSectionLists.length > 0) {
                //this.SelectQuoteTemplateSection = this.QuoteTemplateSectionLists[0];
            }
        }
        var myCreateStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTCR")[0];
        var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];

        this.ReportVersions = this.QuotePM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });

        this.ComputeVersionTypeName();

        if (((!this.ReportVersions.some(v => v.IsSent == false) && (this.QuotePM.StageId == myCreateStage.Id || this.QuotePM.StageId == myDraftStage.Id)) || this.ReportVersions.length == 0)) {
            this.BuildDocumentVersion();
            this.SelectedMode = "Generate";
        }
        else {

            var orderdVersionsByDate = [];
            this.ReportVersions.forEach(item => {
                orderdVersionsByDate.push(item);
            });

            orderdVersionsByDate.sort((a, b) => { return (DateTool.GetDateFromDate(a.UpdateDate) === DateTool.GetDateFromDate(b.UpdateDate)) ? 0 : (DateTool.GetDateFromDate(a.UpdateDate) > DateTool.GetDateFromDate(b.UpdateDate)) ? -1 : 1 });


            this.currentDocumentVersion = orderdVersionsByDate[0];

            if (this.currentDocumentVersion.VersionType == "G") {
                this.SelectedMode = "Generate";
                this.UpdateCurrentVersion(isGenerate);

            }
            else {
                this.SelectedMode = "Upload";
                //txtMessage.Text = "File uploaded manually, no overview is available.";
                this.IsFileUploadedManually = true;
                //btnEdit.IsEnabled = false;
                this.IsEditButtonEnabled = false;
                this.IsEditEnabled = false;
            }
        }


    }

    UpdateCurrentVersion(isGenerate: boolean = false) {
        if (this.currentDocumentVersion != null && this.currentDocumentVersion.VersionType == "G") {
 
            var enableGenerate: boolean = true;
            var sentStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTST")[0];
            var declinedStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDC")[0];
            var acceptedStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTAC")[0];

            if (sentStage != null) {
                if (this.QuotePM.StageId == sentStage.Id) {
                    enableGenerate = false;
                }
            }
            if (acceptedStage != null) {
                if (this.QuotePM.StageId == acceptedStage.Id) {
                    enableGenerate = false;
                }
            }

            if (declinedStage != null) {
                if (this.QuotePM.StageId == declinedStage.Id) {
                    enableGenerate = false;
                }
            }

            if (enableGenerate) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Generating....");

                if (this.QuotePM.QuoteTemplateId != this.SelectedQuoteTemplate.Id) {
                    this.QuotePM.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
                    var templateIds = "";

                    this.SelectedQuoteTemplate.TemplateSections.forEach(section => {
                        templateIds += (section.Id + ",");

                    });
                    if (templateIds.length > 0) {
                        templateIds = templateIds.substring(0, templateIds.length - 1);
                    }

                    this.QuotePM.QuotationSections = templateIds;
                    this.QuotePM.LastVersionNumber = this.currentDocumentVersion.VersionNumber

                }

                this.quoteTemplateExtendedPMService.GetUpdatedQuoteDocumentVersion(this.currentDocumentVersion.QuoteId, this.currentDocumentVersion.VersionNumber, this.SelectedQuoteTemplate.Id, SessionLocator.LoggedUserId, this.currentDocumentVersion.Tenant, isGenerate).subscribe(response => {
                    var pmResponse: ServiceResponse = response;
                    this.CurrentSession.StopBusyIndicator();
                    if (!pmResponse.HasError && pmResponse.Result) {

                        var buffer = EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                        var blob = new Blob([buffer], { type: 'application/pdf' });
                        var objectURL = URL.createObjectURL(blob);


                        this.IFrameURI = objectURL;

                        this.IsQuoteTemplateSectionInCludedChange = false;

                        this.RefreshVersions();

                        this.ClearLastQuoteTemplateVersionDocuemnt();
           
                    }
                });


            }
            else {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
                this.GetQuoteTemplatePdf();
            }
        }
    }


    BuildDocumentVersion() {

        if (this.SelectedQuoteTemplate != null) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Generating....");

            var quoteDocumentVersion: QuoteDocumentVersionPM = new QuoteDocumentVersionPM(this.QuotePM);

            quoteDocumentVersion.Tenant = SessionLocator.Tenant;
            quoteDocumentVersion.QuoteId = this.QuotePM.Id;
            quoteDocumentVersion.CreatedByUserId = SessionLocator.LoggedUserId;
            quoteDocumentVersion.UpdatedByUserId = SessionLocator.LoggedUserId;
            quoteDocumentVersion.VersionType = "G";
            quoteDocumentVersion.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            quoteDocumentVersion.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.UpdateByUserName = SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.CreatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.VersionNumber = -1;
            quoteDocumentVersion.VersionTypeName = "Generated";
            quoteDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("Quote.Quotation.S.Generated");

            this.QuotePM.AddQuoteDocumentVersionPM(quoteDocumentVersion);

            var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];
            if (myDraftStage != null && !this.IsQuoteSent) {
                this.QuotePM.StageId = myDraftStage.Id;
                this.QuotePM.StageName = myDraftStage.Name;
            }

            this.QuotePM.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            var templateIds = "";
            this.SelectedQuoteTemplate.TemplateSections.forEach(section => {
                templateIds += (section.Id + ",");

            });
            if (templateIds.length > 0) {
                templateIds = templateIds.substring(0, templateIds.length - 1);
            }
            this.QuotePM.QuotationSections = templateIds;

            this.quotePMService.update(this.QuotePM).subscribe(response => {

                this.CurrentSession.CurrentWindow.StopBusyIndicator();

                if (!response.HasError) {
                    this.ReportVersions = this.QuotePM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });

                    this.ComputeVersionTypeName();
                    this.currentDocumentVersion = this.ReportVersions.filter(v => v.IsSent == false)[0];

                    this.ClearLastQuoteTemplateVersionDocuemnt();

                  
                    this.GetQuoteTemplatePdf();
                }
                else { }

                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });

            //this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(response => {

            //});
            //this.CurrentSession.CurrentEditComponent.SaveChanges();

            //this.CurrentSession.CurrentEditComponent.SaveChanges();
            //this.quotePMService.update(this.QuotePM).subscribe(response => {
            //    if (!response.HasError) {
            //        this.ReportVersions = this.QuotePM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });
            //        this.currentDocumentVersion = this.ReportVersions.filter(v => v.IsSent == false)[0];

            //        //CleaerLastQuoteTemplateVersionDocuemnt();


            //        this.GetQuoteTemplatePdf();
            //    }
            //    else { }

            //});
            //templatesListBox.SelectedItem = SelectedQuoteTemplate;


        }
    }

    public ClearLastQuoteTemplateVersionDocuemnt() {
        if (this.SelectedQuoteTemplate.IsLastQuoteTemplateDocumentVersion != true) {
            var lastVersions = this.QuotationTemplates.filter(d => d.IsLastQuoteTemplateDocumentVersion == true);
            lastVersions.forEach(list => {
                list.IsLastQuoteTemplateDocumentVersion = false;
            });

            this.SelectedQuoteTemplate.IsLastQuoteTemplateDocumentVersion = true;
        }
    }


    RefreshVersions() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.quoteTemplateExtendedPMService.GetQuoteDocumentVersionsByQuoteId(this.QuotePM.Id, SessionLocator.Tenant).subscribe(response => {
            var pmResponse: ServiceResponse = response;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.ReportVersions = pmResponse.Result;
                this.ComputeVersionTypeName();
            }
        });

        // LoadOperation loadVersions = quotesContext.Load(quotesContext.GetQuoteDocumentVersionsByQuoteIdQuery(this.QuotePM.Id, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, false);
        // loadVersions.Completed += loadVersions_Completed;
    }

    OnPrint() {
        if (this.currentDocumentVersion != null) {
            this.OpenDocumentVersion(this.currentDocumentVersion);
        }
    }


    ComputeVersionTypeName() {
        this.ReportVersions.forEach(item => {
            item.DisplayVersionTypeName = TextCodeTranslator.Translate("Quote.Quotation.S." + item.VersionTypeName);
        });
    }

    GetQuoteTemplatePdf() {
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
        this.quoteTemplateSectionExtendedPMService.GetQuoteTemplatePdfReport(this.QuotePM.Id, this.SelectedQuoteTemplate.Id, SessionLocator.LoggedUserId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {

                var buffer = EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                var blob = new Blob([buffer], { type: 'application/pdf' });
                var objectURL = URL.createObjectURL(blob);
                this.IFrameURI = objectURL;
                this.IsQuoteTemplateSectionInCludedChange = false;



            }


        });

    }

    OpenDocumentVersion(item: QuoteDocumentVersionPM) {
        if (item != null) {
        
            DownloadManager.DownloadPage(item.DocumentId);
        }
    }


    OnUploadQuote() {
        document.getElementById(this.UploadFileId).click();
    }

    File: any;
    IsUploadVisibile: boolean = false;
    FileName: string;
    FileSize: string;
    FileExtension: string;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    FileData: number;

    VersionFileDataParam: VersionFileData;
    UploadFile(event: any) {


        var file: any = attachmentUploader(this.UploadFileId);
        //document.querySelector('#UploadFile').files[0];
        if (file && file.size>0 ) {

            this.VersionFileDataParam = new VersionFileData();
            this.FileName = file.name;
            this.FileExtension = file.name.split('.')[1];

            this._documentsFilingExtendedPMService.GetFileSizeAndUnit(file.size).subscribe(res => {

                var pmResponse: ServiceResponse = res;

                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.FileSize = myResult;
                    }
                }

                if (this.FileExtension && this.FileExtension.length > 10) {
                    this.ShowMessage("File extension should be less than or equal 10 characters");
                }
                else {

                    //this.IsUploadVisibile = true;
                    //this.IsShowProgressBar = true;
                    //this.UploadButtonIsEnabled = false;
                    //this.IsUploadInProgress = true;
                    //this.filterImageParameter = new ImageParameter();

                    //this.filterImageParameter.IsFirstTry = true;
                    //this.filterImageParameter.Tenant = SessionInfo.LoggedUserTenant;
                    //this.filterImageParameter.Extension = this.FileExtension;
                    //this.filterImageParameter.UploadMode = "AttachmentUploader";
                    //this.filterImageParameter.EntityId = this.CurrentDocument.Id;

                    //ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "UploadDocsIn");

                    this.File = file;
                    //var filebuffer = null;
                    //this.filterImageParameter.PartsNumber = this.File.size / 100000;

                    //if (this.filterImageParameter.PartsNumber > 1) filebuffer = this.File.slice(0, 100000);
                    //else filebuffer = this.File.slice(0, file.size);

                    this.VersionFileDataParam.FileExtension = this.FileExtension;
                    this.VersionFileDataParam.QuoteId = this.QuotePM.Id;
                    this.VersionFileDataParam.UpdatedByUserId = SessionInfo.LoggedUserId;
                    this.VersionFileDataParam.VersionNumber = this.currentDocumentVersion.VersionNumber;

                    //this.filterImageParameter.FileSize = file.size;
                    //this.filterImageParameter.SendPartNumber = 1;
                    //this.filterImageParameter.BufferNumber = -1;
                    //this.filterImageParameter.SentSize = 0;
                    //this.filterImageParameter.IsFirstTry = true;
                    //this.filterImageParameter.FileName = this.FileName;
                    var filebuffer = this.File.slice(0, file.size);
                    this.ArrayBufferToBase64(filebuffer, this);



                }

            });
        }

    }

    public ShowMessage(message: string) {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    BuildFileVersion() {
        if (this.currentDocumentVersion != null) {
            this.UploadFileToServer();
        }
        else {

            var quoteDocumentVersion: QuoteDocumentVersionPM = new QuoteDocumentVersionPM(this.QuotePM);

            quoteDocumentVersion.Tenant = SessionLocator.Tenant;
            quoteDocumentVersion.QuoteId = this.QuotePM.Id;
            quoteDocumentVersion.CreatedByUserId = SessionLocator.LoggedUserId;
            quoteDocumentVersion.UpdatedByUserId = SessionLocator.LoggedUserId;
            quoteDocumentVersion.VersionType = "U";
            quoteDocumentVersion.QuoteTemplateId = null;
            quoteDocumentVersion.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.UpdateByUserName = SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.CreatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.VersionNumber = 1;
            quoteDocumentVersion.VersionTypeName = "Uploaded";
            quoteDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("Quote.Quotation.S.Uploaded");
            

            this.QuotePM.AddQuoteDocumentVersionPM(quoteDocumentVersion);

            var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];
            if (myDraftStage != null && !this.IsQuoteSent) {
                this.QuotePM.StageId = myDraftStage.Id;
                this.QuotePM.StageName = myDraftStage.Name;
            }

            this.QuotePM.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            this.quotePMService.update(this.QuotePM).subscribe(response => {



                if (!response.HasError) {
                    //this.ReportVersions = this.QuotePM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });
                    this.currentDocumentVersion = quoteDocumentVersion;//this.ReportVersions.filter(v => v.IsSent == false)[0];
                    this.UploadFileToServer();

                }
                else { this.CurrentSession.CurrentWindow.StopBusyIndicator(); }

                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            // this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
    UploadFileToServer() {
        this.quoteTemplateExtendedPMService.UpLoadQuoteDocumentVersionFile(this.VersionFileDataParam, SessionLocator.Tenant).subscribe(response => {
            if (!response.HasError) {

                this.currentDocumentVersion.VersionType = "U";
                this.currentDocumentVersion.VersionTypeName = "Upload";
                this.currentDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("Quote.Quotation.S.Upload");
                //busyIndicator.Visibility = Visibility.Collapsed;
                //txtMessage.Text = "File uploaded manually, no overview is available.";
                //txtMessage.Visibility = Visibility.Visible;
                this.IsFileUploadedManually = true;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.RefreshVersions();

            }
        });
    }

    ArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            //var fileBase64 = window.btoa(binary);

            viewmodel.VersionFileDataParam.FileBase64String = window.btoa(binary);
            viewmodel.BuildFileVersion();
            // viewmodel.filterImageParameter.Base64String = window.btoa(binary);
            //viewmodel.SendBlockToServer(viewmodel.filterImageParameter);

        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);

    }


    LoadQuoteCustomerEmail() {
        if (this.QuotePM) {
            this.quoteTemplateExtendedPMService.GetQuoteCustomerEmailByContactId(this.QuotePM.CustomerContactId).subscribe(response => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    this.QuoteCustomerEmail = response.Result;
                }
            });
        }

    }


    QuoteCustomerEmail: string;
    GeneralEmailSender: GeneralEmailSender;
    OnSendToCustomer(sendtype: string) {

            if (this.currentDocumentVersion) {

                var documentType = this.DocumentTypesList.filter(d => d.Code === "QUOTE")[0];

                var eventRefreshName = sendtype == "Send to Customer" ? "SendToCustomerCompleted" : "";

                var fileName: string = "Quotation-" + this.QuotePM.QuoteNumber + "-" + this.currentDocumentVersion.VersionNumber;

                var attachment = new AttachmentsList();
                attachment.Tenant = SessionLocator.Tenant;
                attachment.DocumentTypeCopyNameWithDocumentTypeName = fileName;
                attachment.FileSize = this.currentDocumentVersion.FileSize;
                attachment.ShowRemoveLink = true;
                attachment.Id = this.currentDocumentVersion.DocumentId;

                var attachmentsList = new Array<AttachmentsList>();
                attachmentsList.push(attachment);

                if (!this.GeneralEmailSender || (this.GeneralEmailSender && !this.GeneralEmailSender.LoadingSendingComponent)) {
                    this.GeneralEmailSender = new GeneralEmailSender("Quote", "QUOTE", this.QuotePM.Id, this.QuotePM.QuoteNumber, this.QuotePM.CustomerId, "", "", this.SelectedQuoteTemplate.Name, attachmentsList, eventRefreshName, this.QuotePM, false, "QEMO");

                    if (sendtype == "Send to Customer") {
                        this.GeneralEmailSender.ToSpecificeEmail = this.QuoteCustomerEmail;
                    }

                    this.GeneralEmailSender.ExportQuotationsToIntegratedSystem = SessionLocator.TenantPM.ExportQuotationsToIntegratedSystem && !this.QuotePM.IsQuoteDataExternal && !this.QuotePM.IsQuoteDataExternal ? true : false;
                    this.GeneralEmailSender.ShowFullSendControll();
                }

            }
  
    }

    OnEditQuotationTemplate() {
        if (this.selectedQuoteTemplate != null) {
            this.IsShowPreviewPDF = false;
            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.IsNewEntityCall = false;
            windowArgs.CurrentEntity = this.selectedQuoteTemplate;
            windowArgs.QuotePM = this.QuotePM;
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = this.selectedQuoteTemplate.Name;
            logWindow.Width = window.innerWidth - 150;
            logWindow.Height = window.innerHeight - 150;
            logWindow.IsShowCloseButton = true;
            logWindow.DataContext = this;
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");

            logWindow.WindowClosed.subscribe(($event1: any) => {
              this.IsShowPreviewPDF = true;
                
                if ($event1 === "SavedChanges") {
                    this.PreviewQuoteTemplatePdfReport(true);
                }
            });
        }
    }


    IsQuoteTemplateSectionInCludedChange: boolean = false;
    ExcludeSection(sectionViewModel: QuoteTemplateSectionViewModel) {

       // this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.quoteTemplateSectionExtendedPMService.GetMakeQuoteTemplateSectionsExcluded(this.QuotePM.Id, this.selectedQuoteTemplate.Id, sectionViewModel.Id, SessionLocator.Tenant).subscribe(response => {
            this.IsQuoteTemplateSectionInCludedChange = true;
          //  this.CurrentSession.CurrentWindow.StopBusyIndicator();
            //this.PreviewQuoteTemplatePdfReport();
        });

    }
    IncludeSection(sectionViewModel: QuoteTemplateSectionViewModel) {
       // this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.quoteTemplateSectionExtendedPMService.GetMakeQuoteTemplateSectionsIncluded(this.QuotePM.Id, this.selectedQuoteTemplate.Id, sectionViewModel.Id, SessionLocator.Tenant).subscribe(response => {
            this.IsQuoteTemplateSectionInCludedChange = true;
          //  this.CurrentSession.CurrentWindow.StopBusyIndicator();
           // this.PreviewQuoteTemplatePdfReport();
        });
    }
    IsStartEditSession: boolean = false;
    OnEditSectionButtonClicked(item: QuoteTemplateSectionViewModel) {

        if (this.SelectedQuoteTemplate && !this.IsStartEditSession) {
            this.IsStartEditSession = true;
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var percentagewidthwindow = widthwindow * 0.252;
            var percentageHeightwindow = heighthwindow * 0.1764705;
            var sendWindowHeight = heighthwindow - percentageHeightwindow;
            var sendWindowWidth = widthwindow - percentagewidthwindow;
            if (sendWindowWidth < 1000) sendWindowWidth = 1000;
            if (sendWindowHeight < 600) sendWindowHeight = 600;

            var windowArgs: any = {};
            windowArgs.FatherComponent = this;
            windowArgs.HeightWindow = sendWindowHeight;
            windowArgs.QuoteTemplateSectionViewModel = item;
            windowArgs.QuoteTemplateId = this.SelectedQuoteTemplate.Id;

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = sendWindowWidth;
            logWindow.Height = sendWindowHeight;
            logWindow.Title = "Edit Section";
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddEditQuoteTemplateSectionComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    this.PreviewQuoteTemplatePdfReport();
                }
                this.IsStartEditSession = false;
            });
        }


    }

    AddQuoteTemplateFromLibraryClcik() {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.Type = "Quotation";
        windowArgs.QuoteId = this.QuotePM.Id;
        windowArgs.QuoteTypeCode = this.QuotePM.QuoteTypeCode;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;

        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.NewQuoteTemplate"); 
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddQuoteTemplateFromLibraryComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
                this.LoadTemplates($event);
            }
   
        });
    }



}


export class VersionFileData {
    public FileBase64String: string;
    public QuoteId: string;
    public VersionNumber: number;
    public FileExtension: string;
    public UpdatedByUserId: string;

}
