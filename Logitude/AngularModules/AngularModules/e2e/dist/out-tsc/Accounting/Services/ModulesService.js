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
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ModulesService = /** @class */ (function () {
    function ModulesService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
    }
    ModulesService.prototype.GetAccountPayablesSummary = function () {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAccountPayablesSummary', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    ModulesService.prototype.GetAccountingReceivablesSummary = function () {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAccountingReceivablesSummary', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    ModulesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ModulesService);
    return ModulesService;
}());
exports.ModulesService = ModulesService;
//# sourceMappingURL=ModulesService.js.map