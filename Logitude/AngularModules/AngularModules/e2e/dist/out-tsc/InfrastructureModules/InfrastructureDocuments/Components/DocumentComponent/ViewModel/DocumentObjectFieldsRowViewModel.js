"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ObjectFieldPM_1 = require("../../../../../Infrastructure/EntityPMs/ObjectFieldPM");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DocumentObjectFieldsRowViewModel = /** @class */ (function () {
    function DocumentObjectFieldsRowViewModel(objectField, resultFieldName, objectFieldType) {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        if (objectField) {
            this.CurrentObjectField = objectField;
            this.FieldName = objectField.FieldName;
            this.ResultFieldName = resultFieldName;
            this.ObjectFieldType = objectFieldType;
            this.Id = objectField.Id;
            if (!this.Id) {
                this.Id = Guid.newGuid();
            }
            var result = "";
            if ((objectField.FieldName == "Date" || objectField.FieldName == "Time") && objectField.DataTypeCode == "None") {
                result = objectField.FieldName;
            }
            else {
                if (this.CurrentObjectField.ShortNameTextCodeId) {
                    result = TextCodeTranslator_1.TextCodeTranslator.Translate(objectField.ShortNameTextCodeCode);
                }
                else {
                    result = TextCodeTranslator_1.TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);
                }
            }
            if (result) {
                this.TranslatedText = result;
            }
            else {
                this.TranslatedText = this.FieldName;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TranslatedText)) {
                if (this.TranslatedText.indexOf("(%") > -1 && this.TranslatedText.indexOf("Code)") > -1) {
                    this.TranslatedText = this.TranslatedText.split('(%')[0];
                }
            }
            this.FullNameTextCodeCode = objectField.FullNameTextCodeCode;
            if (objectField.DataTypeCode == "LookUp" || objectField.IsMulti || objectField.DataTypeCode == "DateTime") {
                this.HasTree = true;
            }
            else {
                this.HasTree = false;
            }
        }
    }
    DocumentObjectFieldsRowViewModel.prototype.Load = function () {
        var _this = this;
        this.Items = new Array();
        var list = new Array();
        var views = new Array();
        if (this.CurrentObjectField.IsMulti || this.CurrentObjectField.DataTypeCode == "LookUp") {
            var table = null;
            if (this.CurrentObjectField.IsMulti) {
                table = window.ObjectTables.filter(function (d) { return d.Id == _this.CurrentObjectField.MultiTableId; })[0];
            }
            else {
                table = window.ObjectTables.filter(function (d) { return d.Id == _this.CurrentObjectField.LookUpTableId; })[0];
            }
            this._entityResourceService.getEntityResourceByTableName(table.Name).subscribe(function (response) {
                if (_this.CurrentObjectField.IsMulti) {
                    table = window.ObjectTables.filter(function (d) { return d.Id == _this.CurrentObjectField.MultiTableId; })[0];
                    list = window.ObjectFields.filter(function (f) { return f.ObjectTableId == table.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                }
                else if (_this.CurrentObjectField.DataTypeCode == "LookUp") {
                    table = window.ObjectTables.filter(function (d) { return d.Id == _this.CurrentObjectField.LookUpTableId; })[0];
                    if (!_this.DisplayListOnly) {
                        if (_this.ObjectFieldType == "LookUpContactOrUser" || _this.ObjectFieldType == "Emails") {
                            list = window.ObjectFields.filter(function (f) { return f.ObjectTableId == _this.CurrentObjectField.LookUpTableId && (f.PMPropertyPath == "Email" || f.ListPropertyPath == " Email") && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                        }
                        else {
                            list = window.ObjectFields.filter(function (f) { return f.ObjectTableId == _this.CurrentObjectField.LookUpTableId && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                        }
                    }
                    else {
                        list = window.ObjectFields.filter(function (f) { return f.ObjectTableId == _this.CurrentObjectField.LookUpTableId && f.ListPropertyPath != null && !f.IsMulti && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                    }
                }
                if (_this.ResultFieldName.indexOf(".") > -1) {
                    list.forEach(function (objectField) {
                        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, _this.ResultFieldName + "." + objectField.FieldName, _this.ObjectFieldType);
                        views.push(documentObjectFieldsRowViewModel);
                    });
                }
                else {
                    list.forEach(function (objectField) {
                        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, _this.CurrentObjectField.FieldName + "." + objectField.FieldName, _this.ObjectFieldType);
                        views.push(documentObjectFieldsRowViewModel);
                    });
                }
                _this.Items = views;
                _this.IsViewTree = true;
            });
        }
        else if (this.CurrentObjectField.DataTypeCode == "DateTime") {
            list = new Array();
            var item = new ObjectFieldPM_1.ObjectFieldPM();
            item.FieldName = "Date";
            item.DataTypeCode = "None";
            list.push(item);
            item = new ObjectFieldPM_1.ObjectFieldPM();
            item.FieldName = "Time";
            item.DataTypeCode = "None";
            list.push(item);
            if (this.ResultFieldName.indexOf(".") > -1) {
                list.forEach(function (objectField) {
                    var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, _this.ResultFieldName + "." + objectField.FieldName, _this.ObjectFieldType);
                    views.push(documentObjectFieldsRowViewModel);
                });
            }
            else {
                list.forEach(function (objectField) {
                    var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, _this.CurrentObjectField.FieldName + "." + objectField.FieldName, _this.ObjectFieldType);
                    views.push(documentObjectFieldsRowViewModel);
                });
            }
            this.Items = views;
            this.IsViewTree = true;
        }
        //if (this.ResultFieldName.indexOf(".") > -1) {
        //    list.forEach((objectField) => {
        //        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.ResultFieldName + "." + objectField.FieldName, this.ObjectFieldType);
        //        views.push(documentObjectFieldsRowViewModel);
        //    });
        //}
        //else {
        //    list.forEach((objectField) => {
        //        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.CurrentObjectField.FieldName + "." + objectField.FieldName, this.ObjectFieldType);
        //        views.push(documentObjectFieldsRowViewModel);
        //    });
        //}
    };
    DocumentObjectFieldsRowViewModel.prototype.LoadItems = function () {
        if (this.IsViewTree) {
            this.IsViewTree = false;
        }
        else {
            if (!this.Items) {
                this.Load();
            }
            else
                this.IsViewTree = true;
        }
    };
    return DocumentObjectFieldsRowViewModel;
}());
exports.DocumentObjectFieldsRowViewModel = DocumentObjectFieldsRowViewModel;
var Guid = /** @class */ (function () {
    function Guid() {
    }
    Guid.newGuid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };
    return Guid;
}());
//# sourceMappingURL=DocumentObjectFieldsRowViewModel.js.map