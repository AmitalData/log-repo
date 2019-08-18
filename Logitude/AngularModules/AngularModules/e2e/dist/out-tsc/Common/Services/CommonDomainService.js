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
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var VatTypePercentagePM_1 = require("../EntityPMs/VatTypePercentagePM");
var VATTypesGroupPM_1 = require("../EntityPMs/VATTypesGroupPM");
var CurrencyListService_1 = require("../Services/StandardLists/CurrencyListService");
var UserLicensePM_1 = require("../EntityPMs/UserLicensePM");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var ComputingPartnerList_1 = require("../EntityLists/ComputingPartnerList");
var PerformanceLogger_1 = require("../../Infrastructure/Utilities/PerformanceLogger");
var VatTypePMService_1 = require("../Services/StandardPMs/VatTypePMService");
var FilingInboxPM_1 = require("../EntityPMs/FilingInboxPM");
var AccountingSettingPM_1 = require("../EntityPMs/AccountingSettingPM");
var CustomFieldClass_1 = require("../../Infrastructure/DataContracts/CustomFieldClass");
var CommonDomainService = /** @class */ (function () {
    function CommonDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
    }
    CommonDomainService.prototype.InvokeUpdateAutoDisplay = function (chargeTypeId, propertyTypeCode, isAutoDisplay) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateAutoDisplay?myChargeTypeId=' + chargeTypeId + '&myPropertyTypeCode=' + propertyTypeCode + '&isAutoDisplay=' + isAutoDisplay;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.InsertNewCurrency = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
    };
    CommonDomainService.prototype.GetBlueSnapToken = function (VaultedShopperId, countryName) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetBlueSnapToken?VaultedShopperId=' + VaultedShopperId + '&countryname=' + countryName, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetBlueSnapSecretToken = function (VaultedShopperId, countryName) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetBlueSnapSecretToken?VaultedShopperId=' + VaultedShopperId + '&countryname=' + countryName, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId = function (CustomerId, CustomerTenantAccessId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId?CustomerId=' + CustomerId + '&CustomerTenantAccessId=' + CustomerTenantAccessId, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetTranslationHeadersByTenant = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTranslationHeadersByTenant?Id=' + Id;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapTranslationHeaders(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                return _mappedListsArray;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.CopyCurrencyToTenant = function (CurrencyId, CurrencyRate, RateDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCopyCurrencyToTenant?CurrencyId=' + CurrencyId + '&CurrencyRate=' + CurrencyRate + '&RateDate=' + RateDate;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var list = response.json();
                var service = new CurrencyListService_1.CurrencyListService();
                var entity;
                if (list) {
                    entity = service.MapJsonToEntityList(list);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetPortCopyToCurrentTenant = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetPortCopyToCurrentTenant?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetCopyCommodityToTenant = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCopyCommodityToTenant?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetQuickSearch = function (ObjectTableName, SearchFields) {
        var _this = this;
        var callTime = new Date();
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuickSearch?ObjectTableName=' + ObjectTableName + '&SearchFields=' + SearchFields;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.CallTime = callTime;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetVatTypePercentagePMByDate = function (date) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetVatTypePercentagePMByDate?dateString=' + ServiceHelper_1.ServiceHelper.GetDateString(date);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToVatTypePercentagePM(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetAllVatTypesGroups = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllVatTypesGroups';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToVATTypesGroupPM(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetSingleVatTypeByCode = function (Code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSingleVatTypeByCode?Code=' + Code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResponse = response.json();
                var myService = new VatTypePMService_1.VatTypePMService();
                var myResult = myService.MapJsonToEntityPM(myResponse);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.MapJsonToVatTypePercentagePM = function (jsonList) {
        var entityList;
        entityList = new VatTypePercentagePM_1.VatTypePercentagePM(null);
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CommonDomainService.prototype.MapJsonToVATTypesGroupPM = function (jsonList) {
        var entityList;
        entityList = new VATTypesGroupPM_1.VATTypesGroupPM(null);
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CommonDomainService.prototype.MapTranslationHeaders = function (jsonList) {
        var entityList;
        entityList = new TranslationHeader();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CommonDomainService.prototype.GetContactsCounts = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetContactsCounts';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new ContactSummary();
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
    CommonDomainService.prototype.GetGettingStartedData = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetGettingStartedData';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.UpdateUserData = function (displayGettingStarted) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetUserGettingStartedData?displayGettingStarted=' + displayGettingStarted, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetDropBoxAuthURI = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDropBoxAuthURI?tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetDropBoxComLog = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDropBoxComLog?tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetDropBoxComLogTestFile = function (tenant, FileName, FolderName, FullText, ObjectTableId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDropBoxComLogTestFile?tenant=' + tenant + '&FileName=' + FileName + '&FolderName=' + FolderName + '&FullText=' + FullText + '&ObjectTableId=' + ObjectTableId, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetDropBoxAccessTocken = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDropBoxAccessTocken?tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                var myResult = response.json();
                if (myResult) {
                    var serviceResponse;
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = null;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetRedOfDropBoxAccessTocken = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetRedOfDropBoxAccessTocken?tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var myResult = response.json();
                if (myResult) {
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = null;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetDropBoxConnectionTest = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDropBoxConnectionTest?tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var myResult = response.json();
                if (myResult == "invalid_access_token") {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.Result = myResult;
                }
                return serviceResponse;
            });
        });
    };
    CommonDomainService.prototype.GetUpdateCustomerActualData = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetUpdateCustomerActualData?entityId=' + entityId, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetContactsByEmails = function (emails) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetContactsByEmails?emails=' + emails, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetUsersByEmails = function (emails) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetUsersByEmails?emails=' + emails, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetSingleCustomerTenantAccess = function (CustomerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleCustomerTenantAccess?CustomerId=' + CustomerId, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetUserListsByidsString = function (ids) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetUserListsByidsString?ids=' + ids, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetComputingPartnerTranslationsByPartnerAndTableId = function (ComputingPartnerId, ObjectTableId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetComputingPartnerTranslationsByPartnerAndTableId?ComputingPartnerId=' + ComputingPartnerId + '&ObjectTableId=' + ObjectTableId + '&tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetCustomerTenantAccessCard = function (CustomerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerTenantAccessCard?CustomerId=' + CustomerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetHybridTenantThresholdByIdTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetHybridTenantThresholdByIdTenant', {
                headers: authHeader
            }).map(function (response) {
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                var myResult = response.json();
                if (myResult) {
                    var serviceResponse;
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = null;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetCustomsInterfaceListByTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCustomsInterfaceListByTenant?', {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.UpdateUserLicense = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToUserLicenseUpdateHelper(entity, false);
            return _this._http.put(_this._apiUrl + "/PutUserLicenses", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToUserLicenseUpdateHelper(myJsonResult, true, entity);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.getNoneZeroTenantTranslation = function (computingPartnerId, ObjectTableId, Code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNoneZeroTenantTranslation?ComputingPartnerId=' + computingPartnerId + '&ObjectTableId=' + ObjectTableId + '&Code=' + Code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.getByFilters = function (filters) {
        var _this = this;
        var callTime = new Date();
        var urlparameters = '/GetByFiltersGrouping?';
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
        var _apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ComputingPartnerViewsExtended';
        var callUrl = _apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                //var _mappedListsArray: Array<ComputingPartnerList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {
                //        var entity: ComputingPartnerList;
                //        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);
                //    }
                //}
                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ComputingPartner", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.getByFiltersSingle = function (filters) {
        var _this = this;
        var callTime = new Date();
        var urlparameters = '/GetByFiltersGrouping?';
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
        var _apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ComputingPartnerViewsExtended';
        var callUrl = _apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                //var _mappedListsArray: Array<ComputingPartnerList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {
                //        var entity: ComputingPartnerList;
                //        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);
                //    }
                //}
                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ComputingPartner", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetDeafaultMyWarehouse = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDeafaultMyWarehouse', {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetSignRequestReceived = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSignRequestReceived?', {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new ComputingPartnerList_1.ComputingPartnerList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CommonDomainService.prototype.MapJsonToUserLicenseUpdateHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new UserLicenseUpdateHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else if (property === "Items") {
                entityPM.Items = new Array();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];
                    var newItemPM;
                    newItemPM = this.MapUserLicensePM(jItem);
                    entityPM.Items.push(newItemPM);
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    CommonDomainService.prototype.MapUserLicensePM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new UserLicensePM_1.UserLicensePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    CommonDomainService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CommonDomainService.prototype.MapTranslationItem = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new TranslationItem();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CommonDomainService.prototype.GetOnCreatingMexicanTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetOnCreatingMexicanTenant', {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetOnCreatingUSTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetOnCreatingUSTenant', {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.OnCreatingMoroccoTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetOnCreatingMoroccoTenant', {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.OnCreatingIsraelTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetOnCreatingIsraelTenant', { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var entity;
                if (myResult) {
                    entity = _this.MapJsonToAccountingSettingPM(myResult);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    // FilingInbox
    CommonDomainService.prototype.GetFilingInboxes = function (filters, userId, isShowDeleted) {
        //var authHeader = new Headers();
        //authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        //return Observable.defer(() => {
        //    return this._http.get(this._apiUrl + '/GetFilingInboxes?userId=' + userId + '&isShowDeleted=' + isShowDeleted, {
        //        headers: authHeader
        //    }).map(response => {
        //        var myResult = response.json();
        //        var serviceResponse: ServiceResponse;
        //        serviceResponse = new ServiceResponse();
        //        serviceResponse.Result = myResult;
        //        return serviceResponse;
        //    }).catch(ServiceHelper.HandleServiceError);
        //});
        var _this = this;
        var urlparameters = this._apiUrl + '/GetFilingInboxes?';
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
        urlparameters = urlparameters.concat("&userId=" + userId + '&isShowDeleted=' + isShowDeleted);
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(urlparameters, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse = response.json();
                //var _mappedListsArray: Array<FilingInboxPM> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {
                //        var entity: FilingInboxPM;
                //        entity = this.MapJsonToFilingInboxPM(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);
                //    }
                //}
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.MapJsonToFilingInboxPM = function (jsonList) {
        var entityList;
        entityList = new FilingInboxPM_1.FilingInboxPM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CommonDomainService.prototype.GetFilingAttachPdfReport = function (documentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetFilingAttachPdfReport?documentId=' + documentId, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.PutFilingInboxLogs = function (summary) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return _this._http.put(_this._apiUrl + '/PutFilingInboxLogs', JSON.stringify(summary), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetProductTypesByTenant = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetProductTypesByTenant?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var myList = response.json();
                return myList;
            });
        });
    };
    CommonDomainService.prototype.GetTenantLogoUri = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTenantLogoUri?tenant=' + Id;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.GetTenantEcommerceSupportEmail = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTenantEcommerceSupportEmail?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                //var entity: TenantPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService.prototype.MapJsonToAccountingSettingPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new AccountingSettingPM_1.AccountingSettingPM();
        }
        var customFields = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    CommonDomainService.prototype.GetAirlineAreas = function (airlineId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAirlineAreas?airlineId=' + airlineId, {
                headers: authHeader
            }).map(function (response) {
                var myResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CommonDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CommonDomainService);
    return CommonDomainService;
}());
exports.CommonDomainService = CommonDomainService;
var TranslationHeader = /** @class */ (function () {
    function TranslationHeader() {
    }
    return TranslationHeader;
}());
exports.TranslationHeader = TranslationHeader;
var ContactSummary = /** @class */ (function () {
    function ContactSummary() {
    }
    return ContactSummary;
}());
exports.ContactSummary = ContactSummary;
var HelpResource = /** @class */ (function () {
    function HelpResource() {
    }
    return HelpResource;
}());
exports.HelpResource = HelpResource;
var UserLicenseUpdateHelper = /** @class */ (function () {
    function UserLicenseUpdateHelper() {
        this.Items = [];
    }
    return UserLicenseUpdateHelper;
}());
exports.UserLicenseUpdateHelper = UserLicenseUpdateHelper;
var CustomApiQueryFilters = /** @class */ (function () {
    function CustomApiQueryFilters(getAll) {
        if (getAll === void 0) { getAll = false; }
        this.AdditionalFilters = [];
        this.ForceCacheRefresh = false;
        this.IsClosedTable = false;
        this.GetAll = getAll;
    }
    CustomApiQueryFilters.prototype.addAdditionalFilter = function (FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient) {
        if (IgnoreFilter === void 0) { IgnoreFilter = false; }
        if (IsCacheOnClient === void 0) { IsCacheOnClient = false; }
        if (!IsCacheOnClient) {
            if (typeof (FieldValue) === "string") {
                //var temp = encodeURIComponent("\"");
                FieldValue = encodeURIComponent(FieldValue);
                //FieldValue = FieldValue.replace("%22", "\%22");
            }
            if (typeof (FieldValue2) === "string") {
                FieldValue2 = encodeURIComponent(FieldValue2);
                //FieldValue = FieldValue.replace("%20", " ");
            }
            if (typeof (FieldValue3) === "string") {
                FieldValue3 = encodeURIComponent(FieldValue3);
                //FieldValue = FieldValue.replace("%20", " ");
            }
        }
        var existedItem = this.AdditionalFilters.find(function (d) { return d.FieldName == FieldName; });
        if (!existedItem) {
            var item = new ApiQueryFilters_1.FilterItem(FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient);
            this.AdditionalFilters.push(item);
        }
    };
    CustomApiQueryFilters.prototype.removeAdditionalFilter = function (FieldName) {
        var item = this.AdditionalFilters.filter(function (d) { return d.FieldName == FieldName; })[0];
        if (item) {
            var index = this.AdditionalFilters.indexOf(item);
            this.AdditionalFilters.splice(index, 1);
        }
    };
    return CustomApiQueryFilters;
}());
exports.CustomApiQueryFilters = CustomApiQueryFilters;
var TranslationItem = /** @class */ (function () {
    function TranslationItem() {
    }
    return TranslationItem;
}());
exports.TranslationItem = TranslationItem;
var FilingInboxSummary = /** @class */ (function () {
    function FilingInboxSummary() {
        this.Attaches = [];
    }
    return FilingInboxSummary;
}());
exports.FilingInboxSummary = FilingInboxSummary;
var FilingInboxAttachItem = /** @class */ (function () {
    function FilingInboxAttachItem() {
    }
    return FilingInboxAttachItem;
}());
exports.FilingInboxAttachItem = FilingInboxAttachItem;
//# sourceMappingURL=CommonDomainService.js.map