import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
 import { ContainerizationRequestParams } from '../../DataContract/RequestParams/ContainerizationRequestParams';
import { ContainerizationPM } from 'Customs/EntityPMs/ContainerizationPM';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { ClassLevelValidator } from 'Infrastructure/Validators/ClassLevelValidator';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';
  
@Injectable()

export class ContainerizationExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ContainerizationListExtended';
    }

    public connectedSelectAll: boolean;
    public disconnectedSelectAll: boolean;
    public isNotDirty: boolean;
    public ConnectedDeclarations: string;
    public SelectedDeclarations: boolean;
    public AllDeclarations: string;
    public IsDirectCharging: string;
    public containerizationRequestParams:ContainerizationRequestParams;
    public IsError: boolean;
    public  ErrorsList:string[];
    public  countConnect:number=0;
    public Id:string
    getPromiseByFilters(filters: ApiQueryFilters) {

        return new Promise((resolve, reject) => {

            resolve(this.getByFilters(filters));

        });
    }

	CreateContainerizations(entityPM: ContainerizationPM ) {
      
		var callTime = new Date();  
		
		return defer(() => {

			var serviceResponse: ServiceResponse = new ServiceResponse();
			var validator: ClassLevelValidator = new ClassLevelValidator();                
			var errorsArray = validator.Validate("Customs.Containerization", entityPM);


			if (errorsArray.length == 0) {

				var mappedEntity: ContainerizationPM = this.MapJsonToEntityPM(entityPM, false);
				
				return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
					.pipe(
						map((response: HttpResponse<any>) => {

							var pm = response.body;
							if (pm) {
								var mappedResult: ContainerizationPM = this.MapJsonToEntityPM(pm, true, entityPM);
								serviceResponse.Result = mappedResult;
							}						

							var servertime = response.headers.get('ServerExecutionTime');
							PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Containerization", "SaveChanges", "");                    
												                             
							return serviceResponse;
						}),

						catchError(ServiceHelper.HandleServiceError));
			}

			else {
				serviceResponse.HasError = true;
				serviceResponse.ErrorsArray = errorsArray;
				return of(serviceResponse);
			}
		});

    }

    getByFilters(filters: ApiQueryFilters) {
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map((response: any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
                var _mappedListsArray: Array<DeclarationList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DeclarationList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapJsonToEntityList(jsonList: any) {

        var entityList: DeclarationList;
        entityList = new DeclarationList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ContainerizationPM = null) {

         
        if (!entityPM) {
            
            entityPM = new ContainerizationPM();
			entityPM.DisableMarkAsDirty = true;
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
	    entityPM.DisableMarkAsDirty = false;

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
