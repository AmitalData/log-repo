import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer } from 'rxjs';

@Injectable()
export class ExpenseAllocationSettingExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExpenseAllocationSettingExtended';
    }

    getExpenseAllocationSettingByEntityIdAndObjectTable(entityId: string, objectTableId: string) {
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleByEntityIdAndObjectTable/?' + '&entityId=' + entityId + '&objectTableId=' + objectTableId, ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    
   

    
}
 