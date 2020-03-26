import { Injectable } from '@angular/core';
//import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AccountingNoteList } from '../../EntityLists/AccountingNoteList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AppTool } from '../../../Infrastructure/Tools';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Token': SessionInfo.Token
    })
};


@Injectable()

export class AccountingNoteExtendedListService {
  //  private _http: Http
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     //   this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        httpOptions.headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Token': SessionInfo.Token })
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingNoteViews';
    }


    GetNotesByCard(cardId: string) {
      //  var authHeader = new Headers();
     //   authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetNotesByCard?cardId=' + cardId;

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

    DeleteNote(noteId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = ServiceHelper.GetLogitudeURL() + 'api/AccountingNotes' + '/PostDeleteNote?noteId=' + noteId;
        return this.httpClient.post(url, null,httpOptions).pipe(
            map(response => {
                var serviceResponse = new ServiceResponse();
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

        //     var serviceResponse: ServiceResponse = new ServiceResponse();

        //     return this._http.post(url, null ,{ headers: authHeader }).map((res) => {

        //             return serviceResponse;

        //         }).catch(ServiceHelper.HandleServiceError);
        // });

    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: AccountingNoteList;
        entityList = new AccountingNoteList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}
