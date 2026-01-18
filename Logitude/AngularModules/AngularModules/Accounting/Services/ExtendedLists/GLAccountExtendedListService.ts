declare var window: any;

import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { GLAccountList } from '../../EntityLists/GLAccountList';
import { GLAccountTotalByMonthList } from '../../EntityLists/GLAccountTotalByMonthList';
import { PeriodM } from '../../DataContracts/PeriodM';
import { AgingReportParameters } from '../../DataContracts/AgingReportParameters';
import { LedgerTransactionListService } from '../StandardLists/LedgerTransactionListService'
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../../Infrastructure/Tools';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';

@Injectable()

export class GLAccountExtendedListService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {

        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/glaccountviews';
     }

    GetRecentGLAccounts(accountTypeCode: string) {


        var url = this._apiUrl + '/GetRecentGLAccounts?accountTypeCode=' + accountTypeCode;
        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GetInsertControlAccount(ControlAccountId:string , ChartOfAccountsId: string )
    {


        var url = this._apiUrl + '/GetInsertControlAccount?ControlAccountId=' + ControlAccountId + '&ChartOfAccountsId=' + ChartOfAccountsId;
        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var resAccountId = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = resAccountId;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GetChildrenGLAccounts(GLAccountId: string) {


        return this.httpClient.get(this._apiUrl + '/GetChildrenGLAccounts?GLAccountId=' + GLAccountId,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                var _mappedListsArray: Array<GLAccountList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: GLAccountList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetSplittedByCurrencyGLAccounts(accountId: string) {


        return this.httpClient.get(this._apiUrl + '/GetSplittedByCurrencyGLAccounts?accountId=' + accountId,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                var _mappedListsArray: Array<GLAccountList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: GLAccountList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));



    }

    CheckIfHasLedgerTransactions(accountId: string) {



        var url = this._apiUrl + '/GetAccountTransactions?accountId=' + accountId;
         return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map((response:any) => {
                var res:any = response;
                if (res && res != null) {
                    if (res.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    CheckIfSplitted(accountId: string) {



        var url = this._apiUrl + '/CheckIfSplitted?accountId=' + accountId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map((response:any) => {
                var res = response;
                if (res && res != null) {
                    if (res.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }),
            catchError(ServiceHelper.HandleServiceError));


    }
    GetTotalOpenChequesInLocalCurById(glaccountId: string) {
        var api = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
        var url = api + '/GetTotalOpenChequesInLocalCurById?glaccountId=' + glaccountId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map((response:any) => {
                var res = response;
                if (res && res != null) {
                    return res;
                }
                else {
                    return 0;
                }
            }),
            catchError(ServiceHelper.HandleServiceError));

    }
    GetGLAccountsSummary() {



      return this.httpClient.get(this._apiUrl + '/GetGLAccountsSummary?',  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {

            var allLists = response;
            return allLists;

        }),
        catchError(ServiceHelper.HandleServiceError));


    }

    CalculateFututreCheques() {

        return this.httpClient.get(this._apiUrl + '/GetCalculateFututreCheques?',  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                return response;

            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GetAccountCurrencies(accountId: string) {



        var url = this._apiUrl + '/GetAccountCurrencies?accountId=' + accountId;
        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GetAccountReconcilesCount(accountId: string) {

       return this.httpClient.get(this._apiUrl + '/GetAccountReconcilesCount?glAccountId='+ accountId,  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {

            var res = response;
            return res;
        }),
        catchError(ServiceHelper.HandleServiceError));

    }

    GetAgingReport(args: AgingReportParameters) {

        var urlParameters="";
        urlParameters += "agingForDate=" + args.AgingForDate.toISOString();
        urlParameters +=  ( "&numberOfmonthsbackwards=" + args.NumberOfmonthsbackwards );
        urlParameters +=  "&vendorCustomerId=" + args.VendorCustomerId;
        urlParameters +=  "&category1Id=" + args.Category1Id;
        urlParameters +=  "&category2Id=" + args.Category2Id;
        urlParameters +=  "&category3Id=" + args.Category3Id;
        urlParameters +=  "&category4Id=" + args.Category4Id;
        urlParameters +=  "&category5Id=" + args.Category5Id;
        urlParameters +=  "&collectorId=" + args.CollectorId;
        urlParameters += "&salesmanId=" + args.SalesmanId;
        urlParameters += "&isCustomer=" + args.IsCustomer;
        urlParameters += "&groupByDate=" + args.GroupByDate;
        urlParameters += "&forceUseMonthMethod=" + args.ForceUseMonthMethod;

        return this.httpClient.get(this._apiUrl + '/GetGLAccountsAgingReport?'+ urlParameters,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                var _mappedListsArray: Array<PeriodM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: PeriodM;
                        entity = this.MapJsonToPeriodMList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetTopDeptors(filter: string, accountTypeCode: string) {


        return this.httpClient.get(this._apiUrl + '/GetTopDeptors?filterString=' + filter + '&accountTypeCode=' + accountTypeCode,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                var _mappedListsArray: Array<GLAccountList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: GLAccountList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    SetParentAccountId(id: string, parentId:string) {



        return this.httpClient.get(this._apiUrl + '/GetParentAccountId?' + 'id=' + id + '&' + 'parentId=' + parentId,  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {

            var list = response;


            var entity: GLAccountList;
            if (list) {
                entity = this.MapJsonToEntityList(list);
            }

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = entity;
            return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));


    }

    GetAccountOpenTransactionsCount(accountId: string) {


        var url = this._apiUrl + '/GetAccountOpenTransactionsCount?accountId=' + accountId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                var result = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetGLAccountExternalTransactionsTotal(accountId: string) {



        var url = this._apiUrl + '/GetGLAccountExternalTransactionsTotal?accountId=' + accountId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                var result = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }
    GetAccountTransactionsCount(accountId: string) {


        var url = this._apiUrl + '/GetAccountTransactionsCount?accountId=' + accountId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                var result = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }
    private _http: HttpClient;

    getByFilters(filters: ApiQueryFilters) {

        var callTime = new Date();
        var urlparameters = '/getbyfiltersshort?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                {
					propValue = encodeURIComponent(propValue);
					urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
				}

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
     
        this._http = ServiceHelper.HttpClient;
        var callUrl = this._apiUrl.concat(urlparameters);//
        
	   return defer(() => {
           return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {

               var serviceResponse: ServiceResponse;
               serviceResponse = response.body;
                var _mappedListsArray: Array< GLAccountList> = [];
				if(serviceResponse.Result)
				{
                for (var key in serviceResponse.Result) {
				
				   var entity: GLAccountList;
                   entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
				   _mappedListsArray.push(entity);

				 }
                }   

                serviceResponse.Result = _mappedListsArray;       
				serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "GLAccount", "GetByFilters", "PageIndex:" +filters.PageIndex +", PageSize:"+filters.PageSize + ", GetAll:" + filters.GetAll);
				           
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));;
        });        
    }

    getByFiltersShort(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (table.IsCustom) {
            filters.addAdditionalFilter("ObjectTableName", objectTableName, null, null, "Equals", false, false, false, "string");
        }
        var entityListService:EntityListService=new EntityListService()
        let servicelink = entityListService.GetServiceLink(table, MethodName);
               
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(this.getByFilters(filters));
            });
        });
        
    }



    MapJsonToEntityList(jsonList: any) {

        var entityList: GLAccountList;
        entityList = new GLAccountList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
         }


        return entityList;
    }

    MapJsonToPeriodMList(jsonList: any) {

        var entityList: PeriodM;
        entityList = new PeriodM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToGLAccountTotalByMonthEntityList(jsonList: any) {

        var entityList: GLAccountTotalByMonthList;
        entityList = new GLAccountTotalByMonthList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }


 }
