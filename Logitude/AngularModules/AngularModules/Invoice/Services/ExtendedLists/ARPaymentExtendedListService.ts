import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ARPaymentList} from '../../EntityLists/ARPaymentList';

@Injectable()

export class ARPaymentExtendedListService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ARPaymentViews';
    }
    
    GetByARPaymentNumber(paymentNumber) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetByPaymentNumber?paymentNumber=' + paymentNumber;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var list = response.json();

                var entity: ARPaymentList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: ARPaymentList;
        entityList = new ARPaymentList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }


}