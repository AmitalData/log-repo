import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import { GuaranteeCertificateRequestParams } from '../../DataContract/RequestParams/GuaranteeCertificateRequestParams';
import { FaultProceduralRequestParams } from '../../DataContract/RequestParams/FaultProceduralRequestParams';
import { GuaranteeFileFilterRequestParams } from '../../DataContract/RequestParams/GuaranteeFileFilterRequestParams';
import { DeclarationFilterRequestParams } from '../../DataContract/RequestParams/DeclarationFilterRequestParams';
import { BankAccountToRefundRequestParams } from '../../DataContract/RequestParams/BankAccountToRefundRequestParams';
import { DepositPMService } from '../StandardPMs/DepositPMService';
import { DepositPM } from '../../EntityPMs/DepositPM';
import { DeficitPMService } from '../StandardPMs/DeficitPMService';
import { DeficitConnFileParagraphTypeListService } from '../StandardLists/DeficitConnFileParagraphTypeListService';
import { DeficitPM } from '../../EntityPMs/DeficitPM';
import { GuaranteePMService } from '../StandardPMs/GuaranteePMService';
import { GuaranteePM } from '../../EntityPMs/GuaranteePM';

@Injectable()

export class TapagMessagesService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TapagMessages';
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TapagMessages';

    }

    PostGuaranteeCertificateRequest(entity: GuaranteeCertificateRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGuaranteeCertificateRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostFaultQueryRequest(entity: FaultProceduralRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostFaultQueryRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostGuaranteeFileFilterQueryRequest(entity: GuaranteeFileFilterRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGuaranteeFileFilterQueryRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetDeclarationTapagsLists(declarationId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationTapagsLists/?declarationId=" + declarationId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var res = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetDepositPMByPaymentOrderNumberOrTapagId(paymentNumber: string, tapagId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var depositPMService: DepositPMService = new DepositPMService();

            return this._http.get(this._apiUrl + "/GetDepositPMByPaymentOrderNumberOrTapagId/?paymentNumber=" + paymentNumber + "&tapagId=" + tapagId + "&tenant=" + tenant
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var pm = response;
                    var entity: DepositPM;
                    if (pm) {
                        entity = depositPMService.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetDeficitPMByPaymentOrderNumberOrTapagId(paymentNumber: string, tapagId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var deficitPMService: DeficitPMService = new DeficitPMService();

            return this._http.get(this._apiUrl + "/GetDeficitPMByPaymentOrderNumberOrTapagId/?paymentNumber=" + paymentNumber + "&tapagId=" + tapagId + "&tenant=" + tenant
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var pm = response;
                    var entity: DeficitPM;
                    if (pm) {
                        entity = deficitPMService.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetSingleTapagList(tapagId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSingleTapagList/?id=" + tapagId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var res = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetGuaranteeByTapagId(tapagId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var guaranteePMService: GuaranteePMService = new GuaranteePMService();

            return this._http.get(this._apiUrl + "/GetGuaranteeByTapagId/?tapagId=" + tapagId + "&tenant=" + tenant
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var pm = response;
                    var entity: GuaranteePM;
                    if (pm) {
                        entity = guaranteePMService.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetDeficitConnectedFileParagraphTypeList(declarationId: string, deficitId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var deficitConnFileParagraphTypeListService: DeficitConnFileParagraphTypeListService = new DeficitConnFileParagraphTypeListService();

            return this._http.get(this._apiUrl + "/GetDeficitConnectedFileParagraphTypeList/?declarationId=" + declarationId + "&deficitId=" + deficitId + "&tenant=" + tenant
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    //var pm = response;
                    //var entity: DeficitConnFileParagraphTypeList;
                    //if (pm) {
                    //    entity = deficitConnFileParagraphTypeListService.MapJsonToEntityList(pm);
                    //}

                    //var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse.Result = entity;
                    var res = response;
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    PostDeclarationFilterRequestParams(entity: DeclarationFilterRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeclarationFilterRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostBankAccountToRefundQueryRequest(entity: BankAccountToRefundRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostBankAccountToRefundQueryRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
}