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
var Rx_1 = require("rxjs/Rx");
var PortList_1 = require("../../EntityLists/PortList");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var PortService = /** @class */ (function () {
    function PortService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/portlistviewsexteded';
        //this.CachedData = [];
    }
    PortService.prototype.setServiceArgs = function (serviceArgs) {
        //this._serviceArgs = serviceArgs;
        //this._http = serviceArgs.http;
        //this._apiUrl = logitude_url + 'api/portlistviewsexteded';
    };
    PortService.prototype.GetPortCopyToCurrentTenant = function (zeroPortId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getportcopytocurrenttenant/?' + 'id=' + zeroPortId, {
                headers: authHeader
            }).map(function (response) {
                var list = response.json();
                var entity;
                if (list) {
                    entity = _this.MapJsonToEntityList(list);
                }
                return entity;
            });
        });
    };
    PortService.prototype.getAll = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ngMetaData?tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant + '&inActive=false&inland=true&air=true&ocean=true', { headers: authHeader })
            .map(function (ports) { /*console.log(ports.json());*/ return ports.json(); });
    };
    PortService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new PortList_1.PortList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    PortService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], PortService);
    return PortService;
}());
exports.PortService = PortService;
//# sourceMappingURL=PortService.js.map