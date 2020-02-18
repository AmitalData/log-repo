import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { LedgerTransactionList } from '../../EntityLists/LedgerTransactionList';

@Injectable()

export class InterestTransactionExtendedListService {
    private _http: Http
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InterestTransactionViews';
    }


    GetAllInterestTransactionByDate(ReportId: string, InterestCalculationDate: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + "/GetAllInterestTransactionByDate?ReportId=" + ReportId + "&InterestCalculationDate=" + InterestCalculationDate;
        return Observable.defer(() => {
            return this._http.get(url, {
                headers: authHeader
            }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse = new ServiceResponse();;
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}
