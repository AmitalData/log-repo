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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var FroalaEditorSetting_1 = require("./DocsOut/FroalaEditorSetting");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var HtmlEditorService_1 = require("../../../../Common/Services/DocumentServices/HtmlEditorService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var HeaderAndFooterComponent = /** @class */ (function () {
    function HeaderAndFooterComponent(_documentTypeTemplatePMExtendedService, cd, _htmlEditorService) {
        this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        this.cd = cd;
        this._htmlEditorService = _htmlEditorService;
        this.OnHeaderAndFooterCompleteEvent = new core_1.EventEmitter();
        this.OldDataTemplateByte = null;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowHeightBox = true;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
    }
    HeaderAndFooterComponent.prototype.ngOnInit = function () {
    };
    HeaderAndFooterComponent.prototype.SetWindowArgs = function (args) {
        this.froalaEditorSetting = new FroalaEditorSetting_1.FroalaEditorSetting();
        this.documentTypeTemplatePM = args.DocumentTypeTemplatePM;
        this.PageType = args.PageType;
        this.HtmlString = args.HtmlString;
        this.HeightValue = args.HeightValue;
        this.ObjectTableId = args.ObjectTableId;
        this.DataViewModel = args.DataViewModel;
        this.ChildObjectTableId = args.ChildObjectTableId ? args.ChildObjectTableId : "";
        this.PageRequse = args.PageRequse;
        this.OnHeaderAndFooterCompleteEvent = args.OnHeaderAndFooterCompleteEvent;
        if (this.PageRequse == "Send")
            this.IsShowHeightBox = false;
        if (!this.HeightValue) {
            this.HeightValue = 0;
        }
        this.HeightLable = this.PageType == "Header" ? "Header height" : "Footer height";
        if (this.documentTypeTemplatePM) {
            this.HeightValue = this.PageType == "Header" ? this.documentTypeTemplatePM.TemplateHeaderHeight : this.documentTypeTemplatePM.TemplateFooterHeight;
            this.IsShowAddDataField = true;
        }
        else
            this.IsShowAddDataField = false;
        this.Run();
    };
    HeaderAndFooterComponent.prototype.Run = function () {
        this.froalaEditorSetting.PageType = "HeaderAndFooter";
        this.froalaEditorSetting.Id = Guid_1.Guid.newGuid();
        this.froalaEditorSetting.Height = 145;
        if (this.documentTypeTemplatePM) {
            this.FillData();
        }
        else {
            this.froalaEditorSetting.HtmlString = this.HtmlString;
            this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.HtmlString);
        }
    };
    HeaderAndFooterComponent.prototype.FillData = function () {
        if (this.documentTypeTemplatePM) {
            if (this.froalaEditorSetting) {
                this.DocumentTypeCode = this.documentTypeTemplatePM.DocumentTypeCode;
                if (this.PageType == "Header") {
                    if (this.documentTypeTemplatePM.TemplateHeaderHtml) {
                        this.froalaEditorSetting.HtmlString = Base64ToString(this.documentTypeTemplatePM.TemplateHeaderHtml);
                    }
                    if (this.DataViewModel) {
                        this.froalaEditorSetting.HtmlString = !Tools_1.AppTool.IsNullOrEmpty(this.DataViewModel.TemplateHeaderHtml) ? Base64ToString(this.DataViewModel.TemplateHeaderHtml) : "";
                        this.HeightValue = this.DataViewModel.TemplateHeaderHeight;
                    }
                }
                else if (this.PageType == "Footer") {
                    if (this.documentTypeTemplatePM.TemplateFooterHtml) {
                        this.froalaEditorSetting.HtmlString = Base64ToString(this.documentTypeTemplatePM.TemplateFooterHtml);
                    }
                    if (this.DataViewModel) {
                        this.froalaEditorSetting.HtmlString = !Tools_1.AppTool.IsNullOrEmpty(this.DataViewModel.TemplateFooterHtml) ? Base64ToString(this.DataViewModel.TemplateFooterHtml) : "";
                        this.HeightValue = this.DataViewModel.TemplateFooterHeight;
                    }
                }
                this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.HtmlString);
            }
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    HeaderAndFooterComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    HeaderAndFooterComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.HtmlString = this.froalaEditorSetting.froalaEditorComponent.getHtml();
        if (!this.documentTypeTemplatePM && this.OnHeaderAndFooterCompleteEvent) {
            this.OnHeaderAndFooterCompleteEvent.emit(this);
        }
        this.DestroyfroalaEditor();
        this.CurrentSession.CurrentWindow.Close("");
    };
    HeaderAndFooterComponent.prototype.DestroyfroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
        }
    };
    HeaderAndFooterComponent.prototype.SaveButtonClicked = function () {
        var froalaString = "";
        if (this.documentTypeTemplatePM) {
            if (this.PageType == "Header") {
                this.documentTypeTemplatePM.TemplateHeaderHtml = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                this.documentTypeTemplatePM.TemplateHeaderHeight = this.HeightValue;
                if (this.DataViewModel) {
                    this.DataViewModel.TemplateHeaderHtml = this.documentTypeTemplatePM.TemplateHeaderHtml;
                    this.DataViewModel.TemplateHeaderHeight = this.documentTypeTemplatePM.TemplateHeaderHeight;
                }
            }
            else if (this.PageType == "Footer") {
                this.documentTypeTemplatePM.TemplateFooterHtml = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                this.documentTypeTemplatePM.TemplateFooterHeight = this.HeightValue;
                if (this.DataViewModel) {
                    this.DataViewModel.TemplateFooterHtml = this.documentTypeTemplatePM.TemplateFooterHtml;
                    this.DataViewModel.TemplateFooterHeight = this.documentTypeTemplatePM.TemplateFooterHeight;
                }
            }
            this.documentTypeTemplatePM.TemplateTechnologyCode = "AG";
        }
        this.ValidationErrorsList = [];
        if (this.HeightValue > 8 || this.HeightValue < 1) {
            if (this.HeightValue < 1 && Tools_1.AppTool.IsNullOrEmpty(this.froalaEditorSetting.froalaEditorComponent.getHtml())) {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CloseButtonClicked();
            }
            else
                this.ValidationErrorsList.push("Height should be have value between 1 cm and 8 cm");
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.CloseButtonClicked();
        }
    };
    HeaderAndFooterComponent.prototype.ReloadFroalaEditor = function () {
        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    };
    HeaderAndFooterComponent.prototype.AddDataField = function (type) {
        var _this = this;
        var tableName = "";
        var tableId = !Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId;
        var table = window.ObjectTables.filter(function (d) { return d.Id == tableId; })[0];
        if (table)
            tableName = table.Name;
        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(function (response) {
            if (table) {
                _this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
                    _this.ViewDataField(type, "", tableId);
                });
            }
            else
                _this.ViewDataField(type, "", tableId);
        });
    };
    HeaderAndFooterComponent.prototype.ViewDataField = function (type, objectTypeField, tableId) {
        var _this = this;
        var windowArgs = {};
        windowArgs.ObjectTypeField = objectTypeField;
        windowArgs.InSertDataFieldType = "FroalaEditor";
        windowArgs.DocumentTypeCode = this.DocumentTypeCode;
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
    };
    HeaderAndFooterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'HeaderAndFooter',
            templateUrl: './HeaderAndFooterComponent.html',
            providers: [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, HtmlEditorService_1.HtmlEditorService]
        }),
        __metadata("design:paramtypes", [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, core_1.ChangeDetectorRef, HtmlEditorService_1.HtmlEditorService])
    ], HeaderAndFooterComponent);
    return HeaderAndFooterComponent;
}());
exports.HeaderAndFooterComponent = HeaderAndFooterComponent;
//# sourceMappingURL=HeaderAndFooterComponent.js.map