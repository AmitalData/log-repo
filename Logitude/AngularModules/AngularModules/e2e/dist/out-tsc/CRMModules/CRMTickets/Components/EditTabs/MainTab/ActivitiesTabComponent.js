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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ActivitiesTabComponent = /** @class */ (function (_super) {
    __extends(ActivitiesTabComponent, _super);
    function ActivitiesTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ActivitiesList = [];
        _this.ActivitiesList = [];
        return _this;
    }
    ActivitiesTabComponent.prototype.InitTab = function (trigger) {
        this.Trigger = trigger;
        this.EntityPM = this.Trigger.EntityPM;
        this.ObjectTableName = this.Trigger.ObjectTableName;
        this.ActivitiesList = this.Trigger.ActivitiesList;
    };
    ActivitiesTabComponent.prototype.Updated = function (arg) {
        if (arg) {
            this.Trigger.LoadActivities();
        }
    };
    ActivitiesTabComponent = __decorate([
        core_1.Component({
            selector: 'ActivitiesTabComponent',
            moduleId: module.id,
            templateUrl: './ActivitiesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ActivitiesTabComponent);
    return ActivitiesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ActivitiesTabComponent = ActivitiesTabComponent;
//# sourceMappingURL=ActivitiesTabComponent.js.map