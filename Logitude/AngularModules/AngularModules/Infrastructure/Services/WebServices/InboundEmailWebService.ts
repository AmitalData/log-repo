import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';


export class InboundEmailWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InboundEmailWebService';
    }

    SendInboundEmailAsync(Recepient: string, Tenant: number, Subject: string, Body: string, entityId: string) {
        var url = this._apiUrl + '/GetMessageResult?recepient=' + Recepient + '&tenant=' + Tenant + '&subject=' + Subject + '&body=' + Body + '&entityId=' + entityId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var mappedResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class InboundEmailResult {
    public Id: string;
    public IsValid: boolean;
}
