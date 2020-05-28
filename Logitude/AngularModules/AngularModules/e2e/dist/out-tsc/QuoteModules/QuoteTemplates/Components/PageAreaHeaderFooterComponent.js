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
var QuoteTemplateTextDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var PageAreaHeaderFooterComponent = /** @class */ (function (_super) {
    __extends(PageAreaHeaderFooterComponent, _super);
    function PageAreaHeaderFooterComponent() {
        var _this = _super.call(this) || this;
        //Text
        _this.PageAreaFreeText = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsSaveRuning = false;
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        _this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService_1.QuoteTemplateTextDesignPMService();
        return _this;
    }
    PageAreaHeaderFooterComponent.prototype.ngOnInit = function () {
    };
    PageAreaHeaderFooterComponent.prototype.SetWindowArgs = function (args) {
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.AreaType = args.AreaType;
        this.AreaMode = args.AreaMode;
        //Logo
        if (this.AreaMode == "Logo") {
            if (this.AreaType == "Area1") {
                this.ImageId = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1ImageDetailId : this.QuoteTemplateSettingPM.PageFooterArea1ImageDetailId;
                this.ImageWidth = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderImage1Width : this.QuoteTemplateSettingPM.PageFooterImage1Width;
                this.ImageHeight = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Height : this.QuoteTemplateSettingPM.PageFooterArea1Height;
                this.Alignment = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1ImageAlignment : this.QuoteTemplateSettingPM.PageFooterArea1ImageAlignment;
            }
            else if (this.AreaType == "Area2") {
                this.ImageId = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2ImageDetailId : this.QuoteTemplateSettingPM.PageFooterArea2ImageDetailId;
                this.ImageWidth = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderImage2Width : this.QuoteTemplateSettingPM.PageFooterImage2Width;
                this.ImageHeight = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Height : this.QuoteTemplateSettingPM.PageFooterArea2Height;
                this.Alignment = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2ImageAlignment : this.QuoteTemplateSettingPM.PageFooterArea2ImageAlignment;
            }
            else if (this.AreaType == "Area3") {
                this.ImageId = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3ImageDetailId : this.QuoteTemplateSettingPM.PageFooterArea3ImageDetailId;
                this.ImageWidth = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderImage3Width : this.QuoteTemplateSettingPM.PageFooterImage3Width;
                this.ImageHeight = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Height : this.QuoteTemplateSettingPM.PageFooterArea3Height;
                this.Alignment = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3ImageAlignment : this.QuoteTemplateSettingPM.PageFooterArea3ImageAlignment;
            }
        }
        else if (this.AreaMode == "Text") {
            if (this.AreaType == "Area1") {
                this.PageAreaFreeText = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1FreeText : this.QuoteTemplateSettingPM.PageFooterArea1FreeText;
                this.LoadDesignAreaFreeText(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1FreeTextDesignId : this.QuoteTemplateSettingPM.PageFooterArea1FreeTextDesignId);
            }
            else if (this.AreaType == "Area2") {
                this.PageAreaFreeText = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2FreeText : this.QuoteTemplateSettingPM.PageFooterArea2FreeText;
                this.LoadDesignAreaFreeText(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2FreeTextDesignId : this.QuoteTemplateSettingPM.PageFooterArea2FreeTextDesignId);
            }
            else if (this.AreaType == "Area3") {
                this.PageAreaFreeText = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3FreeText : this.QuoteTemplateSettingPM.PageFooterArea3FreeText;
                this.LoadDesignAreaFreeText(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3FreeTextDesignId : this.QuoteTemplateSettingPM.PageFooterArea3FreeTextDesignId);
            }
        }
    };
    PageAreaHeaderFooterComponent.prototype.LoadDesignAreaFreeText = function (headerDesignId) {
        var _this = this;
        this.quoteTemplateTextDesignPMService.get(headerDesignId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.DesignAreaFreeTextPM = pmResponse.Result;
                _this.DesignAreaFreeTextPM.TextValue = _this.PageAreaFreeText;
                _this.DesignAreaFreeTextPM.Title = "DesignAreaFreeText";
            }
        });
    };
    PageAreaHeaderFooterComponent.prototype.AlignmentButtonClick = function (alignment) {
        if (!Tools_1.AppTool.IsNullOrEmpty(alignment)) {
            if (alignment == this.Alignment)
                alignment = "";
            this.Alignment = alignment;
        }
    };
    PageAreaHeaderFooterComponent.prototype.ImageUploadedCompleted = function (imageId) {
        this.ImageId = imageId;
    };
    PageAreaHeaderFooterComponent.prototype.ImageNumericButtonClicked = function (propName, isIncreas) {
        var value = propName == "ImageHeight" ? this.ImageHeight : this.ImageWidth;
        value -= 0;
        if (isIncreas)
            value += 1;
        else
            value -= 1;
        if (propName == "ImageHeight")
            this.ImageHeight = value;
        else
            this.ImageWidth = value;
    };
    PageAreaHeaderFooterComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        if (this.AreaMode == "Logo") {
            if (this.AreaType == "Area1") {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea1Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageHeaderImage1Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageHeaderArea1ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageHeaderArea1ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteTemplateSettingPM.PageFooterArea1Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageFooterImage1Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageFooterArea1ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageFooterArea1ImageAlignment = this.Alignment;
                }
            }
            else if (this.AreaType == "Area2") {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea2Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageHeaderImage2Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageHeaderArea2ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageHeaderArea2ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteTemplateSettingPM.PageFooterArea2Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageFooterImage2Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageFooterArea2ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageFooterArea2ImageAlignment = this.Alignment;
                }
            }
            else if (this.AreaType == "Area3") {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea3Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageHeaderImage3Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageHeaderArea3ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageHeaderArea3ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteTemplateSettingPM.PageFooterArea3Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageFooterImage3Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageFooterArea3ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageFooterArea3ImageAlignment = this.Alignment;
                }
            }
        }
        else if (this.AreaMode == "Text") {
            this.PageAreaFreeText = this.DesignAreaFreeTextPM.TextValue;
            if (this.AreaType == "Area1") {
                if (this.QuoteTemplateSectionTypeName == "Header")
                    this.QuoteTemplateSettingPM.PageHeaderArea1FreeText = this.PageAreaFreeText;
                else
                    this.QuoteTemplateSettingPM.PageFooterArea1FreeText = this.QuoteTemplateSettingPM.PageFooterArea1FreeText = this.PageAreaFreeText;
            }
            else if (this.AreaType == "Area2") {
                if (this.QuoteTemplateSectionTypeName == "Header")
                    this.QuoteTemplateSettingPM.PageHeaderArea2FreeText = this.PageAreaFreeText;
                else
                    this.QuoteTemplateSettingPM.PageFooterArea2FreeText = this.QuoteTemplateSettingPM.PageFooterArea2FreeText = this.PageAreaFreeText;
            }
            else if (this.AreaType == "Area3") {
                if (this.QuoteTemplateSectionTypeName == "Header")
                    this.QuoteTemplateSettingPM.PageHeaderArea3FreeText = this.PageAreaFreeText;
                else
                    this.QuoteTemplateSettingPM.PageFooterArea3FreeText = this.QuoteTemplateSettingPM.PageFooterArea3FreeText = this.PageAreaFreeText;
            }
        }
        if (this.QuoteTemplateSettingPM.IsDirty || (this.DesignAreaFreeTextPM && this.DesignAreaFreeTextPM.IsDirty)) {
            this.IsSaveRuning = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        }
        if (this.DesignAreaFreeTextPM && this.DesignAreaFreeTextPM.IsDirty) {
            this.quoteTemplateTextDesignPMService.update(this.DesignAreaFreeTextPM).subscribe(function (res) {
                _this.SaveQuoteTemplateSetting();
            });
        }
        else {
            this.SaveQuoteTemplateSetting();
        }
    };
    PageAreaHeaderFooterComponent.prototype.SaveQuoteTemplateSetting = function () {
        var _this = this;
        if (this.QuoteTemplateSettingPM.IsDirty) {
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                _this.QuoteTemplateSettingPM.IsDirty = false;
                _this.SaveCompleted();
            });
        }
        else {
            this.SaveCompleted();
        }
    };
    PageAreaHeaderFooterComponent.prototype.SaveCompleted = function () {
        this.CurrentSession.StopBusyIndicator();
        if (this.IsSaveRuning)
            this.CurrentSession.CurrentWindow.Close("Refresh");
        else
            this.CurrentSession.CloseCurrentWindow();
    };
    PageAreaHeaderFooterComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PageAreaHeaderFooterComponent = __decorate([
        core_1.Component({
            selector: 'PageAreaHeaderFooterComponent',
            moduleId: module.id,
            templateUrl: './PageAreaHeaderFooterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PageAreaHeaderFooterComponent);
    return PageAreaHeaderFooterComponent;
}(BaseComponent_1.BaseComponent));
exports.PageAreaHeaderFooterComponent = PageAreaHeaderFooterComponent;
//# sourceMappingURL=PageAreaHeaderFooterComponent.js.map