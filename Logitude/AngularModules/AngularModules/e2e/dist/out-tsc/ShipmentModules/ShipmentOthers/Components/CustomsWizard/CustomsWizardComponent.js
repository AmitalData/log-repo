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
var ABMWebService_1 = require("../../../../Infrastructure/Services/WebServices/ABMWebService");
var CustomsWizardComponent = /** @class */ (function () {
    function CustomsWizardComponent() {
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsSendingMessageVisible = false;
        this.IsSendButtonEnabled = true;
        this.CancelButtonContent = "Cancel";
        this.myABMWebService = new ABMWebService_1.ABMWebService();
    }
    CustomsWizardComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs.EntityPM;
    };
    CustomsWizardComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsWizardComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending in Progress..");
            this.myABMWebService.Send(this.EntityPM.Id).subscribe(function (myResponse) {
                if (myResponse == null) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult == null) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.IsSendingMessageVisible = true;
                        _this.IsSendButtonEnabled = false;
                        _this.CancelButtonContent = "Close";
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    };
    CustomsWizardComponent = __decorate([
        core_1.Component({
            selector: 'CustomsWizardComponent',
            moduleId: module.id,
            templateUrl: './CustomsWizardComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsWizardComponent);
    return CustomsWizardComponent;
}());
exports.CustomsWizardComponent = CustomsWizardComponent;
//# sourceMappingURL=CustomsWizardComponent.js.map