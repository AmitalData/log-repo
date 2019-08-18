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
var Tools_1 = require("../../Tools");
var Tools_2 = require("../../../Infrastructure/Tools");
var FieldTemplateComponent = /** @class */ (function () {
    function FieldTemplateComponent() {
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.SpotlightDataTemplate = null;
        this.IsSpotLightTemplate = false;
        this.ImageSrc = null;
        // Rank
        this.RankSource1 = null;
        this.RankSource2 = null;
        this.RankSource3 = null;
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
            if (this.ObjectTableName == "Ticket") {
                if (this.FieldName == "RankCode") {
                    this.SetRanksSource();
                }
                else if (this.FieldName == "LastCompletedActivityTypeCode") {
                    if (!Tools_2.AppTool.IsNullOrEmpty(this.Entity.LastCompletedActivityTypeCode)) {
                        this.ImageSrc = Tools_1.CRMTool.GetActivityImageSrc(this.Entity.LastCompletedActivityTypeCode);
                    }
                }
                else if (this.FieldName == "NextActivityTypeCode") {
                    if (!Tools_2.AppTool.IsNullOrEmpty(this.Entity.NextActivityTypeCode)) {
                        this.ImageSrc = Tools_1.CRMTool.GetActivityImageSrc(this.Entity.NextActivityTypeCode);
                    }
                }
            }
        }
    };
    FieldTemplateComponent.prototype.SetRanksSource = function () {
        var RankCode = this.Entity['RankCode'];
        switch (RankCode) {
            case "1": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }
            case "2": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }
            case "3": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarOrange.png";
                break;
            }
            default: {
                this.RankSource1 = "./Images/Icons/StarGray.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
            }
        }
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