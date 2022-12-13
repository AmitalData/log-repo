import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { defer } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

@Injectable()
export class ChartOfAccountsTypesExtendedPMService {
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl =
            ServiceHelper.GetLogitudeURL() +
            'api/ChartOfAccountsTypesExtended';
    }

    UpdateChartOfAccountsTypesOrder(orders: number[]) {
        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this.httpClient
                .post(
                    this._apiUrl + '/UpdateChartOfAccountsTypesOrder',
                    JSON.stringify(orders),
                    ServiceHelper.GetHttpHeaders()
                )
                .pipe(
                    map((res) => {
                        if (res) {
                            serviceResponse.Result = res;
                        }
                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError)
                );
        });
    }
}

export class CreateJournalReconcileAdjustBankFeeM {
    LedgerTransactionIds: string[];
    ReconcileExternalPageLineIdList: string[];
}
