import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {DocumentsFilingPM} from '../../EntityPMs/DocumentsFilingPM';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class ImageLibraryService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ImageLibrary';
    }

    DownloadFile(filename: string, documentExtension: string, fileLocation: string, tenant: number , type:string = "",tokenTenant:number = -1 ) {
        tokenTenant = tokenTenant > -1 ? tokenTenant:tenant;
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdownloadfile/?' + 'filename=' + filename + '&documentExtension=' + documentExtension + '&fileLocation=' + fileLocation + '&type=' + type + '&tenant=' + tenant+'&tokenTenant='+tokenTenant, ServiceHelper.GetHttpHeaders()).pipe(map(result => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }
    UploadFile(imageuploadFilter: any) {
        imageuploadFilter.TokenTenant = imageuploadFilter.TokenTenant > -1 ? imageuploadFilter.TokenTenant:imageuploadFilter.Tenant;
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadFile", JSON.stringify(imageuploadFilter) ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    PostImageAfterResize(imageuploadFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostImageAfterResize", JSON.stringify(imageuploadFilter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }
    UploadPdfFile(imageuploadFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadPdfFile", JSON.stringify(imageuploadFilter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }
    CancelUpload(documentId: string,  tenant:number ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getcancelupload/?' + 'documentId=' + documentId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }
    RemoveFile(documentId: string , tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getremovefile/?' + 'documentId=' + documentId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }

    GetAllParticipantsConversationHeaderMessageId(conversationHeaderId: string) {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeaderParticipantExtended';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAllParticipantsConversationHeaderMessageId/?' + 'conversationHeaderId=' + conversationHeaderId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

}

