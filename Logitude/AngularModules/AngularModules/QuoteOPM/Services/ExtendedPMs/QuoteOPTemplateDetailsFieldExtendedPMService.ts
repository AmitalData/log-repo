
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

import {QuoteOPTemplateDetailsFieldPM} from '../../EntityPMs/QuoteOPTemplateDetailsFieldPM';



@Injectable()

export class QuoteOPTemplateDetailsFieldExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteOPTemplateDetailsFieldExtended';
    }







    GetQuoteTemplateDetailsFieldByQuoteTemplateId(quoteTemplateId: string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetQuoteTemplateDetailsFieldByQuoteTemplateId/?' + 'quoteTemplateId=' + quoteTemplateId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
            var entity: QuoteOPTemplateDetailsFieldPM;
            var QuoteOPTemplateDetailsFieldPMLists: QuoteOPTemplateDetailsFieldPM[];
            QuoteOPTemplateDetailsFieldPMLists = new Array<QuoteOPTemplateDetailsFieldPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                QuoteOPTemplateDetailsFieldPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = QuoteOPTemplateDetailsFieldPMLists;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }



    updateDetailsFields(quoteTemplateDetailsFields: any) {
        return defer(() => {

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutQuoteTemplateDetailsFields', JSON.stringify(quoteTemplateDetailsFields), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var pm = res;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );

    }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: QuoteOPTemplateDetailsFieldPM;
        entityPM = new QuoteOPTemplateDetailsFieldPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}
