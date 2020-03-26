import {Injectable} from '@angular/core';
//import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CashBookList} from '../../EntityLists/CashBookList';
import {CashBookStatusChart} from '../../DataContracts/CashBookStatusChart';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class CashBookExtendedListService {
   // private _http: Http
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
       // this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CashBookViews';
    }

    GetCashBookStatusChartData() {
        // var authHeader = new Headers();
        // authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetCashBookStatusChartData';

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
           

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                var _mappedListsArray: Array<CashBookStatusChart> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CashBookStatusChart;
                        entity = this.MapJsonToCashBookStatusChart(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

        // return Observable.defer(() => {
        //     return this._http.get(url, { headers: authHeader }).map(response => {


        //         var serviceResponse: ServiceResponse = new ServiceResponse();
        //         serviceResponse.Result = response.json();
        //         var _mappedListsArray: Array<CashBookStatusChart> = [];
        //         if (serviceResponse.Result) {
        //             for (var key in serviceResponse.Result) {

        //                 var entity: CashBookStatusChart;
        //                 entity = this.MapJsonToCashBookStatusChart(serviceResponse.Result[key]);
        //                 _mappedListsArray.push(entity);

        //             }
        //         }

        //         serviceResponse.Result = _mappedListsArray;
        //         return serviceResponse;
        //     }).catch(ServiceHelper.HandleServiceError);
        // });
    }

    GetCashBookSummary() {
        
        
       // var authHeader = new Headers();
      //  authHeader.append('Token', SessionInfo.Token);

        
        return this.httpClient.get(this._apiUrl + '/GetCashBookSummary?',  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;
                return allLists;
            }),
            catchError(ServiceHelper.HandleServiceError)); 


        // return Observable.defer(() => {
        //     return this._http.get(this._apiUrl + '/GetCashBookSummary?', {
        //         headers: authHeader
        //     }).map(response => {

        //         var allLists = response.json();
        //         return allLists;
        //     });
        // });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: CashBookList;
        entityList = new CashBookList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToCashBookStatusChart(jsonList: any) {

        var entityList: CashBookStatusChart;
        entityList = new CashBookStatusChart();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}
