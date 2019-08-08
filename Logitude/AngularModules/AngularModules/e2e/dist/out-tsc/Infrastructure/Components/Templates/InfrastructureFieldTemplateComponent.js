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
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var InfrastructureFieldTemplateComponent = /** @class */ (function () {
    function InfrastructureFieldTemplateComponent(cd) {
        this.cd = cd;
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.SpotlightDataTemplate = null;
        this.IsSpotLightTemplate = false;
        this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    InfrastructureFieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
            if (this.cd) {
                var isDestroyed = this.cd['destroyed'];
                if (!isDestroyed) {
                    this.cd.detectChanges();
                }
            }
        }
    };
    InfrastructureFieldTemplateComponent.prototype.GetStatusColor = function () {
        if (this.Entity.StatusCode == "D") { // D- Done
            return 'green';
        }
        else if (this.Entity.StatusCode == "F") { // F- Failed
            return 'red';
        }
        else if (this.Entity.StatusCode == "I") { // I- In Progress
            return 'blue';
        }
        else {
            return 'black';
        }
    };
    InfrastructureFieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './InfrastructureFieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], InfrastructureFieldTemplateComponent);
    return InfrastructureFieldTemplateComponent;
}());
exports.InfrastructureFieldTemplateComponent = InfrastructureFieldTemplateComponent;
//# sourceMappingURL=InfrastructureFieldTemplateComponent.js.map