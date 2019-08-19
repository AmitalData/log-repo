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
var CustomReferenceListTemplate = /** @class */ (function () {
    function CustomReferenceListTemplate(CD) {
        this.CD = CD;
        this.MyLabel = "";
    }
    CustomReferenceListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.rowData['CustomerReference1'])) {
            this.MyLabel += this.rowData['CustomerReference1'];
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.rowData['CustomerReference1']) && !Tools_1.AppTool.IsNullOrEmpty(this.rowData['CustomerReference2'])) {
            this.MyLabel += this.rowData['CustomerReference2'];
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.rowData['CustomerReference1']) && !Tools_1.AppTool.IsNullOrEmpty(this.rowData['CustomerReference2'])) {
            this.MyLabel += ' / ' + this.rowData['CustomerReference2'];
        }
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    CustomReferenceListTemplate = __decorate([
        core_1.Component({
            template: "<div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;\">\n                      {{MyLabel}}\n               </div>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], CustomReferenceListTemplate);
    return CustomReferenceListTemplate;
}());
exports.CustomReferenceListTemplate = CustomReferenceListTemplate;
//# sourceMappingURL=CustomReferenceListTemplate.js.map