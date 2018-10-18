import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class CommunicationLogStepListService {



    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogStep';
    }



    getCommunicationLogStepsListsByLogId(logId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '?logId=' + logId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    


    getCommunicationLogStepsRequestParamResponseData(mainInterfaceCode: string, logId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            this._apiUrl + '/GetCommunicationLogStepsRequestParamResponseData/' + '?mainInterfaceCode=' + mainInterfaceCode + '&logId=' + logId + '&tenant=' + tenant,
            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetExportExcelByLogId(mainInterfaceCode: string, logId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            this._apiUrl + '/GetExportExcelByLogId/' + '?mainInterfaceCode=' + mainInterfaceCode + '&logId=' + logId + '&tenant=' + tenant,
            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    GetExportExcelByRequestId(mainInterfaceCode: string, requestId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            this._apiUrl + '/GetExportExcelByRequestId/' + '?mainInterfaceCode=' + mainInterfaceCode + '&requestId=' + requestId + '&tenant=' + tenant,
            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    GetCommunicationLogStepsDocumentDataBystringStepFilter(mainInterfaceCode: string, communicationLogId: string, tenant: number, stepFilter: Array<number>, suppressHugeData?: boolean) {
        var authHeader = new Headers();
        var $stepFilter = "";
        for (let a in stepFilter) {
            if ($stepFilter) {
                $stepFilter += ",";
            }
            $stepFilter += stepFilter[a];
        }
        var suppressHugeDataValue: boolean = false;
        if (suppressHugeData) {
            suppressHugeDataValue = true;
        }
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            //GetCommunicationLogStepsDocumentDataBystringStepFilter(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter)
            this._apiUrl + '/GetCommunicationLogStepsDocumentDataBystringStepFilter/' + '?mainInterfaceCode=' + mainInterfaceCode + '&communicationLogId=' + communicationLogId + '&tenant=' + tenant + '&stringStepFilter=' + $stepFilter + "&suppressHugeData=" + suppressHugeDataValue,

            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }




    GetRequestComminicationIdByEntityId2(

        tenant: number, InterfaceTypeCode: string, RequestStatusCode: string, ObjectTableId2: string, EntityId2: string
    ) {
        var authHeader = new Headers();


        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(

            this._apiUrl + '/GetRequestComminicationIdByEntityId2/' +
            //tenant: number,                 InterfaceTypeCode: string,                  RequestStatusCode: string,                  ObjectTableId2: string,               EntityId2: string
            '?tenant=' + tenant.toString() + '&InterfaceTypeCode=' + InterfaceTypeCode + '&RequestStatusCode=' + RequestStatusCode + '&ObjectTableId2=' + ObjectTableId2 + '&EntityId2=' + encodeURIComponent(EntityId2),

            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }









}

