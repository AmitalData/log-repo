import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';


@Injectable()

export class QuantityTypeMessageService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuantityType';

    }

    GetQuantityType(classificationCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

    
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetQuantityType?' + 'classificationCode=' + classificationCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }


}