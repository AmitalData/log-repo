
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import { CardPM } from '../../EntityPMs/CardPM';


@Injectable()

export class CardExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CardExtended';
    }


    DisconnectGLAccountFromCard(id: string, partnerTypeId: string, eventTypeCode:string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDisconnectGLAccountFromCard?' + 'id=' + id + '&partnerTypeId=' + partnerTypeId + '&eventTypeCode=' + eventTypeCode, {
                headers: authHeader
            }).map(response => {
                var result = response.json();
                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;



                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }





}
