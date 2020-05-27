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
var InvoiceDueDateForegroundPipe = /** @class */ (function () {
    function InvoiceDueDateForegroundPipe() {
    }
    InvoiceDueDateForegroundPipe.prototype.transform = function (dateField, isClosed) {
        var myResult = Tools_1.FontTool.Black;
        if (!Tools_1.AppTool.IsNullOrEmpty(dateField) && !isClosed) {
            var myDateTicks = Tools_1.DateTool.GetDateParts(dateField).DateTicks;
            var todayDateTicks = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateTicks;
            if (myDateTicks < todayDateTicks) {
                myResult = Tools_1.FontTool.Red;
            }
            //var todayDate: Date = DateTool.GetCurrentDateAsUtc();
            ////todayDate = DateTool.TruncateTime(todayDate);
            //var entityDate: Date = DateTool.GetDateParts(dateField).DateObject;
            ////entityDate = DateTool.TruncateTime(entityDate);
            //if (entityDate.valueOf() < todayDate.valueOf()) {
            //    myResult = FontTool.Red;
            //}
        }
        return myResult;
    };
    InvoiceDueDateForegroundPipe = __decorate([
        core_1.Pipe({ name: 'InvoiceDueDateForegroundPipe' })
    ], InvoiceDueDateForegroundPipe);
    return InvoiceDueDateForegroundPipe;
}());
exports.InvoiceDueDateForegroundPipe = InvoiceDueDateForegroundPipe;
//# sourceMappingURL=InvoiceDueDateForegroundPipe.js.map