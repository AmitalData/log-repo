import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';

export class QueueMessagesWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QueueMessagesWebService';
    }

    UpdateTenantManagementStatistics(tenantId: number) {
        var url = this._apiUrl + '/GetUpdateTenantManagementStatistics?tenantId=' + tenantId;

        return Observable.defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                //var myJsonResult = response;
                //var serviceResponse: ServiceResponse;
                //serviceResponse = new ServiceResponse();
                //serviceResponse.Result = myJsonResult;

                return response;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
