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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var GroupByPipe_1 = require("../../../../../Infrastructure/Pipes/GroupByPipe");
var CustomerSalesmanByProductPM_1 = require("../../../../../Common/EntityPMs/CustomerSalesmanByProductPM");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var CustomerSalesmanByProductSplitComponent = /** @class */ (function (_super) {
    __extends(CustomerSalesmanByProductSplitComponent, _super);
    function CustomerSalesmanByProductSplitComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customer";
        _this.ProductTypes = [];
        _this.IsUnifreightEditable = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.oldItems = [];
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CustomerSalesmanByProductSplitComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.ProductTypes = args['ProductTypes'];
        this.Clone();
        if (args.IsUnifreightEditable) {
            this.IsUnifreightEditable = args.IsUnifreightEditable;
            //if (this.IsUnifreightEditable === true) {
            // this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
            //this.EntityPM.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
            // }
        }
        this.ProductTypes.forEach(function (item) {
            var itemPM = _this.EntityPM.CustomerSalesmanByProducts.filter(function (d) { return d.ProductTypeCode == item.Code; })[0];
            var itemClass = new CustomerSalesmanByProductSplitLineViewModel(_this.EntityPM, item, itemPM);
            // itemClass.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
            //itemPM.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, !this.IsUnifreightEditable);
            _this.ItemsSource.Insert(itemClass);
        });
    };
    CustomerSalesmanByProductSplitComponent.prototype.CencelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomerSalesmanByProductSplitComponent.prototype.OkButtonClicked = function () {
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var myBaseData = this.EntityPM.CustomerSalesmanByProducts.filter(function (f) { return f.SalesmanUserId != null; });
        var Forwarders = myPipe.transform(myBaseData, "SalesmanUserId");
        if (Forwarders.length == 1) {
            this.EntityPM.SalesmanUserId = myBaseData[0].SalesmanUserId;
            this.EntityPM.SalesmanUserEnglishName = myBaseData[0].SalesmanUserName;
        }
        else {
            this.EntityPM.SalesmanUserId = null;
            this.EntityPM.SalesmanUserEnglishName = null;
        }
        this.CurrentSession.CurrentWindow.Close("OK");
    };
    CustomerSalesmanByProductSplitComponent.prototype.Clone = function () {
        var _this = this;
        this.EntityPM.CustomerSalesmanByProducts.forEach(function (item) {
            var oldItem = new CustomerSalesmanByProductPM_1.CustomerSalesmanByProductPM(null);
            oldItem.SalesmanUserId = item.SalesmanUserId;
            oldItem.SalesmanUserName = item.SalesmanUserName;
            oldItem.ProductTypeCode = item.ProductTypeCode;
            oldItem.CustomerId = item.CustomerId;
            oldItem.IsDirty = item.IsDirty;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.Tenant = item.Tenant;
            oldItem.EntityParentPM = item.EntityParentPM;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            _this.oldItems.push(oldItem);
        });
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('SalesmanUserId');
        this.myCloner.AddField('SalesmanUserEnglishName');
        this.myCloner.AddEntity(this.EntityPM);
    };
    CustomerSalesmanByProductSplitComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldItems.forEach(function (item) {
            var existingItem = _this.EntityPM.CustomerSalesmanByProducts.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.CustomerSalesmanByProducts.forEach(function (item) {
            var oldItem = _this.oldItems.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode; })[0];
            if (oldItem) {
                if (item.SalesmanUserId != oldItem.SalesmanUserId) {
                    item.SalesmanUserId = oldItem.SalesmanUserId;
                }
                if (item.SalesmanUserName != oldItem.SalesmanUserName) {
                    item.SalesmanUserName = oldItem.SalesmanUserName;
                }
                if (item.IsDirty != oldItem.IsDirty) {
                    item.IsDirty = oldItem.IsDirty;
                }
            }
            else {
                addedItems.push(item);
            }
        });
        addedItems.forEach(function (item) {
            _this.EntityPM.RemoveCustomerSalesmanByProductPM(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.AddCustomerSalesmanByProductPM(item);
        });
        this.myCloner.RejectChanges();
    };
    CustomerSalesmanByProductSplitComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerSalesmanByProductSplitComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerSalesmanByProductSplitComponent);
    return CustomerSalesmanByProductSplitComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerSalesmanByProductSplitComponent = CustomerSalesmanByProductSplitComponent;
var CustomerSalesmanByProductSplitLineViewModel = /** @class */ (function (_super) {
    __extends(CustomerSalesmanByProductSplitLineViewModel, _super);
    function CustomerSalesmanByProductSplitLineViewModel(myCustomerPM, entityList, entityPM) {
        var _this = _super.call(this) || this;
        _this.myPartnerId = null;
        _this.myPartnerName = null;
        _this.myPartner = null;
        _this.Customer = myCustomerPM;
        _this.EntityList = entityList;
        _this.EntityPM = entityPM;
        _this.SetPartner();
        return _this;
    }
    CustomerSalesmanByProductSplitLineViewModel.prototype.SetPartner = function () {
        if (this.EntityPM) {
            this.PartnerId = this.EntityPM.SalesmanUserId;
            this.PartnerName = this.EntityPM.SalesmanUserName;
        }
        else {
            this.PartnerId = null;
            this.PartnerName = null;
        }
    };
    Object.defineProperty(CustomerSalesmanByProductSplitLineViewModel.prototype, "ProductTypeCode", {
        get: function () { return this.EntityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductSplitLineViewModel.prototype, "ProductTypeName", {
        get: function () { return this.EntityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductSplitLineViewModel.prototype, "PartnerId", {
        get: function () { return this.myPartnerId; },
        set: function (value) {
            if (this.myPartnerId != value) {
                this.myPartnerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductSplitLineViewModel.prototype, "PartnerName", {
        get: function () { return this.myPartnerName; },
        set: function (value) {
            if (this.myPartnerName != value) {
                this.myPartnerName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductSplitLineViewModel.prototype, "Partner", {
        get: function () { return this.myPartner; },
        set: function (value) {
            if (this.myPartner || value) {
                var isChanged = true;
                if (this.myPartner && value) {
                    isChanged = false;
                    if (this.myPartner.Id != value.Id) {
                        isChanged = true;
                    }
                }
                if (isChanged) {
                    this.myPartner = value;
                    if (value) {
                        if (this.EntityPM) {
                            if (this.EntityPM.SalesmanUserId != value.Id) {
                                this.EntityPM.SalesmanUserId = value.Id;
                            }
                            if (this.EntityPM.SalesmanUserName != value.EnglishName) {
                                this.EntityPM.SalesmanUserName = value.EnglishName;
                            }
                        }
                        else {
                            this.EntityPM = new CustomerSalesmanByProductPM_1.CustomerSalesmanByProductPM(null);
                            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                            this.EntityPM.CustomerId = this.Customer.Id;
                            this.EntityPM.SalesmanUserId = value.Id;
                            this.EntityPM.SalesmanUserName = value.EnglishName;
                            this.Customer.AddCustomerSalesmanByProductPM(this.EntityPM);
                        }
                    }
                    else {
                        this.Customer.RemoveCustomerSalesmanByProductPM(this.EntityPM);
                        this.EntityPM = null;
                    }
                    this.SetPartner();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return CustomerSalesmanByProductSplitLineViewModel;
}(BaseComponent_1.BaseComponent));
exports.CustomerSalesmanByProductSplitLineViewModel = CustomerSalesmanByProductSplitLineViewModel;
//# sourceMappingURL=CustomerSalesmanByProductSplitComponent.js.map