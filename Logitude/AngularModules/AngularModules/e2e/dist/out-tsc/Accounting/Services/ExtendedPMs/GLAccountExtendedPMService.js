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
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var GLAccountPM_1 = require("../../EntityPMs/GLAccountPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var GLAccountExtendedPMService = /** @class */ (function () {
    function GLAccountExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/GLAccountViews';
    }
    GLAccountExtendedPMService.prototype.GetSplittedByCurrencyGLAccounts = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSplittedByCurrencyGLAccounts?accountId=' + accountId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            });
        });
    };
    GLAccountExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityList;
        entityList = new GLAccountPM_1.GLAccountPM();
        var jsonListKeys = Object.keys(jsonPM);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonPM[property];
        }
        return entityList;
    };
    GLAccountExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], GLAccountExtendedPMService);
    return GLAccountExtendedPMService;
}());
exports.GLAccountExtendedPMService = GLAccountExtendedPMService;
//# sourceMappingURL=GLAccountExtendedPMService.js.map