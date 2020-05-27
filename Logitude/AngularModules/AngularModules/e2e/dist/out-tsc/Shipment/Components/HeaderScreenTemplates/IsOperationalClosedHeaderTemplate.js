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
var IsOperationalClosedHeaderTemplate = /** @class */ (function () {
    function IsOperationalClosedHeaderTemplate() {
    }
    IsOperationalClosedHeaderTemplate.prototype.setVariables = function (entityPM, fieldName) {
        this.entityPM = entityPM;
        this.fieldName = fieldName;
        if (entityPM.IsOperationalClosed == true) {
            this.ImageSRC = "./Images/Icons/IsClosed.png";
        }
        else {
            this.ImageSRC = "./Images/Icons/IsOpened.png";
        }
    };
    IsOperationalClosedHeaderTemplate = __decorate([
        core_1.Component({
            template: "\n    <span style=\"width: 100%; height:100%; position: relative;\">\n        <img [attr.src]=\"ImageSRC\" style=\"top: -5px;\" />\n    </span>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], IsOperationalClosedHeaderTemplate);
    return IsOperationalClosedHeaderTemplate;
}());
exports.IsOperationalClosedHeaderTemplate = IsOperationalClosedHeaderTemplate;
//# sourceMappingURL=IsOperationalClosedHeaderTemplate.js.map