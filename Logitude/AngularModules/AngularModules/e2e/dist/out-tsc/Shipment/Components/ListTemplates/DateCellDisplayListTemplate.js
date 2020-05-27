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
var DateCellDisplayListTemplate = /** @class */ (function () {
    function DateCellDisplayListTemplate(CD) {
        this.CD = CD;
    }
    DateCellDisplayListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var pmDate = new Date(rowData[fieldName]);
        this.dateValue = pmDate;
        //console.log("pmDate", pmDate);
        //var nowDate = new Date();
        ////console.log("nowDate", nowDate);
        ////var valueDate = pmDate.setDate(nowDate.getDate());
        ////console.log("valueDate = pmDate.getDate", valueDate);
        //if (pmDate.getDate() == nowDate.getDate() && pmDate.getMonth() == nowDate.getMonth() && pmDate.getFullYear() == nowDate.getFullYear()) {
        //    //return "Today";
        //    this.dateValue = "Today";
        //}
        //else if (pmDate.getDate() == (nowDate.getDate() - 1) && pmDate.getMonth() == nowDate.getMonth() && pmDate.getFullYear() == nowDate.getFullYear()) {
        //    //return "Yesterday";
        //    this.dateValue = "Yesterday";
        //}
        //else if (pmDate.getDate() == (nowDate.getDate() + 1) && pmDate.getMonth() == nowDate.getMonth() && pmDate.getFullYear() == nowDate.getFullYear()) {
        //    //return "Tomorrow";
        //    this.dateValue = "Tomorrow";
        //}
        //else {
        //    this.dateValue = pmDate.getDate() + "/" + pmDate.getMonth() + "/" + pmDate.getFullYear();
        //}
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    DateCellDisplayListTemplate = __decorate([
        core_1.Component({
            template: "\n                <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;color:green;\">{{dateValue | LogBoxStatusDatePipe}}</div>\n            ",
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DateCellDisplayListTemplate);
    return DateCellDisplayListTemplate;
}());
exports.DateCellDisplayListTemplate = DateCellDisplayListTemplate;
//# sourceMappingURL=DateCellDisplayListTemplate.js.map