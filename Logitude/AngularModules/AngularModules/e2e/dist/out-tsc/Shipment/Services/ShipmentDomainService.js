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
var MessagingStockList_1 = require("../EntityLists/MessagingStockList");
var ShipmentPMService_1 = require("./StandardPMs/ShipmentPMService");
var ShipmentList_1 = require("../EntityLists/ShipmentList");
var MessagingStockUsageHistoryList_1 = require("../EntityLists/MessagingStockUsageHistoryList");
var ShipmentDomainService = /** @class */ (function () {
    function ShipmentDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
    }
    ShipmentDomainService.prototype.GetShipmentsCounts = function (myDirectionId, myTransportModeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentsCounts?myDirectionId=' + myDirectionId + '&myTransportModeId=' + myTransportModeId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new ShipmentsSummary();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.CheckHousesOpenAmounts = function (masterId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/CheckHousesOpenAmounts?masterId=' + masterId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetRecentShipments = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRecentShipments';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetDeparturesArrivals = function (myDirectionId, myTransportModeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDeparturesArrivals?myDirectionId=' + myDirectionId + '&myTransportModeId=' + myTransportModeId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = [];
                for (var key in myJsonResult) {
                    var entity;
                    entity = _this.MapFlightSummary(myJsonResult[key]);
                    myResult.push(entity);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentCarrierStatuses = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentCarrierStatuses?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapShipmentCarrierStatus(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetLoggedTenantMessagingStockLists = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetLoggedTenantMessagingStockLists';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapMessagingStockList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetLoggedTenantMessagingStockUsageHistoryLists = function (stockId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetLoggedTenantMessagingStockUsageHistoryLists?stockId=' + stockId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapMessagingStockUsageHistoryList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.ValidateShipmentMasterFieldExistance = function (entityId, myBookingId, myMasterField, myAirlinePrefixField, myDirectionId, myTransportModeId, myShipmentLevelCode, isCancelled) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var args = new ValidateShipmentMasterArgs();
            args.ShipmentId = entityId;
            args.BookingId = myBookingId;
            args.Master = myMasterField;
            args.AirlinePrefix = myAirlinePrefixField;
            args.DirectionId = myDirectionId;
            args.TransportModeId = myTransportModeId;
            args.ShipmentLevelCode = myShipmentLevelCode;
            args.IsCancelled = isCancelled;
            var mappedEntity = _this.MapJsonToValidateShipmentMasterArgs(args, false);
            return _this._http.post(_this._apiUrl + "/PostValidateShipmentMasterArgs", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                return myJsonResult;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetMasterReceivables = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMasterReceivables?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetInvoiceOpenAmountReceivables = function (invoiceTypeCode, entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetInvoiceOpenAmountReceivables?invoiceTypeCode=' + invoiceTypeCode + '&entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentsQuotesCount = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentsQuotesCount?';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentConsolidationPackages = function (masterId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentConsolidationPackages?masterId=' + masterId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentConnectedEntities = function (shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentConnectedEntities?shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapShipmentConnectedEntity(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentsQueriesCounts = function (tenant, transportModeId, directionId, SearchFilter, serviceContextUser, TypeCode) {
        var _this = this;
        if (TypeCode === void 0) { TypeCode = null; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentsQueriesCounts?tenant=' + tenant + '&transportModeId=' + transportModeId + '&directionId=' + directionId + '&SearchFilter=' + SearchFilter + '&serviceContextUser=' + serviceContextUser + '&TypeCode=' + TypeCode, {
                headers: authHeader
            }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new ImporterQueriesDataCounts();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }
                return myResult;
            });
        });
    };
    ShipmentDomainService.prototype.GetAllMasterHousesPayables = function (allHousesIdsString) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllMasterHousesPayables?allHousesIdsString=' + allHousesIdsString;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetAllMasterHousesReceivables = function (allHousesIdsString) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllMasterHousesReceivables?allHousesIdsString=' + allHousesIdsString;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetConnectedShipmentsByMasterIdAndTenant = function (masterId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetConnectedShipmentsByMasterIdAndTenant?masterId=' + masterId + '&currentTenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                var service = new ShipmentPMService_1.ShipmentPMService();
                for (var itemJeson in listJason) {
                    var itemMapped = service.MapJsonToEntityPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentsCountByQuoteId = function (quoteId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentsCountByQuoteId?quoteId=' + quoteId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentLevelCode = function (myShipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentLevelCode?myShipmentId=' + myShipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetInvoiceOpenAmountPayables = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetInvoiceOpenAmountPayables?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetPayableInvoices = function (PayableId, PayableParentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetPayableInvoices?PayableId=' + PayableId + '&PayableParentId=' + PayableParentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentsByQuoteId = function (quoteId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentsByQuoteId?quoteId=' + quoteId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetSingleShipmentPMByNumber = function (shipmentNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', sessionStorage.getItem("Token"));
        var url = this._apiUrl + '/GetSingleShipmentPMByNumber?shipmentNumber=' + shipmentNumber;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetSingleShipmentPMWithoutComposition = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSingleShipmentPMWithoutComposition?id=' + id;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var service = new ShipmentPMService_1.ShipmentPMService();
                var shipment = service.MapJsonToEntityPM(myResult);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = shipment;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.BlockNewARInvoice = function (shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBlockNewARInvoice?shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentFullTextSearch = function (filters) {
        var _this = this;
        var urlparameters = this._apiUrl + '/GetShipmentFullTextSearch?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            if (!urlparameters.endsWith('?')) {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(urlparameters, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetMessagingStockListForTenantManagmentTab = function (tenantManagementId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMessagingStockListForTenantManagmentTab?tenantManagementId=' + tenantManagementId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetShipmentCustomsTransmissionByShipmnetId = function (shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentCustomsTransmissionByShipmnetId?shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.DisconnectQuote = function (shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDisconnectQuote?shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.MapFlightSummary = function (jsonList) {
        var entityList;
        entityList = new FlightSummary();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentDomainService.prototype.MapShipmentCarrierStatus = function (jsonList) {
        var entityList;
        entityList = new ShipmentCarrierStatusList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentDomainService.prototype.MapMessagingStockList = function (jsonList) {
        var entityList;
        entityList = new MessagingStockList_1.MessagingStockList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentDomainService.prototype.MapMessagingStockUsageHistoryList = function (jsonList) {
        var entityList;
        entityList = new MessagingStockUsageHistoryList_1.MessagingStockUsageHistoryList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentDomainService.prototype.MapJsonToValidateShipmentMasterArgs = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new ValidateShipmentMasterArgs();
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
    ShipmentDomainService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new ShipmentList_1.ShipmentList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentDomainService.prototype.SendToCustoms_AES = function (shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSendToAESCustoms?shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.GetArtemusStatus = function (shipmentNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetArtemusStatus?shipmentNumber=' + shipmentNumber;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.MapShipmentConnectedEntity = function (jsonList) {
        var entityList;
        entityList = new ShipmentConnectedEntity();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentDomainService.prototype.DownloadShipmentPackages = function (shipmentNumber, shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDownloadShipmentPackages?shipmentNumber=' + shipmentNumber + '&shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.CreateMissingMasters = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCreateMissingMasterData';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.CheckIfConnectedEntryOrRelease = function (shipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetIfConnectedEntryOrRelease?shipmentId=' + shipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService.prototype.PostUploadExcelFile = function (filter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + "/PostUploadExcelFile", JSON.stringify(filter), {
                headers: authHeader,
            }).map(function (response) {
                var result = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ShipmentDomainService);
    return ShipmentDomainService;
}());
exports.ShipmentDomainService = ShipmentDomainService;
var ShipmentsSummary = /** @class */ (function () {
    function ShipmentsSummary() {
    }
    return ShipmentsSummary;
}());
exports.ShipmentsSummary = ShipmentsSummary;
var FlightSummary = /** @class */ (function () {
    function FlightSummary() {
    }
    return FlightSummary;
}());
exports.FlightSummary = FlightSummary;
var ShipmentCarrierStatusList = /** @class */ (function () {
    function ShipmentCarrierStatusList() {
    }
    return ShipmentCarrierStatusList;
}());
exports.ShipmentCarrierStatusList = ShipmentCarrierStatusList;
var ValidateShipmentMasterArgs = /** @class */ (function () {
    function ValidateShipmentMasterArgs() {
    }
    return ValidateShipmentMasterArgs;
}());
exports.ValidateShipmentMasterArgs = ValidateShipmentMasterArgs;
var ImporterQueriesDataCounts = /** @class */ (function () {
    function ImporterQueriesDataCounts() {
    }
    return ImporterQueriesDataCounts;
}());
exports.ImporterQueriesDataCounts = ImporterQueriesDataCounts;
var ShipmentConnectedEntity = /** @class */ (function () {
    function ShipmentConnectedEntity() {
    }
    return ShipmentConnectedEntity;
}());
exports.ShipmentConnectedEntity = ShipmentConnectedEntity;
var ExcelPackageFilter = /** @class */ (function () {
    function ExcelPackageFilter() {
    }
    return ExcelPackageFilter;
}());
exports.ExcelPackageFilter = ExcelPackageFilter;
var ExcelPackage = /** @class */ (function () {
    function ExcelPackage() {
    }
    return ExcelPackage;
}());
exports.ExcelPackage = ExcelPackage;
//# sourceMappingURL=ShipmentDomainService.js.map