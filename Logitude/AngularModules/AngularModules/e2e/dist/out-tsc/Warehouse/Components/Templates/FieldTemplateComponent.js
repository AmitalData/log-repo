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
var FieldTemplateComponent = /** @class */ (function () {
    function FieldTemplateComponent() {
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.IsHeaderScreenTemplate = false;
        this.BackgroudColor = "";
        this.WarehouseDateType = "";
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];
                if (this.ObjectTableName == "WarehouseEntry" && this.FieldName == "ActualEntryDate") {
                    this.BackgroudColor = this.transform(this.FieldValue, this.Entity['ExpectedEntryDate']);
                }
                //if (this.ObjectTableName == "WarehouseRelease" && this.FieldName == "ActualReleaseDate") {
                //    this.BackgroudColor = this.transform(this.FieldValue, this.Entity['ExpectedReleaseDate']);
                //}
                if (this.ObjectTableName == "WarehouseRelease" && this.FieldName == "ReleaseDate") {
                    this.BackgroudColor = this.transform(this.Entity['ActualReleaseDate'], this.Entity['ExpectedReleaseDate']);
                }
            }
        }
    };
    FieldTemplateComponent.prototype.transform = function (actual, expected) {
        var myResult = Tools_1.FontTool.Black;
        if (actual != null) {
            myResult = Tools_1.FontTool.Green;
            this.WarehouseDateType = " (actual)";
        }
        else if (expected != null) {
            myResult = Tools_1.FontTool.Red;
            this.WarehouseDateType = " (expected)";
            var name = this.FieldName.replace("Actual", "Expected");
            this.FieldValue = this.Entity[name];
        }
        return myResult;
    };
    FieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FieldTemplateComponent);
    return FieldTemplateComponent;
}());
exports.FieldTemplateComponent = FieldTemplateComponent;
//# sourceMappingURL=FieldTemplateComponent.js.map