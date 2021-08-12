
import { Injectable } from '@angular/core';

import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { CardPM } from '../../EntityPMs/CardPM';


@Injectable()

export class CardExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CardExtended';
    }


    DisconnectGLAccountFromCard(id: string, partnerTypeId: string, eventTypeCode:string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDisconnectGLAccountFromCard?' + 'id=' + id + '&partnerTypeId=' + partnerTypeId + '&eventTypeCode=' + eventTypeCode,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;



                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    GetAllConnectedPartnersByGLAccountId(glAccountId: string) {

        var url = this._apiUrl + '/GetAllConnectedPartnersByGLAccountId?glAccountId=' + glAccountId;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

    GetAllCardsByVatNumber(vatNumber: string) {

        var url = this._apiUrl + '/GetAllCardsByVatNumber?vatNumber=' + vatNumber  ;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

}
