import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
@Injectable()

export class PaymentChequeService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PaymentChequeViews';
    }


    CheckIfPaymentChequeExists(bankAccountId: string,chequeNumber:string) {
 

        var url = this._apiUrl + '/GetCheckIfPaymentChequeExists?BankAccountId=' + bankAccountId + '&ChequeNumber=' + chequeNumber;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var res = response;
                if (res && res != null) {
                    if (res==true) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    

}
