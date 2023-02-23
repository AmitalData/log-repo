import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';

@Injectable()

export class DashboardPMExtendedService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DashboardPMExtended';
    }

    GetDefaultDashboardId() {
        var url = this._apiUrl + '/GetDefaultDashboardId';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetUsersDashboardsCount() {
        var url = this._apiUrl + '/GetUsersDashboardsCount';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
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

    Delete(dashboardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.delete(this._apiUrl + '?dashboardId=' + dashboardId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetDashboardsFromIds(dashboardsIds: string) {
        var url = this._apiUrl + '/GetDashboardsFromIds?dashboardsIds=' + dashboardsIds;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDashboardsUserSettings(userId: string) {
        var url = this._apiUrl + '/GetDashboardsUserSettings?userId=' + userId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetPredefinedDashboardsFromTenantZero() {
        var url = this._apiUrl + '/GetPredefinedDashboardsFromTenantZero';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    PinDashboard(pinnedDashboardTab: PinnedDashboard) {
        var url = this._apiUrl + '/PostPinDashboard';

        return defer(() => {
            return this._http.post(url, JSON.stringify(pinnedDashboardTab), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myResponse = new ServiceResponse();
                myResponse.Result = res;
                return myResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    PinPredefinedDashboards(dashboardIds: string[]) {
        var url = this._apiUrl + '/PostPinPredefinedDashboards';

        return defer(() => {
            return this._http.post(url, JSON.stringify(dashboardIds), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myResponse = new ServiceResponse();
                myResponse.Result = res;
                return myResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    UnpinDashboard(dashboardsUserSettingId: string, dashboardId: string) {
        var url = this._apiUrl + '/GetUnPinDashboard?dashboardsUserSettingsId=' + dashboardsUserSettingId + '&dashboardId=' + dashboardId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class PinnedDashboard {
    public Id: string;
    public Order: number;
}
