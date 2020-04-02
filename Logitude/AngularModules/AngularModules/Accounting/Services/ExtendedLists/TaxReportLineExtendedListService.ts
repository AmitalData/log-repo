import {Injectable} from '@angular/core';
//import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
// import {TaxReportLineList} from '../../EntityLists/TaxReportLineList';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 

@Injectable()

export class TaxReportLineExtendedListService {
  //  private _http: Http
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
    //    this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxReportOp';
    }

    getByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/GetLinesByFilters?';
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

      //  var authHeader = new Headers();
      //  authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//

        return this.httpClient.get(callUrl,  ServiceHelper.GetHttpHeaders()).pipe(
            map((response:ServiceResponse) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = response;

                //console.log("serviceResponse: ", serviceResponse);

                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
        // return Observable.defer(() => {
        //     return this._http.get(callUrl, {
        //         headers: authHeader
        //     }).map(response => {

        //         var serviceResponse: ServiceResponse;
        //         serviceResponse = response.json();

        //         //console.log("serviceResponse: ", serviceResponse);

        //         //serviceResponse.Result = _mappedListsArray;
        //         return serviceResponse;
        //     }).catch(ServiceHelper.HandleServiceError);
        // });
    }

    // MapJsonToEntityList(jsonList: any) {

    //     var entityList: TaxReportLineList;
    //     entityList = new TaxReportLineList();
    //     var jsonListKeys = Object.keys(jsonList);

    //     for (var key in jsonListKeys) {
    //         var property = jsonListKeys[key];
    //         entityList[property] = jsonList[property];
    //     }


    //     return entityList;
    // }


}
