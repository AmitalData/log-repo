import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';



@Injectable()

export class ExportDeclarationClosingDatasExtendPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExportDeclarationClosingDatas';
    }

    GetSingleWithEFIFILEMData(declarationid: string) {
    
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');

        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleWithEFIFILEMData?' + 'declarationid=' + declarationid, ServiceHelper.GetHttpHeaders()).pipe(map(res => {
                serviceResponse.Result = res;
                return serviceResponse;


            }),catchError(ServiceHelper.HandleServiceError));
        });
    }



}
