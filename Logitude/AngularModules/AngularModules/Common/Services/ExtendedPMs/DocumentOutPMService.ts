import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {DocumentOutPM} from '../../EntityPMs/DocumentOutPM';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { property } from 'cypress/types/lodash';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';


@Injectable()
export class DocumentOutPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentOutExtended';
    }


    GetEntityPartners(entityId: string, objectTableName: string, childEntityId: string = '' , childobjectTableName: string = '' , GlAccountId: string = '' ) {
        const authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getentitypartners/?' + 'entityId=' + entityId +
            '&objectTableName=' + objectTableName +
            '&childEntityId=' + childEntityId +
            '&childobjectTableName=' + childobjectTableName +
            '&gLAccountId=' + GlAccountId
            , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    getDocumentOutsByEntityIdAndObjectTable(entityId: string, childEntityId: string, objectTableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumentoutsbyentityidandobjecttable/?' + 'entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var result :any = response;
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
        }),catchError(ServiceHelper.HandleServiceError));
    }


    private CurrentSession = SessionLocator.SelectedSession;

    getCreateDocumentOut(documentTypeId: string, entityId: string, childEntityId: string, childReference: string, objectTableId: string, tenant: number, documentTypeTemplateId:string = null,signHSM:boolean=false) {

        var createDocumentOutArgs: CreateDocumentOutArgs = new CreateDocumentOutArgs();
        createDocumentOutArgs.DocumentTypeId = documentTypeId;
        createDocumentOutArgs.ChildEntityId = childEntityId;
        createDocumentOutArgs.EntityId = entityId;
        createDocumentOutArgs.ChildReference = childReference;
        createDocumentOutArgs.ObjectTableId = objectTableId;
        createDocumentOutArgs.Tenant = tenant;
        createDocumentOutArgs.DocumentTypeTemplateId = documentTypeTemplateId;
        createDocumentOutArgs.SignHSM = signHSM;

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + "/PutCreateDocumentOut", JSON.stringify(createDocumentOutArgs), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                 var result: any = response;
                var entity: DocumentOutPM;
                entity = this.MapJsonToEntityPM(result);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = entity;
                this.CurrentSession.FireEvent("IsSignedChanged");
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );

    }




    //getCreateDocumentOut(documentTypeId: string, entityId: string, childEntityId: string, childReference: string, objectTableId: string, tenant: number) {

    //    var createDocumentOutArgs: CreateDocumentOutArgs = new CreateDocumentOutArgs();
    //    createDocumentOutArgs.DocumentTypeId = documentTypeId;
    //    createDocumentOutArgs.ChildEntityId = childEntityId;
    //    createDocumentOutArgs.EntityId = entityId;
    //    createDocumentOutArgs.ChildReference = childReference;
    //    createDocumentOutArgs.ObjectTableId = objectTableId;
    //    createDocumentOutArgs.Tenant = tenant;


    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


    //    return this._http.get(this._apiUrl + '/getcreatedocumentout/?' +'documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
    //            var result :any = response;
    //            var entity: DocumentOutPM;
    //            entity = this.MapJsonToEntityPM(result);
    //            var pmresponse: ServiceResponse;
    //            pmresponse = new ServiceResponse();
    //            pmresponse.Result = entity;
    //            return pmresponse;
    //    }),catchError(ServiceHelper.HandleServiceError));



    //    //var authHeader = new Headers();
    //    //authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


    //    //return this._http.get(logitude_url + 'api/DocumentOutExtended' + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
    //    //    var result :any = response;
    //    //    var entity: DocumentOutPM;
    //    //    entity = this.MapJsonToEntityPM(result);
    //    //    return entity;
    //    //});







    //}


    getSingleDocumentOutPM(id: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/getsingledocumentoutpm/?'+ 'id=' + id + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
                var entity: DocumentOutPM;
                entity = this.MapJsonToEntityPM(result);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    GetCalculatedFileNameForDocumentOutCopy(documentOutId: string, documentTypeCopyId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetCalculatedFileNameForDocumentOutCopy/?' + 'documentOutId=' + documentOutId + '&documentTypeCopyId=' + documentTypeCopyId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }




    putDocumentOut(entityPM: DocumentOutPM) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + "/PutDocumentOut", JSON.stringify(entityPM),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
               var result :any = response;
                var entity: DocumentOutPM;
                entity = this.MapJsonToEntityPM(result);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    getDocumentOutByDocumentTypeEntityAndChild(entityId: string, tenant: number, childEntityId: string,documentTypeId: string ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/getDocumentOutByDocumentTypeEntityAndChild/?' + 'entityId=' + entityId + '&tenant=' + tenant + '&childEntityId=' + childEntityId + '&documentTypeId=' + documentTypeId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var entity: DocumentOutPM ;
            entity = result;
            if (result) {
                entity = this.MapJsonToEntityPM(result);
            }
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
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
export class CreateDocumentOutArgs{

    DocumentTypeId: string;
    EntityId: string;
    ChildEntityId: string;
    ChildReference: string;
    ObjectTableId: string;
    Tenant: number;
    DocumentTypeTemplateId: string;
    SignHSM:boolean
}

