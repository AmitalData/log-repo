
import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';



@Injectable()
export class HtmlEditorService {


    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/HtmlEditor';
    }


    getEditorHtmlData(docOutId: string, entityId: string, objecttableId: string, childEntityId: string, childEntityObjectTableId: string, tenant: number, userId: string, isSendMail: boolean, documentTemplateId: string, subject: string, mode: string = null, from: string = null, replyTo: string = null, cc: string = null, bcc: string = null, to:string = null) {

        var htmlEditorArgs: HtmlEditorResolveArgs = new HtmlEditorResolveArgs();
        htmlEditorArgs.DocumentOutId = docOutId;
        htmlEditorArgs.EntityId = entityId;
        htmlEditorArgs.ObjectTableId = objecttableId;
        htmlEditorArgs.ChildEntityId = childEntityId;
        htmlEditorArgs.ChildEntityObjectTableId = childEntityObjectTableId;
        htmlEditorArgs.Tenant = tenant;
        htmlEditorArgs.UserId = userId;
        htmlEditorArgs.IsSendMail = isSendMail;
        htmlEditorArgs.DocumentTemplateId = documentTemplateId;
        htmlEditorArgs.Subject = subject;
        htmlEditorArgs.Mode = mode;
        htmlEditorArgs.From = from;
        htmlEditorArgs.ReplyTo = replyTo;
        htmlEditorArgs.Cc = cc;
        htmlEditorArgs.Bcc = bcc;
        htmlEditorArgs.To = to;

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.post(this._apiUrl + '/PostGetEditorHtmlData', JSON.stringify(htmlEditorArgs), ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );


    }


    getSentMessageHtmlBody(documentId: string, tenant: number){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        return this._http.get(this._apiUrl + '?documentId=' + documentId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(result => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));

    }
    sendDocumentHtml(sendHtmlFilter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
                return this._http.post(this._apiUrl + '/postsendhtmldocument', JSON.stringify(sendHtmlFilter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
       
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                    pmresponse.Result = response;
                return pmresponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }



    saveEditedReportToServer(filters: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + '/putsaveeditedreporttoserver', JSON.stringify(filters),ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }






}


export class HtmlEditorResolveArgs{

    DocumentOutId: string;
    EntityId: string;
    ObjectTableId: string;
    ChildEntityId: string;
    ChildEntityObjectTableId: string;
    Tenant: number;
    UserId: string;
    DocumentTemplateId: string;
    Subject: string;
    From: string;
    ReplyTo: string;
    Cc: string;
    Bcc: string;
    To: string;
    Mode: string;
    ObjectTableName: string;
    HtmlString: string;
    IsSendMail: boolean;

    
}

