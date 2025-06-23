import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map, takeWhile } from 'rxjs/operators';
;
import { BehaviorSubject, defer, interval, of, Subject, Subscription } from 'rxjs';
import { TenantManagementList } from '../../Infrastructure/EntityLists/TenantManagementList';
import { BatchServicesDefinitionPM } from '../../Infrastructure/EntityPMs/BatchServicesDefinitionPM';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { TenantManagementPM } from '../../Infrastructure/EntityPMs/TenantManagementPM';
import { TenantManagementJS } from '../../Infrastructure/DataContracts/TenantManagementJS';
import { ObjectsUpdater } from '../../Infrastructure/Locators/ObjectsUpdater';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AppTool } from 'Infrastructure/Tools';
import { MenuItemClass } from 'Report/Components/ProcessMenuComponent';

@Injectable()

export class ProcessMenuService {
    private _apiUrl: string;
    private _http: HttpClient;
    relatedProcessSubject = new BehaviorSubject<MenuItemClass[]>([]);
    relatedProcess: MenuItemClass[];
    private processCount = new Subject<number>();
    processCount$ = this.processCount.asObservable();
    private subscription: Subscription | null = null;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ProcessMenu';
    }

    GetProcessesByTenantAndUserToMenu(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetProcessesByTenantAndUserLastWeek" + '?id=' + userId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    CheckProcessesStatus(reportKeys: string) {
        if (reportKeys?.length == 0) return of(null);

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetCheckReportsStatus" + '?ids=' + reportKeys, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
    DeleteFromMenu(reportId: string, type: number) {

        return this._http.post(this._apiUrl + '/PostDeleteFromMenu?reportId=' + reportId + '&type=' + type, null, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                let serviceResponse :any= response;
                if(!serviceResponse.HasError){
                 this.relatedProcess = this.relatedProcess
                    ?.filter(item => item.Id !== reportId)
                     ?.sort((a, b) => new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime());
                this.relatedProcessSubject.next(this.relatedProcess);
                this.processCount.next(this.relatedProcess.filter(item => item.StatusCode === 'D').length);

                return serviceResponse;
                }
                
            }),
            catchError(ServiceHelper.HandleServiceError));
    }
   

    public LoadMenuItems() {
        this.subscription?.unsubscribe();
        this.GetProcessesByTenantAndUserToMenu(SessionLocator.LoggedUserPM?.Id).subscribe((response: ServiceResponse) => {
             

            if (!AppTool.IsNullOrEmpty(response)) {

                this.relatedProcess  = [];
                var relatedDocs: MenuItemClass[];
                relatedDocs = response.Result;
                this.relatedProcess  = relatedDocs?.sort((a, b) => new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime());
                this.relatedProcessSubject.next(relatedDocs);
                this.processCount.next(this.relatedProcess.filter(item => item.StatusCode === 'D').length);

                this.StartCheckingStatus();

            }
        });
    }
    StartCheckingStatus() {
        this.subscription = interval(5000)
            .pipe(takeWhile(() => this.relatedProcess?.length > 0))
            .subscribe(() => this.CheckStatus());
    }
    
    public CheckStatus() {

        const itemIds = this.relatedProcess?.filter(item => item.StatusCode === 'P' || item.StatusCode === 'W' || item.StatusCode === 'C'|| item.StatusCode === 'I')?.map(item => item.Id).join(',');
        this.CheckProcessesStatus(itemIds).subscribe(statusResponse => {
            if (statusResponse && !statusResponse.HasError) {
                statusResponse?.Result?.forEach((status: any) => {
                    const itemIndex = this.relatedProcess.findIndex(r => r.Id === status.Id);
                    if (itemIndex !== -1) {
                        this.relatedProcess[itemIndex] = status;
                    }
                });
                this.processCount.next(this.relatedProcess.filter(item => item.StatusCode === 'D').length);

                this.relatedProcessSubject.next(this.relatedProcess);
            }
        });

    }

}


