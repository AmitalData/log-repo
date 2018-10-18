import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {CustomerProductPM} from '../../EntityPMs/CustomerProductPM';

import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class CustomerProductExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http; 

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomerProductExtended';
    }


    GetCustomerProducts(customerId: string, tenant:number){
    
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?customerId=' + customerId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: CustomerProductPM;
            var customerProductPMLists: CustomerProductPM[];
            customerProductPMLists = new Array<CustomerProductPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                customerProductPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = customerProductPMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

   
    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomerProductPM;
        entityPM = new CustomerProductPM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }




}

