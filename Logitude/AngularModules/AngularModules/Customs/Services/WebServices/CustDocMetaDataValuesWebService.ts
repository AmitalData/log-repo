import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentMetaDataValuePM} from '../../EntityPMs/CustomsDocumentMetaDataValuePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustDocMetaDataValuesWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocMetaDataValuesWebService';
    }

    GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocumentFilingsIds:string ){
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');
        var params = encodeURIComponent(customsDocumentFilingsIds);
        return defer(() => {
            var x = 10;
            var custDocMetadataValue = new CustomsDocumentMetaDataValuePM(null);
            custDocMetadataValue.CustomsDocumentId = customsDocumentFilingsIds;;
            custDocMetadataValue.MetaDataTypeCode = '1';
            return this._http.post(this._apiUrl, JSON.stringify(custDocMetadataValue), ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
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
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    GetCustomsDocumentMetaDataValuesByConnectedEntity(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var params = encodeURIComponent(entityId);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsDocumentMetaDataValuesByConnectedEntity?' + 'entityId=' + params, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
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
            }),catchError(ServiceHelper.HandleServiceError));


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