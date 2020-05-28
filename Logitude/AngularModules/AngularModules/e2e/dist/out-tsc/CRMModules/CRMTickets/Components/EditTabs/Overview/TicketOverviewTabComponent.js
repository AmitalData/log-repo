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
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var CRMDomainService_1 = require("../../../../../CRM/Services/CRMDomainService");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TicketOverviewTabComponent = /** @class */ (function () {
    function TicketOverviewTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = "Ticket";
        this.PerformanceChartIdExistance = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Statistics
        this.NumberOfExCorrespondences = 0;
        this.NumberOfInCorrespondences = 0;
        this.NumberOfAllActivities = 0;
        this.NumberOfEscalations = 0;
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.LoadAllData();
    }
    TicketOverviewTabComponent.prototype.InitializeServices = function () {
        this.CRMDomainService = new CRMDomainService_1.CRMDomainService();
        this.PerformanceChartId = "PerformanceChartId_" + this.CurrentSession.GetNewId("PerformanceChartId");
        this.LegendDiv = "LegendDiv_" + this.CurrentSession.GetNewId("LegendDiv");
    };
    TicketOverviewTabComponent.prototype.LoadAllData = function () {
        this.LoadStatisticsDataCounts();
        this.LoadOpenedPerformanceData();
        this.LoadLogsData();
    };
    TicketOverviewTabComponent.prototype.LoadLogsData = function () {
        var _this = this;
        this.CRMDomainService.GetBusinessHours(this.EntityPM.Id).subscribe(function (response) {
            if (response != null && !response.HasError) {
                var result = response.Result;
                _this.ResolveSLAViolated = result.ResolveSLAViolated;
                _this.ResponseSLAViolated = result.ResponseSLAViolated;
                _this.ResponseSLAViolatedTotalMinutes = result.ResponseSLAViolatedTotalMinutes;
                _this.ResolveSLAViolatedTotalMinutes = result.ResolveSLAViolatedTotalMinutes;
                _this.SummaryOpenPeriodMinutes = result.OpenPeriodMinutes;
                _this.RefreshLogData();
            }
        });
    };
    TicketOverviewTabComponent.prototype.LoadStatisticsDataCounts = function () {
        var _this = this;
        this.CRMDomainService.GetTicketOverViewStatisticsSummary(this.EntityPM.Id).subscribe(function (response) {
            if (response != null && !response.HasError) {
                var result = response.Result;
                _this.NumberOfExCorrespondences = result.Tickets_ExternalLines;
                _this.NumberOfInCorrespondences = result.Tickets_InternalLines;
                _this.NumberOfAllActivities = result.Tickets_Activities;
                _this.NumberOfEscalations = result.Tickets_Escalations;
            }
        });
    };
    TicketOverviewTabComponent.prototype.LoadOpenedPerformanceData = function () {
        var _this = this;
        this.CRMDomainService.GetTicketOverviewPerformance(this.EntityPM.Id).subscribe(function (response) {
            if (response != null && !response.HasError) {
                var result = response.Result;
                _this.fillPerformanceChart(result, _this);
            }
            else
                _this.PerformanceChartIdExistance = false;
        });
    };
    TicketOverviewTabComponent.prototype.fillPerformanceChart = function (data, viewModel) {
        var lineData = this.FillLine(data);
        var graphs = [{
                id: "g3",
                "useNegativeColorIfDown": false,
                "bullet": "round",
                "balloonFunction": function (item, graph) {
                    return viewModel.MathValues(item.dataContext.data1);
                },
                "bulletBorderAlpha": 1,
                "bulletBorderColor": "#FFFFFF",
                "hideBulletsCount": 50,
                "lineThickness": 2,
                "dashLength": 3,
                "lineColor": "#3a5cba",
                "negativeLineColor": "#3a5cba",
                "valueField": "data1",
                "title": "SLA"
            },
            {
                id: "g4",
                "useNegativeColorIfDown": false,
                "bullet": "square",
                "balloonFunction": function (item, graph) {
                    return viewModel.MathValues(item.dataContext.data2);
                },
                "bulletBorderAlpha": 1,
                "bulletBorderColor": "#FFFFFF",
                "hideBulletsCount": 50,
                "lineThickness": 2,
                "lineColor": "#FF0000",
                "negativeLineColor": "#FF0000",
                "valueField": "data2",
                "title": "Actual"
            }
        ];
        if (this.PerformanceChartIdExistance)
            makeAMLineChartMultiple(this.PerformanceChartId, lineData, null, graphs, true, this.LegendDiv);
    };
    TicketOverviewTabComponent.prototype.MathValues = function (values) {
        if (values) {
            if (values % 2 == 0)
                return values + " Hours 0 Min";
            else {
                var stringValue = values.toString();
                var arrValue = stringValue.split('.');
                var min = parseFloat("0." + arrValue[1]) * 60;
                var mins = Tools_1.AppTool.Round(min, 0);
                return arrValue[0] + " Hours " + Tools_1.AppTool.Round(min, 1) + " Min";
            }
        }
    };
    TicketOverviewTabComponent.prototype.FillLine = function (data) {
        data = data.filter(function (d) { return d.DataTypeCode == "SLA" || d.DataTypeCode == "Actual"; });
        if (data)
            if (data.Length != 0)
                this.PerformanceChartIdExistance = true;
        var AmLineChartTest = [{ "Category": '', "data1": '', "data2": '' }, { "Category": '', "data1": '', "data2": '' }];
        if (this.PerformanceChartIdExistance) {
            var index = 0;
            data.forEach(function (element) {
                if (element.DataTypeCode == "SLA") {
                    if (index == 0)
                        AmLineChartTest[0].Category = element.LabelProperty;
                    if (index % 2 == 0)
                        AmLineChartTest[0].data1 = !Tools_1.AppTool.IsNullOrEmpty(AmLineChartTest[0].data1) ? (parseInt(AmLineChartTest[0].data1) + parseInt(element.IntegerProperty + "") / 60) + "" : AmLineChartTest[0].data1 = parseInt(element.IntegerProperty + "") / 60 + "";
                    else
                        AmLineChartTest[1].data1 = !Tools_1.AppTool.IsNullOrEmpty(AmLineChartTest[1].data1) ? (parseInt(AmLineChartTest[1].data1) + parseInt(element.IntegerProperty + "") / 60) + "" : AmLineChartTest[1].data1 = parseInt(element.IntegerProperty + "") / 60 + "";
                }
                else {
                    if (index == 1)
                        AmLineChartTest[1].Category = element.LabelProperty;
                    if (index % 2 == 0)
                        AmLineChartTest[0].data2 = !Tools_1.AppTool.IsNullOrEmpty(AmLineChartTest[0].data2) ? (parseFloat(AmLineChartTest[0].data2) + element.DoubleProperty) + "" : AmLineChartTest[0].data2 = element.DoubleProperty + "";
                    else
                        AmLineChartTest[1].data2 = !Tools_1.AppTool.IsNullOrEmpty(AmLineChartTest[1].data2) ? (parseFloat(AmLineChartTest[1].data2) + element.DoubleProperty) + "" : AmLineChartTest[1].data2 = element.DoubleProperty + "";
                    index++;
                }
            });
        }
        return AmLineChartTest;
    };
    Object.defineProperty(TicketOverviewTabComponent.prototype, "CreateDate", {
        // Props
        get: function () { return this.EntityPM.CreateDate; },
        set: function (value) {
            if (this.EntityPM.CreateDate != value) {
                this.EntityPM.CreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketOverviewTabComponent.prototype, "FirstResponseTime", {
        get: function () { return this.EntityPM.FirstResponseTime; },
        set: function (value) {
            if (this.EntityPM.FirstResponseTime != value) {
                this.EntityPM.FirstResponseTime = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketOverviewTabComponent.prototype, "FullResolvedTime", {
        get: function () { return this.EntityPM.FullResolvedTime; },
        set: function (value) {
            if (this.EntityPM.FullResolvedTime != value) {
                this.EntityPM.FullResolvedTime = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketOverviewTabComponent.prototype.RefreshLogData = function () {
        this.GetResponseSLAViolatedString();
        this.GetResolveSLAViolatedString();
        this.GetOpenPeriodMinutes();
        this.GetTotalOpenPeriodVisibility();
        this.GetResponseSLAViolatedVisibility();
        this.GetResolveSLAViolatedVisibility();
    };
    TicketOverviewTabComponent.prototype.GetResponseSLAViolatedString = function () {
        var result = "";
        var val = this.ResponseSLAViolatedTotalMinutes;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        this.ResponseSLAViolatedString = result;
    };
    TicketOverviewTabComponent.prototype.GetResolveSLAViolatedString = function () {
        var result = "";
        var val = this.ResolveSLAViolatedTotalMinutes;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        this.ResolveSLAViolatedString = result;
    };
    TicketOverviewTabComponent.prototype.GetOpenPeriodMinutes = function () {
        var val = this.SummaryOpenPeriodMinutes;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        this.OpenPeriodMinutes = result;
    };
    TicketOverviewTabComponent.prototype.GetTotalOpenPeriodVisibility = function () {
        var myResult = false;
        if (this.SummaryOpenPeriodMinutes != 0) {
            myResult = true;
        }
        this.TotalOpenPeriodVisibility = myResult;
    };
    TicketOverviewTabComponent.prototype.GetResponseSLAViolatedVisibility = function () {
        var myResult = false;
        if (this.ResponseSLAViolatedTotalMinutes != 0) {
            myResult = true;
        }
        this.ResponseSLAViolatedVisibility = myResult;
    };
    TicketOverviewTabComponent.prototype.GetResolveSLAViolatedVisibility = function () {
        var myResult = false;
        if (this.ResolveSLAViolatedTotalMinutes != 0) {
            myResult = true;
        }
        this.ResolveSLAViolatedVisibility = myResult;
    };
    TicketOverviewTabComponent.prototype.ViewAllData = function (arg) {
        var _this = this;
        switch (arg) {
            case "Escalations":
                {
                    var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
                    filterAgrs.addAdditionalFilter("TicketId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string", false, false);
                    var listArgs = new Args_1.ListComponentArgs();
                    listArgs.Filters = filterAgrs;
                    listArgs.QueryCode = "All Ticket Escalations";
                    listArgs.ObjectTableName = "TicketEscalation";
                    listArgs.BackButtonTitle = "Tickets";
                    this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                        });
                    });
                    break;
                }
            case "Activites":
                {
                    var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
                    filterAgrs.addAdditionalFilter("TicketId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string", false, false);
                    var listArgs = new Args_1.ListComponentArgs();
                    listArgs.Filters = filterAgrs;
                    listArgs.QueryCode = "All Activities";
                    listArgs.ObjectTableName = "Activity";
                    listArgs.BackButtonTitle = "Tickets";
                    this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                        });
                    });
                    break;
                }
        }
    };
    // Refresh Bbutton 
    TicketOverviewTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllData();
    };
    TicketOverviewTabComponent = __decorate([
        core_1.Component({
            selector: 'TicketOverviewTabComponent',
            moduleId: module.id,
            templateUrl: './TicketOverviewTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TicketOverviewTabComponent);
    return TicketOverviewTabComponent;
}());
exports.TicketOverviewTabComponent = TicketOverviewTabComponent;
//# sourceMappingURL=TicketOverviewTabComponent.js.map