import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { BusinessRoleList } from '../../EntityLists/BusinessRoleList';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { BIReportList } from '../../EntityLists/BIReportList';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { BIReportFolderList } from '../../EntityLists/BIReportFolderList';

@Injectable()
export class BIReportFolderExtendedListService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<BusinessRoleList> = [];
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/bireportfolderextended';
    }

    GetPermittedFolders(loggedUserId) {

        var callTime = new Date();

        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetPermittedFolders?loggedUserId=' + loggedUserId, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {

                        var allLists = response.body;
                        var _mappedListsArray: Array<BIReportFolderList> = [];
                        if (allLists) {
                            for (var key in allLists) {
                                var entity: BIReportFolderList = this.MapJsonToEntityList(allLists[key]);
                                _mappedListsArray.push(entity);
                            }
                        }

                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = _mappedListsArray;
                        serviceResponse.CallTime = callTime;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BIReportFolder", "GetAllLists", "");

                        return serviceResponse;
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: BIReportFolderList;
        entityList = new BIReportFolderList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
}
