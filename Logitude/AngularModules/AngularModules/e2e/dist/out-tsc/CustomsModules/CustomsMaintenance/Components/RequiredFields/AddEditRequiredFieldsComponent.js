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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CustomsRequiredFieldList_1 = require("../../../../Customs/EntityLists/CustomsRequiredFieldList");
var CustomsRequierdFieldsWebService_1 = require("../../../../Customs/Services/WebServices/CustomsRequierdFieldsWebService");
var AddEditRequiredFieldsComponent = /** @class */ (function (_super) {
    __extends(AddEditRequiredFieldsComponent, _super);
    function AddEditRequiredFieldsComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.SelectedObjectFields = [];
        _this.customsRequierdFieldsWebService = new CustomsRequierdFieldsWebService_1.CustomsRequierdFieldsWebService();
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //row selection on grid
        _this.SelectedRow = null;
        _this.FieldsList = new ObservableCollection_1.ObservableCollection([]);
        _this.OriginalFieldsList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    AddEditRequiredFieldsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.ObjectTableName = args.SelectedObjectTableName;
            //this.SelectedObjectFields = args.SelectedObjectFields;
            if (args.SelectedObjectFields) {
                args.SelectedObjectFields.forEach(function (el) {
                    _this.SelectedObjectFields.push(el);
                });
            }
            //this.CurrentSession.StartBusyIndicatorLoading();
            this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (res) {
                _this.GetObjectFields();
            });
        }
    };
    AddEditRequiredFieldsComponent.prototype.GetObjectFields = function () {
        var _this = this;
        var objectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        var objectFields = window.ObjectFields.filter(function (x) { return x.ObjectTableId == objectTable.Id && (!x.IsMulti && x.FieldName != "ImporterId" && x.FieldName != "TransferImporterId" && x.FieldName != "EntitleImporterId"); });
        this.FieldsList.Clear();
        var items = [];
        objectFields.forEach(function (objectField) {
            var requierdField = new CustomsRequiredFieldList_1.CustomsRequiredFieldList();
            requierdField.ObjectfieldId = objectField.Id;
            requierdField.ObjectFieldName = objectField.FieldName;
            requierdField.ObjectTableId = objectField.ObjectTableId;
            requierdField.Tenant = SessionLocator_1.SessionLocator.Tenant;
            var item = new RequiredFieldItemModel(objectField, requierdField);
            item.TranslatedName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);
            if (_this.SelectedObjectFields.find(function (d) { return d.ObjectfieldId == objectField.Id; })) {
                item.Active = true;
            }
            items.push(item);
        });
        this.FieldsList.InsertCollection(items);
        this.OriginalFieldsList.InsertCollection(items);
        //this.CurrentSession.StopBusyIndicator();
        //BuildSelectedList();
    };
    AddEditRequiredFieldsComponent.prototype.TextChanged = function (text) {
        if (text) {
            //console.log("Searching for " + text + " ...");
            var originalList = this.OriginalFieldsList.Collection;
            var filteredList = originalList.filter(function (d) { return d.TranslatedName.toLocaleLowerCase().includes(text.trim().toLocaleLowerCase()); });
            this.FieldsList.Clear();
            this.FieldsList.InsertCollection(filteredList);
        }
        else {
            this.FieldsList.InsertCollection(this.OriginalFieldsList.Collection);
        }
    };
    AddEditRequiredFieldsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditRequiredFieldsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var items = [];
        // 1- 
        this.OriginalFieldsList.Collection.forEach(function (field) {
            var i = new RequierdFieldObject();
            i.ObjectfieldId = field.ObjectfieldId;
            i.ObjectTableId = field.ObjectField.ObjectTableId;
            i.ObjectFieldName = field.ObjectField.FieldName;
            i.Active = field.Active;
            items.push(i);
        });
        this.customsRequierdFieldsWebService.PostRequiredFields(items).subscribe(function (response) {
            var res = response.Result;
            console.log("[Response] customsRequierdFieldsWebService.PostRequiredFields: ", res);
            _this.CurrentSession.StopBusyIndicator();
            _this.CurrentSession.CloseCurrentWindow();
        });
    };
    AddEditRequiredFieldsComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    AddEditRequiredFieldsComponent.prototype.CheckBoxChanged = function (event, itemModel) {
        console.log("<CheckBoxChanged> ", event, itemModel);
        if (itemModel) {
            if (event == true) {
                this.SelectedObjectFields.push(itemModel.RequierdField);
                this.FieldsList.Collection.find(function (d) { return d.Name == itemModel.ObjectField.FullNameTextCodeCode; }).Active = true;
                this.OriginalFieldsList.Collection.find(function (d) { return d.Name == itemModel.ObjectField.FullNameTextCodeCode; }).Active = true;
            }
            else {
                var index = this.SelectedObjectFields.findIndex(function (d) { return d.ObjectFieldName == itemModel.ObjectField.FieldName; });
                this.SelectedObjectFields.splice(index, 1);
                this.FieldsList.Collection.find(function (d) { return d.Name == itemModel.ObjectField.FullNameTextCodeCode; }).Active = false;
                this.OriginalFieldsList.Collection.find(function (d) { return d.Name == itemModel.ObjectField.FullNameTextCodeCode; }).Active = false;
            }
        }
    };
    AddEditRequiredFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddEditRequiredFieldsComponent',
            templateUrl: './AddEditRequiredFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AddEditRequiredFieldsComponent);
    return AddEditRequiredFieldsComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditRequiredFieldsComponent = AddEditRequiredFieldsComponent;
var RequiredFieldItemModel = /** @class */ (function (_super) {
    __extends(RequiredFieldItemModel, _super);
    function RequiredFieldItemModel(ObjectField, RequierdField) {
        var _this = _super.call(this) || this;
        _this.ObjectField = ObjectField;
        _this.RequierdField = RequierdField;
        _this.DataContext = _this;
        _this.active = false;
        _this.ObjectfieldId = ObjectField.Id;
        return _this;
    }
    Object.defineProperty(RequiredFieldItemModel.prototype, "ObjectfieldId", {
        get: function () { return this.objectfieldId; },
        set: function (value) {
            if (this.objectfieldId != value) {
                this.objectfieldId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredFieldItemModel.prototype, "Name", {
        get: function () { return this.ObjectField.FullNameTextCodeCode; },
        set: function (value) {
            if (this.ObjectField.FullNameTextCodeCode != value) {
                this.ObjectField.FullNameTextCodeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredFieldItemModel.prototype, "TranslatedName", {
        get: function () { return this.translatedName; },
        set: function (value) {
            if (this.translatedName != value) {
                this.translatedName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredFieldItemModel.prototype, "Active", {
        get: function () { return this.active; },
        set: function (value) {
            if (this.active != value) {
                this.active = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return RequiredFieldItemModel;
}(BaseComponent_1.BaseComponent));
exports.RequiredFieldItemModel = RequiredFieldItemModel;
var RequierdFieldObject = /** @class */ (function () {
    function RequierdFieldObject() {
    }
    return RequierdFieldObject;
}());
exports.RequierdFieldObject = RequierdFieldObject;
//# sourceMappingURL=AddEditRequiredFieldsComponent.js.map