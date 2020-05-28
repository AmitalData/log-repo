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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ShipmentPMService_1 = require("../../Services/StandardPMs/ShipmentPMService");
var ReactivateShipmentComponent = /** @class */ (function () {
    function ReactivateShipmentComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ReactivateShipmentComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
    };
    Object.defineProperty(ReactivateShipmentComponent.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (newValue) {
            this.notes = newValue;
        },
        enumerable: true,
        configurable: true
    });
    ReactivateShipmentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ReactivateShipmentComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.EntityPM.EventNote = this.Notes;
        this.EntityPM.IsCancelled = false;
        // save
        var myService = new ShipmentPMService_1.ShipmentPMService();
        myService.update(this.EntityPM).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            _this.CurrentSession.CloseCurrentWindowEmit('OK');
        });
    };
    ReactivateShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReactivateShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ReactivateShipmentComponent);
    return ReactivateShipmentComponent;
}());
exports.ReactivateShipmentComponent = ReactivateShipmentComponent;
//# sourceMappingURL=ReactivateShipmentComponent.js.map