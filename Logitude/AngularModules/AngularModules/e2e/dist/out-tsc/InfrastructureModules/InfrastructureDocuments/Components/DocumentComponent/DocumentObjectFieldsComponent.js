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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DocumentObjectFieldsRowViewModel_1 = require("./ViewModel/DocumentObjectFieldsRowViewModel");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var forms_1 = require("@angular/forms");
var DocumentObjectFieldsComponent = /** @class */ (function () {
    function DocumentObjectFieldsComponent() {
        this.ObjectTypeField = "";
        this.IsShowTabObjectField = false;
        this.HideSystemDataTab = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.displayListOnly = false;
        this.IsSearchIconVisible = true;
    }
    DocumentObjectFieldsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.SearchTextValue = new forms_1.FormControl();
        this.SearchTextValue.valueChanges
            .debounceTime(500)
            .distinctUntilChanged()
            .subscribe(function (search) {
            _this.Search(search);
        });
    };
    DocumentObjectFieldsComponent.prototype.SetWindowArgs = function (args) {
        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTypeField = args.ObjectTypeField;
        this.InSertDataFieldType = args.InSertDataFieldType;
        this.HideSystemDataTab = args.HideSystemDataTab;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ObjectTypeField)) {
            if (this.InSertDataFieldType == "From" || this.InSertDataFieldType == "ReplyTo" || this.InSertDataFieldType == "CC")
                this.ObjectTypeField = "Emails";
        }
        this.Run();
    };
    DocumentObjectFieldsComponent.prototype.Run = function () {
        var _this = this;
        this.ObjectTablesList = window.ObjectTables;
        this.ObsList = new Array();
        this.ObsListAll = new Array();
        this.SystemObsList = new Array();
        this.SystemDataSource = new Array();
        this.AllSystemDataSourceViewsLists = new Array();
        this.AllObjectDataSourceViewsLists = new Array();
        if (!this.ObjectTypeField) {
            this.ObjectTypeField = null;
        }
        // Abed
        this.SelectedObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.ObjectTableSelectionChangedMethod(this.SelectedObjectTable);
    };
    DocumentObjectFieldsComponent.prototype.ObjectTableSelectionChangedMethod = function (objectTable) {
        var _this = this;
        this.objectFieldsList = new Array();
        //Tab Entity
        if (objectTable != null) {
            this.IsShowTabObjectField = true;
            this.SelectedTabCode = "DAF";
            this.ObsList = new Array();
            if (this.ObjectTypeField) {
                if (this.ObjectTypeField == "Emails") {
                    this.objectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && (f.DataTypeCode == _this.ObjectTypeField || f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact") && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                }
                else if (this.ObjectTypeField == "LookUpContactOrUser") {
                    this.objectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == objectTable.Id && (f.PMPropertyPath || f.ListPropertyPath) && (f.FieldName == "TicketReplyto" || f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact") && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                }
                else if (this.ObjectTypeField == "DocuemntFileName") {
                    this.objectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.DisplayInDocumentReferences; });
                }
            }
            else {
                if (objectTable.Name == "LogitudeMessagesTransmissionLog") {
                    this.objectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && f.DataTypeCode != "Emails" && f.DisplayInEntityVariables == true; });
                }
                else {
                    this.objectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == objectTable.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && f.DataTypeCode != "Emails" && !f.DisplayOnly && f.DisplayInEntityVariables == true; });
                }
            }
            this.objectFieldsList.forEach(function (field) {
                if (field.FieldName == "OBLTypeCode") {
                    var d = "f";
                }
                var view = new DocumentObjectFieldsRowViewModel_1.DocumentObjectFieldsRowViewModel(field, field.FieldName, _this.ObjectTypeField);
                _this.ObsList.push(view);
                _this.ObsListAll.push(view);
            });
            this.DataSource = this.ObsList;
        }
        else
            this.SelectedTabCode = "SAF";
        if (this.objectFieldsList.length > 0)
            this.IsTextSearchEnabled = true;
        else
            this.IsTextSearchEnabled = false;
        //Tab SystemData
        if (!this.HideSystemDataTab) {
            var systemDataObjectFieldsList = new Array();
            var table = window.ObjectTables.filter(function (d) { return d.Name == "SystemData"; })[0];
            if (this.ObjectTypeField) {
                if (this.ObjectTypeField == "Emails") {
                    systemDataObjectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == table.Id && (f.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || f.Tenant == 0) && (f.DataTypeCode == _this.ObjectTypeField || f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact" || f.FieldName == "Supportemail"); });
                }
                if (this.ObjectTypeField == "LookUpContactOrUser") {
                    systemDataObjectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == table.Id && (f.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || f.Tenant == 0) && (f.ObjectTable_LookUpTableName == "User" || f.ObjectTable_LookUpTableName == "Contact" || f.FieldName == "Supportemail"); });
                }
            }
            else {
                systemDataObjectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == table.Id && (f.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || f.Tenant == 0) && f.DataTypeCode != "Emails"; });
            }
            var order = 0;
            systemDataObjectFieldsList.forEach(function (field) {
                var view = new DocumentObjectFieldsRowViewModel_1.DocumentObjectFieldsRowViewModel(field, field.FieldName, _this.ObjectTypeField);
                view.Order = order;
                order += 1;
                _this.SystemObsList.push(view);
            });
            var smailLogo = this.SystemObsList.filter(function (d) { return d.FieldName == "SmallLogo"; })[0];
            if (smailLogo) {
                var wideLogo = this.SystemObsList.filter(function (d) { return d.FieldName == "WideLogo"; })[0];
                if (wideLogo) {
                    wideLogo.Order = (smailLogo.Order + 1);
                    order = (wideLogo.Order + 1);
                    this.SystemObsList.filter(function (d) { return d.Order > smailLogo.Order && d.FieldName != "WideLogo"; }).forEach(function (field) {
                        field.Order = order;
                        order += 1;
                    });
                }
            }
            this.SystemDataSource = this.SystemObsList.sort(function (a, b) { return a.Order - b.Order; });
        }
        //  Search
    };
    DocumentObjectFieldsComponent.prototype.SystemDataSourceChangeSelected = function (selectedItem) {
        this.SelectSystemDataObjectFieldsRowViewModel = selectedItem;
        this.SelectObjectDataFieldsRowViewModel = null;
        var item = this.AllSystemDataSourceViewsLists.filter(function (d) { return d.Id == selectedItem.Id; })[0];
        if (!item) {
            this.AllSystemDataSourceViewsLists.push(selectedItem);
        }
        this.AllSystemDataSourceViewsLists.forEach(function (field) {
            field.DivSelectBackgroud = "#ffffff";
        });
        selectedItem.DivSelectBackgroud = "#B6E0F5";
    };
    DocumentObjectFieldsComponent.prototype.ObjectDataSourceChangeSelected = function (selectedItem) {
        this.SelectSystemDataObjectFieldsRowViewModel = null;
        this.SelectObjectDataFieldsRowViewModel = selectedItem;
        var item = this.AllSystemDataSourceViewsLists.filter(function (d) { return d.Id == selectedItem.Id; })[0];
        if (!item) {
            this.AllSystemDataSourceViewsLists.push(selectedItem);
        }
        this.AllSystemDataSourceViewsLists.forEach(function (field) {
            field.DivSelectBackgroud = "#ffffff";
        });
        selectedItem.DivSelectBackgroud = "#B6E0F5";
    };
    DocumentObjectFieldsComponent.prototype.Search = function (textsearch) {
        this.SearchText = textsearch;
        this.ObsList = new Array();
        var views = new Array();
        if (textsearch) {
            this.DataSource = this.ObsList = this.ObsListAll.filter(function (d) { return TextCodeTranslator_1.TextCodeTranslator.Translate(d.FullNameTextCodeCode).toLowerCase().indexOf(textsearch.toLowerCase()) > -1; });
        }
        else {
            this.DataSource = this.ObsListAll;
        }
    };
    DocumentObjectFieldsComponent.prototype.DisplayListFieldsOnly = function () {
        var _this = this;
        this.displayListOnly = true;
        this.ObsList = new Array();
        this.ObsListAll = new Array();
        if (this.SelectedObjectTable) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.SelectedObjectTable.Name; })[0];
            this.objectFieldsList = window.ObjectFields.filter(function (f) { return f.ObjectTableId == table.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && f.IsMulti == false && f.DisplayOnly == false && f.DisplayInEntityVariables == true; });
            var views = new Array();
            this.objectFieldsList.forEach(function (field) {
                var view = new DocumentObjectFieldsRowViewModel_1.DocumentObjectFieldsRowViewModel(field, field.FieldName, _this.ObjectTypeField);
                view.DisplayListOnly = true;
                views.push(view);
                _this.ObsList.push(view);
                _this.ObsListAll.push(view);
            });
            this.DataSource = this.ObsList;
            if (this.objectFieldsList.length > 0) {
                this.IsTextSearchEnabled = true;
                return;
            }
        }
    };
    DocumentObjectFieldsComponent.prototype.OnFucos = function () {
        this.IsSearchIconVisible = false;
    };
    DocumentObjectFieldsComponent.prototype.OnLostFucos = function () {
        if (this.SearchText) {
            this.IsSearchIconVisible = false;
        }
        else
            this.IsSearchIconVisible = true;
    };
    DocumentObjectFieldsComponent.prototype.SaveButtonClicked = function () {
        this.TextSelected = "";
        if (this.SelectObjectDataFieldsRowViewModel) {
            var selectedField = this.SelectObjectDataFieldsRowViewModel;
            this.TextSelected = "[" + selectedField.ResultFieldName + "]";
            if (this.InSertDataFieldType == "FroalaEditor")
                this.TextSelected = "<span>" + this.TextSelected + "</span>";
        }
        else if (this.SelectSystemDataObjectFieldsRowViewModel) {
            var selectedField = this.SelectSystemDataObjectFieldsRowViewModel;
            if (this.InSertDataFieldType != "FroalaEditor") {
                if (selectedField.FieldName && (selectedField.FieldName.toLowerCase() == "logo" || selectedField.FieldName.toLowerCase() == "smalllogo" || selectedField.FieldName.toLowerCase() == "signature")) {
                    return;
                }
                this.TextSelected = "[SystemData." + selectedField.ResultFieldName + "]";
            }
            else {
                this.TextSelected = "[SystemData." + selectedField.ResultFieldName + "]";
                this.TextSelected = "<span>" + this.TextSelected + "</span>";
            }
        }
        this.CurrentSession.CurrentWindow.Close(this.TextSelected);
    };
    DocumentObjectFieldsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("");
    };
    DocumentObjectFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentObjectFields',
            templateUrl: './DocumentObjectFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DocumentObjectFieldsComponent);
    return DocumentObjectFieldsComponent;
}());
exports.DocumentObjectFieldsComponent = DocumentObjectFieldsComponent;
//# sourceMappingURL=DocumentObjectFieldsComponent.js.map