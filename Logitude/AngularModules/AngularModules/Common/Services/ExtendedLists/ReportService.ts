import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, takeWhile } from 'rxjs/operators';
import { BehaviorSubject, defer, interval, of, Subject, Subscription } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import {ReportFliter} from '../../../Report/Components/Filters/ReportFliter';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ReportExecutionLogPM } from 'Common/EntityPMs/ReportExecutionLogPM';
import { AppTool } from 'Infrastructure/Tools';

@Injectable()
export class ReportService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Report';
    }

    GetReportListsByGroupId(groupId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '?groupId=' + groupId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    
    GetPrepareSendReport(type: string, fileName: string,  tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetPrepareSendReport" + '?type=' + type + '&fileName=' + fileName  +  '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    GetDataProviderProperties(code: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetDataProviderProperties" + '?code=' + code ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    GetReportByTenantAndUserToMenu(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetReportByTenantAndUserToMenu"+ '?id=' + userId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    CheckReportsStatus(reportKeys:string) {
        if(reportKeys?.length == 0) return of(null);
   
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetCheckReportsStatus" + '?ids=' + reportKeys,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    DeleteFromMenu(reportId: string) {
        
        return this._http.post(this._apiUrl + '/PostDeleteFromMenu?reportId=' + reportId, null, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                let serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }
    GetCheckIfStimulSoftReportIsBliud(reportKey: string,  tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetCheckIfStimulSoftReportIsBliud" + '?reportKey=' + reportKey + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetCheckIfReportsRunUsingWR() {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetCheckIfReportsRunUsingWR",ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    
    GenerateReportMethod(filter: ReportFliter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(filter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }
    relatedReportSubject = new BehaviorSubject<ReportExecutionLogPM[]>([]);
    relatedReport: ReportExecutionLogPM[];
    private reportsCount = new Subject<number>();
    reportsCount$ = this.reportsCount.asObservable();
    private subscription: Subscription | null = null;

    public LoadReports() {
        this.subscription?.unsubscribe();
        this.GetReportByTenantAndUserToMenu(SessionLocator.LoggedUserPM?.Id).subscribe((response: ServiceResponse) => {
             

            if (!AppTool.IsNullOrEmpty(response)) {

                this.relatedReport = [];
                var relatedDocs: ReportExecutionLogPM[];
                relatedDocs = response.Result;
                this.relatedReport = relatedDocs?.sort((a, b) => new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime());
                this.relatedReportSubject.next(relatedDocs);
                this.reportsCount.next(this.relatedReport.filter(report => report.StatusCode === 'D').length);

                this.StartCheckingStatus();

            }
        });
    }
    StartCheckingStatus() {
        this.subscription = interval(5000)
            .pipe(takeWhile(() => this.relatedReport?.length > 0))
            .subscribe(() => this.CheckStatus());
    }
    
    public CheckStatus() {

        const reportIds = this.relatedReport?.filter(report => report.StatusCode === 'P' || report.StatusCode === 'W')?.map(report => report.Id).join(',');
        this.CheckReportsStatus(reportIds).subscribe(statusResponse => {
            if (statusResponse && !statusResponse.HasError) {
                statusResponse?.Result?.forEach((status: any) => {
                    const reportIndex = this.relatedReport.findIndex(r => r.Id === status.Id);
                    if (reportIndex !== -1) {
                        this.relatedReport[reportIndex] = status;
                    }
                });
                this.reportsCount.next(this.relatedReport.filter(report => report.StatusCode === 'D').length);

                this.relatedReportSubject.next(this.relatedReport);
            }
        });

    }
    GenerateReportForCustomerPotentialActual(filter: ReportFliter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(filter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;
                var myResult = new CustomersDataProvider();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetExcel(reportKey: string, reportName: string, tenant: string, reportCode: string): Promise<any> {
        return this._http.get(
            this._apiUrl + "/GetExcel",
            { 
                params: { reportKey, reportName, tenant, reportCode }, 
                headers: ServiceHelper.GetHttpHeaders().headers, 
                responseType: 'blob' 
            }
        ).toPromise() as Promise<any>;
    }
}

export class CustomersDataProvider {
    public Customers: CustomersData[] = [];
}

export class CustomersData {
    public CustomerId: string;
    public CustomerName: string;
    public PrimaryContactName: string;
    public PrimaryContactEmail: string;
    public Salesman: string;
    public LocationsCount: number;
    public AD_POT: number;
    public AR_POT: number;
    public AE_POT: number;
    public AI_POT: number;
    public ID_POT: number;
    public IR_POT: number;
    public IE_POT: number;
    public II_POT: number;
    public OD_POT: number;
    public OR_POT: number;
    public OE_POT: number;
    public OI_POT: number;
    public CI_POT: number;
    public DL_POT: number;
    public IN_POT: number;
    public AD_ACT: number;
    public AR_ACT: number;
    public AE_ACT: number;
    public AI_ACT: number;
    public ID_ACT: number;
    public IR_ACT: number;
    public IE_ACT: number;
    public II_ACT: number;
    public OD_ACT: number;
    public OR_ACT: number;
    public OE_ACT: number;
    public OI_ACT: number;
    public CI_ACT: number;
    public DL_ACT: number;
    public IN_ACT: number;
}