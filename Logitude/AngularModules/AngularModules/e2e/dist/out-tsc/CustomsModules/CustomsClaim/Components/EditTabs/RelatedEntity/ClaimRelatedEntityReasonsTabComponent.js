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
var ClaimsRelatedEntityPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM");
var ClaimsRelatedEntitiesReasonPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesReasonPM");
var ClaimsRelatedEntsReasonsExpPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntsReasonsExpPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ClaimRelatedEntReasonExpComponent_1 = require("./ClaimRelatedEntReasonExpComponent");
var ClaimRelatedEntityReasonsTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityReasonsTabComponent, _super);
    function ClaimRelatedEntityReasonsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(new ClaimPM_1.ClaimPM());
        _this.ClaimPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.isControlEnabled = true;
        _this.ValidationErrors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrors = [];
        _this.ClaimsRelatedEntityReasonslist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        return _this;
    }
    ClaimRelatedEntityReasonsTabComponent.prototype.Listen = function () {
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
                    _this.BuildReasonslist();
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
    ClaimRelatedEntityReasonsTabComponent.prototype.InitTab = function (entityPM, claimPM, isEnable) {
        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.isControlEnabled = isEnable;
        this.BuildReasonslist();
    };
    ClaimRelatedEntityReasonsTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRelatedEntityReasonsTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityReasonsTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRelatedEntityReasonsTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityReasonsTabComponent.prototype, "ClaimExplanation", {
        get: function () { return this.EntityPM.ClaimExplanation; },
        set: function (newValue) { this.EntityPM.ClaimExplanation = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityReasonsTabComponent.prototype.BuildReasonslist = function () {
        this.ClaimsRelatedEntityReasonslist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimsRelatedEntitiesReasons != null && this.EntityPM.ClaimsRelatedEntitiesReasons.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntitiesReasons; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntityReasonslist.Insert(new ClaimRelatedEntityReasonComponent(item, this.EntityPM));
            }
        }
    };
    ClaimRelatedEntityReasonsTabComponent.prototype.AddEntityReasonCommand = function () {
        if (!this.IsControlEnabled)
            return;
        this.ValidationErrors = [];
        if (this.ClaimsRelatedEntityReasonslist != null && this.ClaimsRelatedEntityReasonslist.Length > 0) {
            var nullVM = this.ClaimsRelatedEntityReasonslist.Collection.filter(function (vm) { return vm.ReasonListTypeCode == null; });
            if (nullVM.length > 0) {
                this.ValidationErrors = [];
                this.ValidationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow"));
                return;
            }
        }
        if (this.ClaimsRelatedEntityReasonslist != null && this.ClaimsRelatedEntityReasonslist.Length >= 6) {
            this.ValidationErrors = [];
            this.ValidationErrors.push("לא ניתן להוסיף יותר מ 6 שורות");
            return;
        }
        var newClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM_1.ClaimsRelatedEntitiesReasonPM(this.EntityPM);
        newClaimsRelatedEntitiesReasonPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntitiesReasonPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntitiesReasonPM.CounterKey = this.EntityPM.EntityCounterKey;
        newClaimsRelatedEntitiesReasonPM.LineNo = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimsRelatedEntitiesReasons, "LineNo") + 1);
        this.ClaimsRelatedEntityReasonslist.Insert(new ClaimRelatedEntityReasonComponent(newClaimsRelatedEntitiesReasonPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntitiesReason(newClaimsRelatedEntitiesReasonPM);
    };
    ClaimRelatedEntityReasonsTabComponent.prototype.DeleteExportDeclarationCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntityReasonslist.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntitiesReason(item.entityPM);
        }
    };
    ClaimRelatedEntityReasonsTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        if (!this.IsControlEnabled)
            return;
        this.ValidationErrors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(item.ReasonListTypeCode)) {
            this.ValidationErrors = [];
            this.ValidationErrors.push("חובה להזין סיבת תביעה");
            return;
        }
        var windowArgs = {};
        windowArgs.EntityPM = item.entityPM;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 400;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.ReasonAndExplanations");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.SelectionCompleted($event, item); });
        logitudeWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntReasonExpComponent');
    };
    ClaimRelatedEntityReasonsTabComponent.prototype.SelectionCompleted = function (msg, item) {
        if (msg == "Ok") {
            item.BuildReasonsExplanationList();
        }
    };
    ClaimRelatedEntityReasonsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntityReasonsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ClaimRelatedEntityReasonsTabComponent);
    return ClaimRelatedEntityReasonsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityReasonsTabComponent = ClaimRelatedEntityReasonsTabComponent;
var ClaimRelatedEntityReasonComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityReasonComponent, _super);
    function ClaimRelatedEntityReasonComponent(entityPM, claimsRelatedEntity) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.claimsRelatedEntity = claimsRelatedEntity;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntitiesReason";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ClaimsRelatedEntsReasonsExpslist = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildReasonsExplanationList();
        return _this;
    }
    Object.defineProperty(ClaimRelatedEntityReasonComponent.prototype, "ReasonListTypeCode", {
        get: function () { return this.entityPM.ReasonListTypeCode; },
        set: function (newValue) { this.entityPM.ReasonListTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityReasonComponent.prototype, "ReasonListTypeName", {
        get: function () { return this.entityPM.ReasonListTypeName; },
        set: function (newValue) { this.entityPM.ReasonListTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityReasonComponent.prototype, "ClaimExplanationTypeCode", {
        get: function () { return this._ClaimExplanationTypeCode; },
        set: function (newValue) { this._ClaimExplanationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityReasonComponent.prototype, "ClaimExplanationTypeName", {
        get: function () { return this._ClaimExplanationTypeName; },
        set: function (newValue) { this._ClaimExplanationTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityReasonComponent.prototype, "ExplanationNote", {
        get: function () { return this._ExplanationNote; },
        set: function (newValue) { this._ExplanationNote = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityReasonComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    ClaimRelatedEntityReasonComponent.prototype.BuildReasonsExplanationList = function () {
        this.ClaimsRelatedEntsReasonsExpslist = new ObservableCollection_1.ObservableCollection([]);
        if (this.entityPM.ClaimsRelatedEntsReasonsExps != null && this.entityPM.ClaimsRelatedEntsReasonsExps.length > 0) {
            for (var _i = 0, _a = this.entityPM.ClaimsRelatedEntsReasonsExps; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpComponent_1.ClaimRelatedEntReasonExpLineComponent(item, this.entityPM));
                if (this.entityPM.ClaimsRelatedEntsReasonsExps.length == 1) {
                    this.ClaimExplanationTypeCode = this.entityPM.ClaimsRelatedEntsReasonsExps[0].ClaimExplanationTypeCode;
                    this.ClaimExplanationTypeName = this.entityPM.ClaimsRelatedEntsReasonsExps[0].ClaimExplanationTypeName;
                    this.ExplanationNote = this.entityPM.ClaimsRelatedEntsReasonsExps[0].ExplanationNote;
                }
                else if (this.entityPM.ClaimsRelatedEntsReasonsExps.length > 1) {
                    this.ClaimExplanationTypeName = "List";
                    this.ExplanationNote = "List";
                }
            }
        }
    };
    ClaimRelatedEntityReasonComponent.prototype.ReasonsExplanationLostFocus = function (event) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.ReasonListTypeCode)
            || this.ClaimExplanationTypeName == "List"
            || Tools_1.AppTool.IsNullOrEmpty(this.ClaimExplanationTypeName)) {
            return;
        }
        if (this.ClaimsRelatedEntsReasonsExpslist == null || this.ClaimsRelatedEntsReasonsExpslist.Length == 0) {
            this.AddNewClaimsRelatedEntsReasonsExp(this.ClaimExplanationTypeCode, this.ExplanationNote);
        }
        else {
            this.entityPM.ClaimsRelatedEntsReasonsExps[0].ClaimExplanationTypeCode = this.ClaimExplanationTypeCode;
            this.entityPM.ClaimsRelatedEntsReasonsExps[0].ExplanationNote = this.ExplanationNote;
            this.ClaimsRelatedEntsReasonsExpslist.Collection[0].entityPM.ClaimExplanationTypeCode = this.ClaimExplanationTypeCode;
            this.ClaimsRelatedEntsReasonsExpslist.Collection[0].entityPM.ExplanationNote = this.ExplanationNote;
        }
    };
    ClaimRelatedEntityReasonComponent.prototype.AddNewClaimsRelatedEntsReasonsExp = function (claimExplanationTypeCode, explanationNote) {
        var newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM_1.ClaimsRelatedEntsReasonsExpPM(this.EntityPM);
        newClaimsRelatedEntsReasonsExpPM.Tenant = this.entityPM.Tenant;
        newClaimsRelatedEntsReasonsExpPM.ClaimId = this.entityPM.ClaimId;
        newClaimsRelatedEntsReasonsExpPM.CounterKey = this.entityPM.LineNo;
        newClaimsRelatedEntsReasonsExpPM.LineNo = (Tools_1.ArrayTool.Max(this.entityPM.ClaimsRelatedEntsReasonsExps, "LineNo") + 1);
        if (!Tools_1.AppTool.IsNullOrEmpty(claimExplanationTypeCode)) {
            newClaimsRelatedEntsReasonsExpPM.ClaimExplanationTypeCode = claimExplanationTypeCode;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(explanationNote)) {
            newClaimsRelatedEntsReasonsExpPM.ExplanationNote = explanationNote;
        }
        this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpComponent_1.ClaimRelatedEntReasonExpLineComponent(newClaimsRelatedEntsReasonsExpPM, this.entityPM));
        this.entityPM.AddClaimsRelatedEntsReasonsExp(newClaimsRelatedEntsReasonsExpPM);
    };
    return ClaimRelatedEntityReasonComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityReasonComponent = ClaimRelatedEntityReasonComponent;
//# sourceMappingURL=ClaimRelatedEntityReasonsTabComponent.js.map