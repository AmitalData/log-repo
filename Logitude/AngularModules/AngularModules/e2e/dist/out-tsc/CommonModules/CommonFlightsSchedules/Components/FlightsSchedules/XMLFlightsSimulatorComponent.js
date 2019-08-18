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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FlightsSchedulesRequestPMService_1 = require("../../../../Booking/Services/StandardPMs/FlightsSchedulesRequestPMService");
var FVRWebService_1 = require("../../../../Infrastructure/Services/WebServices/FVRWebService");
var XMLFlightsSimulatorComponent = /** @class */ (function () {
    function XMLFlightsSimulatorComponent() {
        this.BookingId = null;
        this.ShipmentId = null;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsFNA = false;
        this.RequestId = null;
        this.XML_Text = null;
        this.useRequestId = false;
    }
    XMLFlightsSimulatorComponent.prototype.SetWindowArgs = function (args) {
        this.BookingId = args['BookingId'];
        this.ShipmentId = args['ShipmentId'];
        this.FatherComponent = args['FatherComponent'];
    };
    Object.defineProperty(XMLFlightsSimulatorComponent.prototype, "UseRequestId", {
        get: function () { return this.useRequestId; },
        set: function (newValue) {
            if (this.useRequestId != newValue) {
                this.useRequestId = newValue;
                if (newValue) {
                    this.XML_Text = null;
                }
                else {
                    this.RequestId = null;
                }
                this.IsFNA = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    XMLFlightsSimulatorComponent.prototype.CancelClicked = function () {
        this.Close();
    };
    XMLFlightsSimulatorComponent.prototype.OkClicked = function () {
        var errors = [];
        if (this.UseRequestId) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.RequestId)) {
                errors.push("Please fill your Request Id");
            }
        }
        else {
            if (Tools_1.AppTool.IsNullOrEmpty(this.XML_Text)) {
                errors.push("Please fill your XML body");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Simulating...");
            if (this.UseRequestId) {
                this.SimulateRequest();
            }
            else {
                this.SimulateXML();
            }
        }
    };
    XMLFlightsSimulatorComponent.prototype.SimulateRequest = function () {
        var _this = this;
        if (this.entityPMService == null) {
            this.entityPMService = new FlightsSchedulesRequestPMService_1.FlightsSchedulesRequestPMService();
        }
        this.entityPMService.get(this.RequestId).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var entityPM = myResponse.Result;
                if (entityPM == null) {
                    var errors = [];
                    errors.push("This Request not exists");
                    _this.ValidationErrorsList = errors;
                }
                else {
                    var myResult = new FVRWebService_1.FVASimulatorResult();
                    myResult.ETD = entityPM.ETD;
                    myResult.AirlineId = entityPM.AirlineId;
                    myResult.FromPortId = entityPM.FromPortId;
                    myResult.ToPortId = entityPM.ToPortId;
                    myResult.RequestId = entityPM.Id;
                    _this.FatherComponent.UpgradeFromXML(myResult);
                    _this.Close();
                }
            }
        });
    };
    XMLFlightsSimulatorComponent.prototype.SimulateXML = function () {
        var _this = this;
        if (this.myFVRWebService == null) {
            this.myFVRWebService = new FVRWebService_1.FVRWebService();
        }
        this.myFVRWebService.SimulateXML(this.XML_Text, this.ShipmentId, this.BookingId, this.IsFNA).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult.IsValid) {
                        _this.FatherComponent.UpgradeFromXML(myResult);
                        _this.Close();
                    }
                    else {
                        if (!myResult.IsValidXML) {
                            _this.ValidationErrorsList = [];
                            _this.ValidationErrorsList.push("Invalid FVA XML");
                        }
                        else if (!myResult.IsValid) {
                            _this.ValidationErrorsList = myResult.Errors;
                        }
                    }
                }
            }
        });
    };
    XMLFlightsSimulatorComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    XMLFlightsSimulatorComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './XMLFlightsSimulatorComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], XMLFlightsSimulatorComponent);
    return XMLFlightsSimulatorComponent;
}());
exports.XMLFlightsSimulatorComponent = XMLFlightsSimulatorComponent;
//# sourceMappingURL=XMLFlightsSimulatorComponent.js.map