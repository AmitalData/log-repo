import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { filter } from 'rxjs/operator/filter';


@Injectable()
export class SharedLogisticsService {       
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SharedLogistics';
    }

    getSharedLogisticsStatistics(tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getSharedLogisticsStatistics/?' + 'tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getSharedLogisticsSummaryData(tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getSharedLogisticsSummaryData/?' + 'tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getLastLoginPartners(tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getLastLoginPartners/?' + 'tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getCardLogDetails(partnerTypeId: string, dateParameter: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getcardlogdetails/?' + 'partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getCardLogActivityDetailsList(cardId: string, contactId: string, partnerTypeId: string, dateParameter: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getcardlogactivitydetailslist/?' + 'cardId=' + cardId + '&contactId=' + contactId + '&partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getCustomerTenantAccessRequestStatusCount(tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/GetCustomerTenantAccessRequestStatusCount/?' + 'tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetLastCustomerRequest(tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/GetLastCustomerRequest/?' + 'tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetSharedShipments(filters: ShipmentFilters) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSharedShipments?partnerId=' + filters.PartnerId + '&partnerType=' + filters.PartnerType + '&directionId=' + filters.DirectionId
            + '&transportModeId=' + filters.TransportModeId + '&levelCode=' + filters.ShipmentLevelCode + '&searchField=' + filters.SearchField
            + '&pageSize=' + filters.PageSize + '&pageIndex=' + filters.PageIndex + '&isOperationalClosed=' + filters.IsOperationalClosed;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}

export class ShipmentFilters {
    public PartnerId: string;
    public PartnerType: string;
    public DirectionId: string;
    public TransportModeId: string;
    public ShipmentLevelCode: string;
    public IsOperationalClosed: boolean;
    public SearchField: string;
    public PageSize: number;
    public PageIndex: number;
    public IsShipmentTracking: boolean;
    public QueryType: string;
    public ContactId: string;
}
