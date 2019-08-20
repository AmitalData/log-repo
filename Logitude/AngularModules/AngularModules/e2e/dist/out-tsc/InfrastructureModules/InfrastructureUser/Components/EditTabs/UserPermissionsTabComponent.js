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
var UserPermittedBranchPM_1 = require("../../../../Common/EntityPMs/UserPermittedBranchPM");
var UserPermittedProductPM_1 = require("../../../../Common/EntityPMs/UserPermittedProductPM");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var BranchListService_1 = require("../../../../Common/Services/StandardLists/BranchListService");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var UserPermissionsTabComponent = /** @class */ (function (_super) {
    __extends(UserPermissionsTabComponent, _super);
    function UserPermissionsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "User";
        _this.DataContext = _this;
        _this.UserBranches = [];
        _this.UserProducts = [];
        _this.ComboBoxBranches = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = false;
        _this.IsProductsVisible = false;
        _this.isNotBranchRestricted = false;
        _this.isNotProductRestricted = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.isNotBranchRestricted = !_this.IsBranchRestricted;
        _this.isNotProductRestricted = !_this.IsProductRestricted;
        _this.SetUIProperties();
        _this.BuildPermittedBranchesList();
        _this.BuildPermittedProductsList();
        _this.Listen();
        return _this;
    }
    UserPermissionsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.SetUIProperties();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.SetUIProperties();
                    }
                });
            }
        }
    };
    UserPermissionsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    UserPermissionsTabComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = true;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "PRODUCTS")) {
            this.IsProductsVisible = true;
        }
    };
    Object.defineProperty(UserPermissionsTabComponent.prototype, "BranchId", {
        // Branches
        get: function () { return this.EntityPM.BranchId; },
        set: function (value) {
            if (this.EntityPM.BranchId != value) {
                this.EntityPM.BranchId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermissionsTabComponent.prototype, "IsNotBranchRestricted", {
        get: function () { return this.isNotBranchRestricted; },
        set: function (value) {
            if (this.isNotBranchRestricted != value) {
                this.isNotBranchRestricted = value;
                this.EntityPM.IsBranchRestricted = !value;
                this.EntityPM.UserPermittedBranches = [];
                this.BuildPermittedBranchesList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermissionsTabComponent.prototype, "IsBranchRestricted", {
        get: function () { return this.EntityPM.IsBranchRestricted; },
        set: function (value) {
            if (this.EntityPM.IsBranchRestricted != value) {
                this.isNotBranchRestricted = !value;
                this.EntityPM.IsBranchRestricted = value;
                this.BuildPermittedBranchesList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermissionsTabComponent.prototype, "SelectedBranch", {
        get: function () { return this.selectedBranch; },
        set: function (value) {
            if (this.selectedBranch != value) {
                this.selectedBranch = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    UserPermissionsTabComponent.prototype.SelectedBranchChanged = function (myBranch) {
        if (this.SelectedBranch != myBranch) {
            this.SelectedBranch = myBranch;
            if (myBranch == null) {
                this.BranchId = null;
            }
            else {
                this.BranchId = myBranch.Id;
            }
        }
    };
    Object.defineProperty(UserPermissionsTabComponent.prototype, "ProductTypeCode", {
        // Products
        get: function () { return this.EntityPM.ProductTypeCode; },
        set: function (value) {
            if (this.EntityPM.ProductTypeCode != value) {
                this.EntityPM.ProductTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermissionsTabComponent.prototype, "IsNotProductRestricted", {
        get: function () { return this.isNotProductRestricted; },
        set: function (value) {
            if (this.isNotProductRestricted != value) {
                this.isNotProductRestricted = value;
                this.EntityPM.IsProductRestricted = !value;
                this.EntityPM.UserPermittedProducts = [];
                this.BuildPermittedProductsList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermissionsTabComponent.prototype, "IsProductRestricted", {
        get: function () { return this.EntityPM.IsProductRestricted; },
        set: function (value) {
            if (this.EntityPM.IsProductRestricted != value) {
                this.isNotProductRestricted = !value;
                this.EntityPM.IsProductRestricted = value;
                this.BuildPermittedProductsList();
            }
        },
        enumerable: true,
        configurable: true
    });
    UserPermissionsTabComponent.prototype.BuildPermittedBranchesList = function () {
        var _this = this;
        var myService = new BranchListService_1.BranchListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            _this.UserBranches = [];
            _this.ComboBoxBranches = [];
            if (!myResponse.HasError) {
                var allItems = myResponse.Result;
                allItems = allItems.sort(function (a, b) { return a.Id.toLowerCase() == b.Id.toLowerCase() ? 0 : a.Id.toLowerCase() < b.Id.toLowerCase() ? -1 : 1; });
                allItems.forEach(function (item) {
                    var newBranch = new UserBranchClass(item, _this);
                    _this.UserBranches.push(newBranch);
                    if (_this.IsNotBranchRestricted) {
                        _this.ComboBoxBranches.push(item);
                    }
                    else {
                        if (newBranch.IsPermitted) {
                            _this.ComboBoxBranches.push(item);
                        }
                    }
                });
                _this.SelectedBranch = _this.ComboBoxBranches.filter(function (f) { return f.Id == _this.BranchId; })[0];
            }
        });
    };
    UserPermissionsTabComponent.prototype.BuildPermittedProductsList = function () {
        var _this = this;
        if (this.IsProductsVisible) {
            var myService = new ProductTypeListService_1.ProductTypeListService();
            myService.getAllFromCache().subscribe(function (myResponse) {
                _this.UserProducts = [];
                if (!myResponse.HasError) {
                    var allItems = myResponse.Result;
                    allItems = allItems.filter(function (f) { return f.InActive == false; });
                    allItems = allItems.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });
                    allItems.forEach(function (item) {
                        _this.UserProducts.push(new UserProductClass(item, _this));
                    });
                }
            });
        }
    };
    UserPermissionsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UserPermissionsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], UserPermissionsTabComponent);
    return UserPermissionsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.UserPermissionsTabComponent = UserPermissionsTabComponent;
var UserBranchClass = /** @class */ (function () {
    function UserBranchClass(entity, fatherComponent) {
        var _this = this;
        this.fatherComponent = fatherComponent;
        this.isPermitted = false;
        this.Entity = entity;
        if (fatherComponent.EntityPM.UserPermittedBranches.filter(function (f) { return f.BranchId == _this.Id; }).length > 0) {
            this.isPermitted = true;
        }
    }
    Object.defineProperty(UserBranchClass.prototype, "Id", {
        get: function () { return this.Entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserBranchClass.prototype, "Name", {
        get: function () { return this.Entity.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserBranchClass.prototype, "IsDefauldBranch", {
        get: function () { return this.Id == this.fatherComponent.BranchId ? true : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserBranchClass.prototype, "IsPermitted", {
        get: function () { return this.isPermitted; },
        set: function (value) {
            var _this = this;
            if (this.isPermitted != value) {
                this.isPermitted = value;
                var itemPM = this.fatherComponent.EntityPM.UserPermittedBranches.filter(function (f) { return f.BranchId == _this.Id; })[0];
                if (value) {
                    if (itemPM == null) {
                        itemPM = new UserPermittedBranchPM_1.UserPermittedBranchPM(null);
                        itemPM.BranchId = this.Id;
                        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        itemPM.UserId = this.fatherComponent.EntityPM.Id;
                        this.fatherComponent.EntityPM.AddUserPermittedBranchPM(itemPM);
                    }
                }
                else {
                    if (itemPM != null) {
                        this.fatherComponent.EntityPM.RemoveUserPermittedBranchPM(itemPM);
                    }
                }
                this.UpdateFather();
            }
        },
        enumerable: true,
        configurable: true
    });
    UserBranchClass.prototype.UpdateFather = function () {
        var _this = this;
        if (this.fatherComponent.EntityPM.UserPermittedBranches.length == 1) {
            this.fatherComponent.BranchId = this.fatherComponent.EntityPM.UserPermittedBranches[0].BranchId;
        }
        else if (this.fatherComponent.EntityPM.UserPermittedBranches.filter(function (f) { return f.BranchId == _this.fatherComponent.BranchId; }).length == 0) {
            if (this.fatherComponent.EntityPM.UserPermittedBranches.length == 0) {
                this.fatherComponent.BranchId = null;
            }
            else {
                this.fatherComponent.BranchId = this.fatherComponent.EntityPM.UserPermittedBranches[0].BranchId;
            }
        }
    };
    return UserBranchClass;
}());
exports.UserBranchClass = UserBranchClass;
var UserProductClass = /** @class */ (function () {
    function UserProductClass(entity, fatherComponent) {
        var _this = this;
        this.fatherComponent = fatherComponent;
        this.isPermitted = false;
        this.Entity = entity;
        if (fatherComponent.EntityPM.UserPermittedProducts.filter(function (f) { return f.ProductTypeCode == _this.Code; }).length > 0) {
            this.isPermitted = true;
        }
    }
    Object.defineProperty(UserProductClass.prototype, "Id", {
        get: function () { return this.Entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserProductClass.prototype, "Code", {
        get: function () { return this.Entity.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserProductClass.prototype, "Name", {
        get: function () { return this.Entity.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserProductClass.prototype, "DirectionId", {
        get: function () {
            if (this.Code == "CI") {
                return "C";
            }
            else {
                return this.Code[1];
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserProductClass.prototype, "TransportModeId", {
        get: function () {
            if (this.Code == "CI") {
                return null;
            }
            else {
                return this.Code[0];
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserProductClass.prototype, "IsDefauldProduct", {
        get: function () { return this.Code == this.fatherComponent.ProductTypeCode ? true : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserProductClass.prototype, "IsPermitted", {
        get: function () { return this.isPermitted; },
        set: function (value) {
            var _this = this;
            if (this.isPermitted != value) {
                this.isPermitted = value;
                var itemPM = this.fatherComponent.EntityPM.UserPermittedProducts.filter(function (f) { return f.ProductTypeCode == _this.Code; })[0];
                if (value) {
                    if (itemPM == null) {
                        itemPM = new UserPermittedProductPM_1.UserPermittedProductPM(null);
                        itemPM.ProductTypeCode = this.Code;
                        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        itemPM.UserId = this.fatherComponent.EntityPM.Id;
                        this.fatherComponent.EntityPM.AddUserPermittedProductPM(itemPM);
                    }
                }
                else {
                    if (itemPM != null) {
                        this.fatherComponent.EntityPM.RemoveUserPermittedProductPM(itemPM);
                    }
                }
                this.UpdateFather();
            }
        },
        enumerable: true,
        configurable: true
    });
    UserProductClass.prototype.UpdateFather = function () {
        var _this = this;
        if (this.fatherComponent.EntityPM.UserPermittedProducts.length == 1) {
            this.fatherComponent.ProductTypeCode = this.fatherComponent.EntityPM.UserPermittedProducts[0].ProductTypeCode;
        }
        else if (this.fatherComponent.EntityPM.UserPermittedProducts.filter(function (f) { return f.ProductTypeCode == _this.fatherComponent.ProductTypeCode; }).length == 0) {
            if (this.fatherComponent.EntityPM.UserPermittedProducts.length == 0) {
                this.fatherComponent.ProductTypeCode = null;
            }
            else {
                this.fatherComponent.ProductTypeCode = this.fatherComponent.EntityPM.UserPermittedProducts[0].ProductTypeCode;
            }
        }
    };
    return UserProductClass;
}());
exports.UserProductClass = UserProductClass;
//# sourceMappingURL=UserPermissionsTabComponent.js.map