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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypeCustomFieldService_1 = require("../../../../../Common/Services/ExtendedPMs/DocumentTypeCustomFieldService");
var ClassLevelValidator_1 = require("../../../../../Infrastructure/Validators/ClassLevelValidator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var DocumentTypeCustomFieldsViewModel_1 = require("../ViewModel/DocumentTypeCustomFieldsViewModel");
var AddEditDocumentTypeCustomFieldComponent = /** @class */ (function (_super) {
    __extends(AddEditDocumentTypeCustomFieldComponent, _super);
    function AddEditDocumentTypeCustomFieldComponent(entityArgs, _documentTypeCustomFieldService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._documentTypeCustomFieldService = _documentTypeCustomFieldService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        return _this;
    }
    AddEditDocumentTypeCustomFieldComponent.prototype.ngOnInit = function () {
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.Run = function () {
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args.EntityPM;
        this.DataViewModel = args.DataViewModel;
        this.Mode = args.Mode;
        this.FieldDataTypeLists = args.FieldDataTypeLists;
        this.MultiLine = this.EntityPM.MultiLine;
        if (this.FieldDataTypeLists) {
            this.SelectedFieldDataTypeCode = this.FieldDataTypeLists.filter(function (d) { return d.Code == _this.EntityPM.FieldDataTypeCode; })[0];
            if (this.Mode == "Add") {
                this.SelectedFieldDataTypeCode = this.FieldDataTypeLists[0];
                if (this.SelectedFieldDataTypeCode) {
                    this.EntityPM.FieldDataTypeCode = this.SelectedFieldDataTypeCode.Code;
                }
            }
        }
        this.ShowMultiLineCheckBox = this.MultiLine;
        this.name = this.EntityPM.Name;
        this.datatype = this.EntityPM.FieldDataTypeCode;
        this.defultvalue = this.EntityPM.DefaultValue;
        this.inactive = this.EntityPM.InActive;
        this.required = this.EntityPM.IsRequired;
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.ComboBoxFieldDataTypeCodeValueChanged = function (event) {
        if (event) {
            this.SelectedFieldDataTypeCode = event;
            this.EntityPM.FieldDataTypeCode = event.Code;
            if (this.EntityPM.FieldDataTypeCode == "Text") {
                this.ShowMultiLineCheckBox = true;
            }
            else {
                this.MultiLine = false;
                this.ShowMultiLineCheckBox = false;
            }
        }
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.MultiLineChange = function () {
        this.MultiLine = !this.MultiLine;
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.EntityPM.Name) {
            if (!this.EntityPM.FieldCode) {
                this.EntityPM.FieldCode = this.EntityPM.Name.replace(" ", "");
            }
        }
        var errorsArray = this.validator.Validate("DocumentTypeCustomField", this.EntityPM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            if (this.Mode == "Add") {
                this._documentTypeCustomFieldService.Insert(this.EntityPM).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (_this.DataViewModel && _this.DataViewModel.CustomFieldsLists) {
                            var item = new DocumentTypeCustomFieldsViewModel_1.DocumentTypeCustomFieldsViewModel(myResult);
                            _this.DataViewModel.CustomFieldsLists.push(item);
                            _this.DataViewModel.SelectedDocumentTypeCustomFieldsViewModel = item;
                        }
                    }
                    _this.Close();
                });
            }
            else if (this.Mode == "Edit") {
                this._documentTypeCustomFieldService.update(this.EntityPM).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (_this.DataViewModel && _this.DataViewModel.CustomFieldsLists) {
                            _this.DataViewModel.CustomFieldsLists = _this.DataViewModel.CustomFieldsLists.filter(function (d) { return d.Id != myResult.Id; });
                            var item = new DocumentTypeCustomFieldsViewModel_1.DocumentTypeCustomFieldsViewModel(myResult);
                            _this.DataViewModel.CustomFieldsLists.push(item);
                            _this.DataViewModel.SelectedDocumentTypeCustomFieldsViewModel = item;
                        }
                    }
                    _this.Close();
                });
            }
        }
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.CancelButtonClicked = function () {
        if (this.Mode == "Edit") {
            this.EntityPM.Name = this.name;
            this.EntityPM.FieldDataTypeCode = this.datatype;
            this.EntityPM.DefaultValue = this.defultvalue;
            this.EntityPM.InActive = this.inactive;
            this.EntityPM.IsRequired = this.required;
        }
        this.Close();
    };
    AddEditDocumentTypeCustomFieldComponent.prototype.Close = function () {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditDocumentTypeCustomFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddEditDocumentTypeCustomField',
            templateUrl: './AddEditDocumentTypeCustomFieldComponent.html',
            providers: [DocumentTypeCustomFieldService_1.DocumentTypeCustomFieldService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, DocumentTypeCustomFieldService_1.DocumentTypeCustomFieldService])
    ], AddEditDocumentTypeCustomFieldComponent);
    return AddEditDocumentTypeCustomFieldComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditDocumentTypeCustomFieldComponent = AddEditDocumentTypeCustomFieldComponent;
//# sourceMappingURL=AddEditDocumentTypeCustomFieldComponent.js.map