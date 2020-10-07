import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

import {DocumentTypeList} from '../../EntityLists/DocumentTypeList';

@Injectable()
export class DocumentTypeListExtendedService {



    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeExtended';
    }



    getDocumentTypeListById(id: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getDocumentTypeListById/?' + 'id=' + id + '&tenant=' + tenant + '&s=true',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
       
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    getDocumentTypeListsByEnityIdAndTenant(transportModeId: string, shipmentLevelCode: string, objecttableId: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getDocumentTypeListsByEnityIdAndTenant/?' + 'transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode + '&objecttableId=' + objecttableId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    

    getDocumentTypeListByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
  
        return this._http.get(this._apiUrl + '/getDocumentTypeListByCode/?' + 'tenant=' + tenant + '&code=' + code + '&s=true',ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


   getDocumentTypesListByObjectTableAndTenant(tenant: number, objectTableid: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getDocumentTypesListByObjectTableAndTenant/?' + 'tenant=' + tenant + '&objectTableid=' + objectTableid + "&s='ss'" ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    
   getDocumentTypeListsByObjectTableIdForAutomations(objectTableId: string,  tenant:number) {


       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

       return this._http.get(this._apiUrl + '/getdocumenttypelistsbyobjecttableidforautomations/?' + 'objectTableId=' + objectTableId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = response;
           return pmresponse;
       }),catchError(ServiceHelper.HandleServiceError));
   }

   getTop5DocumentTypesPMsByObjectTableAndTenant(tenant: number, objectTableid: string) {
       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

       return this._http.get(this._apiUrl + '/getTop5DocumentTypesPMsByObjectTableAndTenant/?' + 'tenant=' + tenant + '&objectTableid=' + objectTableid,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = response;
           return pmresponse;
       }),catchError(ServiceHelper.HandleServiceError));
   }

   
    GetDocumentTypeCopyLists(objectTableId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/GetDocumentTypeCopyLists/?' + 'objectTableId=' + objectTableId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
  


}

