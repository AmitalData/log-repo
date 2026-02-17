import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
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
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TapagMessages';
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TapagMessages';

    }

    PostGuaranteeCertificateRequest(entity: GuaranteeCertificateRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGuaranteeCertificateRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostFaultQueryRequest(entity: FaultProceduralRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostFaultQueryRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostGuaranteeFileFilterQueryRequest(entity: GuaranteeFileFilterRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGuaranteeFileFilterQueryRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    GetDeclarationTapagsLists(declarationId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationTapagsLists/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetDepositPMByPaymentOrderNumberOrTapagId(paymentNumber: string, tapagId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var depositPMService: DepositPMService = new DepositPMService();

            return this._http.get(this._apiUrl + "/GetDepositPMByPaymentOrderNumberOrTapagId/?paymentNumber=" + paymentNumber + "&tapagId=" + tapagId + "&tenant=" + tenant
                , {
                    headers: authHeader
                }).map(response => {

                    var pm = response.json();
                    var entity: DepositPM;
                    if (pm) {
                        entity = depositPMService.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetDeficitPMByPaymentOrderNumberOrTapagId(paymentNumber: string, tapagId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var deficitPMService: DeficitPMService = new DeficitPMService();

            return this._http.get(this._apiUrl + "/GetDeficitPMByPaymentOrderNumberOrTapagId/?paymentNumber=" + paymentNumber + "&tapagId=" + tapagId + "&tenant=" + tenant
                , {
                    headers: authHeader
                }).map(response => {

                    var pm = response.json();
                    var entity: DeficitPM;
                    if (pm) {
                        entity = deficitPMService.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetSingleTapagList(tapagId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSingleTapagList/?id=" + tapagId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetGuaranteeByTapagId(tapagId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var guaranteePMService: GuaranteePMService = new GuaranteePMService();

            return this._http.get(this._apiUrl + "/GetGuaranteeByTapagId/?tapagId=" + tapagId + "&tenant=" + tenant
                , {
                    headers: authHeader
                }).map(response => {

                    var pm = response.json();
                    var entity: GuaranteePM;
                    if (pm) {
                        entity = guaranteePMService.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetDeficitConnectedFileParagraphTypeList(declarationId: string, deficitId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var deficitConnFileParagraphTypeListService: DeficitConnFileParagraphTypeListService = new DeficitConnFileParagraphTypeListService();

            return this._http.get(this._apiUrl + "/GetDeficitConnectedFileParagraphTypeList/?declarationId=" + declarationId + "&deficitId=" + deficitId + "&tenant=" + tenant
                , {
                    headers: authHeader
                }).map(response => {

                    //var pm = response.json();
                    //var entity: DeficitConnFileParagraphTypeList;
                    //if (pm) {
                    //    entity = deficitConnFileParagraphTypeListService.MapJsonToEntityList(pm);
                    //}

                    //var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse.Result = entity;
                    var res = response.json();
                    serviceResponse.Result = res;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    PostDeclarationFilterRequestParams(entity: DeclarationFilterRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeclarationFilterRequestParams/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostBankAccountToRefundQueryRequest(entity: BankAccountToRefundRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostBankAccountToRefundQueryRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }
}