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
var EntityPMServiceResponse_1 = require("../../../Infrastructure/DataContracts/EntityPMServiceResponse");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var DWQueryFilterPM_1 = require("../../EntityPMs/DWQueryFilterPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var DWQueryFilterPMService = /** @class */ (function () {
    function DWQueryFilterPMService() {
    }
    DWQueryFilterPMService.prototype.setServiceArgs = function (serviceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DWQueryFilter';
    };
    DWQueryFilterPMService.prototype.getDWqueryfiltersbytenant = function (tenant, userid) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getDWqueryfiltersbytenant?' + 'tenant=' + tenant + '&loggedcontactid=' + userid, {
                headers: authHeader
            }).map(function (response) {
                var pms = response.json();
                return pms;
            });
        });
    };
    DWQueryFilterPMService.prototype.getuserDWqueryfilterbytenantobjecttablequery = function (tenant, dwobjecttableid, dwqueryid, userid) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getDWqueryfiltersbytenantuserobjecttablequery?' + 'tenant=' + tenant + '&dwobjecttableid=' + dwobjecttableid + '&dwqueryid=' + dwqueryid + '&loggedcontactid=' + userid, {
                headers: authHeader
            }).map(function (response) {
                var pms = response.json();
                return pms;
            });
        });
    };
    DWQueryFilterPMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("DWQueryFilter", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                        response.Result = mappedResult;
                    }
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    DWQueryFilterPMService.prototype.delete = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("DWQueryFilter", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                        response.Result = mappedResult;
                    }
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    DWQueryFilterPMService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DWQueryFilterPM_1.DWQueryFilterPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        //entityPM.IsDirty = false;
        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entityPM;
    };
    DWQueryFilterPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DWQueryFilterPMService);
    return DWQueryFilterPMService;
}());
exports.DWQueryFilterPMService = DWQueryFilterPMService;
//# sourceMappingURL=DWQueryFilterPMService.js.map