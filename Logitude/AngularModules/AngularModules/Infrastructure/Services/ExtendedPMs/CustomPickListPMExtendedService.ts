
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {CustomPickListPM} from '../../EntityPMs/CustomPickListPM';



@Injectable()

export class CustomPickListPMExtendedService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomPickListExtended';
    }







    GetCustomPickListsByCode(code: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCustomPickListsByCode/?' + 'code=' + code + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: CustomPickListPM;
            var CustomPickListPMLists: CustomPickListPM[];
            CustomPickListPMLists = new Array<CustomPickListPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                CustomPickListPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = CustomPickListPMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    InsertupdateCustomPickLists(CustomPickLists: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutCreateUpdateCustomPickListPMs', JSON.stringify(CustomPickLists),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomPickListPM;
        entityPM = new CustomPickListPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}
