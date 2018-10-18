import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {RatesTablePM} from '../../Infrastructure/EntityPMs/RatesTablePM';
import {Observable} from 'rxjs/Rx';

export class LastRate {
    Id: string;
    Tenant: number;
    ForeignCurrencyId: string;
    ForeignCurrencyCode: string;
    ForeignCurrencyName: string;
    BaseCurrencyId: string;
    BaseCurrencyCode: string;
    ValueDate: Date;
    LogDateTime: Date;
    Rate: number;
    HistoryCount: number;
}

@Injectable()

export class CurrencyRatesService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/currencyrates';
    }

    getAll(baseCurrencyId: string, date: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/getall?baseCurrencyId=' + baseCurrencyId + '&dateString=' + ServiceHelper.GetDateString(date);

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<LastRate> = [];

                for (var key in allLists) {
                    var entity: LastRate;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCurrenciesExchangeRateByValueDate(currencyId: string, loadingDate: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCurrenciesExchangeRateByValueDate?currencyId=' + currencyId + '&dateString=' + ServiceHelper.GetDateString(loadingDate);

        return Observable.defer(() => {

            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<LastRate> = [];

                for (var key in allLists) {

                    var entity: LastRate;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);

                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetRatesByValueDate(currencyId: string, date: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRatesByValueDate?currencyId=' + currencyId + '&dateString=' + ServiceHelper.GetDateString(date);

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<RatesTablePM> = [];

                for (var key in allLists) {

                    var entity: RatesTablePM;
                    entity = this.MapJsonToRatesTableList(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    InsertListOfRatesTable(ratesTables: LastRate[]) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetInsertListOfRatesTable?ratesTables=' + ratesTables;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    MapJsonToEntityList(jsonList: any) {
        var entityList: LastRate;
        entityList = new LastRate();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapJsonToRatesTableList(jsonList: any) {
        var entityList: RatesTablePM;
        entityList = new RatesTablePM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}