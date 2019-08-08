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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var QuoteUtilities_1 = require("../../../../Quote/Utilities/QuoteUtilities");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var RoutingsTabComponent = /** @class */ (function () {
    function RoutingsTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = null;
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
    }
    RoutingsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            var isInlandDomestic = QuoteUtilities_1.QuoteUtilities.IsInlandDomestic(this.EntityPM);
            if (isInlandDomestic) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteTabs/Components/Routings/InlandDomesticRoutingsComponent', this.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.InitTab(_this.EntityPM, _this.ObjectTableName);
                });
            }
            else {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./QuoteModules/QuoteTabs/Components/Routings/OrdinaryRoutingsComponent', this.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.InitTab(_this.EntityPM, _this.ObjectTableName);
                });
            }
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], RoutingsTabComponent.prototype, "viewContainerRef", void 0);
    RoutingsTabComponent = __decorate([
        core_1.Component({
            selector: 'RoutingsTabComponent',
            template: "\n        <div class=\"TabHolder\">\n            <table>\n                <tr>\n                    <td class=\"CellStretch\">\n                        <div class=\"CellContent\">\n                            <div #Child></div>\n                        </div>\n                    </td>\n                </tr>\n            </table>\n        </div>\n    ",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], RoutingsTabComponent);
    return RoutingsTabComponent;
}());
exports.RoutingsTabComponent = RoutingsTabComponent;
//# sourceMappingURL=RoutingsTabComponent.js.map