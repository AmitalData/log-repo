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
var SimplogInfoPopupComponent = /** @class */ (function () {
    function SimplogInfoPopupComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SimplogInfoPopupComponent.prototype.ngOnInit = function () {
    };
    SimplogInfoPopupComponent.prototype.SetDataContext = function (dataContext) {
    };
    SimplogInfoPopupComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("Cancel");
    };
    SimplogInfoPopupComponent.prototype.SaveButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("Regenerate");
    };
    SimplogInfoPopupComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SimplogInfoPopup',
            templateUrl: './SimplogInfoPopupComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SimplogInfoPopupComponent);
    return SimplogInfoPopupComponent;
}());
exports.SimplogInfoPopupComponent = SimplogInfoPopupComponent;
//# sourceMappingURL=SimplogInfoPopupComponent.js.map