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

    public GetDigitalProfileName() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalProfileName?', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    public GetDigitalProfilesObjetTables() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalProfilesObjetTables?', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
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

    public GetFeildPermissionByFilters(cardId: string, objectTableId: string, profileId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetFeildPermissionByFilters?cardId=' + cardId + "&objectTableId=" + objectTableId + "&profileId=" + profileId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    public GetTextCodesByFilters(cardId: string, objectTableId: string, profileId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTextCodesByFilters?cardId=' + cardId + "&objectTableId=" + objectTableId + "&profileId=" + profileId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
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
            return this._http.post(this._apiUrl + "/UpdateTextCodes", JSON.stringify(labels), ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: any) => {
                        
                    }),
                    catchError(ServiceHelper.HandleServiceError));

        });
    }

    UpdateFeildPermission(labels: DigitalFeildSecurityObjectModel) {
        
        return defer(() => {
            return this._http.post(this._apiUrl + "/UpdateFeildPermission", JSON.stringify(labels), ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: any) => {
                       
                    }),
                    catchError(ServiceHelper.HandleServiceError));

        });
    }
}

export class DigitalTextCodeUpdateModel {
    public ObjectTableId: string;
    public CardId: string;
    public ProfileId: string;
    public Lables: DigitalTextCodeObject[];
}

export class DigitalTextCodeObject {
    public DisplayText: string;
    public DefaultText: string;
    public TextCode: string;
    public FieldCode: string;
}

export class DigitalFeildSecurityObjectModel {
    public ObjectTableId: string;
    public CardId: string;
    public ProfileId: string;
    public DefaultSettings: DigitalFeildSecurityUpdateModel[];
}

export class DigitalFeildSecurityUpdateModel {
    public FieldCode: string;
}
