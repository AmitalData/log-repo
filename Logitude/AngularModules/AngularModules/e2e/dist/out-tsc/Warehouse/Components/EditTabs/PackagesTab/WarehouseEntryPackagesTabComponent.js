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
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var WarehouseEntryPackagesTabComponent = /** @class */ (function () {
    function WarehouseEntryPackagesTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.DataContext = this;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
    }
    WarehouseEntryPackagesTabComponent.prototype.ngOnInit = function () {
        this.warehouseEntryPM = this.entityArgs.EntityPM;
        this.Listen();
        if (this.warehouseEntryPM) {
            this.RunComponent();
        }
    };
    WarehouseEntryPackagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.warehouseEntryPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        if (_this.WarehouseEntryPackagesDetailsComponent) {
                            var windowArgs = { WarehouseEntryPM: _this.warehouseEntryPM, ViewModelTrigger: _this, IsFromShipment: true, IsEditMode: true };
                            _this.WarehouseEntryPackagesDetailsComponent.ReloadComponent(windowArgs);
                        }
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.warehouseEntryPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    WarehouseEntryPackagesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    WarehouseEntryPackagesTabComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.LoadChildComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    WarehouseEntryPackagesTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    WarehouseEntryPackagesTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        var warehouseEntryPackagesDetailsComponenttLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == "WEPD"; })[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseEntryPackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                .then(function (cmpRef) {
                _this.WarehouseEntryPackagesDetailsComponent = cmpRef.instance;
                var windowArgs = { WarehouseEntryPM: _this.warehouseEntryPM, ViewModelTrigger: _this, IsEditMode: true, ShowAddPackageButton: true, ShowPackageSummary: true };
                cmpRef.instance.SetWindowArgs(windowArgs);
                //   ShowAddPackageButton
            });
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], WarehouseEntryPackagesTabComponent.prototype, "AllLocations", void 0);
    WarehouseEntryPackagesTabComponent = __decorate([
        core_1.Component({
            selector: 'WarehouseEntryPackagesTabComponent',
            moduleId: module.id,
            templateUrl: './WarehouseEntryPackagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], WarehouseEntryPackagesTabComponent);
    return WarehouseEntryPackagesTabComponent;
}());
exports.WarehouseEntryPackagesTabComponent = WarehouseEntryPackagesTabComponent;
//# sourceMappingURL=WarehouseEntryPackagesTabComponent.js.map