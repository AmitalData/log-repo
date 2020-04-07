
import {Injectable} from '@angular/core';
//import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {PaymentChequeList} from '../../EntityLists/PaymentChequeList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 

@Injectable()

export class PaymentChequeExtendedListService {
   // private _http: Http
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     //   this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PaymentChequeViews';
    }


    GetPymentChequesSummary() {
        // var authHeader = new Headers();
        // authHeader.append('Token', SessionInfo.Token);


        return this.httpClient.get(this._apiUrl + '/GetPymentChequesSummary',  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;
               return allLists;
            }),
            catchError(ServiceHelper.HandleServiceError)); 

        // return Observable.defer(() => {
        //     return this._http.get(this._apiUrl + '/GetPymentChequesSummary', {
        //         headers: authHeader
        //     }).map(response => {

        //         var allLists = response.json();
        //         return allLists;
        //     });
        // });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: PaymentChequeList;
        entityList = new PaymentChequeList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}