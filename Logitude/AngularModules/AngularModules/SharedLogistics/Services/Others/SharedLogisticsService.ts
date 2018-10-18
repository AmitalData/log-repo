import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


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


   getCardLogDetails(partnerTypeId: string, dateParameter: string,   tenant: number) {

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
       return this._http.get(this._apiUrl + '/getcardlogactivitydetailslist/?' + 'cardId=' + cardId + '&contactId=' + contactId  +'&partnerTypeId=' + partnerTypeId + '&dateParameter=' + dateParameter + '&tenant=' + tenant, { headers: authHeader }).map(response => {
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
 


}

