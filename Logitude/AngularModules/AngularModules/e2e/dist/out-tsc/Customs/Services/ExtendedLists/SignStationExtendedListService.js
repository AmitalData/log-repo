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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var SignStationExtendedListService = /** @class */ (function () {
    function SignStationExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        //CustomsRequestsSheetViewsController
        //CustomsSettingExtended
        //CustomsRequestsSheetViews
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/SignStationExtended';
    }
    SignStationExtendedListService.prototype.getSingle = function (declarationid, invoicecounterkey, lineNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle/?' + 'declarationid=' + declarationid + '&' + 'invoicecounterkey=' + invoicecounterkey + '&' + 'lineNumber=' + lineNumber, { headers: authHeader }).map(function (response) {
                var list = response.json();
                var entity;
                if (list) {
                    entity = _this.MapJsonToEntityList(list);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SignStationExtendedListService.prototype.getAll = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getall', { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SignStationExtendedListService.prototype.GetSignStationGroupByStatus = function (searchfields) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSignStationGroupByStatus?' + "&searchfields=" + searchfields, { headers: authHeader })
                .map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                //var _mappedListsArray: Array<SignStationGroup> = [];
                //if (serviceResponse.Result) {
                //    _mappedListsArray = serviceResponse.Result;
                //}
                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SignStationExtendedListService.prototype.getByFilters //(filters: ApiQueryFilters) {
     = function (skip, take, sortingCol, sortingDir, getCount, searchfields, FilterByStatus) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSignStations?' + "&skip=" + skip.toString() + "&take=" + take.toString() + "&sortingCol=" + sortingCol.toString() + "&sortingDir=" + sortingDir.toString() + "&searchfields=" + searchfields + "&FilterByStatus=" + FilterByStatus.toString(), { headers: authHeader })
                .map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                var _mappedListsArray = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {
                //        var entity: SignStationList;
                //        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);
                //    }
                //}
                if (serviceResponse.Result) {
                    _mappedListsArray = serviceResponse.Result;
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SignStationExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        return jsonList;
        //var entityList: SignStationList;
        //entityList = new SignStationList();
        //var jsonListKeys = Object.keys(jsonList);
        //for (var key in jsonListKeys) {
        //    var property = jsonListKeys[key];
        //    entityList[property] = jsonList[property];
        //}
        //return entityList;
    };
    SignStationExtendedListService.CachedData = [];
    SignStationExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], SignStationExtendedListService);
    return SignStationExtendedListService;
}());
exports.SignStationExtendedListService = SignStationExtendedListService;
//export class SignStationList {
//    public PersonId: string;
//    public SignerName: string;
//    public CustomsAgentId: string;
//    public MachineName: string;
//    public MachineUser: string;
//    public IsPersonalSignOn: boolean;
//    public IsCompanySignOn : boolean;
//    public Status: string;
//    public LastSignAt: Date;
//    public IsOk?: boolean;
//}
//# sourceMappingURL=SignStationExtendedListService.js.map