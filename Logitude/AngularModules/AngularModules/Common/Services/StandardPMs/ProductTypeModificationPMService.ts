import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
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
import { ProductTypeModificationPM } from '../../EntityPMs/ProductTypeModificationPM';

@Injectable()

export class ProductTypeModificationPMService {
 private _http: HttpClient;
 private _apiUrl: string;
 constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ProductTypeModifications';      
    }

 get(code: string) {
         
         
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();		
		 return defer(() => {
                return this._http.get(this._apiUrl+'/getsingle?'+'code=' + code, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                    var pm = response.body;

                   
					
                    var entity: ProductTypeModificationPM;
					if(pm)
					{
                      entity = this.MapJsonToEntityPM(pm);
                    }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
              
			    var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ProductTypeModification", "GetSinglePM", 'code=' + code);
				 
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
            });                    
    }

	 insert(entityPM: ProductTypeModificationPM) {
 
        var callTime = new Date();        
        return defer(() => {

                var authHeader = new Headers();
                authHeader.append('Token', SessionInfo.Token);
                authHeader.append('Content-Type', 'application/json');

                var validator: ClassLevelValidator;
                 
                validator = new ClassLevelValidator();
                 
                var errorsArray = validator.Validate("ProductTypeModification", entityPM);
                 

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
				 if (errorsArray.length == 0) {
                    var mappedEntity: ProductTypeModificationPM;
                    mappedEntity = this.MapJsonToEntityPM(entityPM, false);
				
				    return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {

                            var pm = response.body;
							if(pm)
							{
                               var mappedResult:  ProductTypeModificationPM;
                               mappedResult = this.MapJsonToEntityPM(pm,true,entityPM);
							   serviceResponse.Result = mappedResult;
							}
							

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ProductTypeModification", "SaveChanges", "");                    
												 
                            
                            return serviceResponse;

                        }),catchError(ServiceHelper.HandleServiceError));
                }
                else {

                    serviceResponse.HasError = true;
                    serviceResponse.ErrorsArray = errorsArray;

                    return of(serviceResponse);
                   
                }
            });
    }

    update(entityPM: ProductTypeModificationPM) {

            var callTime = new Date();         
            return defer(() => {

                var authHeader = new Headers();
                authHeader.append('Token', SessionInfo.Token);
                authHeader.append('Content-Type', 'application/json');
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                    var mappedEntity: ProductTypeModificationPM;
                    mappedEntity = this.MapJsonToEntityPM(entityPM, false);
				
				    return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                 

                            var pm = response.body;
							if(pm)
							{
                               var mappedResult:  ProductTypeModificationPM;
                               mappedResult = this.MapJsonToEntityPM(pm,true,entityPM);
							   serviceResponse.Result = mappedResult;
							 }
							 
                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ProductTypeModification", "SaveChanges", "");                    
					                           
                            return serviceResponse;

                        }),catchError(ServiceHelper.HandleServiceError));

                    return of(serviceResponse);
                   
            });

    }

   

	  MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ProductTypeModificationPM = null) {

         
        if (!entityPM) {
            
            entityPM = new ProductTypeModificationPM();
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
            
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }


}
