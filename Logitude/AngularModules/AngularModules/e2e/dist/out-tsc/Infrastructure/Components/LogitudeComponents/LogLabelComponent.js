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
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var UIProperties_1 = require("./UIProperties");
var Tools_1 = require("../../Tools");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var LogLabelComponent = /** @class */ (function () {
    function LogLabelComponent() {
        this.HideColumns = false;
        this.IsSmallLabel = false;
        this.LabelColor = "#6E7172";
        this.LabelOpacity = 1;
        this.ShowWarning = false;
        this.NoValidation = false;
        this.NoObjectField = false;
        this.LayoutDirection = 'ltr';
        this.isFieldValid = true;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    Object.defineProperty(LogLabelComponent.prototype, "IsFieldValid", {
        get: function () { return this.isFieldValid; },
        set: function (newValue) {
            if (this.isFieldValid != newValue) {
                var hehehehe = this.ObjectFieldName;
                this.isFieldValid = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLabelComponent.prototype, "Text", {
        get: function () { return this.text; },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
                this.SetLabel();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLabelComponent.prototype, "TextCode", {
        get: function () { return this.textCode; },
        set: function (newValue) {
            if (this.textCode != newValue) {
                this.textCode = newValue;
                this.SetLabel();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLabelComponent.prototype, "Replace", {
        get: function () { return this.replace; },
        set: function (newValue) {
            if (this.replace != newValue) {
                this.replace = newValue;
                this.SetLabel();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLabelComponent.prototype, "ReplaceWith", {
        get: function () { return this.replaceWith; },
        set: function (newValue) {
            if (this.replaceWith != newValue) {
                this.replaceWith = newValue;
                this.SetLabel();
            }
        },
        enumerable: true,
        configurable: true
    });
    LogLabelComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.DataContext != null) {
            this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            this.objectfield = window.ObjectFields.filter(function (d) { return d.FieldName == _this.ObjectFieldName && d.ObjectTableName == _this.ObjectTableName; })[0];
            if (this.uiProperty != null) {
                this.uiProperty.UIPropertyChanged.subscribe(function (value) {
                    if (value instanceof UIProperties_1.UIPropertyArgs) {
                        var uiPropertyArgs = value;
                        var uiProperty = uiPropertyArgs.uiProperty;
                        if (uiProperty.FieldName == _this.ObjectFieldName && uiProperty.ObjectTableName == _this.ObjectTableName) {
                            if (uiPropertyArgs.property == "IsRequired" || uiPropertyArgs.property == "IsValid") {
                                if (!_this.NoValidation) {
                                    _this.uiProperty.IsRequired = uiProperty.IsRequired;
                                }
                            }
                            if (uiPropertyArgs.property == "HasWarning") {
                                if (uiProperty.HasWarning) {
                                    if (_this.DataContext[_this.ObjectFieldName]) {
                                        _this.ShowWarning = false;
                                    }
                                    else {
                                        _this.ShowWarning = true;
                                    }
                                }
                                else {
                                    _this.ShowWarning = false;
                                }
                            }
                        }
                    }
                    //if (uiProperty != "valuechanges") 
                    //    if (!this.NoValidation) {
                    //        this.uiProperty.IsRequired = uiProperty.IsRequired;
                    //    }
                    //    this.uiProperty.IsEnabled = uiProperty.IsEnabled;
                    //}
                    if (!_this.NoValidation) {
                        _this.Validate();
                    }
                });
                this.SetLabel();
                if (!this.NoValidation) {
                    this.Validate();
                }
            }
            if (!this.objectfield && !this.NoObjectField) {
                console.warn(this.ObjectFieldName + " LABEL has no object field!");
            }
        }
    };
    LogLabelComponent.prototype.SetLabel = function () {
        if (this.DataContext != null) {
            var labelText = "";
            if (this.Text != null) {
                labelText = this.Text + ":";
            }
            else if (this.TextCode != null) {
                labelText = TextCodeTranslator_1.TextCodeTranslator.Translate(this.TextCode) + ":";
            }
            else {
                if (this.objectfield != null) {
                    var textcodecode = this.objectfield.FullNameTextCodeCode;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.Replace)) {
                        if (this.objectfield.ShortNameTextCodeCode) {
                            textcodecode = this.objectfield.ShortNameTextCodeCode;
                        }
                    }
                    var translatedText = TextCodeTranslator_1.TextCodeTranslator.Translate(textcodecode);
                    if (Tools_1.AppTool.IsNullOrEmpty(translatedText)) {
                        textcodecode = this.objectfield.FullNameTextCodeCode;
                        translatedText = TextCodeTranslator_1.TextCodeTranslator.Translate(textcodecode);
                    }
                    labelText = translatedText + ":";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Replace)) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.ReplaceWith)) {
                        labelText = labelText.replace(this.Replace, "");
                    }
                    else {
                        labelText = labelText.replace(this.Replace, this.ReplaceWith);
                    }
                }
            }
            this.LabelText = labelText;
        }
    };
    LogLabelComponent.prototype.Validate = function () {
        var isValid = true;
        if (this.uiProperty != null) {
            if (this.DataContext != null) {
                if (this.uiProperty.IsValidManually == false) {
                    isValid = false;
                }
                else if (!this.uiProperty.ValidValue) {
                    isValid = false;
                }
                else if (this.uiProperty.IsRequired) {
                    if (this.objectfield) {
                        if (this.objectfield.DataTypeCode.toLowerCase() != 'boolean') {
                            //if (this.DataContext[this.ObjectFieldName] == null || this.DataContext[this.ObjectFieldName] == "") {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext[this.ObjectFieldName])) {
                                isValid = false;
                            }
                        }
                    }
                    else {
                        isValid = false;
                    }
                }
                //else if (!this.uiProperty.ValidValue) {
                //    isValid = false;
                //}
                else if (this.uiProperty.HasWarning) {
                    if (this.DataContext[this.ObjectFieldName]) {
                        this.ShowWarning = false;
                    }
                    else {
                        this.ShowWarning = true;
                    }
                }
                else {
                    if (this.objectfield != null) {
                        if (this.objectfield.DataTypeCode != null) {
                            switch (this.objectfield.DataTypeCode.toLowerCase()) {
                                case "text":
                                case "ntext": {
                                    if (!this.objectfield.IsMaxLength) {
                                        var fieldValueLength = 0;
                                        if (!Tools_1.AppTool.IsNullOrEmpty(this.DataContext[this.ObjectFieldName])) {
                                            fieldValueLength = this.DataContext[this.ObjectFieldName].length;
                                        }
                                        if (fieldValueLength > this.objectfield.MaxLength) {
                                            isValid = false;
                                        }
                                        else {
                                            if (this.objectfield.MinLength != 0) {
                                                if (fieldValueLength > 0) {
                                                    if (fieldValueLength < this.objectfield.MinLength) {
                                                        isValid = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        this.IsFieldValid = isValid;
        this.SetLabelStyle();
    };
    LogLabelComponent.prototype.SetLabelStyle = function () {
        var myColor = "#6E7172";
        var myOpacity = 1;
        if (this.uiProperty != null) {
            if (this.IsFieldValid) {
                myColor = "#6E7172";
            }
            else {
                myColor = "#E53030";
            }
            if (this.uiProperty.IsEnabled) {
                myOpacity = 1;
            }
            else {
                myOpacity = 0.5;
            }
        }
        this.LabelColor = myColor;
        //this.LabelOpacity = myOpacity;
        this.LabelOpacity = 1;
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLabelComponent.prototype, "NoObjectField", void 0);
    LogLabelComponent = __decorate([
        core_1.Component({
            selector: 'LogLabel',
            inputs: [
                'ObjectFieldName',
                'ObjectTableName',
                'DataContext',
                'Text',
                'TextCode',
                "HideColumns",
                "ShowWarning",
                "NoValidation",
                "IsSmallLabel",
                "Replace",
                "ReplaceWith",
            ],
            template: "\n    <table *ngIf=\"uiProperty!=null && uiProperty.IsVisible\" style=\"table-layout: fixed;\">\n        <tr [ngStyle]=\"{opacity: LabelOpacity}\">\n\n            <td style=\"width: 10px; vertical-align: middle;\" *ngIf=\"!HideColumns\">\n                <div style=\"width: 10px;\">\n                    <div style=\"width: 7px; height: 7px; background: #E45A26; -webkit-border-radius: 25px; -moz-border-radius: 25px; border-radius: 25px;\" *ngIf=\"!IsFieldValid\"></div>\n                    <div style=\"width: 7px; height: 7px; background: #FFCB00; -webkit-border-radius: 25px; -moz-border-radius: 25px; border-radius: 25px;\" *ngIf=\"IsFieldValid && ShowWarning\"></div>\n                </div>\n            </td>\n\n            <td class=\"TextTrimming\" style=\"vertical-align:middle;\"  [style.text-align]=\"LayoutDirection=='rtl' ? 'right' : 'left'\" *ngIf=\"!IsSmallLabel\">\n                <label class=\"Label\" [ngStyle]=\"{color: LabelColor}\">{{LabelText}}</label>\n            </td>\n\n            <td class=\"TextTrimming\" style=\"vertical-align:middle;\" [style.text-align]=\"LayoutDirection=='rtl' ? 'right' : 'left'\" *ngIf=\"IsSmallLabel\">\n                <label class=\"SmallLabel\" [ngStyle]=\"{color: LabelColor}\">{{LabelText}}</label>\n            </td>\n        </tr>\n    </table>\n    ",
        }),
        __metadata("design:paramtypes", [])
    ], LogLabelComponent);
    return LogLabelComponent;
}());
exports.LogLabelComponent = LogLabelComponent;
//# sourceMappingURL=LogLabelComponent.js.map