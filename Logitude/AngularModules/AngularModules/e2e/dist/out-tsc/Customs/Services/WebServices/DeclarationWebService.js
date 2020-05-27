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
/*
* Contains a functions used in customs answers like:
*  GetDeclarationErrors, constraints , ....
*/
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var DeclarationErrorView_1 = require("../../EntityPMs/Extended/DeclarationErrorView");
var DeclarationCorrectionView_1 = require("../../EntityPMs/Extended/DeclarationCorrectionView");
var SupplierInvoicePM_1 = require("../../EntityPMs/SupplierInvoicePM");
var CertificateTicket_1 = require("../../DataContract/CertificateTicket");
var SupplierInvoiceItemPM_1 = require("../../EntityPMs/SupplierInvoiceItemPM");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var SupplierInvoicePMService_1 = require("../../Services/StandardPMs/SupplierInvoicePMService");
var DeclarationPaymentPMService_1 = require("../../Services/StandardPMs/DeclarationPaymentPMService");
var CustomsCollateralPM_1 = require("../../EntityPMs/CustomsCollateralPM");
var CollateralsRequestFileCondPM_1 = require("../../EntityPMs/CollateralsRequestFileCondPM");
var CustomsCollateralsAnswerPM_1 = require("../../EntityPMs/CustomsCollateralsAnswerPM");
var CustomsCollateralsConditionPM_1 = require("../../EntityPMs/CustomsCollateralsConditionPM");
var DeclarationWebService = /** @class */ (function () {
    function DeclarationWebService() {
        this._SupplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        this._DeclarationPaymentPMService = new DeclarationPaymentPMService_1.DeclarationPaymentPMService();
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';
    }
    //customs answers
    DeclarationWebService.prototype.GetDeclarationConstraintsByDeclrationId = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationConstraintsByDeclrationId/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationErrors = function (declarationId, listVersionId, courierFilter) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationErrors/?declarationId=" + declarationId
                + "&listVersionId=" + listVersionId + "&courierFilter=" + courierFilter, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntity(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetSingleCustomsCollateral = function (id) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSingleCustomsCollateral/?id=" + id, {
                headers: authHeader
            }).map(function (response) {
                var entity;
                var pm = response.json();
                if (pm) {
                    entity = _this.MapJsonToCustomsCollateralPM(pm);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.CheckIfDocumentPointerExistsForConstraint = function (constraintNumber) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCheckIfDocumentPointerExistsForConstraint/?constraintNumber=" + constraintNumber, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //customs ansewers -> supplier invoices
    DeclarationWebService.prototype.GetSupplierInvoiceBySequenceNumber = function (declarationId, invoiceSequence, skip, take) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSupplierInvoiceBySequenceNumber/?declarationId=" + declarationId
                + "&invoiceSequence=" + invoiceSequence
                + "&skip=" + skip
                + "&take=" + take, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                if (res) {
                    res = _this._SupplierInvoicePMService.MapJsonToEntityPM(res, true);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetSupplierInvoiceWithItemBySequenceNumber = function (declarationId, invoiceSequence, itemSequence, skip, take, type) {
        var _this = this;
        if (type === void 0) { type = null; }
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSupplierInvoiceWithItemBySequenceNumber/?declarationId=" + declarationId
                + "&invoiceSequence=" + invoiceSequence
                + "&itemSequence=" + itemSequence
                + "&skip=" + skip
                + "&take=" + take
                + "&type=" + type, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                if (res)
                    res = _this._SupplierInvoicePMService.MapJsonToEntityPM(res, true);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetSupplierInvoiceWithSpecificItemByCounterKey = function (declarationId, counterKey, itemSequence) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSupplierInvoiceWithSpecificItemByCounterKey/?declarationId=" + declarationId
                + "&counterKey=" + counterKey
                + "&itemSequence=" + itemSequence, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                //res = this.MapJsonToEntityPM(res, true);
                if (res)
                    res = _this._SupplierInvoicePMService.MapJsonToEntityPM(res, true);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //Certificates
    DeclarationWebService.prototype.GetCertificateTickets = function (declarationId, reqConfirmationType, invoiceNumber, invoiceCounterKey, demandState) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCertificateTickets/?declarationId=" + declarationId
                + "&reqConfirmationType=" + reqConfirmationType
                + "&invoiceNumber=" + invoiceNumber
                + "&invoiceCounterKey=" + invoiceCounterKey
                + "&demandState=" + demandState, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToCertificateTicket(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationInvoicesNumbers = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationInvoicesNumbers/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //supplier invoice -> ItemCode double click logic
    DeclarationWebService.prototype.GetCustomsPartnersItemsForSelection = function (vendorId, CustomerId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCustomsPartnersItemsForSelection/?vendorId=" + vendorId
                + "&CustomerId=" + CustomerId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetGTBITEMPartnersItemList = function (vendorId, customerCode, search, top) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetGTBITEMPartnersItemList/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&search=" + search
                + "&top=" + top, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetGITITEMPartnersItemList = function (vendorId, customerCode, search, top, isSearchNULLVendor) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetGITITEMPartnersItemList/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&search=" + search
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetGITITEMPartnersItemListByItemCode = function (vendorId, customerCode, itemCode, top, isSearchNULLVendor) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetGITITEMPartnersItemListByItemCode/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&itemCode=" + itemCode
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetGITITEMPartnersItemListByName = function (vendorId, customerCode, name, top) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetGITITEMPartnersItemListByName/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&name=" + name
                + "&top=" + top, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //Send declaration
    DeclarationWebService.prototype.PostSendDeclaration = function (genericRequestParams) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return _this._http.post(_this._apiUrl + '/PostSendDeclaration/', JSON.stringify(genericRequestParams), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.PostSendManifest = function (genericRequestParams) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return _this._http.post(_this._apiUrl + '/PostSendManifest/', JSON.stringify(genericRequestParams), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.PostSendDeclarationChecksAndPrecalculations = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSendDeclarationChecksAndPrecalculations/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetRequiredFieldsForDeclaration = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetRequiredFieldsForDeclaration/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetRequiredFieldsForCourierDeclaration = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetRequiredFieldsForCourierDeclaration/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.CheckCertificateStatus = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCheckCertificateStatus/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetMAWBCourierMasterByDeclaration = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetMAWBCourierMasterByDeclaration/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDocumentDeclarationId = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDocumentDeclarationId/?DeclarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationDocumentList = function (parentEntityId, parentEntityCode) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationDocumentList/?parentEntityId=" + parentEntityId
                + "&parentEntityCode=" + parentEntityCode, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationMandatoryTicketList = function (parentEntityId, parentEntityCode) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationMandatoryTicketList/?parentEntityId=" + parentEntityId
                + "&parentEntityCode=" + parentEntityCode, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.CheckFreightAmountsByIncoterm = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCheckFreightAmountsByIncoterm/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.DeclarationClosureMethod = function (declarationId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationClosureMethod/?declarationId=" + declarationId + '&tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.CancelDeclarationClosureMethod = function (declarationId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCancelDeclarationClosureMethod/?declarationId=" + declarationId + '&tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //Payment Orders
    DeclarationWebService.prototype.GetSingleDeclarationPaymentPMandDefaultExplain = function (id, CustomerCode) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSingleDeclarationPaymentPMandDefaultExplain/?id=" + id
                + "&CustomerCode=" + CustomerCode, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                var entity;
                if (res) {
                    entity = _this._DeclarationPaymentPMService.MapJsonToEntityPM(res, true);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetCustomBanksForCard = function (cardId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCustomBanksForCard/?cardId=" + cardId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                ////serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetAllRequiredFieldsForDeclarationPayment = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetAllRequiredFieldsForDeclarationPayment/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    // Amendments
    DeclarationWebService.prototype.GetDeclarationCorrection = function (declarationId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationCorrection/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(function (response) {
                var mappedEntity;
                var allLists = response.json();
                if (allLists)
                    mappedEntity = _this.MapJsonToCorrectionView(allLists);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedEntity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    // Tapag
    DeclarationWebService.prototype.GetDeclarationByTapagConnectionConnection = function (tapagId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationByTapagConnectionConnection/?tapagId=" + tapagId, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationCollateralsList = function (declarationId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationCollateralsList/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationCargoSplitByDeclarationIdList = function (declarationId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationCargoSplitByDeclarationIdList/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationWebService.prototype.GetDeclarationMamanSpecialAction = function (declarationId, tenant, actionCode, mamanSpecialActionCode) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationMamanSpecialAction/?declarationId=" + declarationId + "&tenant=" + tenant + "&actionCode=" + actionCode + "&mamanSpecialActionCode=" + mamanSpecialActionCode, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    // ---------------------------------------- MAPING --------------------------------------------------
    DeclarationWebService.prototype.MapJsonToEntity = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DeclarationErrorView_1.DeclarationErrorView();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        //entityPM.IsDirty = false;
        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entityPM;
    };
    DeclarationWebService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new SupplierInvoicePM_1.SupplierInvoicePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapSupplierInvoiceItems(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.SupplierInvoiceItems = [];
            for (var item in entityPM.SupplierInvoiceItems) {
                var mySupplierInvoiceItemPM = entityPM.SupplierInvoiceItems[item];
                var newSupplierInvoiceItemPM = this.clone(mySupplierInvoiceItemPM);
                entityPM.OldEntityPM.SupplierInvoiceItems.push(newSupplierInvoiceItemPM);
            }
            entityPM.OldEntityPM.SupplierInvoiceModifications = [];
            for (var item in entityPM.SupplierInvoiceModifications) {
                var mySupplierInvoiceModificationPM = entityPM.SupplierInvoiceModifications[item];
                var newSupplierInvoiceModificationPM = this.clone(mySupplierInvoiceModificationPM);
                entityPM.OldEntityPM.SupplierInvoiceModifications.push(newSupplierInvoiceModificationPM);
            }
            entityPM.OldEntityPM.SupplierInvoiceFreightAmounts = [];
            for (var item in entityPM.SupplierInvoiceFreightAmounts) {
                var mySupplierInvoiceFreightAmountPM = entityPM.SupplierInvoiceFreightAmounts[item];
                var newSupplierInvoiceFreightAmountPM = this.clone(mySupplierInvoiceFreightAmountPM);
                entityPM.OldEntityPM.SupplierInvoiceFreightAmounts.push(newSupplierInvoiceFreightAmountPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    DeclarationWebService.prototype.MapJsonToCertificateTicket = function (json, mapParent, entity) {
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
    DeclarationWebService.prototype.MapSupplierInvoiceItems = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldSupplierInvoiceItems = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItems = entityPM.OldEntityPM.SupplierInvoiceItems;
        }
        entityPM.SupplierInvoiceItems = new Array();
        for (var item in jsonPM.SupplierInvoiceItems) {
            var jItem = jsonPM.SupplierInvoiceItems[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemPM;
            if (mapParent) {
                newSupplierInvoiceItemPM = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(entityPM);
            }
            else {
                newSupplierInvoiceItemPM = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemPM.IsDirty = false;
            if (mapParent) {
                newSupplierInvoiceItemPM.UniqueKey = Guid_1.Guid.newGuid();
                newSupplierInvoiceItemPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemPM.OldEntityPM = this.clone(newSupplierInvoiceItemPM);
            }
            else {
                if (newSupplierInvoiceItemPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newSupplierInvoiceItemPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceItemPM.ChangeSetOp = "Insert";
                }
                newSupplierInvoiceItemPM.OldEntityPM = null;
                newSupplierInvoiceItemPM.EntityParentPM = null;
            }
            entityPM.SupplierInvoiceItems.push(newSupplierInvoiceItemPM);
        }
        if (oldSupplierInvoiceItems) {
            for (var itemKey in oldSupplierInvoiceItems) {
                if (entityPM.SupplierInvoiceItems.filter(function (p) { return p.UniqueKey === oldSupplierInvoiceItems[itemKey].UniqueKey; }).length === 0) {
                    if (oldSupplierInvoiceItems[itemKey]) {
                        //oldSupplierInvoiceItems[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItems.push(oldSupplierInvoiceItems[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItems[itemKey];
                        var deletedPM = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(null);
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
                        entityPM.SupplierInvoiceItems.push(deletedPM);
                    }
                }
            }
        }
    };
    DeclarationWebService.prototype.MapJsonToCustomsCollateralPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new CustomsCollateralPM_1.CustomsCollateralPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapCustomsCollateralsConditions(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomsCollateralsAnswers(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.CustomsCollateralsConditions = [];
            for (var item in entityPM.CustomsCollateralsConditions) {
                var myCustomsCollateralsConditionPM = entityPM.CustomsCollateralsConditions[item];
                var newCustomsCollateralsConditionPM = this.clone(myCustomsCollateralsConditionPM);
                entityPM.OldEntityPM.CustomsCollateralsConditions.push(newCustomsCollateralsConditionPM);
            }
            entityPM.OldEntityPM.CustomsCollateralsAnswers = [];
            for (var item in entityPM.CustomsCollateralsAnswers) {
                var myCustomsCollateralsAnswerPM = entityPM.CustomsCollateralsAnswers[item];
                var newCustomsCollateralsAnswerPM = this.clone(myCustomsCollateralsAnswerPM);
                newCustomsCollateralsAnswerPM.CollateralsRequestFileConds = [];
                for (var k in myCustomsCollateralsAnswerPM.CollateralsRequestFileConds) {
                    var myCollateralsRequestFileCondPM = myCustomsCollateralsAnswerPM.CollateralsRequestFileConds[k];
                    var newCollateralsRequestFileCondPM = this.clone(myCustomsCollateralsAnswerPM.CollateralsRequestFileConds[k]);
                    newCustomsCollateralsAnswerPM.CollateralsRequestFileConds.push(newCollateralsRequestFileCondPM);
                }
                entityPM.OldEntityPM.CustomsCollateralsAnswers.push(newCustomsCollateralsAnswerPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    DeclarationWebService.prototype.MapCustomsCollateralsConditions = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldCustomsCollateralsConditions = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomsCollateralsConditions = entityPM.OldEntityPM.CustomsCollateralsConditions;
        }
        entityPM.CustomsCollateralsConditions = new Array();
        for (var item in jsonPM.CustomsCollateralsConditions) {
            var jItem = jsonPM.CustomsCollateralsConditions[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomsCollateralsConditionPM;
            if (mapParent) {
                newCustomsCollateralsConditionPM = new CustomsCollateralsConditionPM_1.CustomsCollateralsConditionPM(entityPM);
            }
            else {
                newCustomsCollateralsConditionPM = new CustomsCollateralsConditionPM_1.CustomsCollateralsConditionPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomsCollateralsConditionPM[pmProperty] = jItem[pmProperty];
            }
            newCustomsCollateralsConditionPM.IsDirty = false;
            if (mapParent) {
                newCustomsCollateralsConditionPM.UniqueKey = Guid_1.Guid.newGuid();
                newCustomsCollateralsConditionPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomsCollateralsConditionPM.OldEntityPM = this.clone(newCustomsCollateralsConditionPM);
            }
            else {
                if (newCustomsCollateralsConditionPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newCustomsCollateralsConditionPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomsCollateralsConditionPM.ChangeSetOp = "Insert";
                }
                newCustomsCollateralsConditionPM.OldEntityPM = null;
                newCustomsCollateralsConditionPM.EntityParentPM = null;
            }
            entityPM.CustomsCollateralsConditions.push(newCustomsCollateralsConditionPM);
        }
        if (oldCustomsCollateralsConditions) {
            for (var itemKey in oldCustomsCollateralsConditions) {
                if (entityPM.CustomsCollateralsConditions.filter(function (p) { return p.UniqueKey === oldCustomsCollateralsConditions[itemKey].UniqueKey; }).length === 0) {
                    if (oldCustomsCollateralsConditions[itemKey]) {
                        //oldCustomsCollateralsConditions[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomsCollateralsConditions.push(oldCustomsCollateralsConditions[itemKey]);
                        var oldItemJson = oldCustomsCollateralsConditions[itemKey];
                        var deletedPM = new CustomsCollateralsConditionPM_1.CustomsCollateralsConditionPM(null);
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
                        entityPM.CustomsCollateralsConditions.push(deletedPM);
                    }
                }
            }
        }
    };
    DeclarationWebService.prototype.MapCustomsCollateralsAnswers = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldCustomsCollateralsAnswers = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomsCollateralsAnswers = entityPM.OldEntityPM.CustomsCollateralsAnswers;
        }
        entityPM.CustomsCollateralsAnswers = new Array();
        for (var item in jsonPM.CustomsCollateralsAnswers) {
            var jItem = jsonPM.CustomsCollateralsAnswers[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomsCollateralsAnswerPM;
            if (mapParent) {
                newCustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM_1.CustomsCollateralsAnswerPM(entityPM);
            }
            else {
                newCustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM_1.CustomsCollateralsAnswerPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomsCollateralsAnswerPM[pmProperty] = jItem[pmProperty];
            }
            newCustomsCollateralsAnswerPM.IsDirty = false;
            if (mapParent) {
                newCustomsCollateralsAnswerPM.UniqueKey = Guid_1.Guid.newGuid();
                newCustomsCollateralsAnswerPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomsCollateralsAnswerPM.OldEntityPM = this.clone(newCustomsCollateralsAnswerPM);
                this.MapCollateralsRequestFileConds(newCustomsCollateralsAnswerPM, jItem, mapParent);
                newCustomsCollateralsAnswerPM.OldEntityPM.CollateralsRequestFileConds = [];
                for (var k in newCustomsCollateralsAnswerPM.CollateralsRequestFileConds) {
                    var clonedInside = this.clone(newCustomsCollateralsAnswerPM.CollateralsRequestFileConds[k]);
                    newCustomsCollateralsAnswerPM.OldEntityPM.CollateralsRequestFileConds.push(clonedInside); // clone old CollateralsRequestFileConds//
                }
            }
            else {
                if (newCustomsCollateralsAnswerPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newCustomsCollateralsAnswerPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomsCollateralsAnswerPM.ChangeSetOp = "Insert";
                }
                this.MapCollateralsRequestFileConds(newCustomsCollateralsAnswerPM, jItem, mapParent);
                newCustomsCollateralsAnswerPM.OldEntityPM = null;
                newCustomsCollateralsAnswerPM.EntityParentPM = null;
            }
            entityPM.CustomsCollateralsAnswers.push(newCustomsCollateralsAnswerPM);
        }
        if (oldCustomsCollateralsAnswers) {
            for (var itemKey in oldCustomsCollateralsAnswers) {
                if (entityPM.CustomsCollateralsAnswers.filter(function (p) { return p.UniqueKey === oldCustomsCollateralsAnswers[itemKey].UniqueKey; }).length === 0) {
                    if (oldCustomsCollateralsAnswers[itemKey]) {
                        //oldCustomsCollateralsAnswers[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomsCollateralsAnswers.push(oldCustomsCollateralsAnswers[itemKey]);
                        var oldItemJson = oldCustomsCollateralsAnswers[itemKey];
                        var deletedPM = new CustomsCollateralsAnswerPM_1.CustomsCollateralsAnswerPM(null);
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
                        this.MapCollateralsRequestFileConds(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomsCollateralsAnswers.push(deletedPM);
                    }
                }
            }
        }
    };
    DeclarationWebService.prototype.MapCollateralsRequestFileConds = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldCollateralsRequestFileConds = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollateralsRequestFileConds = entityPM.OldEntityPM.CollateralsRequestFileConds;
        }
        entityPM.CollateralsRequestFileConds = new Array();
        for (var item in jsonPM.CollateralsRequestFileConds) {
            var jItem = jsonPM.CollateralsRequestFileConds[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCollateralsRequestFileCondPM;
            if (mapParent) {
                newCollateralsRequestFileCondPM = new CollateralsRequestFileCondPM_1.CollateralsRequestFileCondPM(entityPM);
            }
            else {
                newCollateralsRequestFileCondPM = new CollateralsRequestFileCondPM_1.CollateralsRequestFileCondPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCollateralsRequestFileCondPM[pmProperty] = jItem[pmProperty];
            }
            newCollateralsRequestFileCondPM.IsDirty = false;
            if (mapParent) {
                newCollateralsRequestFileCondPM.UniqueKey = Guid_1.Guid.newGuid();
                newCollateralsRequestFileCondPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCollateralsRequestFileCondPM.OldEntityPM = this.clone(newCollateralsRequestFileCondPM);
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newCollateralsRequestFileCondPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newCollateralsRequestFileCondPM.UniqueKey) {
                        if (jItem.IsDirty)
                            newCollateralsRequestFileCondPM.ChangeSetOp = "Update";
                    }
                    else {
                        newCollateralsRequestFileCondPM.ChangeSetOp = "Insert";
                    }
                }
                newCollateralsRequestFileCondPM.OldEntityPM = null;
                newCollateralsRequestFileCondPM.EntityParentPM = null;
            }
            entityPM.CollateralsRequestFileConds.push(newCollateralsRequestFileCondPM);
        }
        if (oldCollateralsRequestFileConds) {
            for (var itemKey in oldCollateralsRequestFileConds) {
                if (entityPM.CollateralsRequestFileConds.filter(function (p) { return p.UniqueKey === oldCollateralsRequestFileConds[itemKey].UniqueKey; }).length === 0) {
                    if (oldCollateralsRequestFileConds[itemKey]) {
                        //oldCollateralsRequestFileConds[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CollateralsRequestFileConds.push(oldCollateralsRequestFileConds[itemKey]);
                        var oldItemJson = oldCollateralsRequestFileConds[itemKey];
                        var deletedPM = new CollateralsRequestFileCondPM_1.CollateralsRequestFileCondPM(null);
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
                        entityPM.CollateralsRequestFileConds.push(deletedPM);
                    }
                }
            }
        }
    };
    DeclarationWebService.prototype.MapJsonToCorrectionView = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DeclarationCorrectionView_1.DeclarationCorrectionView();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        //entityPM.IsDirty = false;
        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entityPM;
    };
    // --------------------------------------------------------------------------------------------------
    DeclarationWebService.prototype.clone = function (jsonPM) {
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
    DeclarationWebService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new CustomsCollateralPM_1.CustomsCollateralPM();
        entityPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        return entityPM;
    };
    DeclarationWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DeclarationWebService);
    return DeclarationWebService;
}());
exports.DeclarationWebService = DeclarationWebService;
//# sourceMappingURL=DeclarationWebService.js.map