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
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../DataContracts/ServiceResponse");
var TextCodePMService_1 = require("./StandardPMs/TextCodePMService");
var DefaultTranslationService = /** @class */ (function () {
    function DefaultTranslationService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DefaultTranslation';
    }
    DefaultTranslationService.prototype.Post = function (args) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToDefaultTranslationAPIHelper(args, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToDefaultTranslationAPIHelper(myJsonResult, true, args);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DefaultTranslationService.prototype.MapJsonToDefaultTranslationAPIHelper = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new DefaultTranslationAPIHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        var myPMService = new TextCodePMService_1.TextCodePMService();
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "TextCodes") {
                entity.TextCodes = new Array();
                for (var item in jsonPM.TextCodes) {
                    var jItem = jsonPM.TextCodes[item];
                    var newItemPM = myPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entity.TextCodes.push(newItemPM);
                }
            }
            else if (property === "UpdatedTextCodes") {
                entity.UpdatedTextCodes = new Array();
                for (var item in jsonPM.UpdatedTextCodes) {
                    var jItem = jsonPM.UpdatedTextCodes[item];
                    var newItemPM = myPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entity.UpdatedTextCodes.push(newItemPM);
                }
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    DefaultTranslationService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DefaultTranslationService);
    return DefaultTranslationService;
}());
exports.DefaultTranslationService = DefaultTranslationService;
var DefaultTranslationAPIHelper = /** @class */ (function () {
    function DefaultTranslationAPIHelper() {
        this.TextCodes = [];
        this.UpdatedTextCodes = [];
    }
    return DefaultTranslationAPIHelper;
}());
exports.DefaultTranslationAPIHelper = DefaultTranslationAPIHelper;
//# sourceMappingURL=DefaultTranslationService.js.map