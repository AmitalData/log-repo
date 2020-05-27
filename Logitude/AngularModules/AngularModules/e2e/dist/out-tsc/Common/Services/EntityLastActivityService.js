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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var Rx_1 = require("rxjs/Rx");
var EntityLastActivityService = /** @class */ (function () {
    function EntityLastActivityService() {
    }
    EntityLastActivityService.prototype.setServiceArgs = function (serviceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/EntityLastActivity';
    };
    EntityLastActivityService.prototype.AddActivityLog = function (entityId, objectTableId, loggedContactId, logCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActivityLog?entityId=' + entityId + '&objectTableId=' + objectTableId + '&loggedContactId=' + loggedContactId + '&logCode=' + logCode, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                return myResult;
            });
        });
    };
    EntityLastActivityService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], EntityLastActivityService);
    return EntityLastActivityService;
}());
exports.EntityLastActivityService = EntityLastActivityService;
//# sourceMappingURL=EntityLastActivityService.js.map