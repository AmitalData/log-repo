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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ShipmentDomainService_1 = require("../../../Shipment/Services/ShipmentDomainService");
var CreditLimitPopupComponent = /** @class */ (function () {
    function CreditLimitPopupComponent() {
        this.IsError = false;
        this.IsOkButtonVisible = false;
        this.HasCreditOverrideFeature = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        this.HasCreditOverrideFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
    }
    CreditLimitPopupComponent.prototype.SetWindowArgs = function (args) {
        var errors = args['Errors'];
        var warnings = args['Warnings'];
        var shipmentId = args['ShipmentId'];
        var isBlockingShipment = args['IsBlockingShipment'];
        if (errors.length > 0) {
            this.IsError = true;
            this.ErrorMessage = errors[0];
            this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Approve");
            if (this.HasCreditOverrideFeature) {
                this.IsOkButtonVisible = true;
            }
        }
        else {
            this.WarningMessage = warnings[0];
            this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Continue");
            this.IsOkButtonVisible = true;
        }
        if (isBlockingShipment == true) {
            var myService = new ShipmentDomainService_1.ShipmentDomainService();
            myService.BlockNewARInvoice(shipmentId).subscribe(function (myResponse) {
            });
        }
    };
    CreditLimitPopupComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CreditLimitPopupComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    CreditLimitPopupComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CreditLimitPopupComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CreditLimitPopupComponent);
    return CreditLimitPopupComponent;
}());
exports.CreditLimitPopupComponent = CreditLimitPopupComponent;
//# sourceMappingURL=CreditLimitPopupComponent.js.map