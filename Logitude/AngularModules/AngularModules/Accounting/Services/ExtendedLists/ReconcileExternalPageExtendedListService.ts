import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';

@Injectable()

export class ReconcileExternalPageExtendedListService {
    private _http: Http
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconcileExternalPages';
    }


    getExternalReoncilioationsByFilter(bankAccountId: string, filters: ApiQueryFilters) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + "/getExternalReoncilioationsByFilter";

        var urlparameters = '?bankAccountId=' + bankAccountId;

        // Parse Filters into URI
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var callTime = new Date(); 
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
        // End Parse
       

        var callUrl = url.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();
                console.log("serviceResponse: ", serviceResponse);

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getBankPageLinesByIds(Ids: string[]) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var params: string = ""; 
        for (var id of Ids) {
            params += "Ids[]=" + id + "&";
        }

        var url = this._apiUrl + '/getBankPageLinesByIds?' + params;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: LedgerTransactionList;
        entityList = new LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }


}