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

import {UserLastSettingsPM} from '../../EntityPMs/UserLastSettingsPM';


@Injectable()

export class UserLastSettingsExtendedPMService {
 private _http: Http;
 private _apiUrl: string;
 constructor() {
        this._http = ServiceHelper.Http;
     this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/userlastsettingsextended';      
    }

  
    getsingleByUserIdNameSpace(UserId: string, NameSpace: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getbyuseridnamespace?' + 'userid=' + UserId + '&regionnamespace=' + NameSpace, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                var entity: UserLastSettingsPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                var servertime = response.headers.get('ServerExecutionTime');
               // PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "UserLastSettings", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getallByUserIdNameSpace(UserId: string, NameSpace: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getallByUserIdNameSpace?' + 'userid=' + UserId + '&regionnamespace=' + NameSpace, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                //var entity: UserLastSettingsPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;

                var servertime = response.headers.get('ServerExecutionTime');
                // PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "UserLastSettings", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
 
	  MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: UserLastSettingsPM = null) {

         
        if (!entityPM) {
            
            entityPM = new UserLastSettingsPM();
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
				
			  if(customFields.indexOf(property) > -1)
                {
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

	  public GetNewEntityPM() {		 
		    var entityPM: UserLastSettingsPM;
			entityPM = new UserLastSettingsPM();
			entityPM.Tenant = InfraSettings.TenantPM.Id;
			return entityPM;
    }
		 

}
