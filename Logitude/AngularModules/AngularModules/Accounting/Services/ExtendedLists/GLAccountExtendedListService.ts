import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable }     from 'rxjs/Rx';
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

@Injectable()

export class GLAccountExtendedListService {
    private _http: Http
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/glaccountviews';
     }


    GetRecentGLAccounts(accountTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetRecentGLAccounts?accountTypeCode=' + accountTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, {  headers: authHeader  }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
             }).catch(ServiceHelper.HandleServiceError);
         });
     }


    GetChildrenGLAccounts(GLAccountId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetChildrenGLAccounts?GLAccountId=' + GLAccountId , {
                headers: authHeader
            }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();

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
            });
        });
    }

    GetSplittedByCurrencyGLAccounts(accountId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSplittedByCurrencyGLAccounts?accountId=' + accountId, {
                headers: authHeader
            }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();

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
            });
        });

    }

    CheckIfHasLedgerTransactions(accountId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetAccountTransactions?accountId=' + accountId;

        return Observable.defer(()=> {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var res = response.json();
                if (res && res != null) {
                    if (res.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CheckIfSplitted(accountId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/CheckIfSplitted?accountId=' + accountId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var res = response.json();
                if (res && res != null) {
                    if (res.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetGLAccountsSummary() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetGLAccountsSummary?', {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }

    CalculateFututreCheques() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCalculateFututreCheques?', {
                headers: authHeader
            }).map(response => {

              
                return response.json();
            });
        });
    }

    GetAccountCurrencies(accountId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetAccountCurrencies?accountId=' + accountId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAccountReconcilesCount(accountId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAccountReconcilesCount?glAccountId=' + accountId, {
                headers: authHeader
            }).map(response => {
                var res = response.json();
                return res;
            });
        });
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

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetGLAccountsAgingReport?' + urlParameters, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
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
            });
        });
    }

    GetTopDeptors(filter: string, accountTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTopDeptors?filterString=' + filter + '&accountTypeCode=' + accountTypeCode, {
                headers: authHeader
            }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();

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
            });
        });
    }

    SetParentAccountId(id: string, parentId:string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetParentAccountId?' + 'id=' + id + '&' + 'parentId=' + parentId, {
                headers: authHeader
            }).map(response => {
                var list = response.json();


                var entity: GLAccountList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAccountOpenTransactionsCount(accountId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetAccountOpenTransactionsCount?accountId=' + accountId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var result = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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
