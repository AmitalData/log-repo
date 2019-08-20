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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var core_1 = require("@angular/core");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var TextDesignComponent = /** @class */ (function () {
    function TextDesignComponent(elementRef, cd) {
        this.cd = cd;
        this.FontFamilyLists = [];
        this.FontSizeLists = [];
        this.SelectFonteSize = "";
        this.BorderTypes = [];
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.Title = "";
        this.QuoteTextAreaInputId = Guid_1.Guid.newGuid();
        this.elementRef = elementRef;
    }
    TextDesignComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.QuoteTemplateTextDesignPM) {
            this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S." + this.QuoteTemplateTextDesignPM.Title.replace(" ", ""));
            var fontFamilyString = "Arial,Arial Black,Calibri,Comic Sans MS,Courier New,Georgia,Lucida Sans Unicode,Times New Roman,Trebuchet MS,Verdana";
            var fontSizeString = "8,9,10,11,12,14,16,18,20,22,24,26,28,36,48,72";
            fontSizeString.split(',').forEach(function (fontsize) { _this.FontSizeLists.push(Number(fontsize)); });
            this.FontFamilyLists = fontFamilyString.split(',');
            this.SetFonteSize();
            this.SetFontStyle();
            this.SetTextDecoration();
        }
        this.FullBorderTypesLists();
    };
    TextDesignComponent.prototype.FullBorderTypesLists = function () {
        var _this = this;
        if (this.QuoteTemplateTableDesignPM) {
            this.BorderTypes = [];
            this.BorderTypes.push(new BorderType("None", "NONE"));
            this.BorderTypes.push(new BorderType("All", "ALL"));
            this.BorderTypes.push(new BorderType("Box", "BOX"));
            this.BorderTypes.push(new BorderType("Horizontal Only", "HORIZONTALLINES"));
            this.BorderTypes.push(new BorderType("Vertical Only", "VERTICALLINES"));
            this.BorderTypesSelected = this.BorderTypes.filter(function (d) { return d.Code == _this.QuoteTemplateTableDesignPM.BorderTypeCode; })[0];
        }
    };
    Object.defineProperty(TextDesignComponent.prototype, "TextColor", {
        get: function () {
            var textColor = "";
            if (this.QuoteTemplateTextDesignPM) {
                textColor = this.GetColorFromOrginal(this.QuoteTemplateTextDesignPM.TextColor);
            }
            return textColor;
        },
        set: function (value) {
            if (this.QuoteTemplateTextDesignPM) {
                this.QuoteTemplateTextDesignPM.TextColor = this.GetOrginalFromColor(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextDesignComponent.prototype, "BackgroundColor", {
        get: function () {
            var backgroundColor = "";
            if (this.QuoteTemplateTextDesignPM) {
                backgroundColor = this.GetColorFromOrginal(this.QuoteTemplateTextDesignPM.BackgroundColor);
            }
            return backgroundColor;
        },
        set: function (value) {
            if (this.QuoteTemplateTextDesignPM) {
                this.QuoteTemplateTextDesignPM.BackgroundColor = this.GetOrginalFromColor(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextDesignComponent.prototype, "BorderColor", {
        get: function () {
            var borderColor = "";
            if (this.QuoteTemplateTableDesignPM) {
                borderColor = this.GetColorFromOrginal(this.QuoteTemplateTableDesignPM.BorderColor);
            }
            return borderColor;
        },
        set: function (value) {
            if (this.QuoteTemplateTableDesignPM) {
                this.QuoteTemplateTableDesignPM.BorderColor = this.GetOrginalFromColor(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    //UnDerLineButton
    TextDesignComponent.prototype.QuoteTemplateTextDesignButtonClick = function (type) {
        if (type == "BoldButton") {
            this.QuoteTemplateTextDesignPM.FontWeight = this.QuoteTemplateTextDesignPM.FontWeight == "bold" ? this.QuoteTemplateTextDesignPM.FontWeight = "normal" : this.QuoteTemplateTextDesignPM.FontWeight = "bold";
        }
        else if (type == "ItalicButton") {
            this.QuoteTemplateTextDesignPM.Italic = this.QuoteTemplateTextDesignPM.Italic ? false : true;
            this.SetFontStyle();
        }
        else if (type == "UnDerLineButton") {
            this.QuoteTemplateTextDesignPM.UnDerLine = this.QuoteTemplateTextDesignPM.UnDerLine ? false : true;
            this.SetTextDecoration();
        }
    };
    TextDesignComponent.prototype.AlignmentButtonClick = function (alignment) {
        if (this.QuoteTemplateTextDesignPM) {
            if (alignment != this.QuoteTemplateTextDesignPM.Alignment) {
                this.QuoteTemplateTextDesignPM.Alignment = alignment;
            }
            else
                this.QuoteTemplateTextDesignPM.Alignment = "";
        }
    };
    TextDesignComponent.prototype.FontSizeSelectedChange = function (fontsize) {
        this.QuoteTemplateTextDesignPM.FontSize = fontsize;
        this.SetFonteSize();
    };
    TextDesignComponent.prototype.SetFontStyle = function () {
        if (this.QuoteTemplateTextDesignPM) {
            this.FontStyle = this.QuoteTemplateTextDesignPM.Italic ? "italic" : "normal";
        }
    };
    TextDesignComponent.prototype.SetTextDecoration = function () {
        if (this.QuoteTemplateTextDesignPM) {
            this.TextDecoration = this.QuoteTemplateTextDesignPM.UnDerLine ? "underline" : "none";
        }
    };
    TextDesignComponent.prototype.SetFonteSize = function () {
        if (this.QuoteTemplateTextDesignPM && this.QuoteTemplateTextDesignPM.FontSize) {
            this.SelectFonteSize = this.QuoteTemplateTextDesignPM.FontSize.toString() + "px";
        }
    };
    TextDesignComponent.prototype.GetOrginalFromColor = function (color) {
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
    TextDesignComponent.prototype.GetColorFromOrginal = function (color) {
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
    TextDesignComponent.prototype.BorderTypesSelectedChanged = function (border) {
        if (this.QuoteTemplateTableDesignPM) {
            this.QuoteTemplateTableDesignPM.BorderTypeCode = border.Code;
        }
    };
    TextDesignComponent.prototype.AddDataField = function () {
        var _this = this;
        var tableId = "";
        var table = window.ObjectTables.filter(function (d) { return d.Name == "Quote"; })[0];
        if (table)
            tableId = table.Id;
        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Quote").subscribe(function (response) {
                var windowArgs = {};
                windowArgs.ObjectTableId = tableId;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                windowArgs.InSertDataFieldType = "TextArea";
                logWindow.Title = "Insert Data Field";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event) {
                        if (_this.QuoteTemplateTextDesignPM) {
                            _this.QuoteTemplateTextDesignPM.TextValue = insertAtSubject(_this.QuoteTextAreaInputId, $event);
                        }
                    }
                });
            });
        });
    };
    TextDesignComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TextDesignComponent',
            templateUrl: './TextDesignComponent.html',
            inputs: ['QuoteTemplateTextDesignPM', 'QuoteTemplateTableDesignPM',]
        }),
        __metadata("design:paramtypes", [core_1.ElementRef, core_1.ChangeDetectorRef])
    ], TextDesignComponent);
    return TextDesignComponent;
}());
exports.TextDesignComponent = TextDesignComponent;
var BorderType = /** @class */ (function () {
    function BorderType(name, code) {
        this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S." + name.replace(" ", ""));
        this.Code = code;
    }
    return BorderType;
}());
exports.BorderType = BorderType;
//# sourceMappingURL=TextDesignComponent.js.map