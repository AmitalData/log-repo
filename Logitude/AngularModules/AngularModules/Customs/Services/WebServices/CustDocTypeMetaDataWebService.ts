import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomDocumentTypeMetaDataPM} from '../../EntityPMs/CustomDocumentTypeMetaDataPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustDocTypeMetaDataWebService {

    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocTypeMetaDataWebService';
    }

    GetCustomDocumentTypeMetaDataByType(customDocumentTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var params = encodeURIComponent(customDocumentTypeCode);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomDocumentTypeMetaDataByType?' + 'customDocumentTypeCode=' + params, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
                var _mappedListsArray: Array<CustomDocumentTypeMetaDataPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomDocumentTypeMetaDataPM;
                        entity = this.MapJsonToEntityPMs(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
        
    }

    MapJsonToEntityPMs(jsonPM: any) {
        var entityPM: CustomDocumentTypeMetaDataPM;
        entityPM = new CustomDocumentTypeMetaDataPM();
        var jsonListKeys = Object.keys(jsonPM);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}