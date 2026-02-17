import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

@Injectable()

export class UserExtendedListService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/userextended';
    }

    GetCustomDataByFilters(filters: ApiQueryFilters) {
        var callTime = new Date();

        var urlparameters = '/GetUserExtendedByFilters?';
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

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();

                var _mappedListsArray: Array<UserExtendedList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: UserExtendedList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "User", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: UserExtendedList;
        entityList = new UserExtendedList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    GetUserLicensesCountForUser(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUserLicensesCountForUser?userId=' + userId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}

export class UserExtendedList {
    constructor() {
        
    }

    public Id: string;
    public Tenant: number;  
    public EnglishName: string; 
    public Email: string;    
    public SearchFields: string;
    
    public PackageCode1: string;
    public PackageCode2: string;
    public PackageCode3: string;
    public PackageCode4: string;
    public PackageCode5: string;
    public PackageCode6: string;
    public PackageCode7: string;
    public PackageCode8: string;
    public PackageCode9: string;
    public PackageCode10: string;

    public IsChecked1: boolean;
    public IsChecked2: boolean;
    public IsChecked3: boolean;
    public IsChecked4: boolean;
    public IsChecked5: boolean;
    public IsChecked6: boolean;
    public IsChecked7: boolean;
    public IsChecked8: boolean;
    public IsChecked9: boolean;
    public IsChecked10: boolean;
}