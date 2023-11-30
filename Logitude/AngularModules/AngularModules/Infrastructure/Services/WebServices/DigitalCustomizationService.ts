import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { ObjectFieldList } from '../../EntityLists/ObjectFieldList';

@Injectable()
export class DigitalCustomizationService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DigitalCustomization';
    }

    public GetDigitalPortalScreens(objectTableId: string, screenCode: string, profileCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalPortalScreens?objectTableId=' + objectTableId + "&screenCode=" + screenCode + "&profileCode=" + profileCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    public GetDigitalPortalScreenNames(profileCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalPortalScreenNames?profileCode=' + profileCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
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

    AddCustomField(data: AddCustomFieldRequest) {
        return defer(() => {
            return this._http.post(this._apiUrl + "/AddCustomField", JSON.stringify(data), ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: any) => {
                        var myResult = response;
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = myResult;
                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));

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

    GetDefaultScreenLayout(objectTableId: string, screenCode: string, profileCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDefaultScreenLayout?objectTableId=' + objectTableId + "&screenCode=" + screenCode + "&profileCode=" + profileCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetObjectFieldsByFilters(filters: ApiQueryFilters) {
        var urlparameters = '/GetObjectFieldsByFilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }

        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);

        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {

                var viewResponse: ServiceResponse = response.body;
                var _mappedListsArray: Array<ObjectFieldList> = [];
                if (viewResponse.Result) {
                    for (var key in viewResponse.Result) {

                        var entity: ObjectFieldList;
                        entity = this.MapJsonToEntityList(viewResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                viewResponse.Result = _mappedListsArray;
                return viewResponse;
            }));
        }
        );
    }


    MapJsonToEntityList(jsonList: any) {
        var entityList: ObjectFieldList;
        entityList = new ObjectFieldList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
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
    public IsList : boolean;
    public ProfileId: string;
    public ProfileCode: string;
}

export class AddCustomFieldRequest {
    public ObjectTableId: string;
    public ParentObjectTableId: string;
    public ProfileId: string;
    public ProfileCode: string;
    public FieldCode: string;
    public DefaultText: string;
    public TextCode: string;
    public CreatedBy: string;
    public ModifiedBy: string;
    public DisplayText: string;
    public IsList: boolean;
    public IsPm: boolean;
}
