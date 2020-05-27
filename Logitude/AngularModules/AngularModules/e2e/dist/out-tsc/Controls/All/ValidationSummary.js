"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var ValidationSummary = /** @class */ (function () {
    function ValidationSummary() {
        this.ErrorsCount = 0;
        this.ItemWidth = "50%";
        this.isSingleError = false;
        this.LayoutDirection = 'ltr';
        this.singleLine = false;
        this.itemsSource = [];
    }
    ValidationSummary.prototype.ngOnInit = function () {
        this.ErrorsFoundText = TextCodeTranslator_1.TextCodeTranslator.Translate('General.O.ErrorsFound');
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        this.UpdateItemWidth();
    };
    Object.defineProperty(ValidationSummary.prototype, "SingleLine", {
        get: function () { return this.singleLine; },
        set: function (newValue) {
            if (this.singleLine != newValue) {
                this.singleLine = newValue;
                this.UpdateItemWidth();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ValidationSummary.prototype, "ItemsSource", {
        get: function () { return this.itemsSource; },
        set: function (newValue) {
            var _this = this;
            if (this.itemsSource != newValue) {
                this.itemsSource = [];
                if (newValue != null) {
                    newValue.forEach(function (item) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item) && typeof (item) == "string") {
                            if (item.startsWith("!!")) {
                                item = item.substr(1);
                            }
                            if (item.indexOf(';') > -1) {
                                var itemParts = item.split(';');
                                itemParts.forEach(function (itemPart) {
                                    itemPart = Tools_1.AppTool.Replace(itemPart, "(ᵜ)", ";");
                                    if (_this.itemsSource.indexOf(itemPart) == -1) {
                                        _this.itemsSource.push(itemPart);
                                    }
                                });
                            }
                            else {
                                item = Tools_1.AppTool.Replace(item, "(ᵜ)", ";");
                                if (_this.itemsSource.indexOf(item) == -1) {
                                    _this.itemsSource.push(item);
                                }
                            }
                        }
                    });
                }
                if (this.itemsSource.length == 1) {
                    this.isSingleError = true;
                }
                else {
                    this.isSingleError = false;
                }
                this.ErrorsCount = this.itemsSource.length;
                this.UpdateItemWidth();
            }
        },
        enumerable: true,
        configurable: true
    });
    ValidationSummary.prototype.UpdateItemWidth = function () {
        if (this.ErrorsCount <= 1) {
            this.ItemWidth = "100%";
        }
        else {
            if (this.SingleLine) {
                this.ItemWidth = "100%";
            }
            else {
                this.ItemWidth = "50%";
            }
        }
    };
    ValidationSummary = __decorate([
        core_1.Component({
            selector: 'ValidationSummary',
            inputs: ['ItemsSource', 'SingleLine'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <table style=\"min-height: 25px;\">\n        <tr>\n            <td>\n                <div class=\"ValidationSummary\">\n                    <table>\n                        <tr>\n                            <td [style.text-align]=\"LayoutDirection=='rtl' ? 'right' : 'left'\"\n                                [ngStyle]=\"LayoutDirection == 'rtl' ? {'padding-right': '8px'} : {'padding-left': '8px'}\"\n                                 style=\"width: 110px; vertical-align: top; padding-top: 3px; padding-bottom: 3px; color: #B02020; font-size: 12px;\">\n                                {{ErrorsCount}} {{ErrorsFoundText}}:\n                            </td>\n\n                            <td style=\"position: relative; vertical-align:top; padding-top: 2px; padding-bottom: 2px;\" >\n\n                                <div *ngIf=\"isSingleError\" class=\"SingleError\" [ngStyle]=\"LayoutDirection == 'rtl' ? {'padding-right': '25px'} : {'padding-left': '25px'}\"><img [className]=\"LayoutDirection == 'rtl' ? 'RightCenter' : 'LeftCenter'\" src=\"./Images/ValidationError.png\" />{{ItemsSource[0]}}</div>\n\n                                <ul *ngIf=\"!isSingleError\">\n                                    <li class=\"ValidationItem\" [ngStyle]=\"{width: ItemWidth}\" *ngFor=\"let item of ItemsSource\" [style.float]=\"LayoutDirection=='rtl' ? 'right' : 'left'\">\n                                        <img [className]=\"LayoutDirection == 'rtl' ? 'RightCenter' : 'LeftCenter'\" src=\"./Images/ValidationError.png\" />\n                                        <span [className]=\"LayoutDirection == 'rtl' ? 'RightCenter' : 'LeftCenter'\" [style.text-align]=\"LayoutDirection=='rtl' ? 'right' : 'left'\"\n                                                [ngStyle]=\"LayoutDirection == 'rtl' ? {'margin-right': '20px'} : {'margin-left': '20px'}\">{{item}}</span>\n                                    </li>\n                                </ul>\n                            </td>\n                        </tr>\n                    </table>\n                </div>\n            </td>\n        </tr>\n\n        <tr style=\"height: 5px;\">\n            <td>\n                <div></div>\n            </td>\n        </tr>\n    </table>\n    ",
            styles: ["\n    .ValidationSummary span {\n        text-align: left;\n        width: calc(100% - 25px);\n        overflow: hidden;\n        white-space: nowrap;\n        text-overflow: ellipsis;\n    }\n\n    .ValidationSummary .ValidationItem {\n        display: block;\n        float: left;\n        width: 50%;\n        height: 18px;\n        line-height: 20px;\n        position: relative;\n    }\n\n    .ValidationSummary ul {\n        max-height: 80px;\n        box-sizing: border-box;\n        -moz-box-sizing: border-box;\n        -webkit-box-sizing: border-box;\n        overflow-x: hidden;\n        overflow-y: auto;\n    }\n\n    .ValidationSummary {\n        height: 100%;\n        width: 100%;\n        border: 1px solid #DA6F6F;\n        border-radius: 5px;\n        background: rgba(247, 227, 227, 1);\n        box-sizing: border-box;\n        -moz-box-sizing: border-box;\n        -webkit-box-sizing: border-box;\n        overflow: hidden;\n    }    \n\n    .SingleError{\n        white-space: normal;\n        max-height: 40px;\n        padding-top: 2px;\n    }\n    "],
        })
    ], ValidationSummary);
    return ValidationSummary;
}());
exports.ValidationSummary = ValidationSummary;
//# sourceMappingURL=ValidationSummary.js.map