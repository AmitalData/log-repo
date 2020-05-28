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
var DeclarationQueryListTemplate = /** @class */ (function () {
    function DeclarationQueryListTemplate(cd) {
        this.cd = cd;
    }
    DeclarationQueryListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.cd.detectChanges();
    };
    DeclarationQueryListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationQueryListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DeclarationQueryListTemplate);
    return DeclarationQueryListTemplate;
}());
exports.DeclarationQueryListTemplate = DeclarationQueryListTemplate;
//# sourceMappingURL=DeclarationQueryListTemplate.js.map