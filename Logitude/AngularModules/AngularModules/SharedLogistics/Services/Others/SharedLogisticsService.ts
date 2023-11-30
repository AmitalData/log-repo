import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class SharedLogisticsService {       
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SharedLogistics';
    }

    getSharedLogisticsStatistics(tenant: number, invitationStatusType: string) {

        return this._http.get(this._apiUrl + '/getSharedLogisticsStatistics/?' + 'tenant=' + tenant + '&invitationStatusType=' + invitationStatusType, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getSharedLogisticsSummaryData(tenant: number) {

        return this._http.get(this._apiUrl + '/getSharedLogisticsSummaryData/?' + 'tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getLastLoginPartners(tenant: number) {


        return this._http.get(this._apiUrl + '/getLastLoginPartners/?' + 'tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getCardLogDetails(partnerTypeId: string, dateParameter: string, tenant: number) {


        return this._http.get(this._apiUrl + '/getcardlogdetails/?' + 'partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getDigitalSharedLogisticsSummaryData(tenant: number) {
        return this._http.get(ServiceHelper.GetLogitudeURL() + 'api/DigitalActivity/GetDigitalSharedLogisticsSummaryData/?' + 'tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    getDigitalCardLogDetails(partnerTypeId: string, dateParameter: string, tenant: number)
    {
        return this._http.get(ServiceHelper.GetLogitudeURL() + 'api/DigitalActivity/GetDigitalCardLogDetails/?' + 'partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getCardLogActivityDetailsList(cardId: string, contactId: string, partnerTypeId: string, dateParameter: string, tenant: number) {


        return this._http.get(this._apiUrl + '/getcardlogactivitydetailslist/?' + 'cardId=' + cardId + '&contactId=' + contactId + '&partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getDigitalCardLogActivityDetailsList(cardId: string, contactId: string, partnerTypeId: string, dateParameter: string, tenant: number) {
        return this._http.get(ServiceHelper.GetLogitudeURL() + 'api/DigitalActivity/getcardlogactivitydetailslist/?' + 'cardId=' + cardId + '&contactId=' + contactId + '&partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getCustomerTenantAccessRequestStatusCount(tenant: number) {


        return this._http.get(this._apiUrl + '/GetCustomerTenantAccessRequestStatusCount/?' + 'tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetLastCustomerRequest(tenant: number) {



        return this._http.get(this._apiUrl + '/GetLastCustomerRequest/?' + 'tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetSharedShipments(filters: ShipmentFilters) {


        var url = this._apiUrl + '/GetSharedShipments?partnerId=' + filters.PartnerId + '&partnerType=' + filters.PartnerType + '&directionId=' + filters.DirectionId
            + '&transportModeId=' + filters.TransportModeId + '&levelCode=' + filters.ShipmentLevelCode + '&searchField=' + filters.SearchField
            + '&pageSize=' + filters.PageSize + '&pageIndex=' + filters.PageIndex + '&isOperationalClosed=' + filters.IsOperationalClosed;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
