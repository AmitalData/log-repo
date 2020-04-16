import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {CustomerProductPM} from '../../EntityPMs/CustomerProductPM';

import { defer, of } from 'rxjs';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class CustomerProductExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient; 

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomerProductExtended';
    }


    GetCustomerProducts(customerId: string, tenant:number){
    
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?customerId=' + customerId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
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
        }),catchError(ServiceHelper.HandleServiceError));
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

