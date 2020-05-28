"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CustomsErrorsComponent = /** @class */ (function () {
    function CustomsErrorsComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CustomsErrorsComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.ComponentHeight = windowArgs.ComponentHeight;
        this.Errors = windowArgs.Errors;
        this.ErrorsCount = "Errors Found: ";
        if (this.Errors) {
            this.ErrorsCount = this.ErrorsCount + this.Errors.length;
        }
        this.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.General.B.No');
        this.CancelButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.General.B.Cancel');
        this.SaveButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.General.B.OK');
        this.CancelButtonVisibility = windowArgs.CancelButtonVisibility;
        this.NoButtonVisibility = windowArgs.NoButtonVisibility;
        if (windowArgs.NoButtonText) {
            this.NoButtonText = windowArgs.NoButtonText;
        }
        if (windowArgs.CancelButtonText) {
            this.CancelButtonText = windowArgs.CancelButtonText;
        }
        if (windowArgs.SaveButtonText) {
            this.SaveButtonText = windowArgs.SaveButtonText;
        }
    };
    CustomsErrorsComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    CustomsErrorsComponent.prototype.NoButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("no");
    };
    CustomsErrorsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    CustomsErrorsComponent = __decorate([
        core_1.Component({
            selector: 'CustomsErrorsComponent',
            moduleId: module.id,
            templateUrl: './CustomsErrorsComponent.html',
        })
    ], CustomsErrorsComponent);
    return CustomsErrorsComponent;
}());
exports.CustomsErrorsComponent = CustomsErrorsComponent;
//# sourceMappingURL=CustomsErrorsComponent.js.map