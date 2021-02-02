import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {JournalList} from '../../EntityLists/JournalList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {JournalLinePM} from '../../EntityPMs/JournalLinePM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';

@Injectable()

export class AccountingOpService {
    

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingOp';//AccountingOpController
    }
    GetTestOperation(operationId: string, myparams:string) :any{
        var url = this._apiUrl + '/GetTestOperation?operationId=' + operationId +'&myparams='+myparams;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;
                
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    Generate1000(email: string): any {
 
        var url = this._apiUrl + '/GetGenerate1000?email=' + email;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;
                var entity: JournalPM;
                if (result) {
                    //entity = this.MapJsonToEntityPM(result);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }
    
    PutSystem1000File(fileUploadParamerter: ImageParameter) {
         
        return defer(() => {
            return this._http.put(this._apiUrl + '/PutSystem1000File', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }

    PutFunctionalTestXLS(fileUploadParamerter: ImageParameter) {
         
        return defer(() => {
            return this._http.put(this._apiUrl + '/PutFunctionalTestXLS', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }


}
