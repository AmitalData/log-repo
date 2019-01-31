import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {List} from '../DataContracts/Dashboard/List';
import {DashBoardClass} from '../DataContracts/Dashboard/DashBoardClass';

@Injectable()

export class ChartsService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        //this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
    }

    GetMoneyStatusForTenant(ActivityType: string, months: number, days: number, tenant: number, index: number, currency: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetMoneyStatusForTenant?type=' + ActivityType + '&months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }
    GetMoneyOutStatusForTenant(months: number, days: number, tenant: number, index: number, currency: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetMoneyOutStatusForTenant?months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }
    GetActivityStatusByType(ActivityType: string, fromDate: Date, toDate: Date, currentTenant: string, customerid: string = null, directionId: string = null, transportmodeId:string=null) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetActivityStatusByType?type=' + ActivityType + '&FromDate=' + ServiceHelper.GetDateString(fromDate) + '&ToDate=' + ServiceHelper.GetDateString(toDate) + '&currentTenant=' + currentTenant + '&customerid=' + customerid + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
                headers: authHeader
            }).map(response => {

                var allLists: DashBoardClass[] = response.json();
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });

    }
    GetActivityStatus(ActivityType: string, lastMonths: number, lastDays: number, currentTenant: number, customerid: string = null) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetActivityStatus?type=' + ActivityType + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
                headers: authHeader
            }).map(response => {

                var allLists: DashBoardClass[] = response.json();
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });

    }
    GetShipmentByDirectionAndTransmode(type: string, lastMonths: number, lastDays: number, currentTenant: number, customerid: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentByDirectionAndTransmode?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
                headers: authHeader
            }).map(response => {

                var allLists: DashBoardClass[] = response.json();
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });

    }
    GetShipmentsByTop10CountriesDashBoard(type: string, lastMonths: number, lastDays: number, measurment: number, currentTenant: number, top: number, includeOthers: boolean, customerid: string, directionId: string, transmodeId: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentsByTop10CountriesDashBoard?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId, {
                headers: authHeader
            }).map(response => {

                var allLists: DashBoardClass[] = response.json();
                var myList: List<DashBoardClass> = new List<DashBoardClass>();
                for (var key in allLists) {
                    var entity: DashBoardClass;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
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
