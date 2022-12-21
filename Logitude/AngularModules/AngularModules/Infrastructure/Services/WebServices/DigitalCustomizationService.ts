import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()
export class DigitalCustomizationService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DigitalCustomization';
    }

    public GetDigitalPortalScreens(objectTableId: string, screenCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalPortalScreens?objectTableId='  + objectTableId + "&screenCode=" + screenCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    public GetDigitalPortalScreenNames() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalPortalScreenNames?', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    public GetDigitalPreDefinedComponents(objectTableId: string, name: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalPreDefinedComponents?objectTableId=' + objectTableId + "&name=" + name, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateDigitalPortalScreen(data: DigitalPortalScreenUpdateModel) {
        return defer(() => {
            return this._http.post(this._apiUrl + "/UpdateDigitalPortalScreen", JSON.stringify(data), ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: any) => {

                    }),
                    catchError(ServiceHelper.HandleServiceError));

        });
    }

    ResetToDefault() {
        
    }

}

export class DigitalPortalScreenUpdateModel {
    public Id: string;
    public Tenant: number;
    public ObjectTableId: string;
    public ScreenCode: string;
    public Name: string;
    public Content: string;
    public DraftContent: string;
    public IsDraft: boolean;
}
