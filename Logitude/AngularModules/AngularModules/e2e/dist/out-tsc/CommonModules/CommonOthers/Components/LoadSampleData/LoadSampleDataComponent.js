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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LoadSampleDataComponent = /** @class */ (function () {
    function LoadSampleDataComponent() {
        this.ShipmentNumbersList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ShipmentNumbersList.push(100);
        this.ShipmentNumbersList.push(1000);
        this.ShipmentNumbersList.push(5000);
        this.ShipmentNumbersList.push(10000);
        this.ShipmentNumbersList.push(50000);
        this.myService = new GlobalDomainService_1.GlobalDomainService();
    }
    Object.defineProperty(LoadSampleDataComponent.prototype, "SelectedShipmentsNumber", {
        get: function () { return this.selectedShipmentsNumber; },
        set: function (value) {
            if (this.selectedShipmentsNumber != value) {
                this.selectedShipmentsNumber = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    LoadSampleDataComponent.prototype.ButtonClicked = function (myCommand) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCommand)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.myService.UpdateTenantZeroService(myCommand).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                var messageWindow = new MessageWindow_1.MessageWindow();
                if (myResponse.HasError) {
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
                else {
                    messageWindow.Show("Updating tenant 0 an email will be sent as soon as update completes.");
                }
            });
        }
    };
    LoadSampleDataComponent.prototype.Validate = function () {
        var errorsMessage;
        if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedShipmentsNumber)) {
            errorsMessage = "Please select number of shipments";
        }
        this.ErrorsMessage = errorsMessage;
    };
    LoadSampleDataComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    LoadSampleDataComponent.prototype.OkButtonClicked = function () {
        this.Validate();
    };
    LoadSampleDataComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LoadSampleDataComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LoadSampleDataComponent);
    return LoadSampleDataComponent;
}());
exports.LoadSampleDataComponent = LoadSampleDataComponent;
//# sourceMappingURL=LoadSampleDataComponent.js.map