import {Injectable} from '@angular/core';
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

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
  
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
     

    }

    GetSingleWithoutLines(id: string) {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        return this.httpClient.get(this._apiUrl + '/GetSingleWithoutLines?id=' + id  ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
             }),
            catchError(ServiceHelper.HandleServiceError));
    
    }


}
