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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CustomerFieldsUpdateSettingPMService_1 = require("../../../Services/StandardPMs/CustomerFieldsUpdateSettingPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AddEditCustomerFieldsUpdateSettingComponent = /** @class */ (function (_super) {
    __extends(AddEditCustomerFieldsUpdateSettingComponent, _super);
    function AddEditCustomerFieldsUpdateSettingComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.customerFieldsUpdateSettingPMService = new CustomerFieldsUpdateSettingPMService_1.CustomerFieldsUpdateSettingPMService();
        _this.IsNewEntity = false;
        _this.ObjectTableName = "CustomerFieldsUpdateSetting";
        _this.LabelColumnWidth = 100;
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.UpdateDirectionLists = [];
        _this.UpdateDirectionLists.push(new UpdateDirection("NOUP", "No Update"));
        _this.UpdateDirectionLists.push(new UpdateDirection("UNFU", "UNF updates Logitude"));
        return _this;
    }
    Object.defineProperty(AddEditCustomerFieldsUpdateSettingComponent.prototype, "SelectedUpdateDirection", {
        get: function () {
            var _this = this;
            if (this.EntityPM) {
                this.selectedUpdateDirection = this.UpdateDirectionLists.filter(function (t) { return t.Code === _this.EntityPM.UpdateDirection; })[0];
            }
            return this.selectedUpdateDirection;
        },
        set: function (newValue) {
            if (this.selectedUpdateDirection != newValue) {
                this.selectedUpdateDirection = newValue;
                this.EntityPM.UpdateDirection = newValue.Code;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomerFieldsUpdateSettingComponent.prototype, "SelectedObjectField", {
        get: function () {
            var _this = this;
            if (this.EntityPM) {
                this.selectedObjectField = this.ObjectFieldPMLists.filter(function (t) { return t.Id === _this.EntityPM.ObjectFieldId; })[0];
            }
            return this.selectedObjectField;
        },
        set: function (newValue) {
            if (this.selectedObjectField != newValue) {
                this.selectedObjectField = newValue;
                this.EntityPM.ObjectFieldId = newValue.Id;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditCustomerFieldsUpdateSettingComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (args != null) {
            this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res1) {
                _this.entityResourceService.getEntityResourceByTableName("Customer").subscribe(function (res2) {
                    var objectTablePM = window.ObjectTables.filter(function (d) { return d.Name === "Customer"; })[0];
                    var tableObjectFieldPM = window.ObjectFields.filter(function (d) { return d.ObjectTableId === objectTablePM.Id; });
                    var m = tableObjectFieldPM.filter(function (f) { return f.FieldName === "SalesmanUserId"; })[0];
                    _this.ObjectFieldPMLists = window.ObjectFields.filter(function (d) { return d.AllowedInCustomerFieldsSettings === true && d.ObjectTableId === objectTablePM.Id; });
                    _this.EntityPM = args.EntityPM;
                    _this.EntityId = args.EntityId;
                    if (args.IsNew) {
                        _this.IsNewEntity = true;
                        _this.EntityPM = _this.customerFieldsUpdateSettingPMService.GetNewEntityPM();
                        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        _this.IsResourcesReady = true;
                    }
                    else {
                        _this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityId)) {
                            _this.customerFieldsUpdateSettingPMService.get(_this.EntityId).subscribe(function (response) {
                                _this.CurrentSession.StopBusyIndicator();
                                var pmResponse = response;
                                if (!pmResponse.HasError && pmResponse.Result) {
                                    _this.EntityPM = pmResponse.Result;
                                    _this.SelectedObjectField = _this.ObjectFieldPMLists.filter(function (d) { return d.Id == _this.EntityPM.ObjectFieldId; })[0];
                                    _this.SelectedUpdateDirection = _this.UpdateDirectionLists.filter(function (t) { return t.Code === _this.EntityPM.UpdateDirection; })[0];
                                }
                                _this.IsResourcesReady = true;
                            });
                        }
                    }
                });
            });
        }
    };
    AddEditCustomerFieldsUpdateSettingComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //// Required check
        //if (AppTool.IsNullOrEmpty(this.DepositBankAccountId) || AppTool.IsNullOrEmpty(this.CashBookId)) {
        //    errors.push("All Fields Required!");
        //} else {
        //    this.CheckIfThereIsCheques(this.CashBookId);
        //}
        // if (errors.length > 0) {
        this.ValidationErrorsList = errors;
        // }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            if (this.IsNewEntity) {
                this.customerFieldsUpdateSettingPMService.insert(this.EntityPM).subscribe(function (response) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        _this.ValidationErrorsList = response.ErrorsArray;
                    }
                });
            }
            else {
                this.customerFieldsUpdateSettingPMService.update(this.EntityPM).subscribe(function (response) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (!response.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit(response.Result.Id);
                    }
                    else {
                        _this.ValidationErrorsList = response.ErrorsArray;
                    }
                });
            }
        }
    };
    AddEditCustomerFieldsUpdateSettingComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCustomerFieldsUpdateSettingComponent = __decorate([
        core_1.Component({
            selector: 'AccountingSettingsComponent',
            moduleId: module.id,
            templateUrl: './AddEditCustomerFieldsUpdateSettingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditCustomerFieldsUpdateSettingComponent);
    return AddEditCustomerFieldsUpdateSettingComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomerFieldsUpdateSettingComponent = AddEditCustomerFieldsUpdateSettingComponent;
var UpdateDirection = /** @class */ (function () {
    function UpdateDirection(Code, Name) {
        this.Code = Code;
        this.Name = Name;
    }
    return UpdateDirection;
}());
exports.UpdateDirection = UpdateDirection;
//# sourceMappingURL=AddEditCustomerFieldsUpdateSettingComponent.js.map