import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AnalyticsFactsFieldsMetaDataPM } from '../../../DashboardModule/EntityPMs/AnalyticsFactsFieldsMetaDataPM';

@Injectable()

export class AnalyticsFactsFieldsMetaDataPMExtendedService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AnalyticsFactsFieldsMetaDataPMExtended';
    }

    GetAllByAnalyticsFactsMetaDataId(analyticsFactsMetaDataId: string) {
        var url = this._apiUrl + '/GetAllByAnalyticsFactsMetaDataId/analyticsFactsMetaDataId=' + analyticsFactsMetaDataId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var listMapped: Array<AnalyticsFactsFieldsMetaDataPM> = [];
                for (var itemJeson in listJason) {
                    var itemMapped: AnalyticsFactsFieldsMetaDataPM = this.MapAnalyticsFactsFieldsMetaDataPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetPresetFilters() {
        var url = this._apiUrl + '/GetPresetFilters';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var listMapped: Array<AnalyticsFactsFieldsMetaDataPM> = [];
                for (var itemJeson in listJason) {
                    var itemMapped: AnalyticsFactsFieldsMetaDataPM = this.MapAnalyticsFactsFieldsMetaDataPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    MapAnalyticsFactsFieldsMetaDataPM(jsonList: any) {
        var entityList: AnalyticsFactsFieldsMetaDataPM;
        entityList = new AnalyticsFactsFieldsMetaDataPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
