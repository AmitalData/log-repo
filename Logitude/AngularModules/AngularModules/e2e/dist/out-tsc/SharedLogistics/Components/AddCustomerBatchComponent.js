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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CustomerTenantAccessCardsBatchPM_1 = require("../../Common/EntityPMs/CustomerTenantAccessCardsBatchPM");
var Tools_1 = require("../../Infrastructure/Tools");
var CustomerTenantAccessCardsBatchPMService_1 = require("../../Common/Services/StandardPMs/CustomerTenantAccessCardsBatchPMService");
var AddCustomerBatchComponent = /** @class */ (function (_super) {
    __extends(AddCustomerBatchComponent, _super);
    function AddCustomerBatchComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddCustomerBatchComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.AccessCardPM = args.AccessCardPM;
            this.Parent = args.Parent;
        }
    };
    Object.defineProperty(AddCustomerBatchComponent.prototype, "FromDatetime", {
        get: function () {
            if (this.fromDatetime == null || this.fromDatetime.getFullYear() == 1 || this.fromDatetime == Tools_1.DateTool.GetDateFormats(new Date()).DateParts.DateObject) {
                var date = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                date.setDate(date.getDate() - 90);
                this.fromDatetime = date;
            }
            return this.fromDatetime;
        },
        set: function (value) {
            if (this.fromDatetime != value)
                this.fromDatetime = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddCustomerBatchComponent.prototype, "ToDatetime", {
        get: function () {
            if (this.toDatetime == null || this.toDatetime.getFullYear() == 1 || this.toDatetime == Tools_1.DateTool.GetDateFormats(new Date()).DateParts.DateObject) {
                this.toDatetime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            }
            return this.toDatetime;
        },
        set: function (value) {
            if (this.toDatetime != value)
                this.toDatetime = value;
        },
        enumerable: true,
        configurable: true
    });
    AddCustomerBatchComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddCustomerBatchComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.FromDatetime > this.ToDatetime)
            this.ValidationErrorsList.push("From date must be smaller\equal to To date");
        if (this.ValidationErrorsList.length == 0) {
            var service = new CustomerTenantAccessCardsBatchPMService_1.CustomerTenantAccessCardsBatchPMService();
            var customerTenantAccessCardsBatchPM = new CustomerTenantAccessCardsBatchPM_1.CustomerTenantAccessCardsBatchPM();
            customerTenantAccessCardsBatchPM.CustomerId = this.AccessCardPM.CustomerId;
            customerTenantAccessCardsBatchPM.CustomerTenantAccessId = this.AccessCardPM.CustomerTenantAccessId;
            customerTenantAccessCardsBatchPM.ToDatetime = this.ToDatetime;
            customerTenantAccessCardsBatchPM.FromDatetime = this.FromDatetime;
            customerTenantAccessCardsBatchPM.Status = "Created";
            customerTenantAccessCardsBatchPM.Tenant = this.AccessCardPM.Tenant;
            customerTenantAccessCardsBatchPM.TotalFailed = 0;
            customerTenantAccessCardsBatchPM.TotalShipment = 0;
            customerTenantAccessCardsBatchPM.Totalsucceeded = 0;
            service.insert(customerTenantAccessCardsBatchPM).subscribe(function (p) {
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            });
            ;
        }
    };
    AddCustomerBatchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddCustomerBatchComponent',
            templateUrl: './AddCustomerBatchComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddCustomerBatchComponent);
    return AddCustomerBatchComponent;
}(BaseComponent_1.BaseComponent));
exports.AddCustomerBatchComponent = AddCustomerBatchComponent;
//# sourceMappingURL=AddCustomerBatchComponent.js.map