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
var INTRAWebService_1 = require("../../../../Shipment/Services/INTRAWebService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var SimulatorComponent = /** @class */ (function () {
    function SimulatorComponent() {
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AnalyzeQueueId = null;
        this.XML_Text = null;
        this.useAnalyzeQueueId = false;
    }
    Object.defineProperty(SimulatorComponent.prototype, "UseAnalyzeQueueId", {
        get: function () { return this.useAnalyzeQueueId; },
        set: function (value) {
            if (this.useAnalyzeQueueId != value) {
                this.useAnalyzeQueueId = value;
                if (value) {
                    this.XML_Text = null;
                }
                else {
                    this.AnalyzeQueueId = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SimulatorComponent.prototype.CancelClicked = function () {
        this.Close();
    };
    SimulatorComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SimulatorComponent.prototype.SimulateClicked = function () {
        var _this = this;
        var errors = [];
        if (this.UseAnalyzeQueueId) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.AnalyzeQueueId)) {
                errors.push("Please fill your Analyze Queue Id");
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
            var simulator = new INTRAWebService_1.INTTRASimulator();
            simulator.AnalyzeQueueId = this.AnalyzeQueueId;
            simulator.XmlString = this.XML_Text;
            var myService = new INTRAWebService_1.INTRAWebService();
            myService.Simulate(simulator).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult.Success) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("Simulated Successfully");
                    }
                    else {
                        _this.ValidationErrorsList = myResult.Errors;
                    }
                }
            });
        }
    };
    SimulatorComponent.prototype.ReadFTPClicked = function () {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = errors;
        this.CurrentSession.StartBusyIndicator("Reading FTP...");
        var myService = new INTRAWebService_1.INTRAWebService();
        myService.ReadFTP().subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var myResult = myResponse.Result;
                if (myResult.Success) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    if (myResult.FilesCount == 0) {
                        messageWindow.Show("The FTP folder is empty");
                    }
                    else {
                        messageWindow.Show(myResult.FilesCount + " Files found");
                    }
                }
                else {
                    _this.ValidationErrorsList = myResult.Errors;
                }
            }
        });
    };
    SimulatorComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SimulatorComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SimulatorComponent);
    return SimulatorComponent;
}());
exports.SimulatorComponent = SimulatorComponent;
//# sourceMappingURL=SimulatorComponent.js.map