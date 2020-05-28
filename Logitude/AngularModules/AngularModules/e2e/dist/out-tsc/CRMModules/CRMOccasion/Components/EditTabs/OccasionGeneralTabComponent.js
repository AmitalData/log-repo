"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var OccasionGeneralTabComponent = /** @class */ (function (_super) {
    __extends(OccasionGeneralTabComponent, _super);
    function OccasionGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "Occasion";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.Listen();
        return _this;
    }
    OccasionGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    OccasionGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    OccasionGeneralTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("OccasionTypeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OccasionTypeId));
    };
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) { if (this.EntityPM.Name != value)
            this.EntityPM.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "StartDateTime", {
        get: function () { return this.EntityPM.StartDateTime; },
        set: function (value) { if (this.EntityPM.StartDateTime != value)
            this.EntityPM.StartDateTime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "EndDateTime", {
        get: function () { return this.EntityPM.EndDateTime; },
        set: function (value) { if (this.EntityPM.EndDateTime != value)
            this.EntityPM.EndDateTime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "Location", {
        get: function () { return this.EntityPM.Location; },
        set: function (value) { if (this.EntityPM.Location != value)
            this.EntityPM.Location = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "Goal", {
        get: function () { return this.EntityPM.Goal; },
        set: function (value) { if (this.EntityPM.Goal != value)
            this.EntityPM.Goal = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "IndustryId", {
        get: function () { return this.EntityPM.IndustryId; },
        set: function (value) { if (this.EntityPM.IndustryId != value)
            this.EntityPM.IndustryId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "OwnerId", {
        get: function () { return this.EntityPM.OwnerId; },
        set: function (value) { if (this.EntityPM.OwnerId != value)
            this.EntityPM.OwnerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "OccasionStatusId", {
        get: function () { return this.EntityPM.OccasionStatusId; },
        set: function (value) { if (this.EntityPM.OccasionStatusId != value)
            this.EntityPM.OccasionStatusId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OccasionGeneralTabComponent.prototype, "OccasionTypeId", {
        get: function () { return this.EntityPM.OccasionTypeId; },
        set: function (value) {
            if (this.EntityPM.OccasionTypeId != value) {
                this.EntityPM.OccasionTypeId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    OccasionGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './OccasionGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OccasionGeneralTabComponent);
    return OccasionGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OccasionGeneralTabComponent = OccasionGeneralTabComponent;
//# sourceMappingURL=OccasionGeneralTabComponent.js.map