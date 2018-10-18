import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DocumentsFilingPM} from '../../../Common/EntityPMs/DocumentsFilingPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustDocRelatedDocsWebService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocRelatedDocsWebService';
    }

    GetDocumentsFilingsForRelatedDocuments(entityId: string, childEntityId: string, objectTableId: string, directionCode: string, referenceNumber:string, filterVlaue:string ){
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDocumentsFilingsForRelatedDocuments?' + 'entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&referenceNumber=' + referenceNumber + '&filterVlaue=' + filterVlaue, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();
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
            }).catch(ServiceHelper.HandleServiceError);


        });


    }

    GetSingleDocumentsFilingPM(id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        id = encodeURIComponent(id);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleDocumentsFilingPM?' + 'id=' + id ,{ headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                var entity: DocumentsFilingPM;
                entity = this.MapJsonToEntityPMs(serviceResponse.Result);
                serviceResponse.Result = entity
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


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