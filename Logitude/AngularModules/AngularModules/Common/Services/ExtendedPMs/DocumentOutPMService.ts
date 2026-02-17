import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable}     from 'rxjs/Rx';

import {DocumentOutPM} from '../../EntityPMs/DocumentOutPM';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 


@Injectable()
export class DocumentOutPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentOutExtended';
    }


    GetEntityPartners(entityId: string, objectTableName: string, childEntityId: string = "" , childobjectTableName: string = "" ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getentitypartners/?' + 'entityId=' + entityId + '&objectTableName=' + objectTableName + '&childEntityId=' + childEntityId + '&childobjectTableName=' + childobjectTableName, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }








    getDocumentOutsByEntityIdAndObjectTable(entityId: string, childEntityId: string, objectTableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumentoutsbyentityidandobjecttable/?' + 'entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&tenant=' + tenant , { headers: authHeader }).map(response => {
        
            
                var result = response.json();
                var entity: DocumentOutPM;
                var DocumentOutPMLists: DocumentOutPM[];
                DocumentOutPMLists = new Array<DocumentOutPM>();
                result.forEach((item) => {
                    entity = this.MapJsonToEntityPM(item);
                    DocumentOutPMLists.push(entity);
                });
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = DocumentOutPMLists;
                return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    getCreateDocumentOut(documentTypeId: string, entityId: string, childEntityId: string, childReference: string, objectTableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getcreatedocumentout/?' +'documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
                var result = response.json();
                var entity: DocumentOutPM;
                entity = this.MapJsonToEntityPM(result);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = entity;
                return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);



        //var authHeader = new Headers();
        //authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        //return this._http.get(logitude_url + 'api/DocumentOutExtended' + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
        //    var result = response.json();
        //    var entity: DocumentOutPM;
        //    entity = this.MapJsonToEntityPM(result);
        //    return entity;
        //});







    }


    getSingleDocumentOutPM(id: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/getsingledocumentoutpm/?'+ 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
                var entity: DocumentOutPM;
                entity = this.MapJsonToEntityPM(result);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


    GetCalculatedFileNameForDocumentOutCopy(documentOutId: string, documentTypeCopyId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetCalculatedFileNameForDocumentOutCopy/?' + 'documentOutId=' + documentOutId + '&documentTypeCopyId=' + documentTypeCopyId, { headers: authHeader }).map(response => {
            var result = response.json();
          
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    
 
    
    putDocumentOut(entityPM: DocumentOutPM) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(entityPM), {
                headers: authHeader,

            }).map(response => {
               var result = response.json();
                var entity: DocumentOutPM;
                entity = this.MapJsonToEntityPM(result);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
                }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    getDocumentOutByDocumentTypeEntityAndChild(entityId: string, tenant: number, childEntityId: string,documentTypeId: string ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/getDocumentOutByDocumentTypeEntityAndChild/?' + 'entityId=' + entityId + '&tenant=' + tenant + '&childEntityId=' + childEntityId + '&documentTypeId=' + documentTypeId, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: DocumentOutPM ;
            entity = result;
            if (result) {
                entity = this.MapJsonToEntityPM(result);
            }
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    





    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: DocumentOutPM;
        entityPM = new DocumentOutPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }



}

