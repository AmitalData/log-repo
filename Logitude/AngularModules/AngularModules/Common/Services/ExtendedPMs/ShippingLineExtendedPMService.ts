import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ShippingLinePM } from '../../EntityPMs/ShippingLinePM';

@Injectable()

export class ShippingLineExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/shippinglineextended';
    }

    GetShippingLinesForTenant(tenant: number) {
        var url = this._apiUrl + '/getShippingLinesForTenant/?tenant=' + tenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result: any = response;
            var entity: ShippingLinePM;
            var shippingLineLists: ShippingLinePM[];
            shippingLineLists = new Array<ShippingLinePM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                shippingLineLists.push(entity);
            });

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = shippingLineLists;
            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    Update(shippingLines: ShippingLinePM[]) {
        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl, JSON.stringify(shippingLines), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any) {
        var entityPM: ShippingLinePM = new ShippingLinePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    }
}
