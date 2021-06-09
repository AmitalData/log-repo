import { Injectable, } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentFile } from '../../DataContracts/DocumentFile';


@Injectable()
export class DocumentFileService { 
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentFile';
    }

    insert(documentFile: DocumentFile) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
              
            return this._http.post(this._apiUrl, JSON.stringify(documentFile), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => { 
                        if (response.body) { 
                            serviceResponse.Result = response.body;
                        } 
                        return serviceResponse; 
                    }), catchError(ServiceHelper.HandleServiceError)); 
        });
    } 
     

    GetDocumentsByIdsList(documentIds: string) {
        var url = this._apiUrl + '?documentIds=' + documentIds;

            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();;  
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
  

    DeleteDocumentFile(documentId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken()); 
        return this._http.delete(this._apiUrl + '?documentId=' + documentId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse(); 
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


}
