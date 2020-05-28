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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var ShipmentPMService_1 = require("../Services/StandardPMs/ShipmentPMService");
var SplitShipmentService = /** @class */ (function () {
    function SplitShipmentService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/SplitShipment';
    }
    SplitShipmentService.prototype.Split = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapSplitShipmentHelper(entityPM, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapSplitShipmentHelper(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SplitShipmentService.prototype.MapSplitShipmentHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new SplitShipmentHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else if (property === "Shipment") {
                if (jsonPM[property]) {
                    var myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
                    entityPM[property] = myShipmentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "SplitPackages") {
                entityPM.SplitPackages = new Array();
                for (var item in jsonPM.SplitPackages) {
                    var jItem = jsonPM.SplitPackages[item];
                    var newItemPM = this.MapSplitPackage(jItem);
                    entityPM.SplitPackages.push(newItemPM);
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    SplitShipmentService.prototype.MapSplitPackage = function (jsonItem) {
        var entity = new SplitPackage();
        var jsonItemKeys = Object.keys(jsonItem);
        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entity[property] = jsonItem[property];
        }
        return entity;
    };
    SplitShipmentService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], SplitShipmentService);
    return SplitShipmentService;
}());
exports.SplitShipmentService = SplitShipmentService;
var SplitShipmentHelper = /** @class */ (function () {
    function SplitShipmentHelper() {
        this.SplitPackages = [];
    }
    return SplitShipmentHelper;
}());
exports.SplitShipmentHelper = SplitShipmentHelper;
var SplitPackage = /** @class */ (function () {
    function SplitPackage() {
    }
    return SplitPackage;
}());
exports.SplitPackage = SplitPackage;
//# sourceMappingURL=SplitShipmentService.js.map