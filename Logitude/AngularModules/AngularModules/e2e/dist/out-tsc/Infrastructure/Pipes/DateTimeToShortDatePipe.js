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
var SessionLocator_1 = require("../Utilities/SessionLocator");
var DateTimeToShortDatePipe = /** @class */ (function () {
    function DateTimeToShortDatePipe() {
    }
    DateTimeToShortDatePipe.prototype.transform = function (value) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var myDate = Tools_1.DateTool.GetDateFromDate(value, true);
            var myCurrentDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
            var plus = Tools_1.DateTool.GetDateByDay(1);
            var minus = Tools_1.DateTool.GetDateByDay(-1);
            var yyyy = myDate.getFullYear().toString();
            var mm = (myDate.getMonth() + 1).toString();
            var dd = myDate.getDate().toString();
            if (dd.length == 1) {
                dd = "0" + dd;
            }
            if (mm.length == 1) {
                mm = "0" + mm;
            }
            var myResult = "";
            var tenantFormat = SessionLocator_1.SessionLocator.TenantPM.DateTimeFormat;
            var compare = tenantFormat.localeCompare("MM\/dd\/yyyy");
            if (compare != -1) {
                myResult = mm + "/" + dd + "/" + yyyy;
            }
            else {
                myResult = dd + "/" + mm + "/" + yyyy;
            }
        }
        return myResult;
    };
    DateTimeToShortDatePipe = __decorate([
        core_1.Pipe({ name: 'DateTimeToShortDatePipe' })
    ], DateTimeToShortDatePipe);
    return DateTimeToShortDatePipe;
}());
exports.DateTimeToShortDatePipe = DateTimeToShortDatePipe;
//# sourceMappingURL=DateTimeToShortDatePipe.js.map