import { defer, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { HttpClient } from '@angular/common/http';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';

export abstract class BaseService {
    public HttpClient: HttpClient;
    public BaseURL: string;
    public ApiURL: string;
    public HttpHeaders: any;
    constructor() {
        this.HttpClient = ServiceHelper.HttpClient;
        this.BaseURL = ServiceHelper.GetLogitudeURL();
        this.HttpHeaders = ServiceHelper.GetHttpHeaders()
    }

    GetServiceResponse(response: any) {
        var output = new ServiceResponse();
        output.Result = response;
        return output;
    }

    Get(url: string) {
        return this.HttpClient.get(url, this.HttpHeaders)
            .pipe(
                map(response => {
                    return response;
                }),
                catchError(ServiceHelper.HandleServiceError)
            );
    }


}
