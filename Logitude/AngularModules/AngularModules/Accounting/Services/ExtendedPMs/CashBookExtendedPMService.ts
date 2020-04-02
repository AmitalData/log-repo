import {Injectable} from '@angular/core';
//import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {CashBookPM} from '../../EntityPMs/CashBookPM';
import {CashBookLinePM} from '../../EntityPMs/CashBookLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 

@Injectable()
export class CashBookExtendedPMService {
  //  private _http: Http;
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
       // this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CashBookOp';
    }

    GetSingleWithoutLines(id: string) {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
       return this.httpClient.get(this._apiUrl + '/GetSingleWithoutLines?id=' + id ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

        //     var serviceResponse: ServiceResponse;
        //     serviceResponse = new ServiceResponse();

        //     return this._http.get(this._apiUrl + '/GetSingleWithoutLines?id=' + id , { headers: authHeader })
        //         .map((res) => {
        //             serviceResponse.Result = res.json();
        //             return serviceResponse;
        //         })
        //             .catch(ServiceHelper.HandleServiceError);
        //     });
    }


    GetCashbookChequesCounter(cashbookId: string) {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        return this.httpClient.get(this._apiUrl + '/GetCashbookChequesCounter?cashbookId=' + cashbookId ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

        //     var serviceResponse: ServiceResponse;
        //     serviceResponse = new ServiceResponse();

        //     return this._http.get(this._apiUrl + '/GetCashbookChequesCounter?cashbookId=' + cashbookId , { headers: authHeader })
        //         .map((res) => {
        //             serviceResponse.Result = res.json();
        //             return serviceResponse;
        //         })
        //             .catch(ServiceHelper.HandleServiceError);
        //     });
    }

    GetCashbookUndepositedChequesCount(cashbookId: string) {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        return this.httpClient.get(this._apiUrl + '/GetCashbookUndepositedChequesCount?cashbookId=' + cashbookId  ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

        //     var serviceResponse: ServiceResponse;
        //     serviceResponse = new ServiceResponse();

        //     return this._http.get(this._apiUrl + '/GetCashbookUndepositedChequesCount?cashbookId=' + cashbookId , { headers: authHeader })
        //         .map((res) => {
        //             serviceResponse.Result = res.json();
        //             return serviceResponse;
        //         })
        //             .catch(ServiceHelper.HandleServiceError);
        //     });
    }

    GetCashbookTotalAmount(cashbookId: string, chequeFilterType: string) {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        return this.httpClient.get(this._apiUrl + '/GetCashbookTotalAmount?cashbookId=' + cashbookId + '&chequeFilterType=' + chequeFilterType, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

        //     var serviceResponse: ServiceResponse;
        //     serviceResponse = new ServiceResponse();

        //     return this._http.get(this._apiUrl + '/GetCashbookTotalAmount?cashbookId=' + cashbookId + '&chequeFilterType=' + chequeFilterType,
        //      { headers: authHeader })
        //         .map((res) => {
        //             serviceResponse.Result = res.json();
        //             return serviceResponse;
        //         })
        //             .catch(ServiceHelper.HandleServiceError);
        //     });
    }


    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

}
