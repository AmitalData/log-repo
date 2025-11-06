import { HttpHeaders ,HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';
import { catchError, map } from 'rxjs/operators';


@Injectable()
export class LedgerTransactionExtendedListService {

    private httpClient: HttpClient;
    private _apiUrl: string;
    private _reconciliationUrl: string;

    constructor() {

        this.httpClient=ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LedgerTransactions';
        this._reconciliationUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconciliationOp';
    }

    GetFirstLedgerTransaction(AccountId:string) {

        var urlparameters = '/GetFirstLedgerTransaction?AccountId=' + AccountId;


        var callUrl = this._apiUrl.concat(urlparameters);
        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response : ServiceResponse)=> {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetARPyamentChequesListAsLedgerTransactions(filters: ApiQueryFilters) {
        var urlparameters = '/GetARPyamentChequesListAsLedgerTransactions?';
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

        var callUrl = this._apiUrl.concat(urlparameters);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                console.log("cheque list", response)

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    getByFilters(filters: ApiQueryFilters) {

        if (filters.SortBy === "LocalAmountCredit") {
            filters.SortBy = "CalculatedLocalAmount";
        }
        else if (filters.SortBy === "ForeignAmountCredit") {
            filters.SortBy = "CalculatedForeignAmount";
        }

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

        var callUrl = this._apiUrl.concat(urlparameters);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


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

        var callUrl = this._apiUrl.concat(urlparameters);
         return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
             map((response: ServiceResponse) => {
                 var serviceResponse: ServiceResponse = new ServiceResponse();
                 serviceResponse = response;
                 return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    getOpenReconciliationsByFilter(accountId: string, filters: ApiQueryFilters) {


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
        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }


    // External Reconciliations
    getReconciliationsByFilter(accountId: string, filters: ApiQueryFilters) {

        var url = this._reconciliationUrl + "/GetReconciliationsByFilter";

        var urlparameters = '?gLAccountId=' + accountId
                            + '&tenant=' + SessionInfo.LoggedUserTenant; // Get Open Transaction by Itzik service , the get is inside post method

        urlparameters = this.parseFiltersToURL(filters, urlparameters);

        var callUrl = url.concat(urlparameters);
        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    private parseFiltersToURL(filters: ApiQueryFilters, urlparameters: string) {
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
        return urlparameters;
    }

    getAutomaticReconcileByFilter(method1: string, method2: string, method3: string, accountId: string, filters: ApiQueryFilters) {


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
        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    getLedgerTransactionsByIds(Ids: string[]) {

        var params: string = "";
        for (var id of Ids) {
            params += "Ids[]=" + id + "&";
        }

        var url = this._apiUrl + '/getLedgerTransactionsByIds?' + params;
        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }


    GetFirst500LedgerForReconciliation(accountId: string, filters: ApiQueryFilters) {


        var url = this._reconciliationUrl + "/GetFirst500LedgerForReconciliation";

        var callUrl = this.ParseFiltersIntoURL(accountId, filters, url);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }


    GetFirstXLedgerForReconciliationByParam(accountId: string, filters: ApiQueryFilters) {


        var url = this._reconciliationUrl + "/GetFirstXLedgerForReconciliationByParam";

        var callUrl = this.ParseFiltersIntoURL(accountId, filters, url);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }
    GetFirstXLedgerForReconciliationByParams(accountId: string, filters: ApiQueryFilters) {
      

        var url = this._reconciliationUrl + "/GetReconciliationsByFilter";

        var callUrl = this.ParseFiltersIntoURL(accountId, filters, url);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GetFirstXLedgerForExtReconciliationByParam(objectTableName: string, objectTableId: string, entityId: string, filters: ApiQueryFilters) {
      
        
        var url = ServiceHelper.GetLogitudeURL()+'api/ReconcileExternalPagesExtended/getExternalReoncilioationsByFilter?objectTableId='+
         objectTableId+'&entityId='+entityId;
        var callUrl = this.parseFiltersToURL(filters,url);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    private ParseFiltersIntoURL(accountId: string, filters: ApiQueryFilters, url: string)
    {
        var urlparameters = '?gLAccountId=' + accountId  + '&tenant=' + SessionInfo.LoggedUserTenant;
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
        return url.concat(urlparameters);
    }

    getLast10TransactionsForAccount(accountId: string) {

        var url = this._apiUrl + '/GetLast10TransactionsForAccount?AccountId=' + accountId;
        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    getTransactionsForARPayment(arpaymentId: string, billToGLAccountId: string, paymentCurrencyId:string) {



        var url = this._apiUrl + '/GetTransactionsForARPayment?arpaymentId=' + arpaymentId
            + '&billToGLAccountId=' + billToGLAccountId + '&paymentCurrencyId=' + paymentCurrencyId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GetTransactionsForAPPayment(appaymentId: string, billToGLAccountId: string, paymentCurrencyId:string) {



        var url = this._apiUrl + '/GetTransactionsForAPPayment?appaymentId=' + appaymentId
            + '&billToGLAccountId=' + billToGLAccountId + '&paymentCurrencyId=' + paymentCurrencyId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    

    GetTransactionsCurrencies(AccountId:string, splittedByCurrencyCheckBox: boolean, attachedGLAccountChanged: boolean) {

        var urlparameters = '/GetTransactionsCurrencies?AccountId=' + AccountId + '&splittedByCurrencyCheckBox=' + splittedByCurrencyCheckBox
        + '&attachedGLAccountChanged=' + attachedGLAccountChanged;

        var callUrl = this._apiUrl.concat(urlparameters);
        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response:ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


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
