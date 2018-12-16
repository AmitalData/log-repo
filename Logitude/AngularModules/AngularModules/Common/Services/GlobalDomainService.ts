import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Rx';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {TenantManagementList} from '../../Infrastructure/EntityLists/TenantManagementList';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ARInvoicePM} from '../../Invoice/EntityPMs/ARInvoicePM';
import {Guid} from '../../Infrastructure/Utilities/Guid';

import {ARInvoiceLinePM} from '../../Invoice/EntityPMs/ARInvoiceLinePM';
import {ARInvoiceEntityPM} from '../../Invoice/EntityPMs/ARInvoiceEntityPM';
import {ARInvoicePaymentPM} from '../../Invoice/EntityPMs/ARInvoicePaymentPM';
import {ARInvoiceTransferHistoryPM} from '../../Invoice/EntityPMs/ARInvoiceTransferHistoryPM';
import {ConstituentPM} from '../../Invoice/EntityPMs/ConstituentPM';
import {ARInvoiceValidator} from '../../Invoice/Validators/ARInvoiceValidator';
import {BatchServicesDefinitionPM} from '../../Infrastructure/EntityPMs/BatchServicesDefinitionPM';

@Injectable()

export class GlobalDomainService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GlobalDomain';
    }

    GetMessagingStockTenantsList(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMessagingStockTenantsList?tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<TenantManagementList> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TenantManagementList = this.MapTenantManagementList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetQuickBooksOnlineVatTypesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {
         


            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineVatTypesById?Id=' + Id , {
                headers: authHeader
            }).map(response => {

            
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetQuickBooksOnlineReceivableChargesTypesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineReceivableChargesTypesById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetQuickBooksOnlinePayablesChargesTypesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlinePayablesChargesTypesById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetQuickBooksOnlinePaymentMethodsById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlinePaymentMethodsById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetQuickBooksOnlinePaymentTermsById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlinePaymentTermsById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetQuickBooksOnlineCurrenciesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineCurrenciesById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetQuickBooksOnlineCustomerById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineCustomersById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetQuickBooksOnlineVendorById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return Observable.defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineVendorById?Id=' + Id, {
                headers: authHeader
            }).map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetAccountingSystem(AccountingSystemCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAccountingSystem?AccountingSystemCode=' + AccountingSystemCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myResultJason = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResultJason;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    InvoiceToQuickBooks(invoiceId: string, id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';        
 

        return Observable.defer(() => {

            return this._http.get(this._apiUrl + '/GetInvoiceToQuickBooks?Customerid=' + id + '&invoiceId=' + invoiceId, {
                headers: authHeader
            }).map(response => {     
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetQuickBooksQueries(args: any,SearchText:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return Observable.defer(() => {

            return this._http.get(this._apiUrl + '/GetQuickBooksQueries?CardName=' + args.CardName + '&SearchField=' + args.SearchField + '&SearchText=' + SearchText + '&ReceivableCard=' + args.ReceivableCard + '&PayableCard=' + args.PayableCard + '&LogitudeCardName=' + args.LogitudeCardName, {
                headers: authHeader
            }).map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    MapTenantManagementList(jsonList: any) {
        var entityList: TenantManagementList;
        entityList = new TenantManagementList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    
    GetAllHelpResources() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAllHelpResources?', {
                headers: authHeader
            }).map(response => {
                var myResult = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAirlineTenantExistsForAirline(code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineTenantExistsForAirline?code=' + code;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myResultJason = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResultJason;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAllBatchServicesDefinitionsPMs(filterByDateCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllBatchServicesDefinitionsPMs?filterByDateCode=' + filterByDateCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<BatchServicesDefinitionPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: BatchServicesDefinitionPM = this.MapBatchServicesDefinitionPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapBatchServicesDefinitionPM(jsonList: any) {
        var entityList: BatchServicesDefinitionPM;
        entityList = new BatchServicesDefinitionPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    UpdateTenantZeroService(Message:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpdateTenantZeroService?Message=' + Message;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var itemJason = response.json();
                var itemMapped: Boolean = itemJason;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetParentTenants() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetParentTenants?';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<TenantManagementList> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TenantManagementList = this.MapTenantManagementList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}
