
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {PortPM} from '../../EntityPMs/PortPM';

import {PortValidator} from '../../Validators/PortValidator';

@Injectable()

export class PortExtendedPMService {
 private _http: HttpClient;
 private _apiUrl: string;
 constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PortExtended';      
    }

 getSinglePort(Code: string, CountryCode: string, Tenant: number) {
         
         
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
		
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSinglePortPMByCodeCountryCode?' + 'Code=' + Code + '&CountryCode=' + CountryCode + '&tenant=' + Tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var pm = response;
                    
					
                    var entity: PortPM;
					if(pm)
					{
                      entity = this.MapJsonToEntityPM(pm);
                    }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
            });                    
    }

 
	  MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: PortPM = null) {

         
        if (!entityPM) {
            
            entityPM = new PortPM();
        }
           
            var jsonPMKeys = Object.keys(jsonPM);

            for (var key in jsonPMKeys) {
			 if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];
            }
			
			 
            entityPM.IsDirty = false;

		if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);

		}
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }


	  public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

	  public GetNewEntityPM() {		 
		    var entityPM: PortPM;
			entityPM = new PortPM();
			entityPM.Tenant = InfraSettings.TenantPM.Id;
			return entityPM;
    }
		 

}
