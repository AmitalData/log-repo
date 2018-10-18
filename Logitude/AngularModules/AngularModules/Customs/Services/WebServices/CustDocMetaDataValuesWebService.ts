import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentMetaDataValuePM} from '../../EntityPMs/CustomsDocumentMetaDataValuePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustDocMetaDataValuesWebService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocMetaDataValuesWebService';
    }

    GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocumentFilingsIds:string ){
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');
        var params = encodeURIComponent(customsDocumentFilingsIds);
        return Observable.defer(() => {
            var x = 10;
            var custDocMetadataValue = new CustomsDocumentMetaDataValuePM(null);
            custDocMetadataValue.CustomsDocumentId = customsDocumentFilingsIds;;
            custDocMetadataValue.MetaDataTypeCode = '1';
            return this._http.post(this._apiUrl, JSON.stringify(custDocMetadataValue), { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();
                var _mappedListsArray: Array<CustomsDocumentMetaDataValuePM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomsDocumentMetaDataValuePM;
                        entity = this.MapJsonToEntityPMs(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });


    }

    GetCustomsDocumentMetaDataValuesByConnectedEntity(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var params = encodeURIComponent(entityId);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsDocumentMetaDataValuesByConnectedEntity?' + 'entityId=' + params, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();
                var _mappedListsArray: Array<CustomsDocumentMetaDataValuePM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomsDocumentMetaDataValuePM;
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
        var entityPM: CustomsDocumentMetaDataValuePM;
        entityPM = new CustomsDocumentMetaDataValuePM(null);
        var jsonListKeys = Object.keys(jsonPM);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}