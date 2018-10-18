import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentPointerPM} from '../../EntityPMs/CustomsDocumentPointerPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentPointersExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentPointer';
    }

    GetCustomDocumentPointersForItems(parentEntityId: string, invCounterKey: string, itemsLineNumbers: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomDocumentPointersForItems/?' + 'parentEntityId=' + parentEntityId + '&invCounterKey=' + invCounterKey + '&itemsLineNumbers=' + itemsLineNumbers, { headers: authHeader }).map(response => {

                var allLists = response.json();
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

            }).catch(ServiceHelper.HandleServiceError);
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