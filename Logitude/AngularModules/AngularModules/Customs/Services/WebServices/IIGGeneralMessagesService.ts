import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { MorningMessageRequestParams } from '../../DataContract/RequestParams/MorningMessageRequestParams';
import { CourierBOLQueryRequestParams } from '../../DataContract/RequestParams/CourierBOLQueryRequestParams';
import { ExchangeRatesQueryRequestParams } from '../../DataContract/RequestParams/ExchangeRatesQueryRequestParams';
import { MasterBOLQueryRequestParams } from '../../DataContract/RequestParams/MasterBOLQueryRequestParams';
import { CustomItemLegalDemandsQueryRequestParams } from '../../DataContract/RequestParams/CustomItemLegalDemandsQueryRequestParams';
import { CreditQueryRequestParams } from '../../DataContract/RequestParams/CreditQueryRequestParams';
import { ImporterDeclarationRequestParams } from '../../DataContract/RequestParams/ImporterDeclarationRequestParams';
import { SystemTableRequestParams } from '../../DataContract/RequestParams/SystemTableRequestParams';
import { SpecialActivityRequestParams } from '../../DataContract/RequestParams/SpecialActivityRequestParams';
import { UpdateDeleteVehicleRequestParams } from '../../DataContract/RequestParams/UpdateDeleteVehicleRequestParams';
import { CH_NG_191_MSG2_ChangingTimeRequestParams } from '../../DataContract/RequestParams/CH_NG_191_MSG2_ChangingTimeRequestParams';
import { CargoQueryRequestParams } from '../../DataContract/RequestParams/CargoQueryRequestParams';


@Injectable()

export class IIGGeneralMessagesService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/IIGGeneralMessages';

    }

    PostMorningMessages(entity: MorningMessageRequestParams) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostMorningMessages/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    PostCourierBOLRequest(entity: CourierBOLQueryRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCourierBOLRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    PostMasterBOLRequest(entity: MasterBOLQueryRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostMasterBOLRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostCreditQueryRequest(entity: CreditQueryRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCreditQueryRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostGoldCreditQueryRequest(entity: CreditQueryRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGoldCreditQueryRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostSpecialActivityRequest(entity: SpecialActivityRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSpecialActivityRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostImporterDeclarationRequest(entity: ImporterDeclarationRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostImporterDeclarationRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetClientProgressBarIndicatorCurrentStage(tenant: number, CustomsRequestsSheetId: string, BasicResponse: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetClientProgressBarIndicatorCurrentStage/?' + '&BasicResponse=' + BasicResponse + '&tenant=' + tenant + '&CustomsRequestsSheetId=' + CustomsRequestsSheetId,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    PostExchangeRatesQuery(entity: ExchangeRatesQueryRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostExchangeRatesQuery/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    PostCustomItemLegalDemandsQuery(entity: CustomItemLegalDemandsQueryRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCustomItemLegalDemandsQuery/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    PostUpdateClosedTables(entity: SystemTableRequestParams) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostUpdateClosedTables/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostFillNotExistedClosedTables(entity: SystemTableRequestParams) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostFillNotExistedClosedTables/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }



    PostVehicleRequest(entity: UpdateDeleteVehicleRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostVehicleRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


    PostCustomFileCredit(entity: any) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCustomFileCredit/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    GetDefBankForCustomer(customerCode: string, tenant: number) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(
                this._apiUrl + '/GetDefBankForCustomer/?' + 'customerCode=' + customerCode + '&tenant=' + tenant.toString(),
                //JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    PostChangingTimeRequestParams(entity: CH_NG_191_MSG2_ChangingTimeRequestParams) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostChangingTimeRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }




    GetResetDeclarationNumber(declarationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetResetDeclarationNumber/?' + '&declarationId=' + declarationId + '&tenant=' + tenant,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse.Result = response;
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });


    }


    PostCargoQueryRequestParams(entity: CargoQueryRequestParams) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCargoQueryRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


    PostMessageRestoreRequestParams(entity: any) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostMessageRestoreRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    PostBlockListInWarehouseRequestParams(entity: any) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostBlockListInWarehouseRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }



    GetLOGISUPPACC(declarationId: string,
        InvoiceCounterKey: number, LineNumber: number  ,
        tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        //GetLOGISUPPACC(string declarationId, int InvoiceCounterKey, int LineNumber, int tenant)

        return defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetLOGISUPPACC/?' +
                '&declarationId=' + declarationId +
                '&InvoiceCounterKey=' + InvoiceCounterKey +
                '&LineNumber=' + LineNumber +
                '&tenant=' + tenant,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var myObj: any = response;
                    serviceResponse.Result =myObj.MyXML;
                    //serviceResponse.Result = (response as any)._body;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });


    }



    PostCustomsBookInRequestParams(entity: any) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCustomsBookInRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    PostDeficitFileFilterRequestParams(entity: any) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostDeficitFileFilterRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    PostTPG_NG_8244_ClaimFileFilterRequestParams(entity: any) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostTPG_NG_8244_ClaimFileFilterRequestParams/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    
}
////////////////////////////////////////////////
export class ResultClientProgressBar {
    public stopMeNow: boolean;
    public continueInBackground: boolean;
    public responseDataXml: string;
    public ProgressStage: number;

}
////////////////////////////////////////
   