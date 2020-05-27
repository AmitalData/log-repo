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
var DateAgeHelper_1 = require("../../../../Infrastructure/Utilities/DateAgeHelper");
var DocumentTypeCustomFieldPMViewModel_1 = require("./ViewModel/DocumentTypeCustomFieldPMViewModel");
var DocumentTypeCustomFieldService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeCustomFieldService");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DocumentCustomFieldsComponent = /** @class */ (function (_super) {
    __extends(DocumentCustomFieldsComponent, _super);
    function DocumentCustomFieldsComponent(_documentTypeCustomFieldService) {
        var _this = _super.call(this) || this;
        _this._documentTypeCustomFieldService = _documentTypeCustomFieldService;
        _this.DateAgeHelper = new DateAgeHelper_1.DateAgeHelper(null);
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    DocumentCustomFieldsComponent.prototype.ngOnInit = function () {
        this.DocumentCustomArgs.DocumentCustomFields = this;
        this.LoadFormCustomFieldsByDocument();
    };
    DocumentCustomFieldsComponent.prototype.LoadFormCustomFieldsByDocument = function () {
        var _this = this;
        this.DocumentTypeCustomFieldLists = [];
        this.DocumentCustomArgs.DocumentTypeCustomFieldLists.forEach(function (item) {
            _this.DocumentTypeCustomFieldLists.push(new DocumentTypeCustomFieldPMViewModel_1.DocumentTypeCustomFieldPMViewModel(item, _this));
        });
        this._documentTypeCustomFieldService.getFormCustomFieldsByDocument(SessionInfo_1.SessionInfo.LoggedUserTenant, this.DocumentCustomArgs.DocumentTypeId, this.DocumentCustomArgs.EntityId, this.DocumentCustomArgs.ObjectTableId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.FormCustomFieldPMLists = myResult;
                    _this.DocumentTypeCustomFieldLists.forEach(function (item) {
                        var value = _this.FormCustomFieldPMLists.filter(function (d) { return d.FieldCode == item.FieldCode; })[0].Value;
                        if (!value) {
                            if ((item.FieldDataTypeCode == "Date" || item.FieldDataTypeCode == "DateTime") && _this.DocumentCustomArgs.EditCustomField) {
                                item.IsLoad = true;
                            }
                            else
                                value = " ";
                        }
                        else if (item.FieldDataTypeCode == "Boolean") {
                            if (value.toLocaleLowerCase() == "false") {
                                if (_this.DocumentCustomArgs.EditCustomField) {
                                    item.FieldValue = false;
                                }
                                else {
                                    item.FieldValue = "No";
                                }
                            }
                            else if (value.toLocaleLowerCase() == "true") {
                                if (_this.DocumentCustomArgs.EditCustomField) {
                                    item.FieldValue = true;
                                }
                                else {
                                    item.FieldValue = "Yes";
                                }
                            }
                        }
                        else if ((item.FieldDataTypeCode == "Date" || item.FieldDataTypeCode == "DateTime") && _this.DocumentCustomArgs.EditCustomField) {
                            if (value.indexOf('GMT') > -1) {
                                item.FieldValue = _this.GetDateFromString(value.split('GMT')[0], 1);
                            }
                            else if (value.indexOf('T') > -1) {
                                item.FieldValue = _this.DateAgeHelper.GetDateFromString(value);
                            }
                            else {
                                item.FieldValue = _this.GetDateFromString(value, 2);
                            }
                            item.IsLoad = true;
                        }
                        else {
                            item.FieldValue = value;
                        }
                    });
                }
            }
        });
    };
    DocumentCustomFieldsComponent.prototype.EditCustomField = function (item) {
        this.DocumentCustomArgs.IsEditCustomField = true;
        this.DocumentCustomArgs.IsChangeCustomField = true;
        var fromCustomFieldPM = this.FormCustomFieldPMLists.filter(function (d) { return d.FieldCode == item.FieldCode; })[0];
        if (item.FieldDataTypeCode == "Boolean") {
            item.FieldValue = !item.FieldValue;
            if (item.FieldValue)
                fromCustomFieldPM.Value = "True";
            else
                fromCustomFieldPM.Value = "False";
            this.SaveAndRefresh(fromCustomFieldPM);
        }
        else {
            if (fromCustomFieldPM.Value != item.FieldValue) {
                fromCustomFieldPM.Value = item.FieldValue;
                this.SaveAndRefresh(fromCustomFieldPM);
            }
        }
    };
    DocumentCustomFieldsComponent.prototype.ValueDateChange = function (date, item) {
        if (item.FieldValue != date) {
            item.FieldValue = date;
            this.DocumentCustomArgs.IsEditCustomField = true;
            this.DocumentCustomArgs.IsChangeCustomField = true;
            var fromCustomFieldPM = this.FormCustomFieldPMLists.filter(function (d) { return d.FieldCode == item.FieldCode; })[0];
            if (item.FieldDataTypeCode == "DateTime" || item.FieldDataTypeCode == "Date") {
                fromCustomFieldPM.Value = item.FieldValue.toString();
                this.SaveAndRefresh(fromCustomFieldPM);
            }
        }
    };
    DocumentCustomFieldsComponent.prototype.SetCustomFieldItem = function (item) {
        this.SelectedDocumentTypeCustomFieldPMViewModel = item;
    };
    DocumentCustomFieldsComponent.prototype.SaveAndRefresh = function (item) {
        var _this = this;
        if (this.ValidateCustomFields().length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicator("Saving...");
        if (this.DocumentCustomArgs.editDocumentComponent != null) {
            this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = [];
        }
        this._documentTypeCustomFieldService.UpdateFormCustomField(item).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            if (res.HasError) {
                if (_this.DocumentCustomArgs.editDocumentComponent != null) {
                    _this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = res.ErrorsArray;
                }
            }
            if (_this.DocumentCustomArgs.editDocumentComponent != null) {
                _this.DocumentCustomArgs.editDocumentComponent.LoadstimulData(null, true, true, 1, "GenerateReport", "", "Refreshing Document...");
                _this.DocumentCustomArgs.IsChangeCustomField = false;
            }
        });
    };
    DocumentCustomFieldsComponent.prototype.ValidateCustomFields = function () {
        var _this = this;
        var ValidationErrorsList = [];
        if (this.DocumentCustomArgs.editDocumentComponent != null) {
            this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = [];
        }
        //this.ValidationErrorsList = [];
        this.FormCustomFieldPMLists.forEach(function (cf) {
            if (!Tools_1.AppTool.IsNullOrEmpty(cf.Value) && cf.Value.length > 250) {
                var translation = cf.FieldCode;
                var field = _this.DocumentTypeCustomFieldLists.filter(function (f) { return f.FieldCode == cf.FieldCode; })[0];
                if (field) {
                    var ft = TextCodeTranslator_1.TextCodeTranslator.TranslateCached(field.Name);
                    if (!Tools_1.AppTool.IsNullOrEmpty(ft)) {
                        translation = ft;
                    }
                    //var trans = TextCodeTranslator.
                }
                var error = translation + " Maximum length should be less than 1000";
                if (_this.DocumentCustomArgs.editDocumentComponent != null) {
                    ValidationErrorsList.push(error);
                }
                //            DocumentTypeCustomFieldPM field = DocumentCustomFieldsList.FirstOrDefault(f => f.FieldCode == cf.FieldCode);
                //            if (field != null) {
                //                FieldsTranslations ft = FieldsTranslationsCachedDataProvider.GetFieldsTranslations(field.Name);
                //                if (ft != null) {
                //                    translation = ft.TranslatedText;
                //                }
                //                else {
                //                    translation = field.Name;
                //                }
                //            }
                //            ValidationResult error = new ValidationResult(translation + " Maximum length should be less than 1000");
                //            errors.Add(error);
            }
        });
        this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = ValidationErrorsList;
        return ValidationErrorsList;
        //for (FormCustomFieldPM cf in this.FormCustomFieldPMLists) {
        //    if (!AppTool.IsNullOrEmpty(cf.v) && cf.Value.Length > 250) {
        //    }
        //}
    };
    //public List<ValidationResult> ValidateCustomFields() {
    //    List < ValidationResult > errors = new List<ValidationResult>();
    //    foreach(FormCustomFieldPM cf in fromCustomFieldsList)
    //    {
    //        if (!string.IsNullOrEmpty(cf.Value) && cf.Value.Length > 250) {
    //            string translation = cf.FieldCode;
    //            DocumentTypeCustomFieldPM field = DocumentCustomFieldsList.FirstOrDefault(f => f.FieldCode == cf.FieldCode);
    //            if (field != null) {
    //                FieldsTranslations ft = FieldsTranslationsCachedDataProvider.GetFieldsTranslations(field.Name);
    //                if (ft != null) {
    //                    translation = ft.TranslatedText;
    //                }
    //                else {
    //                    translation = field.Name;
    //                }
    //            }
    //            ValidationResult error = new ValidationResult(translation + " Maximum length should be less than 1000");
    //            errors.Add(error);
    //        }
    //    }
    //    if (errors.Count > 0) {
    //        this.previewControl.FillErrors(errors);
    //    }
    //    return errors;
    //}
    DocumentCustomFieldsComponent.prototype.GetDateFromString = function (datestring, fromat) {
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
    DocumentCustomFieldsComponent.prototype.getMonthFromString = function (mon) {
        var d = Date.parse(mon + "1, 2012");
        if (!isNaN(d)) {
            var x = new Date(d).getMonth();
            return x;
        }
        return -1;
    };
    DocumentCustomFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentCustomFields',
            templateUrl: './DocumentCustomFieldsComponent.html',
            inputs: ['DocumentCustomArgs'],
            providers: [DocumentTypeCustomFieldService_1.DocumentTypeCustomFieldService],
        }),
        __metadata("design:paramtypes", [DocumentTypeCustomFieldService_1.DocumentTypeCustomFieldService])
    ], DocumentCustomFieldsComponent);
    return DocumentCustomFieldsComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentCustomFieldsComponent = DocumentCustomFieldsComponent;
//# sourceMappingURL=DocumentCustomFieldsComponent.js.map