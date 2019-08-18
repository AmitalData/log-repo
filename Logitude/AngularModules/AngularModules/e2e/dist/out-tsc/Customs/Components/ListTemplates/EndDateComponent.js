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
var Tools_1 = require("../../../Infrastructure/Tools");
var EndDateComponent = /** @class */ (function () {
    function EndDateComponent(CD) {
        this.CD = CD;
    }
    EndDateComponent.prototype.setVariables = function (rowData, fieldName, additionalData) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (rowData.EndDate) {
            var valueDate = new Date(rowData.EndDate.valueOf()).valueOf();
        }
        var today = Tools_1.DateTool.GetCurrentDateAsUtc().valueOf();
        if (valueDate != null && valueDate < today) {
            this.fontcolor = "red";
        }
        else {
            this.fontcolor = "limegreen";
        }
        this.CD.detectChanges();
    };
    EndDateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EndDateComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], EndDateComponent);
    return EndDateComponent;
}());
exports.EndDateComponent = EndDateComponent;
//# sourceMappingURL=EndDateComponent.js.map