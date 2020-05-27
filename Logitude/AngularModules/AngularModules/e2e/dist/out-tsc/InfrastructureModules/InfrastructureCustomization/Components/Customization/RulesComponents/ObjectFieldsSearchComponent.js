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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ServiceArgs_1 = require("../../../../../Infrastructure/DataContracts/ServiceArgs");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ServiceHelper_1 = require("../../../../../Infrastructure/Utilities/ServiceHelper");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ObjectFieldsSearchComponent = /** @class */ (function () {
    function ObjectFieldsSearchComponent(CD) {
        this.CD = CD;
        this.onSelectedDataLoadedEvent = new core_1.EventEmitter();
        this.onUnSelectedDataLoadedEvent = new core_1.EventEmitter();
        this.onDataSourceChangedEvent = new core_1.EventEmitter();
        this.onUnselectedDataSourceChangedEvent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.serviceArgs = new ServiceArgs_1.ServiceArgs();
        this.serviceArgs.http = ServiceHelper_1.ServiceHelper.Http;
        ;
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "ObjectFieldSearchFields_-1_-1";
        }
        else {
            this.SearchFieldsId = "ObjectFieldSearchFields_" + this.CurrentSession.GetNewId("ObjectFieldSearchFields");
        }
        //this.Run();
    }
    ObjectFieldsSearchComponent.prototype.SetWindowArgs = function (args) {
        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Name == args.currentObjectTable; })[0];
        this.Run();
    };
    ObjectFieldsSearchComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    };
    ObjectFieldsSearchComponent.prototype.FillPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "Search";
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    };
    ObjectFieldsSearchComponent.prototype.Run = function () {
        var _this = this;
        this.unselectedObjectFields = window.ObjectFields.filter(function (f) { return f.ObjectTableId == _this.ObjectTableId && !Tools_1.AppTool.IsNullOrEmpty(f.PMPropertyPath) && !f.DisplayOnly; });
        this.unselected = this.unselectedObjectFields;
        this.unSelectedList = this.unselected.sort(function (a, b) { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1; });
        this.CD.detectChanges();
        //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
        this.Fixedunselected = this.unSelectedList;
        this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
    };
    Object.defineProperty(ObjectFieldsSearchComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            this.searchText = newValue;
            if (newValue != null && newValue != "") {
                this.unSelectedList = this.Fixedunselected.filter(function (f) { return TextCodeTranslator_1.TextCodeTranslator.Translate(f.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
            }
            else {
                this.unSelectedList = this.Fixedunselected;
            }
            this.onUnselectedDataSourceChangedEvent.emit(this.unSelectedList);
            //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldsSearchComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            this.selectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldsSearchComponent.prototype, "FieldSelectedItem", {
        get: function () { return this.fieldSelectedItem; },
        set: function (newValue) {
            this.fieldSelectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    ObjectFieldsSearchComponent.prototype.onSelectedItemChanged = function (item) {
        this.SelectedItem = item;
        this.onUnSelectedDataLoadedEvent.emit(null);
    };
    ObjectFieldsSearchComponent.prototype.onFieldSelectedItemChanged = function (item) {
        this.FieldSelectedItem = item;
    };
    ObjectFieldsSearchComponent.prototype.SaveChanges = function () {
        this.CurrentSession.CurrentWindow.Close(this.FieldSelectedItem.Id);
    };
    ObjectFieldsSearchComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ObjectFieldsSearchComponent.prototype, "onSelectedDataLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ObjectFieldsSearchComponent.prototype, "onUnSelectedDataLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ObjectFieldsSearchComponent.prototype, "onDataSourceChangedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ObjectFieldsSearchComponent.prototype, "onUnselectedDataSourceChangedEvent", void 0);
    ObjectFieldsSearchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ObjectFieldsSearch',
            templateUrl: './ObjectFieldsSearchComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ObjectFieldsSearchComponent);
    return ObjectFieldsSearchComponent;
}());
exports.ObjectFieldsSearchComponent = ObjectFieldsSearchComponent;
//# sourceMappingURL=ObjectFieldsSearchComponent.js.map