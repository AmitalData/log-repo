import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { of, defer } from 'rxjs';

@Injectable()
export class CounterDefinitionPMExtendedService {
    private httpClient: HttpClient;
    private apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CounterDefinitionExtended';
    }

    GetCustomizedCounterDefinitionsByCounterId(counterId: string) {
        return defer(() => {
            return this.httpClient.get(this.apiUrl + '/GetCustomizedCounterDefinitionsByCounterId?' + 'counterId=' + counterId, ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        let serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;

                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCounterDefinitionsByCounterName(counterName: string) {
        return defer(() => {
            return this.httpClient.get(this.apiUrl + '/GetCounterDefinitionsByCounterName?' + 'counterName=' + counterName, ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        let serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;

                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
        });
    }
   

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}
