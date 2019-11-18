import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionInfo} from '../../Utilities/SessionInfo';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { Observable } from 'rxjs';


@Injectable()

export class BIReportExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BIReportsExtended';
    }

    DoesReportExist(name: string, folderId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        let callUrl = this._apiUrl + '/getReportExist?' + 'name=' + name + '&folderId=' + folderId;
        return this._http.get(callUrl, {
            headers: authHeader
        }).map(response => {
            var result = response.json();

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = result;

            return serviceResponse;

        }).catch(ServiceHelper.HandleServiceError);

    }

}
