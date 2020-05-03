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

@Injectable()
export class BIReportExtendedListService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<BusinessRoleList> = [];
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/bireportsextended';
    }

    GetReportsByTenantNumber(filters: ApiQueryFilters, copyFromTenant: number) {
        return new Promise((resolve, reject) => {
            resolve(this.getReports(filters, copyFromTenant));
        });
    }

    getReports(filters: ApiQueryFilters, copyFromTenant: number) {
        var callTime = new Date();
        return defer(() => {
            var urlparameters = '?';
            var mykeys = Object.keys(filters);
            var addtionalFiltersValues = null;
            for (var i in mykeys) {
                var propName = mykeys[i];
                var propValue = filters[propName];
                var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

                if (urlparameters != "?") {
                    urlparameters = urlparameters.concat('&');
                }
                if (!ignoreFilter) {
                    propValue = encodeURIComponent(propValue);
                    urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
                }

                if (propName == "AdditionalFilters" && propValue.length > 0)
                    addtionalFiltersValues = JSON.stringify(propValue);
            }
            if (addtionalFiltersValues) {
                urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
            }

            var url: string = this._apiUrl + '/GetTenantReports' + urlparameters + '&copyFromTenant=' + copyFromTenant;
            return this._httpClient.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        var serviceResponse: ServiceResponse;
                        serviceResponse = response.body;
                        var mappedListsArray: Array<BIReportList> = [];
                        if (serviceResponse.Result) {
                            for (var key in serviceResponse.Result) {
                                var entity: BIReportList;
                                entity = this.MapJsonToEntityList(serviceResponse.Result[key]);

                                mappedListsArray.push(entity);
                            }
                        }
                        serviceResponse.Result = mappedListsArray;
                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BIReport", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                        return serviceResponse;
                    }
                }),
                catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: BIReportList;
        entityList = new BIReportList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
