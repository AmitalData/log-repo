import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { BusinessRoleList } from '../../EntityLists/BusinessRoleList';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class DWObjectTableExtendedListService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<BusinessRoleList> = [];
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/dwobjecttableextended';
    }

    GetFactTablesNames() {
        var url = this._apiUrl + '/GetFactTablesNames';

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = list;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
