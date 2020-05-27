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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var QuoteTemplateExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService");
var QuoteTemplateSectionPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSectionPMService");
var QuoteTemplatePMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplatePMService");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var QuoteTemplateTextCodeExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService");
var QuoteTemplateSectionExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService");
var FroalaEditorSetting_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting");
var QuoteTemplateSectionPM_1 = require("../../../Quote/EntityPMs/QuoteTemplateSectionPM");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var EditQuoteTemplateComponent = /** @class */ (function (_super) {
    __extends(EditQuoteTemplateComponent, _super);
    function EditQuoteTemplateComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsNewEntityCall = true;
        _this.QuoteTemplateSectionLists = [];
        _this.AllQuoteTemplateSectionLists = [];
        _this.QuoteTemplateTextCodeLists = [];
        _this.IsLoadQuoteTemplateSectionRuning = false;
        _this.IsLoadQuoteTemplateTextCodeRuning = false;
        _this.IsLoadQuoteTemplateSettingsRuning = false;
        _this.IsSaveQuoteTemplateSectionRuning = false;
        _this.IsSaveQuoteTemplateRuning = false;
        _this.IsSaveQuoteTemplateSettingsRuning = false;
        _this.IsAddSectionRunning = false;
        _this.VisibilityGeneralSetting = true;
        _this.IsShowFroalaEditor = false;
        _this.IsDisableEditButton = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsReady = false;
        _this.RefreshQuoteTemplate = false;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("QuoteTemplate", "UPDATE"))
            _this.IsDisableEditButton = true;
        _this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        _this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService_1.QuoteTemplateExtendedPMService();
        _this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService_1.QuoteTemplateTextCodeExtendedPMService();
        _this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService_1.QuoteTemplateSectionExtendedPMService();
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        _this.quoteTemplateSectionPMService = new QuoteTemplateSectionPMService_1.QuoteTemplateSectionPMService();
        _this.quoteTemplatePMService = new QuoteTemplatePMService_1.QuoteTemplatePMService();
        return _this;
    }
    EditQuoteTemplateComponent.prototype.ngOnInit = function () {
        this.ShowLocalLanguageCheckBoxKey = Guid_1.Guid.newGuid();
        this.ShowRightToLeftCheckBoxKey = Guid_1.Guid.newGuid();
    };
    EditQuoteTemplateComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var _entityResourceService = new EntityResourceService_1.EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteTemplate").subscribe(function (response) {
            _this.IsReady = true;
            _this.Load(args);
        });
    };
    EditQuoteTemplateComponent.prototype.Load = function (args) {
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.froalaEditorSetting.IsDisableEdit = true;
        this.froalaEditorSetting.HtmlString = "";
        this.froalaEditorSetting.Height = (this.CurrentSession.CurrentWindow.Height - 100);
        this.IsShowFroalaEditor = true;
        this.IsNewEntityCall = args.IsNewEntityCall;
        this.EntityPM = args.EntityPM;
        this.QuotePM = args.QuotePM;
        this.CurrentEntity = args.CurrentEntity;
        this.QuoteTemplateId = args.QuoteTemplateId;
        if (this.EntityPM) {
            this.QuoteTemplateId = this.EntityPM.Id;
            this.LoadData();
        }
        else {
            if (this.CurrentEntity)
                this.QuoteTemplateId = this.CurrentEntity.Id;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuoteTemplateId))
                this.LoadQuoteTemplatePM();
            else
                this.LoadCompleted();
        }
        if (this.QuotePM != null) {
            this.VisibilityGeneralSetting = false;
        }
    };
    Object.defineProperty(EditQuoteTemplateComponent.prototype, "SelectQuoteTemplateSection", {
        get: function () { return this.selectQuoteTemplateSection; },
        set: function (newValue) {
            var _this = this;
            if (this.selectQuoteTemplateSection != newValue || !newValue.IsLoaded) {
                this.selectQuoteTemplateSection = newValue;
                this.QuoteTemplateSectionLists.forEach(function (item) {
                    if (item.Id != _this.selectQuoteTemplateSection.Id) {
                        item.Background = "white";
                        item.IsShowArrowUpDown = false;
                    }
                });
                this.selectQuoteTemplateSection.Background = "#96D3F0";
                if (this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "PF" && this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "PH" && this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "QH" && this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "QD") {
                    this.selectQuoteTemplateSection.IsShowArrowUpDown = true;
                }
                this.LoadSectionPreviewData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditQuoteTemplateComponent.prototype, "RightToLeft", {
        get: function () {
            var rightToLeft = false;
            if (this.QuoteTemplateSettingPM)
                rightToLeft = this.QuoteTemplateSettingPM.RightToLeft;
            return rightToLeft;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSettingPM.RightToLeft != value) {
                    this.QuoteTemplateSettingPM.RightToLeft = value;
                    this.IsSaveQuoteTemplateSettingsRuning = true;
                    this.SaveQuoteTemplateSetting();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditQuoteTemplateComponent.prototype, "ShowLocalLanguage", {
        get: function () {
            var showLocalLanguage = false;
            if (this.QuoteTemplateSettingPM)
                showLocalLanguage = this.QuoteTemplateSettingPM.ShowLocalLanguage;
            return showLocalLanguage;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSettingPM.ShowLocalLanguage != value) {
                    this.QuoteTemplateSettingPM.ShowLocalLanguage = value;
                    this.IsSaveQuoteTemplateSettingsRuning = true;
                    this.SaveQuoteTemplateSetting();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    EditQuoteTemplateComponent.prototype.GeneralSettingsButtonClicked = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.QuoteTemplatePM = this.EntityPM;
        windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 620;
        logWindow.Height = 400;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.General");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteTemplateGeneralSetting");
    };
    EditQuoteTemplateComponent.prototype.PreviewPdf = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.QuoteTemplateId = this.EntityPM.Id;
        windowArgs.QuoteId = this.QuotePM != null ? this.QuotePM.Id : "";
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.PreviewTemplate");
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = (window.innerHeight - 130);
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/PreviewQuoteTemplateReportComponent");
    };
    EditQuoteTemplateComponent.prototype.PreviewButtonClicked = function () {
        this.SaveDirtySections(true);
    };
    //Event Area
    EditQuoteTemplateComponent.prototype.EditSectionButtonClicked = function (item) {
        var _this = this;
        if (item.IsSection) {
            this.AddEditSection(item);
        }
        else {
            var componentPath = "";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var windowArgs = {};
            windowArgs.QuoteTemplatePM = this.EntityPM;
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            windowArgs.QuoteTemplateSectionViewModel = item;
            windowArgs.QuoteId = this.QuotePM != null ? this.QuotePM.Id : "";
            //Pricing Setting
            if (item.QuoteTemplateSectionTypeCode == "PP" || item.QuoteTemplateSectionTypeCode == "PC") {
                windowArgs.QuoteTemplateSectionTypeName = item.QuoteTemplateSectionTypeCode == "PP" ? "Packages" : "Containers";
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.Pricing" + windowArgs.QuoteTemplateSectionTypeName.replace(" ", "") + "Settings");
                windowArgs.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodeLists;
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteTemplatePricingSettingComponent";
                logWindow.Width = 950;
                logWindow.Height = 660;
            }
            //Page Header && Footer  Setting
            else if (item.QuoteTemplateSectionTypeCode == "PH" || item.QuoteTemplateSectionTypeCode == "PF") {
                windowArgs.QuoteTemplateSectionTypeName = item.QuoteTemplateSectionTypeCode == "PH" ? "Header" : "Footer";
                windowArgs.EditQuoteTemplateComponent = this;
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteTemplateHeaderFooterSettingComponent";
                logWindow.Width = (window.innerWidth / 1.476); // 1920/1300
                logWindow.Height = 600;
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.Page" + windowArgs.QuoteTemplateSectionTypeName.replace(" ", "") + "Settings");
            }
            else if (item.QuoteTemplateSectionTypeCode == "QH" || item.QuoteTemplateSectionTypeCode == "QD") {
                windowArgs.QuoteTemplateSectionTypeName = item.QuoteTemplateSectionTypeCode == "QH" ? "QuoteHeader" : "QuoteDetails";
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S." + windowArgs.QuoteTemplateSectionTypeName.replace(" ", "") + "Settings");
                windowArgs.QuoteTemplateSectionTypeCode = item.QuoteTemplateSectionTypeCode;
                windowArgs.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodeLists;
                windowArgs.QuotePM = this.QuotePM;
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteTemplateHeaderDetailsSettingComponent";
                logWindow.Width = 940;
                logWindow.Height = 660;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(componentPath)) {
                logWindow.WindowArgs = windowArgs;
                logWindow.Show(componentPath);
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event == "Refresh") {
                        if (windowArgs.QuoteTemplateSectionTypeName == "Header" || windowArgs.QuoteTemplateSectionTypeName == "Footer") {
                            if (_this.SelectQuoteTemplateSection != item) {
                                _this.SelectQuoteTemplateSection = item;
                            }
                            else {
                                _this.LoadSectionPreviewData();
                            }
                        }
                        else {
                            if (item.QuoteTemplateSectionTypeCode == "QH") {
                                var quoteTemplateSectionHeaderViewModel = _this.QuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PH"; })[0];
                                if (quoteTemplateSectionHeaderViewModel) {
                                    quoteTemplateSectionHeaderViewModel.IsLoaded = false;
                                }
                            }
                            item.IsLoaded = false;
                            _this.SelectQuoteTemplateSection = item;
                        }
                    }
                });
                //  Refresh
            }
        }
    };
    EditQuoteTemplateComponent.prototype.AddEditSection = function (item) {
        var _this = this;
        this.IsAddSectionRunning = true;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 900)
            sendWindowWidth = 900;
        if (sendWindowHeight < 500)
            sendWindowHeight = 500;
        var windowArgs = {};
        windowArgs.FatherComponent = this;
        windowArgs.HeightWindow = sendWindowHeight;
        windowArgs.QuoteTemplateSectionViewModel = item;
        windowArgs.QuoteTemplateId = this.EntityPM.Id;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = sendWindowWidth;
        logWindow.Height = sendWindowHeight;
        logWindow.Title = !item ? TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.AddSection") : TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.EditSection");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddEditQuoteTemplateSectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "Refresh") {
                _this.QuoteTemplateSectionLists = _this.AllQuoteTemplateSectionLists.filter(function (d) { return !d.IsCancel; });
                _this.LoadSectionPreviewData();
            }
            _this.IsAddSectionRunning = false;
        });
    };
    EditQuoteTemplateComponent.prototype.ShowRightToLeftClick = function () {
    };
    EditQuoteTemplateComponent.prototype.ShowLocalLanguageClick = function () {
    };
    EditQuoteTemplateComponent.prototype.ArrowUpButtonClicked = function (item) {
        if (item.QuoteTemplateSectionTypeCode != "QH" && item.QuoteTemplateSectionTypeCode != "QD" && item.QuoteTemplateSectionTypeCode != "PH" && item.QuoteTemplateSectionTypeCode != "PF") {
            var i = this.AllQuoteTemplateSectionLists.indexOf(item);
            var upColumn = this.AllQuoteTemplateSectionLists[i - 1];
            if (upColumn.QuoteTemplateSectionTypeCode != "PH") {
                if (i > 0) {
                    this.AllQuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(function (d) { return d.Id != upColumn.Id; });
                    var tempOrder = item.Order;
                    item.Order = upColumn.Order;
                    upColumn.Order = tempOrder;
                    this.AllQuoteTemplateSectionLists.splice(i, 0, upColumn);
                }
            }
            this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(function (d) { return !d.IsCancel; });
        }
    };
    EditQuoteTemplateComponent.prototype.ArrowDownButtonClicked = function (item) {
        if (item.QuoteTemplateSectionTypeCode != "QH" && item.QuoteTemplateSectionTypeCode != "QD" && item.QuoteTemplateSectionTypeCode != "PH" && item.QuoteTemplateSectionTypeCode != "PF") {
            var i = this.AllQuoteTemplateSectionLists.indexOf(item);
            var downColumn = this.AllQuoteTemplateSectionLists[i + 1];
            if (downColumn.QuoteTemplateSectionTypeCode != "PF") {
                if (i < this.AllQuoteTemplateSectionLists.length - 1) {
                    this.AllQuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(function (d) { return d.Id != downColumn.Id; });
                    var tempOrder = item.Order;
                    item.Order = downColumn.EntityPM.Order;
                    downColumn.Order = tempOrder;
                    this.AllQuoteTemplateSectionLists.splice(i, 0, downColumn);
                }
            }
            this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(function (d) { return !d.IsCancel; });
        }
    };
    //End Event Area
    //Loading Area
    EditQuoteTemplateComponent.prototype.LoadQuoteTemplatePM = function () {
        var _this = this;
        this.quoteTemplatePMService.get(this.QuoteTemplateId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.EntityPM = pmResponse.Result;
                _this.QuoteTemplateId = _this.EntityPM.Id;
                _this.LoadData();
            }
            else
                _this.CurrentSession.StopBusyIndicator();
        });
    };
    EditQuoteTemplateComponent.prototype.LoadData = function () {
        this.IsLoadQuoteTemplateSectionRuning = true;
        this.IsLoadQuoteTemplateTextCodeRuning = true;
        this.IsLoadQuoteTemplateSettingsRuning = true;
        this.IsLoadPreviewSectionRuning = true;
        this.LoadQuoteTemplateSectionLists();
        this.LoadQuoteTemplateTextCodeLists();
        this.LoadSetting();
    };
    EditQuoteTemplateComponent.prototype.LoadSetting = function () {
        var _this = this;
        this.quoteTemplateSettingPMService.get(this.EntityPM.QuoteTemplateSettingId).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadQuoteTemplateSettingsRuning = false;
            _this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateSettingPM = pmResponse.Result;
            }
        });
    };
    EditQuoteTemplateComponent.prototype.LoadQuoteTemplateSectionLists = function () {
        var _this = this;
        this.QuoteTemplateSectionLists = [];
        this.quoteTemplateSectionExtendedPMService.GetQuoteTemplateSectionByQuoteTemplateId(this.EntityPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadQuoteTemplateSectionRuning = false;
            _this.LoadCompleted();
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    _this.BuildingQuoteTemplateSection(pmResponse.Result);
                }
            }
        });
    };
    EditQuoteTemplateComponent.prototype.LoadQuoteTemplateTextCodeLists = function () {
        var _this = this;
        this.QuoteTemplateTextCodeLists = [];
        this.quoteTemplateTextCodeExtendedPMService.GetQuoteTemplateTextCodeByQuoteTemplateId(this.EntityPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadQuoteTemplateTextCodeRuning = false;
            _this.LoadCompleted();
            if (!pmResponse.HasError) {
                _this.QuoteTemplateTextCodeLists = pmResponse.Result;
            }
        });
    };
    EditQuoteTemplateComponent.prototype.LoadSectionPreviewData = function () {
        var _this = this;
        if (this.selectQuoteTemplateSection != null) {
            if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                this.froalaEditorSetting.froalaEditorComponent.SetHtml("");
                this.ReloadFroalaEditor();
            }
            if (this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "PB") {
                if (!this.selectQuoteTemplateSection.IsLoaded) {
                    this.selectQuoteTemplateSection.IsLoaded = true;
                    if (!this.IsLoadPreviewSectionRuning) {
                        this.IsLoadPreviewSectionRuning = true;
                        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
                    }
                    var quoteId = this.QuotePM != null ? this.QuotePM.Id : "";
                    var sectionDocId = !Tools_1.AppTool.IsNullOrEmpty(this.selectQuoteTemplateSection.SectionDocId) ? this.selectQuoteTemplateSection.SectionDocId : "";
                    this.quoteTemplateSectionExtendedPMService.DownloadQuoteTemplateSectionPdfFile(this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode, sectionDocId, this.EntityPM.Id, this.EntityPM.QuoteTemplateSettingId, quoteId, this.EntityPM.CreatedByUserId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                        var pmResponse = res;
                        _this.IsLoadPreviewSectionRuning = false;
                        _this.LoadCompleted();
                        if (!pmResponse.HasError && pmResponse.Result) {
                            _this.selectQuoteTemplateSection.HtmlBody = pmResponse.Result;
                            if (_this.froalaEditorSetting && _this.froalaEditorSetting.froalaEditorComponent) {
                                _this.froalaEditorSetting.froalaEditorComponent.SetHtml(pmResponse.Result);
                                _this.ReloadFroalaEditor();
                            }
                        }
                    });
                }
                else {
                    this.IsLoadPreviewSectionRuning = false;
                    this.LoadCompleted();
                    if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.selectQuoteTemplateSection.HtmlBody);
                        this.ReloadFroalaEditor();
                    }
                }
            }
            else {
                this.IsLoadPreviewSectionRuning = false;
                this.LoadCompleted();
            }
        }
    };
    EditQuoteTemplateComponent.prototype.LoadCompleted = function () {
        if (!this.IsLoadQuoteTemplateSectionRuning && !this.IsLoadQuoteTemplateTextCodeRuning && !this.IsLoadQuoteTemplateSettingsRuning && !this.IsLoadPreviewSectionRuning) {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    //End Loading Area
    EditQuoteTemplateComponent.prototype.AddPageBreakSection = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        var order = 0;
        if (this.SelectQuoteTemplateSection != null && !this.SelectQuoteTemplateSection.IsCancel)
            order = (this.SelectQuoteTemplateSection.Order + 1);
        else {
            var footerItem = this.AllQuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PF"; })[0];
            order = footerItem.Order;
        }
        var newQuoteTemplateSectionPM = new QuoteTemplateSectionPM_1.QuoteTemplateSectionPM();
        newQuoteTemplateSectionPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        newQuoteTemplateSectionPM.QuoteTemplateId = this.QuoteTemplateId;
        newQuoteTemplateSectionPM.QuoteTemplateSectionTypeCode = "PB";
        newQuoteTemplateSectionPM.Name = "Page Break";
        newQuoteTemplateSectionPM.Order = order;
        this.quoteTemplateSectionPMService.insert(newQuoteTemplateSectionPM).subscribe(function (res) {
            var pmResponse = res;
            var pageBreakSection = new QuoteTemplateSectionViewModel(pmResponse.Result);
            if (!pmResponse.HasError) {
                var items = [];
                _this.AllQuoteTemplateSectionLists.forEach(function (item) {
                    if (item.Order == (newQuoteTemplateSectionPM.Order - 1)) {
                        if (item.QuoteTemplateSectionTypeCode != "PF") {
                            items.push(item);
                            items.push(pageBreakSection);
                        }
                        else {
                            items.push(pageBreakSection);
                            items.push(item);
                        }
                    }
                    else {
                        items.push(item);
                    }
                });
                _this.UpdateOrderOfSections(items);
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    EditQuoteTemplateComponent.prototype.RemoveQuoteTemplateSectionClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.DeleteSectionConfirmMessage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                item.IsCancel = true;
                _this.QuoteTemplateSectionLists = _this.AllQuoteTemplateSectionLists.filter(function (d) { return d.IsCancel == false; });
                //if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                //    this.froalaEditorSetting.froalaEditorComponent.SetHtml("");
                //    this.ReloadFroalaEditor();
                //}
                _this.SelectQuoteTemplateSection = _this.QuoteTemplateSectionLists.filter(function (d) { return d.Order == (item.Order + 1); })[0];
            }
        });
    };
    EditQuoteTemplateComponent.prototype.SaveDirtySections = function (previewPdfAfterSave) {
        var _this = this;
        var quoteTemplateSectionChangedLists = [];
        this.AllQuoteTemplateSectionLists.filter(function (d) { return d.EntityPM.IsDirty; }).forEach(function (item) {
            quoteTemplateSectionChangedLists.push(item.EntityPM);
        });
        if (quoteTemplateSectionChangedLists.length > 0) {
            this.AllQuoteTemplateSectionLists.filter(function (d) { return d.EntityPM.IsDirty; }).forEach(function (item) {
                item.EntityPM.IsDirty = false;
            });
            this.quoteTemplateSectionExtendedPMService.updateSections(quoteTemplateSectionChangedLists).subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                if (previewPdfAfterSave)
                    _this.PreviewPdf();
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            if (previewPdfAfterSave)
                this.PreviewPdf();
        }
    };
    EditQuoteTemplateComponent.prototype.UpdateOrderOfSections = function (items) {
        var order = 0;
        items.forEach(function (sec) {
            sec.Order = order;
            order += 1;
        });
        this.AllQuoteTemplateSectionLists = items;
        this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(function (d) { return d.IsCancel == false; });
        this.SaveDirtySections(false);
    };
    //Saving Area
    EditQuoteTemplateComponent.prototype.SaveButtonClicked = function () {
        this.ValidationErrorsList = [];
        var quoteTemplateSectionChangedLists = [];
        this.AllQuoteTemplateSectionLists.filter(function (d) { return d.EntityPM.IsDirty; }).forEach(function (item) {
            quoteTemplateSectionChangedLists.push(item.EntityPM);
        });
        if (quoteTemplateSectionChangedLists.length > 0)
            this.IsSaveQuoteTemplateSectionRuning = true;
        if (this.EntityPM.IsDirty)
            this.IsSaveQuoteTemplateRuning = true;
        if (this.IsSaveQuoteTemplateSectionRuning || this.IsSaveQuoteTemplateRuning) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            this.SaveQuoteTemplateSection(quoteTemplateSectionChangedLists);
            this.SaveQuoteTemplate();
        }
        else
            this.SaveCompleted();
    };
    EditQuoteTemplateComponent.prototype.SaveQuoteTemplateSection = function (sections) {
        var _this = this;
        if (this.IsSaveQuoteTemplateSectionRuning) {
            this.quoteTemplateSectionExtendedPMService.updateSections(sections).subscribe(function (res) {
                _this.IsSaveQuoteTemplateSectionRuning = false;
                _this.SaveCompleted();
            });
        }
    };
    EditQuoteTemplateComponent.prototype.SaveQuoteTemplate = function () {
        var _this = this;
        if (this.IsSaveQuoteTemplateRuning) {
            this.RefreshQuoteTemplate = true;
            this.quoteTemplatePMService.update(this.EntityPM).subscribe(function (res) {
                _this.IsSaveQuoteTemplateRuning = false;
                _this.SaveCompleted();
            });
        }
    };
    EditQuoteTemplateComponent.prototype.SaveQuoteTemplateSetting = function () {
        var _this = this;
        if (this.IsSaveQuoteTemplateSettingsRuning) {
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                _this.IsSaveQuoteTemplateSettingsRuning = false;
                _this.SaveCompleted("RefreshPreviewData");
            });
        }
    };
    EditQuoteTemplateComponent.prototype.SaveCompleted = function (proess) {
        if (proess === void 0) { proess = null; }
        if (!this.IsSaveQuoteTemplateSectionRuning && !this.IsSaveQuoteTemplateRuning && !this.IsSaveQuoteTemplateSettingsRuning) {
            this.CurrentSession.StopBusyIndicator();
            if (this.QuotePM == null && this.RefreshQuoteTemplate) {
                this.CurrentSession.FireEvent("ReloadAllList");
            }
            if (proess == "RefreshPreviewData") {
                this.AllQuoteTemplateSectionLists.forEach(function (item) {
                    item.IsLoaded = false;
                });
                this.LoadSectionPreviewData();
            }
            else
                this.CurrentSession.CurrentWindow.Close("SavedChanges"); //this.CloseButtonClicked();
        }
    };
    //End Saving Area
    EditQuoteTemplateComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditQuoteTemplateComponent.prototype.BuildingQuoteTemplateSection = function (list) {
        var _this = this;
        if (list) {
            list = list.sort(function (a, b) { return a.Order - b.Order; });
            list.forEach(function (item) {
                var viewModelSection = new QuoteTemplateSectionViewModel(item);
                _this.QuoteTemplateSectionLists.push(viewModelSection);
                _this.AllQuoteTemplateSectionLists.push(viewModelSection);
            });
            if (this.QuoteTemplateSectionLists && this.QuoteTemplateSectionLists.length > 0) {
                this.SelectQuoteTemplateSection = this.QuoteTemplateSectionLists[0];
            }
            this.QuoteTemplateSectionLists = this.QuoteTemplateSectionLists.filter(function (d) { return d.IsCancel == false; });
        }
    };
    EditQuoteTemplateComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditQuoteTemplateComponent.prototype, "viewContainerRef", void 0);
    EditQuoteTemplateComponent = __decorate([
        core_1.Component({
            selector: 'EditQuoteTemplateComponent',
            moduleId: module.id,
            templateUrl: './EditQuoteTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditQuoteTemplateComponent);
    return EditQuoteTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.EditQuoteTemplateComponent = EditQuoteTemplateComponent;
var QuoteTemplateSectionViewModel = /** @class */ (function () {
    function QuoteTemplateSectionViewModel(quoteTemplateSection, quotationComponent) {
        if (quotationComponent === void 0) { quotationComponent = null; }
        this.Background = "white";
        this.IsShowArrowUpDown = false;
        this.IsSection = false;
        this.IsLoaded = false;
        this.SectionDocId = "";
        this.IsPagebrackSession = false;
        this.isExcluded = false;
        this.EntityPM = quoteTemplateSection;
        this.quotationComponent = quotationComponent;
        this.Id = quoteTemplateSection.Id;
        this.SectionDocId = quoteTemplateSection.SectionDocId;
        this.QuoteTemplateSectionTypeCode = quoteTemplateSection.QuoteTemplateSectionTypeCode;
        this.Description = quoteTemplateSection.Description;
        this.Templatedata = quoteTemplateSection.Templatedata;
        this.Order = quoteTemplateSection.Order;
        this.Name = quoteTemplateSection.Name;
        this.IsCancel = quoteTemplateSection.IsCancel;
        var code = this.EntityPM.QuoteTemplateSectionTypeCode;
        this.DisplayName = this.EntityPM.Name;
        if (code == "PP" || code == "PC" || code == "PF" || code == "PH" || code == "QH" || code == "QD" || code == "PB") {
            this.TranslationTextCode(this.EntityPM.QuoteTemplateSectionTypeCode);
        }
        if (quoteTemplateSection.QuoteTemplateSectionTypeCode != "PP" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PC" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "QH" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "QD" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PH" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PF" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PB") {
            this.IsSection = true;
        }
        else if (quoteTemplateSection.QuoteTemplateSectionTypeCode == "PB") {
            this.IsPagebrackSession = true;
        }
        if (this.IsPagebrackSession || this.IsSection)
            this.IsShowDeleteButton = true;
    }
    Object.defineProperty(QuoteTemplateSectionViewModel.prototype, "IsCancel", {
        get: function () {
            if (this.EntityPM) {
                this.isCancel = this.EntityPM.IsCancel;
            }
            return this.isCancel;
        },
        set: function (newValue) {
            if (this.isCancel != newValue) {
                this.isCancel = newValue;
                if (this.EntityPM)
                    this.EntityPM.IsCancel = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionViewModel.prototype, "Order", {
        get: function () {
            if (this.EntityPM) {
                this.order = this.EntityPM.Order;
            }
            return this.order;
        },
        set: function (newValue) {
            if (this.order != newValue) {
                this.order = newValue;
                if (this.EntityPM)
                    this.EntityPM.Order = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionViewModel.prototype, "Name", {
        get: function () {
            if (this.EntityPM) {
                this.name = this.EntityPM.Name;
            }
            return this.name;
        },
        set: function (newValue) {
            if (this.name != newValue) {
                this.name = newValue;
                if (this.EntityPM)
                    this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionViewModel.prototype, "Templatedata", {
        get: function () {
            if (this.EntityPM) {
                this.templatedata = this.EntityPM.Templatedata;
            }
            return this.templatedata;
        },
        set: function (newValue) {
            if (this.templatedata != newValue) {
                this.templatedata = newValue;
                if (this.EntityPM)
                    this.EntityPM.Templatedata = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionViewModel.prototype, "IsExcluded", {
        get: function () {
            if (this.EntityPM) {
                this.isExcluded = this.EntityPM.IsExcluded;
            }
            return this.isExcluded;
        },
        set: function (newValue) {
            if (this.isExcluded != newValue) {
                this.isExcluded = newValue;
                this.EntityPM.IsExcluded = newValue;
                if (newValue === true) {
                    this.quotationComponent.ExcludeSection(this);
                }
                else {
                    this.quotationComponent.IncludeSection(this);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteTemplateSectionViewModel.prototype.TranslationTextCode = function (sectionTypeCode) {
        var result = "";
        var textCode = "QuoteTemplate.S.";
        var textCodeToolTip = "QuoteTemplate.M.";
        if (sectionTypeCode == "PP") {
            textCode += "PricingPackages";
            textCodeToolTip += "PricingTableDescriptionMessage";
        }
        else if (sectionTypeCode == "PC") {
            textCode += "PricingContainers";
            textCodeToolTip += "PricingTableDescriptionMessage";
        }
        else if (sectionTypeCode == "PF") {
            textCode += "PageFooter";
            textCodeToolTip += "PageFooterDescriptionMessage";
        }
        else if (sectionTypeCode == "PH") {
            textCode += "PageHeader";
            textCodeToolTip += "PageHeaderDescriptionMessage";
        }
        else if (sectionTypeCode == "QH") {
            textCode += "QuoteHeader";
            textCodeToolTip += "QuoteHeaderDescriptionMessage";
        }
        else if (sectionTypeCode == "QD") {
            textCode += "QuoteDetails";
            textCodeToolTip += "QuoteDetailsDescriptionMessage";
        }
        else if (sectionTypeCode == "PB") {
            textCode = "QuoteTemplate.B.PageBreak";
        }
        this.DisplayName = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode);
        this.ToolTipDisplay = TextCodeTranslator_1.TextCodeTranslator.Translate(textCodeToolTip);
        return result;
    };
    return QuoteTemplateSectionViewModel;
}());
exports.QuoteTemplateSectionViewModel = QuoteTemplateSectionViewModel;
//# sourceMappingURL=EditQuoteTemplateComponent.js.map