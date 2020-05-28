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
var TicketPM_1 = require("../EntityPMs/TicketPM");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var ClassLevelValidator_1 = require("../../Infrastructure/Validators/ClassLevelValidator");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var SLAHeaderPM_1 = require("../EntityPMs/SLAHeaderPM");
var SLALinePM_1 = require("../EntityPMs/SLALinePM");
var SLAEscalationPM_1 = require("../EntityPMs/SLAEscalationPM");
var SLAEscalationRecepientPM_1 = require("../EntityPMs/SLAEscalationRecepientPM");
var CustomFieldClass_1 = require("../../Infrastructure/DataContracts/CustomFieldClass");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var CRMDomainService = /** @class */ (function () {
    function CRMDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CRMDomain';
    }
    CRMDomainService.prototype.InserNewTicket = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("Ticket", entityPM);
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.post(_this._apiUrl + '/InserNewTicket?entityPM=' + entityPM, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                        response.Result = mappedResult;
                    }
                    return response;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    CRMDomainService.prototype.GetOwnerEmployeeGroup = function (ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOwnerEmployeeGroup?ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetContactCards = function (companyId, contactId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetContactCards?companyId=' + companyId + '&contactId=' + contactId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetConnectContactCards = function (companyId, contactId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetConnectContactCards?companyId=' + companyId + '&contactId=' + contactId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetActivitiesDashBoard = function (ownerId, businessUnitId, activityTypeCodeFilter, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesDashBoard?OwnerId=' + ownerId + '&BusinessUnitId=' + businessUnitId + '&activityTypeCodeFilter=' + activityTypeCodeFilter + '&RecordsTypeCode=' + RecordsTypeCode;
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
    CRMDomainService.prototype.GetOpportunitiesChartDataCustom = function (FromDate, ToDate, ownerId, businessUnitId, chartCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesChartDataCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
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
    CRMDomainService.prototype.GetOpportunitiesChartData = function (code, ownerId, businessUnitId, chartCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesChartData?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
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
    CRMDomainService.prototype.GetActivitiesChartDataCustom = function (FromDate, ToDate, ownerId, businessUnitId, chartCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesChartDataCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
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
    CRMDomainService.prototype.GetActivitiesChartData = function (code, ownerId, businessUnitId, chartCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesChartData?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
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
    CRMDomainService.prototype.GetQuotesChartDataCustom = function (FromDate, ToDate, ownerId, businessUnitId, chartCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesChartDataCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
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
    CRMDomainService.prototype.GetQuotesChartData = function (code, ownerId, businessUnitId, chartCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesChartData?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
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
    CRMDomainService.prototype.GetOpenedTicketsGroupByClassification = function (code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsGroupByClassification?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetOpenedTicketsGroupBySeverity = function (code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsGroupBySeverity?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetOpenedTicketsGroupByOwner = function (code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsGroupByOwner?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetOpenedTicketsBySLAViolation = function (selectedIndex, code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsBySLAViolation?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetOpenedTicketsByOpenedStage = function (selectedIndex, code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsByOpenedStage?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetClosedTicketsGroupByClassification = function (code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsGroupByClassification?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetClosedTicketsGroupBySeverity = function (code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsGroupBySeverity?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetClosedTicketsGroupByType = function (code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsGroupByType?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetClosedTicketsBySLAViolation = function (selectedIndex, code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsBySLAViolation?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetClosedTicketsBySolvedStage = function (selectedIndex, code, ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsBySolvedStage?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetOpenTicketsGroupByClassification = function (ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenTicketsGroupByClassification?ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetOpenTicketsByDueTime = function (ownerId, employeeGroupId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenTicketsByDueTime?ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
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
    CRMDomainService.prototype.GetTicketOverviewPerformance = function (ticketId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTicketOverviewPerformance?ticketId=' + ticketId;
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
    CRMDomainService.prototype.GetRecentTickets = function (myOwnerId, myEmployeeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetRecentTickets?ownerId=' + myOwnerId + '&employeeGroupId=' + myEmployeeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    CRMDomainService.prototype.GetRecentOpportunities = function (myOwnerId, myEmployeeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetRecentOpportunities?ownerId=' + myOwnerId + '&businessUnitId=' + myEmployeeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    CRMDomainService.prototype.GetCorrespondencesList = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTicketCorrespondences?entityId=' + entityId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    CRMDomainService.prototype.GetTicketsCounts = function (myOwnerId, myEmployeeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTicketsCounts?ownerId=' + myOwnerId + '&employeeGroupId=' + myEmployeeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    CRMDomainService.prototype.GetTopTickets = function (myOwnerId, myEmployeeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTopTickets?ownerId=' + myOwnerId + '&employeeGroupId=' + myEmployeeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    CRMDomainService.prototype.GetTicketsCountByShipmentNumber = function (shipmentNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTicketsCountByShipmentNumber?shipmentNumber=' + shipmentNumber, {
                headers: authHeader
            }).map(function (response) {
                var count = response.json();
                return count;
            });
        });
    };
    CRMDomainService.prototype.GetEmployeeGroupsPMList = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        //authHeader.append('Token', sessionStorage.getItem("Token"));
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetEmployeeGroupsPMList?', {
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
    CRMDomainService.prototype.GetUsersByEmployeeGroupIds = function (employeeIds) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        //authHeader.append('Token', sessionStorage.getItem("Token"));
        var url = this._apiUrl + '/GetUsersByEmployeeGroupIds?employeeIds=' + employeeIds;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetContactListsByEmailsString = function (emails) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetContactListsByEmailsString?emails=' + emails;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetUserListsByEmailsString = function (emails) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetUserListsByEmailsString?emails=' + emails;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetTicketEscalationListsByTicketId = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTicketEscalationListsByTicketId?entityId=' + entityId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            });
        });
    };
    CRMDomainService.prototype.GetTicketOverViewStatisticsSummary = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTicketOverViewStatisticsSummary?entityId=' + entityId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            });
        });
    };
    CRMDomainService.prototype.GetBusinessHours = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetBusinessHours?entityId=' + entityId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            });
        });
    };
    CRMDomainService.prototype.GetOpportunitiesSummary = function (ownerId, businessUnitId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesSummary?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;
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
    CRMDomainService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new TicketPM_1.TicketPM();
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
    CRMDomainService.prototype.clone = function (jsonPM) {
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
    CRMDomainService.prototype.GetCRMDailySpotlightCounts = function (ownerId, businessUnitId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCRMDailySpotlightCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new DailySpotlightClass();
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
    CRMDomainService.prototype.GetUpcomigActivities = function (ownerId, businessUnitId, activityTypeCodeFilter, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpcomigActivities?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&activityTypeCodeFilter=' + activityTypeCodeFilter + '&RecordsTypeCode=' + RecordsTypeCode;
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
    CRMDomainService.prototype.GetStageFunnelData = function (ownerId, businessUnitId, filterCode, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetStageFunnelData?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&filterCode=' + filterCode + '&RecordsTypeCode=' + RecordsTypeCode;
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
    CRMDomainService.prototype.GetCompleteActivity = function (activityId, post, summary) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCompleteActivity?activityId=' + activityId + '&post=' + post + '&summary=' + summary;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var activity = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = activity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetReopenActivity = function (activityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetReopenActivity?activityId=' + activityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var activity = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = activity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetActivitiesSummary = function (activityTypeCodeFilter, ownerId, businessUnitId, RecordsTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesSummary?activityTypeCodeFilter=' + activityTypeCodeFilter + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;
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
    CRMDomainService.prototype.GetActiveSLAbyTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActiveSLAbyTenant?', {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var entity;
                var allLists;
                allLists = new Array();
                result.forEach(function (item) {
                    entity = _this.MapJsonToSLAHeaderEntityPM(item);
                    allLists.push(entity);
                });
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = allLists;
                return pmresponse;
                //var allLists = response.json();
                //var myResponse: ServiceResponse;
                //myResponse = new ServiceResponse();
                //myResponse.Result = allLists;
                //return myResponse;
            });
        });
    };
    CRMDomainService.prototype.GetSingleSLAHeaderPMByTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleSLAHeaderPMByTenant?', {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToSLAHeaderEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            });
        });
    };
    CRMDomainService.prototype.MapJsonToSLAHeaderEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new SLAHeaderPM_1.SLAHeaderPM();
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
        this.MapSLALines(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapSLAEscalations(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.SLALines = [];
            for (var item in entityPM.SLALines) {
                var mySLALinePM = entityPM.SLALines[item];
                var newSLALinePM = this.clone(mySLALinePM);
                entityPM.OldEntityPM.SLALines.push(newSLALinePM);
            }
            entityPM.OldEntityPM.SLAEscalations = [];
            for (var item in entityPM.SLAEscalations) {
                var mySLAEscalationPM = entityPM.SLAEscalations[item];
                var newSLAEscalationPM = this.clone(mySLAEscalationPM);
                newSLAEscalationPM.SLAEscalationRecepients = [];
                for (var k in mySLAEscalationPM.SLAEscalationRecepients) {
                    var mySLAEscalationRecepientPM = mySLAEscalationPM.SLAEscalationRecepients[k];
                    var newSLAEscalationRecepientPM = this.clone(mySLAEscalationPM.SLAEscalationRecepients[k]);
                    newSLAEscalationPM.SLAEscalationRecepients.push(newSLAEscalationRecepientPM);
                }
                entityPM.OldEntityPM.SLAEscalations.push(newSLAEscalationPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    CRMDomainService.prototype.MapSLALines = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldSLALines = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSLALines = entityPM.OldEntityPM.SLALines;
        }
        entityPM.SLALines = new Array();
        for (var item in jsonPM.SLALines) {
            var jItem = jsonPM.SLALines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSLALinePM;
            if (mapParent) {
                newSLALinePM = new SLALinePM_1.SLALinePM(entityPM);
            }
            else {
                newSLALinePM = new SLALinePM_1.SLALinePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSLALinePM[pmProperty] = jItem[pmProperty];
            }
            newSLALinePM.IsDirty = false;
            if (mapParent) {
                newSLALinePM.UniqueKey = Guid_1.Guid.newGuid();
                newSLALinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSLALinePM.OldEntityPM = this.clone(newSLALinePM);
            }
            else {
                if (newSLALinePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newSLALinePM.ChangeSetOp = "Update";
                }
                else {
                    newSLALinePM.ChangeSetOp = "Insert";
                }
                newSLALinePM.OldEntityPM = null;
                newSLALinePM.EntityParentPM = null;
            }
            entityPM.SLALines.push(newSLALinePM);
        }
        if (oldSLALines) {
            for (var itemKey in oldSLALines) {
                if (entityPM.SLALines.filter(function (p) { return p.UniqueKey === oldSLALines[itemKey].UniqueKey; }).length === 0) {
                    if (oldSLALines[itemKey]) {
                        //oldSLALines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SLALines.push(oldSLALines[itemKey]);
                        var oldItemJson = oldSLALines[itemKey];
                        var deletedPM = new SLALinePM_1.SLALinePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.SLALines.push(deletedPM);
                    }
                }
            }
        }
    };
    CRMDomainService.prototype.MapSLAEscalations = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldSLAEscalations = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSLAEscalations = entityPM.OldEntityPM.SLAEscalations;
        }
        entityPM.SLAEscalations = new Array();
        for (var item in jsonPM.SLAEscalations) {
            var jItem = jsonPM.SLAEscalations[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSLAEscalationPM;
            if (mapParent) {
                newSLAEscalationPM = new SLAEscalationPM_1.SLAEscalationPM(entityPM);
            }
            else {
                newSLAEscalationPM = new SLAEscalationPM_1.SLAEscalationPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSLAEscalationPM[pmProperty] = jItem[pmProperty];
            }
            newSLAEscalationPM.IsDirty = false;
            if (mapParent) {
                newSLAEscalationPM.UniqueKey = Guid_1.Guid.newGuid();
                newSLAEscalationPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSLAEscalationPM.OldEntityPM = this.clone(newSLAEscalationPM);
                this.MapSLAEscalationRecepients(newSLAEscalationPM, jItem, mapParent);
                newSLAEscalationPM.OldEntityPM.SLAEscalationRecepients = [];
                for (var k in newSLAEscalationPM.SLAEscalationRecepients) {
                    var clonedInside = this.clone(newSLAEscalationPM.SLAEscalationRecepients[k]);
                    newSLAEscalationPM.OldEntityPM.SLAEscalationRecepients.push(clonedInside); // clone old SLAEscalationRecepients//
                }
            }
            else {
                if (newSLAEscalationPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newSLAEscalationPM.ChangeSetOp = "Update";
                }
                else {
                    newSLAEscalationPM.ChangeSetOp = "Insert";
                }
                this.MapSLAEscalationRecepients(newSLAEscalationPM, jItem, mapParent);
                newSLAEscalationPM.OldEntityPM = null;
                newSLAEscalationPM.EntityParentPM = null;
            }
            entityPM.SLAEscalations.push(newSLAEscalationPM);
        }
        if (oldSLAEscalations) {
            for (var itemKey in oldSLAEscalations) {
                if (entityPM.SLAEscalations.filter(function (p) { return p.UniqueKey === oldSLAEscalations[itemKey].UniqueKey; }).length === 0) {
                    if (oldSLAEscalations[itemKey]) {
                        //oldSLAEscalations[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SLAEscalations.push(oldSLAEscalations[itemKey]);
                        var oldItemJson = oldSLAEscalations[itemKey];
                        var deletedPM = new SLAEscalationPM_1.SLAEscalationPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        this.MapSLAEscalationRecepients(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.SLAEscalations.push(deletedPM);
                    }
                }
            }
        }
    };
    CRMDomainService.prototype.MapSLAEscalationRecepients = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldSLAEscalationRecepients = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSLAEscalationRecepients = entityPM.OldEntityPM.SLAEscalationRecepients;
        }
        entityPM.SLAEscalationRecepients = new Array();
        for (var item in jsonPM.SLAEscalationRecepients) {
            var jItem = jsonPM.SLAEscalationRecepients[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSLAEscalationRecepientPM;
            if (mapParent) {
                newSLAEscalationRecepientPM = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(entityPM);
            }
            else {
                newSLAEscalationRecepientPM = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSLAEscalationRecepientPM[pmProperty] = jItem[pmProperty];
            }
            newSLAEscalationRecepientPM.IsDirty = false;
            if (mapParent) {
                newSLAEscalationRecepientPM.UniqueKey = Guid_1.Guid.newGuid();
                newSLAEscalationRecepientPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSLAEscalationRecepientPM.OldEntityPM = this.clone(newSLAEscalationRecepientPM);
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSLAEscalationRecepientPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSLAEscalationRecepientPM.UniqueKey) {
                        if (jItem.IsDirty)
                            newSLAEscalationRecepientPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSLAEscalationRecepientPM.ChangeSetOp = "Insert";
                    }
                }
                newSLAEscalationRecepientPM.OldEntityPM = null;
                newSLAEscalationRecepientPM.EntityParentPM = null;
            }
            entityPM.SLAEscalationRecepients.push(newSLAEscalationRecepientPM);
        }
        if (oldSLAEscalationRecepients) {
            for (var itemKey in oldSLAEscalationRecepients) {
                if (entityPM.SLAEscalationRecepients.filter(function (p) { return p.UniqueKey === oldSLAEscalationRecepients[itemKey].UniqueKey; }).length === 0) {
                    if (oldSLAEscalationRecepients[itemKey]) {
                        //oldSLAEscalationRecepients[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SLAEscalationRecepients.push(oldSLAEscalationRecepients[itemKey]);
                        var oldItemJson = oldSLAEscalationRecepients[itemKey];
                        var deletedPM = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.SLAEscalationRecepients.push(deletedPM);
                    }
                }
            }
        }
    };
    CRMDomainService.prototype.UpdatingActivityMettingSummary = function (mettingSummary, post, activityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdatingActivityMettingSummary?mettingSummary=' + mettingSummary + '&post=' + post + '&activityId=' + activityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CRMDomainService.prototype.GetActivitiesByOpportunityId = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesByOpportunityId?entityId=' + entityId;
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
    CRMDomainService.prototype.GetActivitiesByTicketId = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesByTicketId?entityId=' + entityId;
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
    CRMDomainService.prototype.GetQuotesGroupBySalesman = function (code, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesGroupBySalesman?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetQuotesGroupBySalesmanCustom = function (FromDate, ToDate, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesGroupBySalesmanCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetCustomersGroupBySalesmanCustom = function (FromDate, ToDate, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersGroupBySalesmanCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetCustomersGroupBySalesman = function (days, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersGroupBySalesman?days=' + days + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetActivitiesGroupBySalesmanCustom = function (FromDate, ToDate, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesGroupBySalesmanCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetActivitiesGroupBySalesman = function (code, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesGroupBySalesman?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetOpportunitiesGroupBySalesmanCustom = function (FromDate, ToDate, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesGroupBySalesmanCustom?FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetOpportunitiesGroupBySalesman = function (code, ownerId, businessUnitId, fieldCode, isTopTen) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesGroupBySalesman?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
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
    CRMDomainService.prototype.GetUpdateCorrespondence = function (entityId, rightToLeft) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateCorrespondence?entityId=' + entityId + '&rightToLeft=' + rightToLeft;
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
    CRMDomainService.prototype.GetCommunicationLogs = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCommunicationLogs?entityId=' + entityId;
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
    CRMDomainService.prototype.GetTicketOwnerPermission = function (ownerId, ownerName) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTicketOwnerPermission?ownerId=' + ownerId + '&ownerName=' + ownerName;
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
    CRMDomainService.prototype.GetOccasionsSummary = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOccasionsSummary';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new OccasionSummary();
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
    CRMDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CRMDomainService);
    return CRMDomainService;
}());
exports.CRMDomainService = CRMDomainService;
var DailySpotlightClass = /** @class */ (function () {
    function DailySpotlightClass() {
    }
    return DailySpotlightClass;
}());
exports.DailySpotlightClass = DailySpotlightClass;
var CRMSummary = /** @class */ (function () {
    function CRMSummary() {
    }
    return CRMSummary;
}());
exports.CRMSummary = CRMSummary;
var OccasionSummary = /** @class */ (function () {
    function OccasionSummary() {
    }
    return OccasionSummary;
}());
exports.OccasionSummary = OccasionSummary;
//# sourceMappingURL=CRMDomainService.js.map