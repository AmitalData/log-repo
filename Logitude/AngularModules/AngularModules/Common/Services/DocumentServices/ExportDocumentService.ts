
import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class ExportDocumentService {


    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExportDocument';
    }



    getDocumentPdfFile(documentTypeId: string, entityId: string, entityObjectTableId: string, childEntityId: string, childObjectTableId: string, documentOutId: string, tenant: number, documentTypeCopyId: string,userId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&entityObjectTableId=' + entityObjectTableId + '&childEntityId=' + childEntityId + '&childObjectTableId=' + childObjectTableId + '&documentOutId=' + documentOutId + '&tenant=' + tenant + '&documentTypeCopyId=' + documentTypeCopyId + '&userId=' + userId 
            ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }



    PostReportStimulsoftViewer(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postreportstimulsoftviewer', JSON.stringify(filter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    BuildDocumentViaWorkerRole(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/PostBuildDocumentViaWorkerRole', JSON.stringify(filter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    GetUsedSpaceForTenant( tenant: number  ){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl  + '?tenant=' + tenant 
            ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }

    
    GetResetEditableFields(documentoOutId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + "/GetResetEditableFields" + '?documentoOutId=' + documentoOutId
            ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }
    


    DownloadFileFromServer(documentId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl  + '?documentId=' + documentId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }



    GetIsRunStimulDocumentViaWorkerRole() {
 
        return this._http.get(this._apiUrl + "/GetIsRunStimulDocumentViaWorkerRole", ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


}

