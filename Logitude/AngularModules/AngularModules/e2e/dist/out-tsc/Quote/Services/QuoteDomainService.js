"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var QuoteSettingPM_1 = require("../EntityPMs/QuoteSettingPM");
var QuoteStageList_1 = require("../EntityLists/QuoteStageList");
var QuotePMService_1 = require("./StandardPMs/QuotePMService");
var QuoteDomainService = /** @class */ (function () {
    function QuoteDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuoteDomain';
    }
    QuoteDomainService.prototype.GetQuotesCounts = function (ownerId, businessUnitId, directionId, transportModeId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&directionId=' + directionId + '&transportModeId=' + transportModeId + '&RecordsTypeCode=' + RecordsTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new CRMSummary();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetQuotesByOpportunityId = function (oportunityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesByOpportunityId?oportunityId=' + oportunityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.ConnectQuotesToOpportunity = function (oportunityId, quotesIds) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetConnectQuotesToOpportunity?opportunityId=' + oportunityId + '&quotesIds=' + quotesIds;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetRecentQuotes = function (ownerId, businessUnitId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRecentQuotes?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetDataCountsForCRM = function (tenant, customerid) {
        var _this = this;
        var _apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomersData';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = _apiUrl + '/GetDataCountsForCRM?customerId=' + customerid + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetStageFunnelData = function (ownerId, businessUnitId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetStageFunnelData?OwnerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse.Result;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetCRMMoneyInformation = function (tenant, customerid) {
        var _this = this;
        var _apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomersData';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = _apiUrl + '/GetCRMMoneyInformation?CRMMoneyCustomerId=' + customerid + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetSingleQuoteStageListByCode = function (code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSingleQuoteStageListByCode?code=' + code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult;
                if (myJsonResult) {
                    mappedResult = new QuoteStageList_1.QuoteStageList();
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.ComputeQuoteAutomaticSubject = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var iService = new QuotePMService_1.QuotePMService();
            var mappedEntity = iService.MapJsonToEntityPM(entityPM, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetActivitiesByQuoteId = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesByQuoteId?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetIsQuoteConnectedToShipment = function (quoteId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetIsQuoteConnectedToShipment?quoteId=' + quoteId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var result = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.GetQuoteSettings = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuoteSettings';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJSON = response.json();
                var itemMapped = _this.MapQuoteSettings(itemJSON);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.UpdateQuoteSettings = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapQuoteSettings(entityPM, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapQuoteSettings(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.MapQuoteSettings = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new QuoteSettingPM_1.QuoteSettingPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    QuoteDomainService.prototype.GetQuoteConnectedEntities = function (quoteId, opportunityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuoteConnectedEntities?quoteId=' + quoteId + '&opportunityId=' + opportunityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapQuoteConnectedEntity(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteDomainService.prototype.MapQuoteConnectedEntity = function (jsonList) {
        var entityList;
        entityList = new QuoteConnectedEntity();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    return QuoteDomainService;
}());
exports.QuoteDomainService = QuoteDomainService;
var CRMSummary = /** @class */ (function () {
    function CRMSummary() {
    }
    return CRMSummary;
}());
exports.CRMSummary = CRMSummary;
var QuoteConnectedEntity = /** @class */ (function () {
    function QuoteConnectedEntity() {
    }
    return QuoteConnectedEntity;
}());
exports.QuoteConnectedEntity = QuoteConnectedEntity;
//# sourceMappingURL=QuoteDomainService.js.map