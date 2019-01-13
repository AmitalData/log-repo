import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {List} from '../../Infrastructure/DataContracts/Dashboard/List';
import {DashBoardClass} from '../../Infrastructure/DataContracts/Dashboard/DashBoardClass';
import {DailySpotlightClass} from '../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass';
import {ChartingDataClass} from '../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';

@Injectable()

export class DashboardDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
    }

    GetActivityStatus(ActivityType:string,lastMonths: number, lastDays: number, currentTenant: number, customerid: string = null) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetActivityStatus?type=' + ActivityType+'&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid='+customerid, {
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


    GetActivityStatusByType(ActivityType: string, fromDate: Date, toDate: Date, currentTenant: string, directionId: string, transportmodeId: string, customerid: string = null) {
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

    GetActivityStatusByMessagesLogs(lastDays: number,showType: string = null) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommonDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetActivityStatusByMessagesLogs?lastDays=' + lastDays + '&showType=' + showType, {
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

    GetDashboardSpotlightCounts(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommonDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDashboardSpotlightCounts?tenant=' + currentTenant , {
                headers: authHeader
            }).map(response => {

                var Object: DailySpotlightClass = response.json();

                var mappedEntity: DailySpotlightClass = new DailySpotlightClass();
                mappedEntity = this.MapJsonToEntityListDailySpotlightClass(Object);
                
                return mappedEntity;
            });
        });


    }



    GetAirlineDashboardSpotlightCounts(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommonDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAirlineDashboardSpotlightCounts?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {

                var Object: DailySpotlightClass = response.json();

                var mappedEntity: DailySpotlightClass = new DailySpotlightClass();
                mappedEntity = this.MapJsonToEntityListDailySpotlightClass(Object);

                return mappedEntity;
            });
        });


    }
    GetShipmentByDirectionAndTransmode(type:string,lastMonths: number, lastDays: number, currentTenant: number, customerid: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentByDirectionAndTransmode?type=' + type+'&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
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
    GetShipmentByDirectionAndTransmodeCustom(ActivityType: string, fromDate: Date, toDate: Date, customerid: string = null) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentByDirectionAndTransmodeCustom?type=' + ActivityType + '&FromDate=' + ServiceHelper.GetDateString(fromDate) + '&ToDate=' + ServiceHelper.GetDateString(toDate) + '&customerid=' + customerid, {
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


    GetShipmentsByTop10CountriesDashBoard(type:string,lastMonths: number, lastDays: number, measurment: number, currentTenant: number, top: number, includeOthers: boolean, customerid: string, directionId: string, transmodeId: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentsByTop10CountriesDashBoard?type=' + type +'&lastMonths=' + lastMonths + '&lastDays=' + lastDays+ '&measurment=' + measurment  + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId, {
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

    GetShipmentsByTop10CountriesDashBoardCustom(type: string, FromDate: Date, ToDate: Date, measurment: number, currentTenant: number, top: number, includeOthers: boolean, customerid: string, directionId: string, transmodeId: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentsByTop10CountriesDashBoardCustom?type=' + type + '&FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate)   + '&measurment=' + measurment + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId, {
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

    GetTop10DashBoard(type: string, lastMonths: number, lastDays: number, measurment: number, currentTenant: number, top: number, includeOthers: boolean, directionId: string, transportmodeId: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTop10DashBoard?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
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
    GetTop10DashBoardCustom(type: string, FromDate: Date, ToDate: Date, measurment: number, currentTenant: number, top: number, includeOthers: boolean, directionId: string, transportmodeId: string) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTop10DashBoardCustom?type=' + type + '&FromDate=' + ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper.GetDateString(ToDate) +  '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
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

    GetMoneyStatusForTenant(ActivityType:string,months: number, days: number, tenant: number, index: number, currency: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetMoneyStatusForTenant?type=' + ActivityType+'&months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }


    GetMoneyStatusForTenantCustom(ActivityType: string, fromDate: Date, toDate: Date) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetMoneyStatusForTenantCustom?type=' + ActivityType + '&ToDate=' + ServiceHelper.GetDateString(toDate) + '&FromDate=' + ServiceHelper.GetDateString(fromDate), {
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

    GetDebrotExposure(tenant: number, currency: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDebrotExposure?tenant=' + tenant + '&currency=' + currency, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });


    }


    GetDashBoardBookings(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommonDomain'
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDashBoardBookings?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                return allLists;
            });
        });
    }

    GetTopParticipantsDashBoard(lastDays: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommonDomain'
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTopParticipantsDashBoard?lastDays=' + lastDays, {
                headers: authHeader
            }).map(response => {
                var allLists: ChartingDataClass[] = response.json();
                var myList: ChartingDataClass[]=[];
                for (var key in allLists) {
                    var entity: ChartingDataClass;
                    entity = this.MapJsonToChartingDataEntityList(allLists[key]);
                    myList.push(entity);
                }
                return myList;
            });
        });

    }

    MapJsonToEntityListDailySpotlightClass(jsonList: any) {

        var entityList: DailySpotlightClass;
        entityList = new DailySpotlightClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: DashBoardClass;
        entityList = new DashBoardClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToChartingDataEntityList(jsonList: any) {

        var entityList: ChartingDataClass;
        entityList = new ChartingDataClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}
