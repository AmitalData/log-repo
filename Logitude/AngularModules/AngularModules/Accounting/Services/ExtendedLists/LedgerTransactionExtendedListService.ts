import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';

@Injectable()

export class LedgerTransactionExtendedListService {
    private _http: Http
    private _apiUrl: string;
    private _reconciliationUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LedgerTransactions';
        this._reconciliationUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconciliationOp';
    }

    GetFirstLedgerTransaction(AccountId:string) {

        var urlparameters = '/GetFirstLedgerTransaction?AccountId=' + AccountId;

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();

                //console.log("serviceResponse: ", serviceResponse);

                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/GetLedgerTransactionsByFilters?';
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
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();

                //console.log("serviceResponse: ", serviceResponse);

                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getBalanceByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/GetTransactionsBalanceByFilters?';
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
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();

                //console.log("serviceResponse: ", serviceResponse);

                //serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getOpenReconciliationsByFilter(accountId: string, filters: ApiQueryFilters) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._reconciliationUrl + "/GetOpenReconciliationsByFilters";

        var urlparameters = '?gLAccountId='
            + accountId + '&tenant=' + SessionInfo.LoggedUserTenant; // Get Open Transaction by Itzik service , the get is inside post method

        // Parse Filters into URI
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var callTime = new Date();
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
        // End Parse


        var callUrl = url.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                //serviceResponse.CallTime = callTime;
                serviceResponse = response.json();
                console.log("serviceResponse: ", serviceResponse);

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getAutomaticReconcileByFilter(method1: string, method2: string, method3: string, accountId: string, filters: ApiQueryFilters) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._reconciliationUrl + "/GetAutomaticReconcileByFilter";

        var urlparameters = '?gLAccountId=' + accountId
            + '&tenant=' + SessionInfo.LoggedUserTenant
            + '&method1=' + method1
            + '&method2=' + method2
            + '&method3=' + method3; // Get Open Transaction by Itzik service , the get is inside post method



        //#region Parse Filters into URI
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
        //#endregion End Parse


        var callUrl = url.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();
                var _mappedListsArray: Array<LedgerTransactionList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: LedgerTransactionList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getLedgerTransactionsByIds(Ids: string[]) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var params: string = "";
        for (var id of Ids) {
            params += "Ids[]=" + id + "&";
        }

        var url = this._apiUrl + '/getLedgerTransactionsByIds?' + params;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getLast10TransactionsForAccount(accountId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        var url = this._apiUrl + '/GetLast10TransactionsForAccount?AccountId=' + accountId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getTransactionsForARPayment(arpaymentId:string, billToGLAccountId:string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        var url = this._apiUrl + '/GetTransactionsForARPayment?arpaymentId=' + arpaymentId
        + '&billToGLAccountId=' + billToGLAccountId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }



    MapJsonToEntityList(jsonList: any) {

        var entityList: LedgerTransactionList;
        entityList = new LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }


}
