import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {TMOfficeHourPM} from '../EntityPMs/TMOfficeHourPM';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';
import {TMEmployeeTimePM} from '../EntityPMs/TMEmployeeTimePM'; 

@Injectable()

export class TimeManagementDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TimeManagementDomain';
    }

    GetWeeklyTimeSheetList(employeeUserId: string, locationCode: string, periodStartDate: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetWeeklyTimeSheetList?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper.GetDateString(periodStartDate);
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var args = new TimeManagementAPIHelper();
                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetPeriodTimeSheetList(employeeUserId: string, locationCode: string, startDate: Date, endDate: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDataEntryTimeSheetList?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&startDate=" + ServiceHelper.GetDateString(startDate) + "&endDate=" + ServiceHelper.GetDateString(endDate);
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                //var args = new TimeManagementAPIHelper();
                //var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetTMProjects(employeeUserId: string, locationCode: string, periodStartDate: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTMProjects?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper.GetDateString(periodStartDate);
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var args = new TimeManagementAPIHelper();
                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetNewTMProjectConnect(MainId: string, ConnectedId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNewTMProjectConnect?MainId=' + MainId + "&id=" + ConnectedId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();               
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }
    UpdateTimeSheetList(helper: TimeManagementAPIHelper) {
        return Observable.defer(() => 
           { var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(helper, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, helper);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetProjectsCounts(loggedUserId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetProjectsCounts?loggedUserId=' + loggedUserId , {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                return allLists;
            });
        });
    }
    DeleteTimeSheetItem(Id: string, employeeUserId: string, locationCode: string, periodStartDate: Date, exitDate:Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdatedTimeSheetList?Id=' + Id + "&employeeUserId=" + employeeUserId + " &locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper.GetDateString(periodStartDate) + "&exitDate=" + ServiceHelper.GetDateString(exitDate);
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, new TimeManagementAPIHelper());
                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    private MapJsonToTimeManagementAPIHelper(jsonPM: any, getCallMap: boolean = true, entityPM: TimeManagementAPIHelper = null) {
        if (!entityPM) {
            entityPM = new TimeManagementAPIHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }


    Prorate(EmployeeUserId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetProrate?EmployeeUserId=' + EmployeeUserId, { headers: authHeader }).map(response => {
                var iResponse = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = iResponse;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

}
export class TimeManagementAPIHelper {
    public Id: number;
    public LocationCode: string;
    public EmployeeUserId: string;
    public TotalFromClock: string;
    public StartDate: Date;
    public EndDate: Date;
    public Items: TimeSheetItem[] = [];
    public ItemsPM: TMEmployeeTimePM[] = [];
    public OfficeClockDays: TimeSheetItemDay[] = [];
}
export class TimeSheetItem {
    public ProjectId: string;
    public ProjectName: string;
    public Description: string;
    public WINumber: string;
    public ProjectId_db: string;
    public Description_db: string;
    public WINumber_db: string;
    public LocationCode: string;
    public EmployeeUserId: string;
    public TotalMinutes: number;
    public IsHeaderUpdated: boolean;
    public Days: TimeSheetItemDay[] = [];
}
export class TimeSheetItemDay {
    public Index: number;
    public Minuts: number;
    public Minuts_db: number;
    public Date: Date;
    public TotalFromClock: number;
    public TotalFromClockString: string;
    //public ProjectId: string;
}
