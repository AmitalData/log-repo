import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SupplierInvoiceItemList } from '../../EntityLists/Extended/SupplierInvoiceItemList';


export class SupplierInvoiceExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SupplierInvoice';
    }

    GetSupplierInvoiceItemsForInvoice(declarationId: string, counterkey: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetSupplierInvoiceItemsForInvoice';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSupplierInvoiceItemsForInvoice/?' + 'declarationId=' + declarationId + '&counterkey=' + counterkey, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                var _mappedListsArray: Array<SupplierInvoiceItemList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: SupplierInvoiceItemList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSupplierInvoiceItemsForInvoices(declarationId: string, supplierInvoiceCounterKeys: string, skip: number, take: number, getCount: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetSupplierInvoiceItemsForInvoices';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSupplierInvoiceItemsForInvoices/?' + 'declarationId=' + declarationId + '&supplierInvoiceCounterKeys=' + supplierInvoiceCounterKeys + '&skip=' + skip + '&take=' + take + '&getCount=' + getCount , ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {


                //var serviceResponse: ServiceResponse = new ServiceResponse();
                var serviceResponse: ServiceResponse = response;
                var _mappedListsArray: Array<SupplierInvoiceItemList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: SupplierInvoiceItemList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSelectedSupplierInvoiceItemLists(declarationId: string, supplierInvoiceCounterKeys: string, lineNumbers: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetSelectedSupplierInvoiceItems';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSelectedSupplierInvoiceItems/?' + 'declarationId=' + declarationId + '&supplierInvoiceCounterKeys=' + supplierInvoiceCounterKeys + '&lineNubmers=' + lineNumbers, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {


                //var serviceResponse: ServiceResponse = new ServiceResponse();
                var serviceResponse: ServiceResponse = response;
                var _mappedListsArray: Array<SupplierInvoiceItemList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: SupplierInvoiceItemList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: SupplierInvoiceItemList;
        entityList = new SupplierInvoiceItemList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }




}
