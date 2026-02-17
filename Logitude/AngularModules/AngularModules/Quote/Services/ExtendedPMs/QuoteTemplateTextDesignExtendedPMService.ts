
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

import {QuoteTemplateTextDesignPM} from '../../EntityPMs/QuoteTemplateTextDesignPM';



@Injectable()

export class QuoteTemplateTextDesignExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateTextDesignExtended';
    }


    updateQuoteTemplateTextDesignPMs(quoteTemplateTextDesignPMs: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutQuoteTemplateTextDesignPMs', JSON.stringify(quoteTemplateTextDesignPMs),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }
  
  
    GetQuoteTemplateTextDesignPMListByIds(ids: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateTextDesignPMListByIds/?' + 'ids=' + ids + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: QuoteTemplateTextDesignPM;

           var quoteTemplateTextDesignPMLists = new Array<QuoteTemplateTextDesignPM>();
           if (result) {
               result.forEach((item) => {
                   entity = this.MapJsonToEntityPM(item);
                   quoteTemplateTextDesignPMLists.push(entity);
               });
           }
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = quoteTemplateTextDesignPMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: QuoteTemplateTextDesignPM;
        entityPM = new QuoteTemplateTextDesignPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }


}
