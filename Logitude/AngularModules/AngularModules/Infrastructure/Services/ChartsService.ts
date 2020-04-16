import { DashBoardClass } from '../DataContracts/Dashboard/DashBoardClass';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { List } from '../DataContracts/Dashboard/List';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';


@Injectable()
export class ChartsService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api';
    }

    GetMoneyStatusForTenant(ActivityType: string, months: number, days: number, tenant: number, index: number, currency: number) {
        var url = this._apiUrl + '/InvoiceDomain/GetMoneyStatusForTenant?type=' + ActivityType + '&months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetMoneyOutStatusForTenant(months: number, days: number, tenant: number, index: number, currency: number) {
        var url = this._apiUrl + '/InvoiceDomain/GetMoneyOutStatusForTenant?months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetActivityStatusByType(ActivityType: string, fromDate: Date, toDate: Date, currentTenant: string, customerid: string = null, directionId: string = null, transportmodeId: string = null) {
        var url = this._apiUrl + '/ShipmentDomain/GetActivityStatusByType?type=' + ActivityType + '&FromDate=' + ServiceHelper.GetDateString(fromDate) + '&ToDate=' + ServiceHelper.GetDateString(toDate) + '&currentTenant=' + currentTenant + '&customerid=' + customerid + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetActivityStatus(ActivityType: string, lastMonths: number, lastDays: number, currentTenant: number, customerid: string = null) {
        var url = this._apiUrl + '/ShipmentDomain/GetActivityStatus?type=' + ActivityType + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentByDirectionAndTransmode(type: string, lastMonths: number, lastDays: number, currentTenant: number, customerid: string) {
        var url = this._apiUrl + '/ShipmentDomain/GetShipmentByDirectionAndTransmode?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentsByTop10CountriesDashBoard(type: string, lastMonths: number, lastDays: number, measurment: number, currentTenant: number, top: number, includeOthers: boolean, customerid: string, directionId: string, transmodeId: string) {
        var url = this._apiUrl + '/ShipmentDomain/GetShipmentsByTop10CountriesDashBoard?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapJsonToEntityList(jsonList: any) {

        var entityList: DashBoardClass;
        entityList = new DashBoardClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
