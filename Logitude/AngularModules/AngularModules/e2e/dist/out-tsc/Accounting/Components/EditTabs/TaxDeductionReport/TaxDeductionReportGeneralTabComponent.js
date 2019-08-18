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
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TaxDeductionReportExtendedPMService_1 = require("../../../Services/ExtendedPMs/TaxDeductionReportExtendedPMService");
var BatchTaskExecutionListService_1 = require("../../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService");
var TaxDeductionReportPMService_1 = require("../../../Services/StandardPMs/TaxDeductionReportPMService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var TaxDeductionReportGeneralTabComponent = /** @class */ (function (_super) {
    __extends(TaxDeductionReportGeneralTabComponent, _super);
    function TaxDeductionReportGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "TaxDeductionReport";
        _this.isRTL = false;
        _this.showLocals = false;
        _this.taxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService_1.TaxDeductionReportExtendedPMService();
        _this._BatchTaskExecutionListService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
        _this.Faild = false;
        _this.taxDeductionReportPMService = new TaxDeductionReportPMService_1.TaxDeductionReportPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Building = false;
        _this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.showLocals = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.UIProperties.SetEnabled("IsAdditionalReportExist", "TaxDeductionReport", false);
        _this.UIProperties.SetEnabled("Email", "TaxDeductionReport", false);
        if (_this.entityPM.StatusTypeCode == "4") {
            _this.Faild = true;
        }
        return _this;
    }
    Object.defineProperty(TaxDeductionReportGeneralTabComponent.prototype, "IsAdditionalReportExist", {
        get: function () { return this.entityPM.IsAdditionalReportExist; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxDeductionReportGeneralTabComponent.prototype, "Email", {
        get: function () { return this.entityPM.Email; },
        enumerable: true,
        configurable: true
    });
    TaxDeductionReportGeneralTabComponent.prototype.RunService = function () {
        var _this = this;
        this.entityPM.StatusTypeCode = "2";
        this.Building = true;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.taxDeductionReportPMService.update(this.entityPM).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.taxDeductionReportExtendedPMService.DownloadTaxDeduction856FileInBatch(_this.entityPM).subscribe(function (myResult) {
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
    TaxDeductionReportGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TaxDeductionReportGeneralTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TaxDeductionReportGeneralTabComponent);
    return TaxDeductionReportGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TaxDeductionReportGeneralTabComponent = TaxDeductionReportGeneralTabComponent;
//# sourceMappingURL=TaxDeductionReportGeneralTabComponent.js.map