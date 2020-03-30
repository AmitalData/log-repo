import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class BIReportExtendedPMService {
    private httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BIReportsExtended';
    }

    DoesReportExist(name: string, folderId: string)  {
        var url = this._apiUrl + '/getReportExist?' + 'name=' + name + '&folderId=' + folderId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
}
