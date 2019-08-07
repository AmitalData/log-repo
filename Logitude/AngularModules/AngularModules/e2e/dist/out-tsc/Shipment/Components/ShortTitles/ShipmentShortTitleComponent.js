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
var Tools_1 = require("../../../Infrastructure/Tools");
var ShipmentShortTitleComponent = /** @class */ (function () {
    function ShipmentShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.LoadCompletedEvent = null;
        this.IsRankVisible = false;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
        this.Listen();
    }
    ShipmentShortTitleComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.BuildComponent();
                }
            });
        }
    };
    ShipmentShortTitleComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    //public IsCancelled: boolean = false;
    ShipmentShortTitleComponent.prototype.BuildComponent = function () {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.Background = "rgba(35, 172, 214, 0.15)";
            this.PartnerName = this.EntityPM.AgentName;
        }
        else {
            this.Background = "rgba(235, 235, 235, 1)";
            this.PartnerName = this.EntityPM.CustomerName;
            this.RankName = this.EntityPM.CustomerRankName;
            if (this.RankName != null) {
                switch (this.RankName.toLowerCase()) {
                    case "silver": {
                        this.RankCode = "1";
                        this.RankSource1 = "./Images/Icons/StarOrange.png";
                        this.RankSource2 = "./Images/Icons/StarGray.png";
                        this.RankSource3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "gold": {
                        this.RankCode = "2";
                        this.RankSource1 = "./Images/Icons/StarOrange.png";
                        this.RankSource2 = "./Images/Icons/StarOrange.png";
                        this.RankSource3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "platinum": {
                        this.RankCode = "3";
                        this.RankSource1 = "./Images/Icons/StarOrange.png";
                        this.RankSource2 = "./Images/Icons/StarOrange.png";
                        this.RankSource3 = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        this.RankCode = "0";
                        this.RankSource1 = "./Images/Icons/StarGray.png";
                        this.RankSource2 = "./Images/Icons/StarGray.png";
                        this.RankSource3 = "./Images/Icons/StarGray.png";
                    }
                }
                this.IsRankVisible = true;
            }
        }
        this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
        this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";
    };
    Object.defineProperty(ShipmentShortTitleComponent.prototype, "IsCancelled", {
        get: function () { return this.EntityPM.IsCancelled; },
        enumerable: true,
        configurable: true
    });
    ShipmentShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "ShipmentShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ShipmentShortTitleComponent);
    return ShipmentShortTitleComponent;
}());
exports.ShipmentShortTitleComponent = ShipmentShortTitleComponent;
//# sourceMappingURL=ShipmentShortTitleComponent.js.map