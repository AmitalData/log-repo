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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CardExternalAccountsByProductPM_1 = require("../../../EntityPMs/CardExternalAccountsByProductPM");
var PartnersDomainService_1 = require("../../../Services/PartnersDomainService");
var ProductTypeListService_1 = require("../../../Services/StandardLists/ProductTypeListService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var ExternalAccountsByProductsComponent = /** @class */ (function (_super) {
    __extends(ExternalAccountsByProductsComponent, _super);
    function ExternalAccountsByProductsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.SelectedRow = null;
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllProductTypes = [];
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.myDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.myProductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        return _this;
    }
    ExternalAccountsByProductsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.Clone();
        this.entityResourceService.getEntityResourceByTableName("CardExternalAccountsByProduct").subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.LoadData();
        });
    };
    ExternalAccountsByProductsComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myProductTypeListService.getAllFromCache().subscribe(function (myResponse1) {
            if (!myResponse1.HasError) {
                _this.AllProductTypes = myResponse1.Result;
                _this.AllProductTypes = _this.AllProductTypes.filter(function (f) { return f.Code != "CI" && f.Code != "DL" && f.Code != "IN"; });
            }
            _this.myDomainService.GetCardExternalAccountsByProducts(_this.EntityPM.Id).subscribe(function (myResponse2) {
                if (!myResponse2.HasError) {
                    var myResult = myResponse2.Result;
                    _this.BuildItemsSource(myResult);
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        });
    };
    ExternalAccountsByProductsComponent.prototype.BuildItemsSource = function (loadedList) {
        var _this = this;
        var itemsCollection = [];
        if (loadedList == null) {
            loadedList = [];
        }
        if (loadedList.length > 0) {
            loadedList = loadedList.filter(function (f) { return f.ProductTypeCode != "CI" && f.ProductTypeCode != "DL" && f.ProductTypeCode != "IN"; });
            loadedList.forEach(function (itemPM) {
                itemsCollection.push(new ExternalAccountsByProductsItem(itemPM, false, _this));
                _this.ItemsSource.InsertCollection(itemsCollection);
            });
        }
        else {
            this.AllProductTypes.forEach(function (item) {
                var itemPM = new CardExternalAccountsByProductPM_1.CardExternalAccountsByProductPM();
                itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                itemPM.CardId = _this.EntityPM.Id;
                itemPM.ProductTypeCode = item.Code;
                itemPM.ProductTypeName = item.Name;
                itemPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                itemPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                itemPM.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                itemsCollection.push(new ExternalAccountsByProductsItem(itemPM, true, _this));
                _this.ItemsSource.InsertCollection(itemsCollection);
            });
        }
    };
    Object.defineProperty(ExternalAccountsByProductsComponent.prototype, "ExternalAccountingBusinessArea", {
        get: function () { return this.EntityPM.ExternalAccountingBusinessArea; },
        set: function (value) {
            if (this.EntityPM.ExternalAccountingBusinessArea != value) {
                this.EntityPM.ExternalAccountingBusinessArea = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsComponent.prototype, "ExternalId2", {
        get: function () { return this.EntityPM.ExternalId2; },
        set: function (value) {
            if (this.EntityPM.ExternalId2 != value) {
                this.EntityPM.ExternalId2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ExternalAccountsByProductsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    ExternalAccountsByProductsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            this.CurrentSession.StartBusyIndicatorSaving();
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            this.ItemsSource.Collection.forEach(function (item) {
                Validator_1.Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);
            });
            this.ValidationErrorsList = errors;
            if (errors.length == 0) {
                var args = new PartnersDomainService_1.PartnerExternalAccountsServicePM();
                args.Tenant = SessionLocator_1.SessionLocator.Tenant;
                args.CardId = this.EntityPM.Id;
                args.BusinessArea = this.ExternalAccountingBusinessArea;
                args.ExternalId2 = this.ExternalId2;
                args.ObjectTableName = this.ObjectTableName;
                this.ItemsSource.Collection.forEach(function (item) {
                    args.Items.push(item.EntityPM);
                });
                this.myDomainService.Put(args).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.EntityPM.IsDirty = false;
                        _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }
                });
            }
            else {
                this.CurrentSession.StopBusyIndicator();
            }
        }
    };
    ExternalAccountsByProductsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ExternalAccountingBusinessArea');
        this.myCloner.AddEntity(this.EntityPM);
    };
    ExternalAccountsByProductsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    ExternalAccountsByProductsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ExternalAccountsByProductsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ExternalAccountsByProductsComponent);
    return ExternalAccountsByProductsComponent;
}(BaseComponent_1.BaseComponent));
exports.ExternalAccountsByProductsComponent = ExternalAccountsByProductsComponent;
var ExternalAccountsByProductsItem = /** @class */ (function (_super) {
    __extends(ExternalAccountsByProductsItem, _super);
    function ExternalAccountsByProductsItem(entityPM, isNewEntity, father) {
        var _this = _super.call(this) || this;
        _this.isNewEntity = isNewEntity;
        _this.father = father;
        _this.ObjectTableName = "CardExternalAccountsByProduct";
        _this.DataContext = _this;
        _this.EntityPM = entityPM;
        return _this;
    }
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "Id", {
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "ProductTypeCode", {
        get: function () { return this.EntityPM.ProductTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "ProductTypeName", {
        get: function () { return this.EntityPM.ProductTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "GLAccount", {
        get: function () { return this.EntityPM.GLAccount; },
        set: function (value) {
            if (this.EntityPM.GLAccount != value) {
                this.EntityPM.GLAccount = value;
                this.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                this.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
                this.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                this.father.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "CostCenter", {
        get: function () { return this.EntityPM.CostCenter; },
        set: function (value) {
            if (this.EntityPM.CostCenter != value) {
                this.EntityPM.CostCenter = value;
                this.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                this.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
                this.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                this.father.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        set: function (value) {
            if (this.EntityPM.UpdateDate != value) {
                this.EntityPM.UpdateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "UpdatedByUserId", {
        get: function () { return this.EntityPM.UpdatedByUserId; },
        set: function (value) {
            if (this.EntityPM.UpdatedByUserId != value) {
                this.EntityPM.UpdatedByUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalAccountsByProductsItem.prototype, "UpdatedByUserName", {
        get: function () { return this.EntityPM.UpdatedByUserName; },
        set: function (value) {
            if (this.EntityPM.UpdatedByUserName != value) {
                this.EntityPM.UpdatedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return ExternalAccountsByProductsItem;
}(BaseComponent_1.BaseComponent));
exports.ExternalAccountsByProductsItem = ExternalAccountsByProductsItem;
//# sourceMappingURL=ExternalAccountsByProductsComponent.js.map