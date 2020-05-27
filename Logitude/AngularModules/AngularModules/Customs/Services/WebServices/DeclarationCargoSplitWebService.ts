import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';
import { DeclarationCargoSplitList } from '../../EntityLists/DeclarationCargoSplitList';


@Injectable()

export class DeclarationCargoSplitWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationCargoSplitWebService';
    }

    GetDeclarationCargoSplitByDeclarationIdLists(declarationId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationCargoSplitByDeclarationIdLists/?declarationId=" + declarationId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var res = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: DeclarationCargoSplitList;
        entityPM = new DeclarationCargoSplitList();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }

}
