
import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class ExportDocumentService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExportDocument';
    }



    getDocumentPdfFile(documentTypeId: string, entityId: string, entityObjectTableId: string, childEntityId: string, childObjectTableId: string, documentOutId: string, tenant: number, documentTypeCopyId: string,userId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&entityObjectTableId=' + entityObjectTableId + '&childEntityId=' + childEntityId + '&childObjectTableId=' + childObjectTableId + '&documentOutId=' + documentOutId + '&tenant=' + tenant + '&documentTypeCopyId=' + documentTypeCopyId + '&userId=' + userId 
            , {
                headers: authHeader,

            })
            .map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }



    PostReportStimulsoftViewer(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postreportstimulsoftviewer', JSON.stringify(filter), {
                headers: authHeader,

            }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    BuildDocumentViaWorkerRole(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/PostBuildDocumentViaWorkerRole', JSON.stringify(filter), {
                headers: authHeader,

            }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    GetUsedSpaceForTenant( tenant: number  ){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl  + '?tenant=' + tenant 
            , {
                headers: authHeader,

            }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }

    
    GetResetEditableFields(documentoOutId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + "/GetResetEditableFields" + '?documentoOutId=' + documentoOutId
            , {
                headers: authHeader,

            }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }
    


    DownloadFileFromServer(documentId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl  + '?documentId=' + documentId + '&tenant=' + tenant, {
            headers: authHeader,

        }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);

    }

}

