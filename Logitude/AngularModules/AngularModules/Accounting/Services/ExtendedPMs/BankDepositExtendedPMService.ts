import {Injectable} from '@angular/core';
//import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {LedgerTransactionPM} from '../../EntityPMs/LedgerTransactionPM';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {ReconciliationLinePM} from '../../EntityPMs/ReconciliationLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class BankDepositExtendedPMService {
  //  private _http: Http;
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
       // this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BankDeposit';
    }


    returnCheque(bankDepositId: string, arpChequeId: string, returnType: string, notes: string) {
          var serviceResponse: ServiceResponse = new ServiceResponse();
         return this.httpClient.post(this._apiUrl + "/PostReturnCheque?"
        + "&bankDepositId=" + bankDepositId
        + "&arpChequeId=" + arpChequeId
        + "&returnType=" + returnType
        + "&notes=" + notes
        , null,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var pm = res;
                if (pm) {
                   // var mappedResult: JournalPM;
                    serviceResponse.Result = pm;
                }
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

           
        //     return this._http.post(this._apiUrl + "/PostReturnCheque?"
        //         + "&bankDepositId=" + bankDepositId
        //         + "&arpChequeId=" + arpChequeId
        //         + "&returnType=" + returnType
        //         + "&notes=" + notes
        //         , "",
        //         { headers: authHeader }).map((res) => {
        //             var pm = res.json();
        //             if (pm) {
        //                 var mappedResult: JournalPM;
        //                 serviceResponse.Result = pm;
        //             }
        //             return serviceResponse;

        //         }).catch(ServiceHelper.HandleServiceError);

        // }

        // );

    }

    cancelDeposit(bankDepositId: string) {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        return this.httpClient.post(this._apiUrl + "/PostCancelDeposit?"
        + "&bankDepositId=" + bankDepositId
        , null, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var pm = res;
                if (pm) {
                    var mappedResult: JournalPM;
                    serviceResponse.Result = pm;
                }
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
        // return Observable.defer(() => {

        //     var authHeader = new Headers();
        //     authHeader.append('Token', SessionInfo.Token);
        //     authHeader.append('Content-Type', 'application/json');

        //     var serviceResponse: ServiceResponse;
        //     serviceResponse = new ServiceResponse();

        //     return this._http.post(this._apiUrl + "/PostCancelDeposit?"
        //         + "&bankDepositId=" + bankDepositId
        //         , "",
        //         { headers: authHeader }).map((res) => {
        //             var pm = res.json();
        //             if (pm) {
        //                 var mappedResult: JournalPM;
        //                 serviceResponse.Result = pm;
        //             }
        //             return serviceResponse;

        //         }).catch(ServiceHelper.HandleServiceError);

        // }

        // );

    }

    GetSingleWithoutLines(id: string) {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        return this.httpClient.get(this._apiUrl + '/GetSingleWithoutLines?id=' + id  ,  ServiceHelper.GetHttpHeaders()).pipe(
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


}
