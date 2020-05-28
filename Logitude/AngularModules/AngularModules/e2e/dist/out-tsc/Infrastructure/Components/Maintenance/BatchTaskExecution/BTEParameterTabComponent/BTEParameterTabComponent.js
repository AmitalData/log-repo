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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObjectsLocator_1 = require("../../../../../Infrastructure/Locators/ObjectsLocator");
var EntityResourceService_1 = require("../../../../Services/EntityResourceService");
var BTEParameterTabComponent = /** @class */ (function (_super) {
    __extends(BTEParameterTabComponent, _super);
    function BTEParameterTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = 'BatchTaskExecution';
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.isReady = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this._entityResourceService.getEntityResourceByTableName(_this.ObjectTableName, 0).subscribe(function (response) {
            _this.isReady = true;
        });
        // Set Entity
        _this.EntityPM = entityArgs.EntityPM;
        _this.SetUIProperties();
        _this.Listen();
        return _this;
    }
    BTEParameterTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    Object.defineProperty(BTEParameterTabComponent.prototype, "PrametersXml", {
        //#region Properties
        get: function () { return this.EntityPM.PrametersXml; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    BTEParameterTabComponent.prototype.SetUIProperties = function () {
        //if (!this.EntityPM.TypeCode) {
        this.UIProperties.SetEnabled("PrametersXml", this.ObjectTableName, false);
        //}
    };
    BTEParameterTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BTEParameterTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BTEParameterTabComponent);
    return BTEParameterTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BTEParameterTabComponent = BTEParameterTabComponent;
//# sourceMappingURL=BTEParameterTabComponent.js.map