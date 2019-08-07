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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var TenantManagementPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/TenantManagementPMService");
var QueueMessagesWebService_1 = require("../../../../Infrastructure/Services/WebServices/QueueMessagesWebService");
var TenantManagementStatisticsTabComponent = /** @class */ (function (_super) {
    __extends(TenantManagementStatisticsTabComponent, _super);
    function TenantManagementStatisticsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "TenantManagement";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingAllowed = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        return _this;
    }
    TenantManagementStatisticsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.IsEditingAllowed = this.IsTenantManagementEditable();
            this.SetData();
        }
    };
    TenantManagementStatisticsTabComponent.prototype.IsTenantManagementEditable = function () {
        var myResult = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }
        return myResult;
    };
    TenantManagementStatisticsTabComponent.prototype.SetData = function () {
        this.StatisticsUpdateDate = this.EntityPM.StatisticsUpdateDate;
        this.LastFWBSentDate = this.EntityPM.LastFWBSentDate;
        this.LastFHLSentDate = this.EntityPM.LastFHLSentDate;
        this.LastFFRSentDate = this.EntityPM.LastFFRSentDate;
        this.LastFWBCargonautSentDate = this.EntityPM.LastFWBCargonautSentDate;
        this.LastFHLCargonautSentDate = this.EntityPM.LastFHLCargonautSentDate;
        this.FSRLastSentDate = this.EntityPM.FSRLastSentDate;
        this.FSULastReceivedDate = this.EntityPM.FSULastReceivedDate;
        this.FSALastReceivedDate = this.EntityPM.FSALastReceivedDate;
        this.CustomerLastDate = this.EntityPM.CustomerLastDate;
        this.CustomerTotalLastWeek = this.EntityPM.CustomerTotalLastWeek;
        this.CustomerTotalLastMonth = this.EntityPM.CustomerTotalLastMonth;
        this.ShipmentLastDate = this.EntityPM.ShipmentLastDate;
        this.ShipmentTotalLastWeek = this.EntityPM.ShipmentTotalLastWeek;
        this.ShipmentTotalLastMonth = this.EntityPM.ShipmentTotalLastMonth;
        this.ARInvoiceLastDate = this.EntityPM.ARInvoiceLastDate;
        this.ARInvoiceTotalLastWeek = this.EntityPM.ARInvoiceTotalLastWeek;
        this.ARInvoiceTotalLastMonth = this.EntityPM.ARInvoiceTotalLastMonth;
        this.QuoteLastDate = this.EntityPM.QuoteLastDate;
        this.QuoteTotalLastWeek = this.EntityPM.QuoteTotalLastWeek;
        this.QuoteTotalLastMonth = this.EntityPM.QuoteTotalLastMonth;
        this.APInvoiceLastDate = this.EntityPM.APInvoiceLastDate;
        this.APInvoiceTotalLastWeek = this.EntityPM.APInvoiceTotalLastWeek;
        this.APInvoiceTotalLastMonth = this.EntityPM.APInvoiceTotalLastMonth;
        this.OpportunityLastDate = this.EntityPM.OpportunityLastDate;
        this.OpportunityTotalLastWeek = this.EntityPM.OpportunityTotalLastWeek;
        this.OpportunityTotalLastMonth = this.EntityPM.OpportunityTotalLastMonth;
        this.ActivityLastDate = this.EntityPM.ActivityLastDate;
        this.ActivityTotalLastWeek = this.EntityPM.ActivityTotalLastWeek;
        this.ActivityTotalLastMonth = this.EntityPM.ActivityTotalLastMonth;
        this.ShardLogisticLastDate = this.EntityPM.ShardLogisticLastDate;
        this.ShardLogisticTotalLastWeek = this.EntityPM.ShardLogisticTotalLastWeek;
        this.ShardLogisticTotalLastMonth = this.EntityPM.ShardLogisticTotalLastMonth;
        this.MobileLastDate = this.EntityPM.MobileLastDate;
        this.MobileTotalLastWeek = this.EntityPM.MobileTotalLastWeek;
        this.MobileTotalLastMonth = this.EntityPM.MobileTotalLastMonth;
        this.AgentSharedLogisticsStatisticsLastDate = this.EntityPM.AgentSharedLogisticsStatisticsLastDate;
        this.AgentSharedLogisticsStatisticsLastWeek = this.EntityPM.AgentSharedLogisticsStatisticsLastWeek;
        this.AgentSharedLogisticsStatisticsLastMonth = this.EntityPM.AgentSharedLogisticsStatisticsLastMonth;
        this.SetColors();
    };
    TenantManagementStatisticsTabComponent.prototype.SetColors = function () {
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var date7 = Tools_1.DateTool.AddDays(todayDate, -7).valueOf();
        this.CustomerLastDateColor = Tools_1.FontTool.Gray;
        this.ShipmentLastDateColor = Tools_1.FontTool.Gray;
        this.ARInvoiceLastDateColor = Tools_1.FontTool.Gray;
        this.QuoteLastDateColor = Tools_1.FontTool.Gray;
        this.APInvoiceLastDateColor = Tools_1.FontTool.Gray;
        this.OpportunityLastDateColor = Tools_1.FontTool.Gray;
        this.ActivityLastDateColor = Tools_1.FontTool.Gray;
        this.ShardLogisticLastDateColor = Tools_1.FontTool.Gray;
        this.MobileLastDateColor = Tools_1.FontTool.Gray;
        this.AgentSharedLogisticsStatisticsLastDateColor = Tools_1.FontTool.Gray;
        if (this.EntityPM.CustomerLastDate != null) {
            var myDate = new Date(this.EntityPM.CustomerLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.CustomerLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.ShipmentLastDate != null) {
            var myDate = new Date(this.EntityPM.ShipmentLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.ShipmentLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.ARInvoiceLastDate != null) {
            var myDate = new Date(this.EntityPM.ARInvoiceLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.ARInvoiceLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.QuoteLastDate != null) {
            var myDate = new Date(this.EntityPM.QuoteLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.QuoteLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.APInvoiceLastDate != null) {
            var myDate = new Date(this.EntityPM.APInvoiceLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.APInvoiceLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.OpportunityLastDate != null) {
            var myDate = new Date(this.EntityPM.OpportunityLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.OpportunityLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.ActivityLastDate != null) {
            var myDate = new Date(this.EntityPM.ActivityLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.ActivityLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.ShardLogisticLastDate != null) {
            var myDate = new Date(this.EntityPM.ShardLogisticLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.ShardLogisticLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.MobileLastDate != null) {
            var myDate = new Date(this.EntityPM.MobileLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.MobileLastDateColor = Tools_1.FontTool.Red;
            }
        }
        if (this.EntityPM.AgentSharedLogisticsStatisticsLastDate != null) {
            var myDate = new Date(this.EntityPM.AgentSharedLogisticsStatisticsLastDate.valueOf()).valueOf();
            if (myDate < date7) {
                this.AgentSharedLogisticsStatisticsLastDateColor = Tools_1.FontTool.Red;
            }
        }
    };
    TenantManagementStatisticsTabComponent.prototype.RefreshClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Refreshing....");
        var service = new TenantManagementPMService_1.TenantManagementPMService();
        service.get(this.EntityPM.Id).subscribe(function (response) {
            if (!response.HasError) {
                _this.EntityPM = response.Result;
                if (_this.EntityPM != null) {
                    _this.SetData();
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    TenantManagementStatisticsTabComponent.prototype.UpdateClicked = function () {
        var service = new QueueMessagesWebService_1.QueueMessagesWebService();
        service.UpdateTenantManagementStatistics(this.EntityPM.Id).subscribe(function (myResult) {
            //var myResponse: ServiceResponse = myResult;
            //if (!myResponse.HasError) {
            //}
        });
    };
    TenantManagementStatisticsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TenantManagementStatisticsTabComponent',
            templateUrl: './TenantManagementStatisticsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TenantManagementStatisticsTabComponent);
    return TenantManagementStatisticsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TenantManagementStatisticsTabComponent = TenantManagementStatisticsTabComponent;
//# sourceMappingURL=TenantManagementStatisticsTabComponent.js.map