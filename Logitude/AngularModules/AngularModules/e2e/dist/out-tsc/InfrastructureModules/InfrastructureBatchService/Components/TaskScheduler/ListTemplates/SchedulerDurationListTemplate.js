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
var SchedulerDurationListTemplate = /** @class */ (function () {
    function SchedulerDurationListTemplate(CD) {
        this.CD = CD;
    }
    SchedulerDurationListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var startDate = new Date(rowData["StartDateTime"]);
        var endDate = new Date(rowData["EndDateTime"]);
        var seconds = ((endDate.getTime() - startDate.getTime()) / 1000);
        if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
            this.dateValue = seconds;
        }
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    SchedulerDurationListTemplate = __decorate([
        core_1.Component({
            template: "\n                <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:green;\">{{dateValue}}</div>\n            ",
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SchedulerDurationListTemplate);
    return SchedulerDurationListTemplate;
}());
exports.SchedulerDurationListTemplate = SchedulerDurationListTemplate;
//# sourceMappingURL=SchedulerDurationListTemplate.js.map