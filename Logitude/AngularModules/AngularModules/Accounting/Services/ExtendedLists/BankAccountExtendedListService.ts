import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BankDepositList} from '../../EntityLists/BankDepositList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class BankAccountExtendedListService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/bankaccountviews';
    }

    
    GetBankAccountsSummary() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetBankAccountsSummary', {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: BankDepositList;
        entityList = new BankDepositList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}