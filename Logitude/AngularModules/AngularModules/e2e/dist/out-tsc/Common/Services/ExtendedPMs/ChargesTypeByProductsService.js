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
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ChargesExternalAccountsByProductPMService_1 = require("../StandardPMs/ChargesExternalAccountsByProductPMService");
var ChargesTypeByProductsService = /** @class */ (function () {
    function ChargesTypeByProductsService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ChargesTypeByProducts';
    }
    ChargesTypeByProductsService.prototype.GetChargesTypeExternalAccountsByProducts = function (myChargesTypeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetChargesTypeExternalAccountsByProducts?myChargesTypeId=' + myChargesTypeId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ChargesTypeByProductsService.prototype.Put = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToChargesTypeByProductsControllerHelper(entityPM, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToChargesTypeByProductsControllerHelper(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ChargesTypeByProductsService.prototype.MapJsonToChargesTypeByProductsControllerHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ChargesTypeByProductsControllerHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else if (property === "Items") {
                var myPMService = new ChargesExternalAccountsByProductPMService_1.ChargesExternalAccountsByProductPMService();
                entityPM.Items = new Array();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];
                    var newItemPM;
                    newItemPM = myPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entityPM.Items.push(newItemPM);
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    ChargesTypeByProductsService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ChargesTypeByProductsService);
    return ChargesTypeByProductsService;
}());
exports.ChargesTypeByProductsService = ChargesTypeByProductsService;
var ChargesTypeByProductsControllerHelper = /** @class */ (function () {
    function ChargesTypeByProductsControllerHelper() {
        this.ChargesTypeId = null;
        this.Items = [];
    }
    return ChargesTypeByProductsControllerHelper;
}());
exports.ChargesTypeByProductsControllerHelper = ChargesTypeByProductsControllerHelper;
//# sourceMappingURL=ChargesTypeByProductsService.js.map