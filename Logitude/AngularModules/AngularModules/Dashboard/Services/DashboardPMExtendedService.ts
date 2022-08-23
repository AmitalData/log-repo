import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { DashboardPM } from '../../Infrastructure/EntityPMs/DashboardPM';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class DashboardPMExtendedService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DashboardPMExtended';
    }

    GetDashboardPMs() {
        var url = this._apiUrl + '/GetDashboardPMs';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var listMapped: Array<DashboardPM> = [];
                for (var itemJeson in listJason) {
                    var itemMapped: DashboardPM = this.MapDashboardPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapDashboardPM(jsonList: any) {
        var entityList: DashboardPM;
        entityList = new DashboardPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
