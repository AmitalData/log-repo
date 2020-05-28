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
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var DateAgeHelper_1 = require("../../../../Infrastructure/Utilities/DateAgeHelper");
var CustomFieldClass_1 = require("../../../../Infrastructure/DataContracts/CustomFieldClass");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var GeneratedDocumentCustomFieldComponent = /** @class */ (function (_super) {
    __extends(GeneratedDocumentCustomFieldComponent, _super);
    function GeneratedDocumentCustomFieldComponent() {
        var _this = _super.call(this) || this;
        _this.DateAgeHelper = new DateAgeHelper_1.DateAgeHelper(null);
        _this.HasError = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityPMService = new EntityPMService_1.EntityPMService();
        return _this;
    }
    GeneratedDocumentCustomFieldComponent.prototype.ngOnInit = function () {
        this.ObjectTableId = this.DocumentCustomArgs.ObjectTableId;
        this.ObjectTableName = this.DocumentCustomArgs.ObjectTableName;
        this.ScreenCode = this.DocumentCustomArgs.ScreenCode;
        this.DocumentCustomArgs.GeneratedDocumentCustomFieldComponent = this;
        this.LoadData();
    };
    GeneratedDocumentCustomFieldComponent.prototype.LoadData = function () {
        var _this = this;
        this.CustomFieldLists = [];
        var myScreenColumns = [];
        var myScreen = window.Screens.filter(function (x) { return x.ObjectTableId === _this.ObjectTableId && x.Code.toLowerCase() == _this.ScreenCode.toLowerCase(); })[0];
        if (myScreen != null) {
            var myScreenFields = window.ScreenFields.filter(function (x) { return x.ScreenId === myScreen.Id && x.Tenant === SessionInfo_1.SessionInfo.LoggedUserTenant; });
            if (myScreenFields.length == 0) {
                myScreenFields = window.ScreenFields.filter(function (x) { return x.ScreenId === myScreen.Id; });
            }
            if (myScreenFields.length == 0) {
                this.ShowNoFieldsText = true;
            }
            else {
                var myObjectFields = window.ObjectFields.filter(function (x) { return x.ObjectTableId === _this.ObjectTableId; });
                //this.OnCreateAutomationList = this.OnCreateAutomationList.sort((a, b) => { return a.Order - b.Order });
                myScreenFields.sort(function (a, b) { return a.Row - b.Row; }).forEach(function (screenFields) {
                    var myObjectField = myObjectFields.filter(function (f) { return f.Id == screenFields.ObjectFieldId; })[0];
                    if (myObjectField) {
                        _this.CustomFieldLists.push(new CustomFieldViewModel(myObjectField, _this.DocumentCustomArgs.EntityPM, _this.DocumentCustomArgs.EditCustomField, _this.ObjectTableName));
                    }
                });
            }
        }
    };
    GeneratedDocumentCustomFieldComponent.prototype.ValueChange = function (value, item) {
        var newValue;
        if (item.ObjectField.IsCustom) {
            newValue = value.FieldName ? value.ResolvedValue : value;
        }
        else
            newValue = value;
        if (item.FieldValue != newValue) {
            item.FieldValue = newValue;
            this.EditCustomField(item, true);
        }
    };
    GeneratedDocumentCustomFieldComponent.prototype.EditCustomField = function (item, ischange) {
        var _this = this;
        if (ischange === void 0) { ischange = false; }
        if (item.EntityPM) {
            if (item.FieldDataTypeCode == "Boolean") {
                item.FieldValue = !item.FieldValue;
            }
            if (item.ObjectField.IsCustom) {
                var oldvalue = item.EntityPM[item.FieldName];
                var newValue = new CustomFieldClass_1.CustomFieldClass(item.FieldValue, item.FieldName, this.ObjectTableName);
                var oldValueText = oldvalue ? oldvalue.Value : "";
                var newValueText = newValue ? newValue.Value : "";
                if (oldValueText != newValueText) {
                    item.EntityPM[item.FieldName] = newValue;
                    ischange = true;
                }
            }
            else if (item.EntityPM[item.FieldName] != item.FieldValue) {
                item.EntityPM[item.FieldName] = item.FieldValue;
                ischange = true;
            }
            if (ischange && item.EntityPM.IsDirty) {
                //Save
                this.DocumentCustomArgs.IsEditCustomField = true;
                this.DocumentCustomArgs.IsChangeCustomField = true;
                if (this.DocumentCustomArgs.editDocumentComponent != null) {
                    this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = [];
                }
                this.CurrentSession.StartBusyIndicatorSaving();
                this.entityPMService.update(this.ObjectTableName, item.EntityPM).then(function (res) {
                    res.subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.HasError = true;
                            if (_this.DocumentCustomArgs.editDocumentComponent != null) {
                                _this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = myResponse.ErrorsArray;
                            }
                        }
                        else {
                            _this.HasError = false;
                            item.EntityPM = myResponse.Result;
                            var objectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
                            if (objectTable && objectTable.CacheOnClient) {
                                CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                            }
                            if (_this.DocumentCustomArgs.editDocumentComponent != null) {
                                _this.DocumentCustomArgs.editDocumentComponent.LoadstimulData(null, true, true, 1, "GenerateReport", "", "Refreshing Document...");
                                _this.DocumentCustomArgs.IsChangeCustomField = false;
                            }
                        }
                    });
                });
            }
        }
    };
    GeneratedDocumentCustomFieldComponent.prototype.SetCustomFieldItem = function (item) {
        this.SelectedCustomFieldViewModel = item;
    };
    GeneratedDocumentCustomFieldComponent.prototype.GetDateFromString = function (datestring, fromat) {
        if (datestring) {
            var dateAndTime;
            var timeArray;
            var dateArray;
            var suffix;
            dateAndTime = datestring.split(' ');
            if (fromat == 2) {
                dateArray = dateAndTime[0].split('/');
                var day = Number(dateArray[0]);
                var month = Number(dateArray[1]) - 1;
                var year = Number(dateArray[2]);
                timeArray = dateAndTime[1].split(':');
                var hour = this.DateAgeHelper.GetTimeFor24Mode(Number(timeArray[0]), suffix);
                var minute = Number(timeArray[1]);
                var second = Number(timeArray[2]);
            }
            else {
                var month = Number(this.getMonthFromString(dateAndTime[1]));
                var day = Number(dateAndTime[2]);
                var year = Number(dateAndTime[3]);
                timeArray = dateAndTime[4].split(':');
                var hour = this.DateAgeHelper.GetTimeFor24Mode(Number(timeArray[0]), suffix);
                var minute = Number(timeArray[1]);
                var second = Number(timeArray[2]);
            }
            var date = this.DateAgeHelper.GetDate(year, month, day, hour, minute, second);
            return date;
        }
    };
    GeneratedDocumentCustomFieldComponent.prototype.getMonthFromString = function (mon) {
        var d = Date.parse(mon + "1, 2012");
        if (!isNaN(d)) {
            var x = new Date(d).getMonth();
            return x;
        }
        return -1;
    };
    GeneratedDocumentCustomFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'GeneratedDocumentCustomFieldComponent',
            templateUrl: './GeneratedDocumentCustomFieldComponent.html',
            inputs: ['DocumentCustomArgs'],
        }),
        __metadata("design:paramtypes", [])
    ], GeneratedDocumentCustomFieldComponent);
    return GeneratedDocumentCustomFieldComponent;
}(BaseComponent_1.BaseComponent));
exports.GeneratedDocumentCustomFieldComponent = GeneratedDocumentCustomFieldComponent;
var CustomFieldViewModel = /** @class */ (function () {
    function CustomFieldViewModel(objectField, entityPM, isEnableEditCustomField, objectTableName) {
        this.ObjectField = objectField;
        this.FieldName = objectField.FieldName;
        this.MultiLine = objectField.MultiLine;
        this.FieldDataTypeCode = objectField.DataTypeCode;
        this.IsEnableEditCustomField = isEnableEditCustomField;
        this.keyNo = Guid_1.Guid.newGuid();
        this.keyYes = Guid_1.Guid.newGuid();
        this.ObjectTableName = objectTableName;
        this.EntityPM = entityPM;
        this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);
        var value;
        if (entityPM) {
            if (objectField.IsCustom) {
                if (entityPM[this.FieldName]) {
                    value = entityPM[this.FieldName].ResolvedValue;
                }
            }
            else
                value = entityPM[this.FieldName];
        }
        if (value) {
            if (this.FieldDataTypeCode == "Boolean") {
                if (value.toString().toLowerCase() == "false") {
                    this.FieldValue = isEnableEditCustomField ? false : "No";
                }
                else
                    this.FieldValue = isEnableEditCustomField ? true : "Yes";
            }
            else {
                this.FieldValue = value;
            }
        }
        else {
            if (this.FieldDataTypeCode == "Boolean") {
                this.FieldValue = isEnableEditCustomField ? false : "No";
            }
        }
        if (this.FieldDataTypeCode == "LookUp") {
            //this.EntityPM.UIProperties.SetEnabled(this.ObjectField.FieldName, this.ObjectTableName, isEnableEditCustomField);
        }
    }
    return CustomFieldViewModel;
}());
exports.CustomFieldViewModel = CustomFieldViewModel;
//# sourceMappingURL=GeneratedDocumentCustomFieldComponent.js.map