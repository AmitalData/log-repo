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
var ShowNewFeaturesComponent = /** @class */ (function () {
    function ShowNewFeaturesComponent() {
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.myDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
    }
    ShowNewFeaturesComponent.prototype.ngOnInit = function () {
        this.LoadData();
    };
    ShowNewFeaturesComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetNewFeaturesList().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ItemsSource = myResponse.Result;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ShowNewFeaturesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ShowNewFeaturesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShowNewFeaturesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ShowNewFeaturesComponent);
    return ShowNewFeaturesComponent;
}());
exports.ShowNewFeaturesComponent = ShowNewFeaturesComponent;
//# sourceMappingURL=ShowNewFeaturesComponent.js.map