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
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var Tools_1 = require("../../Infrastructure/Tools");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var ARPaymentPM_1 = require("../EntityPMs/ARPaymentPM");
var ARInvoicePM_1 = require("../EntityPMs/ARInvoicePM");
var ARInvoiceLinePM_1 = require("../EntityPMs/ARInvoiceLinePM");
var ARInvoiceEntityPM_1 = require("../EntityPMs/ARInvoiceEntityPM");
var ARInvoicePaymentPM_1 = require("../EntityPMs/ARInvoicePaymentPM");
var ARInvoiceTransferHistoryPM_1 = require("../EntityPMs/ARInvoiceTransferHistoryPM");
var ConstituentPM_1 = require("../EntityPMs/ConstituentPM");
var ARPaymentInvoicePM_1 = require("../EntityPMs/ARPaymentInvoicePM");
var APInvoiceMultipleShortPM_1 = require("../EntityPMs/APInvoiceMultipleShortPM");
var APInvoiceLinePM_1 = require("../EntityPMs/APInvoiceLinePM");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var InvoiceDomainService = /** @class */ (function () {
    function InvoiceDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
    }
    InvoiceDomainService.prototype.GetAccountingReceivablesSummary = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAccountingReceivablesSummary', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    InvoiceDomainService.prototype.GetAccountPayablesSummary = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAccountPayablesSummary', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    InvoiceDomainService.prototype.GetAccountingTransferSummary = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAccountingTransferSummary', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            });
        });
    };
    InvoiceDomainService.prototype.GetMoneyStatusForTenant = function (months, days, tenant, index, currency) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetMoneyStatusForTenant?months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    InvoiceDomainService.prototype.GetDebrotExposure = function (tenant, currency) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDebrotExposure?tenant=' + tenant + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    InvoiceDomainService.prototype.GetDebrotExposureForGridControl = function (index) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDebrotExposureForGridControl?index=' + index, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            });
        });
    };
    InvoiceDomainService.prototype.GetCreditorExposure = function (index) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCreditorExposure?index=' + index, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            });
        });
    };
    InvoiceDomainService.prototype.GetAgingReportARInvioceData = function (index, customerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAgingReportARInvioceData?index=' + index + '&customerId=' + customerId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            });
        });
    };
    InvoiceDomainService.prototype.GetAgingReportAPInvioceData = function (index) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAgingReportAPInvioceData?index=' + index, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            });
        });
    };
    InvoiceDomainService.prototype.ValidateARPaymentFullAccounting = function (paymentMethod, currency, billTo, code, registergdate, bankAccountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetARPaymentValidatingList?paymentMethod=' + paymentMethod + "&currency=" + currency + "&billTo=" + billTo + "&code=" + code + "&registergDateString=" + ServiceHelper_1.ServiceHelper.GetDateString(registergdate) + "&bankAccountId=" + bankAccountId, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.ValidateAPInvoiceFullAccounting = function (currency, vendor, accountingdate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAPInvoiceValidatingList?currency=' + currency + "&vendor=" + vendor + "&accountingDateString=" + ServiceHelper_1.ServiceHelper.GetDateString(accountingdate), {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.PostARPaymentChequeAndCashBook = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity;
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            mappedEntity = _this.MapARPaymentJsonToEntityPM(entityPM, false);
            return _this._http.post(_this._apiUrl + '/PostARPaymentChequeAndCashBook', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapARPaymentJsonToEntityPM(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.PostARInvoiceJournalAndJournalLines = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity;
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            mappedEntity = _this.MapARInvoiceJsonToEntityPM(entityPM, false);
            return _this._http.post(_this._apiUrl + '/PostARInvoiceJournalAndJournalLines', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapARInvoiceJsonToEntityPM(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.ValidateARInvoiceFullAccounting = function (currency, billTo, accountingdate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetARInvoiceValidatingList?currency=' + currency + "&billTo=" + billTo + "&accountingDateString=" + ServiceHelper_1.ServiceHelper.GetDateString(accountingdate), {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.IsARInvoiceNumberExists = function (InvoiceNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetIsARInvoiceNumberExists?InvoiceNumber=' + InvoiceNumber;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var isExists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = isExists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.AutoCreditARInvoice = function (entityId, IsInvoiceNumberManuallySet, AutoCreditManualNumber, AutoCreditDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAutoCreditARInvoice?entityId=' + entityId + "&IsInvoiceNumberManuallySet=" + IsInvoiceNumberManuallySet + "&AutoCreditManualNumber=" + AutoCreditManualNumber + "&AutoCreditDateString=" + ServiceHelper_1.ServiceHelper.GetDateString(AutoCreditDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var newInvoiceId = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = newInvoiceId;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.CheckVendor_NumberDuplication = function (vendorId, invoiceNumber, entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var args = new APInvoiceNumberDuplicationCheckArgs();
        args.VendorId = vendorId;
        args.EntityId = entityId;
        args.InvoiceNumber = invoiceNumber;
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl, JSON.stringify(args), { headers: authHeader }).map(function (response) {
                var newInvoiceId = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = newInvoiceId;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.CheckARPaymentCashBook = function (paymentMethod, currency, branch) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetARPaymentCashBook?paymentMethod=' + paymentMethod + "&currency=" + currency + "&branch=" + branch, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetCustomerCreditLimitActualAmount = function (myCustomerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerCreditLimitActualAmount?myCustomerId=' + myCustomerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetSingleAPInvoiceShortPM = function (myInvoiceId, myShipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSingleAPInvoiceShortPM?myInvoiceId=' + myInvoiceId + "&myShipmentId=" + myShipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToAPInvoiceMultipleShortPM(pm);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.PutSingleAPInvoiceShortPM = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToAPInvoiceMultipleShortPM(entityPM, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    mappedResult = _this.MapJsonToAPInvoiceMultipleShortPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.RebuildTransferFile = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRebuildTransferFile?entityId=' + entityId;
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
    InvoiceDomainService.prototype.GetNotReadyARInvoicesIds = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNotReadyARInvoicesIds';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetNotReadyAPInvoicesIds = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNotReadyAPInvoicesIds';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetNotReadyARPaymentsIds = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNotReadyARPaymentIds';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetNotReadyAPPaymentsIds = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNotReadyAPPaymentIds';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetRecalculateTransfer = function (ids, entityCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRecalculateTransfer?allIdsString=' + Tools_1.AppTool.GetIdsArrayText(ids) + "&entityCode=" + entityCode;
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
    InvoiceDomainService.prototype.BlockTransferEntities = function (ids, entityCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBlockForTransfer?allIdsString=' + Tools_1.AppTool.GetIdsArrayText(ids) + "&entityCode=" + entityCode;
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
    InvoiceDomainService.prototype.SetAccountingSettingStartDate = function (entityCode, myStartDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var myStartDateString = ServiceHelper_1.ServiceHelper.GetDateString(myStartDate);
        var url = this._apiUrl + '/GetSetAccountingSettingStartDate?entityCode=' + entityCode + "&myStartDateString=" + myStartDateString;
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
    InvoiceDomainService.prototype.SendARPaymentSATXML = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSendARPaymentSATXML?paymentId=' + id;
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
    InvoiceDomainService.prototype.GetARInvoiceSATStatus = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetARInvoiceSATStatus?paymentId=' + id;
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
    InvoiceDomainService.prototype.GetOnStartDateEntitiesIds = function (entityCode, myStartDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var myStartDateString = ServiceHelper_1.ServiceHelper.GetDateString(myStartDate);
        var url = this._apiUrl + '/GetOnStartDateEntitiesIds?entityCode=' + entityCode + "&myStartDateString=" + myStartDateString;
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
    InvoiceDomainService.prototype.GetSingleChargeTypeAccountingList = function (chargesTypeId, vatTypeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSingleChargeTypeAccountingList?chargesTypeId=' + chargesTypeId + "&vatTypeId=" + vatTypeId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.PrintTaxData = function (startDate, endDate, email) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var startDateString = ServiceHelper_1.ServiceHelper.GetDateString(startDate);
        var endDateString = ServiceHelper_1.ServiceHelper.GetDateString(endDate);
        var url = this._apiUrl + '/GetTaxApprovalData?startDateString=' + startDateString + "&endDateString=" + endDateString + "&email=" + email;
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
    InvoiceDomainService.prototype.GetShipmentLevelCode = function (myShipmentId) {
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
    InvoiceDomainService.prototype.GetShipmentIsAccountingClosed = function (myShipmentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentIsAccountingClosed?myShipmentId=' + myShipmentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetARPaymentSATCancellationStatus = function (paymentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetARPaymentSATCancellationStatus?paymentId=' + paymentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetStatusOfARPaymentCheques = function (paymentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetStatusOfARPaymentCheques?paymentId=' + paymentId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetARInvoiceSATCancellationStatus = function (invoiceId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetARInvoiceSATCancellationStatus?invoiceId=' + invoiceId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.getConnectedARPayments = function (invoiceId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/getConnectedARPayments?invoiceId=' + invoiceId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.getConnectedAPPayments = function (invoiceId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/getConnectedAPPayments?invoiceId=' + invoiceId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.GetListOfARInvoiceStockPM = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetListOfARInvoiceStockPM?';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InvoiceDomainService.prototype.MapARPaymentJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ARPaymentPM_1.ARPaymentPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldPaymentInvoices = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPaymentInvoices = entityPM.OldEntityPM.PaymentInvoices;
        }
        entityPM.PaymentInvoices = new Array();
        for (var item in jsonPM.PaymentInvoices) {
            var jItem = jsonPM.PaymentInvoices[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARPaymentInvoicePM;
            if (mapParent) {
                newARPaymentInvoicePM = new ARPaymentInvoicePM_1.ARPaymentInvoicePM(entityPM);
            }
            else {
                newARPaymentInvoicePM = new ARPaymentInvoicePM_1.ARPaymentInvoicePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARPaymentInvoicePM[pmProperty] = jItem[pmProperty];
            }
            newARPaymentInvoicePM.IsDirty = false;
            if (mapParent) {
                newARPaymentInvoicePM.OldEntityPM = this.clone(newARPaymentInvoicePM);
                newARPaymentInvoicePM.UniqueKey = Guid_1.Guid.newGuid();
                newARPaymentInvoicePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newARPaymentInvoicePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newARPaymentInvoicePM.ChangeSetOp = "Update";
                }
                else {
                    newARPaymentInvoicePM.ChangeSetOp = "Insert";
                }
                newARPaymentInvoicePM.OldEntityPM = null;
                newARPaymentInvoicePM.EntityParentPM = null;
            }
            entityPM.PaymentInvoices.push(newARPaymentInvoicePM);
        }
        if (oldPaymentInvoices) {
            for (var itemKey in oldPaymentInvoices) {
                if (entityPM.PaymentInvoices.filter(function (p) { return p.UniqueKey === oldPaymentInvoices[itemKey].UniqueKey; }).length === 0) {
                    if (oldPaymentInvoices[itemKey]) {
                        oldPaymentInvoices[itemKey].ChangeSetOp = "Delete";
                        entityPM.PaymentInvoices.push(oldPaymentInvoices[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.PaymentInvoices = [];
            for (var m in entityPM.PaymentInvoices) {
                entityPM.OldEntityPM.PaymentInvoices.push(this.clone(entityPM.PaymentInvoices[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    InvoiceDomainService.prototype.MapARInvoiceJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ARInvoicePM_1.ARInvoicePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldInvoiceLines = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoiceLines = entityPM.OldEntityPM.InvoiceLines;
        }
        entityPM.InvoiceLines = new Array();
        for (var item in jsonPM.InvoiceLines) {
            var jItem = jsonPM.InvoiceLines[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoiceLinePM;
            if (mapParent) {
                newARInvoiceLinePM = new ARInvoiceLinePM_1.ARInvoiceLinePM(entityPM);
            }
            else {
                newARInvoiceLinePM = new ARInvoiceLinePM_1.ARInvoiceLinePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoiceLinePM[pmProperty] = jItem[pmProperty];
            }
            newARInvoiceLinePM.IsDirty = false;
            if (mapParent) {
                newARInvoiceLinePM.OldEntityPM = this.clone(newARInvoiceLinePM);
                newARInvoiceLinePM.UniqueKey = Guid_1.Guid.newGuid();
                newARInvoiceLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newARInvoiceLinePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newARInvoiceLinePM.ChangeSetOp = "Update";
                }
                else {
                    newARInvoiceLinePM.ChangeSetOp = "Insert";
                }
                newARInvoiceLinePM.OldEntityPM = null;
                newARInvoiceLinePM.EntityParentPM = null;
            }
            entityPM.InvoiceLines.push(newARInvoiceLinePM);
        }
        if (oldInvoiceLines) {
            for (var itemKey in oldInvoiceLines) {
                if (entityPM.InvoiceLines.filter(function (p) { return p.UniqueKey === oldInvoiceLines[itemKey].UniqueKey; }).length === 0) {
                    if (oldInvoiceLines[itemKey]) {
                        oldInvoiceLines[itemKey].ChangeSetOp = "Delete";
                        entityPM.InvoiceLines.push(oldInvoiceLines[itemKey]);
                    }
                }
            }
        }
        var oldInvoiceEntities = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoiceEntities = entityPM.OldEntityPM.InvoiceEntities;
        }
        entityPM.InvoiceEntities = new Array();
        for (var item in jsonPM.InvoiceEntities) {
            var jItem = jsonPM.InvoiceEntities[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoiceEntityPM;
            if (mapParent) {
                newARInvoiceEntityPM = new ARInvoiceEntityPM_1.ARInvoiceEntityPM();
            }
            else {
                newARInvoiceEntityPM = new ARInvoiceEntityPM_1.ARInvoiceEntityPM();
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoiceEntityPM[pmProperty] = jItem[pmProperty];
            }
            newARInvoiceEntityPM.IsDirty = false;
            if (mapParent) {
                newARInvoiceEntityPM.OldEntityPM = this.clone(newARInvoiceEntityPM);
                newARInvoiceEntityPM.UniqueKey = Guid_1.Guid.newGuid();
                newARInvoiceEntityPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newARInvoiceEntityPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newARInvoiceEntityPM.ChangeSetOp = "Update";
                }
                else {
                    newARInvoiceEntityPM.ChangeSetOp = "Insert";
                }
                newARInvoiceEntityPM.OldEntityPM = null;
                newARInvoiceEntityPM.EntityParentPM = null;
            }
            entityPM.InvoiceEntities.push(newARInvoiceEntityPM);
        }
        if (oldInvoiceEntities) {
            for (var itemKey in oldInvoiceEntities) {
                if (entityPM.InvoiceEntities.filter(function (p) { return p.UniqueKey === oldInvoiceEntities[itemKey].UniqueKey; }).length === 0) {
                    if (oldInvoiceEntities[itemKey]) {
                        oldInvoiceEntities[itemKey].ChangeSetOp = "Delete";
                        entityPM.InvoiceEntities.push(oldInvoiceEntities[itemKey]);
                    }
                }
            }
        }
        var oldInvoicePayments = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoicePayments = entityPM.OldEntityPM.InvoicePayments;
        }
        entityPM.InvoicePayments = new Array();
        for (var item in jsonPM.InvoicePayments) {
            var jItem = jsonPM.InvoicePayments[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoicePaymentPM;
            if (mapParent) {
                newARInvoicePaymentPM = new ARInvoicePaymentPM_1.ARInvoicePaymentPM(entityPM);
            }
            else {
                newARInvoicePaymentPM = new ARInvoicePaymentPM_1.ARInvoicePaymentPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoicePaymentPM[pmProperty] = jItem[pmProperty];
            }
            newARInvoicePaymentPM.IsDirty = false;
            if (mapParent) {
                newARInvoicePaymentPM.OldEntityPM = this.clone(newARInvoicePaymentPM);
                newARInvoicePaymentPM.UniqueKey = Guid_1.Guid.newGuid();
                newARInvoicePaymentPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newARInvoicePaymentPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newARInvoicePaymentPM.ChangeSetOp = "Update";
                }
                else {
                    newARInvoicePaymentPM.ChangeSetOp = "Insert";
                }
                newARInvoicePaymentPM.OldEntityPM = null;
                newARInvoicePaymentPM.EntityParentPM = null;
            }
            entityPM.InvoicePayments.push(newARInvoicePaymentPM);
        }
        if (oldInvoicePayments) {
            for (var itemKey in oldInvoicePayments) {
                if (entityPM.InvoicePayments.filter(function (p) { return p.UniqueKey === oldInvoicePayments[itemKey].UniqueKey; }).length === 0) {
                    if (oldInvoicePayments[itemKey]) {
                        oldInvoicePayments[itemKey].ChangeSetOp = "Delete";
                        entityPM.InvoicePayments.push(oldInvoicePayments[itemKey]);
                    }
                }
            }
        }
        entityPM.InvoiceTransfers = new Array();
        for (var item in jsonPM.InvoiceTransfers) {
            var jItem = jsonPM.InvoiceTransfers[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoiceTransferHistoryPM;
            newARInvoiceTransferHistoryPM = new ARInvoiceTransferHistoryPM_1.ARInvoiceTransferHistoryPM();
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoiceTransferHistoryPM[pmProperty] = jItem[pmProperty];
            }
            newARInvoiceTransferHistoryPM.IsDirty = false;
            entityPM.InvoiceTransfers.push(newARInvoiceTransferHistoryPM);
        }
        var oldConstituentInvoices = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldConstituentInvoices = entityPM.OldEntityPM.ConstituentInvoices;
        }
        entityPM.ConstituentInvoices = new Array();
        for (var item in jsonPM.ConstituentInvoices) {
            var jItem = jsonPM.ConstituentInvoices[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newConstituentPM;
            if (mapParent) {
                newConstituentPM = new ConstituentPM_1.ConstituentPM(entityPM);
            }
            else {
                newConstituentPM = new ConstituentPM_1.ConstituentPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newConstituentPM[pmProperty] = jItem[pmProperty];
            }
            newConstituentPM.IsDirty = false;
            if (mapParent) {
                newConstituentPM.OldEntityPM = this.clone(newConstituentPM);
                newConstituentPM.UniqueKey = Guid_1.Guid.newGuid();
                newConstituentPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newConstituentPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newConstituentPM.ChangeSetOp = "Update";
                }
                else {
                    newConstituentPM.ChangeSetOp = "Insert";
                }
                newConstituentPM.OldEntityPM = null;
                newConstituentPM.EntityParentPM = null;
            }
            entityPM.ConstituentInvoices.push(newConstituentPM);
        }
        if (oldConstituentInvoices) {
            for (var itemKey in oldConstituentInvoices) {
                if (entityPM.ConstituentInvoices.filter(function (p) { return p.UniqueKey === oldConstituentInvoices[itemKey].UniqueKey; }).length === 0) {
                    if (oldConstituentInvoices[itemKey]) {
                        oldConstituentInvoices[itemKey].ChangeSetOp = "Delete";
                        entityPM.ConstituentInvoices.push(oldConstituentInvoices[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.InvoiceLines = [];
            for (var m in entityPM.InvoiceLines) {
                entityPM.OldEntityPM.InvoiceLines.push(this.clone(entityPM.InvoiceLines[m]));
            }
            entityPM.OldEntityPM.InvoiceEntities = [];
            for (var m in entityPM.InvoiceEntities) {
                entityPM.OldEntityPM.InvoiceEntities.push(this.clone(entityPM.InvoiceEntities[m]));
            }
            entityPM.OldEntityPM.InvoicePayments = [];
            for (var m in entityPM.InvoicePayments) {
                entityPM.OldEntityPM.InvoicePayments.push(this.clone(entityPM.InvoicePayments[m]));
            }
            entityPM.OldEntityPM.InvoiceTransfers = [];
            for (var m in entityPM.InvoiceTransfers) {
                entityPM.OldEntityPM.InvoiceTransfers.push(this.clone(entityPM.InvoiceTransfers[m]));
            }
            entityPM.OldEntityPM.ConstituentInvoices = [];
            for (var m in entityPM.ConstituentInvoices) {
                entityPM.OldEntityPM.ConstituentInvoices.push(this.clone(entityPM.ConstituentInvoices[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    InvoiceDomainService.prototype.MapJsonToAPInvoiceMultipleShortPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new APInvoiceMultipleShortPM_1.APInvoiceMultipleShortPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapShortAPInvoiceLines(entityPM, jsonPM, mapParent);
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.InvoiceLines = [];
            for (var item in entityPM.InvoiceLines) {
                var myInvoiceLinePM = entityPM.InvoiceLines[item];
                var newInvoiceLinePM = this.clone(myInvoiceLinePM);
                entityPM.OldEntityPM.InvoiceLines.push(newInvoiceLinePM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    InvoiceDomainService.prototype.MapShortAPInvoiceLines = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldInvoiceLines = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoiceLines = entityPM.OldEntityPM.InvoiceLines;
        }
        entityPM.InvoiceLines = new Array();
        for (var item in jsonPM.InvoiceLines) {
            var jItem = jsonPM.InvoiceLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newAPInvoiceLinePM;
            if (mapParent) {
                newAPInvoiceLinePM = new APInvoiceLinePM_1.APInvoiceLinePM(entityPM);
            }
            else {
                newAPInvoiceLinePM = new APInvoiceLinePM_1.APInvoiceLinePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newAPInvoiceLinePM[pmProperty] = jItem[pmProperty];
            }
            newAPInvoiceLinePM.IsDirty = false;
            if (mapParent) {
                newAPInvoiceLinePM.UniqueKey = Guid_1.Guid.newGuid();
                newAPInvoiceLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newAPInvoiceLinePM.OldEntityPM = this.clone(newAPInvoiceLinePM);
            }
            else {
                if (newAPInvoiceLinePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newAPInvoiceLinePM.ChangeSetOp = "Update";
                }
                else {
                    newAPInvoiceLinePM.ChangeSetOp = "Insert";
                }
                newAPInvoiceLinePM.OldEntityPM = null;
                newAPInvoiceLinePM.EntityParentPM = null;
            }
            entityPM.InvoiceLines.push(newAPInvoiceLinePM);
        }
        if (oldInvoiceLines) {
            for (var itemKey in oldInvoiceLines) {
                if (entityPM.InvoiceLines.filter(function (p) { return p.UniqueKey === oldInvoiceLines[itemKey].UniqueKey; }).length === 0) {
                    if (oldInvoiceLines[itemKey]) {
                        //oldInvoiceLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.InvoiceLines.push(oldInvoiceLines[itemKey]);
                        var oldItemJson = oldInvoiceLines[itemKey];
                        var deletedPM = new APInvoiceLinePM_1.APInvoiceLinePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.InvoiceLines.push(deletedPM);
                    }
                }
            }
        }
    };
    InvoiceDomainService.prototype.clone = function (jsonPM) {
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
    InvoiceDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], InvoiceDomainService);
    return InvoiceDomainService;
}());
exports.InvoiceDomainService = InvoiceDomainService;
var DebtorsClass = /** @class */ (function () {
    function DebtorsClass() {
    }
    DebtorsClass.prototype.DebtorsClass = function () {
        this.linePrimary = ++DebtorsClass.counter;
    };
    DebtorsClass.counter = 0;
    return DebtorsClass;
}());
exports.DebtorsClass = DebtorsClass;
var CreditorsClass = /** @class */ (function () {
    function CreditorsClass() {
    }
    CreditorsClass.prototype.CreditorsClass = function () {
        this.linePrimary = ++CreditorsClass.counter;
    };
    CreditorsClass.counter = 0;
    return CreditorsClass;
}());
exports.CreditorsClass = CreditorsClass;
var ARInvoiceSATStatus = /** @class */ (function () {
    function ARInvoiceSATStatus() {
    }
    return ARInvoiceSATStatus;
}());
exports.ARInvoiceSATStatus = ARInvoiceSATStatus;
var APInvoiceNumberDuplicationCheckArgs = /** @class */ (function () {
    function APInvoiceNumberDuplicationCheckArgs() {
    }
    return APInvoiceNumberDuplicationCheckArgs;
}());
//# sourceMappingURL=InvoiceDomainService.js.map