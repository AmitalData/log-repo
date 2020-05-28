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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var ClaimsRelatedEntityPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ClaimRelatedEntityClaimDecisionTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityClaimDecisionTabComponent, _super);
    function ClaimRelatedEntityClaimDecisionTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(new ClaimPM_1.ClaimPM());
        _this.ClaimPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.IsControlEnabled = true;
        _this.ValidationErrors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrors = [];
        _this.ClaimsRelatedEntitiesSeizureslist = new ObservableCollection_1.ObservableCollection([]);
        _this.ClaimsRelatedEntitiesRefundslist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        return _this;
    }
    ClaimRelatedEntityClaimDecisionTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildSeizureslist();
                    _this.BuildRefundslist();
                }
            });
            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    //if (tabCode == "CLMG") {
                    //    this.RefreshEntity();
                    //    this.BuildReasonslist();
                    //}
                }
            });
        }
    };
    ClaimRelatedEntityClaimDecisionTabComponent.prototype.InitTab = function (entityPM, claimPM, isEnable) {
        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.UIProperties.SetEnabled("DecisionCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DecisionNote", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EilatVatRefoundDecision", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DepositingAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RefundAmount", this.ObjectTableName, false);
        this.BuildSeizureslist();
        this.BuildRefundslist();
    };
    ClaimRelatedEntityClaimDecisionTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityClaimDecisionTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "DecisionCode", {
        get: function () { return this.EntityPM.DecisionCode; },
        set: function (newValue) { this.EntityPM.DecisionCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "DecisionName", {
        get: function () { return this.EntityPM.DecisionName; },
        set: function (newValue) { this.EntityPM.DecisionName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "DepositingAmount", {
        get: function () { return this.EntityPM.DepositingAmount; },
        set: function (newValue) { this.EntityPM.DepositingAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "RefundAmount", {
        get: function () { return this.EntityPM.RefundAmount; },
        set: function (newValue) { this.EntityPM.RefundAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "DecisionNote", {
        get: function () { return this.EntityPM.DecisionNote; },
        set: function (newValue) { this.EntityPM.DecisionNote = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityClaimDecisionTabComponent.prototype, "EilatVatRefoundDecision", {
        get: function () { return this.EntityPM.EilatVatRefoundDecision; },
        set: function (newValue) { this.EntityPM.EilatVatRefoundDecision = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityClaimDecisionTabComponent.prototype.BuildSeizureslist = function () {
        this.ClaimsRelatedEntitiesSeizureslist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimsRelatedEntitiesSeizures != null && this.EntityPM.ClaimsRelatedEntitiesSeizures.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntitiesSeizures; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntitiesSeizureslist.Insert(item);
            }
        }
    };
    ClaimRelatedEntityClaimDecisionTabComponent.prototype.BuildRefundslist = function () {
        this.ClaimsRelatedEntitiesRefundslist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimsRelatedEntitiesRefunds != null && this.EntityPM.ClaimsRelatedEntitiesRefunds.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntitiesRefunds; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntitiesRefundslist.Insert(item);
            }
        }
    };
    ClaimRelatedEntityClaimDecisionTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntityClaimDecisionTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ClaimRelatedEntityClaimDecisionTabComponent);
    return ClaimRelatedEntityClaimDecisionTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityClaimDecisionTabComponent = ClaimRelatedEntityClaimDecisionTabComponent;
//# sourceMappingURL=ClaimRelatedEntityClaimDecisionTabComponent.js.map