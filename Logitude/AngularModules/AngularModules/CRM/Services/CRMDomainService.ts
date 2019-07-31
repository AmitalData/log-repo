import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {TicketPM} from '../EntityPMs/TicketPM';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {SLAHeaderPM} from '../EntityPMs/SLAHeaderPM';
import {SLALinePM} from '../EntityPMs/SLALinePM';
import {SLAEscalationPM} from '../EntityPMs/SLAEscalationPM';
import {SLAEscalationRecepientPM} from '../EntityPMs/SLAEscalationRecepientPM';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass'; 
import {Guid} from '../../Infrastructure/Utilities/Guid';

@Injectable()

export class CRMDomainService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CRMDomain';
    }

    InserNewTicket(entityPM: TicketPM) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Ticket", entityPM);


            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TicketPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl + '/InserNewTicket?entityPM=' + entityPM, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: TicketPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = mappedResult;
                        }



                        return response;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );

    }

    GetOwnerEmployeeGroup(ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetOwnerEmployeeGroup?ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetContactCards(companyId: string, contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetContactCards?companyId=' + companyId + '&contactId=' + contactId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetConnectContactCards(companyId: string, contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetConnectContactCards?companyId=' + companyId + '&contactId=' + contactId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetActivitiesDashBoard(ownerId: string, businessUnitId: string, activityTypeCodeFilter: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetActivitiesDashBoard?OwnerId=' + ownerId + '&BusinessUnitId=' + businessUnitId + '&activityTypeCodeFilter=' + activityTypeCodeFilter + '&RecordsTypeCode=' + RecordsTypeCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOpportunitiesChartDataCustom(FromDate: Date, ToDate: Date, ownerId: string, businessUnitId: string, chartCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetOpportunitiesChartDataCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOpportunitiesChartData(code: string, ownerId: string, businessUnitId: string, chartCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetOpportunitiesChartData?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId +'&chartCode=' + chartCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetActivitiesChartDataCustom(FromDate: Date,ToDate:Date, ownerId: string, businessUnitId: string, chartCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetActivitiesChartDataCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) +'&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetActivitiesChartData(code: string, ownerId: string, businessUnitId: string, chartCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetActivitiesChartData?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetQuotesChartDataCustom(FromDate: Date, ToDate: Date, ownerId: string, businessUnitId: string, chartCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetQuotesChartDataCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate)+ '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetQuotesChartData(code: string, ownerId: string, businessUnitId: string, chartCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetQuotesChartData?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&chartCode=' + chartCode;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetOpenedTicketsGroupByClassification(code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetOpenedTicketsGroupByClassification?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetOpenedTicketsGroupBySeverity(code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetOpenedTicketsGroupBySeverity?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetOpenedTicketsGroupByOwner(code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsGroupByOwner?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetOpenedTicketsBySLAViolation(selectedIndex: number, code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsBySLAViolation?selectedIndex=' +selectedIndex+'&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetOpenedTicketsByOpenedStage(selectedIndex: number, code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenedTicketsByOpenedStage?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetClosedTicketsGroupByClassification(code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetClosedTicketsGroupByClassification?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetClosedTicketsGroupBySeverity(code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetClosedTicketsGroupBySeverity?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetClosedTicketsGroupByType(code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsGroupByType?code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetClosedTicketsBySLAViolation(selectedIndex: number, code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsBySLAViolation?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetClosedTicketsBySolvedStage(selectedIndex: number, code: string, ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetClosedTicketsBySolvedStage?selectedIndex=' + selectedIndex + '&code=' + code + '&ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOpenTicketsGroupByClassification(ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenTicketsGroupByClassification?ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOpenTicketsByDueTime(ownerId: string, employeeGroupId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpenTicketsByDueTime?ownerId=' + ownerId + '&employeeGroupId=' + employeeGroupId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetTicketOverviewPerformance(ticketId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTicketOverviewPerformance?ticketId=' + ticketId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    

    
    GetRecentTickets(myOwnerId: string, myEmployeeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetRecentTickets?ownerId=' + myOwnerId + '&employeeGroupId=' + myEmployeeId, {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                return allLists;
            });
        });
    }

    GetRecentOpportunities(myOwnerId: string, myEmployeeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetRecentOpportunities?ownerId=' + myOwnerId + '&businessUnitId=' + myEmployeeId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }

    GetCorrespondencesList(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketCorrespondences?entityId=' + entityId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }

    GetTicketsCounts(myOwnerId: string, myEmployeeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketsCounts?ownerId=' + myOwnerId + '&employeeGroupId=' + myEmployeeId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }

    GetTopTickets(myOwnerId: string, myEmployeeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTopTickets?ownerId=' + myOwnerId + '&employeeGroupId=' + myEmployeeId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }

    GetTicketsCountByShipmentNumber(shipmentNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketsCountByShipmentNumber?shipmentNumber=' + shipmentNumber, {
                headers: authHeader
            }).map(response => {
                var count = response.json();
                return count;
            });
        });
    }

    GetEmployeeGroupsPMList() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        //authHeader.append('Token', sessionStorage.getItem("Token"));
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetEmployeeGroupsPMList?', {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            });
        });
    }
 

    GetUsersByEmployeeGroupIds(employeeIds: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        //authHeader.append('Token', sessionStorage.getItem("Token"));

        var url = this._apiUrl + '/GetUsersByEmployeeGroupIds?employeeIds=' + employeeIds;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetContactListsByEmailsString(emails: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetContactListsByEmailsString?emails=' + emails;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetUserListsByEmailsString(emails: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetUserListsByEmailsString?emails=' + emails;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetTicketEscalationListsByTicketId(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketEscalationListsByTicketId?entityId=' + entityId, {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            });
        });
    }

    GetTicketOverViewStatisticsSummary(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketOverViewStatisticsSummary?entityId=' + entityId, {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            });
        });
    }

    GetBusinessHours(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetBusinessHours?entityId=' + entityId, {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            });
        });
    }

    GetOpportunitiesSummary(ownerId: string, businessUnitId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesSummary?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new CRMSummary();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TicketPM = null) {
        if (!entityPM) {

            entityPM = new TicketPM();
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
    }

    public clone(jsonPM: any) {
        var entityPM: any;
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
    }

    GetCRMDailySpotlightCounts(ownerId: string, businessUnitId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCRMDailySpotlightCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new DailySpotlightClass();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetUpcomigActivities(ownerId: string, businessUnitId: string, activityTypeCodeFilter: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpcomigActivities?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&activityTypeCodeFilter=' + activityTypeCodeFilter + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetStageFunnelData(ownerId: string, businessUnitId: string, filterCode: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetStageFunnelData?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&filterCode=' + filterCode + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCompleteActivity(activityId: string, post: boolean, summary: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCompleteActivity?activityId=' + activityId + '&post=' + post + '&summary=' + summary;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var activity = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = activity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetReopenActivity(activityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetReopenActivity?activityId=' + activityId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var activity = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = activity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetActivitiesSummary(activityTypeCodeFilter: string, ownerId: string, businessUnitId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesSummary?activityTypeCodeFilter=' + activityTypeCodeFilter + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new CRMSummary();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetActiveSLAbyTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetActiveSLAbyTenant?', {
                headers: authHeader
            }).map(response => {

                var result = response.json();
                var entity: SLAHeaderPM;
                var allLists: SLAHeaderPM[];
                allLists = new Array<SLAHeaderPM>();
                result.forEach((item) => {
                    entity = this.MapJsonToSLAHeaderEntityPM(item);
                    allLists.push(entity);
                });
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = allLists;
                return pmresponse;

                //var allLists = response.json();
                //var myResponse: ServiceResponse;
                //myResponse = new ServiceResponse();
                //myResponse.Result = allLists;
                //return myResponse;
            });
        });
    }
    GetSingleSLAHeaderPMByTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleSLAHeaderPMByTenant?', {
                headers: authHeader
            }).map(response => {
                var pm = response.json();
                var entity: SLAHeaderPM;
                if (pm) {
                    entity = this.MapJsonToSLAHeaderEntityPM(pm);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            });
        });
    }
    MapJsonToSLAHeaderEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: SLAHeaderPM = null) {

        if (!entityPM) {

            entityPM = new SLAHeaderPM();
        }

        var customFields: Array<string> = [];
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
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
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
                var newSLALinePM: SLALinePM = this.clone(mySLALinePM);


                entityPM.OldEntityPM.SLALines.push(newSLALinePM);
            }

            entityPM.OldEntityPM.SLAEscalations = [];
            for (var item in entityPM.SLAEscalations) {
                var mySLAEscalationPM = entityPM.SLAEscalations[item];
                var newSLAEscalationPM: SLAEscalationPM = this.clone(mySLAEscalationPM);

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
    }
    MapSLALines(entityPM: SLAHeaderPM, jsonPM: any, mapParent: boolean = true) {

        var oldSLALines: SLALinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSLALines = entityPM.OldEntityPM.SLALines;
        }

        entityPM.SLALines = new Array<SLALinePM>();
        for (var item in jsonPM.SLALines) {
            var jItem = jsonPM.SLALines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSLALinePM: SLALinePM;

            if (mapParent) {
                newSLALinePM = new SLALinePM(entityPM);
            }
            else {
                newSLALinePM = new SLALinePM(null);
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
                newSLALinePM.UniqueKey = Guid.newGuid();
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
                if (entityPM.SLALines.filter(p => p.UniqueKey === oldSLALines[itemKey].UniqueKey).length === 0) {

                    if (oldSLALines[itemKey]) {
                        //oldSLALines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SLALines.push(oldSLALines[itemKey]);
                        var oldItemJson = oldSLALines[itemKey];
                        var deletedPM: SLALinePM = new SLALinePM(null);
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
    }
    MapSLAEscalations(entityPM: SLAHeaderPM, jsonPM: any, mapParent: boolean = true) {

        var oldSLAEscalations: SLAEscalationPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSLAEscalations = entityPM.OldEntityPM.SLAEscalations;
        }

        entityPM.SLAEscalations = new Array<SLAEscalationPM>();
        for (var item in jsonPM.SLAEscalations) {
            var jItem = jsonPM.SLAEscalations[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSLAEscalationPM: SLAEscalationPM;

            if (mapParent) {
                newSLAEscalationPM = new SLAEscalationPM(entityPM);
            }
            else {
                newSLAEscalationPM = new SLAEscalationPM(null);
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
                newSLAEscalationPM.UniqueKey = Guid.newGuid();
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
                if (entityPM.SLAEscalations.filter(p => p.UniqueKey === oldSLAEscalations[itemKey].UniqueKey).length === 0) {

                    if (oldSLAEscalations[itemKey]) {
                        //oldSLAEscalations[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SLAEscalations.push(oldSLAEscalations[itemKey]);
                        var oldItemJson = oldSLAEscalations[itemKey];
                        var deletedPM: SLAEscalationPM = new SLAEscalationPM(null);
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
    }
    MapSLAEscalationRecepients(entityPM: SLAEscalationPM, jsonPM: any, mapParent: boolean = true) {

        var oldSLAEscalationRecepients: SLAEscalationRecepientPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSLAEscalationRecepients = entityPM.OldEntityPM.SLAEscalationRecepients;
        }

        entityPM.SLAEscalationRecepients = new Array<SLAEscalationRecepientPM>();
        for (var item in jsonPM.SLAEscalationRecepients) {
            var jItem = jsonPM.SLAEscalationRecepients[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSLAEscalationRecepientPM: SLAEscalationRecepientPM;

            if (mapParent) {
                newSLAEscalationRecepientPM = new SLAEscalationRecepientPM(entityPM);
            }
            else {
                newSLAEscalationRecepientPM = new SLAEscalationRecepientPM(null);
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
                newSLAEscalationRecepientPM.UniqueKey = Guid.newGuid();
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
                if (entityPM.SLAEscalationRecepients.filter(p => p.UniqueKey === oldSLAEscalationRecepients[itemKey].UniqueKey).length === 0) {

                    if (oldSLAEscalationRecepients[itemKey]) {
                        //oldSLAEscalationRecepients[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SLAEscalationRecepients.push(oldSLAEscalationRecepients[itemKey]);
                        var oldItemJson = oldSLAEscalationRecepients[itemKey];
                        var deletedPM: SLAEscalationRecepientPM = new SLAEscalationRecepientPM(null);
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
    }

    UpdatingActivityMettingSummary(mettingSummary: string, post: boolean, activityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpdatingActivityMettingSummary?mettingSummary=' + mettingSummary + '&post=' + post + '&activityId=' + activityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetActivitiesByOpportunityId(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesByOpportunityId?entityId=' + entityId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetActivitiesByTicketId(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesByTicketId?entityId=' + entityId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetQuotesGroupBySalesman(code: string, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesGroupBySalesman?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetQuotesGroupBySalesmanCustom(FromDate: Date, ToDate: Date, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesGroupBySalesmanCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCustomersGroupBySalesmanCustom(FromDate: Date, ToDate: Date, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersGroupBySalesmanCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCustomersGroupBySalesman(days: number, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersGroupBySalesman?days=' + days + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetActivitiesGroupBySalesmanCustom(FromDate: Date,ToDate:Date, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesGroupBySalesmanCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetActivitiesGroupBySalesman(code: string, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesGroupBySalesman?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOpportunitiesGroupBySalesmanCustom(FromDate: Date,ToDate:Date, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesGroupBySalesmanCustom?FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOpportunitiesGroupBySalesman(code: string, ownerId: string, businessUnitId: string, fieldCode: string, isTopTen: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOpportunitiesGroupBySalesman?code=' + code + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&fieldCode=' + fieldCode + '&isTopTen=' + isTopTen;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetUpdateCorrespondence(entityId: string, rightToLeft: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateCorrespondence?entityId=' + entityId + '&rightToLeft=' + rightToLeft;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCommunicationLogs(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCommunicationLogs?entityId=' + entityId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetTicketOwnerPermission(ownerId: string, ownerName:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTicketOwnerPermission?ownerId=' + ownerId + '&ownerName=' + ownerName ;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetOccasionsSummary() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOccasionsSummary';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new OccasionSummary();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

}

export class DailySpotlightClass {
    public Id: number;

    public Quotes_Today: number;
    public Quotes_Yesterday: number;
    public Quotes_LastWeek: number;

    public PotentialCustomers_Today: number;
    public PotentialCustomers_Yesterday: number;
    public PotentialCustomers_LastWeek: number;

    public Customers_Today: number;
    public Customers_Yesterday: number;
    public Customers_LastWeek: number;

    public Activities_Today: number;
    public Activities_Yesterday: number;
    public Activities_LastWeek: number;

    public Opportunities_Today: number;
    public Opportunities_Yesterday: number;
    public Opportunities_LastWeek: number;
}

export class CRMSummary {
    public Id: number;
    public MyOpenDataCount: number;
    public AllOpenDataCount: number;
    public OpenByStageCount: number;
}

export class OccasionSummary {
    public Id: number;
    public AllOccasionsCount: number;
}
