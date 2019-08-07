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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var ObjectFieldPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var ObjectFieldPM_1 = require("../../../../Infrastructure/EntityPMs/ObjectFieldPM");
var EditStandardFieldComponent = /** @class */ (function (_super) {
    __extends(EditStandardFieldComponent, _super);
    function EditStandardFieldComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.EntityPMLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsMaxLengthEnabled = true;
        _this.IsControlFieldsVisible = false;
        _this.IsFullLabelEnabled = false;
        _this.IsShortLabelEnabled = false;
        _this.IsListHeaderLabelEnabled = false;
        _this.IsHelpTextEnabled = false;
        _this.IsRequieredEnabled = true;
        _this.EntityPM = new ObjectFieldPM_1.ObjectFieldPM();
        _this.myService = new ObjectFieldPMService_1.ObjectFieldPMService();
        _this.generalService = new GeneralDomainService_1.GeneralDomainService();
        return _this;
    }
    EditStandardFieldComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EditedFieldItem = args;
        this.generalService.GetSingleObjectFieldFromZeroTenant(args.ObjectFieldId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                _this.EntityPMLoaded = true;
                _this.SetUIProperties();
                _this.BuildContolFieldsLists();
            }
        });
    };
    EditStandardFieldComponent.prototype.SetUIProperties = function () {
        var isMaxLengthEnabled = true;
        var isControlFieldsVisible = false;
        var isFullLabelEnabled = false;
        var isShortLabelEnabled = false;
        var isListHeaderLabelEnabled = false;
        var isHelpTextEnabled = false;
        var isRequieredEnabled = true;
        if (this.EntityPM.DataTypeCode == "LookUp" || this.EntityPM.DataTypeCode == "DateTime") {
            isMaxLengthEnabled = false;
        }
        if (this.EntityPM.DataTypeCode == "LookUp") {
            isControlFieldsVisible = true;
        }
        if (this.EditedFieldItem.fullLabelObject != null) {
            isFullLabelEnabled = true;
        }
        if (this.EditedFieldItem.shortLabelObject != null) {
            isShortLabelEnabled = true;
        }
        if (this.EditedFieldItem.listLabelObject != null) {
            isListHeaderLabelEnabled = true;
        }
        if (this.EditedFieldItem.helpLabelObject != null) {
            isHelpTextEnabled = true;
        }
        if (this.EntityPM.TenantZeroIsRequired) {
            isRequieredEnabled = false;
        }
        this.IsMaxLengthEnabled = isMaxLengthEnabled;
        this.IsControlFieldsVisible = isControlFieldsVisible;
        this.IsFullLabelEnabled = isFullLabelEnabled;
        this.IsShortLabelEnabled = isShortLabelEnabled;
        this.IsListHeaderLabelEnabled = isListHeaderLabelEnabled;
        this.IsHelpTextEnabled = isHelpTextEnabled;
        this.IsRequieredEnabled = isRequieredEnabled;
    };
    EditStandardFieldComponent.prototype.BuildContolFieldsLists = function () {
        var _this = this;
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];
        var lookupTable = window.ObjectTables.filter(function (t) { return t.Id == _this.EntityPM.LookUpTableId; })[0];
        if (lookupTable != null) {
            this.ContolFieldsList1 = this.EditedFieldItem.loadedFields.filter(function (f) { return f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == _this.EntityPM.ObjectTableId; });
            this.ContolFieldsList2 = this.EditedFieldItem.loadedFields.filter(function (f) { return f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == _this.EntityPM.ObjectTableId; });
        }
    };
    Object.defineProperty(EditStandardFieldComponent.prototype, "SelectedControlFieldItem1", {
        get: function () {
            var _this = this;
            var controlField1 = null;
            if (this.EntityPM.ControlField1 != null) {
                controlField1 = this.ContolFieldsList1.filter(function (f) { return f.FieldName == _this.EntityPM.ControlField1; })[0];
            }
            return controlField1;
        },
        set: function (newValue) {
            var controlField1 = newValue;
            if (controlField1 != null) {
                this.EntityPM.ControlField1 = controlField1.FieldName;
            }
            else {
                this.EntityPM.ControlField1 = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "SelectedControlFieldItem2", {
        get: function () {
            var _this = this;
            var controlField2 = null;
            if (this.EntityPM.ControlField2 != null) {
                controlField2 = this.ContolFieldsList2.filter(function (f) { return f.FieldName == _this.EntityPM.ControlField2; })[0];
            }
            return controlField2;
        },
        set: function (newValue) {
            var controlField2 = newValue;
            if (controlField2 != null) {
                this.EntityPM.ControlField2 = controlField2.FieldName;
            }
            else {
                this.EntityPM.ControlField2 = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "FullLabelText", {
        get: function () { return this.EditedFieldItem.fullLabelObject == null ? "" : this.EditedFieldItem.fullLabelObject.TranslatedText; },
        set: function (newValue) {
            if (this.EditedFieldItem.fullLabelObject != null) {
                if (this.EditedFieldItem.fullLabelObject.TranslatedText != newValue) {
                    this.EditedFieldItem.fullLabelObject.TranslatedText = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "ShortLabelText", {
        get: function () { return this.EditedFieldItem.shortLabelObject == null ? "" : this.EditedFieldItem.shortLabelObject.TranslatedText; },
        set: function (newValue) {
            if (this.EditedFieldItem.shortLabelObject != null) {
                if (this.EditedFieldItem.shortLabelObject.TranslatedText != newValue) {
                    this.EditedFieldItem.shortLabelObject.TranslatedText = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "ListHeaderLabelText", {
        get: function () { return this.EditedFieldItem.listLabelObject == null ? "" : this.EditedFieldItem.listLabelObject.TranslatedText; },
        set: function (newValue) {
            if (this.EditedFieldItem.listLabelObject != null) {
                if (this.EditedFieldItem.listLabelObject.TranslatedText != newValue) {
                    this.EditedFieldItem.listLabelObject.TranslatedText = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "HelpTextText", {
        get: function () { return this.EditedFieldItem.helpLabelObject == null ? "" : this.EditedFieldItem.helpLabelObject.TranslatedText; },
        set: function (newValue) {
            if (this.EditedFieldItem.helpLabelObject != null) {
                if (this.EditedFieldItem.helpLabelObject.TranslatedText != newValue) {
                    this.EditedFieldItem.helpLabelObject.TranslatedText = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "IsRequiered", {
        get: function () { return this.EntityPM.IsRequiered; },
        set: function (newValue) {
            if (this.EntityPM.IsRequiered != newValue) {
                this.EntityPM.IsRequiered = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditStandardFieldComponent.prototype, "MaxLength", {
        get: function () { return this.EntityPM.MaxLength; },
        set: function (newValue) {
            if (this.EntityPM.MaxLength != newValue) {
                this.EntityPM.MaxLength = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditStandardFieldComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditStandardFieldComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "ObjectField", errors);
        if (this.EntityPM.TenantZeroMaxLength < this.EntityPM.MaxLength) {
            errors.push("Max Length can't be over " + this.EntityPM.TenantZeroMaxLength);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var list = [];
            if (this.EditedFieldItem.fullLabelObject != null && this.EditedFieldItem.fullLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.fullLabelObject);
            }
            if (this.EditedFieldItem.shortLabelObject != null && this.EditedFieldItem.shortLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.shortLabelObject);
            }
            if (this.EditedFieldItem.listLabelObject != null && this.EditedFieldItem.listLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.listLabelObject);
            }
            if (this.EditedFieldItem.helpLabelObject != null && this.EditedFieldItem.helpLabelObject.IsDirty) {
                list.push(this.EditedFieldItem.helpLabelObject);
            }
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myService.update(this.EntityPM).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        if (list.length == 0) {
                            CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                                _this.CurrentSession.StopBusyIndicator();
                                _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                            });
                        }
                    }
                });
            }
            if (list.length > 0) {
                var myServiceHelper = new GeneralDomainService_1.FieldsUpdateHelper();
                myServiceHelper.Tenant = SessionLocator_1.SessionLocator.Tenant;
                myServiceHelper.Items = list;
                var generalService = new GeneralDomainService_1.GeneralDomainService();
                generalService.UpdateFieldsTranslations(myServiceHelper).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                        });
                    }
                });
            }
        }
    };
    EditStandardFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditStandardFieldComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditStandardFieldComponent);
    return EditStandardFieldComponent;
}(BaseComponent_1.BaseComponent));
exports.EditStandardFieldComponent = EditStandardFieldComponent;
//# sourceMappingURL=EditStandardFieldComponent.js.map