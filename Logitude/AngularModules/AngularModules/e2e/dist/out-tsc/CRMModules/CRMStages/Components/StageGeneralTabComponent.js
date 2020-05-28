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
var StagePM_1 = require("../../../CRM/EntityPMs/StagePM");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var StageGeneralTabComponent = /** @class */ (function (_super) {
    __extends(StageGeneralTabComponent, _super);
    function StageGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Stage";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = entityArgs.EntityPM;
        if (_this.entityArgs.EntityPM != null)
            _this.EntityPM = _this.entityArgs.EntityPM;
        else if (_this.CurrentSession.CurrentWindow.WindowArgs.EntityPM != null)
            _this.EntityPM = _this.CurrentSession.CurrentWindow.WindowArgs.EntityPM;
        else
            _this.EntityPM = new StagePM_1.StagePM();
        _this.SetUIProperties();
        return _this;
    }
    StageGeneralTabComponent.prototype.SetUIProperties = function () {
        if (this.EntityPM.Code == "CWN" || this.EntityPM.Code == "CLS") {
            this.UIProperties.SetEnabled("Probability", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("Probability", this.ObjectTableName, true);
        }
    };
    Object.defineProperty(StageGeneralTabComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) { this.EntityPM.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StageGeneralTabComponent.prototype, "Probability", {
        get: function () { return this.EntityPM.Probability; },
        set: function (value) { this.EntityPM.Probability = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StageGeneralTabComponent.prototype, "MaxDays", {
        get: function () { return this.EntityPM.MaxDays; },
        set: function (value) { this.EntityPM.MaxDays = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StageGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) { this.EntityPM.InActive = value; },
        enumerable: true,
        configurable: true
    });
    StageGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'StageGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './StageGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], StageGeneralTabComponent);
    return StageGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.StageGeneralTabComponent = StageGeneralTabComponent;
//# sourceMappingURL=StageGeneralTabComponent.js.map