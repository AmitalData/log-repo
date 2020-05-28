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
var RoutingCellDisplayListTemplate = /** @class */ (function () {
    function RoutingCellDisplayListTemplate() {
    }
    RoutingCellDisplayListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (rowData[fieldName]) {
            var routing = rowData[fieldName];
            this.value = routing.replace(",", ">");
        }
        //else {
        //}
    };
    RoutingCellDisplayListTemplate = __decorate([
        core_1.Component({
            template: '<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;" *ngIf="value">{{value}}</div>'
        }),
        __metadata("design:paramtypes", [])
    ], RoutingCellDisplayListTemplate);
    return RoutingCellDisplayListTemplate;
}());
exports.RoutingCellDisplayListTemplate = RoutingCellDisplayListTemplate;
//# sourceMappingURL=RoutingCellDisplayListTemplate.js.map