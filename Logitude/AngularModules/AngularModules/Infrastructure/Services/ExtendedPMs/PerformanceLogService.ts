
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ServiceArgs } from '../../DataContracts/ServiceArgs';
import { EntityPMServiceResponse } from '../../DataContracts/EntityPMServiceResponse';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { Guid } from '../../Utilities/Guid';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { PerformanceLog } from '../../Others/PerformanceLog';
@Injectable()
export class PerformanceLogService {
    private _http: Http;
    private _apiUrl: string;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/performancelogs';

    }

    

    insert(entity: PerformanceLog) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("ErrorLog", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
             

                return this._http.post(this._apiUrl, JSON.stringify(entity),
                    { headers: authHeader }).map((res) => {
                        var result = res.json();
                        response.Result = result;
                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }


    insertLogsList(logs: PerformanceLog[]) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("ErrorLog", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {


                return this._http.post(this._apiUrl + '/PostLogsList', JSON.stringify(logs),
                    { headers: authHeader }).map((res) => {
                        var result = res.json();
                        response.Result = result;
                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }


}
