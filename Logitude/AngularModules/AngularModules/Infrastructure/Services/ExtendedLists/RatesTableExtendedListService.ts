import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { RatesTableList } from '../../EntityLists/RatesTableList';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class RatesTableExtendedListService {
    private _http: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<RatesTableList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ratestableviews';
    }

    getClosestRate(baseCurrenyId: string, foreignCurrencyId: string) {
        var url = this._apiUrl + '/GetClosestRate/?' + 'baseCurrenyId=' + baseCurrenyId + '&foreignCurrencyId=' + foreignCurrencyId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: RatesTableList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

 getExchageRateByValueAndDate(baseCurrenyId: string, foreignCurrencyId: string, date: Date) {
        var url = this._apiUrl + '/GetExchageRateByValueAndDate/?' + 'baseCurrenyId=' + baseCurrenyId + '&foreignCurrencyId=' + foreignCurrencyId + '&dateString=' + ServiceHelper.GetDateString(date);

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: RatesTableList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });

    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: RatesTableList;
        entityList = new RatesTableList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
