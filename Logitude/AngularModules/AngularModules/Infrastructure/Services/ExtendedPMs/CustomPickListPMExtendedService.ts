import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { CustomPickListPM } from '../../EntityPMs/CustomPickListPM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class CustomPickListPMExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomPickListExtended';
    }

    GetCustomPickListsByCode(code: string, tenant: number) {
        var url = this._apiUrl + '/GetCustomPickListsByCode/?' + 'code=' + code + '&tenant=' + tenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
            var entity: CustomPickListPM;
            var CustomPickListPMLists: CustomPickListPM[];
            CustomPickListPMLists = new Array<CustomPickListPM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                CustomPickListPMLists.push(entity);
            });
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            serviceResponse.Result = CustomPickListPMLists;
            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    InsertupdateCustomPickLists(CustomPickLists: any) {
        var url = this._apiUrl + '/PutCreateUpdateCustomPickListPMs';

        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(url, JSON.stringify(CustomPickLists), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                //var pm = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });

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
