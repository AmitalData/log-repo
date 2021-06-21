import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';


@Injectable()

export class ContainerizationMessagesService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ContainerizationWebService';
        
    }

    SendContainerization(genericRequestParams: GenericRequestParams) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/SendContainerization/',
                JSON.stringify(genericRequestParams),
                ServiceHelper.GetHttpHeaders()).pipe(map((res: any) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


}
