import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../Utilities/CachedDataManager';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {LocalStorageManager} from '../../Utilities/LocalStorageManager';
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';
import {BusinessRoleList} from '../../EntityLists/BusinessRoleList';
import { BIReportList } from '../../EntityLists/BIReportList';

@Injectable()

export class BIReportExtendedListService {
    private _http: Http;
    private _apiUrl: string;
    public static CachedData: Array<BusinessRoleList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/bireportsextended';
    }

    GetReportsByTenantNumber(filters: ApiQueryFilters, copyFromTenant: number) {
                
        return new Promise((resolve, reject) => {
            resolve(this.getReports(filters, copyFromTenant));
        });
    }

    getReports(filters: ApiQueryFilters, copyFromTenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
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
            return this._http.get(this._apiUrl + '/GetTenantReports' + urlparameters + '&copyFromTenant=' + copyFromTenant, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();
                var _mappedListsArray: Array<BIReportList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: BIReportList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }   

                serviceResponse.Result = _mappedListsArray;
                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BIReport", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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
