import { Inject, Injectable } from '@angular/core';
import { defer } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';

@Injectable()
export class LogboxShipmentExportExcelService {
    private _apiUrl: string;
    constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl =
            ServiceHelper.GetAppURL(baseUrl) + 'api/LogitudeGridExportToExcel';
    }

    GetQueryToExcelData(logboxShipmentExportExcelArgs: any) {
        return defer(() => {
            return this._httpClient
                .post(
                    this._apiUrl + '/PostGetQueryToExcelData',
                    JSON.stringify(logboxShipmentExportExcelArgs),
                    ServiceHelper.GetHeadersWithToken()
                )
                .pipe(
                    map((response) => {
                        var result = response;

                        var pmresponse: ServiceResponse;
                        pmresponse = new ServiceResponse();

                        pmresponse.Result = result;
                        return pmresponse;
                    }),
                    catchError(null)
                );
        });
    }
}
