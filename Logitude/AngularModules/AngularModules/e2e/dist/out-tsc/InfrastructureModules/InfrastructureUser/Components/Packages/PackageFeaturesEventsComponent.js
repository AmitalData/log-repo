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
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PackageFeaturesEventsComponent = /** @class */ (function () {
    function PackageFeaturesEventsComponent() {
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.myDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
    }
    PackageFeaturesEventsComponent.prototype.SetWindowArgs = function (myPackageCode) {
        var _this = this;
        if (myPackageCode) {
            this.myDomainService.GetPackageFeaturesChanges(myPackageCode).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                }
            });
        }
    };
    PackageFeaturesEventsComponent.prototype.ShowNotesClicked = function (notes) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Package Events Changes";
        logWindow.WindowArgs = notes;
        logWindow.Width = 700;
        logWindow.Height = 450;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/FeaturesEventChangesComponent');
    };
    PackageFeaturesEventsComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PackageFeaturesEventsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PackageFeaturesEventsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PackageFeaturesEventsComponent);
    return PackageFeaturesEventsComponent;
}());
exports.PackageFeaturesEventsComponent = PackageFeaturesEventsComponent;
//# sourceMappingURL=PackageFeaturesEventsComponent.js.map