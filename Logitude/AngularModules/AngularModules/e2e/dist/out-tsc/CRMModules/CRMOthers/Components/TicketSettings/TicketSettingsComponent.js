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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TenantPM_1 = require("../../../../Common/EntityPMs/TenantPM");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TicketSettingsComponent = /** @class */ (function (_super) {
    __extends(TicketSettingsComponent, _super);
    function TicketSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.TenantPm = new TenantPM_1.TenantPM();
        _this.IsVisibile = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
        _this.Filters.GetAll = true;
        _this.LoadTenantPMMethod();
        return _this;
    }
    TicketSettingsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.TenantPm = response.Result;
            _this.IsVisibile = true;
        });
    };
    Object.defineProperty(TicketSettingsComponent.prototype, "IsCorrespondenceRightToLeftEnabled", {
        get: function () { return this.TenantPm.IsCorrespondenceRightToLeftEnabled; },
        set: function (value) {
            if (this.TenantPm.IsCorrespondenceRightToLeftEnabled != value) {
                this.TenantPm.IsCorrespondenceRightToLeftEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketSettingsComponent.prototype, "IsInternalTicketByDefault", {
        get: function () { return this.TenantPm.IsInternalTicketByDefault; },
        set: function (value) {
            if (this.TenantPm.IsInternalTicketByDefault != value) {
                this.TenantPm.IsInternalTicketByDefault = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketSettingsComponent.prototype, "DefaultSLAId", {
        get: function () {
            return this.TenantPm.DefaultSLAId;
        },
        set: function (value) {
            if (this.TenantPm.DefaultSLAId != value) {
                this.TenantPm.DefaultSLAId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    TicketSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TicketSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.TenantPm).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.TenantPm;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
            }
        });
    };
    TicketSettingsComponent = __decorate([
        core_1.Component({
            selector: 'TicketSettingsComponent',
            moduleId: module.id,
            templateUrl: './TicketSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TicketSettingsComponent);
    return TicketSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.TicketSettingsComponent = TicketSettingsComponent;
//# sourceMappingURL=TicketSettingsComponent.js.map