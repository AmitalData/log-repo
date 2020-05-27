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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var CustomerDepositionListExtendedService_1 = require("../../../../../Common/Services/ExtendedLists/CustomerDepositionListExtendedService");
var CustomsShipperGeneralTabComponent = /** @class */ (function () {
    function CustomsShipperGeneralTabComponent(entityArgs) {
        var _this = this;
        this.entityArgs = entityArgs;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ItemsSource = [];
        this.IsReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.customerDepositionListExtendedService = new CustomerDepositionListExtendedService_1.CustomerDepositionListExtendedService();
        this._entityResourceService.getEntityResourceByTableName("CustomerDeposition").subscribe(function (response) {
            _this.IsReady = true;
            _this.LoadData();
        });
    }
    CustomsShipperGeneralTabComponent.prototype.ngOnInit = function () {
    };
    CustomsShipperGeneralTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.customerDepositionListExtendedService.GetCustomerDepositionListsByCustomsShipperId(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    CustomsShipperGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'CustomsShipperGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './CustomsShipperGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomsShipperGeneralTabComponent);
    return CustomsShipperGeneralTabComponent;
}());
exports.CustomsShipperGeneralTabComponent = CustomsShipperGeneralTabComponent;
//# sourceMappingURL=CustomsShipperGeneralTabComponent.js.map