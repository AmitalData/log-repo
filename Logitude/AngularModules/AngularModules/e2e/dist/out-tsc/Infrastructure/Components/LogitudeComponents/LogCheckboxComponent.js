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
var UIProperties_1 = require("./UIProperties");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var forms_1 = require("@angular/forms");
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var LogCheckboxComponent = /** @class */ (function () {
    function LogCheckboxComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ControlId = null;
        this.ShowHelp = false;
        this.ObjectFieldName = null;
        this.ObjectFieldHelp = null;
        this.ObjectTableName = null;
        this.HideColumns = false;
        this.NoObjectField = false;
        this.ValueChanged = new core_1.EventEmitter();
        this.show = false;
    }
    Object.defineProperty(LogCheckboxComponent.prototype, "IsDisabled", {
        get: function () {
            return this.isDisabled;
        },
        set: function (value) {
            this.isDisabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCheckboxComponent.prototype, "Value", {
        //value: boolean;
        get: function () {
            return this.boolValue;
        },
        set: function (newValue) {
            if (this.BoolValue != newValue) {
                //this.value = newValue;
                this.BoolValue = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCheckboxComponent.prototype, "BoolValue", {
        get: function () {
            return this.boolValue;
        },
        set: function (newValue) {
            var _this = this;
            var dataContextValue = this.DataContext[this.ObjectFieldName];
            if (!this.ObjectField) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
                if (table) {
                    this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.ObjectFieldName; })[0];
                }
            }
            if (this.ObjectField && this.ObjectField.IsCustom) {
                var customFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    dataContextValue = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }
            }
            if (this.boolValue != newValue) {
                if (typeof (newValue) == 'boolean') {
                    this.boolValue = newValue;
                    if (typeof (this.boolValue) == 'boolean') {
                        if (dataContextValue != this.boolValue) {
                            if (this.ObjectField && this.ObjectField.IsCustom) {
                                var customFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = this.boolValue + "";
                                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }
                            }
                            else {
                                this.DataContext[this.ObjectFieldName] = this.boolValue;
                            }
                            this.ValueChanged.emit(this.boolValue);
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCheckboxComponent.prototype, "Checked", {
        get: function () {
            return this.checked;
        },
        set: function (newValue) {
            if (this.checked != newValue) {
                this.checked = newValue;
                this.BoolValue = this.checked;
            }
        },
        enumerable: true,
        configurable: true
    });
    LogCheckboxComponent.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    LogCheckboxComponent.prototype.SetControlIds = function (baseIdCombination) {
        this.ControlId = baseIdCombination;
    };
    LogCheckboxComponent.prototype.ngOnInit = function () {
        var _this = this;
        //if (this.ValueChangedEvent) {
        //    this.ValueChangedEvent.subscribe((res) => {
        //        this.BoolValue = res;
        //    });
        //}
        var objectFieldAvailable = true;
        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }
        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }
        this.SetControlIds(baseIdCombination);
        //if (this.FocusOnMe) {// it means it is inside a grid.
        if (this.CurrentSession) {
            this.CopyValueSubs = this.CurrentSession.CopyCellIntoMemory.subscribe(function (id) {
                if (id == _this.ControlId) {
                    _this.CurrentSession.CopiedCell = _this.DataContext[_this.ObjectFieldName];
                }
            });
            if (this.CurrentSession.CopiedCell) {
                this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
                this.CurrentSession.CopiedCell = null;
            }
        }
        //}
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (table) {
            this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.ObjectFieldName; })[0];
            if (!this.ObjectField) {
                objectFieldAvailable = false;
            }
            else if (this.ObjectField.HelpTextCodeId != null) {
                this.ObjectFieldHelp = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectField.HelpTextTextCodeCode);
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                    if (this.ObjectFieldHelp.length > 1) {
                        this.ShowHelp = true;
                    }
                }
            }
        }
        else {
            objectFieldAvailable = false;
        }
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.IsDisabled = !this.uiProperty.IsEnabled;
        if (objectFieldAvailable) {
            this.uiProperty.UIPropertyChanged.subscribe(function (value) {
                if (value instanceof UIProperties_1.UIPropertyArgs) {
                    var uiPropertyArgs = value;
                    var uiProperty = uiPropertyArgs.uiProperty;
                    if (uiProperty.FieldName == _this.ObjectFieldName && uiProperty.ObjectTableName == _this.ObjectTableName) {
                        if (uiPropertyArgs.property == "IsEnabled") {
                            var isEnabled = uiPropertyArgs.newValue;
                            _this.IsDisabled = !isEnabled;
                            _this.uiProperty.IsEnabled = isEnabled;
                        }
                    }
                }
            });
        }
        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " CHECKBOX has no object field!");
        }
        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                this.BoolValue = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        else {
            this.BoolValue = this.DataContext[this.ObjectFieldName];
        }
    };
    LogCheckboxComponent.prototype.onFocus = function () {
    };
    LogCheckboxComponent.prototype.onBlur = function () {
    };
    LogCheckboxComponent.prototype.ngOnDestroy = function () {
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogCheckboxComponent.prototype, "NoObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean),
        __metadata("design:paramtypes", [Boolean])
    ], LogCheckboxComponent.prototype, "Value", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean),
        __metadata("design:paramtypes", [Boolean])
    ], LogCheckboxComponent.prototype, "Checked", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", forms_1.FormGroup)
    ], LogCheckboxComponent.prototype, "LogitudeForm", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogCheckboxComponent.prototype, "ValueChanged", void 0);
    LogCheckboxComponent = __decorate([
        core_1.Component({
            selector: 'LogCheckBox',
            template: "\n    <table *ngIf=\"uiProperty.IsVisible\">\n        <tr>\n            <td style=\"width: 16px;\">\n                <div class=\"CheckBox\">\n                    <input [attr.id]=\"ControlId\" type=\"checkbox\" [disabled]=\"IsDisabled\" [(ngModel)]=\"BoolValue\" (focus)=\"onFocus()\" (blur)=\"onBlur()\" />\n                    <label [attr.for]=\"ControlId\">{{Text}}</label>\n                </div>\n            </td>\n\n            <td style=\"width: 18px;\" *ngIf=\"!HideColumns\">                \n                <HelpIcon *ngIf=\"ShowHelp\" [HideHeader]=\"true\" [Text]=\"ObjectFieldHelp\" [IconSize]=\"15\"></HelpIcon>\n            </td>\n\n            <td>\n                <div></div>\n            </td>\n        </tr>\n    </table>\n    ",
            inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'HideColumns', 'IsDisabled', 'Text'],
        }),
        __metadata("design:paramtypes", [])
    ], LogCheckboxComponent);
    return LogCheckboxComponent;
}());
exports.LogCheckboxComponent = LogCheckboxComponent;
//# sourceMappingURL=LogCheckboxComponent.js.map