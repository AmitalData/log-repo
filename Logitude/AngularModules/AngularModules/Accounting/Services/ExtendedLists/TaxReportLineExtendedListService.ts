import { Injectable } from '@angular/core';
import { Observable, defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';

@Injectable()

export class TaxReportLineExtendedListService {
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxReportOp';
    }
    DownloadPaFile(filters: ApiQueryFilters): Observable<{ filename: string; blob: Blob }> {
        const urlparameters = '/PostDownloadPaFile?';
        const callUrl = this._apiUrl + urlparameters + this.getUrlParamsFromFilters(filters);
    
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Token': ServiceHelper.GetLoggedUserToken()
        });
    
        return this.httpClient.post(callUrl, null, {
          headers: headers,
          responseType: 'blob',
          observe: 'response'
        }).pipe(
          map((response: HttpResponse<Blob>) => {
            const filename = this.getFilenameFromResponseHeaders(response);
            return { filename: filename, blob: response.body };
          }),
          catchError(error => {
            ServiceHelper.HandleServiceError(error);
            throw error;
          })
        );
      }
    getUrlParamsFromFilters(filters: ApiQueryFilters) {
        var urlparameters = '';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        return urlparameters;
    }

    getByFilters(filters: ApiQueryFilters) {
        var urlparameters = '/GetLinesByFilters?';

        var callUrl = this._apiUrl + urlparameters + this.getUrlParamsFromFilters(filters);

        return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = response;


                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    private getFilenameFromResponseHeaders(response: HttpResponse<Blob>): string {
        const contentDispositionHeader = response.headers.get('Content-Disposition');
        if (contentDispositionHeader) {
            const matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(contentDispositionHeader);
            if (matches && matches[1]) {
                return matches[1].replace(/['"]/g, '');
            }
        }
        return 'PA.txt';
    }
    PostCreateTaxReportLinesByTextFile(fileUploadParamerter: ImageParameter) {

        return defer(() => {
            return this.httpClient.post(this._apiUrl + '/PostCreateTaxReportLines', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }), catchError(ServiceHelper.HandleServiceError));
        }
        );

    }
}
