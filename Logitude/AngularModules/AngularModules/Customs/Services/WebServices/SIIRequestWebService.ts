import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { CertificateOfOriginInvoicePM } from 'Customs/EntityPMs/CertificateOfOriginInvoicePM';
import { CertificateOfOriginItemPM } from 'Customs/EntityPMs/CertificateOfOriginItemPM';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { ClassLevelValidator } from 'Infrastructure/Validators/ClassLevelValidator';
import { PrivateLabelsBrandingDataService } from 'Infrastructure/Services/WebServices/PrivateLabelsBrandingDataService';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';


@Injectable()

export class SIIRequestWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CertificateOfOrigin';
    }

    getRequestsByDeclarationId(declarationId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/getRequestsByDeclarationId/?declarationId=" + declarationId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                var mappedResult: SIIRequestPM = new SIIRequestPM();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


}
