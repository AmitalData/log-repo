/// <reference path="../../../infrastructure/datacontracts/automationargs.ts" />

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
import {AutomationResultEmailRecipientPM} from '../../EntityPMs/AutomationResultEmailRecipientPM';
import {AutomationArgs} from '../../../Infrastructure/DataContracts/AutomationArgs'

@Injectable()
export class AutomationResultEmailRecipientExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AutomationResultEmailRecipientExtended';
    }


    getAutomationResultEmailRecipientByAutomationId(automationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getAutomationResultEmailRecipientByAutomationId" + '?automationId=' + automationId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: AutomationResultEmailRecipientPM;
            var resultEmailRecipientPMLists: AutomationResultEmailRecipientPM[];
            resultEmailRecipientPMLists = new Array<AutomationResultEmailRecipientPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                resultEmailRecipientPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = resultEmailRecipientPMLists;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }






    update(items: AutomationArgs[]) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
    

            //var resultEmailRecipientPMLists: AutomationResultEmailRecipientPM[] = [];
            //if (items) {
            //    items.forEach((item) => {
            //        var mappedEntity = this.MapJsonToEntityPM(item);
            //        resultEmailRecipientPMLists.push(mappedEntity);
            //    });
            //}
            return this._http.put(this._apiUrl + '/putautomationresultemailrecipient', JSON.stringify(items),
                { headers: authHeader }).map((res) => {
                    var result = res.json();

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = result;
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

    

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AutomationResultEmailRecipientPM = null) {


        if (!entityPM) {

            entityPM = new AutomationResultEmailRecipientPM();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }




        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

}

