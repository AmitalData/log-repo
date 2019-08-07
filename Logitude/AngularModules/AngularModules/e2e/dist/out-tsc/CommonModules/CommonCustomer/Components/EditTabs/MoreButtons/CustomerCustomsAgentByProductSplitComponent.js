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
var CustomerCustomsAgentByProductPM_1 = require("../../../../../Common/EntityPMs/CustomerCustomsAgentByProductPM");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var CustomerCustomsAgentByProductSplitComponent = /** @class */ (function (_super) {
    __extends(CustomerCustomsAgentByProductSplitComponent, _super);
    function CustomerCustomsAgentByProductSplitComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customer";
        _this.ProductTypes = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.oldItems = [];
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CustomerCustomsAgentByProductSplitComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.ProductTypes = args['ProductTypes'];
        this.Clone();
        this.ProductTypes.forEach(function (item) {
            var itemPM = _this.EntityPM.CustomerCustomsAgentByProducts.filter(function (d) { return d.ProductTypeCode == item.Code; })[0];
            var itemClass = new CustomerCustomsAgentByProductSplitLineViewModel(_this.EntityPM, item, itemPM);
            _this.ItemsSource.Insert(itemClass);
        });
    };
    CustomerCustomsAgentByProductSplitComponent.prototype.CencelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomerCustomsAgentByProductSplitComponent.prototype.OkButtonClicked = function () {
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var myBaseData = this.EntityPM.CustomerCustomsAgentByProducts.filter(function (f) { return f.CustomsAgentId != null; });
        var Forwarders = myPipe.transform(myBaseData, "CustomsAgentId");
        if (Forwarders.length == 1) {
            this.EntityPM.CustomsAgentId = myBaseData[0].CustomsAgentId;
            this.EntityPM.CustomsAgentName = myBaseData[0].CustomsAgentName;
        }
        else {
            this.EntityPM.CustomsAgentId = null;
            this.EntityPM.CustomsAgentName = null;
        }
        this.CurrentSession.CurrentWindow.Close("OK");
    };
    CustomerCustomsAgentByProductSplitComponent.prototype.Clone = function () {
        var _this = this;
        this.EntityPM.CustomerCustomsAgentByProducts.forEach(function (item) {
            var oldItem = new CustomerCustomsAgentByProductPM_1.CustomerCustomsAgentByProductPM(null);
            oldItem.CustomsAgentId = item.CustomsAgentId;
            oldItem.CustomsAgentName = item.CustomsAgentName;
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
        this.myCloner.AddField('CustomsAgentId');
        this.myCloner.AddField('CustomsAgentName');
        this.myCloner.AddEntity(this.EntityPM);
    };
    CustomerCustomsAgentByProductSplitComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldItems.forEach(function (item) {
            var existingItem = _this.EntityPM.CustomerCustomsAgentByProducts.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.CustomerCustomsAgentByProducts.forEach(function (item) {
            var oldItem = _this.oldItems.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode; })[0];
            if (oldItem) {
                if (item.CustomsAgentId != oldItem.CustomsAgentId) {
                    item.CustomsAgentId = oldItem.CustomsAgentId;
                }
                if (item.CustomsAgentName != oldItem.CustomsAgentName) {
                    item.CustomsAgentName = oldItem.CustomsAgentName;
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
            _this.EntityPM.RemoveCustomerCustomsAgentByProductPM(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.AddCustomerCustomsAgentByProductPM(item);
        });
        this.myCloner.RejectChanges();
    };
    CustomerCustomsAgentByProductSplitComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerCustomsAgentByProductSplitComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerCustomsAgentByProductSplitComponent);
    return CustomerCustomsAgentByProductSplitComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerCustomsAgentByProductSplitComponent = CustomerCustomsAgentByProductSplitComponent;
var CustomerCustomsAgentByProductSplitLineViewModel = /** @class */ (function (_super) {
    __extends(CustomerCustomsAgentByProductSplitLineViewModel, _super);
    function CustomerCustomsAgentByProductSplitLineViewModel(myCustomerPM, entityList, entityPM) {
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
    CustomerCustomsAgentByProductSplitLineViewModel.prototype.SetVisibility = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
            this.LOVIsVisible = false;
        }
    };
    CustomerCustomsAgentByProductSplitLineViewModel.prototype.SetPartner = function () {
        if (this.EntityPM) {
            this.PartnerId = this.EntityPM.CustomsAgentId;
            this.PartnerName = this.EntityPM.CustomsAgentName;
        }
        else {
            this.PartnerId = null;
            this.PartnerName = null;
        }
    };
    Object.defineProperty(CustomerCustomsAgentByProductSplitLineViewModel.prototype, "ProductTypeCode", {
        get: function () { return this.EntityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductSplitLineViewModel.prototype, "ProductTypeName", {
        get: function () { return this.EntityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductSplitLineViewModel.prototype, "PartnerId", {
        get: function () { return this.myPartnerId; },
        set: function (value) {
            if (this.myPartnerId != value) {
                this.myPartnerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductSplitLineViewModel.prototype, "PartnerName", {
        get: function () { return this.myPartnerName; },
        set: function (value) {
            if (this.myPartnerName != value) {
                this.myPartnerName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductSplitLineViewModel.prototype, "Partner", {
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
                            if (this.EntityPM.CustomsAgentId != value.Id) {
                                this.EntityPM.CustomsAgentId = value.Id;
                            }
                            if (this.EntityPM.CustomsAgentName != value.EnglishName) {
                                this.EntityPM.CustomsAgentName = value.EnglishName;
                            }
                        }
                        else {
                            this.EntityPM = new CustomerCustomsAgentByProductPM_1.CustomerCustomsAgentByProductPM(null);
                            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                            this.EntityPM.CustomerId = this.Customer.Id;
                            this.EntityPM.CustomsAgentId = value.Id;
                            this.EntityPM.CustomsAgentName = value.EnglishName;
                            this.Customer.AddCustomerCustomsAgentByProductPM(this.EntityPM);
                        }
                    }
                    else {
                        this.Customer.RemoveCustomerCustomsAgentByProductPM(this.EntityPM);
                        this.EntityPM = null;
                    }
                    this.SetPartner();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return CustomerCustomsAgentByProductSplitLineViewModel;
}(BaseComponent_1.BaseComponent));
exports.CustomerCustomsAgentByProductSplitLineViewModel = CustomerCustomsAgentByProductSplitLineViewModel;
//# sourceMappingURL=CustomerCustomsAgentByProductSplitComponent.js.map