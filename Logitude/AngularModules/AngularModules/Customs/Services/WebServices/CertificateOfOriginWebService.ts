import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CLAIM_2340_ClaimRequestRequestParams } from '../../DataContract/RequestParams/CLAIM_2340_ClaimRequestRequestParams';
import { ContinuousRequestOnClaimFileRequestParams } from '../../DataContract/RequestParams/ContinuousRequestOnClaimFileRequestParams';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';


@Injectable()

export class CertificateOfOriginWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CertificateOfOrigin';
    }

    PostCertificateOfOriginRequest(entity: CertificateOfOriginRequestRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCertificateOfOriginRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetCertificateOfOriginByID(declarationId: string,tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCertificateOfOriginByID/?declarationId=" + declarationId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
   
}
