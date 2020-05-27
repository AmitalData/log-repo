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
var RulesValidator_1 = require("../../Validators/RulesValidator");
var UIProperty = /** @class */ (function () {
    function UIProperty(FieldName, ObjectTableName) {
        this.FieldName = FieldName;
        this.ObjectTableName = ObjectTableName;
        this.UIPropertyChanged = new core_1.EventEmitter();
        this.IsEnabled = true;
        this.IsRequired = false;
        this.IsVisible = true;
        this.ValidValue = true;
        this.HasWarning = false;
    }
    Object.defineProperty(UIProperty.prototype, "IsEnabled", {
        get: function () {
            if (this.isEnabled == undefined) {
                this.isEnabled = true;
            }
            return this.isEnabled;
        },
        set: function (newValue) {
            this.isEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "IsRequired", {
        get: function () {
            if (this.isRequired == undefined) {
                this.isRequired = false;
            }
            return this.isRequired;
        },
        set: function (newValue) {
            this.isRequired = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "IsVisible", {
        get: function () {
            if (this.isVisible == undefined) {
                this.isVisible = true;
            }
            return this.isVisible;
        },
        set: function (newValue) {
            this.isVisible = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "ValidValue", {
        get: function () {
            if (this.validValue == undefined) {
                this.validValue = true;
            }
            return this.validValue;
        },
        set: function (newValue) {
            this.validValue = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "ValidationError", {
        get: function () {
            return this.validationError;
        },
        set: function (newValue) {
            this.validationError = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "ManualValidationError", {
        get: function () {
            return this.manualValidationError;
        },
        set: function (newValue) {
            this.manualValidationError = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "IsValidManually", {
        get: function () {
            return this.isValidManually;
        },
        set: function (newValue) {
            this.isValidManually = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UIProperty.prototype, "HasWarning", {
        get: function () {
            if (this.hasWarning == undefined) {
                this.hasWarning = false;
            }
            return this.hasWarning;
        },
        set: function (newValue) {
            this.hasWarning = newValue;
        },
        enumerable: true,
        configurable: true
    });
    UIProperty.prototype.ngOnInit = function () {
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], UIProperty.prototype, "UIPropertyChanged", void 0);
    return UIProperty;
}());
exports.UIProperty = UIProperty;
var UIProperties = /** @class */ (function () {
    // public EntityPM: any;
    function UIProperties(entity) {
        if (entity === void 0) { entity = null; }
        this.UIPropertyList = new Array();
        //this.EntityPM = entity;
    }
    UIProperties.prototype.GetUIProperty = function (fieldName, objectTableName, dataContext, applyRules) {
        if (applyRules === void 0) { applyRules = true; }
        if (!this._RulesValidator) {
            this._RulesValidator = new RulesValidator_1.RulesValidator();
        }
        var entityPM = dataContext;
        if (dataContext) {
            if (dataContext.EntityPM)
                entityPM = dataContext.EntityPM;
            var isNewEntity = (entityPM.OldEntityPM === null || entityPM.OldEntityPM === undefined);
            if (this._RulesValidator.IsNewEntity != isNewEntity) {
                this._RulesValidator.IsNewEntity = isNewEntity;
                this._RulesValidator.Initizialize();
            }
        }
        var uiProperty;
        uiProperty = this.UIPropertyList.filter(function (d) { return d.FieldName == fieldName && d.ObjectTableName == objectTableName; })[0];
        if (!uiProperty) {
            uiProperty = new UIProperty(fieldName, objectTableName);
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
            if (objectTable != null && objectTable != undefined) {
                var objectField = window.ObjectFields.filter(function (d) { return d.FieldName == fieldName && d.ObjectTableId == objectTable.Id; })[0];
                if (objectField != null && objectField != undefined) {
                    if (objectField.DisplayOnly || objectField.AutomaticField) {
                        uiProperty.IsEnabled = false;
                    }
                }
            }
            if (applyRules && entityPM) {
                this._RulesValidator.ApplyUnConditionalSetFieldRules(fieldName, entityPM, objectTableName);
                this._RulesValidator.ApplyConditionalBlockFieldRules(fieldName, entityPM, objectTableName, false, uiProperty);
                this._RulesValidator.ApplyUnConditionalBlockFieldRules(fieldName, entityPM, objectTableName, false, uiProperty);
                this._RulesValidator.ApplyRequiredFieldRules(fieldName, entityPM, objectTableName, this);
            }
            this.UIPropertyList.push(uiProperty);
        }
        if (uiProperty.IsValidManually == undefined) {
            uiProperty = this.RefreshUIProperty(fieldName, objectTableName);
        }
        return uiProperty;
    };
    UIProperties.prototype.RefreshUIProperty = function (fieldName, objectTableName) {
        var uiProperty;
        uiProperty = this.UIPropertyList.filter(function (d) { return d.FieldName == fieldName && d.ObjectTableName == objectTableName; })[0];
        var objectFieldAvailable = true;
        var field;
        var table;
        table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (table) {
            field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === fieldName; })[0];
            if (!field) {
                objectFieldAvailable = false;
            }
        }
        else {
            objectFieldAvailable = false;
        }
        if (objectFieldAvailable) {
            uiProperty.IsRequired = field.IsRequiered;
        }
        return uiProperty;
    };
    UIProperties.prototype.SetVisibility = function (fieldName, objectTableName, value) {
        if (value === void 0) { value = false; }
        var uiProperty = this.GetUIProperty(fieldName, objectTableName, null, false);
        uiProperty.IsVisible = value;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsVisible", value));
    };
    UIProperties.prototype.SetEnabled = function (fieldName, objectTableName, value) {
        if (value === void 0) { value = false; }
        var uiProperty = this.GetUIProperty(fieldName, objectTableName, null, false);
        uiProperty.IsEnabled = value;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsEnabled", value));
    };
    UIProperties.prototype.SetRequired = function (fieldName, objectTableName, value) {
        if (value === void 0) { value = false; }
        var translatedRequiredError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        var translatedFieldName = null;
        if (table) {
            var field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === fieldName; })[0];
            if (field) {
                translatedFieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(field.FullNameTextCodeCode);
            }
        }
        if (!translatedFieldName) {
            translatedFieldName = fieldName;
        }
        var fieldError = translatedRequiredError.replace("%FieldName", translatedFieldName);
        var uiProperty = this.GetUIProperty(fieldName, objectTableName, null, false);
        uiProperty.IsRequired = value;
        if (uiProperty.IsRequired) {
            uiProperty.IsValidManually = false;
            uiProperty.ManualValidationError = fieldError;
            uiProperty.ValidValue = false;
            uiProperty.ValidationError = fieldError;
        }
        else {
            uiProperty.IsValidManually = true;
            uiProperty.ManualValidationError = null;
            uiProperty.ValidValue = true;
            uiProperty.ValidationError = null;
        }
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsRequired", value));
    };
    UIProperties.prototype.SetValidity = function (fieldName, objectTableName, value, errorMessage) {
        if (value === void 0) { value = false; }
        var uiProperty = this.GetUIProperty(fieldName, objectTableName, null, false);
        uiProperty.IsValidManually = value;
        uiProperty.ManualValidationError = errorMessage;
        uiProperty.ValidValue = value;
        uiProperty.ValidationError = errorMessage;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "IsValid", value));
    };
    UIProperties.prototype.SetWarning = function (fieldName, objectTableName, value) {
        if (value === void 0) { value = false; }
        var uiProperty = this.GetUIProperty(fieldName, objectTableName, false);
        uiProperty.HasWarning = value;
        uiProperty.UIPropertyChanged.emit(new UIPropertyArgs(uiProperty, "HasWarning", value));
    };
    return UIProperties;
}());
exports.UIProperties = UIProperties;
var UIPropertyArgs = /** @class */ (function () {
    function UIPropertyArgs(uiProperty, property, newValue) {
        this.uiProperty = uiProperty;
        this.property = property;
        this.newValue = newValue;
    }
    return UIPropertyArgs;
}());
exports.UIPropertyArgs = UIPropertyArgs;
//# sourceMappingURL=UIProperties.js.map