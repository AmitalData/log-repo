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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var INTRAWebService_1 = require("../../../../Shipment/Services/INTRAWebService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var SimulatorBookingComponent = /** @class */ (function () {
    function SimulatorBookingComponent() {
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SimulatorBookingComponent.prototype.SetWindowArgs = function (args) {
        this.ShipmentId = args;
    };
    SimulatorBookingComponent.prototype.CancelClicked = function () {
        this.Close();
    };
    SimulatorBookingComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SimulatorBookingComponent.prototype.SimulateClicked = function () {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending e-Booking...");
            var myService = new INTRAWebService_1.INTRAWebService();
            myService.SendEBooking(this.ShipmentId).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult.Success) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("Simulated Successfully");
                    }
                    else {
                        _this.ValidationErrorsList = myResult.Errors;
                    }
                }
            });
        }
    };
    SimulatorBookingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SimulatorBookingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SimulatorBookingComponent);
    return SimulatorBookingComponent;
}());
exports.SimulatorBookingComponent = SimulatorBookingComponent;
//# sourceMappingURL=SimulatorBookingComponent.js.map