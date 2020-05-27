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
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var FBLStockExtenedPMService_1 = require("../../../../Shipment/Services/ExtendedPMs/FBLStockExtenedPMService");
var FBLStackSelectionComponent = /** @class */ (function () {
    function FBLStackSelectionComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ShipperId = null;
        this.AirlineId = null;
        this.AirlineName = null;
        this.ItemsCount = 0;
        this.SelectedItem = null;
        this.ObjectTableName = "FBLStock";
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ItemsSource = [];
        this.FBLStockExtenedPMService = new FBLStockExtenedPMService_1.FBLStockExtenedPMService();
        this.CurrentSession.StartBusyIndicator("Loading FBL Numbers");
    }
    FBLStackSelectionComponent.prototype.SetWindowArgs = function (windowArgs) {
        var _this = this;
        this.args = windowArgs;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.Load();
        });
    };
    FBLStackSelectionComponent.prototype.Load = function () {
        var _this = this;
        this.ItemsCount = 0;
        this.ItemsSource = [];
        //this.FBLStockExtenedPMService.GetFBLStockPMsByTenant(SessionLocator.Tenant, 100, 1).subscribe((response: any) => {
        this.FBLStockExtenedPMService.GetAllFBLStockPMsByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            if (response.Result) {
                var data = response.Result;
                _this.ItemsSource = data.sort(function (a, b) { return a.Number - b.Number; });
            }
            _this.CurrentSession.StopBusyIndicator();
        });
        this.FBLStockExtenedPMService.GetAllFBLStockPMsCountByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            _this.ItemsCount = response.Result;
        });
    };
    FBLStackSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    FBLStackSelectionComponent.prototype.OkButtonClicked = function () {
        if (this.SelectedItem != null) {
            this.args.SelectedFBLStock = this.SelectedItem;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    FBLStackSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FBLStackSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], FBLStackSelectionComponent);
    return FBLStackSelectionComponent;
}());
exports.FBLStackSelectionComponent = FBLStackSelectionComponent;
//# sourceMappingURL=FBLStackSelectionComponent.js.map