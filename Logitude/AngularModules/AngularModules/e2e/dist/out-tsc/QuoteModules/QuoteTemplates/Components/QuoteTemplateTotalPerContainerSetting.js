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
var Tools_1 = require("../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var QuoteTemplatePricingSettingComponent_1 = require("./QuoteTemplatePricingSettingComponent");
var QuoteTemplateTableDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService");
var QuoteTemplateTextDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var QuoteTemplateTextCodeExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService");
var QuoteTemplateTextDesignExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var QuoteTemplateTotalPerContainerSetting = /** @class */ (function (_super) {
    __extends(QuoteTemplateTotalPerContainerSetting, _super);
    function QuoteTemplateTotalPerContainerSetting() {
        var _this = _super.call(this) || this;
        _this.QuoteTemplateTextCodePMList = [];
        _this.QuoteTemplateTextDesignPMLists = [];
        _this.IsLoadPage = false;
        _this.IsSaveQuoteTemplateTextDesignRuning = false;
        _this.IsSaveQuoteTemplateTableDesignRuning = false;
        _this.IsSaveQuoteTemplateTextCodeRuning = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        _this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService_1.QuoteTemplateTableDesignPMService();
        _this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService_1.QuoteTemplateTextDesignPMService();
        _this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService_1.QuoteTemplateTextDesignExtendedPMService();
        _this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService_1.QuoteTemplateTextCodeExtendedPMService();
        return _this;
    }
    QuoteTemplateTotalPerContainerSetting.prototype.ngOnInit = function () {
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
            if (args.QuoteTemplateTextCodePMList) {
                this.QuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList.filter(function (d) { return d.Area == "TotalPerContainers"; });
                this.BuildItemsSource();
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuoteTemplateSettingPM.TotalPerContainersTableDesignId)) {
                this.LoadData();
            }
            else
                this.IsLoadPage = false;
        }
    };
    Object.defineProperty(QuoteTemplateTotalPerContainerSetting.prototype, "TotalPerContainersCurrencyType", {
        //ShowTotalSplitToMultipleCurrencies
        get: function () {
            var totalPerContainersCurrencyType = "";
            if (this.QuoteTemplateSettingPM)
                totalPerContainersCurrencyType = this.QuoteTemplateSettingPM.TotalPerContainersCurrencyType;
            return totalPerContainersCurrencyType;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                this.QuoteTemplateSettingPM.TotalPerContainersCurrencyType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateTotalPerContainerSetting.prototype, "ShowTitleTotalPerContainersTable", {
        get: function () {
            var showTitleTotalPerContainersTable = false;
            if (this.QuoteTemplateSettingPM) {
                showTitleTotalPerContainersTable = this.QuoteTemplateSettingPM.ShowTitleTotalPerContainersTable;
            }
            return showTitleTotalPerContainersTable;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                this.QuoteTemplateSettingPM.ShowTitleTotalPerContainersTable = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateTotalPerContainerSetting.prototype, "ShowPageBreakBeforeTotalPerContainersTable", {
        get: function () {
            var showPageBreakBeforeTotalPerContainersTable = false;
            if (this.QuoteTemplateSettingPM) {
                showPageBreakBeforeTotalPerContainersTable = this.QuoteTemplateSettingPM.ShowPageBreakBeforeTotalPerContainersTable;
            }
            return showPageBreakBeforeTotalPerContainersTable;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                this.QuoteTemplateSettingPM.ShowPageBreakBeforeTotalPerContainersTable = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteTemplateTotalPerContainerSetting.prototype.BuildItemsSource = function () {
        var itemsCollection = [];
        this.QuoteTemplateTextCodePMList.forEach(function (item) {
            itemsCollection.push(new QuoteTemplatePricingSettingComponent_1.TextCodeData(item));
        });
        this.ItemsSource.AppendCollection(itemsCollection);
    };
    QuoteTemplateTotalPerContainerSetting.prototype.LoadData = function () {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.QuoteTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
    };
    QuoteTemplateTotalPerContainerSetting.prototype.LoadTableDesign = function () {
        var _this = this;
        this.quoteTemplateTableDesignPMService.get(this.QuoteTemplateSettingPM.TotalPerContainersTableDesignId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.TableDesignPM = pmResponse.Result;
            }
            _this.LoadTextDesign();
        });
    };
    QuoteTemplateTotalPerContainerSetting.prototype.LoadTextDesign = function () {
        var _this = this;
        var titleTextDesignId = this.QuoteTemplateSettingPM.TotalPerContainersAdditionalTextDesignId;
        var ids = titleTextDesignId;
        if (this.TableDesignPM) {
            ids += ("," + this.TableDesignPM.HeaderDesignId);
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }
        this.quoteTemplateTextDesignExtendedPMService.GetQuoteTemplateTextDesignPMListByIds(ids, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateTextDesignPMLists = pmResponse.Result;
                if (_this.TableDesignPM) {
                    _this.HeaderTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.TableDesignPM.HeaderDesignId; })[0];
                    if (_this.HeaderTextDesignPM) {
                        _this.HeaderTextDesignPM.Title = "Header";
                    }
                    _this.RowTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.TableDesignPM.LinesDesignId; })[0];
                    if (_this.RowTextDesignPM) {
                        _this.RowTextDesignPM.Title = "Rows";
                        _this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                _this.TitleTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == titleTextDesignId; })[0];
                if (_this.TitleTextDesignPM) {
                    _this.TitleTextDesignPM.Title = "Title";
                }
            }
            _this.IsLoadPage = true;
        });
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var textDesignPmLists = this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.IsDirty == true; });
        if (textDesignPmLists.length > 0)
            this.IsSaveQuoteTemplateTextDesignRuning = true;
        if (this.TableDesignPM.IsDirty)
            this.IsSaveQuoteTemplateTableDesignRuning = true;
        var textCodeDataLists = this.ItemsSource.Collection.filter(function (d) { return d.EntityPM.IsDirty == true; });
        if (textCodeDataLists.length > 0)
            this.IsSaveQuoteTemplateTextCodeRuning = true;
        if (this.IsSaveQuoteTemplateTextDesignRuning || this.IsSaveQuoteTemplateTableDesignRuning || this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                    _this.QuoteTemplateSettingPM.IsDirty = false;
                    _this.SaveOthers(textDesignPmLists, textCodeDataLists);
                });
            }
            else
                this.SaveOthers(textDesignPmLists, textCodeDataLists);
        }
        else {
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.SaveQuoteTemplateSetting();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveOthers = function (textDesignPmLists, textCodeDataLists) {
        if (this.IsSaveQuoteTemplateTextDesignRuning)
            this.SaveQuoteTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteTemplateTableDesignRuning)
            this.SaveQuoteTemplateTableDesign();
        if (this.IsSaveQuoteTemplateTextCodeRuning)
            this.SaveQuoteTemplateTextCode(textCodeDataLists);
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveQuoteTemplateTextDesign = function (items) {
        var _this = this;
        items.forEach(function (item) { item.IsDirty = false; });
        this.quoteTemplateTextDesignExtendedPMService.updateQuoteTemplateTextDesignPMs(items).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTextDesignRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveQuoteTemplateTableDesign = function () {
        var _this = this;
        this.quoteTemplateTableDesignPMService.update(this.TableDesignPM).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTableDesignRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveQuoteTemplateSetting = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
            _this.QuoteTemplateSettingPM.IsDirty = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveQuoteTemplateTextCode = function (items) {
        var _this = this;
        var quoteTemplateTextCodePMLists = [];
        items.forEach(function (item) {
            if (item.EntityPM) {
                item.EntityPM.IsDirty = false;
                quoteTemplateTextCodePMLists.push(item.EntityPM);
            }
        });
        this.quoteTemplateTextCodeExtendedPMService.updateTextCodes(quoteTemplateTextCodePMLists).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTextCodeRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateTotalPerContainerSetting.prototype.SaveCompleted = function () {
        if (!this.IsSaveQuoteTemplateTextDesignRuning && !this.IsSaveQuoteTemplateTableDesignRuning && !this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }
    };
    QuoteTemplateTotalPerContainerSetting.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("");
    };
    QuoteTemplateTotalPerContainerSetting = __decorate([
        core_1.Component({
            selector: 'QuoteTemplateTotalPerContainerSetting',
            moduleId: module.id,
            templateUrl: 'QuoteTemplateTotalPerContainerSetting.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateTotalPerContainerSetting);
    return QuoteTemplateTotalPerContainerSetting;
}(BaseComponent_1.BaseComponent));
exports.QuoteTemplateTotalPerContainerSetting = QuoteTemplateTotalPerContainerSetting;
//# sourceMappingURL=QuoteTemplateTotalPerContainerSetting.js.map