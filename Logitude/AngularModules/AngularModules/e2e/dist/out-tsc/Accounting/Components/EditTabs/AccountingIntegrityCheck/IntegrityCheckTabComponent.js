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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AccountingEntegrityCheckExtendedPMService_1 = require("../../../Services/ExtendedPMs/AccountingEntegrityCheckExtendedPMService");
var AccountingIntegrityCheckPMService_1 = require("../../../Services/StandardPMs/AccountingIntegrityCheckPMService");
var IntegrityCheckTabComponent = /** @class */ (function (_super) {
    __extends(IntegrityCheckTabComponent, _super);
    function IntegrityCheckTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.entityPM = null;
        _this.ObjectTableName = "AccountingIntegrityCheck";
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.showLocals = false;
        _this.AccountingEntegrityCheckExtendedPMService = new AccountingEntegrityCheckExtendedPMService_1.AccountingEntegrityCheckExtendedPMService();
        _this._parameters = new IntegrityCheckParameters();
        _this.AccountingIntegrityCheckPMService = new AccountingIntegrityCheckPMService_1.AccountingIntegrityCheckPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.HasException = false;
        _this.Fixing = false;
        //#endregion
        //#region Date Filters Validation
        _this.isValidate = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.showLocals = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.entityPM = entityArgs.EntityPM;
        _this.HasException = _this.entityPM.HasException;
        // this.encodeParameters();
        // this.decodeParameters();
        if (_this.entityPM.StatusCode == "2")
            _this.Fixing = true;
        _this.SetUIProperty();
        return _this;
    }
    IntegrityCheckTabComponent.prototype.ngOnInit = function () {
    };
    IntegrityCheckTabComponent.prototype.ReloadScreen = function () {
    };
    IntegrityCheckTabComponent.prototype.SetUIProperty = function () {
        this.UIProperties.SetEnabled("ResultXML", this.ObjectTableName, false);
        if (this.entityPM.Id && this.entityPM.Id != "new") {
            this.UIProperties.SetEnabled("FromMonthInclusive", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToMonthInclusive", this.ObjectTableName, false);
        }
    };
    Object.defineProperty(IntegrityCheckTabComponent.prototype, "Tenant", {
        //#region Properties
        get: function () { return this._parameters.Tenant; },
        set: function (value) {
            if (this.entityPM.Tenant != value) {
                this.entityPM.Tenant = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(IntegrityCheckTabComponent.prototype, "FromMonthInclusive", {
        get: function () { return this.entityPM.FromMonthInclusive; },
        set: function (value) {
            if (this.entityPM.FromMonthInclusive != value) {
                this.entityPM.FromMonthInclusive = value;
                // this.encodeParameters();
                if (!this.isValidate)
                    this.validateDates();
                else {
                    this.isValidate = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(IntegrityCheckTabComponent.prototype, "ToMonthInclusive", {
        get: function () { return this.entityPM.ToMonthInclusive; },
        set: function (value) {
            if (this.entityPM.ToMonthInclusive != value) {
                this.entityPM.ToMonthInclusive = value;
                // this.encodeParameters();
                if (!this.isValidate)
                    this.validateDates();
                else {
                    this.isValidate = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(IntegrityCheckTabComponent.prototype, "ResultXML", {
        get: function () { return this.entityPM.ResultXML; },
        set: function (value) {
            if (this.entityPM.ResultXML != value) {
                this.entityPM.ResultXML = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    IntegrityCheckTabComponent.prototype.validateDates = function () {
        var _this = this;
        setTimeout(function () {
            if (_this.FromMonthInclusive > _this.ToMonthInclusive) {
                _this.UIProperties.SetValidity("ToMonthInclusive", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                _this.UIProperties.SetValidity("FromMonthInclusive", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                _this.CD.detectChanges();
            }
            else {
                _this.UIProperties.SetValidity("ToMonthInclusive", _this.ObjectTableName, true, "");
                _this.UIProperties.SetValidity("FromMonthInclusive", _this.ObjectTableName, true, "");
                _this.CD.detectChanges();
            }
        }, 200);
    };
    IntegrityCheckTabComponent.prototype.ReloadData = function () {
    };
    IntegrityCheckTabComponent.prototype.RunService = function () {
        var _this = this;
        this.Fixing = true;
        this.entityPM.StatusCode = "2";
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.AccountingIntegrityCheckPMService.update(this.entityPM).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.AccountingEntegrityCheckExtendedPMService.PostFixEntegrityCheckErrorInBatch(_this.entityPM).subscribe(function (myResult) {
                        _this.CurrentSession.StopBusyIndicator();
                        var mm = myResult;
                        var entity = mm.Result;
                    });
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    IntegrityCheckTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './IntegrityCheckTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], IntegrityCheckTabComponent);
    return IntegrityCheckTabComponent;
}(BaseComponent_1.BaseComponent));
exports.IntegrityCheckTabComponent = IntegrityCheckTabComponent;
var IntegrityCheckParameters = /** @class */ (function () {
    function IntegrityCheckParameters() {
    }
    return IntegrityCheckParameters;
}());
exports.IntegrityCheckParameters = IntegrityCheckParameters;
//# sourceMappingURL=IntegrityCheckTabComponent.js.map