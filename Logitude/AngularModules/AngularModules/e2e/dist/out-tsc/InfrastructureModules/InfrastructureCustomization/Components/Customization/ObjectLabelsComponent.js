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
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ObjectLabelsComponent = /** @class */ (function (_super) {
    __extends(ObjectLabelsComponent, _super);
    function ObjectLabelsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.TranslationList = [];
        _this.ListBoxItemSource = [];
        _this.CountText = 0;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SelectedRow = null;
        _this.myService = new EntityResourceService_1.EntityResourceService();
        _this.TabsList = new ObservableCollection_1.ObservableCollection([]);
        _this.DirtyItems = [];
        _this.EntityPM = new GeneralDomainService_1.FieldsTranslations();
        return _this;
    }
    Object.defineProperty(ObjectLabelsComponent.prototype, "Singular", {
        get: function () { return this.EntityPM.TranslatedText; },
        set: function (value) { if (this.EntityPM.TranslatedText != value)
            this.EntityPM.TranslatedText = value; },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(ObjectLabelsComponent.prototype, "Plural", {
        get: function () { return this.EntityPM.TranslatedTextPlural; },
        set: function (value) { if (this.EntityPM.TranslatedTextPlural != value)
            this.EntityPM.TranslatedTextPlural = value; },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(ObjectLabelsComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
                this.FillList();
            }
        },
        enumerable: true,
        configurable: true
    });
    ObjectLabelsComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    ObjectLabelsComponent.prototype.SelectionChanged = function (Item) {
        this.SelectedItem = Item;
        this.ListBoxSelectionMethod(Item);
    };
    ObjectLabelsComponent.prototype.SetWindowArgs = function (windowArgs) {
        var _this = this;
        this.ObjecttableId = windowArgs.ObjectTableID;
        this.DefaultText = windowArgs.DefaultText;
        this.Singular = windowArgs.TranslatedText;
        this.Plural = windowArgs.TranslatedTextPlural;
        this.ObjectTableName = windowArgs.ObjectTableName;
        this.UIProperties.SetEnabled("DefaultText", this.ObjectTableName, false);
        this.ObjectTablePM = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjecttableId; })[0];
        this.EntityPM = windowArgs;
        this.FillListBox();
    };
    ObjectLabelsComponent.prototype.FillListBox = function () {
        var _this = this;
        this.ListBoxItemSource = [];
        var list = window.TextCodes.filter(function (d) { return d.ObjectTableId == _this.ObjecttableId; });
        this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("All", "All"));
        if (list != null) {
            if (list.filter(function (d) { return d.TextCodeTypeCode == "TH"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("TH", "Tab Headers"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "B"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("B", "Buttons And Actions"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "H"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("H", "Help Text"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "M"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("M", "Messages"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "MH"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("MH", "Menu Headers"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "MC"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("MC", "Maintenance"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "Q"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("Q", "Queries"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "S"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("S", "Screens"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "L"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("L", "Links"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "O"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("O", "Others"));
            }
            ;
            if (list.filter(function (d) { return d.TextCodeTypeCode == "G"; })[0]) {
                this.ListBoxItemSource.push(new CodeNameClass_1.CodeNameClass("G", "General"));
            }
            ;
        }
        this.SelectionChanged(this.ListBoxItemSource[0]);
    };
    ObjectLabelsComponent.prototype.ListBoxSelectionMethod = function (Item) {
        var _this = this;
        this.TabsList.Clear();
        this.list = [];
        this.CountText = 0;
        var myService = new GeneralDomainService_1.GeneralDomainService();
        this.CurrentSession.StartBusyIndicatorLoading();
        myService.GetTranslationsByParam(Item.Code, this.ObjecttableId, InfraSettings_1.InfraSettings.TenantPM.Language).subscribe(function (myResult) {
            if (myResult) {
                _this.list = myResult.Result;
                _this.FillList();
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ObjectLabelsComponent.prototype.FillList = function () {
        var _this = this;
        var temp = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            this.list.forEach(function (item) {
                temp.push(new FieldsTranslationsItem(item, _this));
            });
        }
        else {
            this.list.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.DefaultText) && d.DefaultText.toUpperCase().startsWith(_this.SearchText.toUpperCase())
                || !Tools_1.AppTool.IsNullOrEmpty(d.TranslatedText) && d.TranslatedText.toUpperCase().startsWith(_this.SearchText.toUpperCase())
                || !Tools_1.AppTool.IsNullOrEmpty(d.Code) && d.Code.toUpperCase().startsWith(_this.SearchText.toUpperCase()); })
                .forEach(function (item) {
                temp.push(new FieldsTranslationsItem(item, _this));
            });
        }
        this.TabsList.InsertCollection(temp);
        this.CountText = this.TabsList.Length;
    };
    ObjectLabelsComponent.prototype.CancelClicked = function () { this.CurrentSession.CloseCurrentWindow(); };
    ObjectLabelsComponent.prototype.SaveClicked = function () {
        var _this = this;
        if (this.DirtyItems.length > 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myServiceHelper = new GeneralDomainService_1.FieldsUpdateHelper();
            myServiceHelper.Tenant = SessionLocator_1.SessionLocator.Tenant;
            myServiceHelper.Items = this.DirtyItems;
            var generalService = new GeneralDomainService_1.GeneralDomainService();
            generalService.UpdateFieldsTranslations(myServiceHelper).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    ObjectLabelsComponent.prototype.OkClicked = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.Singular)) {
            this.ValidationErrorsList.push("Singular is Required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Plural)) {
            this.ValidationErrorsList.push("Plural is Required");
        }
        if (this.DirtyItems.indexOf(this.EntityPM) == -1)
            this.DirtyItems.push(this.EntityPM);
        if (this.ValidationErrorsList.length == 0)
            this.SaveClicked();
    };
    ObjectLabelsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ObjectLabelsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ObjectLabelsComponent);
    return ObjectLabelsComponent;
}(BaseComponent_1.BaseComponent));
exports.ObjectLabelsComponent = ObjectLabelsComponent;
var FieldsTranslationsItem = /** @class */ (function (_super) {
    __extends(FieldsTranslationsItem, _super);
    function FieldsTranslationsItem(entity, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Entity = entity;
        return _this;
    }
    Object.defineProperty(FieldsTranslationsItem.prototype, "Code", {
        get: function () { return this.Entity.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldsTranslationsItem.prototype, "DefaultText", {
        get: function () { return this.Entity.DefaultText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldsTranslationsItem.prototype, "TranslatedText", {
        get: function () { return this.Entity.TranslatedText; },
        set: function (newValue) {
            if (this.Entity.TranslatedText != newValue) {
                this.Entity.TranslatedText = newValue;
                if (this.fatherComponent.DirtyItems.indexOf(this.Entity) == -1) {
                    this.fatherComponent.DirtyItems.push(this.Entity);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldsTranslationsItem.prototype, "TranslatedTextPlural", {
        get: function () { return this.Entity.TranslatedTextPlural; },
        set: function (newValue) {
            if (this.Entity.TranslatedTextPlural != newValue) {
                this.Entity.TranslatedTextPlural = newValue;
                if (this.fatherComponent.DirtyItems.indexOf(this.Entity) == -1) {
                    this.fatherComponent.DirtyItems.push(this.Entity);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return FieldsTranslationsItem;
}(BaseComponent_1.BaseComponent));
exports.FieldsTranslationsItem = FieldsTranslationsItem;
//# sourceMappingURL=ObjectLabelsComponent.js.map