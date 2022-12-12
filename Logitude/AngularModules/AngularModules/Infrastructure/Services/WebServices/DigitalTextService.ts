import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()
export class DigitalTextService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DigitalTextCode';
    }

    public GetDigitalTextCodesObjetTables() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalTextCodesObjetTables?', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    public GetTextCodesByFilters(cardId: string, objectTableId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTextCodesByFilters?cardId=' + cardId + "&objectTableId=" + objectTableId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateDigitalTextCodes(labels: DigitalTextCodeUpdateModel) {
        return defer(() => {
            return this._http.post(this._apiUrl, JSON.stringify(labels), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: any) => {
                        return response.body;
                    }),
                    catchError(ServiceHelper.HandleServiceError));

        });
    }

}

export class DigitalTextCodeUpdateModel {
    public ObjectTableId: string;
    public CardId: string;
    public Lables: DigitalTextCodeObject[];
}

export class DigitalTextCodeObject {
    public DisplayLable: string;
    public DisplayText: string;
    public Code: string;
}
