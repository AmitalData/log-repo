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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var ClaimImporterDeclarsPage3BPM_1 = require("../../../../../Customs/EntityPMs/ClaimImporterDeclarsPage3BPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimImporterDeclBCTabComponent = /** @class */ (function (_super) {
    __extends(ClaimImporterDeclBCTabComponent, _super);
    function ClaimImporterDeclBCTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.Claim";
        _this.isControlEnabled = true;
        _this.IsLoaded = false;
        _this._ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaleDeclarlist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsPage3B").subscribe(function (response) {
                if (_this.entityArgs.EntityPM != null) {
                    _this.EntityPM = _this.entityArgs.EntityPM;
                    _this.BuildSaleDeclarList();
                }
                _this.Listen();
                _this.IsLoaded = true;
            });
        });
        return _this;
    }
    ClaimImporterDeclBCTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildSaleDeclarList();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "CLMA") {
                        _this.BuildSaleDeclarList();
                    }
                }
            });
        }
    };
    ClaimImporterDeclBCTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimImporterDeclBCTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclBCTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimImporterDeclBCTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclBCTabComponent.prototype, "ValidationErrorsList", {
        get: function () { return this._ValidationErrorsList; },
        set: function (newValue) { this._ValidationErrorsList = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimImporterDeclBCTabComponent.prototype, "RawMaterialsDescription", {
        get: function () { return this.EntityPM.RawMaterialsDescription; },
        set: function (newValue) { this.EntityPM.RawMaterialsDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimImporterDeclBCTabComponent.prototype.BuildSaleDeclarList = function () {
        this.SaleDeclarlist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimImporterDeclarsPage3B != null && this.EntityPM.ClaimImporterDeclarsPage3B.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimImporterDeclarsPage3B; _i < _a.length; _i++) {
                var item = _a[_i];
                this.SaleDeclarlist.Insert(new SaleDeclarLineComponent(item));
            }
        }
    };
    ClaimImporterDeclBCTabComponent.prototype.AddSaleDeclarCommand = function () {
        if (!this.IsControlEnabled)
            return;
        if (this.SaleDeclarlist != null && this.SaleDeclarlist.Length > 0) {
            var nullVM = this.SaleDeclarlist.Collection.filter(function (vm) { return vm.DescriptionOfGoods == null; });
            if (nullVM.length > 0) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow"));
                return;
            }
        }
        var newClaimImporterDeclarsPage3BPM = new ClaimImporterDeclarsPage3BPM_1.ClaimImporterDeclarsPage3BPM(this.EntityPM);
        newClaimImporterDeclarsPage3BPM.Tenant = this.EntityPM.Tenant;
        newClaimImporterDeclarsPage3BPM.ClaimId = this.EntityPM.Id;
        newClaimImporterDeclarsPage3BPM.LineNo = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimImporterDeclarsPage3, "LineNo") + 1);
        this.SaleDeclarlist.Insert(new SaleDeclarLineComponent(newClaimImporterDeclarsPage3BPM));
        this.EntityPM.AddClaimImporterDeclarsPage3B(newClaimImporterDeclarsPage3BPM);
    };
    ClaimImporterDeclBCTabComponent.prototype.DeleteSaleDeclarCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.SaleDeclarlist.Remove(item);
            this.EntityPM.RemoveClaimImporterDeclarsPage3B(item.entityPM);
        }
    };
    ClaimImporterDeclBCTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimImporterDeclBCTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimImporterDeclBCTabComponent);
    return ClaimImporterDeclBCTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimImporterDeclBCTabComponent = ClaimImporterDeclBCTabComponent;
var SaleDeclarLineComponent = /** @class */ (function (_super) {
    __extends(SaleDeclarLineComponent, _super);
    function SaleDeclarLineComponent(entityPM) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.ObjectTableName = "Customs.ClaimImporterDeclarsPage3B";
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(SaleDeclarLineComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.entityPM.DescriptionOfGoods; },
        set: function (newValue) { this.entityPM.DescriptionOfGoods = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SaleDeclarLineComponent.prototype, "SaleAmountBefore", {
        get: function () { return this.entityPM.SaleAmountBefore; },
        set: function (newValue) { this.entityPM.SaleAmountBefore = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SaleDeclarLineComponent.prototype, "SaleAmountAfter", {
        get: function () { return this.entityPM.SaleAmountAfter; },
        set: function (newValue) { this.entityPM.SaleAmountAfter = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SaleDeclarLineComponent.prototype, "SaleAmountClaim", {
        get: function () { return this.entityPM.SaleAmountClaim; },
        set: function (newValue) { this.entityPM.SaleAmountClaim = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SaleDeclarLineComponent.prototype, "InventoryAmount", {
        get: function () { return this.entityPM.InventoryAmount; },
        set: function (newValue) { this.entityPM.InventoryAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SaleDeclarLineComponent.prototype, "SoldGoodsAmount", {
        get: function () { return this.entityPM.SoldGoodsAmount; },
        set: function (newValue) { this.entityPM.SoldGoodsAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    SaleDeclarLineComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return SaleDeclarLineComponent;
}(BaseComponent_1.BaseComponent));
exports.SaleDeclarLineComponent = SaleDeclarLineComponent;
//# sourceMappingURL=ClaimImporterDeclBCTabComponent.js.map