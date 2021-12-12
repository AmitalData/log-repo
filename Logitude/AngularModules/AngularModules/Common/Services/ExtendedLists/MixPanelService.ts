import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { MixPanelEvent } from 'Common/DataContracts/MixPanelEvent';


@Injectable()
export class MixPanelService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/MixPanel';
    }

    PostEvent(mixPanelEvent: MixPanelEvent) {
        return this._http.post(this._apiUrl, JSON.stringify(mixPanelEvent), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

}

