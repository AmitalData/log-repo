"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var AccountingPeriodEventComponent = /** @class */ (function (_super) {
    __extends(AccountingPeriodEventComponent, _super);
    function AccountingPeriodEventComponent(_entityResourceService, entityArgs) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingPeriod";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AccountingPeriodEventComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        //this.entityArgs = new EntityArgs();
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        //this.entityArgs = args;
    };
    AccountingPeriodEventComponent.prototype.OkButtonClicked = function () {
        this.CancelButtonClicked();
    };
    AccountingPeriodEventComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingPeriodEventComponent.prototype.ngAfterViewInit = function () {
        this.LoadEventsTab();
    };
    AccountingPeriodEventComponent.prototype.LoadEventsTab = function () {
        var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'EventsLocation'; });
        var myLocation = locs.filter(function (f) { return f.ItemCode == "1"; })[0];
        if (myLocation != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Common/Components/Events/EventsTabComponent", myLocation.viewContainerRef)
                .then(function (cmpRef) {
            });
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AccountingPeriodEventComponent.prototype, "AllLocations", void 0);
    AccountingPeriodEventComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AccountingPeriodEventComponent',
            templateUrl: './AccountingPeriodEventComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityArgs_1.EntityArgs])
    ], AccountingPeriodEventComponent);
    return AccountingPeriodEventComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingPeriodEventComponent = AccountingPeriodEventComponent;
//# sourceMappingURL=AccountingPeriodEventComponent.js.map