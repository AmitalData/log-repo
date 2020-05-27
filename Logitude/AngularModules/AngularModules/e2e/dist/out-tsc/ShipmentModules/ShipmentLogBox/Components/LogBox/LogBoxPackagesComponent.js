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
var LogBoxPackagesComponent = /** @class */ (function () {
    function LogBoxPackagesComponent() {
        this.DataContext = this;
        this.Title = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    LogBoxPackagesComponent.prototype.ngOnInit = function () {
    };
    LogBoxPackagesComponent.prototype.ngAfterViewInit = function () {
    };
    LogBoxPackagesComponent.prototype.SetWindowArgs = function (args) {
        this.ShipmentPM = args.ShipmentPM;
        this.Title = args.Title;
    };
    LogBoxPackagesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    LogBoxPackagesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LogBoxPackagesComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], LogBoxPackagesComponent);
    return LogBoxPackagesComponent;
}());
exports.LogBoxPackagesComponent = LogBoxPackagesComponent;
//# sourceMappingURL=LogBoxPackagesComponent.js.map