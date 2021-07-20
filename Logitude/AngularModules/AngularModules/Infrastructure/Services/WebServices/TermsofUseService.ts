import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { TermsofUsePM } from '../../../Common/EntityPMs/TermsofUsePM'; 
import { defer, of } from 'rxjs'; 

@Injectable()
export class TermsofUseService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TermsofUse';
    }

    insert(entityPM: TermsofUsePM) { 
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
              
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
           
            var mappedEntity: TermsofUsePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: TermsofUsePM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            return serviceResponse;

                        }), catchError(ServiceHelper.HandleServiceError));
           
        });
    }

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: TermsofUsePM = null) {


        if (!entityPM) {

            entityPM = new TermsofUsePM();
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

        if (getCallMap) {
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

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

    GetCheckIfGoToTermUseComponent(tenant: number, userId: string) {
        var url = this._apiUrl + '?tenant=' + tenant + '&userId=' + userId;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTermOfUseByPrivateLabeldId(privateLabeldId: string) {
        var url = this._apiUrl + '?privateLabeldId=' + privateLabeldId;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


    GetSingle(id: number, tenant: number) { 
        var url = this._apiUrl + '?id=' + id + '&tenant=' + tenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

}
