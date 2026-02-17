import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import {TermsofUseSignaturePM} from '../../EntityPMs/TermsofUseSignaturePM';



@Injectable()
export class TermsofUseSignatureExtendedPM {



    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TermsOfUseSignatures';


    }


    GetTermsofUseSignatures(tenant: number, contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&contactId=' + contactId, { headers: authHeader }).map(response => {
           // var result = this.MapJsonToEntityPM(response.json()); 
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: TermsofUseSignaturePM;
        entityPM = new TermsofUseSignaturePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }






}

