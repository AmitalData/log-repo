import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

import {DocumentTypeList} from '../../EntityLists/DocumentTypeList';

@Injectable()
export class DocumentTypeListExtendedService {



    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeExtended';
    }



    getDocumentTypeListById(id: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getDocumentTypeListById/?' + 'id=' + id + '&tenant=' + tenant + '&s=true', { headers: authHeader }).map(response => {
       
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getDocumentTypeListsByEnityIdAndTenant(transportModeId: string, shipmentLevelCode: string, objecttableId: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getDocumentTypeListsByEnityIdAndTenant/?' + 'transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode + '&objecttableId=' + objecttableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    

    getDocumentTypeListByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
  
        return this._http.get(this._apiUrl + '/getDocumentTypeListByCode/?' + 'tenant=' + tenant + '&code=' + code + '&s=true', { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


   getDocumentTypesListByObjectTableAndTenant(tenant: number, objectTableid: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getDocumentTypesListByObjectTableAndTenant/?' + 'tenant=' + tenant + '&objectTableid=' + objectTableid + "&s='ss'" , { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    
   getDocumentTypeListsByObjectTableIdForAutomations(objectTableId: string,  tenant:number) {


       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

       return this._http.get(this._apiUrl + '/getdocumenttypelistsbyobjecttableidforautomations/?' + 'objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = response.json();
           return pmresponse;
       }).catch(ServiceHelper.HandleServiceError);
   }

   getTop5DocumentTypesPMsByObjectTableAndTenant(tenant: number, objectTableid: string) {
       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

       return this._http.get(this._apiUrl + '/getTop5DocumentTypesPMsByObjectTableAndTenant/?' + 'tenant=' + tenant + '&objectTableid=' + objectTableid, { headers: authHeader }).map(response => {

           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = response.json();
           return pmresponse;
       }).catch(ServiceHelper.HandleServiceError);
   }

   


  


}

