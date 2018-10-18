import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class ReportsTemplatesVersionListExtendedService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsTemplatesVersionExtended';
    }

    getReportsTemplatesVersionListsByReportTemplateId(reportTemplateId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetReportsTemplatesVersionListsByReportTemplateId" + '?reportTemplateId=' + reportTemplateId,
            { headers: authHeader }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }

}

