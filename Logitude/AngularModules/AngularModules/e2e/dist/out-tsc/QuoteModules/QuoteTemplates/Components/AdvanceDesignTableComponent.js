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
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var AdvanceDesignTableComponent = /** @class */ (function (_super) {
    __extends(AdvanceDesignTableComponent, _super);
    function AdvanceDesignTableComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.borderColor = "";
        _this.borderThickness = 0;
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        return _this;
    }
    AdvanceDesignTableComponent.prototype.ngOnInit = function () {
    };
    AdvanceDesignTableComponent.prototype.SetWindowArgs = function (args) {
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.BorderColor = this.GetColorFromOrginal(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderBorderColor : this.QuoteTemplateSettingPM.PageFooterBorderColor);
        this.BorderThickness = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderBorderThickness : this.QuoteTemplateSettingPM.PageFooterBorderThickness;
    };
    AdvanceDesignTableComponent.prototype.GetColorFromOrginal = function (color) {
        var result = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(color)) {
            result = color;
            if (color && color.length > 7) {
                var colors = color.split('#');
                if (colors.length > 1) {
                    result = "#" + colors[1].substring(2, 8);
                }
            }
        }
        return result;
    };
    AdvanceDesignTableComponent.prototype.GetOrginalFromColor = function (color) {
        var result = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(color)) {
            result = color;
            var colors = color.split('#');
            if (colors.length > 1) {
                result = ("#" + "FF" + colors[1]);
            }
        }
        return result;
    };
    Object.defineProperty(AdvanceDesignTableComponent.prototype, "BorderColor", {
        get: function () {
            return this.borderColor;
        },
        set: function (newValue) {
            this.borderColor = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvanceDesignTableComponent.prototype, "BorderThickness", {
        get: function () {
            return this.borderThickness;
        },
        set: function (newValue) {
            this.borderThickness = newValue;
        },
        enumerable: true,
        configurable: true
    });
    AdvanceDesignTableComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        if (this.QuoteTemplateSectionTypeName == "Header") {
            this.QuoteTemplateSettingPM.PageHeaderBorderColor = this.GetOrginalFromColor(this.BorderColor);
            this.QuoteTemplateSettingPM.PageHeaderBorderThickness = this.BorderThickness;
        }
        else {
            this.QuoteTemplateSettingPM.PageFooterBorderColor = this.GetOrginalFromColor(this.BorderColor);
            this.QuoteTemplateSettingPM.PageFooterBorderThickness = this.BorderThickness;
        }
        if (this.QuoteTemplateSettingPM.IsDirty) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                _this.QuoteTemplateSettingPM.IsDirty = false;
                _this.CurrentSession.CurrentWindow.Close("Refresh");
            });
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AdvanceDesignTableComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvanceDesignTableComponent = __decorate([
        core_1.Component({
            selector: 'AdvanceDesignTableComponent',
            moduleId: module.id,
            templateUrl: './AdvanceDesignTableComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AdvanceDesignTableComponent);
    return AdvanceDesignTableComponent;
}(BaseComponent_1.BaseComponent));
exports.AdvanceDesignTableComponent = AdvanceDesignTableComponent;
//# sourceMappingURL=AdvanceDesignTableComponent.js.map