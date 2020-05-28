"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var DateAgeHelper_1 = require("../Utilities/DateAgeHelper");
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var Tools_1 = require("../Tools");
var ExchangeRateDatePipe = /** @class */ (function () {
    function ExchangeRateDatePipe() {
    }
    ExchangeRateDatePipe.prototype.transform = function (value) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var helper = new DateAgeHelper_1.DateAgeHelper(value);
            if (helper) {
                if (helper.Years > 0) {
                    myResult = helper.Years == 1 ? helper.Years + " year ago" : helper.Years + " years ago";
                }
                else if (helper.Months > 0) {
                    myResult = helper.Months == 1 ? helper.Months + " month ago" : helper.Months + " months ago";
                }
                else {
                    if (helper.Days == 0) {
                        myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Today");
                    }
                    else if (helper.Days == 1) {
                        myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Yesterday");
                    }
                    else {
                        myResult = helper.Days + " days ago";
                    }
                }
            }
        }
        return myResult;
    };
    ExchangeRateDatePipe = __decorate([
        core_1.Pipe({ name: 'ExchangeRateDatePipe' })
    ], ExchangeRateDatePipe);
    return ExchangeRateDatePipe;
}());
exports.ExchangeRateDatePipe = ExchangeRateDatePipe;
//# sourceMappingURL=ExchangeRateDatePipe.js.map