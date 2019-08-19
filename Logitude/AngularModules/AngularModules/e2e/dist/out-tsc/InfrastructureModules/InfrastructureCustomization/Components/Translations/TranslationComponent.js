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
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var TranslationComponent = /** @class */ (function (_super) {
    __extends(TranslationComponent, _super);
    function TranslationComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SearchText = null;
        _this.isLoading = false;
        _this.SelectedRow = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.myService = new GeneralDomainService_1.GeneralDomainService();
        _this.DirtyItems = [];
        _this.LoadTextCodeTypes();
        return _this;
    }
    TranslationComponent.prototype.LoadTextCodeTypes = function () {
        var _this = this;
        this.myService.GetTextCodeTypes().subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.BuildComponentComboList(myResponse.Result);
            }
        });
    };
    TranslationComponent.prototype.BuildComponentComboList = function (list) {
        var _this = this;
        this.ComponentComboList = [];
        list.forEach(function (item) {
            _this.ComponentComboList.push(item);
        });
    };
    Object.defineProperty(TranslationComponent.prototype, "SelectedComponentFilter", {
        get: function () { return this.selectedComponentFilter; },
        set: function (value) {
            if (this.selectedComponentFilter != value) {
                this.selectedComponentFilter = value;
                this.LoadTranslations();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationComponent.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (value) {
            if (this.objectTableId != value) {
                this.objectTableId = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.LoadTranslations();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TranslationComponent.prototype.OnSearchTextChanged = function (text) {
        this.SearchText = text;
        this.BuildItemsSource();
    };
    TranslationComponent.prototype.LoadTranslations = function () {
        var _this = this;
        var code = null;
        if (this.SelectedComponentFilter != null) {
            code = this.SelectedComponentFilter.Code;
        }
        if (!this.isLoading) {
            this.isLoading = true;
            this.CurrentSession.StartBusyIndicatorLoading();
            this.myService.LoadAllFieldsTranslations(SessionLocator_1.SessionLocator.TenantPM.Language, this.ObjectTableId, code).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.loadedTranslations = myResponse.Result;
                    //this.isLoading = false;
                    if (_this.loadedTranslations != null) {
                        _this.BuildItemsSource();
                    }
                }
            });
        }
    };
    TranslationComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource.Clear();
        var list = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            this.loadedTranslations.forEach(function (item) {
                list.push(new TranslationItem(item, _this));
            });
        }
        else {
            this.loadedTranslations.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.DefaultText) && d.DefaultText.toUpperCase().startsWith(_this.SearchText.toUpperCase())
                || !Tools_1.AppTool.IsNullOrEmpty(d.TranslatedText) && d.TranslatedText.toUpperCase().startsWith(_this.SearchText.toUpperCase())
                || !Tools_1.AppTool.IsNullOrEmpty(d.TranslatedTextPlural) && d.TranslatedTextPlural.toUpperCase().startsWith(_this.SearchText.toUpperCase()); })
                .forEach(function (item) {
                list.push(new TranslationItem(item, _this));
            });
        }
        this.isLoading = false;
        this.ItemsSource.InsertCollection(list);
        this.Count = this.ItemsSource.Length;
        this.CurrentSession.StopBusyIndicator();
    };
    TranslationComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    TranslationComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TranslationComponent.prototype.SaveClicked = function () {
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
                    CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                }
            });
        }
    };
    TranslationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TranslationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TranslationComponent);
    return TranslationComponent;
}(BaseComponent_1.BaseComponent));
exports.TranslationComponent = TranslationComponent;
var TranslationItem = /** @class */ (function (_super) {
    __extends(TranslationItem, _super);
    function TranslationItem(entity, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "FieldsTranslation";
        _this.DataContext = _this;
        _this.Entity = entity;
        return _this;
    }
    Object.defineProperty(TranslationItem.prototype, "Code", {
        get: function () { return this.Entity.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationItem.prototype, "DefaultText", {
        get: function () { return this.Entity.DefaultText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationItem.prototype, "TranslatedText", {
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
    Object.defineProperty(TranslationItem.prototype, "TranslatedTextPlural", {
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
    return TranslationItem;
}(BaseComponent_1.BaseComponent));
exports.TranslationItem = TranslationItem;
//# sourceMappingURL=TranslationComponent.js.map