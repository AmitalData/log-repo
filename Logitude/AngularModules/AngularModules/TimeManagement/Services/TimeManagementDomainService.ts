import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {TMOfficeHourPM} from '../EntityPMs/TMOfficeHourPM';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';
import {TMEmployeeTimePM} from '../EntityPMs/TMEmployeeTimePM'; 

@Injectable()

export class TimeManagementDomainService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TimeManagementDomain';
    }

    GetWeeklyTimeSheetList(employeeUserId: string, locationCode: string, periodStartDate: Date) {

        var url = this._apiUrl + '/GetWeeklyTimeSheetList?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper.GetDateString(periodStartDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;

                var args = new TimeManagementAPIHelper();
                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetPeriodTimeSheetList(employeeUserId: string, locationCode: string, startDate: Date, endDate: Date) {

        var url = this._apiUrl + '/GetDataEntryTimeSheetList?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&startDate=" + ServiceHelper.GetDateString(startDate) + "&endDate=" + ServiceHelper.GetDateString(endDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                //var args = new TimeManagementAPIHelper();
                //var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetTMProjects(employeeUserId: string, locationCode: string, periodStartDate: Date) {

        var url = this._apiUrl + '/GetTMProjects?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper.GetDateString(periodStartDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;

                var args = new TimeManagementAPIHelper();
                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTMProjectsByBatchTask(employeeUserId: string, fromDate: Date, toDate: Date) {

        var url = this._apiUrl + '/GetTMProjectsByBatchTask?employeeUserId=' + employeeUserId + "&fromDate=" + ServiceHelper.GetDateString(fromDate) + "&toDate=" + ServiceHelper.GetDateString(toDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTMProjectsByBatchProject(employeeUserId: string , fromProject: string, toProject: string, fromDate: Date, toDate: Date) {

        var url = this._apiUrl + '/GetTMProjectsByBatchProject?employeeUserId=' + employeeUserId + "&fromProject=" + fromProject + "&toProject=" + toProject +"&fromDate=" + ServiceHelper.GetDateString(fromDate) +"&toDate=" + ServiceHelper.GetDateString(toDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetNewTMProjectConnect(MainId: string, ConnectedId: string) {
        var url = this._apiUrl + '/GetNewTMProjectConnect?MainId=' + MainId + "&id=" + ConnectedId;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;               
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
        return defer(() => 
        {
            var mappedEntity: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(helper, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, helper);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetProjectsCounts(loggedUserId: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetProjectsCounts?loggedUserId=' + loggedUserId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                return allLists;
            }));
        });
    }

    DeleteTimeSheetItem(Id: string, employeeUserId: string, locationCode: string, periodStartDate: Date, exitDate:Date) {

        var url = this._apiUrl + '/GetUpdatedTimeSheetList?Id=' + Id + "&employeeUserId=" + employeeUserId + " &locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper.GetDateString(periodStartDate) + "&exitDate=" + ServiceHelper.GetDateString(exitDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, new TimeManagementAPIHelper());
                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCalculationCompleteWork() {

        var url = this._apiUrl + '/GetCalculationCompleteWork?';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetProrate?EmployeeUserId=' + EmployeeUserId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var iResponse = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = iResponse;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetVacationsSummary(Year: number) {

        var url = this._apiUrl + '/GetVacationsSummary?Year=' + Year;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetVacationsDetails(Year: number, Type:string) {

        var url = this._apiUrl + '/GetVacationsDetails?Year=' + Year + '&Type=' + Type;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DownloadEmployeesTimesToExcel(employeeUserId: string, locationCode: string, startDate: Date, endDate: Date) {

        var url = this._apiUrl + '/GetDownloadEmployeesTimesToExcel?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&startDate=" + ServiceHelper.GetDateString(startDate) + "&endDate=" + ServiceHelper.GetDateString(endDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
