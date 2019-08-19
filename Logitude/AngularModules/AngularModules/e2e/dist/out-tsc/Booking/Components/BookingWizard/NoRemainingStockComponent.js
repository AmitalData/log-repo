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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var NoRemainingStockComponent = /** @class */ (function () {
    function NoRemainingStockComponent() {
        this.StockErrorMessage = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.PurchaseStockUri = "https://www.plimus.com/jsp/buynow.jsp?contractId=3233898&language=ENGLISH&currency=USD&custom1=" + SessionLocator_1.SessionLocator.Tenant + "&quantity=1";
        this.StockErrorMessage = "Your remaining stock is (0) which is insufficient for this operation. Please purchase another messaging stock via the link";
    }
    NoRemainingStockComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NoRemainingStockComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NoRemainingStockComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NoRemainingStockComponent);
    return NoRemainingStockComponent;
}());
exports.NoRemainingStockComponent = NoRemainingStockComponent;
//# sourceMappingURL=NoRemainingStockComponent.js.map