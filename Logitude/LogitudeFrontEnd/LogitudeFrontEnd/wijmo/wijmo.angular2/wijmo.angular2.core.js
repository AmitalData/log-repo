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
        var WjTooltip = (function () {
            function WjTooltip(elRef, injector) {
                this.elRef = elRef;
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
                if (!WjTooltip._toolTip) {
                    WjTooltip._toolTip = new wijmo.Tooltip();
                }
            }
            Object.defineProperty(WjTooltip.prototype, "wjTooltip", {
                get: function () {
                    return this._toolTipText;
                },
                set: function (value) {
                    if (this._toolTipText != value) {
                        this._toolTipText != value;
                        WjTooltip._toolTip.setTooltip(this.elRef.nativeElement, value);
                    }
                },
                enumerable: true,
                configurable: true
            });
            WjTooltip.prototype.ngOnDestroy = function () {
                this.wjTooltip = null;
            };
            __decorate([
                core_1.Input(), 
                __metadata('design:type', String)
            ], WjTooltip.prototype, "wjTooltip", null);
            WjTooltip = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    wjIsDirective: true,
                    wjMetadataId: wijmo.Tooltip,
                    selector: '[wjTooltip]',
                }),
                __param(0, core_1.Inject(core_1.ElementRef)),
                __param(1, core_1.Inject(core_1.Injector)), 
                __metadata('design:paramtypes', [core_1.ElementRef, core_1.Injector])
            ], WjTooltip);
            return WjTooltip;
        })();
        angular2.WjTooltip = WjTooltip;
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Core = wj.angular2;
//# sourceMappingURL=wijmo.angular2.core.js.map