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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TMProjectHelperComponent = /** @class */ (function () {
    function TMProjectHelperComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = this.entityArgs.EntityPM;
    }
    TMProjectHelperComponent.prototype.NewInnerProject = function () {
        var window = new LogitudeWindow_1.LogitudeWindow();
        window.Title = "New Project";
        window.WindowArgs = { EntityArgs: this.EntityPM };
        window.Show('./TimeManagement/Components/NewEntity/NewProjectComponent');
    };
    TMProjectHelperComponent.prototype.ConnectParentProject = function () {
        var window = new LogitudeWindow_1.LogitudeWindow();
        window.Title = "Connect to Parent";
        window.WindowArgs = { EntityArgs: this.EntityPM };
        window.Height = 170;
        window.Width = 500;
        window.Show('./TimeManagement/Components/Connections/ConnectToParentComponent');
    };
    TMProjectHelperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TMProjectHelperComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TMProjectHelperComponent);
    return TMProjectHelperComponent;
}());
exports.TMProjectHelperComponent = TMProjectHelperComponent;
//# sourceMappingURL=TMProjectHelperComponent.js.map