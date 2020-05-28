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
var CertificateConnectedItem_1 = require("../../DataContract/CertificateConnectedItem");
var CertificateTicket_1 = require("../../DataContract/CertificateTicket");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var MultiCertificatesService = /** @class */ (function () {
    function MultiCertificatesService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/MultiCertificates';
    }
    MultiCertificatesService.prototype.GetCertificateConnectedItems = function (declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCertificateConnectedItems?' + 'declarationId=' + declarationId + '&attachmentTypeCode=' + attachmentTypeCode + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode + '&CertificateExemptionTypeCode=' + CertificateExemptionTypeCode + '&CertificateNumber=' + CertificateNumber + '&ResConfirmationTypeCode=' + ResConfirmationTypeCode, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToConnectedItem(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService.prototype.PutCertificateTickets = function (certificateTicket) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToCertificateTicket(certificateTicket, false);
            var jsonstr = JSON.stringify(mappedEntity);
            console.log(jsonstr);
            return _this._http.post(_this._apiUrl + '/PostCertificateTicket/', jsonstr, { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    mappedResult = _this.MapJsonToCertificateTicket(pm, true, certificateTicket);
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService.prototype.UpdateCertificatesBySearchFields = function (declarationId, invoiceCounterKey, externalRequestTypeCode, approvalRequestNumber, reqConfirmationTypeCode, attachmentTypeCode, certificateNumber, certificateExemptionTypeCode, resConfirmationTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetUpdateCertificatesBySearchFields?'
                + 'declarationId=' + declarationId
                + '&invoiceCounterKey=' + invoiceCounterKey
                + '&externalRequestTypeCode=' + externalRequestTypeCode
                + '&approvalRequestNumber=' + approvalRequestNumber
                + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode
                + '&attachmentTypeCode=' + attachmentTypeCode
                + '&certificateNumber=' + certificateNumber
                + '&certificateExemptionTypeCode=' + certificateExemptionTypeCode
                + '&resConfirmationTypeCode=' + resConfirmationTypeCode, { headers: authHeader }).map(function (response) {
                var result = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService.prototype.MapJsonToConnectedItem = function (json, mapParent, entity) {
        if (mapParent === void 0) { mapParent = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new CertificateConnectedItem_1.CertificateConnectedItem();
        }
        var jsonPMKeys = Object.keys(json);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }
        return entity;
    };
    MultiCertificatesService.prototype.MapJsonToCertificateTicket = function (json, mapParent, entity) {
        if (mapParent === void 0) { mapParent = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new CertificateTicket_1.CertificateTicket();
        }
        var jsonPMKeys = Object.keys(json);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }
        //  entity.IsDirty = false;
        return entity;
    };
    MultiCertificatesService.prototype.getByFilters = function (filters, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode) {
        var _this = this;
        var urlparameters = '/GetByFilters?';
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
            callUrl = callUrl + '&declarationId=' + declarationId + '&attachmentTypeCode=' + attachmentTypeCode + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode + '&CertificateExemptionTypeCode=' + CertificateExemptionTypeCode + '&CertificateNumber=' + CertificateNumber + '&ResConfirmationTypeCode=' + ResConfirmationTypeCode;
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToConnectedItem(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService.prototype.getCountByFilters = function (filters, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode) {
        var _this = this;
        var urlparameters = '/getCountByFilters?';
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
            callUrl = callUrl + '&declarationId=' + declarationId + '&attachmentTypeCode=' + attachmentTypeCode + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode + '&CertificateExemptionTypeCode=' + CertificateExemptionTypeCode + '&CertificateNumber=' + CertificateNumber + '&ResConfirmationTypeCode=' + ResConfirmationTypeCode;
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService.prototype.getPromiseByFilters = function (filters, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            resolve(_this.getByFilters(filters, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode));
        });
    };
    MultiCertificatesService.prototype.PutSupplierInvoiceItemCatalogNumber = function (certificateConnectedItem) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToConnectedItem(certificateConnectedItem, false);
            return _this._http.put(_this._apiUrl + '/PutSupplierInvoiceItemCatalogNumber/', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    mappedResult = _this.MapJsonToConnectedItem(pm, true, certificateConnectedItem);
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService.prototype.DeclarationHasInvoices = function (declarationId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            var callURL = _this._apiUrl + '/GetDeclarationHasInvoices?' + 'declarationId=' + declarationId;
            return _this._http.get(callURL, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MultiCertificatesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], MultiCertificatesService);
    return MultiCertificatesService;
}());
exports.MultiCertificatesService = MultiCertificatesService;
//# sourceMappingURL=MultiCertificatesService.js.map