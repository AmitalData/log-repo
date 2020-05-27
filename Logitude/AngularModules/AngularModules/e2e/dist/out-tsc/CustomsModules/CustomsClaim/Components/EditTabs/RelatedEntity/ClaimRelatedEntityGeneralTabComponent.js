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
var ClaimsRelatedEntitiesAmountPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesAmountPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var DeclarationExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimRelatedEntityGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityGeneralTabComponent, _super);
    function ClaimRelatedEntityGeneralTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(null); // added null because it demands a parameter parent.
        _this.ClaimPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.isControlEnabled = true;
        _this.Total = 0;
        _this.DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ClaimsRelatedEntitiesAmountsList = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.CalcClaimsRelatedEntitiesAmountsTotal();
        return _this;
    }
    ClaimRelatedEntityGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            ;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildPaymentAmountList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "CLMG") {
                        _this.RefreshEntity();
                        _this.BuildPaymentAmountList();
                    }
                }
            }));
        }
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.InitTab = function (entityPM, claimPM, isEnable) {
        var _this = this;
        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.isControlEnabled = isEnable;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesAmount").subscribe(function (response) {
                    _this.BuildPaymentAmountList();
                    _this.Listen();
                });
            });
        });
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityGeneralTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "ClaimEntityTypeCode", {
        get: function () { return this.EntityPM.ClaimEntityTypeCode; },
        set: function (newValue) { this.EntityPM.ClaimEntityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "ExternalClaimNumber", {
        get: function () { return this.EntityPM.ExternalClaimNumber; },
        set: function (newValue) { this.EntityPM.ExternalClaimNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "ClaimEntityNumber", {
        get: function () { return this.EntityPM.ClaimEntityNumber; },
        set: function (newValue) { this.EntityPM.ClaimEntityNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "DeclarationVersion", {
        get: function () { return this.EntityPM.DeclarationVersion; },
        set: function (newValue) { this.EntityPM.DeclarationVersion = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "ClaimAmount", {
        get: function () { return this.EntityPM.ClaimAmount; },
        set: function (newValue) { this.EntityPM.ClaimAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityGeneralTabComponent.prototype, "IsFinancialRefundDemand", {
        get: function () { return this.EntityPM.IsFinancialRefundDemand; },
        set: function (newValue) { this.EntityPM.IsFinancialRefundDemand = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityGeneralTabComponent.prototype.BuildPaymentAmountList = function () {
        this.ClaimsRelatedEntitiesAmountsList = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimsRelatedEntitiesAmounts != null && this.EntityPM.ClaimsRelatedEntitiesAmounts.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntitiesAmounts; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntitiesAmountsList.Insert(new ClaimRelatedEntityAmountComponent(item, this.EntityPM));
            }
        }
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.ClaimEntityNumberLostFocus = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ClaimEntityNumber) || this.ClaimEntityTypeCode != "1055") {
            return;
        }
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("");
        this.DeclarationExtendedListService.GetSingleDeclarationByNumber(this.ClaimEntityNumber, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.FetchDeclaration(myResponse, false);
        });
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ExternalClaimNumber)) {
                this.ExternalClaimNumber = lastFetchDeclarationList.CustomFileNo;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(lastFetchDeclarationList.DeclarationVersionId)) {
                this.DeclarationVersion = lastFetchDeclarationList.DeclarationVersionId;
            }
            this.UIProperties.SetValidity("ExternalClaimNumber", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("ClaimEntityNumber", this.ObjectTableName, true, "");
        }
        this.CurrentSession.StopBusyIndicator();
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.ExternalClaimNumberLostFocus = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ExternalClaimNumber) || this.ClaimEntityTypeCode != "1055") {
            return;
        }
        this.UIProperties.SetValidity("ClaimEntityNumber", this.ObjectTableName, true, "");
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("");
        this.DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.ExternalClaimNumber)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchCustomFileNo(myResponse, false);
        });
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.FetchCustomFileNo = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ClaimEntityNumber)) {
                this.ClaimEntityNumber = lastFetchDeclarationList.DeclarationNumber;
                this.UIProperties.SetRequired("ClaimEntityNumber", this.ObjectTableName, false);
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(lastFetchDeclarationList.DeclarationVersionId)) {
                this.DeclarationVersion = lastFetchDeclarationList.DeclarationVersionId;
            }
            this.UIProperties.SetValidity("ExternalClaimNumber", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("ClaimEntityNumber", this.ObjectTableName, true, "");
        }
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.IsFinancialRefundDemandUnchecked = function (item) {
        var _this = this;
        if (item || this.ClaimsRelatedEntitiesAmountsList == null || (this.ClaimsRelatedEntitiesAmountsList != null && this.ClaimsRelatedEntitiesAmountsList.Length == 0)) {
            return;
        }
        var rfundDemandUncheckedWindow = new ConfirmWindow_1.ConfirmWindow();
        rfundDemandUncheckedWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.F.IsFinancialRefundDemand");
        rfundDemandUncheckedWindow.Width = 250;
        rfundDemandUncheckedWindow.Height = 150;
        rfundDemandUncheckedWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
        rfundDemandUncheckedWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
        rfundDemandUncheckedWindow.ShowCancelButton = false;
        rfundDemandUncheckedWindow.Show("בפעולה זו ימחקו כל סעיפי החיוב, האם להמשיך?");
        rfundDemandUncheckedWindow.WindowClosed.subscribe(function (event) {
            if (rfundDemandUncheckedWindow.Yes) {
                _this.DeletePaymentAmoutLines();
            }
            else {
                _this.IsFinancialRefundDemand = true;
            }
        });
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.DeletePaymentAmoutLines = function () {
        if (this.EntityPM.ClaimsRelatedEntitiesAmounts != null && this.EntityPM.ClaimsRelatedEntitiesAmounts.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntitiesAmounts; _i < _a.length; _i++) {
                var item = _a[_i];
                this.EntityPM.RemoveClaimsRelatedEntitiesAmount(item);
            }
            this.ClaimsRelatedEntitiesAmountsList.Clear();
            this.ClaimsRelatedEntitiesAmountsList = new ObservableCollection_1.ObservableCollection([]);
        }
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.AddPaymentAmountCommand = function () {
        if (!this.IsControlEnabled || !this.IsFinancialRefundDemand)
            return;
        var newClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM_1.ClaimsRelatedEntitiesAmountPM(this.EntityPM);
        newClaimsRelatedEntitiesAmountPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntitiesAmountPM.CounterKey = this.EntityPM.EntityCounterKey;
        newClaimsRelatedEntitiesAmountPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntitiesAmountPM.LineNo = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimsRelatedEntitiesAmounts, "LineNo") + 1);
        this.ClaimsRelatedEntitiesAmountsList.Insert(new ClaimRelatedEntityAmountComponent(newClaimsRelatedEntitiesAmountPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntitiesAmount(newClaimsRelatedEntitiesAmountPM);
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.DeletePaymentAmountCommand = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntitiesAmountsList.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntitiesAmount(item.entityPM);
        }
        this.CalcClaimsRelatedEntitiesAmountsTotal();
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.Dispose = function () {
        //this.ClaimsRelatedEntitiesAmountsList.Collection.forEach((item) => {
        //    item.Dispose();
        //});
    };
    ClaimRelatedEntityGeneralTabComponent.prototype.CalcClaimsRelatedEntitiesAmountsTotal = function () {
        var _this = this;
        this.Total = 0;
        if (this.ClaimsRelatedEntitiesAmountsList != null && this.ClaimsRelatedEntitiesAmountsList.Length > 0) {
            this.ClaimsRelatedEntitiesAmountsList.Collection.forEach(function (item) {
                if (item.Amount != null)
                    _this.Total = _this.Total + item.Amount;
            });
        }
        this.EntityPM.ClaimAmount = this.Total;
    };
    ClaimRelatedEntityGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntityGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimRelatedEntityGeneralTabComponent);
    return ClaimRelatedEntityGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityGeneralTabComponent = ClaimRelatedEntityGeneralTabComponent;
var ClaimRelatedEntityAmountComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityAmountComponent, _super);
    function ClaimRelatedEntityAmountComponent(entityPM, claimsRelatedEntity) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.claimsRelatedEntity = claimsRelatedEntity;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntitiesAmount";
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(ClaimRelatedEntityAmountComponent.prototype, "PaymentTypeCode", {
        get: function () { return this.entityPM.PaymentTypeCode; },
        set: function (newValue) { this.entityPM.PaymentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAmountComponent.prototype, "PaymentTypeName", {
        get: function () { return this.entityPM.PaymentTypeName; },
        set: function (newValue) { this.entityPM.PaymentTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityAmountComponent.prototype, "Amount", {
        get: function () { return this.entityPM.Amount; },
        set: function (newValue) { this.entityPM.Amount = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityAmountComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ClaimRelatedEntityAmountComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityAmountComponent = ClaimRelatedEntityAmountComponent;
//# sourceMappingURL=ClaimRelatedEntityGeneralTabComponent.js.map