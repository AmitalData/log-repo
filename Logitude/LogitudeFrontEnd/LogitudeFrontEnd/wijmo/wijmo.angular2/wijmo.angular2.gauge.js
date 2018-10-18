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
var wijmo_angular2_directiveBase_1 = require('./wijmo.angular2.directiveBase');
var wj;
(function (wj) {
    var angular2;
    (function (angular2) {
        'use strict';
        var WjLinearGauge = (function (_super) {
            __extends(WjLinearGauge, _super);
            function WjLinearGauge(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjLinearGauge = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-linear-gauge',
                }),
                core_1.View({
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_1.Inject(core_1.ElementRef)),
                __param(1, core_1.Inject(core_1.Injector)), 
                __metadata('design:paramtypes', [core_1.ElementRef, core_1.Injector])
            ], WjLinearGauge);
            return WjLinearGauge;
        })(wijmo.gauge.LinearGauge);
        angular2.WjLinearGauge = WjLinearGauge;
        var WjBulletGraph = (function (_super) {
            __extends(WjBulletGraph, _super);
            function WjBulletGraph(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjBulletGraph = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-bullet-graph',
                }),
                core_1.View({
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_1.Inject(core_1.ElementRef)),
                __param(1, core_1.Inject(core_1.Injector)), 
                __metadata('design:paramtypes', [core_1.ElementRef, core_1.Injector])
            ], WjBulletGraph);
            return WjBulletGraph;
        })(wijmo.gauge.BulletGraph);
        angular2.WjBulletGraph = WjBulletGraph;
        var WjRadialGauge = (function (_super) {
            __extends(WjRadialGauge, _super);
            function WjRadialGauge(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjRadialGauge = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-radial-gauge',
                }),
                core_1.View({
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_1.Inject(core_1.ElementRef)),
                __param(1, core_1.Inject(core_1.Injector)), 
                __metadata('design:paramtypes', [core_1.ElementRef, core_1.Injector])
            ], WjRadialGauge);
            return WjRadialGauge;
        })(wijmo.gauge.RadialGauge);
        angular2.WjRadialGauge = WjRadialGauge;
        var WjRange = (function (_super) {
            __extends(WjRange, _super);
            function WjRange(elRef, injector) {
                _super.call(this, null);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjRange = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-range',
                    wjParentDirectives: [WjLinearGauge, WjBulletGraph, WjRadialGauge]
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_1.Inject(core_1.ElementRef)),
                __param(1, core_1.Inject(core_1.Injector)), 
                __metadata('design:paramtypes', [core_1.ElementRef, core_1.Injector])
            ], WjRange);
            return WjRange;
        })(wijmo.gauge.Range);
        angular2.WjRange = WjRange;
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Gauge = wj.angular2;
//# sourceMappingURL=wijmo.angular2.gauge.js.map