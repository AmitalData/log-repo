
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {QuoteTemplateTextCodePM} from '../../EntityPMs/QuoteTemplateTextCodePM';



@Injectable()

export class QuoteTemplateTextCodeExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateTextCodeExtended';
    }



 



    GetQuoteTemplateTextCodeByQuoteTemplateId(quoteTemplateId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateTextCodeByQuoteTemplateId/?' + 'quoteTemplateId=' + quoteTemplateId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: QuoteTemplateTextCodePM;
            var quoteTemplateTextCodePMLists: QuoteTemplateTextCodePM[];
            quoteTemplateTextCodePMLists = new Array<QuoteTemplateTextCodePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                quoteTemplateTextCodePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = quoteTemplateTextCodePMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    updateTextCodes(quoteTemplateTextCodes: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutQuoteTemplateTextCodes', JSON.stringify(quoteTemplateTextCodes),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: QuoteTemplateTextCodePM;
        entityPM = new QuoteTemplateTextCodePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}
