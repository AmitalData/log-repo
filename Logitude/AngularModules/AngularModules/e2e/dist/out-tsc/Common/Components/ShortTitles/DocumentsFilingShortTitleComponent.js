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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var DocumentsFilingShortTitleComponent = /** @class */ (function () {
    function DocumentsFilingShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            //this.BuildComponent();
        }
    }
    Object.defineProperty(DocumentsFilingShortTitleComponent.prototype, "Code", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.Code;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingShortTitleComponent.prototype, "DocumentTypeName", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.DocumentTypeName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingShortTitleComponent.prototype, "IsDigitallySigned", {
        get: function () {
            var myResult = false;
            // silver to lower
            if (this.EntityPM != null) {
                myResult = this.EntityPM.IsDigitallySigned;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingShortTitleComponent.prototype, "FileExtension", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.FileExtension;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    DocumentsFilingShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./DocumentsFilingShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], DocumentsFilingShortTitleComponent);
    return DocumentsFilingShortTitleComponent;
}());
exports.DocumentsFilingShortTitleComponent = DocumentsFilingShortTitleComponent;
//# sourceMappingURL=DocumentsFilingShortTitleComponent.js.map