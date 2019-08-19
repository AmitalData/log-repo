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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ContainerFollowupActionsComponent = /** @class */ (function () {
    function ContainerFollowupActionsComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ContainerFollowupActionsComponent.prototype.SetWindowArgs = function (args) {
        this.Code = args['Code'];
        switch (this.Code) {
            case "D": {
                this.RoutingLinkText = "Add Container Delivery";
                break;
            }
            case "R": {
                this.RoutingLinkText = "Add Empty Container Return";
                break;
            }
        }
    };
    ContainerFollowupActionsComponent.prototype.SelectAction = function (typeCode) {
        this.CurrentSession.CloseCurrentWindowEmit(typeCode);
    };
    ContainerFollowupActionsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ContainerFollowupActionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContainerFollowupActionsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ContainerFollowupActionsComponent);
    return ContainerFollowupActionsComponent;
}());
exports.ContainerFollowupActionsComponent = ContainerFollowupActionsComponent;
//# sourceMappingURL=ContainerFollowupActionsComponent.js.map