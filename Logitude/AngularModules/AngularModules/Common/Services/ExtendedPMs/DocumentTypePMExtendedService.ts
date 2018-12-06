


import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeTemplatePM} from '../../EntityPMs/DocumentTypeTemplatePM';

import {DocumentTypePM} from '../../EntityPMs/DocumentTypePM';

@Injectable()
export class DocumentTypePMExtendedService {



    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeExtended';
    }


    GetSinglePMWithOutInclude(id: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSinglePMWithOutInclude/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: DocumentTypePM = this.MapJsonToEntityPM(result);
            var pmresponse: ServiceResponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }



    getDocumentTypesByEnityIdAndtransportModeId(transportModeId: string, shipmentLevelCode: string, objecttableId: string, tenant: number, childrenObjectTableIds: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getdocumenttypesbyenityidandtransportmodeid/?'+ 'transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode + '&objecttableId=' + objecttableId + '&tenant=' + tenant + '&childrenObjectTableIds=' + childrenObjectTableIds, { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetFollowUpDocumentTypeByEntityId(entityId: string,  objectTableName:string,  tenant:number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/GetFollowUpDocumentTypeByEntityId/?' + 'entityId=' + entityId + '&objectTableName=' + objectTableName + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(entityId: string, agentId: string, agentReference: string, objecttableId: string, shipmentLevelCode: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel/?' + 'entityId=' + entityId + '&agentId=' + agentId + '&agentReference=' + agentReference + '&objecttableId=' + objecttableId + '&shipmentLevelCode=' + shipmentLevelCode + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetDocumentTypeCopiesByDocumentTypeId(id: string, documentOutId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDocumentTypeCopiesByDocumentTypeId/?' + 'id=' + id + '&documentOutId=' + documentOutId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }






    GetDocumentTypesPMByObjectTableIdForDocumentPremissions(objecttableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/GetDocumentTypesPMByObjectTableIdForDocumentPremissions/?' + 'objecttableId=' + objecttableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }



    GetDocumentTypesByObjectTableAndTenant(objecttableId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getdocumenttypesbyobjecttableandtenant/?' + 'objecttableId=' + objecttableId + '&tenant=' + tenant  , { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }


    GetDoesDocumentTypeCodeExist(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdoesdocumenttypecodeexist/?' + 'code=' + code + '&tenant=' + tenant + '&x=' + 1 , { headers: authHeader }).map(response => {


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    getSingleDocumentType(id: string, documentOutId: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getsingledocumenttype/?' +  'id=' + id + '&documentOutId=' + documentOutId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
                var entity: DocumentTypePM;
                entity = this.MapJsonToEntityPM(result);

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }




    putDocumentType(entityPM: DocumentTypePM) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/putdocumenttype', JSON.stringify(entityPM), {
                headers: authHeader,

            }).map(response => {
                var pm = response.json();
                var entity: DocumentTypePM;
                  entity = this.MapJsonToEntityPM(pm);
                  var pmresponse: ServiceResponse;
                  pmresponse = new ServiceResponse();

                  pmresponse.Result = entity;
                  return pmresponse;
                }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }

    update(eventTypePMLists: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/putupdatedocumenttypepmlists', JSON.stringify(eventTypePMLists),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

    GetDocumentTypeByCode(code: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumenttypebycode/?'+  'code=' + code + '&tenant=' + tenant , { headers: authHeader }).map(response => {

            var result = response.json();

            var entity: DocumentTypePM;
            if(result)
                entity = this.MapJsonToEntityPM(result);

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
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

