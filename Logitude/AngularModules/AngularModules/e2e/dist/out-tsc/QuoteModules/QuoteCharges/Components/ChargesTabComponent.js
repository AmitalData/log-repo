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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var QuoteUtilities_1 = require("../../../Quote/Utilities/QuoteUtilities");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ChargesTabComponent = /** @class */ (function () {
    function ChargesTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.EntityPM = null;
        this.ObjectTableName = "Quote";
        this.isLCL = false;
        this.LoadCompletedEvent = null;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    ChargesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.isLCL = QuoteUtilities_1.QuoteUtilities.IsLCLQuote(_this.EntityPM);
                    _this.SelectTab();
                }
            });
        }
    };
    ChargesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ChargesTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("QuoteCharge").subscribe(function (res1) {
            _this.entityResourceService.getEntityResourceByTableName("QuotePriceSteps").subscribe(function (res2) {
                _this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe(function (res3) {
                    _this.isLCL = QuoteUtilities_1.QuoteUtilities.IsLCLQuote(_this.EntityPM);
                    _this.SelectTab();
                });
            });
        });
    };
    ChargesTabComponent.prototype.SelectTab = function () {
        this.viewContainerRef.clear();
        if (this.isLCL) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteCharges/Components/LCLChargesComponent', this.viewContainerRef)
                .then(function (cmpRef) {
            });
        }
        else {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteCharges/Components/FCLChargesComponent', this.viewContainerRef)
                .then(function (cmpRef) {
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ChargesTabComponent.prototype, "viewContainerRef", void 0);
    ChargesTabComponent = __decorate([
        core_1.Component({
            selector: 'ChargesTabComponent',
            template: "\n            <table>\n                <tr>\n                    <td>\n                        <div class=\"MediaFill\">\n                            <div #Child></div>\n                        </div>\n                    </td>\n                </tr>\n            </table>\n    ",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ChargesTabComponent);
    return ChargesTabComponent;
}());
exports.ChargesTabComponent = ChargesTabComponent;
//# sourceMappingURL=ChargesTabComponent.js.map