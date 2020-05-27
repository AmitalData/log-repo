import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';

export class FFRWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FFRWebService';
    }

    Send(myBookingId: string, myTenant: number, myRecipient: string, isCancellationSent: boolean) {
        var url = this._apiUrl + '/GetMessageResult?myBookingId=' + myBookingId + '&myTenant=' + myTenant + '&myRecipient=' + myRecipient + '&isCancellationSent=' + isCancellationSent;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: FFRResult = new FFRResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;

                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class FFRResult {
    public Id: string;
    public IsValid: boolean;
    public IsDemoTenant: boolean;
    public HasStockError: boolean;
}
