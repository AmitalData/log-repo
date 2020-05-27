import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
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
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarartionRestore';
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationRestore';
        
    }

    PostDeclarationRequest(entity: DeclarationRestoreRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

      

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeclarationRequest/',
                JSON.stringify(entity),
                    ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                  
                        serviceResponse.Result = res;

                        return serviceResponse;

                    }),catchError(ServiceHelper.HandleServiceError));
           
        }

        );
    }

    PostDeclarationStatusRequest(entity: DeclarationStatusRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeclarationStatusRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostWarehouseBlockBalanceRequest(entity: WarehouseBlockBalanceRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostWarehouseBlockBalanceRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostPrintRequestRequest(entity: PrintRequestRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostPrintRequestRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSendDeclarationConstraint(params: ConstraintApprovalRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendDeclarationConstraint/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSendCollateralAnswers(params: CollateralRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendCollateralAnswers/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSendDeclarationConstraintAgentObjection(params: ConstraintAgentObjectionRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendDeclarationConstraintAgentObjection/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSendPaymentWithCheckCustomFileCredit(params: CustomFileCreditRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendPaymentWithCheckCustomFileCredit/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSendTransferRequest(params: CustomFileCreditRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendTransferRequest/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostCheckCustomFileCreditOnly(params: CustomFileCreditRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostCheckCustomFileCreditOnly/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSendPaymentOnly(params: CustomFileCreditRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendPaymentOnly/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostExportDeclarationDataRequest(entity: ExportDeclarationDataRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostExportDeclarationDataRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    PostStorageEntranceUnloadingRequest(params: StorageEntranceUnloadingRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostStorageEntranceUnloadingRequest/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    PostSendCargoSplit(params: CargoSplitRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';

            return this._http.post(
                this._apiUrl + '/PostSendCargoSplit/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


}
