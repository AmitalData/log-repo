"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var EditQuoteTemplateComponent_1 = require("../../../QuoteTemplates/Components/EditQuoteTemplateComponent");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var QuoteTemplateExtendedPMService_1 = require("../../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var QuoteTemplatePMService_1 = require("../../../../Quote/Services/StandardPMs/QuoteTemplatePMService");
var QuoteTemplateSettingPMService_1 = require("../../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var QuoteTemplateSectionExtendedPMService_1 = require("../../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var QuoteStageListService_1 = require("../../../../Quote/Services/StandardLists/QuoteStageListService");
var QuoteDocumentVersionPM_1 = require("../../../../Quote/EntityPMs/QuoteDocumentVersionPM");
var QuotePMService_1 = require("../../../../Quote/Services/StandardPMs/QuotePMService");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var FroalaEditorSetting_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting");
var GeneralEmailSender_1 = require("../../../../Infrastructure/Helpers/GeneralEmailSender");
var AttachmentsList_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList");
var DocumentTypeListService_1 = require("../../../../Common/Services/StandardLists/DocumentTypeListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var QuotationComponent = /** @class */ (function (_super) {
    __extends(QuotationComponent, _super);
    function QuotationComponent() {
        var _this = _super.call(this) || this;
        _this.IsDataReady = false;
        _this.QuoteTemplateSectionLists = [];
        _this.ReportVersions = [];
        _this.IsFileUploadedManually = false;
        _this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        _this._documentTypeListService = new DocumentTypeListService_1.DocumentTypeListService();
        _this.IsDisableEditQuoteTemplateButton = false;
        _this.IsShowFromLibraryLink = false;
        _this.IsShowPreviewPDF = true;
        _this.IsReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SendToCustomerEvent = null;
        _this.IsQuoteSent = false;
        _this.IsUploadEnabled = false;
        _this.IsEditEnabled = true;
        _this.IsPrintButtonEnabled = true;
        _this.IsEditButtonEnabled = true;
        _this.IsSendButtonEnabled = true;
        _this.HasNoTmplates = false;
        _this.allStages = [];
        _this.QuotationTemplates = [];
        _this.selectedMode = "";
        _this.DocumentTypesList = [];
        _this.IsShowTemplateList = false;
        _this.AttrTitleShowTemplateList = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.B.Expand");
        _this.IFrameURI = "";
        _this.IsUploadVisibile = false;
        _this.IsQuoteTemplateSectionInCludedChange = false;
        _this.IsStartEditSession = false;
        _this.Listen();
        _this.PreviewPdfId = Guid_1.Guid.newGuid();
        _this.UploadFileId = Guid_1.Guid.NewRandomString();
        _this.LoadService();
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("QuoteTemplate", "UPDATE"))
            _this.IsDisableEditQuoteTemplateButton = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("QuoteTemplate", "FROMLIBRARY")) {
            _this.IsShowFromLibraryLink = true;
        }
        return _this;
    }
    QuotationComponent.prototype.ngOnInit = function () {
    };
    QuotationComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.SendToCustomerEvent) {
            this.SendToCustomerEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "SendToCustomerCompleted") {
                    _this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
                    _this.currentDocumentVersion.IsSent = true;
                    _this.currentDocumentVersion.SendDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    var myCreateStage = _this.allStages.filter(function (d) { return d.Code == "QTCR"; })[0];
                    var myDraftStage = _this.allStages.filter(function (d) { return d.Code == "QTDR"; })[0];
                    //this.QuotePM.ActionType = "SentToCustomerFromQuotation";
                    if (_this.QuotePM.StageId == myDraftStage.Id || _this.QuotePM.StageId == myCreateStage.Id) {
                        _this.QuotePM.ActionType = "SetAsSentToCustomer";
                    }
                    //this.CurrentSession.CurrentEditComponent.EntityPM.ActionType = "SetAsSentToCustomer";
                    _this.quotePMService.update(_this.QuotePM).subscribe(function (response) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (myDraftStage != null) {
                            if (_this.QuotePM.StageId != myDraftStage.Id && _this.QuotePM.StageId != myCreateStage.Id) {
                                _this.IsEditEnabled = false;
                                _this.IsUploadEnabled = false;
                                _this.IsEditButtonEnabled = false;
                                _this.IsQuoteSent = true;
                            }
                        }
                        else {
                            _this.BuildDocumentVersion();
                        }
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    });
                }
            });
        }
    };
    QuotationComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SendToCustomerEvent);
    };
    QuotationComponent.prototype.LoadService = function () {
        this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService_1.QuoteTemplateExtendedPMService();
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService_1.QuoteTemplateSectionExtendedPMService();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        this.quoteTemplatePMService = new QuoteTemplatePMService_1.QuoteTemplatePMService();
        this.myQuoteStageListService = new QuoteStageListService_1.QuoteStageListService();
        this.quotePMService = new QuotePMService_1.QuotePMService();
    };
    QuotationComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var _entityResourceService = new EntityResourceService_1.EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteTemplate").subscribe(function (response) {
            _this.IsReady = true;
            _this.QuotePM = args.QuotePM;
            _this.QuoteTypeCode = _this.QuotePM.QuoteTypeCode;
            _this.QuotationWindow = args.QuotationWindow;
            _this.QuotationTitle = args.QuotationWindow ? args.QuotationWindow.Title : "";
            _this.LoadQuoteCustomerEmail();
            _this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
            _this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
            _this.froalaEditorSetting.IsDisableEdit = true;
            _this.froalaEditorSetting.HtmlString = "";
            _this.froalaEditorSetting.Height = (_this.CurrentSession.CurrentWindow.Height - 100);
            _this.HeightPdf = (_this.CurrentSession.CurrentWindow.Height - 100);
            _this.LoadData();
        });
    };
    Object.defineProperty(QuotationComponent.prototype, "SelectedMode", {
        get: function () {
            return this.selectedMode;
        },
        set: function (newValue) {
            if (this.selectedMode != newValue) {
                this.selectedMode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuotationComponent.prototype.SelectedModeClicked = function (mode) {
        var _this = this;
        this.SelectedMode = mode;
        if (this.SelectedMode === "Generate" && this.currentDocumentVersion != null && this.currentDocumentVersion.VersionType == "U") {
            if (this.QuotationTemplates.length > 0) {
                if (this.selectedQuoteTemplate == null) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.QuoteTemplateId)) {
                        this.SelectedQuoteTemplate = this.QuotationTemplates.filter(function (r) { return r.Id == _this.QuotePM.QuoteTemplateId; })[0];
                    }
                    else {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotationDefaultTemplateId)) {
                            this.SelectedQuoteTemplate = this.QuotationTemplates.filter(function (t) { return t.Id == _this.QuotationDefaultTemplateId; })[0];
                        }
                        else
                            this.SelectedQuoteTemplate = this.QuotationTemplates.filter(function (t) { return t.IsDefault == true; })[0];
                    }
                    if (this.SelectedQuoteTemplate == null) {
                        this.SelectedQuoteTemplate = this.QuotationTemplates[0];
                    }
                }
                // Show prompt
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Title = "Generate Document File";
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.Width = 500;
                confirmWindow.Show("Do you want to overwrite the uploaded file?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.currentDocumentVersion.VersionType = "G";
                        _this.currentDocumentVersion.VersionTypeName = "Generated";
                        _this.currentDocumentVersion.DisplayVersionTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.S.Generated");
                        _this.UpdateCurrentVersion();
                    }
                    else if (confirmWindow.No) {
                        _this.SelectedMode = "Upload";
                    }
                });
            }
            else {
                var window = new MessageWindow_1.MessageWindow();
                window.Show("There is not templates to generate");
            }
        }
    };
    QuotationComponent.prototype.LoadData = function () {
        //this.QuotePM.StageId
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.DocumentTypesList = pmResponse.Result;
            }
            _this.allStages = [];
            _this.myQuoteStageListService.getAllFromCache().subscribe(function (resp) {
                if (!resp.HasError) {
                    _this.allStages = resp.Result;
                }
                var myCreateStage = _this.allStages.filter(function (d) { return d.Code == "QTCR"; })[0];
                var myDraftStage = _this.allStages.filter(function (d) { return d.Code == "QTDR"; })[0];
                if (myDraftStage != null) {
                    if (_this.QuotePM.StageId != myDraftStage.Id && _this.QuotePM.StageId != myCreateStage.Id) {
                        _this.IsEditEnabled = false;
                        _this.IsUploadEnabled = false;
                        _this.IsEditButtonEnabled = false;
                        _this.IsQuoteSent = true;
                    }
                }
                _this.LoadProductType();
            });
        });
    };
    QuotationComponent.prototype.LoadTemplates = function (templateId) {
        var _this = this;
        if (templateId === void 0) { templateId = null; }
        this.quoteTemplateExtendedPMService.GetQuoteTemplateListsByQuoteTemplateTypeAndTenant(this.QuotePM.QuoteTypeCode, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!response.HasError) {
                _this.QuotationTemplates = response.Result;
                _this.TemplateListTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.S.Templates") + "(" + _this.QuotationTemplates.length + ")";
                //this.QuotationTemplates.length
                if (!Tools_1.AppTool.IsNullOrEmpty(templateId)) {
                    _this.SelectedQuoteTemplate = _this.QuotationTemplates.filter(function (t) { return t.Id == templateId; })[0];
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(_this.QuotePM.QuoteTemplateId)) {
                    _this.SelectedQuoteTemplate = _this.QuotationTemplates.filter(function (r) { return r.Id == _this.QuotePM.QuoteTemplateId; })[0];
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.QuotationDefaultTemplateId)) {
                        _this.SelectedQuoteTemplate = _this.QuotationTemplates.filter(function (t) { return t.Id == _this.QuotationDefaultTemplateId; })[0];
                    }
                    else
                        _this.SelectedQuoteTemplate = _this.QuotationTemplates.filter(function (t) { return t.IsDefault == true; })[0];
                }
                if (_this.SelectedQuoteTemplate == null) {
                    _this.SelectedQuoteTemplate = _this.QuotationTemplates[0];
                }
                if (_this.QuotationTemplates.length == 0) {
                    _this.IsPrintButtonEnabled = false;
                    _this.IsEditButtonEnabled = false;
                    _this.IsSendButtonEnabled = false;
                    //lstViewOption.IsEnabled = false;
                    //pdfViewer.Visibility = Visibility.Collapsed;
                    //blkMessage.Visibility = System.Windows.Visibility.Visible;
                    _this.HasNoTmplates = true;
                }
            }
            else {
                //ToDo: handle error
            }
        });
    };
    QuotationComponent.prototype.LoadProductType = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.ProductCode)) {
            var _productTypeListService = new ProductTypeListService_1.ProductTypeListService();
            _productTypeListService.getSingleFromCache(this.QuotePM.ProductCode).subscribe(function (result) {
                var productsList = result.Result;
                if (productsList) {
                    if (_this.QuoteTypeCode == "A")
                        _this.QuotationDefaultTemplateId = productsList.QuotationDefaultTemplateId;
                    else
                        _this.QuotationDefaultTemplateId = productsList.RoutingRQuoteDefaultTemplateId;
                }
                _this.LoadTemplates();
            });
        }
        else
            this.LoadTemplates();
    };
    QuotationComponent.prototype.ShowHideTemplateList = function () {
        if (this.IsShowTemplateList) {
            this.IsShowTemplateList = false;
            this.AttrTitleShowTemplateList = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.B.Expand");
        }
        else {
            this.IsShowTemplateList = true;
            this.AttrTitleShowTemplateList = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.B.Hide");
        }
        //var element = document.getElementById(this.AttachmentListId);
        //this.ComputeAttachmentListWidth();
        //if (this.AttachmentsLists.length > 4) {
        //    element.setAttribute("style", "height:60px;margin-left:5px;overflow-y:scroll;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        //}
        //else {
        //    element.setAttribute("style", "height:auto;margin-left:5px;overflow-x:auto;width:" + this.AreaAttachmentWidth);
        //}
    };
    Object.defineProperty(QuotationComponent.prototype, "SelectedQuoteTemplate", {
        get: function () { return this.selectedQuoteTemplate; },
        set: function (newValue) {
            if (this.selectedQuoteTemplate != newValue) {
                this.selectedQuoteTemplate = newValue;
                var title = this.QuotationTitle + (" (" + this.SelectedQuoteTemplate.Name + ")");
                if (this.QuotationWindow && this.QuotationWindow.Title != title) {
                    this.QuotationWindow.Title = title;
                }
                this.PreviewQuoteTemplatePdfReport();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuotationComponent.prototype.PreviewQuoteTemplatePdfReport = function (isGenerate) {
        var _this = this;
        if (isGenerate === void 0) { isGenerate = false; }
        var defultQuoteTemplateId = !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.QuoteTemplateId) ? this.QuotePM.QuoteTemplateId : "";
        var quotationSections = !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.QuotationSections) ? this.QuotePM.QuotationSections : "";
        if (isGenerate)
            defultQuoteTemplateId = "";
        this.quoteTemplateExtendedPMService.GetTemplateSectionsByQuoteTemplateIdAndQuoteId(this.selectedQuoteTemplate.Id, this.QuotePM.Id, SessionLocator_1.SessionLocator.Tenant, defultQuoteTemplateId, quotationSections).subscribe(function (response) {
            if (!response.HasError) {
                _this.selectedQuoteTemplate.TemplateSections = response.Result;
                _this.BuildingQuoteTemplateSectionsAndVersions(isGenerate);
            }
        });
    };
    QuotationComponent.prototype.BuildingQuoteTemplateSectionsAndVersions = function (isGenerate) {
        if (isGenerate === void 0) { isGenerate = false; }
        this.QuoteTemplateSectionLists = [];
        var PricingType = "";
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
                if (!Tools_1.AppTool.IsNullOrEmpty(PricingType)) {
                    if ((PricingType == "P" && item.QuoteTemplateSectionTypeCode === "PC")
                        || (PricingType == "C" && item.QuoteTemplateSectionTypeCode === "PP")) {
                        continue;
                    }
                }
                this.QuoteTemplateSectionLists.push(new EditQuoteTemplateComponent_1.QuoteTemplateSectionViewModel(item, this));
                // }
            }
            if (this.QuoteTemplateSectionLists && this.QuoteTemplateSectionLists.length > 0) {
                //this.SelectQuoteTemplateSection = this.QuoteTemplateSectionLists[0];
            }
        }
        var myCreateStage = this.allStages.filter(function (d) { return d.Code == "QTCR"; })[0];
        var myDraftStage = this.allStages.filter(function (d) { return d.Code == "QTDR"; })[0];
        this.ReportVersions = this.QuotePM.QuoteDocumentVersions.sort(function (a, b) { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1; });
        this.ComputeVersionTypeName();
        if (((!this.ReportVersions.some(function (v) { return v.IsSent == false; }) && (this.QuotePM.StageId == myCreateStage.Id || this.QuotePM.StageId == myDraftStage.Id)) || this.ReportVersions.length == 0)) {
            this.BuildDocumentVersion();
            this.SelectedMode = "Generate";
        }
        else {
            var orderdVersionsByDate = [];
            this.ReportVersions.forEach(function (item) {
                orderdVersionsByDate.push(item);
            });
            orderdVersionsByDate.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.UpdateDate) === Tools_1.DateTool.GetDateFromDate(b.UpdateDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.UpdateDate) > Tools_1.DateTool.GetDateFromDate(b.UpdateDate)) ? -1 : 1; });
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
    };
    QuotationComponent.prototype.UpdateCurrentVersion = function (isGenerate) {
        var _this = this;
        if (isGenerate === void 0) { isGenerate = false; }
        if (this.currentDocumentVersion != null && this.currentDocumentVersion.VersionType == "G") {
            var enableGenerate = true;
            var sentStage = this.allStages.filter(function (d) { return d.Code == "QTST"; })[0];
            var declinedStage = this.allStages.filter(function (d) { return d.Code == "QTDC"; })[0];
            var acceptedStage = this.allStages.filter(function (d) { return d.Code == "QTAC"; })[0];
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
                    this.SelectedQuoteTemplate.TemplateSections.forEach(function (section) {
                        templateIds += (section.Id + ",");
                    });
                    if (templateIds.length > 0) {
                        templateIds = templateIds.substring(0, templateIds.length - 1);
                    }
                    this.QuotePM.QuotationSections = templateIds;
                    this.QuotePM.LastVersionNumber = this.currentDocumentVersion.VersionNumber;
                }
                this.quoteTemplateExtendedPMService.GetUpdatedQuoteDocumentVersion(this.currentDocumentVersion.QuoteId, this.currentDocumentVersion.VersionNumber, this.SelectedQuoteTemplate.Id, SessionLocator_1.SessionLocator.LoggedUserId, this.currentDocumentVersion.Tenant, isGenerate).subscribe(function (response) {
                    var pmResponse = response;
                    _this.CurrentSession.StopBusyIndicator();
                    if (!pmResponse.HasError && pmResponse.Result) {
                        var buffer = EntityResourceService_1.EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                        var blob = new Blob([buffer], { type: 'application/pdf' });
                        var objectURL = URL.createObjectURL(blob);
                        _this.IFrameURI = objectURL;
                        _this.IsQuoteTemplateSectionInCludedChange = false;
                        _this.RefreshVersions();
                        _this.ClearLastQuoteTemplateVersionDocuemnt();
                    }
                });
            }
            else {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
                this.GetQuoteTemplatePdf();
            }
        }
    };
    QuotationComponent.prototype.BuildDocumentVersion = function () {
        var _this = this;
        if (this.SelectedQuoteTemplate != null) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Generating....");
            var quoteDocumentVersion = new QuoteDocumentVersionPM_1.QuoteDocumentVersionPM(this.QuotePM);
            quoteDocumentVersion.Tenant = SessionLocator_1.SessionLocator.Tenant;
            quoteDocumentVersion.QuoteId = this.QuotePM.Id;
            quoteDocumentVersion.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            quoteDocumentVersion.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            quoteDocumentVersion.VersionType = "G";
            quoteDocumentVersion.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            quoteDocumentVersion.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.UpdateByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.CreatedByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.VersionNumber = -1;
            quoteDocumentVersion.VersionTypeName = "Generated";
            quoteDocumentVersion.DisplayVersionTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.S.Generated");
            this.QuotePM.AddQuoteDocumentVersionPM(quoteDocumentVersion);
            var myDraftStage = this.allStages.filter(function (d) { return d.Code == "QTDR"; })[0];
            if (myDraftStage != null && !this.IsQuoteSent) {
                this.QuotePM.StageId = myDraftStage.Id;
                this.QuotePM.StageName = myDraftStage.Name;
            }
            this.QuotePM.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            var templateIds = "";
            this.SelectedQuoteTemplate.TemplateSections.forEach(function (section) {
                templateIds += (section.Id + ",");
            });
            if (templateIds.length > 0) {
                templateIds = templateIds.substring(0, templateIds.length - 1);
            }
            this.QuotePM.QuotationSections = templateIds;
            this.quotePMService.update(this.QuotePM).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    _this.ReportVersions = _this.QuotePM.QuoteDocumentVersions.sort(function (a, b) { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1; });
                    _this.ComputeVersionTypeName();
                    _this.currentDocumentVersion = _this.ReportVersions.filter(function (v) { return v.IsSent == false; })[0];
                    _this.ClearLastQuoteTemplateVersionDocuemnt();
                    _this.GetQuoteTemplatePdf();
                }
                else { }
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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
    };
    QuotationComponent.prototype.ClearLastQuoteTemplateVersionDocuemnt = function () {
        if (this.SelectedQuoteTemplate.IsLastQuoteTemplateDocumentVersion != true) {
            var lastVersions = this.QuotationTemplates.filter(function (d) { return d.IsLastQuoteTemplateDocumentVersion == true; });
            lastVersions.forEach(function (list) {
                list.IsLastQuoteTemplateDocumentVersion = false;
            });
            this.SelectedQuoteTemplate.IsLastQuoteTemplateDocumentVersion = true;
        }
    };
    QuotationComponent.prototype.RefreshVersions = function () {
        var _this = this;
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.quoteTemplateExtendedPMService.GetQuoteDocumentVersionsByQuoteId(this.QuotePM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            var pmResponse = response;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.ReportVersions = pmResponse.Result;
                _this.ComputeVersionTypeName();
            }
        });
        // LoadOperation loadVersions = quotesContext.Load(quotesContext.GetQuoteDocumentVersionsByQuoteIdQuery(this.QuotePM.Id, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, false);
        // loadVersions.Completed += loadVersions_Completed;
    };
    QuotationComponent.prototype.OnPrint = function () {
        if (this.currentDocumentVersion != null) {
            this.OpenDocumentVersion(this.currentDocumentVersion);
        }
    };
    QuotationComponent.prototype.ComputeVersionTypeName = function () {
        this.ReportVersions.forEach(function (item) {
            item.DisplayVersionTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.S." + item.VersionTypeName);
        });
    };
    QuotationComponent.prototype.GetQuoteTemplatePdf = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        this.quoteTemplateSectionExtendedPMService.GetQuoteTemplatePdfReport(this.QuotePM.Id, this.SelectedQuoteTemplate.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                var buffer = EntityResourceService_1.EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                var blob = new Blob([buffer], { type: 'application/pdf' });
                var objectURL = URL.createObjectURL(blob);
                _this.IFrameURI = objectURL;
                _this.IsQuoteTemplateSectionInCludedChange = false;
            }
        });
    };
    QuotationComponent.prototype.OpenDocumentVersion = function (item) {
        if (item != null) {
            DownloadManager_1.DownloadManager.DownloadPage(item.DocumentId);
        }
    };
    QuotationComponent.prototype.OnUploadQuote = function () {
        document.getElementById(this.UploadFileId).click();
    };
    QuotationComponent.prototype.UploadFile = function (event) {
        var _this = this;
        var file = attachmentUploader(this.UploadFileId);
        //document.querySelector('#UploadFile').files[0];
        if (file && file.size > 0) {
            this.VersionFileDataParam = new VersionFileData();
            this.FileName = file.name;
            this.FileExtension = file.name.split('.')[1];
            this._documentsFilingExtendedPMService.GetFileSizeAndUnit(file.size).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.FileSize = myResult;
                    }
                }
                if (_this.FileExtension && _this.FileExtension.length > 10) {
                    _this.ShowMessage("File extension should be less than or equal 10 characters");
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
                    _this.File = file;
                    //var filebuffer = null;
                    //this.filterImageParameter.PartsNumber = this.File.size / 100000;
                    //if (this.filterImageParameter.PartsNumber > 1) filebuffer = this.File.slice(0, 100000);
                    //else filebuffer = this.File.slice(0, file.size);
                    _this.VersionFileDataParam.FileExtension = _this.FileExtension;
                    _this.VersionFileDataParam.QuoteId = _this.QuotePM.Id;
                    _this.VersionFileDataParam.UpdatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                    _this.VersionFileDataParam.VersionNumber = _this.currentDocumentVersion.VersionNumber;
                    //this.filterImageParameter.FileSize = file.size;
                    //this.filterImageParameter.SendPartNumber = 1;
                    //this.filterImageParameter.BufferNumber = -1;
                    //this.filterImageParameter.SentSize = 0;
                    //this.filterImageParameter.IsFirstTry = true;
                    //this.filterImageParameter.FileName = this.FileName;
                    var filebuffer = _this.File.slice(0, file.size);
                    _this.ArrayBufferToBase64(filebuffer, _this);
                }
            });
        }
    };
    QuotationComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    QuotationComponent.prototype.BuildFileVersion = function () {
        var _this = this;
        if (this.currentDocumentVersion != null) {
            this.UploadFileToServer();
        }
        else {
            var quoteDocumentVersion = new QuoteDocumentVersionPM_1.QuoteDocumentVersionPM(this.QuotePM);
            quoteDocumentVersion.Tenant = SessionLocator_1.SessionLocator.Tenant;
            quoteDocumentVersion.QuoteId = this.QuotePM.Id;
            quoteDocumentVersion.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            quoteDocumentVersion.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            quoteDocumentVersion.VersionType = "U";
            quoteDocumentVersion.QuoteTemplateId = null;
            quoteDocumentVersion.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.UpdateByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.CreatedByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
            quoteDocumentVersion.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            quoteDocumentVersion.VersionNumber = 1;
            quoteDocumentVersion.VersionTypeName = "Uploaded";
            quoteDocumentVersion.DisplayVersionTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.S.Uploaded");
            this.QuotePM.AddQuoteDocumentVersionPM(quoteDocumentVersion);
            var myDraftStage = this.allStages.filter(function (d) { return d.Code == "QTDR"; })[0];
            if (myDraftStage != null && !this.IsQuoteSent) {
                this.QuotePM.StageId = myDraftStage.Id;
                this.QuotePM.StageName = myDraftStage.Name;
            }
            this.QuotePM.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            this.quotePMService.update(this.QuotePM).subscribe(function (response) {
                if (!response.HasError) {
                    //this.ReportVersions = this.QuotePM.QuoteDocumentVersions.sort((a, b) => { return (a.VersionNumber === a.VersionNumber) ? 0 : (a.VersionNumber < a.VersionNumber) ? -1 : 1 });
                    _this.currentDocumentVersion = quoteDocumentVersion; //this.ReportVersions.filter(v => v.IsSent == false)[0];
                    _this.UploadFileToServer();
                }
                else {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                }
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            // this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    QuotationComponent.prototype.UploadFileToServer = function () {
        var _this = this;
        this.quoteTemplateExtendedPMService.UpLoadQuoteDocumentVersionFile(this.VersionFileDataParam, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            if (!response.HasError) {
                _this.currentDocumentVersion.VersionType = "U";
                _this.currentDocumentVersion.VersionTypeName = "Upload";
                _this.currentDocumentVersion.DisplayVersionTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.Quotation.S.Upload");
                //busyIndicator.Visibility = Visibility.Collapsed;
                //txtMessage.Text = "File uploaded manually, no overview is available.";
                //txtMessage.Visibility = Visibility.Visible;
                _this.IsFileUploadedManually = true;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.RefreshVersions();
            }
        });
    };
    QuotationComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
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
    };
    QuotationComponent.prototype.LoadQuoteCustomerEmail = function () {
        var _this = this;
        if (this.QuotePM) {
            this.quoteTemplateExtendedPMService.GetQuoteCustomerEmailByContactId(this.QuotePM.CustomerContactId).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    _this.QuoteCustomerEmail = response.Result;
                }
            });
        }
    };
    QuotationComponent.prototype.OnSendToCustomer = function (sendtype) {
        if (this.currentDocumentVersion) {
            var documentType = this.DocumentTypesList.filter(function (d) { return d.Code === "QUOTE"; })[0];
            var eventRefreshName = sendtype == "Send to Customer" ? "SendToCustomerCompleted" : "";
            var fileName = "Quotation-" + this.QuotePM.QuoteNumber + "-" + this.currentDocumentVersion.VersionNumber;
            var attachment = new AttachmentsList_1.AttachmentsList();
            attachment.Tenant = SessionLocator_1.SessionLocator.Tenant;
            attachment.DocumentTypeCopyNameWithDocumentTypeName = fileName;
            attachment.FileSize = this.currentDocumentVersion.FileSize;
            attachment.ShowRemoveLink = true;
            attachment.Id = this.currentDocumentVersion.DocumentId;
            var attachmentsList = new Array();
            attachmentsList.push(attachment);
            if (!this.GeneralEmailSender || (this.GeneralEmailSender && !this.GeneralEmailSender.LoadingSendingComponent)) {
                this.GeneralEmailSender = new GeneralEmailSender_1.GeneralEmailSender("Quote", "QUOTE", this.QuotePM.Id, this.QuotePM.QuoteNumber, this.QuotePM.CustomerId, "", "", this.SelectedQuoteTemplate.Name, attachmentsList, eventRefreshName, this.QuotePM, false, "QEMO");
                if (sendtype == "Send to Customer") {
                    this.GeneralEmailSender.ToSpecificeEmail = this.QuoteCustomerEmail;
                }
                this.GeneralEmailSender.ExportQuotationsToIntegratedSystem = SessionLocator_1.SessionLocator.TenantPM.ExportQuotationsToIntegratedSystem && !this.QuotePM.IsQuoteDataExternal && !this.QuotePM.IsQuoteDataExternal ? true : false;
                this.GeneralEmailSender.ShowFullSendControll();
            }
        }
    };
    QuotationComponent.prototype.OnEditQuotationTemplate = function () {
        var _this = this;
        if (this.selectedQuoteTemplate != null) {
            this.IsShowPreviewPDF = false;
            var windowArgs = {};
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            windowArgs.IsNewEntityCall = false;
            windowArgs.CurrentEntity = this.selectedQuoteTemplate;
            windowArgs.QuotePM = this.QuotePM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = this.selectedQuoteTemplate.Name;
            logWindow.Width = window.innerWidth - 150;
            logWindow.Height = window.innerHeight - 150;
            logWindow.IsShowCloseButton = true;
            logWindow.DataContext = this;
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");
            logWindow.WindowClosed.subscribe(function ($event1) {
                _this.IsShowPreviewPDF = true;
                if ($event1 === "SavedChanges") {
                    _this.PreviewQuoteTemplatePdfReport(true);
                }
            });
        }
    };
    QuotationComponent.prototype.ExcludeSection = function (sectionViewModel) {
        var _this = this;
        // this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.quoteTemplateSectionExtendedPMService.GetMakeQuoteTemplateSectionsExcluded(this.QuotePM.Id, this.selectedQuoteTemplate.Id, sectionViewModel.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            _this.IsQuoteTemplateSectionInCludedChange = true;
            //  this.CurrentSession.CurrentWindow.StopBusyIndicator();
            //this.PreviewQuoteTemplatePdfReport();
        });
    };
    QuotationComponent.prototype.IncludeSection = function (sectionViewModel) {
        var _this = this;
        // this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.quoteTemplateSectionExtendedPMService.GetMakeQuoteTemplateSectionsIncluded(this.QuotePM.Id, this.selectedQuoteTemplate.Id, sectionViewModel.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            _this.IsQuoteTemplateSectionInCludedChange = true;
            //  this.CurrentSession.CurrentWindow.StopBusyIndicator();
            // this.PreviewQuoteTemplatePdfReport();
        });
    };
    QuotationComponent.prototype.OnEditSectionButtonClicked = function (item) {
        var _this = this;
        if (this.SelectedQuoteTemplate && !this.IsStartEditSession) {
            this.IsStartEditSession = true;
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var percentagewidthwindow = widthwindow * 0.252;
            var percentageHeightwindow = heighthwindow * 0.1764705;
            var sendWindowHeight = heighthwindow - percentageHeightwindow;
            var sendWindowWidth = widthwindow - percentagewidthwindow;
            if (sendWindowWidth < 1000)
                sendWindowWidth = 1000;
            if (sendWindowHeight < 600)
                sendWindowHeight = 600;
            var windowArgs = {};
            windowArgs.FatherComponent = this;
            windowArgs.HeightWindow = sendWindowHeight;
            windowArgs.QuoteTemplateSectionViewModel = item;
            windowArgs.QuoteTemplateId = this.SelectedQuoteTemplate.Id;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = sendWindowWidth;
            logWindow.Height = sendWindowHeight;
            logWindow.Title = "Edit Section";
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddEditQuoteTemplateSectionComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "Refresh") {
                    _this.PreviewQuoteTemplatePdfReport();
                }
                _this.IsStartEditSession = false;
            });
        }
    };
    QuotationComponent.prototype.AddQuoteTemplateFromLibraryClcik = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.Type = "Quotation";
        windowArgs.QuoteId = this.QuotePM.Id;
        windowArgs.QuoteTypeCode = this.QuotePM.QuoteTypeCode;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.NewQuoteTemplate");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddQuoteTemplateFromLibraryComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                _this.LoadTemplates($event);
            }
        });
    };
    QuotationComponent = __decorate([
        core_1.Component({
            selector: 'QuotationComponent',
            moduleId: module.id,
            templateUrl: './QuotationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuotationComponent);
    return QuotationComponent;
}(BaseComponent_1.BaseComponent));
exports.QuotationComponent = QuotationComponent;
var VersionFileData = /** @class */ (function () {
    function VersionFileData() {
    }
    return VersionFileData;
}());
exports.VersionFileData = VersionFileData;
//# sourceMappingURL=QuotationComponent.js.map