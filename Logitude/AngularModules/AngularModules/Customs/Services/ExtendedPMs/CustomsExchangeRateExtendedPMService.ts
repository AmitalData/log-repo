import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsExchangeRatePM} from '../../EntityPMs/CustomsExchangeRatePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsExchangeRateExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsExchangeRatesExtended';
    }

    GetCustomsExchangeRateForCurrencyAndDate(currencyTypeCodes: string, date: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsExchangeRateForCurrencyAndDate/?' + 'currencyTypeCode=' + currencyTypeCodes + '&date=' + date,  { headers: authHeader }).map(response => {


                var allLists = response.json();
                var _mappedListsArray: Array<CustomsExchangeRatePM> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: CustomsExchangeRatePM;
                        entity = this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    

    GetCustomsExchangeRateForDate(date: Date) {
        var authHeader = new Headers();
        var stringDate:any = date;
        if (date instanceof Object) {
            stringDate = date.toJSON();
        }
        ///var stringDate = date.toJSON();
        authHeader.append('Token', SessionInfo.Token);
       
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsExchangeRateForDate/?' + 'date=' + stringDate, { headers: authHeader }).map(response => {


                var allLists = response.json();
                var _mappedListsArray: Array<CustomsExchangeRatePM> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: CustomsExchangeRatePM;
                        entity = this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsExchangeRatePM;
        entityPM = new CustomsExchangeRatePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }

}