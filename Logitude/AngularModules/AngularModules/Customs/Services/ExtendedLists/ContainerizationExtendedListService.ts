import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
 import { SendCollateralRequestParams } from '../../DataContract/RequestParams/SendCollateralRequestParams';
  
@Injectable()

export class ContainerizationExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ContainerizationListExtended';
    }

    public connectedSelectAll: boolean;
    public disconnectedSelectAll: boolean;
    public isNotDirty: boolean;
    public ConnectedDeclarations: string;
    public SelectedDeclarations: boolean;

    getPromiseByFilters(filters: ApiQueryFilters) {

        return new Promise((resolve, reject) => {

            resolve(this.getByFilters(filters));

        });
    }

    getByFilters(filters: ApiQueryFilters) {
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map((response: any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
                var _mappedListsArray: Array<DeclarationList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DeclarationList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapJsonToEntityList(jsonList: any) {

        var entityList: DeclarationList;
        entityList = new DeclarationList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}
