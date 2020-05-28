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
var Tools_1 = require("../../Infrastructure/Tools");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var CardPM_1 = require("../EntityPMs/CardPM");
var AddressPM_1 = require("../EntityPMs/AddressPM");
var ContactPM_1 = require("../EntityPMs/ContactPM");
var CustomerPM_1 = require("../EntityPMs/CustomerPM");
var AirlinePM_1 = require("../EntityPMs/AirlinePM");
var TruckerPM_1 = require("../EntityPMs/TruckerPM");
var ShippingLinePM_1 = require("../EntityPMs/ShippingLinePM");
var TarrifHeaderPM_1 = require("../EntityPMs/TarrifHeaderPM");
var TarrifChargePM_1 = require("../EntityPMs/TarrifChargePM");
var TarrifFromToPM_1 = require("../EntityPMs/TarrifFromToPM");
var CardList_1 = require("../EntityLists/CardList");
var AirlineList_1 = require("../EntityLists/AirlineList");
var CustomerListService_1 = require("./StandardLists/CustomerListService");
var AgentPMService_1 = require("./StandardPMs/AgentPMService");
var CustomerPMService_1 = require("./StandardPMs/CustomerPMService");
var CustomAgentPMService_1 = require("./StandardPMs/CustomAgentPMService");
var ShippingAgentPMService_1 = require("./StandardPMs/ShippingAgentPMService");
var VendorPMService_1 = require("./StandardPMs/VendorPMService");
var WarehousePMService_1 = require("./StandardPMs/WarehousePMService");
var AirlinePMService_1 = require("./StandardPMs/AirlinePMService");
var TruckerPMService_1 = require("./StandardPMs/TruckerPMService");
var ShippingLinePMService_1 = require("./StandardPMs/ShippingLinePMService");
var CustomerSalesNotePM_1 = require("../EntityPMs/CustomerSalesNotePM");
var CardExternalAccountsByProductPMService_1 = require("./StandardPMs/CardExternalAccountsByProductPMService");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var PartnersDomainService = /** @class */ (function () {
    function PartnersDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/PartnersDomain';
    }
    PartnersDomainService.prototype.GetAllowedAirlineId = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllowedAirlineId';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetAirlineRules = function (myAirlineCode, myMessageCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var url = this._apiUrl + '/GetMessagingRulesForAirline?myAirlineCode=' + myAirlineCode + '&myMessageCode=' + myMessageCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapAirlineMessagingRuleList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetAllAddressesPMsbyCardId = function (myCardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllAddressesPMsbyCardId?myCardId=' + myCardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapAddressPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomerCardListByTenantVatNumber = function (vatNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerCardListByTenantVatNumber?vatNumber=' + vatNumber;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapCardList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomerActualData = function (customerId, year, month) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerActualData?customerId=' + customerId + '&year=' + year + '&month=' + month;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                return listJason;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomerSalesNotes = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerSalesNotes?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapCustomerSalesNotePM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetAllContactsPMsbyCardId = function (myCardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllContactsPMsbyCardId?myCardId=' + myCardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapContactPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetTarrifHeadersByCardIdAndTypeCode = function (cardId, typeCode, getAll) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTarrifHeadersByCardIdAndTypeCode?cardId=' + cardId + '&typeCode=' + typeCode + '&getAll=' + getAll;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapTarrifHeaderPM(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedListsArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetContactsByEmail = function (email) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetContactsByEmail?email=' + email;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapContactPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCardContactsByContact = function (contactId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCardContactsByContact?contactId=' + contactId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, {
                headers: authHeader
            }).map(function (response) {
                var listJason = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomerProducts = function (customerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerProducts?customerId=' + customerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, {
                headers: authHeader
            }).map(function (response) {
                var listJason = response.json();
                return listJason;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomerProductHistoryActualData = function (customerId, productTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerProductHistoryActualData?customerId=' + customerId + '&productTypeCode=' + productTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, {
                headers: authHeader
            }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.PostPartnerAddress = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToPartnerAddress(entityPM, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToPartnerAddress(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.Put = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToPartnerExternalAccounts(entityPM, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToPartnerExternalAccounts(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCarrierUpdate = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCarrierUpdate?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCarrierCopyToCurrentTenant = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCarrierCopyToCurrentTenant?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetIsCustomerConnectedToEntities = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetIsCustomerConnectedToEntities?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCardsForContact = function (contactId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCardsForContact?contactId=' + contactId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapCardPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetInUseCarrier = function (type, code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetInUseCarrier?type=' + type + '&code=' + code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetAirlineByPrefix = function (Prefix) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAirlineByPrefix?Prefix=' + Prefix;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult;
                if (myJsonResult) {
                    mappedResult = new AirlineList_1.AirlineList();
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
    PartnersDomainService.prototype.GetAirlineByCode = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAirlineByCode?code=' + code + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult;
                if (myJsonResult) {
                    mappedResult = new AirlinePM_1.AirlinePM();
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
    PartnersDomainService.prototype.GetAirlineByICAO = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAirlineByICAO?code=' + code + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetShippingLineByCode = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShippingLineByCode?code=' + code + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetTruckerByCode = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTruckerByCode?code=' + code + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetWarehouseByCode = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetWarehouseByCode?code=' + code + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetBillingAddressListByCardId = function (cardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBillingAddressListByCardId?cardId=' + cardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetMainAddressListByCardId = function (cardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMainAddressListByCardId?cardId=' + cardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetBillingOrMainAddressListByCardId = function (cardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBillingOrMainAddressListByCardId?cardId=' + cardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetAddressByCardAndType = function (cardId, type) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAddressByCardAndType?cardId=' + cardId + '&type=' + type;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomerById = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerById?id=' + id;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult;
                if (myJsonResult) {
                    mappedResult = new CustomerPM_1.CustomerPM();
                    var myCustomerPMService = new CustomerPMService_1.CustomerPMService();
                    mappedResult = myCustomerPMService.MapJsonToEntityPM(myJsonResult);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetIsVATUniqueForCustomer = function (vatNumber, customerId, countryId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetIsVATUniqueForCustomer?vatNumber=' + vatNumber + '&customerId=' + customerId + '&countryId=' + countryId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCardExternalAccountsByProducts = function (myCardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCardExternalAccountsByProducts?myCardId=' + myCardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetCustomersQuickSearch = function (SearchText) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersQuickSearch?SearchText=' + SearchText;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                var myService = new CustomerListService_1.CustomerListService();
                for (var key in allLists) {
                    var entity;
                    entity = myService.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedListsArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new CardList_1.CardList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    PartnersDomainService.prototype.MapCardPM = function (jsonList) {
        var entityPM = new CardPM_1.CardPM();
        if (jsonList) {
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    PartnersDomainService.prototype.MapCardList = function (jsonList) {
        var entityPM = new CardList_1.CardList();
        if (jsonList) {
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }
        return entityPM;
    };
    PartnersDomainService.prototype.MapAddressPM = function (jsonList) {
        var entityPM = null;
        if (jsonList) {
            entityPM = new AddressPM_1.AddressPM();
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }
                entityPM[property] = jsonList[property];
            }
            entityPM.IsDirty = false;
        }
        return entityPM;
    };
    PartnersDomainService.prototype.MapContactPM = function (jsonList) {
        var entityPM = null;
        if (jsonList) {
            entityPM = new ContactPM_1.ContactPM();
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }
                entityPM[property] = jsonList[property];
            }
            entityPM.IsDirty = false;
        }
        return entityPM;
    };
    PartnersDomainService.prototype.MapCustomerSalesNotePM = function (jsonList) {
        var entityPM = null;
        if (jsonList) {
            entityPM = new CustomerSalesNotePM_1.CustomerSalesNotePM(null);
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }
                entityPM[property] = jsonList[property];
            }
            entityPM.IsDirty = false;
        }
        return entityPM;
    };
    PartnersDomainService.prototype.MapTarrifHeaderPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new TarrifHeaderPM_1.TarrifHeaderPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldTarrifCharges = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldTarrifCharges = entityPM.OldEntityPM.TarrifCharges;
        }
        entityPM.TarrifCharges = new Array();
        for (var item in jsonPM.TarrifCharges) {
            var jItem = jsonPM.TarrifCharges[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTarrifChargePM;
            if (mapParent) {
                newTarrifChargePM = new TarrifChargePM_1.TarrifChargePM(entityPM);
            }
            else {
                newTarrifChargePM = new TarrifChargePM_1.TarrifChargePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTarrifChargePM[pmProperty] = jItem[pmProperty];
            }
            newTarrifChargePM.IsDirty = false;
            if (mapParent) {
                newTarrifChargePM.OldEntityPM = this.clone(newTarrifChargePM);
                newTarrifChargePM.UniqueKey = Guid_1.Guid.newGuid();
                newTarrifChargePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newTarrifChargePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newTarrifChargePM.ChangeSetOp = "Update";
                }
                else {
                    newTarrifChargePM.ChangeSetOp = "Insert";
                }
                newTarrifChargePM.OldEntityPM = null;
                newTarrifChargePM.EntityParentPM = null;
            }
            entityPM.TarrifCharges.push(newTarrifChargePM);
        }
        if (oldTarrifCharges) {
            for (var itemKey in oldTarrifCharges) {
                if (entityPM.TarrifCharges.filter(function (p) { return p.UniqueKey === oldTarrifCharges[itemKey].UniqueKey; }).length === 0) {
                    if (oldTarrifCharges[itemKey]) {
                        oldTarrifCharges[itemKey].ChangeSetOp = "Delete";
                        entityPM.TarrifCharges.push(oldTarrifCharges[itemKey]);
                    }
                }
            }
        }
        var oldTarrifFromToes = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldTarrifFromToes = entityPM.OldEntityPM.TarrifFromToes;
        }
        entityPM.TarrifFromToes = new Array();
        for (var item in jsonPM.TarrifFromToes) {
            var jItem = jsonPM.TarrifFromToes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTarrifFromToPM;
            if (mapParent) {
                newTarrifFromToPM = new TarrifFromToPM_1.TarrifFromToPM(entityPM);
            }
            else {
                newTarrifFromToPM = new TarrifFromToPM_1.TarrifFromToPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTarrifFromToPM[pmProperty] = jItem[pmProperty];
            }
            newTarrifFromToPM.IsDirty = false;
            if (mapParent) {
                newTarrifFromToPM.OldEntityPM = this.clone(newTarrifFromToPM);
                newTarrifFromToPM.UniqueKey = Guid_1.Guid.newGuid();
                newTarrifFromToPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newTarrifFromToPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newTarrifFromToPM.ChangeSetOp = "Update";
                }
                else {
                    newTarrifFromToPM.ChangeSetOp = "Insert";
                }
                newTarrifFromToPM.OldEntityPM = null;
                newTarrifFromToPM.EntityParentPM = null;
            }
            entityPM.TarrifFromToes.push(newTarrifFromToPM);
        }
        if (oldTarrifFromToes) {
            for (var itemKey in oldTarrifFromToes) {
                if (entityPM.TarrifFromToes.filter(function (p) { return p.UniqueKey === oldTarrifFromToes[itemKey].UniqueKey; }).length === 0) {
                    if (oldTarrifFromToes[itemKey]) {
                        oldTarrifFromToes[itemKey].ChangeSetOp = "Delete";
                        entityPM.TarrifFromToes.push(oldTarrifFromToes[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.TarrifCharges = [];
            for (var m in entityPM.TarrifCharges) {
                entityPM.OldEntityPM.TarrifCharges.push(this.clone(entityPM.TarrifCharges[m]));
            }
            entityPM.OldEntityPM.TarrifFromToes = [];
            for (var m in entityPM.TarrifFromToes) {
                entityPM.OldEntityPM.TarrifFromToes.push(this.clone(entityPM.TarrifFromToes[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    PartnersDomainService.prototype.MapAirlineMessagingRuleList = function (jsonList) {
        var entityList;
        entityList = new AirlineMessagingRuleList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    PartnersDomainService.prototype.MapJsonToPartnerAddress = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new PartnerServicePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else if (property === "Agent") {
                if (jsonPM[property]) {
                    var myAgentPMService = new AgentPMService_1.AgentPMService();
                    entity[property] = myAgentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Customer") {
                if (jsonPM[property]) {
                    var myCustomerPMService = new CustomerPMService_1.CustomerPMService();
                    entity[property] = myCustomerPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "CustomAgent") {
                if (jsonPM[property]) {
                    var myCustomAgentPMService = new CustomAgentPMService_1.CustomAgentPMService();
                    entity[property] = myCustomAgentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "ShippingAgent") {
                if (jsonPM[property]) {
                    var myShippingAgentPMService = new ShippingAgentPMService_1.ShippingAgentPMService();
                    entity[property] = myShippingAgentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Vendor") {
                if (jsonPM[property]) {
                    var myVendorPMService = new VendorPMService_1.VendorPMService();
                    entity[property] = myVendorPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Warehouse") {
                if (jsonPM[property]) {
                    var myWarehousePMService = new WarehousePMService_1.WarehousePMService();
                    entity[property] = myWarehousePMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Airline") {
                if (jsonPM[property]) {
                    var myAirlinePMService = new AirlinePMService_1.AirlinePMService();
                    entity[property] = myAirlinePMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Trucker") {
                if (jsonPM[property]) {
                    var myTruckerPMService = new TruckerPMService_1.TruckerPMService();
                    entity[property] = myTruckerPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "ShippingLine") {
                if (jsonPM[property]) {
                    var myShippingLinePMService = new ShippingLinePMService_1.ShippingLinePMService();
                    entity[property] = myShippingLinePMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Address") {
                if (jsonPM[property]) {
                    entity[property] = this.MapAddressPM(jsonPM[property]);
                }
            }
            else if (property === "Contact") {
                if (jsonPM[property]) {
                    entity[property] = this.MapContactPM(jsonPM[property]);
                }
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    PartnersDomainService.prototype.MapJsonToPartnerExternalAccounts = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new PartnerExternalAccountsServicePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else if (property === "Items") {
                var myPMService = new CardExternalAccountsByProductPMService_1.CardExternalAccountsByProductPMService();
                entityPM.Items = new Array();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];
                    var newItemPM;
                    newItemPM = myPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entityPM.Items.push(newItemPM);
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    PartnersDomainService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    PartnersDomainService.prototype.SetPartner = function (args, myPartner, partnerTypeId) {
        if (partnerTypeId === void 0) { partnerTypeId = null; }
        if (myPartner) {
            if (partnerTypeId) {
                args.PartnerTypeId = partnerTypeId;
            }
            else {
                args.PartnerTypeId = myPartner.PartnerTypeId;
                args.IsPartnerDirty = myPartner.IsDirty;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(args.PartnerTypeId)) {
                if (myPartner instanceof AirlinePM_1.AirlinePM) {
                    args.PartnerTypeId = "AL";
                }
                else if (myPartner instanceof TruckerPM_1.TruckerPM) {
                    args.PartnerTypeId = "TR";
                }
                else if (myPartner instanceof ShippingLinePM_1.ShippingLinePM) {
                    args.PartnerTypeId = "SL";
                }
            }
            switch (args.PartnerTypeId) {
                case "CS":
                case "PO": {
                    args.Customer = myPartner;
                    break;
                }
                case "AG": {
                    args.Agent = myPartner;
                    break;
                }
                case "CG": {
                    args.CustomAgent = myPartner;
                    break;
                }
                case "SG": {
                    args.ShippingAgent = myPartner;
                    break;
                }
                case "VD": {
                    args.Vendor = myPartner;
                    break;
                }
                case "WH": {
                    args.Warehouse = myPartner;
                    break;
                }
                case "AL": {
                    args.Airline = myPartner;
                    break;
                }
                case "SL": {
                    args.ShippingLine = myPartner;
                    break;
                }
                case "TR": {
                    args.Trucker = myPartner;
                    break;
                }
            }
        }
    };
    PartnersDomainService.prototype.GetRecentCustomers = function (ownerId, businessUnitId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRecentCustomers?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId;
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
    PartnersDomainService.prototype.GetCustomersCounts = function (ownerId, businessUnitId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;
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
    PartnersDomainService.prototype.GetCustomersDecreasedShipments = function (dataTypeCode, startDate, timeRange, ownerId, businessUnitId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersDecreasedShipments?dataTypeCode=' + dataTypeCode + '&startDate=' + ServiceHelper_1.ServiceHelper.GetDateString(startDate) + '&timeRange=' + timeRange + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapCompareDataClass(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.MapCompareDataClass = function (jsonList) {
        var entityList;
        entityList = new CompareDataClass();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    PartnersDomainService.prototype.GetAirlinesBySearchTextAndTenant = function (AWBMessagesCCSTypeCode, searchText, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAirlinesBySearchTextAndTenant?AWBMessagesCCSTypeCode=' + AWBMessagesCCSTypeCode + '&searchText=' + searchText + '&tenant=' + tenant;
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
    PartnersDomainService.prototype.AllowAirline = function (isAllowed, code, myTenantId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllowAirline?isAllowed=' + isAllowed + "&code=" + code + "&myTenantId=" + myTenantId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var done = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetIsDirect = function (forwarderTenantId, airlineTenantId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetIsDirect?forwarderTenantId=' + forwarderTenantId + "&airlineTenantId=" + airlineTenantId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var done = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.RegistrationRequested = function (isRequested, tenantAirlineId, zeroAirlineId, tenantManagmentId, AWBMessagesCCSTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRegistrationRequested?isRequested=' + isRequested + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var done = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.RegisteringAirline = function (isRegistered, tenantAirlineId, zeroAirlineId, tenantManagmentId, AWBMessagesCCSTypeCode, loggedContactName) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRegisteringAirline?isRegistered=' + isRegistered + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode + "&loggedContactName=" + loggedContactName;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var done = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.SetIsDirect = function (isDirect, tenantAirlineId, zeroAirlineId, tenantManagmentId, AWBMessagesCCSTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSetIsDirect?isDirect=' + isDirect + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var done = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.SetIsDeclined = function (isDeclined, declineNotes, tenantAirlineId, zeroAirlineId, tenantManagmentId, AWBMessagesCCSTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSetIsDeclined?isDeclined=' + isDeclined + "&declineNotes=" + declineNotes + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var done = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService.prototype.GetAirlinesForRequestedTenant = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var url = this._apiUrl + '/GetAirlinesForRequestedTenant?tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToAirlineList(allLists[key]);
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
    PartnersDomainService.prototype.MapJsonToAirlineList = function (jsonList) {
        var entityList;
        entityList = new AirlineList_1.AirlineList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    PartnersDomainService.prototype.GetAirlinesByFiltersAndTenant = function (filters, tenant) {
        var _this = this;
        var urlparameters = this._apiUrl + '/GetAirlinesByFiltersAndTenant?';
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
        urlparameters = urlparameters.concat("&tenant=" + tenant);
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(urlparameters, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToAirlineList(allLists[key]);
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
    PartnersDomainService.prototype.GetCustomerCreditLimitActualAmount = function (myCustomerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var myapiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var url = myapiUrl + '/GetCustomerCreditLimitActualAmount?myCustomerId=' + myCustomerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PartnersDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], PartnersDomainService);
    return PartnersDomainService;
}());
exports.PartnersDomainService = PartnersDomainService;
var AirlineMessagingRuleList = /** @class */ (function () {
    function AirlineMessagingRuleList() {
    }
    return AirlineMessagingRuleList;
}());
exports.AirlineMessagingRuleList = AirlineMessagingRuleList;
var PartnerServicePM = /** @class */ (function () {
    function PartnerServicePM() {
        this.AddressId = null;
        this.ContactId = null;
        this.PartnerId = null;
        this.PartnerTypeId = null;
        this.IsAddressDirty = false;
        this.IsContactDirty = false;
        this.IsPartnerDirty = false;
        this.Address = null;
        this.Contact = null;
        this.Agent = null;
        this.Customer = null;
        this.CustomAgent = null;
        this.ShippingAgent = null;
        this.Vendor = null;
        this.Warehouse = null;
        this.Airline = null;
        this.ShippingLine = null;
        this.Trucker = null;
    }
    return PartnerServicePM;
}());
exports.PartnerServicePM = PartnerServicePM;
var PartnerExternalAccountsServicePM = /** @class */ (function () {
    function PartnerExternalAccountsServicePM() {
        this.CardId = null;
        this.BusinessArea = null;
        this.ObjectTableName = null;
        this.ExternalId2 = null;
        this.Items = [];
    }
    return PartnerExternalAccountsServicePM;
}());
exports.PartnerExternalAccountsServicePM = PartnerExternalAccountsServicePM;
var CRMSummary = /** @class */ (function () {
    function CRMSummary() {
    }
    return CRMSummary;
}());
exports.CRMSummary = CRMSummary;
var CompareDataClass = /** @class */ (function () {
    function CompareDataClass() {
    }
    return CompareDataClass;
}());
exports.CompareDataClass = CompareDataClass;
//# sourceMappingURL=PartnersDomainService.js.map