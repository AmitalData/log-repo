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
var ClaimsRelatedEntsReasonsExpPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntsReasonsExpPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ClaimExplanationCodeListService_1 = require("../../../../../Customs/Services/StandardLists/ClaimExplanationCodeListService");
var ClaimRelatedEntReasonExpComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntReasonExpComponent, _super);
    function ClaimRelatedEntReasonExpComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntsReasonsExp";
        _this.isControlEnabled = true;
        _this.ValidationErrors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrors = [];
        _this.ClaimsRelatedEntsReasonsExpslist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        return _this;
    }
    ClaimRelatedEntReasonExpComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        if (this.EntityPM.ClaimsRelatedEntsReasonsExps != null && this.EntityPM.ClaimsRelatedEntsReasonsExps.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntsReasonsExps; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpLineComponent(item, this.EntityPM));
            }
        }
    };
    ClaimRelatedEntReasonExpComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRelatedEntReasonExpComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntReasonExpComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRelatedEntReasonExpComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntReasonExpComponent.prototype.AddNewEntityReasonExplanationCommand = function () {
        if (!this.IsControlEnabled)
            return;
        if (this.ClaimsRelatedEntsReasonsExpslist != null && this.ClaimsRelatedEntsReasonsExpslist.Length > 0) {
            var nullVM = this.ClaimsRelatedEntsReasonsExpslist.Collection.filter(function (vm) { return vm.ClaimExplanationTypeCode == null || vm.ClaimExplanationTypeCode == ""; });
            if (nullVM.length > 0) {
                this.ValidationErrors = [];
                this.ValidationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow"));
                return;
            }
        }
        var newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM_1.ClaimsRelatedEntsReasonsExpPM(this.EntityPM);
        newClaimsRelatedEntsReasonsExpPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntsReasonsExpPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntsReasonsExpPM.CounterKey = this.EntityPM.CounterKey;
        newClaimsRelatedEntsReasonsExpPM.LineNo = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimsRelatedEntsReasonsExps, "LineNo") + 1);
        this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpLineComponent(newClaimsRelatedEntsReasonsExpPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntsReasonsExp(newClaimsRelatedEntsReasonsExpPM);
    };
    ClaimRelatedEntReasonExpComponent.prototype.DeleteReasonExplanationCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntsReasonsExpslist.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntsReasonsExp(item.entityPM);
        }
    };
    ClaimRelatedEntReasonExpComponent.prototype.CancelButtonClicked = function () {
        this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    ClaimRelatedEntReasonExpComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    ClaimRelatedEntReasonExpComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntReasonExpComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ClaimRelatedEntReasonExpComponent);
    return ClaimRelatedEntReasonExpComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntReasonExpComponent = ClaimRelatedEntReasonExpComponent;
var ClaimRelatedEntReasonExpLineComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntReasonExpLineComponent, _super);
    function ClaimRelatedEntReasonExpLineComponent(entityPM, claimsRelatedEntitiesReasonPM) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.claimsRelatedEntitiesReasonPM = claimsRelatedEntitiesReasonPM;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntsReasonsExp";
        _this.DataContext = _this;
        _this.myClaimExplanationCodeListService = new ClaimExplanationCodeListService_1.ClaimExplanationCodeListService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.ClaimExplanationTypeCode) && Tools_1.AppTool.IsNullOrEmpty(_this.ClaimExplanationTypeName)) {
            _this.myClaimExplanationCodeListService.getSingle(_this.ClaimExplanationTypeCode).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.ClaimExplanationTypeName = myResponse.Result.LocalName;
                    }
                }
            });
        }
        return _this;
    }
    Object.defineProperty(ClaimRelatedEntReasonExpLineComponent.prototype, "ClaimExplanationTypeCode", {
        get: function () { return this.entityPM.ClaimExplanationTypeCode; },
        set: function (newValue) { this.entityPM.ClaimExplanationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntReasonExpLineComponent.prototype, "ClaimExplanationTypeName", {
        get: function () { return this.entityPM.ClaimExplanationTypeName; },
        set: function (newValue) { this.entityPM.ClaimExplanationTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntReasonExpLineComponent.prototype, "ExplanationNote", {
        get: function () { return this.entityPM.ExplanationNote; },
        set: function (newValue) { this.entityPM.ExplanationNote = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntReasonExpLineComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ClaimRelatedEntReasonExpLineComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntReasonExpLineComponent = ClaimRelatedEntReasonExpLineComponent;
//# sourceMappingURL=ClaimRelatedEntReasonExpComponent.js.map