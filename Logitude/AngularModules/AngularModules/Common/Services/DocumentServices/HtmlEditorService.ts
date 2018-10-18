
import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';



@Injectable()
export class HtmlEditorService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/HtmlEditor';
    }


    getEditorHtmlData(docOutId: string, entityId: string, objecttableId: string, childEntityId: string, childEntityObjectTableId: string, tenant: number, userId: string, theIsSendMail: boolean, documentTemplateId: string, subject: string, mode: string = null, from: string = null, replyTo:string=null,cc:string=null) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl + '?docOutId=' + docOutId + '&entityId=' + entityId + '&objecttableId=' + objecttableId + '&childEntityId=' + childEntityId + '&childEntityObjectTableId=' + childEntityObjectTableId + '&tenant=' + tenant + '&userId=' + userId + '&theIsSendMail=' + theIsSendMail + '&documentTemplateId=' + documentTemplateId + '&subject=' + subject + "&mode=" + mode + "&from=" + from + "&replyTo=" + replyTo + "&cc=" + cc
            , {
                headers: authHeader,

            })
            .map(result => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);

    }


    getSentMessageHtmlBody(documentId: string, tenant: number){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl + '?documentId=' + documentId + '&tenant=' + tenant, { headers: authHeader, }).map(result => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);

    }
    sendDocumentHtml(sendHtmlFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
                return this._http.post(this._apiUrl + '/postsendhtmldocument', JSON.stringify(sendHtmlFilter), {
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



    saveEditedReportToServer(filters: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/putsaveeditedreporttoserver', JSON.stringify(filters), {

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






}

