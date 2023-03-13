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

    public GetDigitalProfileName(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalProfileName?tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
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

    public GetDigitalSubObjectsProfilesObjetTables(objectTableId) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalSubObjectsProfilesObjetTables?objectTableId=' + objectTableId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
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

    public GetFeildPermissionByFilters(cardId: string, objectTableId: string, profileCode: string, isList: boolean = null) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetFeildPermissionByFilters?cardId=' + cardId + "&objectTableId=" + objectTableId + "&profileCode=" + profileCode

                + "&isList=" + (isList != null ? isList : true) , ServiceHelper.GetHttpHeaders()).pipe(map(response => { 

                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    public GetTextCodesByFilters(cardId: string, objectTableId: string, profileCode: string, langCode: string = '') {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTextCodesByFilters?cardId=' + cardId + "&objectTableId=" + objectTableId + "&profileCode=" + profileCode + "&languageCode=" + langCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
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
    public ProfileCode: string;
    public Lables: DigitalTextCodeObject[];
    public LanguageCode: string;
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
    public ProfileCode: string;
    public ParentObjectTableId: string;
    public DefaultSettings: DigitalFeildSecurityUpdateModel[];
}

export class DigitalFeildSecurityUpdateModel {
    public FieldCode: string;
    public CreatedBy: string;
    public CreatedOn: Date;
    public ModifiedOn: Date;
    public ModifiedBy: string;
    public HasPermission: boolean;
    public IsList: boolean;
    public IsPm: boolean;
}
