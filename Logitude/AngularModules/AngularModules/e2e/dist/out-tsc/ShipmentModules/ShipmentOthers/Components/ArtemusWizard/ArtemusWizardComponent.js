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
var ArtemusWebService_1 = require("../../../../Infrastructure/Services/WebServices/ArtemusWebService");
var ArtemusWizardComponent = /** @class */ (function () {
    function ArtemusWizardComponent() {
        this.ValidationErrorsList = [];
        this.IsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ArtemusWizardComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.myArtemusWebService = new ArtemusWebService_1.ArtemusWebService();
        this.ShipmentId = windowArgs.ShipmentId;
        this.Type = windowArgs.Type;
        this.CurrentSession.StartBusyIndicator("Sending...");
        if (this.Type == "BOL") {
            this.SendToArtemus_Bill();
        }
        else {
            this.SendToArtemus_Voyage();
        }
    };
    ArtemusWizardComponent.prototype.SendToArtemus_Voyage = function () {
        var _this = this;
        this.myArtemusWebService.SendAMS_Voyage(this.ShipmentId).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.IsVisible = true;
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.MessageText = "Checking Required Fields in Shipment...";
                _this.IsMessageValid = false;
            }
            else {
                _this.MessageText = "Voyage message has been sent successfully";
                _this.IsMessageValid = true;
            }
        });
    };
    ArtemusWizardComponent.prototype.SendToArtemus_Bill = function () {
        var _this = this;
        this.myArtemusWebService.SendAMS_Bill(this.ShipmentId).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.IsVisible = true;
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.MessageText = "Checking Required Fields in Shipment...";
                _this.IsMessageValid = false;
            }
            else {
                _this.MessageText = "BOL message has been sent successfully";
                _this.IsMessageValid = true;
            }
        });
    };
    ArtemusWizardComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ArtemusWizardComponent = __decorate([
        core_1.Component({
            selector: 'ArtemusWizardComponent',
            moduleId: module.id,
            templateUrl: './ArtemusWizardComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ArtemusWizardComponent);
    return ArtemusWizardComponent;
}());
exports.ArtemusWizardComponent = ArtemusWizardComponent;
//# sourceMappingURL=ArtemusWizardComponent.js.map