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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DocumentCopiesViewModel_1 = require("./DocsOut/ViewModel/DocumentCopiesViewModel");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var DocumentOutPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ExportDocumentService_1 = require("../../../../Common/Services/DocumentServices/ExportDocumentService");
var DocumentTypeTemplateListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService");
var DocumentTypeCustomFieldService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeCustomFieldService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var HtmlEditorService_1 = require("../../../../Common/Services/DocumentServices/HtmlEditorService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypeTemplateViewModel_1 = require("./DocsOut/ViewModel/DocumentTypeTemplateViewModel");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var DocumentCustomFieldsArgs_1 = require("./DocsOut/Filters/DocumentCustomFieldsArgs");
var FroalaEditorFilters_1 = require("./DocsOut/Filters/FroalaEditorFilters");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var PrintDocumentComponent = /** @class */ (function (_super) {
    __extends(PrintDocumentComponent, _super);
    function PrintDocumentComponent(_documentTypeCustomFieldService, _documentOutPMService, _documentTypePMService, _exportDocumentService, _documentTypeTemplateListExtendedService, _htmlEditorService) {
        var _this = _super.call(this) || this;
        _this._documentTypeCustomFieldService = _documentTypeCustomFieldService;
        _this._documentOutPMService = _documentOutPMService;
        _this._documentTypePMService = _documentTypePMService;
        _this._exportDocumentService = _exportDocumentService;
        _this._documentTypeTemplateListExtendedService = _documentTypeTemplateListExtendedService;
        _this._htmlEditorService = _htmlEditorService;
        _this.OnCloseWindow = new core_1.EventEmitter();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.BuildButtonIsEnabled = true;
        _this.TargetEntityName = "Shipment";
        _this.lastCount = 0;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsNoTemplateDefult = false;
        _this.IsSendClose = false;
        _this.IsNoTemplateFound = false;
        _this.IsDocumentBuildSucceeded = false;
        _this.IsDocumentBuildFailed = false;
        _this.IsQuotationDocument = false;
        _this.BuildingDocumentText = "Building document...";
        return _this;
    }
    PrintDocumentComponent.prototype.ngOnInit = function () {
    };
    PrintDocumentComponent.prototype.InitializeCopeisControl = function () {
        if (!this.CurrentDocumentOut.DocumentTemplateEditorTool && !this.IsQuotationDocument) {
            this.IsNoTemplateDefult = true;
        }
        this.GetTemplates();
        // this.IsLoading = true;
        if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "R") {
            this.IsShowDocumentCustomFields = false;
            this.PrintAllCopiesBtnDisable = true;
        }
        else {
            this.BuildButtonIsEnabled = true;
            this.PrintAllCopiesBtnDisable = false;
        }
        if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
            this.SelectedAsDefaultBtnVisible = false;
            this.BuildButtonIsEnabled = false;
        }
        this.LoadCopiesControl();
        this.LoadDocumentCustomFields();
        if (this.CurrentDocumentOut.IssuedDate) {
            this.LastBuildDate = this.CurrentDocumentOut.IssuedDate; //.toString();
            if (!this.isAWBWizard) {
                this.LastBuildDateVisible = true;
            }
        }
        else {
            this.LastBuildDateVisible = false;
        }
    };
    PrintDocumentComponent.prototype.LoadDocumentCustomFields = function () {
        var _this = this;
        this.DocumentCustomFieldsArgs = new DocumentCustomFieldsArgs_1.DocumentCustomFieldsArgs();
        this.DocumentCustomFieldsArgs.EditCustomField = false;
        this.DocumentCustomFieldsArgs.ObjectTableId = this.ObjectTableId;
        this.DocumentCustomFieldsArgs.DocumentTypeId = this.DataContext.DocumentTypePM.Id;
        this.DocumentCustomFieldsArgs.EntityId = this.EntityId;
        if (!this.IsSystemAdditionalPrintingFields) {
            this._documentTypeCustomFieldService.getDocumentTypeCustomFieldsByDocument(this.CurrentDocumentOut.Tenant, this.DocumentCustomFieldsArgs.DocumentTypeId).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.DocumentTypeCustomFieldLists = myResult;
                        if (_this.DocumentTypeCustomFieldLists.length > 0) {
                            _this.DocumentCustomFieldsArgs.DocumentTypeCustomFieldLists = _this.DocumentTypeCustomFieldLists;
                            _this.IsShowDocumentCustomFields = true;
                        }
                        else {
                            _this.IsShowDocumentCustomFields = false;
                        }
                    }
                    else {
                        _this.IsShowDocumentCustomFields = false;
                    }
                }
                else
                    _this.StopBusyIndicator();
            });
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PrintingFieldsScreenCode)) {
                this.DocumentCustomFieldsArgs.ScreenCode = this.PrintingFieldsScreenCode;
                this.DocumentCustomFieldsArgs.ObjectTableName = this.ObjectTableName;
                this.DocumentCustomFieldsArgs.EntityPM = this.DataContext.EntityPM;
                this.IsShowDocumentCustomFields = true;
            }
        }
    };
    PrintDocumentComponent.prototype.CheckDocumentTemplate = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DataContext.DocumentTypePM.Code)) {
            switch (this.DataContext.DocumentTypePM.Code.toUpperCase()) {
                case "740":
                case "714":
                case "716":
                case "716SD":
                case "784":
                case "781":
                case "999S":
                case "999M":
                case "999C":
                case "740L":
                case "740HL":
                case "CMR":
                case "SCMR":
                case "785A":
                case "785O":
                case "PAO":
                case "PAA":
                case "CPA":
                case "CPO":
                case "CPI":
                case "CPIO":
                case "CPE":
                case "PROF":
                case "APP":
                case "ARP":
                case "LCLL":
                case "SFBL":
                case "PALI":
                case "PALN":
                case "740PP":
                case "714PP":
                case "BCS":
                case "IFI":
                case "GAPS":
                case "DOR":
                case "COO":
                case "ARNT":
                case "PGDF":
                case "DORE":
                case "REOR":
                case "TML":
                case "LCOT":
                case "SBOL":
                case "MBOL":
                case "DEOR":
                case "BCO":
                case "SELE":
                case "DELI":
                case "EXCU":
                case "890":
                case "999P":
                case "999MP":
                case "AVISC":
                case "LAL":
                case "ESU":
                case "860":
                case "865":
                case "852":
                case "PROD":
                case "ATME":
                case "OPPA":
                case "DRA":
                case "SVDF":
                case "CA":
                case "CRCT":
                case "CRCD":
                case "CRCC":
                case "PCRC":
                case "999CI":
                case "JRPR":
                case "HORD":
                case "CDE":
                case "CDR":
                case "OPPB":
                case "SOPI":
                case "INVS":
                case "ETO":
                case "ITO":
                case "SSN":
                case "CRCW":
                case "CRCO":
                case "CRCI":
                case "CRCCU":
                case "CRCCM":
                case "CRCCB":
                case "OMBC":
                case "WHL":
                case "DESCH":
                case "WESL":
                    return true;
                default:
                    return false;
            }
        }
        else
            return false;
    };
    PrintDocumentComponent.prototype.showDialog = function (pageScreen) {
        var _this = this;
        if (this.IsNoTemplateFound || this.IsNoTemplateDefult) {
            return;
        }
        if (!this.CheckDocumentTemplate() && this.CurrentDocumentOut.DocumentTemplateEditorTool == "S") {
            return;
        }
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DocumentTypeload.Name + " Building");
        if (!this.CheckDocumentTemplate() && this.CurrentDocumentOut.DocumentTemplateEditorTool == "S") {
            return;
        }
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Document";
        if (pageScreen == "EditDocument") {
            windowArgs.ModePage = "StimaulEdit";
            if (this.isAWBWizard) {
                windowArgs.ModePage = "AWBWizardEdit";
            }
        }
        else if (pageScreen == "AdditionalPrintingFields") {
            this.IsShowDocumentCustomFields = false;
        }
        else {
            logWindow.Title = "Manage Template";
        }
        windowArgs.PageType = pageScreen;
        windowArgs.DataViewModel = this;
        windowArgs.WindowHeight = window.innerHeight - 100;
        windowArgs.WindowWidth = window.innerWidth - 100;
        windowArgs.CurrentDocument = this.CurrentDocumentOut;
        windowArgs.DocumentTypePM = this.DocumentTypeload;
        var documentTypeTemplate = this.DocumentTypeTemplateLists.filter(function (d) { return d.Id == _this.DataContext.CurrentDocument.DocumentTemplateId; })[0];
        windowArgs.Subject = documentTypeTemplate ? documentTypeTemplate.Subject : "";
        windowArgs.EntityId = this.EntityId;
        windowArgs.DocumentTypeCopyId = this.DataContext.documentOutCopyId;
        windowArgs.DocumentTypeCustomFieldLists = this.DocumentTypeCustomFieldLists;
        windowArgs.ChildEntityId = this.ChildEntityId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.ChildReference = this.ChildReference;
        windowArgs.ObjectTableId = this.ObjectTableId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = windowArgs.WindowWidth;
        logWindow.Height = windowArgs.WindowHeight;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/EditDocumentComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (_this.IsRefreshPrintConrol) {
                _this.UpdateDocument();
                _this.IsRefreshPrintConrol = false;
            }
        });
    };
    ;
    PrintDocumentComponent.prototype.showSendControlDialog = function (documentCopie) {
        var _this = this;
        if (documentCopie.CurrentDocumentOutCopy) {
            this.IsSendClose = false;
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, documentCopie.CurrentDocumentOutCopy.DocoumentTypeCopyName + " Sending");
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
            this.DataContext.documentOutCopyId = documentCopie.CurrentDocumentOutCopy.Id;
            this.DataContext.ModeSendDocument = "Send";
            this.DataContext.PageRequestSendComponent = "PrintDocumentComponent";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = this.DataContext.WindowWidth = sendWindowWidth;
            logWindow.Height = this.DataContext.WindowHeight = sendWindowHeight;
            logWindow.DataContext = this.DataContext;
            logWindow.Title = "Send Message";
            logWindow.NotifyOnClose = true;
            logWindow.IsShowCloseButton = true;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "SendEnd") {
                    if (documentCopie.CurrentDocumentOutCopy.DocumentTypeCopyId == documentCopie.CurrentDocumentType.LimitedPrintCopyId && documentCopie.CurrentDocumentType.IsDocumentOneTimePrintLimited) {
                        documentCopie.IsPrintButtonEnabled = false;
                        var loggedContactName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                        documentCopie.PrintedByMessage = "This document is already printed by " + loggedContactName;
                    }
                    if (!_this.IsSendClose) {
                        _this.IsSendClose = true;
                        _this.CurrentSession.FireEvent("RefreshDocumentOutSend");
                    }
                }
            });
        }
    };
    PrintDocumentComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.setArguments(this.DataContext);
    };
    PrintDocumentComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PrintDocumentComponent.prototype.GetTemplates = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.DataContext.DocumentTypePM.Id, this.DataContext.DocumentTypePM.Tenant).subscribe(function (res) {
            _this.DocumentTypeTemplateLists = new Array();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.filter(function (d) { return d.InActive == false; }).forEach(function (item) {
                        if (item.TemplateType == "P") {
                            _this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel_1.DocumentTypeTemplateViewModel(item));
                        }
                    });
                    if (!_this.IsNoTemplateFound && !_this.IsQuotationDocument) {
                        if (_this.DocumentTypeTemplateLists.length == 0) {
                            _this.ShowMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("DocsOut.M.NoTemplatesFound"));
                            _this.IsNoTemplateFound = true;
                        }
                        else {
                            if (_this.IsNoTemplateDefult)
                                _this.ShowMessage("Please select template as default");
                        }
                    }
                    if (_this.DocumentTypeTemplateLists.length > 0) {
                        _this.CurrentDocumentOut.DocumentTemplateId;
                        var item = _this.DocumentTypeTemplateLists.filter(function (r) { return r.Id == _this.CurrentDocumentOut.DocumentTemplateId; })[0];
                        if (item == null)
                            item = _this.DocumentTypeTemplateLists.filter(function (r) { return r.Id == _this.DataContext.DocumentTypePM.DocumentTypeDefaultReportTemplateId; })[0];
                        if (item == null)
                            item = _this.DocumentTypeTemplateLists[0];
                        _this.CurrentDocumentTypeTemplateList = item;
                    }
                }
            }
            else {
                _this.StopBusyIndicator();
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    _this.ShowMessage(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    PrintDocumentComponent.prototype.alertselected = function (selectedTemplate) {
        var _this = this;
        this.CurrentDocumentTypeTemplateList = selectedTemplate;
        //stimal start
        if (this.CurrentDocumentTypeTemplateList.EditorTool == "S" && this.CurrentDocumentTypeTemplateList.TemplateType == "P") {
            if (!this.IsCancelStimulDocumentBluid) {
                if (this.CurrentDocumentOut != null) {
                    this.CurrentDocumentOut.DocumentTemplateId = this.CurrentDocumentTypeTemplateList.Id;
                    this.CurrentDocumentOut.DocumentTemplateEditorTool = this.CurrentDocumentTypeTemplateList.EditorTool;
                    this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                _this.DataContext.CurrentDocument = _this.CurrentDocumentOut = myResult;
                                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(_this.ObjectTableName, _this.DocumentTypeload.Name + " Building");
                                _this.BuildCurrentCopies(_this.Items, "");
                            }
                        }
                    });
                }
            }
            else {
                this.IsCancelStimulDocumentBluid = false;
            }
            //LoadDocumentCustomFieldsControl();
        }
        // html
        if (this.CurrentDocumentTypeTemplateList.EditorTool == "R" && this.CurrentDocumentTypeTemplateList.TemplateType == "P") {
            if (!this.IsCancelHtmlDocumentBluid) {
                //  this.IsCancelCloseEditWindow = true;
                this.CurrentDocumentOut.DocumentTemplateId = this.CurrentDocumentTypeTemplateList.Id;
                this.DataContext.CurrentDocument = this.CurrentDocumentOut;
                this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.CurrentDocumentOut = myResult;
                            _this.ReBluidHtmlDocument(_this.DocumentTypeload.DocumentTypeCopies[0].Id);
                        }
                    }
                    else
                        _this.StopBusyIndicator();
                });
            }
            else {
                this.IsCancelHtmlDocumentBluid = false;
            }
        }
    };
    PrintDocumentComponent.prototype.OnmMouseOver = function (item) {
        this.Items.forEach(function (item) { item.VisiblePrint = false; });
        item.VisiblePrint = true;
    };
    PrintDocumentComponent.prototype.OnmMouseleave = function (item) {
        this.Items.forEach(function (item) { item.VisiblePrint = false; });
    };
    PrintDocumentComponent.prototype.SortItemSource = function () {
        if (this.Items) {
            this.Items = this.Items.sort(function (d) { return d.IndexOrder; });
        }
    };
    PrintDocumentComponent.prototype.LoadCopiesControl = function () {
        var _this = this;
        this.ItemsSource = new Array();
        this._documentTypePMService.getSingleDocumentType(this.DataContext.DocumentTypePM.Id, this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.DocumentTypeload = myResult;
                    if (_this.DocumentTypeload != null) {
                        _this.DataContext.DocumentTypePM = myResult;
                        if (_this.DataContext.DocumentTypePM.DocumentTypeCopies != null) {
                            _this.DocumentTypeload.DocumentTypeCopies.forEach(function (item) {
                                _this.ItemsSource.push(new DocumentCopiesViewModel_1.DocumentCopiesViewModel(item, _this.CurrentDocumentOut, _this.EntityId, _this.ChildEntityId, _this.ObjectTableId, _this.ChildObjectTableId, _this.DocumentTypeload, _this.ChildReference));
                            });
                            var item = _this.ItemsSource.filter(function (d) { return d.IsSelected; })[0];
                            var anySelected = false;
                            if (!item) {
                                _this.ItemsSource.forEach(function (item) {
                                    item.IsHideSetSelectedAsDefaultBtn = true;
                                    item.IsSelected = item.IsSelectedByDefault;
                                    _this.SelectedAsDefaultBtnVisible = false;
                                    anySelected = true;
                                });
                            }
                            _this.Items = _this.ItemsSource;
                            _this.SortItemSource();
                        }
                    }
                    if (_this.ItemsSource.length == 1) {
                        _this.PrintAllCopiesBtnVisible = false;
                    }
                    else {
                        if (_this.ItemsSource.length > 1) {
                            _this.PrintAllCopiesBtnVisible = true;
                        }
                    }
                    _this.CopiesControlLoaded(_this.ItemsSource);
                }
            }
        });
    };
    PrintDocumentComponent.prototype.CopiesControlLoaded = function (copies) {
        if (copies != null) {
            if (copies.length == 1) {
                this.PrintAllCopiesBtnVisible = false;
                this.SelectedAsDefaultBtnVisible = false;
            }
            else if (copies.length > 1) {
                this.PrintAllCopiesBtnVisible = true;
            }
            if (FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                copies.forEach(function (d) { return d.IsSelectedByDefault = true; });
            }
            if (this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
                copies.forEach(function (item) { item.CurrentDocumentTypeCopy.IsSelectedByDefault = true; });
            }
            this.lastCount = copies.filter(function (d) { return d.CurrentDocumentTypeCopy.IsSelectedByDefault; }).length;
            if ((this.CurrentDocumentOut.DocumentOutCopies.length == 0 || this.CurrentDocumentOut.NeedsRebuild)) {
                var editorToolCode = null;
                if (this.CurrentDocumentOut.DocumentTemplateEditorTool != null && this.CurrentDocumentOut.DocumentTemplateEditorTool != "") {
                    editorToolCode = this.CurrentDocumentOut.DocumentTemplateEditorTool;
                }
                else if (this.DataContext.DocumentTypePM.DocumentTypeDefaultEditorTool != null && this.DataContext.DocumentTypePM.DocumentTypeDefaultEditorTool != "") {
                    editorToolCode = this.DocumentTypeload.DocumentTypeDefaultEditorTool;
                }
                if (editorToolCode) {
                    if (editorToolCode == "S")
                        this.BuildCurrentCopies(copies, "New");
                    if (editorToolCode == "R") {
                        this.Items = new Array();
                        this.Items = copies;
                        this.ReBluidHtmlDocument(this.CurrentDocumentOut.DocumentTypeId);
                    }
                }
                else {
                    this.StopBusyIndicator();
                }
            }
            else {
                this.Items = new Array();
                this.Items = copies;
                this.StopBusyIndicator();
            }
            this.SortItemSource();
        }
    };
    PrintDocumentComponent.prototype.ReBluidHtmlDocument = function (documentTypeCopyId) {
        var _this = this;
        this.IsDocumentBuildSucceeded = false;
        this.IsDocumentBuildFailed = false;
        var documentTypeId = this.CurrentDocumentOut.DocumentTypeId;
        var shipmentId = this.CurrentDocumentOut.EntityId;
        var item = null;
        if (this.DocumentTypeTemplateLists)
            item = this.DocumentTypeTemplateLists.filter(function (d) { return d.Id == _this.CurrentDocumentOut.DocumentTemplateId; })[0];
        if (item && !Tools_1.AppTool.IsNullOrEmpty(item.HtmlResolve)) {
            this.HeaderHeight = item.TemplateHeaderHeight;
            this.FooterHeight = item.TemplateFooterHeight;
            this.HtmlEditorData = item.HtmlResolve;
            this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
            this.SaveReportData(documentTypeCopyId);
            item.HtmlResolve = null;
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._htmlEditorService.getEditorHtmlData(this.CurrentDocumentOut.Id, shipmentId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId, false, this.CurrentDocumentOut.DocumentTemplateId, "", "Edit").subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.HtmlEditorData = "<header>" + "<height>" + "<div style='display:none'>" + myResult.HeaderHeight + "</div></height>" + myResult.HeaderHtml + "</header>" + myResult.Htmlstring + "<footer>" + "<height>" + "<div style='display:none'>" + myResult.FooterHeight + "</div></height>" + myResult.FooterHtml + "</footer>";
                        _this.HeaderHeight = myResult.HeaderHeight;
                        _this.FooterHeight = myResult.FooterHeight;
                    }
                    _this.StopBusyIndicator();
                    _this.CurrentSession.StartBusyIndicator("Building document...");
                    _this.SaveReportData(documentTypeCopyId);
                }
                else
                    _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    PrintDocumentComponent.prototype.SaveReportData = function (documentTypeCopyId) {
        var _this = this;
        var filter = new FroalaEditorFilters_1.FroalaEditorFilters();
        filter.DocumentOutId = this.DataContext.CurrentDocument.Id;
        filter.DocumentTypeCopyId = documentTypeCopyId;
        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filter.HtmlString = this.HtmlEditorData;
        filter.HeaderHeight = this.HeaderHeight;
        filter.FooterHeight = this.FooterHeight;
        filter.EntityId = this.EntityId;
        filter.ChildEntityId = this.ChildEntityId;
        filter.DocumentTypeId = this.DataContext.DocumentTypePM.Id;
        this._htmlEditorService.saveEditedReportToServer(filter).subscribe(function (res) {
            _this.CurrentSession.StartBusyIndicator(_this.BuildingDocumentText);
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.docIds = myResult;
                    _this.idArray = _this.docIds.split(',');
                    _this.AddedDocumentTypeCopyViewModels = new Array();
                    _this.RemovedDocumentTypeCopyViewModels = new Array();
                    var anySelected = false;
                    _this.Items.forEach(function (copy) {
                        if (!_this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
                            if (copy.IsSelected) {
                                anySelected = true;
                                _this.AddedDocumentTypeCopyViewModels.push(copy);
                            }
                            if (copy.Exists && !copy.IsSelected) {
                                _this.RemovedDocumentTypeCopyViewModels.push(copy);
                            }
                        }
                        else {
                            copy.IsSelected = true;
                            anySelected = true;
                            _this.AddedDocumentTypeCopyViewModels.push(copy);
                        }
                    });
                    if (anySelected) {
                        _this.lastCount = _this.AddedDocumentTypeCopyViewModels.length;
                        var numberOfCopy = _this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.IsSelected; }).length;
                        if (numberOfCopy > 0) {
                            _this.CurrentDocumentOut.XamlDocumentId = _this.idArray[1];
                            _this.CurrentDocumentOut.IssuedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                            _this.CurrentDocumentOut.IsChangeIssuedDate = true;
                            _this.SaveContext();
                        }
                        else
                            _this.StopBusyIndicator();
                    }
                    else {
                        _this.StopBusyIndicator();
                    }
                }
                else {
                    _this.StopBusyIndicator();
                }
            }
            else
                _this.StopBusyIndicator();
        });
    };
    PrintDocumentComponent.prototype.BuildCurrentCopies = function (copies, mode) {
        var _this = this;
        this.IsDocumentBuildSucceeded = false;
        this.IsDocumentBuildFailed = false;
        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
        this.AddedDocumentTypeCopyViewModels = new Array();
        this.RemovedDocumentTypeCopyViewModels = new Array();
        var anySelected = false;
        copies.forEach(function (copy) {
            if (!_this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
                if (copy.IsSelected || copies.length == 1) {
                    anySelected = true;
                    if (copies.length == 1) {
                        copy.IsSelected = true;
                        copy.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                        copy.IsDiableSelctedDocumentTypeCopy = true;
                    }
                    else {
                        copy.IsDiableSelctedDocumentTypeCopy = false;
                    }
                    _this.AddedDocumentTypeCopyViewModels.push(copy);
                }
                if (copy.Exists && !copy.IsSelected) {
                    var index = _this.RemovedDocumentTypeCopyViewModels.indexOf(copy, 0);
                    if (index) {
                        _this.RemovedDocumentTypeCopyViewModels.splice(index, 1);
                    }
                }
            }
            else {
                copy.IsSelected = true;
                anySelected = true;
                _this.AddedDocumentTypeCopyViewModels.push(copy);
            }
        });
        if (anySelected) {
            this.lastCount = this.AddedDocumentTypeCopyViewModels.length;
            var numberOfCopy = this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.IsSelected; }).length;
            var count = 0;
            this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.IsSelected; }).forEach(function (copy) {
                _this._exportDocumentService.getDocumentPdfFile(_this.DataContext.DocumentTypePM.Id, _this.EntityId, _this.ObjectTableId, _this.ChildEntityId, _this.ChildObjectTableId, _this.CurrentDocumentOut.Id, _this.CurrentDocumentOut.Tenant, copy.CurrentDocumentTypeCopy.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
                    count += 1;
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult != null) {
                            copy.Status = "Success";
                            copy.Exists = true;
                            if (numberOfCopy == count) {
                                _this.CurrentDocumentOut.Issued = true;
                                _this.CurrentDocumentOut.NeedsRebuild = false;
                                _this.DataContext.Issued = true;
                                _this.CurrentDocumentOut.IssuedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                                _this.CurrentDocumentOut.IsChangeIssuedDate = true;
                                _this.SaveContext();
                            }
                        }
                        else
                            _this.StopBusyIndicator();
                    }
                    else {
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            _this.ShowMessage(pmResponse.ErrorsArray[0]);
                        }
                        _this.StopBusyIndicator();
                    }
                });
            });
            if (mode == "New" && this.AddedDocumentTypeCopyViewModels) {
                var copies = new Array();
                this.Items.forEach(function (copy) {
                    var item = _this.AddedDocumentTypeCopyViewModels.filter(function (d) { return d.Id == copy.Id; })[0];
                    if (item)
                        copies.push(item);
                    else
                        copies.push(copy);
                });
                this.Items = copies;
                this.SortItemSource();
            }
        }
        else {
            this.StopBusyIndicator();
            this.ShowMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("DocsOut.M.SelectCopyThenRebuild"));
        }
    };
    PrintDocumentComponent.prototype.SaveContext = function () {
        var _this = this;
        this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myResult = pmResponse.Result;
                _this._documentOutPMService.getSingleDocumentOutPM(_this.DataContext.CurrentDocument.Id, _this.DataContext.CurrentDocument.Tenant).subscribe(function (res) {
                    _this.StopBusyIndicator();
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            myResult.HasFollowUp = _this.DataContext.CurrentDocument.HasFollowUp;
                            _this.CurrentDocumentOut = myResult;
                            _this.DataContext.CurrentDocument = myResult;
                            if (_this.AddedDocumentTypeCopyViewModels != null) {
                                _this.AddedDocumentTypeCopyViewModels.forEach(function (copy) {
                                    copy.RefereshDocumentOutCopies(_this.CurrentDocumentOut);
                                });
                                _this.DataContext.HasFile = true;
                                if (_this.DataContext.Issued != true) {
                                    _this.DataContext.Issued = true;
                                }
                                _this.LastBuildDate = _this.CurrentDocumentOut.IssuedDate;
                                _this.DataContext.IssuedDate = _this.CurrentDocumentOut.IssuedDate;
                                _this.DataContext.IssuedByUserName = _this.CurrentDocumentOut.IssuedByUserName;
                                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(_this.ObjectTableName, _this.DataContext.DocumentTypePM.Name + " Built");
                                if (_this.DataContext.IsNotFromDocsOutListOpenPrintControl) {
                                    _this.CurrentSession.FireEvent("RefreshDocumentOutPrint");
                                }
                                _this.IsDocumentBuildSucceeded = true;
                            }
                        }
                    }
                    else
                        _this.StopBusyIndicator();
                });
            }
            else
                _this.StopBusyIndicator();
        });
    };
    PrintDocumentComponent.prototype.PrintMethod = function (item) {
        if (item.CurrentDocumentOutCopy) {
            var copyId = item.CurrentDocumentOutCopy.Id;
            var documentName = item.CurrentDocumentOutCopy.Tenant + "~" + item.CurrentDocumentOutCopy.Id;
            if (this.DocumentTypeload.IsDocumentOneTimePrintLimited && this.DataContext.DocumentTypePM.LimitedPrintCopyId == item.CurrentDocumentOutCopy.DocumentTypeCopyId) {
                documentName = documentName + "~" + item.CurrentDocumentOutCopy.DocumentId + "~" + SessionInfo_1.SessionInfo.LoggedUserId;
            }
            this.ViewPage(item.CurrentDocumentOutCopy.DocoumentTypeCopyName, copyId);
            if (item.CurrentDocumentOutCopy.DocumentTypeCopyId == item.CurrentDocumentType.LimitedPrintCopyId && item.CurrentDocumentType.IsDocumentOneTimePrintLimited) {
                item.IsPrintButtonEnabled = false;
                var loggedContactName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                item.PrintedByMessage = "This document is already printed by " + loggedContactName;
            }
        }
    };
    PrintDocumentComponent.prototype.ViewPage = function (docoumentTypeCopyName, id) {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, docoumentTypeCopyName + " Viewing");
        DownloadManager_1.DownloadManager.DownloadPage(id, this.CurrentDocumentOut.SecurityId);
    };
    PrintDocumentComponent.prototype.setArguments = function (item) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocsOut").subscribe(function (response) {
            if (!item.DocumentTypePM) {
                _this.CurrentSession.StartBusyIndicator("Loading...");
                _this._documentTypePMService.GetSinglePMWithOutInclude(item.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        item.DocumentTypePM = pmResponse.Result;
                    }
                    _this.CurrentSession.StopBusyIndicator();
                    _this.Start(item);
                });
            }
            else
                _this.Start(item);
        });
    };
    PrintDocumentComponent.prototype.Start = function (item) {
        var _this = this;
        this.DataContext = item;
        var buildingDocumentText = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument");
        if (!Tools_1.AppTool.IsNullOrEmpty(buildingDocumentText)) {
            this.BuildingDocumentText = buildingDocumentText;
        }
        this.ChildEntityId = item.ChildEntityId ? item.ChildEntityId : "";
        this.ChildObjectTableId = item.ChildObjectTableId ? item.ChildObjectTableId : "";
        this.ChildReference = item.ChildReference ? item.ChildReference : "";
        this.ObjectTableId = item.CurrentObjectTableId;
        this.CurrentDocumentOut = this.DataContext.CurrentDocument;
        this.DocumentTypeTemplateLists = new Array();
        this.EntityId = item.EntityId;
        this.Title = "Print " + this.DataContext.DocumentTypePM.Name;
        this.isAWBWizard = this.DataContext.IsAWBWizard;
        this.IsSystemAdditionalPrintingFields = this.DataContext.DocumentTypePM.IsSystemAdditionalPrintingFields;
        this.PrintingFieldsScreenCode = this.DataContext.DocumentTypePM.PrintingFieldsScreenCode;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        if (table) {
            this.ObjectTableId = table.Id;
            this.ObjectTableName = table.Name;
        }
        if (this.ObjectTableName == "Quote" && item.DocumentTypeCode == "QUOTE") {
            this.IsQuotationDocument = true;
        }
        if (this.isAWBWizard) {
            this.PrintAllCopiesBtnVisible = false;
            this.SelectedAsDefaultBtnVisible = false;
            this.LastBuildDateVisible = false;
        }
        if (this.DataContext.DocumentTypePM.IsReadOnly) {
            this.PrintAllCopiesBtnVisible = false;
            this.SelectedAsDefaultBtnVisible = false;
            this.LastBuildDateVisible = false;
        }
        if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
            this._documentOutPMService.getSingleDocumentOutPM(this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        myResult.HasFollowUp = _this.CurrentDocumentOut.HasFollowUp;
                        _this.CurrentDocumentOut = myResult;
                        _this.InitializeCopeisControl();
                    }
                }
            });
        }
        else {
            this.InitializeCopeisControl();
        }
    };
    PrintDocumentComponent.prototype.UpdateDocument = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow = this.CurrentSession.Windows.filter(function (d) { return d.Title == "Print " + _this.DataContext.DocumentTypePM.Name; })[0];
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DocumentTypeload.Name + " Building");
        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
        if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "S") {
            this.BuildCurrentCopies(this.Items, "");
        }
        else if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "R") {
            this.ReBluidHtmlDocument(this.DocumentTypeload.DocumentTypeCopies[0].Id);
        }
    };
    PrintDocumentComponent.prototype.PrintAllCopiesBtnClick = function () {
        var currentCount = this.Items.filter(function (d) { return d.IsSelected; }).length;
        if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
            this.Items.forEach(function (item) {
                if (item.CurrentDocumentOutCopy && item.CurrentDocumentType) {
                    if (item.CurrentDocumentOutCopy.DocumentTypeCopyId == item.CurrentDocumentType.LimitedPrintCopyId && Tools_1.AppTool.IsNullOrEmpty(item.PrintedByMessage)) {
                        var loggedContactName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                        item.PrintedByMessage = "This document is already printed by " + loggedContactName;
                    }
                }
            });
        }
        if (currentCount != this.lastCount) {
            this.ShowMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("DocsOut.M.RebuildThenPrintAgain"));
        }
        else {
            this.PrintAllDocs();
        }
    };
    PrintDocumentComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        this.IsDocumentBuildFailed = true;
    };
    PrintDocumentComponent.prototype.SetSelectedAsDefaultBtnClick = function () {
        var _this = this;
        this.Items.forEach(function (item) {
            item.CurrentDocumentTypeCopy.IsSelectedByDefault = item.IsSelected;
        });
        this._documentTypePMService.putDocumentType(this.DataContext.DocumentTypePM).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.DataContext.DocumentTypePM = myResult;
                    _this.BuildCurrentCopies(_this.Items, "");
                }
            }
        });
    };
    PrintDocumentComponent.prototype.PrintAllDocs = function () {
        var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/MergeAllPage.aspx?securityId=" + this.CurrentDocumentOut.SecurityId + "~" + SessionInfo_1.SessionInfo.LoggedUserId + "&tempId=" + token);
    };
    PrintDocumentComponent.prototype.DocumentCopySelectedChange = function (item, value) {
        item.IsSelected = value;
        this.SelectedAsDefaultBtnVisible = true;
        item.IsHideSetSelectedAsDefaultBtn = false;
    };
    PrintDocumentComponent.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], PrintDocumentComponent.prototype, "OnCloseWindow", void 0);
    PrintDocumentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'PrintDocument',
            templateUrl: './PrintDocumentView.html',
            providers: [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, DocumentTypeCustomFieldService_1.DocumentTypeCustomFieldService, DocumentOutPMService_1.DocumentOutPMService, ExportDocumentService_1.ExportDocumentService, DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, HtmlEditorService_1.HtmlEditorService],
            inputs: ['DataContext']
        }),
        __metadata("design:paramtypes", [DocumentTypeCustomFieldService_1.DocumentTypeCustomFieldService, DocumentOutPMService_1.DocumentOutPMService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService, ExportDocumentService_1.ExportDocumentService, DocumentTypeTemplateListExtendedService_1.DocumentTypeTemplateListExtendedService, HtmlEditorService_1.HtmlEditorService])
    ], PrintDocumentComponent);
    return PrintDocumentComponent;
}(BaseComponent_1.BaseComponent));
exports.PrintDocumentComponent = PrintDocumentComponent;
//# sourceMappingURL=PrintDocumentComponent.js.map