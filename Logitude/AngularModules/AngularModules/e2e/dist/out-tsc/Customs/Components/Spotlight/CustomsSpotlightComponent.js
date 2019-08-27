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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var PhysicalCheckPMService_1 = require("../../Services/StandardPMs/PhysicalCheckPMService");
var CustomsSpotlightComponent = /** @class */ (function () {
    function CustomsSpotlightComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.showBusyIndicator = false;
    }
    Object.defineProperty(CustomsSpotlightComponent.prototype, "ShowBusyIndicator", {
        get: function () { return this.showBusyIndicator; },
        set: function (value) {
            if (this.showBusyIndicator != value) {
                this.showBusyIndicator = value;
                this.CurrentSession.FireEvent("SpotLightDetectChanges");
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsSpotlightComponent.prototype.Run = function (entityId) {
        this.EntityId = entityId;
        this.LoadPhysicalCheckPM();
    };
    CustomsSpotlightComponent.prototype.LoadPhysicalCheckPM = function () {
        var _this = this;
        this.ShowBusyIndicator = true;
        var physicalCheckPMService = new PhysicalCheckPMService_1.PhysicalCheckPMService();
        physicalCheckPMService.get(this.EntityId).subscribe(function (response) {
            var result = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.MyEntityArg = new EntityArgs_1.EntityArgs();
                _this.MyEntityArg.EntityPM = result;
                _this.MyEntityArg.ObjectTableName = "Customs.PhysicalCheck";
            }
            _this.ShowBusyIndicator = false;
        });
    };
    CustomsSpotlightComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsSpotlightComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsSpotlightComponent);
    return CustomsSpotlightComponent;
}());
exports.CustomsSpotlightComponent = CustomsSpotlightComponent;
//# sourceMappingURL=CustomsSpotlightComponent.js.map