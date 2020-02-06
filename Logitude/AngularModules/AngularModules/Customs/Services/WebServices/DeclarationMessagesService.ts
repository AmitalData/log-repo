import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

//request params
import {DeclarationRestoreRequestParams} from '../../DataContract/RequestParams/DeclarationRestoreRequestParams';
import { DeclarationStatusRequestParams } from '../../DataContract/RequestParams/DeclarationStatusRequestParams';
import { WarehouseBlockBalanceRequestParams } from '../../DataContract/RequestParams/WarehouseBlockBalanceRequestParams';
import { PrintRequestRequestParams } from '../../DataContract/RequestParams/PrintRequestRequestParams';
import { ConstraintApprovalRequestParams } from '../../DataContract/RequestParams/ConstraintApprovalRequestParams';
import {CollateralRequestParams} from '../../DataContract/RequestParams/CollateralRequestParams';
import {ConstraintAgentObjectionRequestParams} from '../../DataContract/RequestParams/ConstraintAgentObjectionRequestParams';
import { CustomFileCreditRequestParams } from '../../DataContract/RequestParams/CustomFileCreditRequestParams';
import { ExportDeclarationDataRequestParams } from '../../DataContract/RequestParams/ExportDeclarationDataRequestParams';
import { StorageEntranceUnloadingRequestParams } from '../../DataContract/RequestParams/StorageEntranceUnloadingRequestParams';
import {CargoSplitRequestParams} from '../../DataContract/RequestParams/CargoSplitRequestParams';
import { CargoSealsRequestParams } from '../../DataContract/RequestParams/CargoSealsRequestParams';


@Injectable()

export class DeclarationMessagesService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarartionRestore';
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationRestore';
        
    }

    PostDeclarationRequest(entity: DeclarationRestoreRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

      

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeclarationRequest/',
                JSON.stringify(entity),
                    { headers: authHeader }).map((res) => {
                  
                        serviceResponse.Result = res.json();

                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
           
        }

        );
    }

    PostDeclarationStatusRequest(entity: DeclarationStatusRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeclarationStatusRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostWarehouseBlockBalanceRequest(entity: WarehouseBlockBalanceRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostWarehouseBlockBalanceRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostPrintRequestRequest(entity: PrintRequestRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostPrintRequestRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendDeclarationConstraint(params: ConstraintApprovalRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendDeclarationConstraint/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendCollateralAnswers(params: CollateralRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendCollateralAnswers/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendDeclarationConstraintAgentObjection(params: ConstraintAgentObjectionRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendDeclarationConstraintAgentObjection/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendPaymentWithCheckCustomFileCredit(params: CustomFileCreditRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendPaymentWithCheckCustomFileCredit/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendTransferRequest(params: CustomFileCreditRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendTransferRequest/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostCheckCustomFileCreditOnly(params: CustomFileCreditRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostCheckCustomFileCreditOnly/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendPaymentOnly(params: CustomFileCreditRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendPaymentOnly/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostExportDeclarationDataRequest(entity: ExportDeclarationDataRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostExportDeclarationDataRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    PostStorageEntranceUnloadingRequest(params: StorageEntranceUnloadingRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostStorageEntranceUnloadingRequest/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    PostSendCargoSplit(params: CargoSplitRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendCargoSplit/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }


}
