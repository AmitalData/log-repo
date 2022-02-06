


import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeTemplatePM} from '../../EntityPMs/DocumentTypeTemplatePM';

import {DocumentTypePM} from '../../EntityPMs/DocumentTypePM';

@Injectable()
export class DocumentTypePMExtendedService {



    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeExtended';
    }


    GetSinglePMWithOutInclude(id: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSinglePMWithOutInclude/?' + 'id=' + id + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var entity: DocumentTypePM = this.MapJsonToEntityPM(result);
            var pmresponse: ServiceResponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }



    getDocumentTypesByEnityIdAndtransportModeId(transportModeId: string, shipmentLevelCode: string, objecttableId: string, tenant: number, childrenObjectTableIds: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getdocumenttypesbyenityidandtransportmodeid/?'+ 'transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode + '&objecttableId=' + objecttableId + '&tenant=' + tenant + '&childrenObjectTableIds=' + childrenObjectTableIds,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
                var entity: DocumentTypePM;
                var documentTypePMLists: DocumentTypePM[];
                documentTypePMLists = new Array<DocumentTypePM>();


                result.forEach((item) => {
                    entity = this.MapJsonToEntityPM(item);
                    documentTypePMLists.push(entity);
                });
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = documentTypePMLists;
                return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetFollowUpDocumentTypeByEntityId(entityId: string,  objectTableName:string,  tenant:number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/GetFollowUpDocumentTypeByEntityId/?' + 'entityId=' + entityId + '&objectTableName=' + objectTableName + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var entity: DocumentTypePM;
            var documentTypePMLists: DocumentTypePM[];
            documentTypePMLists = new Array<DocumentTypePM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(entityId: string, agentId: string, agentReference: string, objecttableId: string, shipmentLevelCode: string, tenant: number, shareDocumentsFrom: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel/?' + 'entityId=' + entityId + '&agentId=' + agentId + '&agentReference=' + agentReference + '&objecttableId=' + objecttableId + '&shipmentLevelCode=' + shipmentLevelCode + '&tenant=' + tenant + '&shareDocumentsFrom=' + shareDocumentsFrom,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetDocumentTypeCopiesByDocumentTypeId(id: string, documentOutId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDocumentTypeCopiesByDocumentTypeId/?' + 'id=' + id + '&documentOutId=' + documentOutId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }






    GetDocumentTypesPMByObjectTableIdForDocumentPremissions(objecttableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/GetDocumentTypesPMByObjectTableIdForDocumentPremissions/?' + 'objecttableId=' + objecttableId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var entity: DocumentTypePM;
            var documentTypePMLists: DocumentTypePM[];
            documentTypePMLists = new Array<DocumentTypePM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }



    GetDocumentTypesByObjectTableAndTenant(objecttableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getdocumenttypesbyobjecttableandtenant/?' + 'objecttableId=' + objecttableId + '&tenant=' + tenant  ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var entity: DocumentTypePM;
            var documentTypePMLists: DocumentTypePM[];
            documentTypePMLists = new Array<DocumentTypePM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    GetDoesDocumentTypeCodeExist(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdoesdocumenttypecodeexist/?' + 'code=' + code + '&tenant=' + tenant + '&x=' + 1 ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }



    getSingleDocumentType(id: string, documentOutId: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getsingledocumenttype/?' +  'id=' + id + '&documentOutId=' + documentOutId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result :any = response;
                var entity: DocumentTypePM;
                entity = this.MapJsonToEntityPM(result);

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }




    putDocumentType(entityPM: DocumentTypePM) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + '/putdocumenttype', JSON.stringify(entityPM),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;
                var entity: DocumentTypePM;
                  entity = this.MapJsonToEntityPM(pm);
                  var pmresponse: ServiceResponse;
                  pmresponse = new ServiceResponse();

                  pmresponse.Result = entity;
                  return pmresponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }

    update(eventTypePMLists: any) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/putupdatedocumenttypepmlists', JSON.stringify(eventTypePMLists), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }

    GetDocumentTypeByCode(code: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumenttypebycode/?'+  'code=' + code + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result :any = response;

            var entity: DocumentTypePM;
            if(result)
                entity = this.MapJsonToEntityPM(result);

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: DocumentTypePM;
        entityPM = new DocumentTypePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}

