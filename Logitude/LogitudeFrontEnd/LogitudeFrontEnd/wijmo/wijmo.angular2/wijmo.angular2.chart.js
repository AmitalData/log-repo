var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
var core_1 = require('angular2/core');
var core_2 = require('angular2/core');
var wijmo_angular2_directiveBase_1 = require('./wijmo.angular2.directiveBase');
var wj;
(function (wj) {
    var angular2;
    (function (angular2) {
        'use strict';
        // WjFlexChart
        var WjFlexChart = (function (_super) {
            __extends(WjFlexChart, _super);
            function WjFlexChart(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            Object.defineProperty(WjFlexChart.prototype, "tooltipContent", {
                get: function () {
                    return this.tooltip.content;
                },
                set: function (value) {
                    this.tooltip.content = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(WjFlexChart.prototype, "labelContent", {
                get: function () {
                    return this.dataLabel.content;
                },
                set: function (value) {
                    this.dataLabel.content = value;
                },
                enumerable: true,
                configurable: true
            });
            WjFlexChart = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-chart',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexChart);
            return WjFlexChart;
        })(wijmo.chart.FlexChart);
        angular2.WjFlexChart = WjFlexChart;
        // WjFlexPie
        var WjFlexPie = (function (_super) {
            __extends(WjFlexPie, _super);
            function WjFlexPie(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            Object.defineProperty(WjFlexPie.prototype, "tooltipContent", {
                get: function () {
                    return this.tooltip.content;
                },
                set: function (value) {
                    this.tooltip.content = value;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(WjFlexPie.prototype, "labelContent", {
                get: function () {
                    return this.dataLabel.content;
                },
                set: function (value) {
                    this.dataLabel.content = value;
                },
                enumerable: true,
                configurable: true
            });
            WjFlexPie = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-pie',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexPie);
            return WjFlexPie;
        })(wijmo.chart.FlexPie);
        angular2.WjFlexPie = WjFlexPie;
        // WjFlexChartAxis
        var WjFlexChartAxis = (function (_super) {
            __extends(WjFlexChartAxis, _super);
            function WjFlexChartAxis(elRef, injector) {
                _super.call(this);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexChartAxis = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-chart-axis',
                    wjParentDirectives: [
                        WjFlexChart,
                        core_2.forwardRef(function () { return WjFlexChartSeries; }),
                    ]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexChartAxis);
            return WjFlexChartAxis;
        })(wijmo.chart.Axis);
        angular2.WjFlexChartAxis = WjFlexChartAxis;
        // WjFlexChartLegend
        var WjFlexChartLegend = (function (_super) {
            __extends(WjFlexChartLegend, _super);
            function WjFlexChartLegend(elRef, injector) {
                throw 'Legend: read-write property neded.';
                _super.call(this, null);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexChartLegend = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-chart-legend',
                    wjParentDirectives: [
                        WjFlexChart,
                        WjFlexPie,
                    ]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexChartLegend);
            return WjFlexChartLegend;
        })(wijmo.chart.Legend);
        angular2.WjFlexChartLegend = WjFlexChartLegend;
        // WjFlexChartDataLabel
        var WjFlexChartDataLabel = (function (_super) {
            __extends(WjFlexChartDataLabel, _super);
            function WjFlexChartDataLabel(elRef, injector) {
                _super.call(this);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexChartDataLabel = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-chart-data-label',
                    wjParentDirectives: [WjFlexChart]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexChartDataLabel);
            return WjFlexChartDataLabel;
        })(wijmo.chart.DataLabel);
        angular2.WjFlexChartDataLabel = WjFlexChartDataLabel;
        // WjFlexPieDataLabel
        var WjFlexPieDataLabel = (function (_super) {
            __extends(WjFlexPieDataLabel, _super);
            function WjFlexPieDataLabel(elRef, injector) {
                _super.call(this);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexPieDataLabel = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-pie-data-label',
                    wjParentDirectives: [WjFlexChart]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexPieDataLabel);
            return WjFlexPieDataLabel;
        })(wijmo.chart.PieDataLabel);
        angular2.WjFlexPieDataLabel = WjFlexPieDataLabel;
        // WjFlexChartSeries
        var WjFlexChartSeries = (function (_super) {
            __extends(WjFlexChartSeries, _super);
            function WjFlexChartSeries(elRef, injector) {
                _super.call(this);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexChartSeries = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-chart-series',
                    wjParentDirectives: [
                        WjFlexChart,
                    ]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexChartSeries);
            return WjFlexChartSeries;
        })(wijmo.chart.Series);
        angular2.WjFlexChartSeries = WjFlexChartSeries;
        // WjFlexChartLineMarker
        var WjFlexChartLineMarker = (function (_super) {
            __extends(WjFlexChartLineMarker, _super);
            function WjFlexChartLineMarker(elRef, injector) {
                var behavior = wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
                _super.call(this, behavior.parentBehavior.directive);
            }
            WjFlexChartLineMarker = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-line-marker',
                    wjParentDirectives: [
                        WjFlexChart,
                    ]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_2.ElementRef)),
                __param(1, core_1.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjFlexChartLineMarker);
            return WjFlexChartLineMarker;
        })(wijmo.chart.LineMarker);
        angular2.WjFlexChartLineMarker = WjFlexChartLineMarker;
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Chart = wj.angular2;
//# sourceMappingURL=wijmo.angular2.chart.js.map