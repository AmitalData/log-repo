import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { LastRunDetailPM } from '../../EntityPMs/LastRunDetailPM';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class LastRunDetailExtendedPMService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/lastrundetailsextended/';
    }

    UpdateLastRunDetails(entityPM: LastRunDetailPM, userId: string) {
        var url = this._apiUrl + "?userid=" + userId;
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        return this._httpClient.put(url, entityPM, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
}
