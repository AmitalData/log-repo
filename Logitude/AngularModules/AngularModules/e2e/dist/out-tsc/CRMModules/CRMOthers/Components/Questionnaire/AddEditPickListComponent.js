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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CustomPickListPM_1 = require("../../../../Infrastructure/EntityPMs/CustomPickListPM");
var CustomPickListPMExtendedService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/CustomPickListPMExtendedService");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var AddEditPickListComponent = /** @class */ (function (_super) {
    __extends(AddEditPickListComponent, _super);
    function AddEditPickListComponent(_customPickListPMExtendedService) {
        var _this = _super.call(this) || this;
        _this._customPickListPMExtendedService = _customPickListPMExtendedService;
        _this.CustomPickLists = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CustomPickListPMLists = [];
        _this.RemoveCustomPickListPMLists = [];
        _this.PickListCode = "";
        _this.Counter = 0;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    AddEditPickListComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.IsMultipleChoice = args.IsMultipleChoice;
            this.IsNewMode = args.IsNewMode;
            this.PickListCode = !Tools_1.AppTool.IsNullOrEmpty(args.PickListCode) ? args.PickListCode : "";
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.PickListCode)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.LoadData();
        }
    };
    AddEditPickListComponent.prototype.LoadData = function () {
        var _this = this;
        this.CustomPickListPMLists = [];
        this._customPickListPMExtendedService.GetCustomPickListsByCode(this.PickListCode, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.CustomPickListPMLists = response.Result;
            _this.BuildItemsSource();
        });
    };
    AddEditPickListComponent.prototype.BuildItemsSource = function () {
        this.ItemsSource.Collection = [];
        if (this.CustomPickListPMLists) {
            var itemsCollection = [];
            this.CustomPickListPMLists.forEach(function (item) {
                itemsCollection.push(new CustomPickListData(item));
            });
            this.ItemsSource.AppendCollection(itemsCollection);
        }
    };
    Object.defineProperty(AddEditPickListComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditPickListComponent.prototype.rowChanged = function (event) {
        this.SelectedItem = event;
    };
    AddEditPickListComponent.prototype.AddPickListClicked = function () {
        this.Counter += 1;
        var pickList = new CustomPickListPM_1.CustomPickListPM();
        pickList.Code = this.PickListCode;
        pickList.IsMultipleChoice = this.IsMultipleChoice;
        pickList.Tenant = SessionLocator_1.SessionLocator.Tenant;
        pickList.Id = (this.Counter + "New").toString();
        this.CustomPickListPMLists.push(pickList);
        this.BuildItemsSource();
    };
    AddEditPickListComponent.prototype.RemovePickListClicked = function (item) {
        if (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.Id)) {
                if (!this.RemoveCustomPickListPMLists)
                    this.RemoveCustomPickListPMLists = [];
                this.RemoveCustomPickListPMLists.push(item.EntityPM);
            }
            this.CustomPickListPMLists = this.CustomPickListPMLists.filter(function (d) { return d != item.EntityPM; });
            this.BuildItemsSource();
        }
    };
    AddEditPickListComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var customPickLists = this.ItemsSource.Collection;
        if (customPickLists && customPickLists.length > 0) {
            customPickLists.forEach(function (item) {
                if (item.EntityPM) {
                    if (customPickLists.filter(function (p) { return p.EntityPM.Value == item.Value && p.EntityPM.Id != item.EntityPM.Id; })[0]) {
                        _this.ValidationErrorsList.push("Some values are duplicated!");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.Value)) {
                        _this.ValidationErrorsList.push("PickList value is required");
                    }
                    else if (item.EntityPM.Value.length > 1000) {
                        _this.ValidationErrorsList.push("PickList value length should be less than 1000 character");
                    }
                }
            });
            if (this.ValidationErrorsList.length == 0) {
                var customPickListLists = this.ItemsSource.Collection.filter(function (d) { return d.EntityPM.IsDirty == true; });
                if ((customPickListLists && customPickListLists.length > 0) || (this.RemoveCustomPickListPMLists && this.RemoveCustomPickListPMLists.length > 0)) {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    var customPickListPMLists = [];
                    customPickListLists.forEach(function (item) {
                        if (item.EntityPM) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.Id)) {
                                if (item.EntityPM.Id.includes("New"))
                                    item.EntityPM.Id = null;
                            }
                            item.EntityPM.Code = _this.PickListCode;
                            item.EntityPM.IsDirty = false;
                            customPickListPMLists.push(item.EntityPM);
                        }
                    });
                    if (this.RemoveCustomPickListPMLists && this.RemoveCustomPickListPMLists.length > 0) {
                        this.RemoveCustomPickListPMLists.forEach(function (item) {
                            item.IsDirty = true;
                            customPickListPMLists.push(item);
                        });
                    }
                    this._customPickListPMExtendedService.InsertupdateCustomPickLists(customPickListPMLists).subscribe(function (res) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CurrentWindow.Close(_this.PickListCode);
                        CachedDataManager_1.CachedDataManager.RefreshTableData("CustomPickList", true);
                    });
                }
                else
                    this.CloseButtonClicked();
            }
        }
    };
    AddEditPickListComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPickListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPickListComponent.html',
            providers: [CustomPickListPMExtendedService_1.CustomPickListPMExtendedService],
        }),
        __metadata("design:paramtypes", [CustomPickListPMExtendedService_1.CustomPickListPMExtendedService])
    ], AddEditPickListComponent);
    return AddEditPickListComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPickListComponent = AddEditPickListComponent;
var CustomPickListData = /** @class */ (function (_super) {
    __extends(CustomPickListData, _super);
    function CustomPickListData(entity) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.EntityPM = entity;
        return _this;
    }
    Object.defineProperty(CustomPickListData.prototype, "Value", {
        get: function () {
            var value = "";
            if (this.EntityPM)
                value = this.EntityPM.Value;
            return value;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                this.EntityPM.Value = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return CustomPickListData;
}(BaseComponent_1.BaseComponent));
exports.CustomPickListData = CustomPickListData;
//# sourceMappingURL=AddEditPickListComponent.js.map