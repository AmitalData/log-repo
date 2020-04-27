import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import {TermsofUseSignaturePM} from '../../EntityPMs/TermsofUseSignaturePM';



@Injectable()
export class TermsofUseSignatureExtendedPM {



    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TermsOfUseSignatures';


    }


    GetTermsofUseSignatures(tenant: number, contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&contactId=' + contactId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
           // var result = this.MapJsonToEntityPM(response.json()); 
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
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

