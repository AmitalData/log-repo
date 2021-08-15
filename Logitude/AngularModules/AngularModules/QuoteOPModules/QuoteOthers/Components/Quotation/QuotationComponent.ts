import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
//import {QuoteOPTemplateSectionViewModel} from '../../../QuoteTemplates/Components/EditQuoteTemplateComponent';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../../Common/Args';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {QuoteOPTemplateExtendedPMService} from '../../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateExtendedPMService';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {QuoteOPTemplatePMService} from '../../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplatePMService';
import {ProductTypeList} from '../../../../Common/EntityLists/ProductTypeList';
import {QuoteOPTemplateSettingPM} from '../../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {QuoteOPPM} from '../../../../QuoteOPM/EntityPMs/QuoteOPPM';
import {QuoteOPTemplateSettingPMService} from '../../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {QuoteOPTemplateSectionExtendedPMService} from '../../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateSectionExtendedPMService';
import {QuoteOPTemplateSectionPM} from '../../../../QuoteOPM/EntityPMs/QuoteOPTemplateSectionPM';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {QuoteOPStageListService} from '../../../../QuoteOPM/Services/StandardLists/QuoteOPStageListService';
import {QuoteOPStageList} from '../../../../QuoteOPM/EntityLists/QuoteOPStageList';
import {QuoteOPTemplateList} from '../../../../QuoteOPM/EntityLists/QuoteOPTemplateList';

import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {QuoteOPPMService} from '../../../../QuoteOPM/Services/StandardPMs/QuoteOPPMService';
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
import { QuoteOPTemplateSectionViewModel } from '../../../QuoteTemplates/Components/EditQuoteTemplateComponent';
import { QuoteOPDocumentVersionPM } from '../../../../QuoteOPM/EntityPMs/QuoteOPDocumentVersionPM';
@Component({
    selector: 'QuotationComponent',
    
    templateUrl: './QuotationComponent.html',
})
export class QuotationComponent extends BaseComponent implements OnInit {
    IsShowDownloadTemplateButton: boolean = false;
    public IsDataReady: boolean = false;
    QuoteOPPM: QuoteOPPM;
    QuoteOPTemplatePMService: QuoteOPTemplatePMService;
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    QuoteOPTemplateExtendedPMService: QuoteOPTemplateExtendedPMService;
    QuoteOPTemplateSectionExtendedPMService: QuoteOPTemplateSectionExtendedPMService;
    myQuoteOPStageListService: QuoteOPStageListService;
    QuoteOPPMService: QuoteOPPMService;
    public QuoteOPTemplateSectionLists: QuoteOPTemplateSectionViewModel[] = [];
    public ReportVersions: QuoteOPDocumentVersionPM[] = [];
    UploadFileId: string;
    public IsFileUploadedManually: boolean = false;
    public _documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
    _documentTypeListService: DocumentTypeListService = new DocumentTypeListService();
    IsDisableEditQuoteOPTemplateButton: boolean = false;
    IsShowFromLibraryLink: boolean = false;
    PreviewPdfId: string;
    IsShowPreviewPDF: boolean = true;
    IsReady: boolean = false;

    QuoteTypeCode: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.Listen();
        this.PreviewPdfId = Guid.newGuid();
        this.UploadFileId = Guid.NewRandomString();
        this.LoadService();

        if (!FeatureLocator.HasFeaturePermession("QuoteOPTemplate", "UPDATE")) this.IsDisableEditQuoteOPTemplateButton = true;
        if (FeatureLocator.HasFeaturePermession("QuoteOPTemplate", "FROMLIBRARY")) {
            this.IsShowFromLibraryLink = true;
        }

        if (SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsShowDownloadTemplateButton = true;
        }
        ServiceLocator.SendTotangoUserActivity("Quotation", "View Quotation");
    }


    ngOnInit() {

    }
    private LoadCompletedEvent: any = null;
    private SendToCustomerEvent: any = null;
    Listen() {
        if (!this.SendToCustomerEvent) {
            this.SendToCustomerEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "SendToCustomerCompleted") {

                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
                    this.currentDocumentVersion.IsSent = true;
                    this.currentDocumentVersion.SendDate = DateTool.GetCurrentDateTimeAsUtc();

                    var myCreateStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTCR")[0];
                    var myDraftStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTDR")[0];

                    //this.QuoteOPPM.ActionType = "SentToCustomerFromQuotation";

                    if (this.QuoteOPPM.StageId == myDraftStage.Id || this.QuoteOPPM.StageId == myCreateStage.Id) {
                        this.QuoteOPPM.ActionType = "SetAsSentToCustomer";
                    }
                    let quoteDocVersion = this.QuoteOPPM.QuoteDocumentVersions.filter(d => d.VersionNumber == this.currentDocumentVersion.VersionNumber)[0];
                    if (quoteDocVersion)
                        quoteDocVersion.IsSent = true;
                    this.QuoteOPPMService.update(this.QuoteOPPM).subscribe((response:any) => {

                        this.CurrentSession.CurrentWindow.StopBusyIndicator();

      

                        if (myDraftStage != null) {
                            if (this.QuoteOPPM.StageId != myDraftStage.Id && this.QuoteOPPM.StageId != myCreateStage.Id) {

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

        if (this.LoadCompletedEvent == null) {
            this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.QuoteOPPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }


    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SendToCustomerEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent); 
        
    }

    LoadService() {

        this.QuoteOPTemplateExtendedPMService = new QuoteOPTemplateExtendedPMService();
        this.QuoteOPTemplateSectionExtendedPMService = new QuoteOPTemplateSectionExtendedPMService();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplatePMService = new QuoteOPTemplatePMService();
        this.myQuoteOPStageListService = new QuoteOPStageListService();
        this.QuoteOPPMService = new QuoteOPPMService();

    }


    public froalaEditorSetting: FroalaEditorSetting;
    QuotationWindow: any;
    QuotationTitle: string;
    SetWindowArgs(args: any) {

        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteOPTemplate").subscribe((response:any) => {
            this.IsReady = true;
 
         this.QuoteOPPM = args.QuoteOPPM;

        this.QuoteTypeCode = this.QuoteOPPM.QuoteTypeCode;
        this.QuotationWindow = args.QuotationWindow;
        this.QuotationTitle = args.QuotationWindow ? args.QuotationWindow.Title : "";
        this.LoadQuoteCustomerEmail();
        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.IsDisableEdit = true;
        this.froalaEditorSetting.UseNormalPreview = true;
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


    private allStages: QuoteOPStageList[] = [];
    public QuotationTemplates: QuoteOPTemplatePM[] = [];

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

    SetDefultQuoteOPTemplate() {

        if (this.QuoteOPPM && this.QuotationTemplates) {
            if (!AppTool.IsNullOrEmpty(this.QuoteOPPM.QuoteTemplateId)) {
                this.SelectedQuoteOPTemplate = this.QuotationTemplates.filter(r => r.Id == this.QuoteOPPM.QuoteTemplateId)[0];
            }

            if (this.SelectedQuoteOPTemplate == null) {
                if (!AppTool.IsNullOrEmpty(this.QuotationDefaultTemplateId)) {
                    this.SelectedQuoteOPTemplate = this.QuotationTemplates.filter(t => t.Id == this.QuotationDefaultTemplateId)[0];
                }
            }

            if (this.SelectedQuoteOPTemplate == null) {
                this.SelectedQuoteOPTemplate = this.QuotationTemplates.filter(t => t.IsDefault == true)[0];
            }

            if (this.SelectedQuoteOPTemplate == null) {
                this.SelectedQuoteOPTemplate = this.QuotationTemplates[0];
            }
        }

    }


    SelectedModeClicked(mode) {
        this.SelectedMode = mode;

        if (this.SelectedMode === "Generate" && this.currentDocumentVersion != null && this.currentDocumentVersion.VersionType == "U") {
            if (this.QuotationTemplates.length > 0) {
                if (this.selectedQuoteOPTemplate == null) {
                    this.SetDefultQuoteOPTemplate();
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
                        this.currentDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("QuoteOP.Quotation.S.Generated");
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
        //this.QuoteOPPM.StageId

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator.Tenant
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.DocumentTypesList = pmResponse.Result;

            }

            this.allStages = [];
            this.myQuoteOPStageListService.getAllFromCache().subscribe((resp: any) => {
                if (!resp.HasError) {
                    this.allStages = resp.Result;
                }
                var myCreateStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTCR")[0];
                var myDraftStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTDR")[0];

                if (myDraftStage != null) {
                    if (this.QuoteOPPM.StageId != myDraftStage.Id && this.QuoteOPPM.StageId != myCreateStage.Id) {
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
    LoadTemplates(templateId: string = null) {
        this.SelectedQuoteOPTemplate = null;
        this.QuoteOPTemplateExtendedPMService.GetQuoteOPTemplateListsByQuoteOPTemplateTypeAndTenant(this.QuoteOPPM.QuoteTypeCode, SessionLocator.Tenant).subscribe((response:any) => {

            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!response.HasError) {
                this.QuotationTemplates = response.Result;

                this.TemplateListTitle = TextCodeTranslator.Translate("QuoteOP.Quotation.S.Templates") + "(" + this.QuotationTemplates.length + ")";

                //this.QuotationTemplates.length

                if (!AppTool.IsNullOrEmpty(templateId)) {
                    this.SelectedQuoteOPTemplate = this.QuotationTemplates.filter(t => t.Id == templateId)[0];
                }

                if (this.SelectedQuoteOPTemplate == null) {
                    this.SetDefultQuoteOPTemplate();
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

        if (!AppTool.IsNullOrEmpty(this.QuoteOPPM.ProductCode)) {
            var _productTypeListService: ProductTypeListService = new ProductTypeListService();
            _productTypeListService.getSingleFromCache(this.QuoteOPPM.ProductCode).subscribe((result:any) => {
                var productsList: ProductTypeList = result.Result;
                if (productsList) {
                    if (this.QuoteTypeCode == "A") this.QuotationDefaultTemplateId = productsList.QuotationDefaultTemplateId;
                    else this.QuotationDefaultTemplateId = productsList.RoutingRQuoteDefaultTemplateId;
                }

                this.LoadTemplates();

            });
        } else this.LoadTemplates();

       
    }


    public IsShowTemplateList: boolean = false;
    public AttrTitleShowTemplateList: string = TextCodeTranslator.Translate("QuoteOP.Quotation.B.Expand")  ;
    ShowHideTemplateList() {
        if (this.IsShowTemplateList) {
            this.IsShowTemplateList = false;
            this.AttrTitleShowTemplateList = TextCodeTranslator.Translate("QuoteOP.Quotation.B.Expand");
        }
        else {
            this.IsShowTemplateList = true;
            this.AttrTitleShowTemplateList = TextCodeTranslator.Translate("QuoteOP.Quotation.B.Hide");
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




     //this.SelectedQuoteOPTemplate
    private selectedQuoteOPTemplate: QuoteOPTemplatePM;
    public get SelectedQuoteOPTemplate() { return this.selectedQuoteOPTemplate; }
    public set SelectedQuoteOPTemplate(newValue: QuoteOPTemplatePM) {
        if (this.selectedQuoteOPTemplate != newValue) {
            this.selectedQuoteOPTemplate = newValue;
            var title = this.QuotationTitle + (" (" + this.SelectedQuoteOPTemplate.Name + ")");
           if (this.QuotationWindow && this.QuotationWindow.Title != title) {
               this.QuotationWindow.Title = title;
            }


            this.PreviewQuoteOPTemplatePdfReport();
        }


    }



    public HeightPdf: number;
    public IFrameURI: string = "";
    PreviewQuoteOPTemplatePdfReport(isGenerate: boolean = false) {

        var defultQuoteOPTemplateId = !AppTool.IsNullOrEmpty(this.QuoteOPPM.QuoteTemplateId) ? this.QuoteOPPM.QuoteTemplateId : "";
        var quotationSections = !AppTool.IsNullOrEmpty(this.QuoteOPPM.QuotationSections) ? this.QuoteOPPM.QuotationSections : "";

        if (isGenerate) defultQuoteOPTemplateId = "";
        this.QuoteOPTemplateExtendedPMService.GetTemplateSectionsByQuoteOPTemplateIdAndQuoteId(this.selectedQuoteOPTemplate.Id, this.QuoteOPPM.Id, SessionLocator.Tenant, defultQuoteOPTemplateId, quotationSections).subscribe((response:any) => {
            if (!response.HasError) {
                this.selectedQuoteOPTemplate.TemplateSections = response.Result;
                this.BuildingQuoteOPTemplateSectionsAndVersions(isGenerate);
            }
        });



    }

    currentDocumentVersion: QuoteOPDocumentVersionPM;
    BuildingQuoteOPTemplateSectionsAndVersions(isGenerate: boolean = false) {

        this.QuoteOPTemplateSectionLists = [];

        var PricingType: string = "";
        if (this.QuoteOPPM != null) {
            if (this.QuoteOPPM.TransportModeId == "A" || this.QuoteOPPM.ShipmentTypeId == "LCL" || this.QuoteOPPM.ShipmentTypeId == "LCLD" || this.QuoteOPPM.ShipmentTypeId == "LTL") {
                PricingType = "P";
            }
            else if (this.QuoteOPPM.ShipmentTypeId == "FCL" || this.QuoteOPPM.ShipmentTypeId == "FTL" || this.QuoteOPPM.ShipmentTypeId == "FCLD") {
                PricingType = "C";
            }
        }

        if (this.SelectedQuoteOPTemplate.TemplateSections) {
            for (var k in this.SelectedQuoteOPTemplate.TemplateSections) {
                var item = this.SelectedQuoteOPTemplate.TemplateSections[k];
                //if (!item.IsCancel) {
                    if (!AppTool.IsNullOrEmpty(PricingType)) {
                        if ((PricingType == "P" && item.QuoteOPTemplateSectionTypeCode === "PC")
                            || (PricingType == "C" && item.QuoteOPTemplateSectionTypeCode === "PP")) {
                            continue;
                        }

                    }

                    this.QuoteOPTemplateSectionLists.push(new QuoteOPTemplateSectionViewModel(item, this))
               // }
            }

            if (this.QuoteOPTemplateSectionLists && this.QuoteOPTemplateSectionLists.length > 0) {
                //this.SelectQuoteOPTemplateSection = this.QuoteOPTemplateSectionLists[0];
            }
        }
        var myCreateStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTCR")[0];
        var myDraftStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTDR")[0];

        this.ReportVersions = this.QuoteOPPM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });

        this.ComputeVersionTypeName();

        if (((!this.ReportVersions.some(v => v.IsSent == false) && (this.QuoteOPPM.StageId == myCreateStage.Id || this.QuoteOPPM.StageId == myDraftStage.Id)) || this.ReportVersions.length == 0)) {
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
            var sentStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTST")[0];
            var declinedStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTDC")[0];
            var acceptedStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTAC")[0];

            if (sentStage != null) {
                if (this.QuoteOPPM.StageId == sentStage.Id) {
                    enableGenerate = false;
                }
            }
            if (acceptedStage != null) {
                if (this.QuoteOPPM.StageId == acceptedStage.Id) {
                    enableGenerate = false;
                }
            }

            if (declinedStage != null) {
                if (this.QuoteOPPM.StageId == declinedStage.Id) {
                    enableGenerate = false;
                }
            }

            if (enableGenerate) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Generating....");

                if (this.QuoteOPPM.QuoteTemplateId != this.SelectedQuoteOPTemplate.Id) {
                    this.QuoteOPPM.QuoteTemplateId = this.SelectedQuoteOPTemplate.Id;
                    var templateIds = "";

                    this.SelectedQuoteOPTemplate.TemplateSections.forEach(section => {
                        templateIds += (section.Id + ",");

                    });
                    if (templateIds.length > 0) {
                        templateIds = templateIds.substring(0, templateIds.length - 1);
                    }

                    this.QuoteOPPM.QuotationSections = templateIds;
                    this.QuoteOPPM.LastVersionNumber = this.currentDocumentVersion.VersionNumber

                }

                this.QuoteOPTemplateExtendedPMService.GetUpdatedQuoteDocumentVersion(this.currentDocumentVersion.QuoteOPId, this.currentDocumentVersion.VersionNumber, this.SelectedQuoteOPTemplate.Id, SessionLocator.LoggedUserId, this.currentDocumentVersion.Tenant, isGenerate).subscribe((response: any) => {
                    var pmResponse: ServiceResponse = response;
                    this.CurrentSession.StopBusyIndicator();
                    if (!pmResponse.HasError && pmResponse.Result) {

                        var buffer = EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                        var blob = new Blob([buffer], { type: 'application/pdf' });
                        var objectURL = URL.createObjectURL(blob);


                        this.IFrameURI = objectURL;

                        this.IsQuoteOPTemplateSectionInCludedChange = false;

                        this.RefreshVersions();

                        this.ClearLastQuoteOPTemplateVersionDocuemnt();
           
                    }
                });


            }
            else {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
                this.GetQuoteOPTemplatePdf();
            }
        }
    }


    BuildDocumentVersion() {

        if (this.SelectedQuoteOPTemplate != null) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Generating....");

            var quoteDocumentVersion: QuoteOPDocumentVersionPM = new QuoteOPDocumentVersionPM(/*this.QuoteOPPM*/);

            quoteDocumentVersion.Tenant = SessionLocator.Tenant;
            quoteDocumentVersion.QuoteOPId = this.QuoteOPPM.Id;
            quoteDocumentVersion.CreatedByUserId = SessionLocator.LoggedUserId;
            quoteDocumentVersion.UpdatedByUserId = SessionLocator.LoggedUserId;
            quoteDocumentVersion.VersionType = "G";
            quoteDocumentVersion.QuoteTemplateId = this.SelectedQuoteOPTemplate.Id;
            quoteDocumentVersion.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.UpdateByUserName = SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.CreatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.VersionNumber = -1;
            quoteDocumentVersion.VersionTypeName = "Generated";
            quoteDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("QuoteOP.Quotation.S.Generated");

            // TODO ???? this.QuoteOPPM.AddQuoteDocumentVersionPM(quoteDocumentVersion);

            var myDraftStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTDR")[0];
            if (myDraftStage != null && !this.IsQuoteSent) {
                this.QuoteOPPM.StageId = myDraftStage.Id;
                this.QuoteOPPM.StageName = myDraftStage.Name;
            }

            this.QuoteOPPM.QuoteTemplateId = this.SelectedQuoteOPTemplate.Id;
            var templateIds = "";
            this.SelectedQuoteOPTemplate.TemplateSections.forEach(section => {
                templateIds += (section.Id + ",");

            });
            if (templateIds.length > 0) {
                templateIds = templateIds.substring(0, templateIds.length - 1);
            }
            this.QuoteOPPM.QuotationSections = templateIds;

            this.QuoteOPPMService.update(this.QuoteOPPM).subscribe((response:any) => {

                this.CurrentSession.CurrentWindow.StopBusyIndicator();

                if (!response.HasError) {
                    this.ReportVersions = this.QuoteOPPM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });

                    this.ComputeVersionTypeName();
                    this.currentDocumentVersion = this.ReportVersions.filter(v => v.IsSent == false)[0];

                    this.ClearLastQuoteOPTemplateVersionDocuemnt();

                  
                    this.GetQuoteOPTemplatePdf();
                }
                else { }

                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });

            //this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((response:any) => {

            //});
            //this.CurrentSession.CurrentEditComponent.SaveChanges();

            //this.CurrentSession.CurrentEditComponent.SaveChanges();
            //this.QuoteOPPMService.update(this.QuoteOPPM).subscribe((response:any) => {
            //    if (!response.HasError) {
            //        this.ReportVersions = this.QuoteOPPM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });
            //        this.currentDocumentVersion = this.ReportVersions.filter(v => v.IsSent == false)[0];

            //        //CleaerLastQuoteOPTemplateVersionDocuemnt();


            //        this.GetQuoteOPTemplatePdf();
            //    }
            //    else { }

            //});
            //templatesListBox.SelectedItem = SelectedQuoteOPTemplate;


        }
    }

    public ClearLastQuoteOPTemplateVersionDocuemnt() {
        if (this.SelectedQuoteOPTemplate.IsLastQuoteOPTemplateDocumentVersion != true) {
            var lastVersions = this.QuotationTemplates.filter(d => d.IsLastQuoteOPTemplateDocumentVersion == true);
            lastVersions.forEach(list => {
                list.IsLastQuoteOPTemplateDocumentVersion = false;
            });

            this.SelectedQuoteOPTemplate.IsLastQuoteOPTemplateDocumentVersion = true;
        }
    }


    RefreshVersions() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.QuoteOPTemplateExtendedPMService.GetQuoteDocumentVersionsByQuoteId(this.QuoteOPPM.Id, SessionLocator.Tenant).subscribe((response:any) => {
            var pmResponse: ServiceResponse = response;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.ReportVersions = pmResponse.Result;
                this.ComputeVersionTypeName();
            }
        });

        // LoadOperation loadVersions = quotesContext.Load(quotesContext.GetQuoteDocumentVersionsByQuoteIdQuery(this.QuoteOPPM.Id, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, false);
        // loadVersions.Completed += loadVersions_Completed;
    }

    OnPrint() {
        if (this.currentDocumentVersion != null) {
            this.OpenDocumentVersion(this.currentDocumentVersion);
        }
    }


    ComputeVersionTypeName() {
        this.ReportVersions.forEach(item => {
            item.DisplayVersionTypeName = TextCodeTranslator.Translate("QuoteOP.Quotation.S." + item.VersionTypeName);
        });
    }

    GetQuoteOPTemplatePdf() {
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
        this.QuoteOPTemplateSectionExtendedPMService.GetQuoteOPTemplatePdfReport(this.QuoteOPPM.Id, this.SelectedQuoteOPTemplate.Id, SessionLocator.LoggedUserId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {

                var buffer = EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                var blob = new Blob([buffer], { type: 'application/pdf' });
                var objectURL = URL.createObjectURL(blob);
                this.IFrameURI = objectURL;
                this.IsQuoteOPTemplateSectionInCludedChange = false;



            }


        });

    }


    OnDownLoadTemplateButtonClick() {
        if (!AppTool.IsNullOrEmpty(this.QuoteOPPM.QuoteHTMLDocumentId)) {
            DownloadManager.DownloadPage(this.QuoteOPPM.QuoteHTMLDocumentId);
        }
    }


    OpenDocumentVersion(item: QuoteOPDocumentVersionPM) {
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

            this._documentsFilingExtendedPMService.GetFileSizeAndUnit(file.size).subscribe((res:any) => {

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
                    this.VersionFileDataParam.QuoteId = this.QuoteOPPM.Id;
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

            var quoteDocumentVersion: QuoteOPDocumentVersionPM = new QuoteOPDocumentVersionPM(/*this.QuoteOPPM*/);

            quoteDocumentVersion.Tenant = SessionLocator.Tenant;
            quoteDocumentVersion.QuoteOPId = this.QuoteOPPM.Id;
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
            quoteDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("QuoteOP.Quotation.S.Uploaded");
            
            
            // ?? todo this.QuoteOPPM.AddQuoteOPDocumentVersionPM(quoteDocumentVersion);

            var myDraftStage: QuoteOPStageList = this.allStages.filter(d => d.Code == "QTDR")[0];
            if (myDraftStage != null && !this.IsQuoteSent) {
                this.QuoteOPPM.StageId = myDraftStage.Id;
                this.QuoteOPPM.StageName = myDraftStage.Name;
            }

            this.QuoteOPPM.QuoteTemplateId = this.SelectedQuoteOPTemplate.Id;
            this.QuoteOPPMService.update(this.QuoteOPPM).subscribe((response:any) => {



                if (!response.HasError) {
                    //this.ReportVersions = this.QuoteOPPM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });
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
        this.QuoteOPTemplateExtendedPMService.UpLoadQuoteDocumentVersionFile(this.VersionFileDataParam, SessionLocator.Tenant).subscribe((response:any) => {
            if (!response.HasError) {

                this.currentDocumentVersion.VersionType = "U";
                this.currentDocumentVersion.VersionTypeName = "Upload";
                this.currentDocumentVersion.DisplayVersionTypeName = TextCodeTranslator.Translate("QuoteOP.Quotation.S.Upload");
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
        if (this.QuoteOPPM) {
            this.QuoteOPTemplateExtendedPMService.GetQuoteCustomerEmailByContactId(this.QuoteOPPM.CustomerContactId).subscribe((response:any) => {
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

                var fileName: string = "Quotation-" + this.QuoteOPPM.QuoteNumber + "-" + this.currentDocumentVersion.VersionNumber;

                var attachment = new AttachmentsList();
                attachment.Tenant = SessionLocator.Tenant;
                attachment.DocumentTypeCopyNameWithDocumentTypeName = fileName;
                attachment.FileSize = this.currentDocumentVersion.FileSize;
                attachment.ShowRemoveLink = true;
                attachment.Id = this.currentDocumentVersion.DocumentId;

                var attachmentsList = new Array<AttachmentsList>();
                attachmentsList.push(attachment);

                if (!this.GeneralEmailSender || (this.GeneralEmailSender && !this.GeneralEmailSender.LoadingSendingComponent)) {
                    this.GeneralEmailSender = new GeneralEmailSender("QuoteOP", "QUOTE", this.QuoteOPPM.Id, this.QuoteOPPM.QuoteNumber, this.QuoteOPPM.CustomerId, "", "", this.SelectedQuoteOPTemplate.Name, attachmentsList, eventRefreshName, this.QuoteOPPM, false, "QEMO");

                    if (sendtype == "Send to Customer") {
                      
                        this.GeneralEmailSender.ToSpecificeEmail = this.QuoteCustomerEmail;
                        ServiceLocator.SendTotangoUserActivity("Quotation", "Send Quotation to Customer");
                    }

                    this.GeneralEmailSender.ShowFullSendControll();
                }

            }
  
    }

    OnEditQuotationTemplate() {
        if (this.selectedQuoteOPTemplate != null) {
            this.IsShowPreviewPDF = false;
            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.IsNewEntityCall = false;
            windowArgs.CurrentEntity = this.selectedQuoteOPTemplate;
            windowArgs.QuoteOPPM = this.QuoteOPPM;
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = this.selectedQuoteOPTemplate.Name;
            logWindow.Width = window.innerWidth - 150;
            logWindow.Height = window.innerHeight - 150;
            logWindow.IsShowCloseButton = true;
            logWindow.DataContext = this;
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");

            logWindow.WindowClosed.subscribe(($event1: any) => {
              this.IsShowPreviewPDF = true;
                
                if ($event1 === "SavedChanges") {
                    this.PreviewQuoteOPTemplatePdfReport(true);
                }
            });
        }
    }


    IsQuoteOPTemplateSectionInCludedChange: boolean = false;
    ExcludeSection(sectionViewModel: QuoteOPTemplateSectionViewModel) {

       // this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.QuoteOPTemplateSectionExtendedPMService.GetMakeQuoteOPTemplateSectionsExcluded(this.QuoteOPPM.Id, this.selectedQuoteOPTemplate.Id, sectionViewModel.Id, SessionLocator.Tenant).subscribe((response:any) => {
            this.IsQuoteOPTemplateSectionInCludedChange = true;
          //  this.CurrentSession.CurrentWindow.StopBusyIndicator();
            //this.PreviewQuoteOPTemplatePdfReport();
        });

    }
    IncludeSection(sectionViewModel: QuoteOPTemplateSectionViewModel) {
       // this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.QuoteOPTemplateSectionExtendedPMService.GetMakeQuoteOPTemplateSectionsIncluded(this.QuoteOPPM.Id, this.selectedQuoteOPTemplate.Id, sectionViewModel.Id, SessionLocator.Tenant).subscribe((response:any) => {
            this.IsQuoteOPTemplateSectionInCludedChange = true;
          //  this.CurrentSession.CurrentWindow.StopBusyIndicator();
           // this.PreviewQuoteOPTemplatePdfReport();
        });
    }
    IsStartEditSession: boolean = false;
    OnEditSectionButtonClicked(item: QuoteOPTemplateSectionViewModel) {

        if (this.SelectedQuoteOPTemplate && !this.IsStartEditSession) {
            this.IsStartEditSession = true;
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var percentagewidthwindow = widthwindow * 0.252;
            var percentageHeightwindow = heighthwindow * 0.1764705;
            var sendWindowHeight = heighthwindow - percentageHeightwindow;
            var sendWindowWidth = widthwindow - percentagewidthwindow;
            if (sendWindowWidth < 1080) sendWindowWidth = 1080;
            if (sendWindowHeight < 600) sendWindowHeight = 600;

            var windowArgs: any = {};
            windowArgs.FatherComponent = this;
            windowArgs.HeightWindow = sendWindowHeight;
            windowArgs.QuoteOPTemplateSectionViewModel = item;
            windowArgs.QuoteTemplateId = this.SelectedQuoteOPTemplate.Id;

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = sendWindowWidth;
            logWindow.Height = sendWindowHeight;
            logWindow.Title = "Edit Section";
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddEditQuoteTemplateSectionComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    this.PreviewQuoteOPTemplatePdfReport();
                }
                this.IsStartEditSession = false;
            });
        }


    }

    AddQuoteOPTemplateFromLibraryClcik() {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.Type = "Quotation";
        windowArgs.QuoteOPId = this.QuoteOPPM.Id;
        windowArgs.QuoteTypeCode = this.QuoteOPPM.QuoteTypeCode;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;

        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.NewQuoteOPTemplate"); 
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
