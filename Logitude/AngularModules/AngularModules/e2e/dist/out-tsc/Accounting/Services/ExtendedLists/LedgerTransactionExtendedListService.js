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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var LedgerTransactionList_1 = require("../../EntityLists/LedgerTransactionList");
var LedgerTransactionExtendedListService = /** @class */ (function () {
    function LedgerTransactionExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/LedgerTransactions';
        this._reconciliationUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReconciliationOp';
    }
    LedgerTransactionExtendedListService.prototype.GetFirstLedgerTransaction = function (AccountId) {
        var _this = this;
        var urlparameters = '/GetFirstLedgerTransaction?AccountId=' + AccountId;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                //console.log("serviceResponse: ", serviceResponse);
                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getByFilters = function (filters) {
        var _this = this;
        var urlparameters = '/GetLedgerTransactionsByFilters?';
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
                //console.log("serviceResponse: ", serviceResponse);
                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getBalanceByFilters = function (filters) {
        var _this = this;
        var urlparameters = '/GetTransactionsBalanceByFilters?';
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
                //console.log("serviceResponse: ", serviceResponse);
                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getOpenReconciliationsByFilter = function (accountId, filters) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._reconciliationUrl + "/GetOpenReconciliationsByFilters";
        var urlparameters = '?gLAccountId='
            + accountId + '&tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant; // Get Open Transaction by Itzik service , the get is inside post method
        // Parse Filters into URI
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var callTime = new Date();
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
        // End Parse
        var callUrl = url.concat(urlparameters);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                //serviceResponse.CallTime = callTime;
                serviceResponse = response.json();
                console.log("serviceResponse: ", serviceResponse);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getReconciliationsByFilter = function (accountId, filters) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._reconciliationUrl + "/GetReconciliationsByFilter";
        var urlparameters = '?gLAccountId='
            + accountId + '&tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant; // Get Open Transaction by Itzik service , the get is inside post method
        // Parse Filters into URI
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var callTime = new Date();
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
        // End Parse
        var callUrl = url.concat(urlparameters);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                //serviceResponse.CallTime = callTime;
                serviceResponse = response.json();
                console.log("serviceResponse: ", serviceResponse);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getAutomaticReconcileByFilter = function (method1, method2, method3, accountId, filters) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._reconciliationUrl + "/GetAutomaticReconcileByFilter";
        var urlparameters = '?gLAccountId=' + accountId
            + '&tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant
            + '&method1=' + method1
            + '&method2=' + method2
            + '&method3=' + method3; // Get Open Transaction by Itzik service , the get is inside post method
        //#region Parse Filters into URI
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
        //#endregion End Parse
        var callUrl = url.concat(urlparameters);
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
    LedgerTransactionExtendedListService.prototype.getLedgerTransactionsByIds = function (Ids) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var params = "";
        for (var _i = 0, Ids_1 = Ids; _i < Ids_1.length; _i++) {
            var id = Ids_1[_i];
            params += "Ids[]=" + id + "&";
        }
        var url = this._apiUrl + '/getLedgerTransactionsByIds?' + params;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getLast10TransactionsForAccount = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetLast10TransactionsForAccount?AccountId=' + accountId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.getTransactionsForARPayment = function (arpaymentId, billToGLAccountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetTransactionsForARPayment?arpaymentId=' + arpaymentId
            + '&billToGLAccountId=' + billToGLAccountId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LedgerTransactionExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new LedgerTransactionList_1.LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    LedgerTransactionExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], LedgerTransactionExtendedListService);
    return LedgerTransactionExtendedListService;
}());
exports.LedgerTransactionExtendedListService = LedgerTransactionExtendedListService;
//# sourceMappingURL=LedgerTransactionExtendedListService.js.map