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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var QuoteTemplateSectionPM_1 = require("../../../Quote/EntityPMs/QuoteTemplateSectionPM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSectionPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSectionPMService");
var EditQuoteTemplateComponent_1 = require("./EditQuoteTemplateComponent");
var FroalaEditorSetting_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var QuoteTemplateSectionExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../../Infrastructure/Tools");
var AddEditQuoteTemplateSectionComponent = /** @class */ (function (_super) {
    __extends(AddEditQuoteTemplateSectionComponent, _super);
    function AddEditQuoteTemplateSectionComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.name = "";
        _this.quoteTemplateSectionPMService = new QuoteTemplateSectionPMService_1.QuoteTemplateSectionPMService();
        _this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService_1.QuoteTemplateSectionExtendedPMService();
        return _this;
    }
    AddEditQuoteTemplateSectionComponent.prototype.ngOnInit = function () {
    };
    AddEditQuoteTemplateSectionComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.froalaEditorSetting.Height = (args.HeightWindow - 175);
        this.QuoteTemplateId = args.QuoteTemplateId;
        this.FatherComponent = args.FatherComponent;
        this.QuoteTemplateSectionViewModel = args.QuoteTemplateSectionViewModel;
        this.IsNewQuoteTemplateSession = this.QuoteTemplateSectionViewModel ? false : true;
        if (this.IsNewQuoteTemplateSession)
            this.QuoteTemplateSectionViewModel = this.GetNewInstance();
        this.Name = this.QuoteTemplateSectionViewModel.Name;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        var sectionDocId = !Tools_1.AppTool.IsNullOrEmpty(this.QuoteTemplateSectionViewModel.EntityPM.SectionDocId) ? this.QuoteTemplateSectionViewModel.EntityPM.SectionDocId : "";
        this.quoteTemplateSectionExtendedPMService.DownloadQuoteTemplateSectionPdfFile(this.QuoteTemplateSectionViewModel.QuoteTemplateSectionTypeCode, sectionDocId, "", "", "", "", SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.ReloadFroalaEditor(pmResponse.Result);
            }
        });
    };
    AddEditQuoteTemplateSectionComponent.prototype.ReloadFroalaEditor = function (html) {
        if (html === void 0) { html = null; }
        if (!html) {
            if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                html = this.froalaEditorSetting.froalaEditorComponent.getHtml();
            }
        }
        else {
            this.froalaEditorSetting.HtmlString = html;
        }
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(html);
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    AddEditQuoteTemplateSectionComponent.prototype.GetNewInstance = function () {
        this.IsNewQuoteTemplateSession = true;
        var newQuoteTemplateSectionPM = new QuoteTemplateSectionPM_1.QuoteTemplateSectionPM();
        newQuoteTemplateSectionPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        newQuoteTemplateSectionPM.QuoteTemplateId = this.QuoteTemplateId;
        newQuoteTemplateSectionPM.QuoteTemplateSectionTypeCode = "S";
        newQuoteTemplateSectionPM.Order = this.FatherComponent.AllQuoteTemplateSectionLists.length - 1;
        var footerItem = this.FatherComponent.AllQuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PF"; })[0];
        if (footerItem) {
            newQuoteTemplateSectionPM.Order = footerItem.Order;
        }
        return new EditQuoteTemplateComponent_1.QuoteTemplateSectionViewModel(newQuoteTemplateSectionPM);
    };
    Object.defineProperty(AddEditQuoteTemplateSectionComponent.prototype, "Name", {
        get: function () {
            return this.name;
        },
        set: function (newValue) {
            this.name = newValue;
        },
        enumerable: true,
        configurable: true
    });
    AddEditQuoteTemplateSectionComponent.prototype.UpdateQuoteTemplateSession = function (item, type) {
        var _this = this;
        this.quoteTemplateSectionPMService.update(item).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    if (type == "Section") {
                        _this.QuoteTemplateSectionViewModel.EntityPM = pmResponse.Result;
                        _this.QuoteTemplateSectionViewModel.EntityPM.IschangeBodySection = false;
                        _this.QuoteTemplateSectionViewModel.SectionDocId = pmResponse.Result.SectionDocId;
                    }
                    else if (type == "Footer") {
                        if (_this.FatherComponent.AllQuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PF"; })[0]) {
                            _this.FatherComponent.AllQuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PF"; })[0].EntityPM = pmResponse.Result;
                        }
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CurrentWindow.Close("Refresh");
            }
            else {
                _this.CloseButtonClicked();
            }
        });
    };
    AddEditQuoteTemplateSectionComponent.prototype.AddDataField = function () {
        var _this = this;
        var tableId = "";
        var table = window.ObjectTables.filter(function (d) { return d.Name == "Quote"; })[0];
        if (table)
            tableId = table.Id;
        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Quote").subscribe(function (response) {
                var windowArgs = {};
                windowArgs.InSertDataFieldType = "FroalaEditor";
                windowArgs.ObjectTableId = tableId;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                logWindow.Title = "Insert Data Field";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event) {
                        if (_this.froalaEditorSetting.froalaEditorComponent) {
                            _this.froalaEditorSetting.froalaEditorComponent.InSertHtml($event);
                            _this.ReloadFroalaEditor();
                        }
                    }
                });
            });
        });
    };
    AddEditQuoteTemplateSectionComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }
        else if (this.Name.length > 60)
            this.ValidationErrorsList.push("Name field must be less than 60 and more than 1");
        if (this.ValidationErrorsList.length == 0) {
            var htmlbody = this.froalaEditorSetting.froalaEditorComponent.getHtml();
            var quotetemplateSectionBody = StringToBase64(htmlbody);
            this.QuoteTemplateSectionViewModel.Name = this.Name;
            this.QuoteTemplateSectionViewModel.DisplayName = this.Name;
            if (this.IsNewQuoteTemplateSession) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
                this.QuoteTemplateSectionViewModel.Templatedata = quotetemplateSectionBody;
                this.RefreshSectionToList();
                this.quoteTemplateSectionPMService.insert(this.QuoteTemplateSectionViewModel.EntityPM).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        _this.QuoteTemplateSectionViewModel.EntityPM = pmResponse.Result;
                        _this.QuoteTemplateSectionViewModel.SectionDocId = pmResponse.Result.SectionDocId;
                        _this.QuoteTemplateSectionViewModel.Id = pmResponse.Result.Id;
                        _this.FatherComponent.SelectQuoteTemplateSection = _this.QuoteTemplateSectionViewModel;
                        var footerItem = _this.FatherComponent.AllQuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PF"; })[0];
                        _this.UpdateQuoteTemplateSession(footerItem.EntityPM, "Footer");
                    }
                    else
                        _this.CloseButtonClicked();
                });
            }
            else {
                this.QuoteTemplateSectionViewModel.EntityPM.IschangeBodySection = this.QuoteTemplateSectionViewModel.HtmlBody != htmlbody ? true : false;
                if (this.QuoteTemplateSectionViewModel.EntityPM.IsDirty || this.QuoteTemplateSectionViewModel.EntityPM.IschangeBodySection) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
                    this.QuoteTemplateSectionViewModel.Templatedata = quotetemplateSectionBody;
                    this.QuoteTemplateSectionViewModel.EntityPM.Templatedata = quotetemplateSectionBody;
                    this.QuoteTemplateSectionViewModel.HtmlBody = "";
                    this.QuoteTemplateSectionViewModel.IsLoaded = false;
                    this.UpdateQuoteTemplateSession(this.QuoteTemplateSectionViewModel.EntityPM, "Section");
                }
                else
                    this.CloseButtonClicked();
            }
        }
        else {
            this.froalaEditorSetting.Height -= 20;
            this.ReloadFroalaEditor();
        }
    };
    AddEditQuoteTemplateSectionComponent.prototype.RefreshSectionToList = function () {
        var footerItem = this.FatherComponent.AllQuoteTemplateSectionLists.filter(function (d) { return d.QuoteTemplateSectionTypeCode == "PF"; })[0];
        if (footerItem) {
            this.QuoteTemplateSectionViewModel.Order = footerItem.Order;
            this.QuoteTemplateSectionViewModel.HtmlBody = "";
            this.QuoteTemplateSectionViewModel.IsLoaded = false;
            var index = this.FatherComponent.AllQuoteTemplateSectionLists.indexOf(footerItem);
            if (index != -1)
                this.FatherComponent.AllQuoteTemplateSectionLists.splice(index, 1);
            footerItem.Order += 1;
            this.FatherComponent.AllQuoteTemplateSectionLists.push(this.QuoteTemplateSectionViewModel);
            this.FatherComponent.AllQuoteTemplateSectionLists.push(footerItem);
        }
    };
    AddEditQuoteTemplateSectionComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("");
    };
    AddEditQuoteTemplateSectionComponent = __decorate([
        core_1.Component({
            selector: 'AddEditQuoteTemplateSectionComponent',
            moduleId: module.id,
            templateUrl: './AddEditQuoteTemplateSectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditQuoteTemplateSectionComponent);
    return AddEditQuoteTemplateSectionComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditQuoteTemplateSectionComponent = AddEditQuoteTemplateSectionComponent;
//# sourceMappingURL=AddEditQuoteTemplateSectionComponent.js.map