import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
 import { SendCollateralRequestParams } from '../../DataContract/RequestParams/SendCollateralRequestParams';
  
@Injectable()

export class OcrDocumentExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/OcrDocumentExtended';
    }

    GetOcrDocumentByDocumentFilingId(tenant: number, docId: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetOcrDocumentByDocId/?' + 'tenant=' + tenant + '&docId=' + docId,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

   


}
