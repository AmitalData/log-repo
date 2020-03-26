import {Injectable} from '@angular/core';
//import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {JournalList} from '../../EntityLists/JournalList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Token': SessionInfo.Token
    })
};

@Injectable()

export class JournalExtendedListService {
   // private _http: Http
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
      //  this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        httpOptions.headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Token': SessionInfo.Token })
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/journalviews';
    }

    GetRecentJournals() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetRecentJournals';
        return this.httpClient.get(url,  httpOptions).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
        // return Observable.defer(() => {
        //     return this._http.get(url, { headers: authHeader }).map(response => {
        //         var allLists = response.json();

        //         var serviceResponse = new ServiceResponse();
        //         serviceResponse.Result = allLists;
        //         return serviceResponse;
        //     }).catch(ServiceHelper.HandleServiceError);
        // });
    }

    GetJournalsByAccountingEntityId(entityId:string, entityCode:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

       
        var url = this._apiUrl + '/GetJournalsByAccountingEntityId?EntityId=' + entityId + '&entityCode=' + entityCode;
       
        return this.httpClient.get(url,  httpOptions).pipe(
            map(response => {
             
                var allLists = response ;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 

        // return Observable.defer(() => {
        //     return this._http.get(url, { headers: authHeader }).map(response => {
        //         var allLists = response.json();

        //         var serviceResponse = new ServiceResponse();
        //         serviceResponse.Result = allLists;
        //         return serviceResponse;
        //     }).catch(ServiceHelper.HandleServiceError);
        // });
    }

    

    GetJournalLinesByJournalId(entityId: string) {
      //  var authHeader = new Headers();
      //  authHeader.append('Token', SessionInfo.Token);


        var url = this._apiUrl + '/GetJournalLinesByJournalId?JournalId=' + entityId;

        return this.httpClient.get(url,  httpOptions).pipe(
            map(response => {
             
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
        
        // return Observable.defer(() => {
        //     return this._http.get(url, { headers: authHeader }).map(response => {
        //         var allLists = response.json();

        //         var serviceResponse = new ServiceResponse();
        //         serviceResponse.Result = allLists;
        //         return serviceResponse;
        //     }).catch(ServiceHelper.HandleServiceError);
        // });
    }

    GetByJournalNumber(journalNumber) {

      //  var authHeader = new Headers();
      //  authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetByJournalNumber?journalNumber=' + journalNumber;

        return this.httpClient.get(url,  httpOptions).pipe(
            map(response => {
             
                var list = response ;

                var entity: JournalList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 

        // return Observable.defer(() => {
        //     return this._http.get(url, { headers: authHeader }).map(response => {

        //         var list = response.json();

        //         var entity: JournalList;
        //         if (list) {
        //             entity = this.MapJsonToEntityList(list);
        //         }
        //         var serviceResponse: ServiceResponse;
        //         serviceResponse = new ServiceResponse();
        //         serviceResponse.Result = entity;
        //         return serviceResponse;

        //     }).catch(ServiceHelper.HandleServiceError);
        // });

    }
    
    GetJournalsSummary() {
       // var authHeader = new Headers();
        //authHeader.append('Token', SessionInfo.Token);

        return this.httpClient.get(this._apiUrl + '/GetJournalsSummary?',  httpOptions).pipe(
            map(response => {
                var allLists = response;
                return allLists;
            }),
            catchError(ServiceHelper.HandleServiceError)); 


        // return Observable.defer(() => {
        //     return this._http.get(this._apiUrl + '/GetJournalsSummary?', {
        //         headers: authHeader
        //     }).map(response => {

        //         var allLists = response.json();
        //         return allLists;
        //     });
        // });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: JournalList;
        entityList = new JournalList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}