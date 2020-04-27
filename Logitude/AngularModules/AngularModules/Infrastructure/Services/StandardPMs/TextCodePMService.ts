import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {TextCodePM} from '../../EntityPMs/TextCodePM';


@Injectable()
export class TextCodePMService {

 private _apiUrl: string;
 private _http: HttpClient;
 private _serviceArgs: ServiceArgs;
 constructor() {
        
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/textcodes';
    }

    get(id: string, tenant: number) {
         
        console.log('--------------------------------------> calling getSingleEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
		
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var pm = response;
                    
					
     //               var entity: TextCodePM;
					//if(pm)
					//{
     //                entity = this.MapJsonToEntityPM(pm);
     //               }
                    return pm;
            }), catchError(ServiceHelper.HandleServiceError));
            });
       
         
    }

    getByCode(code: string, tenant: number) {

        console.log('--------------------------------------> calling getSingleEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingleByCode?' + 'code=' + code + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;


                //               var entity: TextCodePM;
                //if(pm)
                //{
                //                entity = this.MapJsonToEntityPM(pm);
                //               }
                return pm;
            }), catchError(ServiceHelper.HandleServiceError));
        });

    }

    insert(entityPM: TextCodePM) {
        console.log('--------------------------------------> calling updateEntityPM:');
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("TextCode", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TextCodePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: TextCodePM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }



                            return serviceResponse;

                        }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
        });
    }

    update(entityPM: TextCodePM) {

        console.log('--------------------------------------> calling updateEntityPM:');
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("TextCode", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TextCodePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: TextCodePM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }


                            return serviceResponse;

                        }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
        });

    }

   

	  MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: TextCodePM = null) {

         
        if (!entityPM) {
            
            entityPM = new TextCodePM();
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

		if (getCallMap) {
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


}
