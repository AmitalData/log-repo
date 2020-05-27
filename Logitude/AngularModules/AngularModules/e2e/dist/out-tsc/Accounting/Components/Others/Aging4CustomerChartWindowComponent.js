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
var Tools_1 = require("../../../Infrastructure/Tools");
var GLAccountExtendedListService_1 = require("../../Services/ExtendedLists/GLAccountExtendedListService");
var AgingReportParameters_1 = require("../../DataContracts/AgingReportParameters");
var Aging4CustomerChartWindowComponent = /** @class */ (function (_super) {
    __extends(Aging4CustomerChartWindowComponent, _super);
    function Aging4CustomerChartWindowComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        _this.ValidationErrorsList = [];
        _this.ObjectTableName = "GLAccountTotalByMonth";
        _this.noCard = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Chart Code
        _this.barChartLabels = [];
        _this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        return _this;
    }
    Aging4CustomerChartWindowComponent.prototype.ngAfterViewInit = function () {
        this.LoadChartData();
    };
    Aging4CustomerChartWindowComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.AccountId = args.accountId;
            this.CardId = args.cardId;
            this.EntityList = args.entity;
            if (Tools_1.AppTool.IsNullOrEmpty(this.CardId))
                this.noCard = true;
            else
                this.noCard = false;
        }
    };
    Aging4CustomerChartWindowComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("ok");
    };
    Aging4CustomerChartWindowComponent.prototype.LoadChartData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AccountId))
            return;
        var numberOfmonthsbackwards = 3;
        var args = new AgingReportParameters_1.AgingReportParameters();
        args.Tenant = SessionLocator_1.SessionLocator.Tenant,
            args.AgingForDate = new Date();
        args.NumberOfmonthsbackwards = numberOfmonthsbackwards;
        args.VendorCustomerId = this.AccountId;
        //args.Category1Id = "";
        //args.Category2Id = "";
        //args.Category3Id = "";
        //args.Category4Id = "";
        //args.Category5Id = "";
        //args.CollectorId = SessionLocator.LoggedUserId;
        //args.SalesmanId = "";
        this._GLAccountExtendedListService.GetAgingReport(args).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var periods;
                    periods = res;
                    console.log("periods: ", periods);
                    _this.LoadChart(periods);
                }
            }
        });
    };
    Aging4CustomerChartWindowComponent.prototype.LoadChart = function (data) {
        var _this = this;
        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        //Graph Properties
        var Graphs = Graphs = [{
                "balloonText": "[[value]]",
                "fillAlphas": 1,
                "id": "AmGraph-1" + i,
                "title": "Aging",
                "type": "column",
                "valueField": "dataCol",
                "fillColors": ["#FFAD49", "#FFC47C", "#FFC47C", "#FFAD49",],
                "gradientOrientation": "horizontal",
                "borderAlpha": 0,
                "lineColor": "#fff"
            }];
        //#endregion
        data.forEach(function (element) {
            //if (element.Total <= 0) return;
            _this.barChartData[0].label = "Amount";
            // Amount
            var value = element.Total; // + (Math.floor((Math.random() * 2500) + 1));
            _this.barChartData[0].data[i] = value.toString();
            // Labels
            var label = element.PeriodName.replace("b4", "Before"); // replace 'b4' with 'Before'
            label = label.startsWith("Before") ? label.replace("/20", "/") : label; // minimize year in 'Before' Column
            _this.barChartLabels[i] = label;
            // Data
            DataProvider[i] = { "category": _this.barChartLabels[i], "dataCol": _this.barChartData[0].data[i] };
            if (value > max)
                max = value;
            i++;
        });
        makeAmBarChart("GLAccountAgingChartChart2", Graphs, DataProvider, max);
    };
    Aging4CustomerChartWindowComponent = __decorate([
        core_1.Component({
            selector: 'Aging4CustomerChartWindowComponent',
            moduleId: module.id,
            templateUrl: './Aging4CustomerChartWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], Aging4CustomerChartWindowComponent);
    return Aging4CustomerChartWindowComponent;
}(BaseComponent_1.BaseComponent));
exports.Aging4CustomerChartWindowComponent = Aging4CustomerChartWindowComponent;
//# sourceMappingURL=Aging4CustomerChartWindowComponent.js.map