import {Injectable, Injector, Inject} from '@angular/core';

import { defer, of } from 'rxjs';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {CardList} from '../../EntityLists/CardList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
@Injectable()

export class WarehouseExtendedListService {
    private _apiUrl: string;
    private _http: HttpClient;
    private CachedData: Array<CardList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/warehouseextended';
        this.CachedData = [];
    }

    getTenantImportByFilters(filters: ApiQueryFilters) {
        var urlparameters = '/getTenantImportByFilters?';
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
                var _mappedListsArray: Array<CardList> = [];
                if (viewResponse.Result) {
                    for (var key in viewResponse.Result) {

                        var entity: CardList;
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

    GetWarehouseTypeById(warehouseId) {

        return this._http.get(this._apiUrl + '/GetWarehouseTypeById/?' + 'warehouseId=' + warehouseId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


    MapJsonToEntityList(jsonList: any) {

        var entityList: CardList;
        entityList = new CardList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
}
