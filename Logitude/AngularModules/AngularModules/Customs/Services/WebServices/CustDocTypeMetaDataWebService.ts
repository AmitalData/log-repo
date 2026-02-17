import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomDocumentTypeMetaDataPM} from '../../EntityPMs/CustomDocumentTypeMetaDataPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustDocTypeMetaDataWebService {

    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocTypeMetaDataWebService';
    }

    GetCustomDocumentTypeMetaDataByType(customDocumentTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var params = encodeURIComponent(customDocumentTypeCode);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomDocumentTypeMetaDataByType?' + 'customDocumentTypeCode=' + params, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();
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
            }).catch(ServiceHelper.HandleServiceError);
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