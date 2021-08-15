
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';
@Injectable()
export class QuoteOPDocumentVersionExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteOPDocumentVersionExtended';
    }

    GetQuoteDocumentVersionByQuoteId(quoteId: string) {

        return this._http.get(this._apiUrl + '/GetQuoteDocumentVersionByQuoteId/?' + 'quoteId=' + quoteId , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
}
