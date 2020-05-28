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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TariffDetailsTabComponent = /** @class */ (function () {
    function TariffDetailsTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.isComponentInited = false;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    TariffDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.CurrentSession.FireEvent("LoadEventTabData");
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.RunComponent();
                }
            });
        }
    };
    TariffDetailsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    TariffDetailsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe(function (res1) {
            _this.isComponentInited = true;
            _this.RunComponent();
        });
    };
    TariffDetailsTabComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.isComponentInited) {
            this.ClearLocation();
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./TariffModule/Components/EditTabs/Tariff/TariffTabsContentComponent", this.location)
                .then(function (cmpRef) {
                cmpRef.instance.Run({ EntityPM: _this.EntityPM, });
            });
        }
    };
    TariffDetailsTabComponent.prototype.ClearLocation = function () {
        if (this.location) {
            this.location.clear();
        }
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], TariffDetailsTabComponent.prototype, "location", void 0);
    TariffDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TariffDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], TariffDetailsTabComponent);
    return TariffDetailsTabComponent;
}());
exports.TariffDetailsTabComponent = TariffDetailsTabComponent;
//# sourceMappingURL=TariffDetailsTabComponent.js.map