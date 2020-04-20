import {Injectable} from '@angular/core';

import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BankDepositList} from '../../EntityLists/BankDepositList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class BankAccountExtendedListService {
 
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/bankaccountviews';
    }

    
    GetBankAccountsSummary() {
      

        return this.httpClient.get(this._apiUrl + '/GetBankAccountsSummary',  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;
                 return allLists;
            }),
            catchError(ServiceHelper.HandleServiceError));
       
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
