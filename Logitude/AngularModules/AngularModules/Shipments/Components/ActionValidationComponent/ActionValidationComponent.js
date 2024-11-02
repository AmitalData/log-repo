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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ActionValidationComponent = (function () {
    function ActionValidationComponent() {
        this.IsConfirmButtonVisible = false;
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
    }
    ActionValidationComponent.prototype.SetWindowArgs = function (args) {
        this.Tenant = args.EnttiyPM.Tenant;
        this.entityPM = args.EnttiyPM;
        this.ValidationErrorsList = args.ErrorsList;
        this.ValidationWarningsList = args.WarningsList;
        this.InitializeComponent();
    };
    ActionValidationComponent.prototype.InitializeComponent = function () {
        this.SetConfirmButton();
    };
    // Warnings
    ActionValidationComponent.prototype.SetWarnings = function () {
        var warnings = [];
        this.ValidationWarningsList = warnings;
    };
    ActionValidationComponent.prototype.SetConfirmButton = function () {
        if (this.ValidationErrorsList.length == 0) {
            this.IsConfirmButtonVisible = true;
        }
    };
    ActionValidationComponent.prototype.CloseClicked = function () {
        SessionLocator_1.SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    ActionValidationComponent.prototype.ConfirmClicked = function () {
        // SessionLocator.SelectedSession.StartBusyIndicator("Saving...");
        var errors = []; //this.ValidateSending();
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            //this.myCCSWebService.GetSendingValidations(this.entityPM.Id, this.SelectedRecipient, this.isSendingFHLs, this.isSendingCargonaut, this.isSendingDEXX, this.entityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
            //    if (myResponse == null) {
            //        SessionLocator.SelectedSession.StopBusyIndicator();
            //    }
            //    else if (myResponse.HasError) {
            //        this.ValidationErrorsList = myResponse.ErrorsArray;
            //        SessionLocator.SelectedSession.StopBusyIndicator();
            //    }
            //    else {
            //        var myResult: AWBResultClass = myResponse.Result;
            //        this.myValidationResultClass = myResult;
            //        if (myResult.IsValid) {
            //            this.InitializeSendingData();
            //        }
            //        else {
            //            this.ValidationErrorsList = myResult.ErrorsList;
            //            if (myResult.HasStockErrors) {
            //                this.StockErrorMessage = "You are trying to send (" + myResult.SendingCount + ") messages, your remaining stock is (" + myResult.StockRemainingBefore + ") which is insufficient for this operation. Please purchase another messaging stock via the link";
            //                this.StockErrorIsVisible = true;
            //            }
            //            this.SendingResultForeground = this.redForeground;
            //            if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
            //                this.SendingResultMessage = "Error sending FHL(s)";
            //            }
            //            else {
            //                this.SendingResultMessage = "Error sending " + this.MessageType;
            //            }
            //            SessionLocator.SelectedSession.StopBusyIndicator();
            //        }
            //    }
            //});
        }
        else {
            SessionLocator_1.SessionLocator.SelectedSession.StopBusyIndicator();
        }
    };
    return ActionValidationComponent;
}());
ActionValidationComponent = __decorate([
    core_1.Component({
        moduleId: './Shipment/Components/ActionValidationComponent/',
        templateUrl: 'ActionValidationComponent.html',
    }),
    __metadata("design:paramtypes", [])
], ActionValidationComponent);
exports.ActionValidationComponent = ActionValidationComponent;
//# sourceMappingURL=ActionValidationComponent.js.map