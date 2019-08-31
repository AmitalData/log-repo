"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../Tools");
var LogBoxStatusForegroundPipe = /** @class */ (function () {
    function LogBoxStatusForegroundPipe() {
    }
    LogBoxStatusForegroundPipe.prototype.transform = function (value, Parameter) {
        if (Parameter === void 0) { Parameter = null; }
        var myResult = "#282E30";
        if (Parameter == 'CA') {
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                myResult = this.ApplyCAPipe(value);
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                myResult = this.ApplyPipe(value);
            }
        }
        return myResult;
    };
    LogBoxStatusForegroundPipe.prototype.ApplyPipe = function (value) {
        var color = "#282E30";
        switch (value) {
            case "Draft":
                {
                    color = "Orange";
                    break;
                }
            case "Created":
            case "Approval Canceled":
            case "Not Sent":
            case "Partially Sent":
                {
                    // Black
                    color = "#282E30";
                    break;
                }
            case "Void":
                {
                    color = "#6E7172";
                    break;
                }
            case "Booking Request Rejected":
            case "Cancellation Request Rejected":
            case "Declined":
            case "Error":
                {
                    // Red
                    color = "#E53030";
                    break;
                }
            case "Air waybill":
            case "Received by Airline":
            case "Waiting for Confirmation":
            case "Accepted":
            case "Hybriding":
            case "Booking Request":
            case "Used":
                {
                    // Green
                    color = "#009161";
                    break;
                }
            case "Confirmed":
            case "Booking Confirmed":
            case "Departed":
            case "Arrived":
            case "Sent":
            case "In Progress":
                {
                    color = "#27AAE1";
                    break;
                }
            case "Printed":
            case "Pick Up":
            case "On Hand":
                {
                    color = "#F37021";
                    break;
                }
            case "Paid":
            case "Cleared":
            case "Delivery":
            case "Delivered":
                {
                    color = "#2BB673";
                    break;
                }
            case "Approved":
            case "Unpaid":
            case "Approved By Customer":
                {
                    color = "#8DC63F";
                    break;
                }
            case "Auto Credit":
            case "Auto Credited":
            case "Sent To Customer":
                {
                    color = "Orange";
                    break;
                }
            case "Viewed":
                {
                    color = "#00D377";
                    break;
                }
            case "Not Connected":
                {
                    color = "Gray";
                    break;
                }
            case "Connected":
                {
                    color = "Blue";
                    break;
                }
            case "InActive":
                {
                    color = "Red";
                    break;
                }
            case "Waiting":
            case "Waiting For Approval":
                {
                    color = "Orange";
                    break;
                }
            //case "Accepted":
            //    {
            //        color = "Green";
            //        break;
            //    }
        }
        return color;
    };
    LogBoxStatusForegroundPipe.prototype.ApplyCAPipe = function (value) {
        var color = "#282E30";
        switch (value) {
            case "Waiting":
            case "In Progress":
                {
                    color = "Orange";
                    break;
                }
            case "InActive":
                {
                    // Red
                    color = "#E53030";
                    break;
                }
            case "Approved":
                {
                    // Green
                    color = "#009161";
                    break;
                }
        }
        return color;
    };
    LogBoxStatusForegroundPipe = __decorate([
        core_1.Pipe({ name: 'LogBoxStatusForegroundPipe' })
    ], LogBoxStatusForegroundPipe);
    return LogBoxStatusForegroundPipe;
}());
exports.LogBoxStatusForegroundPipe = LogBoxStatusForegroundPipe;
//# sourceMappingURL=LogBoxStatusForegroundPipe.js.map