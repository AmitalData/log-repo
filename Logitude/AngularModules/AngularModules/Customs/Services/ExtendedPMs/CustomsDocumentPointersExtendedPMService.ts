import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentPointerPM} from '../../EntityPMs/CustomsDocumentPointerPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentPointersExtendedPMService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentPointer';
    }

    GetCustomDocumentPointersForItems(parentEntityId: string, invCounterKey: string, itemsLineNumbers: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomDocumentPointersForItems/?' + 'parentEntityId=' + parentEntityId + '&invCounterKey=' + invCounterKey + '&itemsLineNumbers=' + itemsLineNumbers, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<CustomsDocumentPointerPM> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: CustomsDocumentPointerPM;
                        entity = this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsDocumentPointerPM;
        entityPM = new CustomsDocumentPointerPM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}