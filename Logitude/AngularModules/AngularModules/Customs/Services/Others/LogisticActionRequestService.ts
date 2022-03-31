import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationPM} from '../../EntityPMs/DeclarationPM';
import {LogisticActionRequestPM} from '../../EntityPMs/LogisticActionRequestPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { AppTool } from '../../../Infrastructure/Tools';


@Injectable()


export class LogisticActionRequestService {

    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogisticActionRequest';
    }

    GetIfLogisticActionRequestExists(Id: string, CargoIdentifierKey1: string, CargoIdentifierKey2: string, CargoIdentifierKey3: string, CargoIdentifierType: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetIfLogisticActionRequestExists?' + 'Id=' + Id + '&CargoIdentifierKey1=' + CargoIdentifierKey1 + '&CargoIdentifierKey2=' + CargoIdentifierKey2 + '&CargoIdentifierKey3=' + CargoIdentifierKey3 + '&CargoIdentifierType=' + CargoIdentifierType, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));


        });


    }


}
