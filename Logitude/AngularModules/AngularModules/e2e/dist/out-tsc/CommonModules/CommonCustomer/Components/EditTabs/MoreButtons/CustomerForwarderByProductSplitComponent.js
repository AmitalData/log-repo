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
var CustomerForwarderByProductPM_1 = require("../../../../../Common/EntityPMs/CustomerForwarderByProductPM");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var CustomerForwarderByProductSplitComponent = /** @class */ (function (_super) {
    __extends(CustomerForwarderByProductSplitComponent, _super);
    function CustomerForwarderByProductSplitComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customer";
        _this.ProductTypes = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.oldItems = [];
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CustomerForwarderByProductSplitComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.ProductTypes = args['ProductTypes'];
        this.Clone();
        this.ProductTypes.forEach(function (item) {
            var itemPM = _this.EntityPM.CustomerForwarderByProducts.filter(function (d) { return d.ProductTypeCode == item.Code; })[0];
            var itemClass = new CustomerForwarderByProductSplitLineViewModel(_this.EntityPM, item, itemPM);
            _this.ItemsSource.Insert(itemClass);
        });
    };
    CustomerForwarderByProductSplitComponent.prototype.CencelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomerForwarderByProductSplitComponent.prototype.OkButtonClicked = function () {
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerForwarderByProducts.filter(function (f) { return f.ForwarderId != null; }), "ForwarderId");
        if (Forwarders.length == 1) {
            this.EntityPM.ForwarderId = this.EntityPM.CustomerForwarderByProducts.filter(function (f) { return f.ForwarderId != null; })[0].ForwarderId;
            this.EntityPM.ForwarderName = this.EntityPM.CustomerForwarderByProducts.filter(function (f) { return f.ForwarderId != null; })[0].ForwarderName;
        }
        else {
            this.EntityPM.ForwarderId = null;
            this.EntityPM.ForwarderName = null;
        }
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    CustomerForwarderByProductSplitComponent.prototype.Clone = function () {
        var _this = this;
        this.EntityPM.CustomerForwarderByProducts.forEach(function (item) {
            var oldItem = new CustomerForwarderByProductPM_1.CustomerForwarderByProductPM(null);
            oldItem.ForwarderId = item.ForwarderId;
            oldItem.ForwarderName = item.ForwarderName;
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
        this.myCloner.AddField('ForwarderId');
        this.myCloner.AddField('ForwarderName');
        this.myCloner.AddEntity(this.EntityPM);
    };
    CustomerForwarderByProductSplitComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldItems.forEach(function (item) {
            var existingItem = _this.EntityPM.CustomerForwarderByProducts.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.CustomerForwarderByProducts.forEach(function (item) {
            var oldItem = _this.oldItems.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode; })[0];
            if (oldItem) {
                if (item.ForwarderId != oldItem.ForwarderId) {
                    item.ForwarderId = oldItem.ForwarderId;
                }
                if (item.ForwarderName != oldItem.ForwarderName) {
                    item.ForwarderName = oldItem.ForwarderName;
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
            _this.EntityPM.RemoveCustomerForwarderByProductPM(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.AddCustomerForwarderByProductPM(item);
        });
        this.myCloner.RejectChanges();
    };
    CustomerForwarderByProductSplitComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerForwarderByProductSplitComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerForwarderByProductSplitComponent);
    return CustomerForwarderByProductSplitComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerForwarderByProductSplitComponent = CustomerForwarderByProductSplitComponent;
var CustomerForwarderByProductSplitLineViewModel = /** @class */ (function (_super) {
    __extends(CustomerForwarderByProductSplitLineViewModel, _super);
    function CustomerForwarderByProductSplitLineViewModel(myCustomerPM, entityList, entityPM) {
        var _this = _super.call(this) || this;
        _this.LOVIsVisible = true;
        _this.myPartnerId = null;
        _this.myPartnerName = null;
        _this.myPartner = null;
        _this.Customer = myCustomerPM;
        _this.EntityList = entityList;
        _this.EntityPM = entityPM;
        _this.SetPartner();
        _this.SetVisibility();
        return _this;
    }
    CustomerForwarderByProductSplitLineViewModel.prototype.SetVisibility = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
            this.LOVIsVisible = false;
        }
    };
    CustomerForwarderByProductSplitLineViewModel.prototype.SetPartner = function () {
        if (this.EntityPM) {
            this.PartnerId = this.EntityPM.ForwarderId;
            this.PartnerName = this.EntityPM.ForwarderName;
        }
        else {
            this.PartnerId = null;
            this.PartnerName = null;
        }
    };
    Object.defineProperty(CustomerForwarderByProductSplitLineViewModel.prototype, "ProductTypeCode", {
        get: function () { return this.EntityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductSplitLineViewModel.prototype, "ProductTypeName", {
        get: function () { return this.EntityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductSplitLineViewModel.prototype, "PartnerId", {
        get: function () { return this.myPartnerId; },
        set: function (value) {
            if (this.myPartnerId != value) {
                this.myPartnerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductSplitLineViewModel.prototype, "PartnerName", {
        get: function () { return this.myPartnerName; },
        set: function (value) {
            if (this.myPartnerName != value) {
                this.myPartnerName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductSplitLineViewModel.prototype, "Partner", {
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
                            if (this.EntityPM.ForwarderId != value.Id) {
                                this.EntityPM.ForwarderId = value.Id;
                            }
                            if (this.EntityPM.ForwarderName != value.EnglishName) {
                                this.EntityPM.ForwarderName = value.EnglishName;
                            }
                        }
                        else {
                            this.EntityPM = new CustomerForwarderByProductPM_1.CustomerForwarderByProductPM(null);
                            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                            this.EntityPM.CustomerId = this.Customer.Id;
                            this.EntityPM.ForwarderId = value.Id;
                            this.EntityPM.ForwarderName = value.EnglishName;
                            this.Customer.AddCustomerForwarderByProductPM(this.EntityPM);
                        }
                    }
                    else {
                        this.Customer.RemoveCustomerForwarderByProductPM(this.EntityPM);
                        this.EntityPM = null;
                    }
                    this.SetPartner();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return CustomerForwarderByProductSplitLineViewModel;
}(BaseComponent_1.BaseComponent));
exports.CustomerForwarderByProductSplitLineViewModel = CustomerForwarderByProductSplitLineViewModel;
//# sourceMappingURL=CustomerForwarderByProductSplitComponent.js.map