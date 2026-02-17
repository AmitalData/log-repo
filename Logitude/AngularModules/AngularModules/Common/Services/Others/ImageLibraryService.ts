import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {DocumentsFilingPM} from '../../EntityPMs/DocumentsFilingPM';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class ImageLibraryService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ImageLibrary';
    }

    DownloadFile(filename: string, documentExtension: string, fileLocation: string, tenant: number , type:string = "") {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdownloadfile/?' + 'filename=' + filename + '&documentExtension=' + documentExtension + '&fileLocation=' + fileLocation + '&type=' + type + '&tenant=' + tenant, { headers: authHeader }).map(result => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }
    UploadFile(imageuploadFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadFile", JSON.stringify(imageuploadFilter), {
        
                headers: authHeader,

            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
                }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    PostImageAfterResize(imageuploadFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostImageAfterResize", JSON.stringify(imageuploadFilter), {
                headers: authHeader,
            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }
    UploadPdfFile(imageuploadFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadPdfFile", JSON.stringify(imageuploadFilter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }
    CancelUpload(documentId: string,  tenant:number ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getcancelupload/?' + 'documentId=' + documentId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);

    }
    RemoveFile(documentId: string , tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getremovefile/?' + 'documentId=' + documentId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);

    }

    GetAllParticipantsConversationHeaderMessageId(conversationHeaderId: string) {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeaderParticipantExtended';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAllParticipantsConversationHeaderMessageId/?' + 'conversationHeaderId=' + conversationHeaderId, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

}

