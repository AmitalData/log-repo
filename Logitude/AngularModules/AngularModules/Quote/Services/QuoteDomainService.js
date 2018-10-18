"use strict";
var http_1 = require('@angular/http');
require('rxjs/add/operator/map');
require('rxjs/add/operator/catch');
var Rx_1 = require('rxjs/Rx');
var ServiceHelper_1 = require('../../Infrastructure/Utilities/ServiceHelper');
var ServiceResponse_1 = require('../../Infrastructure/DataContracts/ServiceResponse');
var QuoteStageList_1 = require('../EntityLists/QuoteStageList');
var QuoteDomainService = (function () {
    function QuoteDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuoteDomain';
    }
    QuoteDomainService.prototype.GetQuotesCounts = function (ownerId, businessUnitId, directionId, transportModeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&directionId=' + directionId + '&transportModeId=' + transportModeId;
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
    QuoteDomainService.prototype.GetStageFunnelData = function (ownerId, businessUnitId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetStageFunnelData?OwnerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
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
            var args = new QuoteSubjectArgs();
            args.EntityId = entityPM.Id;
            args.DirectionId = entityPM.DirectionId;
            args.TransportModeId = entityPM.TransportModeId;
            args.IncotermId = entityPM.IncotermId;
            args.FromPartnerAddressId = entityPM.FromPartnerAddressId;
            args.ToPartnerAddressId = entityPM.ToPartnerAddressId;
            args.IncludePickUp = entityPM.IncludePickUp;
            args.PickUpAddressId = entityPM.PickUpAddressId;
            args.FromAddressCity = entityPM.FromAddressCity;
            args.FromAddressZipCode = entityPM.FromAddressZipCode;
            args.FromPortId = entityPM.FromPortId;
            args.IncludeDelivery = entityPM.IncludeDelivery;
            args.DeliveryAddressId = entityPM.DeliveryAddressId;
            args.ToAddressCity = entityPM.ToAddressCity;
            args.ToAddressZipCode = entityPM.ToAddressZipCode;
            args.ToPortId = entityPM.ToPortId;
            var mappedEntity = _this.MapJsonToQuoteSubjectArgs(args, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToQuoteSubjectArgs(myJsonResult, true, args);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
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
    QuoteDomainService.prototype.MapJsonToQuoteSubjectArgs = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new QuoteSubjectArgs();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    return QuoteDomainService;
}());
exports.QuoteDomainService = QuoteDomainService;
var CRMSummary = (function () {
    function CRMSummary() {
    }
    return CRMSummary;
}());
exports.CRMSummary = CRMSummary;
var QuoteSubjectArgs = (function () {
    function QuoteSubjectArgs() {
    }
    return QuoteSubjectArgs;
}());
exports.QuoteSubjectArgs = QuoteSubjectArgs;
//# sourceMappingURL=QuoteDomainService.js.map