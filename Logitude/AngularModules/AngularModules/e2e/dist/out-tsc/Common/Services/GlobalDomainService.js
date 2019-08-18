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
require("rxjs/add/operator/map");
var Rx_1 = require("rxjs/Rx");
var TenantManagementList_1 = require("../../Infrastructure/EntityLists/TenantManagementList");
var BatchServicesDefinitionPM_1 = require("../../Infrastructure/EntityPMs/BatchServicesDefinitionPM");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var TenantManagementJS_1 = require("../../Infrastructure/DataContracts/TenantManagementJS");
var ObjectsUpdater_1 = require("../../Infrastructure/Locators/ObjectsUpdater");
var GlobalDomainService = /** @class */ (function () {
    function GlobalDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/GlobalDomain';
    }
    GlobalDomainService.prototype.GetMessagingStockTenantsList = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMessagingStockTenantsList?tenant=' + tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapTenantManagementList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlineVatTypesById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlineVatTypesById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlineReceivableChargesTypesById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlineReceivableChargesTypesById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlinePayablesChargesTypesById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlinePayablesChargesTypesById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlinePaymentMethodsById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlinePaymentMethodsById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlinePaymentTermsById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlinePaymentTermsById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlineCurrenciesById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlineCurrenciesById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlineCustomerById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlineCustomersById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksOnlineVendorById = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksOnlineVendorById?Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetAccountingSystem = function (AccountingSystemCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAccountingSystem?AccountingSystemCode=' + AccountingSystemCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResultJason = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResultJason;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.InvoiceToQuickBooks = function (invoiceId, id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetInvoiceToQuickBooks?Customerid=' + id + '&invoiceId=' + invoiceId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetQuickBooksQueries = function (args, SearchText) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetQuickBooksQueries?CardName=' + args.CardName + '&SearchField=' + args.SearchField + '&SearchText=' + SearchText + '&ReceivableCard=' + args.ReceivableCard + '&PayableCard=' + args.PayableCard + '&LogitudeCardName=' + args.LogitudeCardName, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.MapTenantManagementList = function (jsonList) {
        var entityList;
        entityList = new TenantManagementList_1.TenantManagementList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GlobalDomainService.prototype.GetAllHelpResources = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAllHelpResources?', {
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
    GlobalDomainService.prototype.GetAirlineTenantExistsForAirline = function (code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAirlineTenantExistsForAirline?code=' + code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResultJason = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResultJason;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetAllBatchServicesDefinitionsPMs = function (filterByDateCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllBatchServicesDefinitionsPMs?filterByDateCode=' + filterByDateCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapBatchServicesDefinitionPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.MapBatchServicesDefinitionPM = function (jsonList) {
        var entityList;
        entityList = new BatchServicesDefinitionPM_1.BatchServicesDefinitionPM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GlobalDomainService.prototype.UpdateTenantZeroService = function (Message) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateTenantZeroService?Message=' + Message;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJason = response.json();
                var itemMapped = itemJason;
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetParentTenants = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetParentTenants?';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapTenantManagementList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.GetTenantManagementJS = function (LoggedUserId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTenantManagementJS?loggeduserid=' + LoggedUserId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var iResultJson = response.json();
                var iResultMapped;
                if (iResultJson) {
                    iResultMapped = _this.MapTenantManagementJS(iResultJson);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = iResultMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GlobalDomainService.prototype.MapTenantManagementJS = function (jsonList) {
        var entityList = new TenantManagementJS_1.TenantManagementJS();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GlobalDomainService.prototype.UpdateTenantManagementJS = function (entityPM) {
        var myResult = new TenantManagementJS_1.TenantManagementJS();
        myResult.Id = entityPM.Id;
        myResult.Name = entityPM.Name;
        myResult.PackageCode = entityPM.PackageCode;
        myResult.PackageName = entityPM.PackageName;
        myResult.TTY = entityPM.TTY;
        myResult.PIMA = entityPM.PIMA;
        myResult.AWBMessagesCCSTypeCode = entityPM.AWBMessagesCCSTypeCode;
        myResult.IsAWBStockPrepaid = entityPM.IsAWBStockPrepaid;
        myResult.TrialStartDate = entityPM.TrialStartDate;
        myResult.TrialEndDate = entityPM.TrialEndDate;
        myResult.PaidUntilDate = entityPM.PaidUntilDate;
        myResult.PrivateLabelId = entityPM.PrivateLabelId;
        myResult.PaymentFailure = entityPM.PaymentFailure;
        myResult.SuspendDate = entityPM.SuspendDate;
        myResult.IsTrial = entityPM.IsTrial;
        myResult.IsRecurring = entityPM.IsRecurring;
        myResult.IsEAWBOnlyDemo = entityPM.IsEAWBOnlyDemo;
        myResult.IsRestrictedByAirline = entityPM.IsRestrictedByAirline;
        myResult.IsCargonautEnabled = entityPM.IsCargonautEnabled;
        myResult.IsDEXXConnectionEnabled = entityPM.IsDEXXConnectionEnabled;
        myResult.ManageLicencesPerUser = entityPM.ManageLicencesPerUser;
        myResult.ChangeHeaderColor = entityPM.ChangeHeaderColor;
        myResult.TrailDaysLeft = entityPM.TrailDaysLeft;
        myResult.PaidDaysLeft = entityPM.PaidDaysLeft;
        myResult.SuspendDaysLeft = entityPM.SuspendDaysLeft;
        myResult.NumberOfUsers = entityPM.NumberOfUsers;
        myResult.BluesnapContractId = entityPM.BluesnapContractId;
        myResult.BluesnapAccount = entityPM.BluesnapAccount;
        myResult.ManagesRegisteredAgent = entityPM.ManagesRegisteredAgent;
        myResult.IsINTTRAOnlyDemo = entityPM.IsINTTRAOnlyDemo;
        myResult.IsMultiPackage = entityPM.IsMultiPackage;
        myResult.TemporalPackageCode = entityPM.TemporalPackageCode;
        myResult.PackagesCodes_PK = entityPM.PackagesCodes_PK;
        myResult.PackagesCodes_BS = entityPM.PackagesCodes_BS;
        myResult.TenantManagementLicenses = entityPM.TenantManagementLicenses;
        ObjectsUpdater_1.ObjectsUpdater.UpdateTenantManagementJS(myResult);
    };
    GlobalDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], GlobalDomainService);
    return GlobalDomainService;
}());
exports.GlobalDomainService = GlobalDomainService;
//# sourceMappingURL=GlobalDomainService.js.map