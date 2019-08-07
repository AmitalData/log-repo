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
var QuoteTemplateExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var AddQuoteTemplateFromLibraryComponent = /** @class */ (function (_super) {
    __extends(AddQuoteTemplateFromLibraryComponent, _super);
    function AddQuoteTemplateFromLibraryComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.QuoteTypeCode = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService_1.QuoteTemplateExtendedPMService();
        return _this;
    }
    AddQuoteTemplateFromLibraryComponent.prototype.ngOnInit = function () {
    };
    AddQuoteTemplateFromLibraryComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.QuoteId = args.QuoteId;
        this.AreaName = args.AreaName;
        this.QuoteTypeCode = !Tools_1.AppTool.IsNullOrEmpty(args.QuoteTypeCode) ? args.QuoteTypeCode : "";
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(function (response) {
            _this.IsLoadTextCode = true;
            _this.QuoteTemplateLists = [];
            _this.FullQuoteTemplateLists = [];
            _this.LoadQuoteTemplateList();
        });
    };
    AddQuoteTemplateFromLibraryComponent.prototype.onSearchTextChangeEvent = function (search) {
        if (search) {
            if (search != "Search") {
                this.QuoteTemplateLists = this.FullQuoteTemplateLists.filter(function (d) { return d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1 || d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1; });
            }
        }
        else {
            this.QuoteTemplateLists = this.FullQuoteTemplateLists;
        }
        if (this.QuoteTemplateLists && this.QuoteTemplateLists.length == 0) {
            this.IsShowMessageNoQuoteTemplate = true;
        }
        else
            this.IsShowMessageNoQuoteTemplate = false;
    };
    AddQuoteTemplateFromLibraryComponent.prototype.LoadQuoteTemplateList = function () {
        var _this = this;
        this.QuoteTemplateLists = [];
        this.FullQuoteTemplateLists = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateExtendedPMService.GetQuoteTemplateListsFromLibrary(this.QuoteTypeCode).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                pmResponse.Result.forEach(function (item) {
                    _this.QuoteTemplateLists.push(item);
                    _this.FullQuoteTemplateLists.push(item);
                });
                if (_this.QuoteTemplateLists.length == 0) {
                    _this.IsShowMessageNoQuoteTemplate = true;
                }
                else
                    _this.IsShowMessageNoQuoteTemplate = false;
            }
        });
    };
    AddQuoteTemplateFromLibraryComponent.prototype.OnSelectedDocumentTypeTemplateLists = function (item) {
        this.QuoteTemplateViewModelSelected = item;
    };
    AddQuoteTemplateFromLibraryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddQuoteTemplateFromLibraryComponent.prototype.AddFromLibraryButtonClicked = function (item) {
        var _this = this;
        this.QuoteTemplateViewModelSelected = item;
        this.QuoteTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateExtendedPMService.GetCopyQuoteTemplateFromLibrary(item.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.CurrentSession.CurrentWindow.Close(pmResponse.Result);
            }
        });
    };
    AddQuoteTemplateFromLibraryComponent.prototype.OnSelectedQuoteTemplateLists = function (item) {
        this.QuoteTemplateViewModelSelected = item;
    };
    AddQuoteTemplateFromLibraryComponent.prototype.PreviewFromLibraryButtonClicked = function (item) {
        this.QuoteTemplateViewModelSelected = item;
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.QuoteTemplateId = item.Id;
        windowArgs.QuoteId = !Tools_1.AppTool.IsNullOrEmpty(this.QuoteId) ? this.QuoteId : "";
        windowArgs.AreaName = "FromLibrary";
        logWindow.Title = "Preview Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = (window.innerHeight - 130);
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/PreviewQuoteTemplateReportComponent");
    };
    AddQuoteTemplateFromLibraryComponent = __decorate([
        core_1.Component({
            selector: 'AddQuoteTemplateFromLibraryComponent',
            moduleId: module.id,
            templateUrl: './AddQuoteTemplateFromLibraryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddQuoteTemplateFromLibraryComponent);
    return AddQuoteTemplateFromLibraryComponent;
}(BaseComponent_1.BaseComponent));
exports.AddQuoteTemplateFromLibraryComponent = AddQuoteTemplateFromLibraryComponent;
//# sourceMappingURL=AddQuoteTemplateFromLibraryComponent.js.map