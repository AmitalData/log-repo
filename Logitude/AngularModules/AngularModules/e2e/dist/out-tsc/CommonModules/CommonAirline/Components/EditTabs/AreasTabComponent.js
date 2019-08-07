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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var AreasTabComponent = /** @class */ (function (_super) {
    __extends(AreasTabComponent, _super);
    function AreasTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Airline";
        _this.ResourcesReady = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._entityResourceService.getEntityResourceByTableName("AirlineArea", 0).subscribe(function (response) {
            _this.ResourcesReady = true;
            _this.EntityPM = entityArgs.EntityPM;
            _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        });
        return _this;
    }
    AreasTabComponent.prototype.ngOnInit = function () {
    };
    AreasTabComponent.prototype.AddEditAirlineAreaClicked = function (EditedEntity) {
        var _this = this;
        if (EditedEntity === void 0) { EditedEntity = null; }
        this._entityResourceService.getEntityResourceByTableName("AirlineAreasPort", 0).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            if (EditedEntity == null) {
                logitudeWindow.Title = "New Area";
            }
            else {
                logitudeWindow.Title = "Edit Area";
            }
            logitudeWindow.Width = 700;
            logitudeWindow.Height = 650;
            var isNew = false;
            if (EditedEntity == null) {
                isNew = true;
            }
            else {
                isNew = false;
            }
            logitudeWindow.WindowArgs = { EntityPM: _this.EntityPM, IsNew: isNew, Entity: EditedEntity };
            logitudeWindow.Show("./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAreaComponent");
        });
    };
    AreasTabComponent.prototype.DeleteAirlineAreaClicked = function (EditedEntity) {
        var _this = this;
        if (EditedEntity === void 0) { EditedEntity = null; }
        if (EditedEntity) {
            var window = new ConfirmWindow_1.ConfirmWindow();
            window.Show("Are you sure you want to delete this area?");
            window.WindowClosed.subscribe(function (event) {
                if (window.Yes) {
                    _this.EntityPM.RemoveAirlineAreaPM(EditedEntity);
                }
            });
        }
    };
    AreasTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AreasTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AreasTabComponent);
    return AreasTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AreasTabComponent = AreasTabComponent;
//# sourceMappingURL=AreasTabComponent.js.map