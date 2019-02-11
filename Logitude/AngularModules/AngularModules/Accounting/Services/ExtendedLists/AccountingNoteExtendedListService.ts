import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AccountingNoteList } from '../../EntityLists/AccountingNoteList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AppTool } from '../../../Infrastructure/Tools';

@Injectable()

export class AccountingNoteExtendedListService {
    private _http: Http
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingNoteViews';
    }


    GetNotesByCard(cardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetNotesByCard?cardId=' + cardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    DeleteNote(noteId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = ServiceHelper.GetLogitudeURL() + 'api/AccountingNotes' + '/PostDeleteNote?noteId=' + noteId;

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.post(url, null ,{ headers: authHeader }).map((res) => {

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });

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
