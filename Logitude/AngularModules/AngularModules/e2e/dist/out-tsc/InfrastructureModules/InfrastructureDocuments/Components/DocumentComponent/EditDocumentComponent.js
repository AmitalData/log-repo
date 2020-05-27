"use strict";
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
var core_2 = require("@angular/core");
var DocumentOutPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var StimulsoftArg_1 = require("./StimulsoftArg");
var HtmlEditorService_1 = require("../../../../Common/Services/DocumentServices/HtmlEditorService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DocumentTypeTemplateListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ExportDocumentArgs_1 = require("../../../../Infrastructure/DataContracts/ExportDocumentArgs");
var FroalaEditorFilters_1 = require("./DocsOut/Filters/FroalaEditorFilters");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var FroalaEditorSetting_1 = require("./DocsOut/FroalaEditorSetting");
var DocumentTypeTemplateViewModel_1 = require("./DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var DocumentCustomFieldsArgs_1 = require("./DocsOut/Filters/DocumentCustomFieldsArgs");
var ExportDocumentService_1 = require("../../../../Common/Services/DocumentServices/ExportDocumentService");
var DocumentTypeTemplateFilter_1 = require("./DocsOut/Filters/DocumentTypeTemplateFilter");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var EditDocumentComponent = /** @class */ (function () {
    function EditDocumentComponent(_documentTypePMService, _exportDocumentService, _htmlEditorService, _documentTypeTemplateListExtendedService, _documentTypeTemplatePMExtendedService, cd, _documentOutPMService) {
        this._documentTypePMService = _documentTypePMService;
        this._exportDocumentService = _exportDocumentService;
        this._htmlEditorService = _htmlEditorService;
        this._documentTypeTemplateListExtendedService = _documentTypeTemplateListExtendedService;
        this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        this.cd = cd;
        this._documentOutPMService = _documentOutPMService;
        this.IsDisableAddTemplateFromLibrary = false;
        this.DocumentTypeTemplateId = null;
        this.IsDisplayToggleButtonMenu = false;
        this.IsShowFroalaEditor = false;
        this.DocumentTemplateFileId = Guid_1.Guid.NewRandomString();
        this.HeaderHtml = "";
        this.FooterHtml = "";
        this.BodyHtml = "";
        this.IsEditManageTemplate = false;
        this.OnHeaderAndFooterCompleteEvent = new core_1.EventEmitter();
        this.IsDisplayOnly = true;
        this.CurrentDocumentOutId = null;
        this.DocumenttypeCode = null;
        this.DocumentTypeCopyId = "";
        this.OldHtml = "";
        this.IsOpenHeaderAndFooter = false;
        this.IsShowEditHtml = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoadCompletedEvent = null;
        this.IsCloseViewHeaderAndFooter = false;
        this.IsDownLoadButtonClick = false;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
        this.ShowInactiveCheckBoxKey = Guid_1.Guid.newGuid();
        this.entityPMService = new EntityPMService_1.EntityPMService();
    }
    EditDocumentComponent.prototype.ngOnInit = function () {
        this.DocumentTypeTemplatePMLists = new Array();
        this.IsCheckedInActive = false;
    };
    EditDocumentComponent.prototype.Run = function () {
        var _this = this;
        this.SubjectId = Guid_1.Guid.newGuid();
        if (this.CurrentDocument && this.DocumentTemplateEditorTool) {
            if (this.DocumentTemplateEditorTool == "S") {
                this.stimulsoftArg = new StimulsoftArg_1.StimulsoftArg();
                this.stimulsoftArg.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
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
                if (this.DocumentTemplateEditorTool == "S")
                    FeatureName = "MRTPDFPRINT";
                else
                    FeatureName = "RICHTEXTPRINT";
                if (this.IsManageStimul && FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "UPDATE") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", FeatureName)) {
                    this.IsShowEditStimual = true;
                }
            }
            else if (this.DocumentTemplateEditorTool == "S" && this.PageType == "AdditionalPrintingFields") {
                this.IsSystemAdditionalPrintingFields = this.DocumentTypePM.IsSystemAdditionalPrintingFields;
                this.PrintingFieldsScreenCode = this.DocumentTypePM.PrintingFieldsScreenCode;
                this.stimulsoftArg.ScreenHeight = this.WindowHeight - 105;
                this.stimulsoftArg.ScreenWidth = this.WindowWidth - 390;
                this.DocumentCustomFieldsArgs = new DocumentCustomFieldsArgs_1.DocumentCustomFieldsArgs();
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
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.PrintingFieldsScreenCode) && this.DataViewModel) {
                        this.DocumentCustomFieldsArgs.ScreenCode = this.PrintingFieldsScreenCode;
                        this.DocumentCustomFieldsArgs.EntityPM = this.DataViewModel.DataContext.EntityPM;
                        this.DocumentCustomFieldsArgs.ObjectTableName = this.DataViewModel.DataContext.ObjectTableName;
                        this.IsAdditionalPrintingStimula = true;
                    }
                }
            }
            else if (this.DocumentTemplateEditorTool == "R") {
                this.IsShowSaveAndCancelButton = true;
                this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
                this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
                this.froalaEditorSetting.PageType = "Edit";
                this.IsShowFroalaEditor = true;
                if (this.PageType == "EditDocument") {
                    this.IsShowTemplateList = false;
                    this.froalaEditorSetting.Height = this.WindowHeight - 210;
                    this.IsEditHtml = true;
                    this.froalaEditorSetting.IsDisableEdit = false;
                }
                else if (this.PageType == "ManageTemplate") {
                    this.froalaEditorSetting.IsDisableEdit = true;
                    this.froalaEditorSetting.Height = this.WindowHeight - 130; //window.innerHeight - 300;
                    this.IsShowTemplateList = true;
                    this.IsManageHtml = true;
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "UPDATE") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "HTMLEMAIL")) {
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
                    this._exportDocumentService.DownloadFileFromServer(this.XamlDocumentId, this.Tenant).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                _this.HeaderHtml = myResult[0];
                                _this.BodyHtml = myResult[1];
                                _this.FooterHtml = myResult[2];
                                _this.HeaderHeight = myResult[3];
                                _this.FooterHeight = myResult[4];
                                if (!_this.IsEditHtml) {
                                    _this.BodyHtml = _this.HeaderHtml + _this.BodyHtml + _this.FooterHtml;
                                }
                                _this.froalaEditorSetting.froalaEditorComponent.SetHtml(_this.BodyHtml);
                                _this.OldHtml = _this.froalaEditorSetting.froalaEditorComponent.getHtml();
                                _this.ReloadFroalaEditor();
                                if (_this.IsEditHtml) {
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 230;
                                    logWindow.Height = 85;
                                    logWindow.Title = "";
                                    logWindow.IsHideWindowMargin = true;
                                    logWindow.IsHideHeader = true;
                                    logWindow.DataContext = "";
                                    logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SimplogInfoPopupComponent');
                                    logWindow.WindowClosed.subscribe(function ($event) {
                                        if ($event == "Regenerate") {
                                            _this.LoadHtmlTemplateData(null, "Edit");
                                        }
                                    });
                                }
                            }
                        }
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
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
        else
            this.IsShowSaveAndCancelButton = true;
    };
    EditDocumentComponent.prototype.LoadHtmlTemplateData = function (templateId, mode) {
        var _this = this;
        if (mode === void 0) { mode = null; }
        if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedDocumentTypeTemplateViewModel.HtmlData);
            this.ReloadFroalaEditor();
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            var docoutId = this.CurrentDocumentOutId;
            if (templateId)
                docoutId = "";
            this._htmlEditorService.getEditorHtmlData(docoutId, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId, false, templateId, "", mode).subscribe(function (res) {
                var htmlresult = "";
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        htmlresult = myResult.Htmlstring;
                        _this.Subject = myResult.Subject;
                        if (mode == "Edit") {
                            _this.HeaderHeight = myResult.HeaderHeight;
                            _this.HeaderHtml = myResult.HeaderHtml;
                            _this.FooterHeight = myResult.FooterHeight;
                            _this.FooterHtml = myResult.FooterHtml;
                        }
                        if (_this.SelectedDocumentTypeTemplateViewModel != null) {
                            _this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                            _this.SelectedDocumentTypeTemplateViewModel.HtmlData = htmlresult;
                        }
                        _this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlresult);
                        _this.ReloadFroalaEditor();
                    }
                }
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
        }
    };
    EditDocumentComponent.prototype.CloseButtonClicked = function () {
        var _this = this;
        var isClose = true;
        if (this.IsAdditionalPrintingStimula) {
            if (this.DocumentCustomFieldsArgs && this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent && this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent.HasError) {
                isClose = false;
                this.entityPMService.getSingle(this.DocumentCustomFieldsArgs.ObjectTableName, this.EntityId).then(function (response) {
                    response.subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            _this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent.CustomFieldLists.forEach(function (field) {
                                _this.DocumentCustomFieldsArgs.EntityPM[field.FieldName] = pmResponse.Result[field.FieldName];
                            });
                            _this.DocumentCustomFieldsArgs.GeneratedDocumentCustomFieldComponent.HasError = false;
                            _this.DocumentCustomFieldsArgs.EntityPM.IsDirty = false;
                        }
                        _this.DataViewModel.LoadDocumentCustomFields();
                        if (_this.CurrentSession.CurrentWindow) {
                            _this.CurrentSession.CurrentWindow.Close("");
                        }
                    });
                });
            }
            else
                this.DataViewModel.LoadDocumentCustomFields();
        }
        if (this.IsEditHtml || this.IsManageHtml)
            this.DestroyfroalaEditor();
        if (isClose) {
            this.CurrentSession.CurrentWindow.Close("");
        }
    };
    EditDocumentComponent.prototype.CancelButtonClicked = function () {
        this.DataViewModel.IsRefreshPrintConrol = false;
        this.CloseButtonClicked();
    };
    EditDocumentComponent.prototype.LoadstimulData = function (documenttypetemplateId, isDisplayOnly, isloadingtemplate, pagenumber, processType, reportKey, messageIndicator, iscloseWindow) {
        var _this = this;
        if (messageIndicator === void 0) { messageIndicator = null; }
        if (iscloseWindow === void 0) { iscloseWindow = false; }
        if (this.SelectedDocumentTypeTemplateViewModel != null && this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
            this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
        }
        if (messageIndicator) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(messageIndicator);
        }
        else
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        if (!documenttypetemplateId) {
            documenttypetemplateId = this.DocumentTypeTemplateId;
            this.stimulsoftArg.DocumenttypetemplateId = documenttypetemplateId;
        }
        var exportDocumentArgs = new ExportDocumentArgs_1.ExportDocumentArgs();
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
        exportDocumentArgs.LoggedContactId = SessionInfo_1.SessionInfo.LoggedUserId;
        exportDocumentArgs.ChildEntityId = this.ChildEntityId;
        exportDocumentArgs.ChildObjectTableId = this.ChildObjectTableId;
        exportDocumentArgs.LoggedContactName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        exportDocumentArgs.AccountingCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this._exportDocumentService.PostReportStimulsoftViewer(exportDocumentArgs).subscribe(function (res) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.stimulsoftArg.EditableFieldLists = myResult;
                    if (_this.SelectedDocumentTypeTemplateViewModel != null && pagenumber == 1) {
                        _this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                        _this.SelectedDocumentTypeTemplateViewModel.StimulData = myResult;
                    }
                    if (iscloseWindow) {
                        _this.CloseButtonClicked();
                    }
                    else {
                        _this.ReloadStimulsoftViewer();
                    }
                }
            }
        });
    };
    EditDocumentComponent.prototype.ViewHeaderAndFooter = function (editType) {
        var _this = this;
        this.IsOpenHeaderAndFooter = true;
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
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
            this.LoadCompletedEvent = this.OnHeaderAndFooterCompleteEvent.subscribe(function ($event) {
                if ($event.PageType == "Header") {
                    _this.HeaderHtml = $event.HtmlString;
                    _this.HeaderHeight = $event.HeightValue;
                }
                else if ($event.PageType == "Footer") {
                    _this.FooterHtml = $event.HtmlString;
                    _this.FooterHeight = $event.HeightValue;
                }
                if (_this.LoadCompletedEvent) {
                    _this.LoadCompletedEvent.unsubscribe();
                    _this.LoadCompletedEvent = null;
                }
            });
        }
    };
    EditDocumentComponent.prototype.LoadDocumentTypeTemplates = function (selectId) {
        var _this = this;
        this.ReportTemplates = new Array();
        this.DocumenttypetemplateLists = new Array();
        this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.DocumentTypeId, this.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        if (_this.DocumentTemplateEditorTool == "R") {
                            if (item.DocumentTypeId == _this.DocumentTypeId && item.TemplateType == "P" && _this.TemplateFormatCode == "P" && item.EditorTool == "R") {
                                var template = new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item);
                                if (!item.InActive || item.IsDefault || item.Id == _this.DocumentTypeTemplateId) {
                                    _this.ReportTemplates.push(template);
                                }
                                _this.DocumenttypetemplateLists.push(template);
                            }
                        }
                        else {
                            if (_this.DocumentTemplateEditorTool == "S") {
                                if (item.EditorTool == _this.DocumentTemplateEditorTool && item.DocumentTypeId == _this.DocumentTypeId && item.TemplateType == "P") {
                                    var template = new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item);
                                    if (!item.InActive || item.IsDefault || (item.Id == _this.DocumentTypeTemplateId)) {
                                        _this.ReportTemplates.push(template);
                                    }
                                    _this.DocumenttypetemplateLists.push(template);
                                }
                            }
                        }
                    });
                    var count = _this.ReportTemplates ? _this.ReportTemplates.length.toString() : "0";
                    _this.TitleList = "Templates " + "( " + count + " )";
                    if (_this.ReportTemplates && _this.ReportTemplates.length > 0) {
                        if (!selectId) {
                            _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (r) { return r.Id == _this.DocumentTypeTemplateId; })[0];
                        }
                        else {
                            _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (r) { return r.Id == selectId; })[0];
                        }
                    }
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    EditDocumentComponent.prototype.OnSelectTemplateChange = function (selectedItem) {
        var _this = this;
        this.IsEditManageTemplate = true;
        if (selectedItem != this.SelectedDocumentTypeTemplateViewModel) {
            this.CurrentDocument.DocumentTemplateId = selectedItem.Id;
            this.SelectedDocumentTypeTemplateViewModel = selectedItem;
            if (this.SelectedDocumentTypeTemplateViewModel.IsLoad) {
                if (this.IsManageHtml) {
                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.SelectedDocumentTypeTemplateViewModel.HtmlData);
                    this.ReloadFroalaEditor();
                }
                else if (this.IsManageStimul) {
                    this.stimulsoftArg.NumberOfPage = 1;
                    this.stimulsoftArg.DocumenttypetemplateId = selectedItem.Id;
                    this.stimulsoftArg.EditableFieldLists = this.SelectedDocumentTypeTemplateViewModel.StimulData;
                    this.ReloadStimulsoftViewer();
                }
            }
            else {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
                if (this.IsManageHtml) {
                    this._htmlEditorService.getEditorHtmlData("", this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId, false, selectedItem.Id, "").subscribe(function (res) {
                        var htmlresult = "";
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                htmlresult = myResult.Htmlstring;
                                _this.Subject = myResult.Subject;
                                _this.SelectedDocumentTypeTemplateViewModel.HtmlData = htmlresult;
                                _this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                                _this.froalaEditorSetting.froalaEditorComponent.SetHtml(htmlresult);
                                _this.ReloadFroalaEditor();
                            }
                        }
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    });
                }
                else if (this.IsManageStimul) {
                    this.stimulsoftArg.NumberOfPage = 1;
                    this.stimulsoftArg.DocumenttypetemplateId = selectedItem.Id;
                    this.stimulsoftArg.ReportKey = "";
                    var exportDocumentArgs = new ExportDocumentArgs_1.ExportDocumentArgs();
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
                    exportDocumentArgs.LoggedContactId = SessionInfo_1.SessionInfo.LoggedUserId;
                    exportDocumentArgs.ChildEntityId = this.ChildEntityId;
                    exportDocumentArgs.ChildObjectTableId = this.ChildObjectTableId;
                    exportDocumentArgs.LoggedContactName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                    exportDocumentArgs.AccountingCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
                    this._exportDocumentService.PostReportStimulsoftViewer(exportDocumentArgs).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                _this.SelectedDocumentTypeTemplateViewModel.StimulData = myResult;
                                _this.stimulsoftArg.EditableFieldLists = myResult;
                                _this.ReloadStimulsoftViewer();
                                _this.SelectedDocumentTypeTemplateViewModel.IsLoad = true;
                            }
                        }
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    });
                }
            }
        }
    };
    EditDocumentComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var m = this.viewContainerRef;
        var item = null;
        var isNewTemplate = false;
        if (this.DataViewModel && this.ReportTemplates && this.CurrentDocument) {
            this.DataViewModel.DocumentTypeTemplateLists = this.ReportTemplates.filter(function (d) { return d.InActive == false || d.Id == _this.CurrentDocument.DocumentTemplateId || d.Id == _this.CurrentDocument.EmailTemplateId; });
        }
        if (this.DataViewModel && this.DataViewModel.DocumentTypeTemplateLists) {
            item = this.DataViewModel.DocumentTypeTemplateLists.filter(function (d) { return d.Id == _this.CurrentDocument.DocumentTemplateId; })[0];
        }
        if (this.IsEditHtml || this.IsManageHtml) {
            if (this.IsManageHtml) {
                if (!item) {
                    isNewTemplate = true;
                    item = this.DocumenttypetemplateLists.filter(function (d) { return d.Id == _this.CurrentDocument.DocumentTemplateId; })[0];
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
                if (isNewTemplate == true)
                    this.DataViewModel.DocumentTypeTemplateLists.push(item);
                this.DataViewModel.CurrentDocumentTypeTemplateList = item;
                if (this.IsManageHtml) {
                    item.HtmlResolve = "";
                }
            }
            this.DataViewModel.IsRefreshPrintConrol = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe(function (res) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.CloseButtonClicked();
            });
        }
        else if (this.IsEditStimul || this.IsManageStimul) {
            if (this.ModePage != "StimaulEdit") {
                if (item) {
                    this.DataViewModel.CurrentDocumentTypeTemplateList = item;
                    this.DataViewModel.IsRefreshPrintConrol = true;
                }
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                this._documentOutPMService.putDocumentOut(this.CurrentDocument).subscribe(function (res) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (_this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift()) {
                        _this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
                    }
                    else
                        _this.CloseButtonClicked();
                });
            }
            else {
                this.SaveEditFeild();
            }
        }
        else if (this.IsAdditionalPrintingStimula) {
            if (this.DataViewModel && this.DocumentCustomFieldsArgs && this.DocumentCustomFieldsArgs.IsEditCustomField)
                this.DataViewModel.IsRefreshPrintConrol = true;
            if (this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift()) {
                this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
            }
            else if (!this.DocumentCustomFieldsArgs.IsChangeCustomField) {
                this.CloseButtonClicked();
            }
        }
        if (this.DocumentTypePM.IsDirty) {
            this._documentTypePMService.putDocumentType(this.DocumentTypePM).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    _this.DocumentTypePM.IsDirty = false;
                }
            });
        }
    };
    EditDocumentComponent.prototype.SaveEditFeild = function () {
        var _this = this;
        if (this.stimulsoftArg.IsReset) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._exportDocumentService.GetResetEditableFields(this.CurrentDocumentOutId).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    _this.stimulsoftArg.IsReset = false;
                    if (_this.DataViewModel) {
                        _this.DataViewModel.IsRefreshPrintConrol = true;
                    }
                    if (_this.CurrentDocument)
                        _this.CurrentDocument.EditableFields = pmResponse.Result;
                    _this.SaveStimualField();
                }
            });
        }
        else {
            this.SaveStimualField();
        }
    };
    EditDocumentComponent.prototype.SaveStimualField = function () {
        var _this = this;
        var filter = new DocumentTypeTemplateFilter_1.DocumentTypeTemplateFilter();
        filter.Tenant = this.Tenant;
        filter.DocumentOutId = this.CurrentDocumentOutId;
        filter.Body = "";
        filter.Processtype = "SaveEditFields";
        filter.PageIndex = this.stimulsoftArg ? (this.stimulsoftArg.NumberOfPage - 1) : 0;
        filter.ReportKey = this.stimulsoftArg ? this.stimulsoftArg.ReportKey : "";
        filter.EditableFieldLists = [];
        if (this.stimulsoftArg.StimulsoftViewerComponent.EditableField != null && this.stimulsoftArg.StimulsoftViewerComponent.EditableField.length > 0) {
            this.stimulsoftArg.StimulsoftViewerComponent.EditableField.filter(function (d) { return d.Status == "Change"; }).forEach(function (field) {
                if (field.FieldValue != field.NewValue) {
                    filter.EditableFieldLists.push(field);
                    // filter.Body += Field.FieldName + "^" + Field.NewValue + "*" + Field.PageFieldIndex;
                }
            });
            if (filter.EditableFieldLists && filter.EditableFieldLists.length > 0) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            if (_this.CurrentDocument && myResult)
                                _this.CurrentDocument.EditableFields = myResult;
                            if (_this.DataViewModel)
                                _this.DataViewModel.IsRefreshPrintConrol = true;
                        }
                    }
                    if (_this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift())
                        _this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
                    else {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CloseButtonClicked();
                    }
                });
            }
            else {
                if (this.stimulsoftArg.StimulsoftViewerComponent.CheckIfChangeShift())
                    this.stimulsoftArg.StimulsoftViewerComponent.ApplayShift(true);
                else
                    this.CloseButtonClicked();
            }
        }
    };
    EditDocumentComponent.prototype.ShowDesignStimul = function (item) {
        var title = "";
        if (item != null) {
            title = "Edit Print Template";
        }
        else {
            title = "Edit Document";
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1600;
        logWindow.Height = 850;
        logWindow.Title = title;
        logWindow.DataContext = this.stimulsoftArg;
        logWindow.Show("./Infrastructure/Components/StimulsoftComponent/StimulsoftDesignerComponent");
    };
    EditDocumentComponent.prototype.ShowHtmlDocumentPreview = function (item) {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = this.PageType;
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = this.Tenant;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Html Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.IsEditManageTemplate = true;
            _this.RefreshTemplateId = $event;
            if (_this.RefreshTemplateId) {
                if (!_this.SelectedDocumentTypeTemplateViewModel) {
                    _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                }
                if (_this.SelectedDocumentTypeTemplateViewModel) {
                    if (_this.SelectedDocumentTypeTemplateViewModel.Id != _this.RefreshTemplateId)
                        _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                }
                if (_this.SelectedDocumentTypeTemplateViewModel) {
                    _this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
                    _this.LoadHtmlTemplateData(_this.RefreshTemplateId);
                }
                else {
                    _this.LoadDocumentTypeTemplates(_this.RefreshTemplateId);
                    _this.LoadHtmlTemplateData(_this.RefreshTemplateId);
                }
            }
        });
    };
    EditDocumentComponent.prototype.ShowEditStimaul = function (item) {
        var _this = this;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = item.Entity.Tenant;
        windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
        windowArgs.EntityId = "";
        windowArgs.ChildEntityId = "";
        windowArgs.ChildObjectTableId = "";
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Print Template";
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        window.designerClosed = false;
        logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.RefreshTemplateId = $event;
            if (_this.RefreshTemplateId) {
                if (_this.SelectedDocumentTypeTemplateViewModel.Id != _this.RefreshTemplateId) {
                    _this.SelectedDocumentTypeTemplateViewModel = _this.ReportTemplates.filter(function (d) { return d.Id == _this.RefreshTemplateId; })[0];
                }
                _this.SelectedDocumentTypeTemplateViewModel.IsLoad = false;
                _this.stimulsoftArg.NumberOfPage = 1;
                _this.stimulsoftArg.ReportKey = "";
                _this.stimulsoftArg.DocumenttypetemplateId = _this.RefreshTemplateId;
                _this.LoadstimulData(_this.RefreshTemplateId, _this.IsDisplayOnly, true, _this.stimulsoftArg.NumberOfPage, "GenerateReport", "");
            }
        });
    };
    EditDocumentComponent.prototype.SetTemplateAsDeflut = function (selectitem) {
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
                    this.ReportTemplates.forEach(function (template) {
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
    };
    EditDocumentComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    EditDocumentComponent.prototype.SetTemplateAsInactive = function (selectitem) {
        var _this = this;
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
                var item = this.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == selectitem.Id; })[0];
                if (item) {
                    item.InActive = selectitem.InActive;
                    this.UpdateDocumentTypeTemplate(item);
                }
                else {
                    this.documentTypeTemplatePMService.get(selectitem.Id).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                myResult.InActive = selectitem.InActive;
                                _this.DocumentTypeTemplatePMLists.push(myResult);
                                _this.UpdateDocumentTypeTemplate(myResult);
                            }
                        }
                    });
                }
            }
            else {
                this.ShowMessage("Please note that you can't Inactive the default template, please change the default template first");
            }
        }
    };
    EditDocumentComponent.prototype.AddDataField = function (type) {
        var _this = this;
        var tableName = "";
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        if (table)
            tableName = table.Name;
        var windowArgs = {};
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ObjectTypeField = "";
        windowArgs.InSertDataFieldType = this.InSertDataFieldType = type;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        // logWindow.DataContext = this;
        logWindow.Title = "Insert Data Field";
        this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(function (response) {
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event) {
                        if (type == "Subject") {
                            _this.Subject = insertAtSubject(_this.SubjectId, $event);
                        }
                        else if (type == "FroalaEditor") {
                            _this.froalaEditorSetting.froalaEditorComponent.InSertHtml($event);
                            _this.ReloadFroalaEditor();
                        }
                    }
                });
            });
        });
    };
    EditDocumentComponent.prototype.UpdateDocumentTypeTemplate = function (item) {
        this.documentTypeTemplatePMService.update(item).subscribe(function (myResult) {
        });
    };
    EditDocumentComponent.prototype.EditTemplate = function (selectitem) {
    };
    EditDocumentComponent.prototype.IsTemplateDefualt = function (selectitem) {
        var IsDefualt = false;
        if (selectitem.TemplateType == "P") {
            if (selectitem.Id == this.DocumentTypePM.DocumentTypeDefaultReportTemplateId) {
                IsDefualt = true;
            }
        }
        else if (selectitem.TemplateType == "M") {
            if (selectitem.Id == this.DocumentTypePM.DocumentTypeDefaultHTMLTemplateId) {
                IsDefualt = true;
            }
        }
        return IsDefualt;
    };
    EditDocumentComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
            // this.cd.detectChanges();
        }
    };
    EditDocumentComponent.prototype.ReloadStimulsoftViewer = function () {
        if (this.stimulsoftArg && this.stimulsoftArg.StimulsoftViewerComponent) {
            this.stimulsoftArg.StimulsoftViewerComponent.SetStimualData();
            this.cd.detectChanges();
        }
    };
    EditDocumentComponent.prototype.CheckboxClick = function () {
        if (this.IsCheckedInActive) {
            this.IsCheckedInActive = false;
            this.RefreshDocumentTemplateList(false);
        }
        else {
            this.IsCheckedInActive = true;
            this.RefreshDocumentTemplateList(true);
        }
    };
    EditDocumentComponent.prototype.RefreshDocumentTemplateList = function (isactive) {
        var _this = this;
        if (isactive) {
            this.ReportTemplates = this.DocumenttypetemplateLists;
        }
        else {
            this.ReportTemplates = this.DocumenttypetemplateLists.filter(function (d) { return d.InActive == false || d.IsDefault == true || (_this.SelectedDocumentTypeTemplateViewModel && _this.SelectedDocumentTypeTemplateViewModel.Id == d.Id); });
        }
    };
    EditDocumentComponent.prototype.DestroyfroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
        }
    };
    EditDocumentComponent.prototype.SetWindowArgs = function (args) {
        this.DataViewModel = args.DataViewModel;
        this.PageType = args.PageType;
        this.ModePage = args.ModePage ? args.ModePage : "";
        this.WindowHeight = args.WindowHeight;
        this.WindowWidth = args.WindowWidth;
        this.CurrentDocument = args.CurrentDocument;
        this.DocumentTypePM = args.DocumentTypePM;
        this.Subject = args.Subject ? args.Subject : "";
        this.EntityId = args.EntityId;
        this.DocumentTypeCopyId = args.DocumentTypeCopyId ? args.DocumentTypeCopyId : "";
        this.DocumentTypeCustomFieldLists = args.DocumentTypeCustomFieldLists;
        this.ChildEntityId = args.ChildEntityId ? args.ChildEntityId : "";
        this.ChildObjectTableId = args.ChildObjectTableId ? args.ChildObjectTableId : "";
        this.ObjectTableId = args.ObjectTableId ? args.ObjectTableId : "";
        if (this.DocumentTypePM) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId)) {
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
    };
    EditDocumentComponent.prototype.AddTemplateFromLibrary = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(function (response) {
            _this.IsDisableAddTemplateFromLibrary = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var windowArgs = {};
            windowArgs.DataViewModel = _this;
            windowArgs.ObjectTableId = _this.ObjectTableId;
            windowArgs.EntityId = _this.EntityId;
            if (_this.DocumentTypePM) {
                if (_this.DocumentTypePM.TemplateFormatCode == "P" && _this.DocumentTypePM.DocumentTypeDefaultEditorTool == "S") {
                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("DocumentTypeTemplate.O.NewPrintTemplate");
                }
                else
                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("DocumentTypeTemplate.O.NewHTMLTemplate");
                windowArgs.CurrentDocumentType = _this.DocumentTypePM;
            }
            windowArgs.ChildEntityId = _this.ChildEntityId;
            windowArgs.ChildObjectTableId = _this.ChildObjectTableId;
            windowArgs.PageRequest = "Edit";
            windowArgs.DocumentTemplateEditorTool = _this.DocumentTemplateEditorTool;
            logWindow.Width = 1000;
            logWindow.Height = 550;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeTemplateFromLibraryComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.IsDisableAddTemplateFromLibrary = false;
            });
        });
    };
    EditDocumentComponent.prototype.UploadButtonClicked = function () {
        document.getElementById(this.DocumentTemplateFileId).click();
    };
    EditDocumentComponent.prototype.UpLoadTemplateFileMethod = function (event) {
        var file = querySelection(this.DocumentTemplateFileId);
        if (file) {
            this.ArrayBufferToBase64(file, this);
        }
    };
    EditDocumentComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            if (viewmodel) {
                viewmodel._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(window.btoa(binary)).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var htmltemplate = myResult;
                            if (htmltemplate) {
                                htmltemplate.HeaderHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                htmltemplate.FooterHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";
                                htmltemplate.BodyHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.BodyHtml) ? htmltemplate.BodyHtml : "";
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
    };
    EditDocumentComponent.prototype.DownloadButtonClicked = function () {
        var _this = this;
        if (!this.IsDownLoadButtonClick) {
            this.IsDownLoadButtonClick = true;
            if (this.IsOpenHeaderAndFooter || (this.OldHtml != this.froalaEditorSetting.froalaEditorComponent.getHtml())) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Show("You have unsaved changes, Please save your work first!");
                confirmWindow.YesButtonText = "Save";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CurrentSession.StartBusyIndicatorSaving();
                        var documentTypeCopyId = "";
                        if (_this.DataViewModel && _this.DataViewModel.DocumentTypeload && _this.DataViewModel.DocumentTypeload.DocumentTypeCopies[0]) {
                            documentTypeCopyId = _this.DataViewModel.DocumentTypeload.DocumentTypeCopies[0].Id;
                        }
                        var filter = new FroalaEditorFilters_1.FroalaEditorFilters();
                        filter.DocumentOutId = _this.CurrentDocument.Id;
                        filter.DocumentTypeCopyId = documentTypeCopyId;
                        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                        filter.HtmlString = "<header>" + "<height>" + "<div style='display:none'>" + _this.HeaderHeight + "</div></height>" + _this.HeaderHtml + "</header>" + _this.froalaEditorSetting.froalaEditorComponent.getHtml() + "<footer>" + "<height>" + "<div style='display:none'>" + _this.FooterHeight + "</div></height>" + _this.FooterHtml + "</footer>";
                        filter.HeaderHeight = _this.HeaderHeight;
                        filter.FooterHeight = _this.FooterHeight;
                        filter.EntityId = _this.EntityId;
                        filter.ChildEntityId = _this.ChildEntityId;
                        filter.DocumentTypeId = _this.DocumentTypePM ? _this.DocumentTypePM.Id : "";
                        _this._htmlEditorService.saveEditedReportToServer(filter).subscribe(function (res) {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.IsOpenHeaderAndFooter = false;
                            _this.OldHtml = _this.froalaEditorSetting.froalaEditorComponent.getHtml();
                            _this.DownLoadFile();
                        });
                    }
                });
                if (confirmWindow.No) {
                    this.IsDownLoadButtonClick = false;
                }
            }
            else {
                this.DownLoadFile();
            }
        }
    };
    EditDocumentComponent.prototype.DownLoadFile = function () {
        if (this.CurrentDocument && !Tools_1.AppTool.IsNullOrEmpty(this.CurrentDocument.XamlDocumentId)) {
            var fileName = this.CurrentDocument.DocumentTypeName;
            var id = this.CurrentDocument.XamlDocumentId;
            var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
            var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DownloadTemplateDocumentPage.aspx?id=" + id + "&tempId=" + token + "&fileName=" + fileName + "&XamlDocumentId=" + this.CurrentDocument.XamlDocumentId;
            window.open(url);
        }
        this.IsDownLoadButtonClick = false;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EditDocumentComponent.prototype, "OnHeaderAndFooterCompleteEvent", void 0);
    __decorate([
        core_2.ViewChild('Child', { read: core_2.ViewContainerRef }),
        __metadata("design:type", core_2.ViewContainerRef)
    ], EditDocumentComponent.prototype, "viewContainerRef", void 0);
    EditDocumentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EditDocumentComponent',
            templateUrl: './EditDocumentView.html',
            providers: [HtmlEditorService_1.HtmlEditorService, DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, ExportDocumentService_1.ExportDocumentService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentOutPMService_1.DocumentOutPMService]
        }),
        __metadata("design:paramtypes", [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, ExportDocumentService_1.ExportDocumentService, HtmlEditorService_1.HtmlEditorService, DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, core_1.ChangeDetectorRef, DocumentOutPMService_1.DocumentOutPMService])
    ], EditDocumentComponent);
    return EditDocumentComponent;
}());
exports.EditDocumentComponent = EditDocumentComponent;
//# sourceMappingURL=EditDocumentComponent.js.map