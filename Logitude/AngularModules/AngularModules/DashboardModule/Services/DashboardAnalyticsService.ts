import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer} from 'rxjs';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { WidgetPartArguments } from 'DashboardModule/DataContracts/WidgetPartArguments';
import { WidgetPM } from 'DashboardModule/EntityPMs/WidgetPM';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';

@Injectable()

export class DashboardAnalyticsService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DashboardAnalytics';
    }

    GetDataAnalyticPart(widgetPartArguments : WidgetPartArguments) {
        var url = this._apiUrl + '/PostGetDataAnalyticPart';

        return defer(() => {
            return this._http.post(url, JSON.stringify(widgetPartArguments),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    public GetData(myWidget:ReactWidgetPM) {
        var url = this._apiUrl + '/PostGetDataAnalytic';

        return defer(() => {
            return this._http.post(url, JSON.stringify(myWidget),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
