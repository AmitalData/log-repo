import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DocumentsFilingPM} from '../../../Common/EntityPMs/DocumentsFilingPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustDocRelatedDocsWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocRelatedDocsWebService';
    }

    GetDocumentsFilingsForRelatedDocuments(entityId: string, childEntityId: string, objectTableId: string, directionCode: string, referenceNumber: string, filterVlaue: string, declarationType: string = null,ExportFile?: string) {
    
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDocumentsFilingsForRelatedDocuments?' + 'entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&referenceNumber=' + referenceNumber + '&filterVlaue=' + filterVlaue + '&declarationType='+ declarationType + '&ExportFile='+ ExportFile, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
                var _mappedListsArray: Array<DocumentsFilingPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DocumentsFilingPM;
                        entity = this.MapJsonToEntityPMs(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    GetSingleDocumentsFilingPM(id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        id = encodeURIComponent(id);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleDocumentsFilingPM?' + 'id=' + id ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                var entity: DocumentsFilingPM;
                entity = this.MapJsonToEntityPMs(serviceResponse.Result);
                serviceResponse.Result = entity
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });
    }

    UpdateSupplierInvioceByOcr(declarationId: string, documentsFilingId: string) {
        debugger
       
        return defer(() => {

            return this._http.get(this._apiUrl + '/UpsertSupplierInvioceByOcr?' + 'declarationId=' + declarationId + '&documentsFilingId=' + documentsFilingId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
        debugger
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));


        });


    }

    MapJsonToEntityPMs(jsonPM: any) {
        var entityPM: DocumentsFilingPM;
        entityPM = new DocumentsFilingPM();
        var jsonListKeys = Object.keys(jsonPM);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}
