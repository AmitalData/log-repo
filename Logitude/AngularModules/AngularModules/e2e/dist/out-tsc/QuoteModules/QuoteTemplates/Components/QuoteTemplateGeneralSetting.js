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
/// <reference path="../../../infrastructure/utilities/featurelocator.ts" />
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var QuoteTemplateGeneralSetting = /** @class */ (function (_super) {
    __extends(QuoteTemplateGeneralSetting, _super);
    function QuoteTemplateGeneralSetting() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsShowIsCopiedAtSignup = false;
        _this.IsShowEnableForCustomer = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        return _this;
    }
    QuoteTemplateGeneralSetting.prototype.ngOnInit = function () {
    };
    QuoteTemplateGeneralSetting.prototype.SetWindowArgs = function (args) {
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        if (this.QuoteTemplatePM) {
            this.Name = this.QuoteTemplatePM.Name;
            this.TemplateTypeCode = this.QuoteTemplatePM.TemplateTypeCode;
            this.IsDefault = this.QuoteTemplatePM.IsDefault;
            this.InActive = this.QuoteTemplatePM.InActive;
            this.IsCopiedAtSignup = this.QuoteTemplatePM.IsCopiedAtSignup;
            this.IsEnabledForCustomers = this.QuoteTemplatePM.IsEnabledForCustomers;
        }
        if (this.QuoteTemplateSettingPM) {
            this.QuoteTemplatePDFMarginRight = this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginRight;
            this.QuoteTemplatePDFMarginLeft = this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginLeft;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("QuoteTemplate", "COPYATSIGNUP")) {
            this.IsShowIsCopiedAtSignup = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("QuoteTemplate", "ENABLEDFORCUSTOMERS")) {
            this.IsShowEnableForCustomer = true;
        }
        this.IsLoadPage = true;
    };
    QuoteTemplateGeneralSetting.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.TemplateTypeCode)) {
            this.ValidationErrorsList.push("Please Select QuoteTemplate");
        }
        if (this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginRight > 200) {
            this.ValidationErrorsList.push("Right margin must be less than 200");
        }
        if (this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginLeft > 200) {
            this.ValidationErrorsList.push("Left margin must be less than 200");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.QuoteTemplatePM.Name = this.Name;
            this.QuoteTemplatePM.TemplateTypeCode = this.TemplateTypeCode;
            this.QuoteTemplatePM.IsDefault = this.IsDefault;
            this.QuoteTemplatePM.InActive = this.InActive;
            this.QuoteTemplatePM.IsCopiedAtSignup = this.IsCopiedAtSignup;
            this.QuoteTemplatePM.IsEnabledForCustomers = this.IsEnabledForCustomers;
            this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginRight = this.QuoteTemplatePDFMarginRight;
            this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginLeft = this.QuoteTemplatePDFMarginLeft;
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                    _this.QuoteTemplateSettingPM.IsDirty = false;
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindow();
                });
            }
            else
                this.CurrentSession.CloseCurrentWindow();
        }
    };
    QuoteTemplateGeneralSetting.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], QuoteTemplateGeneralSetting.prototype, "viewContainerRef", void 0);
    QuoteTemplateGeneralSetting = __decorate([
        core_1.Component({
            selector: 'QuoteTemplateGeneralSetting',
            moduleId: module.id,
            templateUrl: './QuoteTemplateGeneralSetting.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateGeneralSetting);
    return QuoteTemplateGeneralSetting;
}(BaseComponent_1.BaseComponent));
exports.QuoteTemplateGeneralSetting = QuoteTemplateGeneralSetting;
//# sourceMappingURL=QuoteTemplateGeneralSetting.js.map