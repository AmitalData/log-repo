import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AccountingPeriodList} from '../../EntityLists/AccountingPeriodList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
 
@Injectable()

export class AccountingPeriodExtendedListService {
 
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/accountingperiodviews';
    }

    getByYear(year: number, typeCode: string) {

        

        return this.httpClient.get(this._apiUrl + '/getbyyear/?' + 'year=' + year + '&typeCode=' + typeCode,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
               var list = response;
               var entity: AccountingPeriodList;
               if (list) {
                     entity = this.MapJsonToEntityList(list);
               }
               var serviceResponse: ServiceResponse;
               serviceResponse = new ServiceResponse();
               serviceResponse.Result = entity;
               return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
       
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: AccountingPeriodList;
        entityList = new AccountingPeriodList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
}
