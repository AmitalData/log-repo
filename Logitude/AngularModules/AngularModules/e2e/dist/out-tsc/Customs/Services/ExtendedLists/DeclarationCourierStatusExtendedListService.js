"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var DeclarationCourierStatusList_1 = require("../../EntityLists/DeclarationCourierStatusList");
var DeclarationCourierStatusExtendedListService = /** @class */ (function () {
    function DeclarationCourierStatusExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/declarationcourierstatusviews';
    }
    DeclarationCourierStatusExtendedListService.prototype.getByFilters = function (filters) {
        var _this = this;
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationCourierStatusExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new DeclarationCourierStatusList_1.DeclarationCourierStatusList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    DeclarationCourierStatusExtendedListService.CachedData = [];
    return DeclarationCourierStatusExtendedListService;
}());
exports.DeclarationCourierStatusExtendedListService = DeclarationCourierStatusExtendedListService;
//# sourceMappingURL=DeclarationCourierStatusExtendedListService.js.map