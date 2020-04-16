import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AccountingNoteList } from '../../EntityLists/AccountingNoteList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AppTool } from '../../../Infrastructure/Tools';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class AccountingNoteExtendedListService {
  
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
 
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingNoteViews';
    }


    GetNotesByCard(cardId: string) {
  
        var url = this._apiUrl + '/GetNotesByCard?cardId=' + cardId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        
    }

    DeleteNote(noteId: string) {
     

        var url = ServiceHelper.GetLogitudeURL() + 'api/AccountingNotes' + '/PostDeleteNote?noteId=' + noteId;
        return this.httpClient.post(url, null,ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse = new ServiceResponse();
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        

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
