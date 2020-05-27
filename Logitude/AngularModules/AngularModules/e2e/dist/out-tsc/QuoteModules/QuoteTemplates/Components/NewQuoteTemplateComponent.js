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
var QuoteTemplatePM_1 = require("../../../Quote/EntityPMs/QuoteTemplatePM");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var QuoteTemplateExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var NewQuoteTemplateComponent = /** @class */ (function (_super) {
    __extends(NewQuoteTemplateComponent, _super);
    function NewQuoteTemplateComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.QuoteTemplateLists = [];
        _this.VisibilityRadioFromTenant = false;
        _this.IsNewEntityCall = true;
        _this.IsReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AddType = "New";
        var _entityResourceService = new EntityResourceService_1.EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteTemplate").subscribe(function (response) {
            _this.IsReady = true;
            _this.EntityPM = _this.GetNewInstance();
            _this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService_1.QuoteTemplateExtendedPMService();
            _this.FromAllTenantRadioButton = Guid_1.Guid.newGuid();
            _this.CopyRadioButton = Guid_1.Guid.newGuid();
            _this.NewRadioButton = Guid_1.Guid.newGuid();
            if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                _this.VisibilityRadioFromTenant = true;
            }
        });
        return _this;
    }
    NewQuoteTemplateComponent.prototype.ngOnInit = function () {
    };
    NewQuoteTemplateComponent.prototype.GetNewInstance = function () {
        var newEntity = new QuoteTemplatePM_1.QuoteTemplatePM();
        newEntity.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newEntity.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newEntity.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newEntity.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newEntity.IsTemplate = true;
        return newEntity;
    };
    NewQuoteTemplateComponent.prototype.SetWindowArgs = function (args) {
    };
    NewQuoteTemplateComponent.prototype.RadioButtonChoice = function (choose) {
        switch (choose) {
            case "New":
                {
                    this.AddType = "New";
                    break;
                }
            case "Copy":
                {
                    this.AddType = "Copy";
                    this.LoadQuoteTemplateList();
                    break;
                }
            case "FromAllTenant":
                {
                    this.AddType = "FromAllTenant";
                    this.LoadQuoteTemplateList();
                    break;
                }
        }
    };
    NewQuoteTemplateComponent.prototype.OnSelectQuoteTemplateChange = function (item) {
        this.SelectedQuoteTemplate = item;
    };
    NewQuoteTemplateComponent.prototype.LoadQuoteTemplateList = function () {
        var _this = this;
        this.QuoteTemplateLists = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateExtendedPMService.GetQuoteTemplateLists(this.AddType).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.QuoteTemplateLists = pmResponse.Result;
            }
        });
    };
    //New 
    NewQuoteTemplateComponent.prototype.NextButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TemplateTypeCode) && this.AddType == "New") {
            this.ValidationErrorsList.push("Please Select QuoteTemplate");
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.AddType == "New") {
                this.CreateNewQuoteTemplate();
            }
            else {
                if (this.SelectedQuoteTemplate) {
                    this.CopyQuoteTemplatePM();
                }
            }
        }
    };
    NewQuoteTemplateComponent.prototype.CreateNewQuoteTemplate = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateExtendedPMService.insert(this.EntityPM).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.EntityPM = pmResponse.Result;
                _this.OpenEditQuoteTemplateComponent();
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    NewQuoteTemplateComponent.prototype.CopyQuoteTemplatePM = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateExtendedPMService.GetCopyQuoteTemplate(this.SelectedQuoteTemplate.Id, this.EntityPM.Name, SessionLocator_1.SessionLocator.LoggedUserId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    _this.EntityPM = pmResponse.Result;
                    _this.OpenEditQuoteTemplateComponent();
                }
                else
                    _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    //OpenEditQuoteTemplate
    NewQuoteTemplateComponent.prototype.OpenEditQuoteTemplateComponent = function () {
        this.CloseButtonClicked();
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.IsNewEntityCall = true;
        windowArgs.EntityPM = this.EntityPM;
        logWindow.Title = this.EntityPM.Name;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = window.innerWidth - 150;
        logWindow.Height = window.innerHeight - 150;
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = this;
        logWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");
    };
    NewQuoteTemplateComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewQuoteTemplateComponent.prototype, "viewContainerRef", void 0);
    NewQuoteTemplateComponent = __decorate([
        core_1.Component({
            selector: 'NewQuoteTemplateComponent',
            moduleId: module.id,
            templateUrl: './NewQuoteTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewQuoteTemplateComponent);
    return NewQuoteTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.NewQuoteTemplateComponent = NewQuoteTemplateComponent;
//# sourceMappingURL=NewQuoteTemplateComponent.js.map