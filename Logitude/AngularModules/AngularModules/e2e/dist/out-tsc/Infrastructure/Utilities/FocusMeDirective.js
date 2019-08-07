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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var FocusMeDirective = /** @class */ (function () {
    function FocusMeDirective(viewContainerRef) {
        this.viewContainerRef = viewContainerRef;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    FocusMeDirective.prototype.ngAfterViewInit = function () {
        // console.log("i will focus the hell out of you." + this.ElementId);
        var element = document.getElementById(this.ElementId);
        element.focus();
        this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id });
        //element.addEventListener("blur", function () {
        //    this.CurrentSession.SessionEvent.emit("LostFocusMe");
        //});
    };
    FocusMeDirective.prototype.BlurMe = function () {
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], FocusMeDirective.prototype, "ElementId", void 0);
    FocusMeDirective = __decorate([
        core_1.Directive({
            selector: '[FocusMe]'
        }),
        __metadata("design:paramtypes", [core_1.ViewContainerRef])
    ], FocusMeDirective);
    return FocusMeDirective;
}());
exports.FocusMeDirective = FocusMeDirective;
//# sourceMappingURL=FocusMeDirective.js.map