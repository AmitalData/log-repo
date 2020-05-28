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
var TaxDeductionReportLogTabComponent = /** @class */ (function (_super) {
    __extends(TaxDeductionReportLogTabComponent, _super);
    function TaxDeductionReportLogTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "TaxDeductionReport";
        _this.isRTL = false;
        _this.showLocals = false;
        _this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.showLocals = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.UIProperties.SetEnabled("ErrorMessage", "TaxDeductionReport", false);
        return _this;
    }
    Object.defineProperty(TaxDeductionReportLogTabComponent.prototype, "ErrorMessage", {
        get: function () { return this.entityPM.ErrorMessage; },
        enumerable: true,
        configurable: true
    });
    TaxDeductionReportLogTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TaxDeductionReportLogTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TaxDeductionReportLogTabComponent);
    return TaxDeductionReportLogTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TaxDeductionReportLogTabComponent = TaxDeductionReportLogTabComponent;
//# sourceMappingURL=TaxDeductionReportLogTabComponent.js.map