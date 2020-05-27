"use strict";
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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var BankDepositShortTitleComponent = /** @class */ (function () {
    function BankDepositShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.isRTL = false;
        this.txt_cash = TextCodeTranslator_1.TextCodeTranslator.Translate('BankDeposit.Q.cash');
        this.txt_chequeDeposit = TextCodeTranslator_1.TextCodeTranslator.Translate('BankDeposit.Q.chequeDeposit');
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoadCompletedEvent = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
        }
        this.Listen();
    }
    BankDepositShortTitleComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.ComponentId;
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    BankDepositShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./BankDepositShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BankDepositShortTitleComponent);
    return BankDepositShortTitleComponent;
}());
exports.BankDepositShortTitleComponent = BankDepositShortTitleComponent;
//# sourceMappingURL=BankDepositShortTitleComponent.js.map