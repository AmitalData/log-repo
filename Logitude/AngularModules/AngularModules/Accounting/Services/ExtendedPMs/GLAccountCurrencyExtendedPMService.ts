

import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';
import {GLAccountCurrencyPM} from '../../EntityPMs/GLAccountCurrencyPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class GLAccountCurrencyExtendedPMService{


    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
       
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GLAccountCurrency';
    }

    insert(entityPM: GLAccountCurrencyPM) {

        var callTime = new Date();
     

         
            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("GLAccountCurrency", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: GLAccountCurrencyPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this.httpClient.post(this._apiUrl, JSON.stringify(mappedEntity) ,  ServiceHelper.GetHttpHeaders()).pipe(
                    map(response => {
                        var pm = response;
                        if (pm) {
                            var mappedResult: GLAccountCurrencyPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }



                        return serviceResponse;

                    }),
                    catchError(ServiceHelper.HandleServiceError));
                 
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
      
    }


    
    Put(entityPM: GLAccountCurrencyPM) {

        var callTime = new Date();
 
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("GLAccountCurrency", entityPM);
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: GLAccountCurrencyPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this.httpClient.put(this._apiUrl, JSON.stringify(mappedEntity) ,  ServiceHelper.GetHttpHeaders()).pipe(
                    map(response => {
                        var pm = response;
                        if (pm) {
                            var mappedResult: GLAccountCurrencyPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        return serviceResponse;

                    }),
                    catchError(ServiceHelper.HandleServiceError));
                 
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
      
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: GLAccountCurrencyPM = null) {


        if (!entityPM) {

            entityPM = new GLAccountCurrencyPM(new GLAccountPM());
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
