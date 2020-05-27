import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { AccountingIntegrityCheckPM } from '../../EntityPMs/AccountingIntegrityCheckPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
 

@Injectable()

export class AccountingEntegrityCheckExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingEntegrityCheck';
    }


    PostFixEntegrityCheckErrorInBatch(accountingEntegrityCheck: AccountingIntegrityCheckPM) {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        var mappedEntity: AccountingIntegrityCheckPM;
        mappedEntity = this.MapJsonToEntityPM(accountingEntegrityCheck, false);

        return this.httpClient.post(this._apiUrl + "/PostFixEntegrityCheckErrorInBatch", JSON.stringify(mappedEntity),  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
      }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AccountingIntegrityCheckPM = null) {


        if (!entityPM) {

            entityPM = new AccountingIntegrityCheckPM();
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
