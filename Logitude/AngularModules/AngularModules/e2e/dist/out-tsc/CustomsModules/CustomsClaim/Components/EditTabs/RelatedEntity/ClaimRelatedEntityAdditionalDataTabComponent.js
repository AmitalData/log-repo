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
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var ClaimsRelatedEntityPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM");
var ClaimsRelatedEntsExpDeclarPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntsExpDeclarPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimRelatedEntityAdditionalDataTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityAdditionalDataTabComponent, _super);
    function ClaimRelatedEntityAdditionalDataTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(null);
        _this.ClaimPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.isControlEnabled = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ClaimsRelatedEntsExpDeclarsList = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        return _this;
    }
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildExportDeclarationlist();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    //if (tabCode == "CLMG") {
                    //    this.RefreshEntity();
                    //    this.BuildExportDeclarationlist();
                    //}
                }
            }));
        }
    };
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.InitTab = function (entityPM, claimPM, isEnable) {
        var _this = this;
        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.isControlEnabled = isEnable;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntsExpDeclar").subscribe(function (response) {
                    _this.BuildExportDeclarationlist();
                    _this.Listen();
                });
            });
        });
    };
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "SeconderyClaimEntityCode", {
        get: function () { return this.EntityPM.SeconderyClaimEntityCode; },
        set: function (newValue) { this.EntityPM.SeconderyClaimEntityCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "SeconderyClaimEntityID", {
        get: function () { return this.EntityPM.SeconderyClaimEntityID; },
        set: function (newValue) { this.EntityPM.SeconderyClaimEntityID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "CourtCode", {
        get: function () { return this.EntityPM.CourtCode; },
        set: function (newValue) { this.EntityPM.CourtCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "ProceedingNumber", {
        get: function () { return this.EntityPM.ProceedingNumber; },
        set: function (newValue) { this.EntityPM.ProceedingNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "AbandonmentDestructionReference", {
        get: function () { return this.EntityPM.AbandonmentDestructionReferenc; },
        set: function (newValue) { this.EntityPM.AbandonmentDestructionReferenc = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAdditionalDataTabComponent.prototype, "WarehouseTypeCode", {
        get: function () { return this.EntityPM.WarehouseTypeCode; },
        set: function (newValue) { this.EntityPM.WarehouseTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.BuildExportDeclarationlist = function () {
        this.ClaimsRelatedEntsExpDeclarsList = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimsRelatedEntsExpDeclars != null && this.EntityPM.ClaimsRelatedEntsExpDeclars.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntsExpDeclars; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntsExpDeclarsList.Insert(new ClaimRelatedEntityExpDeclarationComponent(item, this.EntityPM));
            }
        }
    };
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.AddEntityExpDeclarationCommand = function () {
        if (!this.IsControlEnabled)
            return;
        var newClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM_1.ClaimsRelatedEntsExpDeclarPM(this.EntityPM);
        newClaimsRelatedEntsExpDeclarPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntsExpDeclarPM.CounterKey = this.EntityPM.EntityCounterKey;
        newClaimsRelatedEntsExpDeclarPM.Tenant = this.EntityPM.Tenant;
        this.ClaimsRelatedEntsExpDeclarsList.Insert(new ClaimRelatedEntityExpDeclarationComponent(newClaimsRelatedEntsExpDeclarPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntsExpDeclar(newClaimsRelatedEntsExpDeclarPM);
    };
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.DeleteExportDeclarationCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntsExpDeclarsList.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntsExpDeclar(item.entityPM);
        }
    };
    ClaimRelatedEntityAdditionalDataTabComponent.prototype.Dispose = function () {
        //this.ClaimsRelatedEntitiesAmountsList.Collection.forEach((item) => {
        //    item.Dispose();
        //});
    };
    ClaimRelatedEntityAdditionalDataTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntityAdditionalDataTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimRelatedEntityAdditionalDataTabComponent);
    return ClaimRelatedEntityAdditionalDataTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityAdditionalDataTabComponent = ClaimRelatedEntityAdditionalDataTabComponent;
var ClaimRelatedEntityExpDeclarationComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityExpDeclarationComponent, _super);
    function ClaimRelatedEntityExpDeclarationComponent(entityPM, claimsRelatedEntity) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.claimsRelatedEntity = claimsRelatedEntity;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntitiesAmount";
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(ClaimRelatedEntityExpDeclarationComponent.prototype, "ExportDeclarationNumber", {
        get: function () { return this.entityPM.ExportDeclarationNumber; },
        set: function (newValue) { this.entityPM.ExportDeclarationNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    return ClaimRelatedEntityExpDeclarationComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityExpDeclarationComponent = ClaimRelatedEntityExpDeclarationComponent;
//# sourceMappingURL=ClaimRelatedEntityAdditionalDataTabComponent.js.map