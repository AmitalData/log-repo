import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CashBookList} from '../../EntityLists/CashBookList';
import {CashBookStatusChart} from '../../DataContracts/CashBookStatusChart';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CashBookExtendedListService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CashBookViews';
    }

    GetCashBookStatusChartData() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetCashBookStatusChartData';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {


                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray: Array<CashBookStatusChart> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CashBookStatusChart;
                        entity = this.MapJsonToCashBookStatusChart(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: CashBookList;
        entityList = new CashBookList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToCashBookStatusChart(jsonList: any) {

        var entityList: CashBookStatusChart;
        entityList = new CashBookStatusChart();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}