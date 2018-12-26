import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
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

@Injectable()

export class BankDepositExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BankDeposit';
    }


    returnCheque(bankDepositId: string, arpChequeId: string, returnType: string, notes: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(this._apiUrl + "/PostReturnCheque?"
                + "&bankDepositId=" + bankDepositId
                + "&arpChequeId=" + arpChequeId
                + "&returnType=" + returnType
                + "&notes=" + notes
                , "",
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult: JournalPM;
                        serviceResponse.Result = pm;
                    }
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );

    }

    cancelDeposit(bankDepositId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(this._apiUrl + "/PostCancelDeposit?"
                + "&bankDepositId=" + bankDepositId
                , "",
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult: JournalPM;
                        serviceResponse.Result = pm;
                    }
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );

    }


}
