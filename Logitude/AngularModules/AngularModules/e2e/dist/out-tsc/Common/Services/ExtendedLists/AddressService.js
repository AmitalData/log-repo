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
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var AddressList_1 = require("../../EntityLists/AddressList");
var AddressService = /** @class */ (function () {
    function AddressService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngAddress";
    }
    AddressService.prototype.GetMainAddressByCardId = function (cardId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAddressListByCardId?cardId=' + cardId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var itemJason = response.json();
                var itemMapped;
                if (itemJason) {
                    itemMapped = _this.MapAddressList(itemJason);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = itemMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AddressService.prototype.MapAddressList = function (jsonList) {
        var entityPM = null;
        if (jsonList) {
            entityPM = new AddressList_1.AddressList();
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }
        return entityPM;
    };
    AddressService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], AddressService);
    return AddressService;
}());
exports.AddressService = AddressService;
//# sourceMappingURL=AddressService.js.map