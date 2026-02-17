import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
@Injectable()

export class PaymentChequeService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PaymentChequeViews';
    }


    CheckIfPaymentChequeExists(bankAccountId: string,chequeNumber:string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetCheckIfPaymentChequeExists?BankAccountId=' + bankAccountId + '&ChequeNumber=' + chequeNumber;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var res = response.json();
                if (res && res != null) {
                    if (res==true) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

}