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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var QuoteTemplateTableDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService");
var QuoteTemplateTextDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService");
var QuoteTemplateTextDesignExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var FroalaEditorSetting_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting");
var TextDesignComponent_1 = require("../../../Infrastructure/Components/LogitudeCustomComponents/TextDesignComponent");
var QuoteTemplateSectionExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var QuoteTemplateHeaderFooterSettingComponent = /** @class */ (function (_super) {
    __extends(QuoteTemplateHeaderFooterSettingComponent, _super);
    function QuoteTemplateHeaderFooterSettingComponent() {
        var _this = _super.call(this) || this;
        _this.Alignment = [];
        _this.QuoteTemplateSectionTypeName = "Packages";
        _this.BorderTypes = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Prop
        _this.area1Width = 0;
        _this.area2Width = 0;
        _this.area3Width = 0;
        _this.heightArea = 0;
        _this.IsChangeSetting = false;
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        _this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService_1.QuoteTemplateTableDesignPMService();
        _this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService_1.QuoteTemplateTextDesignPMService();
        _this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService_1.QuoteTemplateTextDesignExtendedPMService();
        _this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService_1.QuoteTemplateSectionExtendedPMService();
        return _this;
    }
    QuoteTemplateHeaderFooterSettingComponent.prototype.ngOnInit = function () {
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.SetWindowArgs = function (args) {
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuoteTemplateSectionViewModel = args.QuoteTemplateSectionViewModel;
        this.EditQuoteTemplateComponent = args.EditQuoteTemplateComponent;
        this.QuoteId = args.QuoteId;
        var areaTypeString = "Logo,Text";
        if (this.QuoteTemplateSectionTypeName == "Header")
            areaTypeString += ",Quote Header";
        areaTypeString += ",None";
        this.AreaType = areaTypeString.split(',');
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.froalaEditorSetting.IsDisableEdit = true;
        if (this.QuoteTemplateSectionViewModel) {
            this.RefreshQuoteTemplateSectionBodyHtml();
            //if (this.QuoteTemplateSectionViewModel.IsLoaded) {
            //    this.froalaEditorSetting.HtmlString = this.QuoteTemplateSectionViewModel.HtmlBody;
            //}
            //else {
            //    this.RefreshQuoteTemplateSectionBodyHtml();
            //}
        }
        this.froalaEditorSetting.Height = ((this.CurrentSession.CurrentWindow.Height / 2) - 20);
        this.FullProperity();
        this.FullBorderTypesLists();
    };
    //Full Data
    QuoteTemplateHeaderFooterSettingComponent.prototype.FullProperity = function () {
        if (this.QuoteTemplateSettingPM) {
            this.HeightArea = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderAreaHeight : this.QuoteTemplateSettingPM.PageFooterAreaHeight;
            this.Area1Width = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Width : this.QuoteTemplateSettingPM.PageFooterArea1Width;
            this.Area2Width = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Width : this.QuoteTemplateSettingPM.PageFooterArea2Width;
            this.Area3Width = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Width : this.QuoteTemplateSettingPM.PageFooterArea3Width;
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.FullBorderTypesLists = function () {
        this.BorderTypes = [];
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("None", "NONE"));
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("All", "ALL"));
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("Box", "BOX"));
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("Horizontal Only", "HORIZONTALLINES"));
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("Vertical Only", "VERTICALLINES"));
        var selectedBorderCode = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderBorderTypeCode : this.QuoteTemplateSettingPM.PageFooterBorderTypeCode;
        this.BorderTypesSelected = this.BorderTypes.filter(function (d) { return d.Code == selectedBorderCode; })[0];
    };
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "Area1Width", {
        get: function () {
            return this.area1Width;
        },
        set: function (newValue) {
            this.area1Width = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "Area2Width", {
        get: function () {
            return this.area2Width;
        },
        set: function (newValue) {
            this.area2Width = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "Area3Width", {
        get: function () {
            return this.area3Width;
        },
        set: function (newValue) {
            this.area3Width = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "HeightArea", {
        get: function () {
            return this.heightArea;
        },
        set: function (newValue) {
            this.heightArea = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "PageArea1Type", {
        get: function () {
            var pageArea1Type = "";
            if (this.QuoteTemplateSettingPM)
                pageArea1Type = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Type : this.QuoteTemplateSettingPM.PageFooterArea1Type;
            return pageArea1Type;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "PageArea2Type", {
        get: function () {
            var pageArea2Type = "";
            if (this.QuoteTemplateSettingPM)
                pageArea2Type = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Type : this.QuoteTemplateSettingPM.PageFooterArea2Type;
            return pageArea2Type;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFooterSettingComponent.prototype, "PageArea3Type", {
        get: function () {
            var pageArea3Type = "";
            if (this.QuoteTemplateSettingPM)
                pageArea3Type = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Type : this.QuoteTemplateSettingPM.PageFooterArea3Type;
            return pageArea3Type;
        },
        enumerable: true,
        configurable: true
    });
    //Event
    QuoteTemplateHeaderFooterSettingComponent.prototype.AreaWidthLostFocusMethod = function (area) {
        this.ValidateFields();
        if (area == "Area1") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Width : this.QuoteTemplateSettingPM.PageFooterArea1Width;
            if (oldValue != this.Area1Width) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea1Width = this.Area1Width;
                }
                else
                    this.QuoteTemplateSettingPM.PageFooterArea1Width = this.Area1Width;
                this.SaveChanges();
            }
        }
        if (area == "Area2") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Width : this.QuoteTemplateSettingPM.PageFooterArea2Width;
            if (oldValue != this.Area2Width) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea2Width = this.Area2Width;
                }
                else
                    this.QuoteTemplateSettingPM.PageFooterArea2Width = this.Area2Width;
                this.SaveChanges();
            }
        }
        if (area == "Area3") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Width : this.QuoteTemplateSettingPM.PageFooterArea3Width;
            if (oldValue != this.Area3Width) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea3Width = this.Area3Width;
                }
                else
                    this.QuoteTemplateSettingPM.PageFooterArea3Width = this.Area3Width;
                this.SaveChanges();
            }
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.HeightAreaLostFocusMethod = function () {
        var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderAreaHeight : this.QuoteTemplateSettingPM.PageFooterAreaHeight;
        if (oldValue != this.HeightArea) {
            if (this.QuoteTemplateSectionTypeName == "Header") {
                this.QuoteTemplateSettingPM.PageHeaderAreaHeight = this.HeightArea;
            }
            else
                this.QuoteTemplateSettingPM.PageFooterAreaHeight = this.HeightArea;
            this.SaveChanges();
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.PageAreaSelectedChanged = function (area, value) {
        if (area == "Area1") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Type : this.QuoteTemplateSettingPM.PageFooterArea1Type;
            if (oldValue != value) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea1Type = value;
                }
                else
                    this.QuoteTemplateSettingPM.PageFooterArea1Type = value;
                this.SaveChanges();
            }
        }
        else if (area == "Area2") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Type : this.QuoteTemplateSettingPM.PageFooterArea2Type;
            if (oldValue != value) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea2Type = value;
                }
                else
                    this.QuoteTemplateSettingPM.PageFooterArea2Type = value;
                this.SaveChanges();
            }
        }
        else if (area == "Area3") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Type : this.QuoteTemplateSettingPM.PageFooterArea3Type;
            if (oldValue != value) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea3Type = value;
                }
                else
                    this.QuoteTemplateSettingPM.PageFooterArea3Type = value;
                this.SaveChanges();
            }
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.BorderTypesSelectedChanged = function (border) {
        if (this.QuoteTemplateSettingPM) {
            if (this.QuoteTemplateSectionTypeName == "Header") {
                if (this.QuoteTemplateSettingPM.PageHeaderBorderTypeCode != border.Code) {
                    this.QuoteTemplateSettingPM.PageHeaderBorderTypeCode = border.Code;
                    this.SaveChanges();
                }
            }
            else {
                if (this.QuoteTemplateSettingPM.PageFooterBorderTypeCode != border.Code) {
                    this.QuoteTemplateSettingPM.PageFooterBorderTypeCode = border.Code;
                    this.SaveChanges();
                }
            }
        }
    };
    //Command
    QuoteTemplateHeaderFooterSettingComponent.prototype.AdvanceButtonClicked = function () {
        var _this = this;
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
        windowArgs.QuoteTemplateSectionTypeName = this.QuoteTemplateSectionTypeName;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 400;
        logWindow.Height = 130;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.DesignTable");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AdvanceDesignTableComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "Refresh") {
                _this.IsChangeSetting = true;
                _this.RefreshQuoteTemplateSectionBodyHtml();
            }
        });
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.EditPageArea = function (type) {
        var _this = this;
        if ((type == "Area1" && this.PageArea1Type == "Quote Header") || (type == "Area2" && this.PageArea2Type == "Quote Header") || (type == "Area3" && this.PageArea3Type == "Quote Header")) {
            this.ShowQuoteHeaderEditWindow();
        }
        else {
            var windowArgs = {};
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            windowArgs.QuoteTemplateSectionTypeName = this.QuoteTemplateSectionTypeName;
            windowArgs.AreaType = type;
            if (type == "Area1")
                windowArgs.AreaMode = this.PageArea1Type;
            else if (type == "Area2")
                windowArgs.AreaMode = this.PageArea2Type;
            else if (type == "Area3")
                windowArgs.AreaMode = this.PageArea3Type;
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 800;
            logWindow.Height = 600;
            logWindow.BottomBorderForTitle = "1px solid LightGray";
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.Edit" + type);
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/PageAreaHeaderFooterComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "Refresh") {
                    _this.IsChangeSetting = true;
                    _this.RefreshQuoteTemplateSectionBodyHtml();
                }
            });
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.ShowQuoteHeaderEditWindow = function () {
        var _this = this;
        if (this.EditQuoteTemplateComponent) {
            var windowArgs = {};
            windowArgs.QuoteTemplatePM = this.QuoteTemplatePM;
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            var quoteTemplateSectionHeaderViewModel = this.EditQuoteTemplateComponent.QuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "QH"; })[0];
            windowArgs.QuoteTemplateSectionViewModel = quoteTemplateSectionHeaderViewModel;
            windowArgs.QuoteId = this.EditQuoteTemplateComponent.QuotePM != null ? this.EditQuoteTemplateComponent.QuotePM.Id : "";
            windowArgs.QuoteTemplateSectionTypeName = "QuoteHeader";
            windowArgs.QuoteTemplateSectionTypeCode = "QH";
            windowArgs.QuoteTemplateTextCodePMList = this.EditQuoteTemplateComponent.QuoteTemplateTextCodeLists;
            windowArgs.QuotePM = this.EditQuoteTemplateComponent.QuotePM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.QuoteHeader" + "Settings");
            logWindow.Width = 940;
            logWindow.Height = 660;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteTemplateHeaderDetailsSettingComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "Refresh") {
                    quoteTemplateSectionHeaderViewModel.IsLoaded = false;
                    _this.IsChangeSetting = true;
                    _this.RefreshQuoteTemplateSectionBodyHtml();
                }
            });
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.SaveChanges = function () {
        var _this = this;
        if (this.QuoteTemplateSettingPM.IsDirty) {
            this.IsChangeSetting = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                _this.QuoteTemplateSettingPM.IsDirty = false;
                _this.CurrentSession.StopBusyIndicator();
                _this.RefreshQuoteTemplateSectionBodyHtml();
            });
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.RefreshQuoteTemplateSectionBodyHtml = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateSectionExtendedPMService.DownloadQuoteTemplateSectionPdfFile(this.QuoteTemplateSectionViewModel.QuoteTemplateSectionTypeCode, this.QuoteTemplateSectionViewModel.Id, this.QuoteTemplateSectionViewModel.EntityPM.QuoteTemplateId, this.QuoteTemplateSettingPM.Id, this.QuoteId, this.QuoteTemplatePM.CreatedByUserId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateSectionViewModel.HtmlBody = pmResponse.Result;
                _this.QuoteTemplateSectionViewModel.IsLoaded = true;
                _this.QuoteTemplateSectionViewModel.HtmlBody = pmResponse.Result;
                if (_this.froalaEditorSetting && _this.froalaEditorSetting.froalaEditorComponent) {
                    _this.froalaEditorSetting.froalaEditorComponent.SetHtml(pmResponse.Result);
                    _this.ReloadFroalaEditor();
                }
            }
        });
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidateFields();
        if (this.ValidationErrorsList.length == 0) {
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                    _this.QuoteTemplateSettingPM.IsDirty = false;
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CurrentWindow.Close("Refresh");
                });
            }
            else {
                if (this.IsChangeSetting)
                    this.CurrentSession.CurrentWindow.Close("Refresh");
                else
                    this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.ValidateFields = function () {
        this.ValidationErrorsList = [];
        var widthArea = 0;
        if ((this.QuoteTemplateSectionTypeName == "Header" && this.QuoteTemplateSettingPM.PageHeaderArea1Type != "None") || (this.QuoteTemplateSectionTypeName == "Footer" && this.QuoteTemplateSettingPM.PageFooterArea1Type != "None")) {
            widthArea += this.Area1Width;
        }
        if ((this.QuoteTemplateSectionTypeName == "Header" && this.QuoteTemplateSettingPM.PageHeaderArea2Type != "None") || (this.QuoteTemplateSectionTypeName == "Footer" && this.QuoteTemplateSettingPM.PageFooterArea2Type != "None")) {
            widthArea += this.Area2Width;
        }
        if ((this.QuoteTemplateSectionTypeName == "Header" && this.QuoteTemplateSettingPM.PageHeaderArea3Type != "None") || (this.QuoteTemplateSectionTypeName == "Footer" && this.QuoteTemplateSettingPM.PageFooterArea3Type != "None")) {
            widthArea += this.Area3Width;
        }
        if (widthArea != 100) {
            if (this.QuoteTemplateSectionTypeName == "Header") {
                if (this.QuoteTemplateSettingPM.PageHeaderArea1Type != "None" || this.QuoteTemplateSettingPM.PageHeaderArea2Type != "None" || this.QuoteTemplateSettingPM.PageHeaderArea3Type != "None") {
                    this.ValidationErrorsList.push("width percentages sum have to be 100");
                }
            }
            else {
                if (this.QuoteTemplateSectionTypeName == "Footer") {
                    if (this.QuoteTemplateSettingPM.PageFooterArea1Type != "None" || this.QuoteTemplateSettingPM.PageFooterArea2Type != "None" || this.QuoteTemplateSettingPM.PageFooterArea3Type != "None") {
                        this.ValidationErrorsList.push("width percentages sum have to be 100");
                    }
                }
            }
        }
        if (this.HeightArea < 1 || this.HeightArea > 7) {
            this.ValidationErrorsList.push("Height should be have value  between 1 cm and 7 cm");
        }
    };
    QuoteTemplateHeaderFooterSettingComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], QuoteTemplateHeaderFooterSettingComponent.prototype, "viewContainerRef", void 0);
    QuoteTemplateHeaderFooterSettingComponent = __decorate([
        core_1.Component({
            selector: 'QuoteTemplateHeaderFooterSettingComponent',
            moduleId: module.id,
            templateUrl: './QuoteTemplateHeaderFooterSettingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateHeaderFooterSettingComponent);
    return QuoteTemplateHeaderFooterSettingComponent;
}(BaseComponent_1.BaseComponent));
exports.QuoteTemplateHeaderFooterSettingComponent = QuoteTemplateHeaderFooterSettingComponent;
//# sourceMappingURL=QuoteTemplateHeaderFooterSettingComponent.js.map