import {Injectable} from '@angular/core';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { CustomerDebtNotificationPM } from 'Accounting/EntityPMs/CustomerDebtNotificationPM';
 

@Injectable()

export class CustomerDebtNotificationExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {

        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomerDebtNotificationExtended';
    }

    GetCustomerDebtNotificationByAccountId(accountId: string) {

     return this.httpClient.get(this._apiUrl + '/GetCustomerDebtNotificationByAccountId?accountId=' + accountId,  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
                var entity: CustomerDebtNotificationPM = new CustomerDebtNotificationPM();
                if(serviceResponse.Result != null) {
                   entity = this.MapJsonToEntityPM(serviceResponse.Result);
                   serviceResponse.Result = entity;
                }             
                return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
       
    }

   
    MapJsonToEntityPM(jsonPM: any) {

        var entityList: CustomerDebtNotificationPM;
        entityList = new CustomerDebtNotificationPM();
        var jsonListKeys = Object.keys(jsonPM);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonPM[property];
        }


        return entityList;
    }

}
