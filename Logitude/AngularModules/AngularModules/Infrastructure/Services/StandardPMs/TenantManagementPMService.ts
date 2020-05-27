import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass'

import {TenantManagementPM} from '../../EntityPMs/TenantManagementPM';

import {TenantManagementLicensePM} from '../../EntityPMs/TenantManagementLicensePM';
import {TenantAddOnPM} from '../../EntityPMs/TenantAddOnPM';
import {TenantManagementValidator} from '../../Validators/TenantManagementValidator';

@Injectable()

export class TenantManagementPMService {
 private _http: HttpClient;
 private _apiUrl: string;
 constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/tenantmanagements';      
    }

    get(id: number) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



                        var entity: TenantManagementPM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TenantManagement", "GetSinglePM", 'id=' + id);

                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: TenantManagementPM) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("TenantManagement", entityPM);
            var customValidator: TenantManagementValidator = new TenantManagementValidator();
            var validationErrorsArr = customValidator.Validate(entityPM);
            if (validationErrorsArr) {
                errorsArray = errorsArray.concat(validationErrorsArr);
            }


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TenantManagementPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {

                            var pm = response.body;
                            if (pm) {
                                var mappedResult: TenantManagementPM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }


                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TenantManagement", "SaveChanges", "");


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

    update(entityPM: TenantManagementPM) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("TenantManagement", entityPM);
            var customValidator: TenantManagementValidator = new TenantManagementValidator();
            var validationErrorsArr = customValidator.Validate(entityPM);
            if (validationErrorsArr) {
                errorsArray = errorsArray.concat(validationErrorsArr);
            }


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TenantManagementPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {


                            var pm = response.body;
                            if (pm) {
                                var mappedResult: TenantManagementPM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TenantManagement", "SaveChanges", "");

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

   

	  MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TenantManagementPM = null) {

         
        if (!entityPM) {
            
            entityPM = new TenantManagementPM();
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
			
               this.MapTenantManagementLicenses(entityPM, jsonPM, mapParent); // Call composition tables map methods
               this.MapAddOns(entityPM, jsonPM, mapParent); // Call composition tables map methods
			 
            

		if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
			   			   
            entityPM.OldEntityPM.TenantManagementLicenses = [];
            for (var item in entityPM.TenantManagementLicenses) {
            var myTenantManagementLicensePM = entityPM.TenantManagementLicenses[item];
            var newTenantManagementLicensePM: TenantManagementLicensePM = this.clone(myTenantManagementLicensePM);
						
							 
            entityPM.OldEntityPM.TenantManagementLicenses.push(newTenantManagementLicensePM);
            }
			   			   			   
            entityPM.OldEntityPM.AddOns = [];
            for (var item in entityPM.AddOns) {
            var myTenantAddOnPM = entityPM.AddOns[item];
            var newTenantAddOnPM: TenantAddOnPM = this.clone(myTenantAddOnPM);
						
							 
            entityPM.OldEntityPM.AddOns.push(newTenantAddOnPM);
            }
			   
		}
        else {

            entityPM.OldEntityPM = null;
        }
		entityPM.IsDirty = false;
        return entityPM;
    }

    MapTenantManagementLicenses(entityPM: TenantManagementPM, jsonPM: any, mapParent: boolean = true) {

        var oldTenantManagementLicenses: TenantManagementLicensePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldTenantManagementLicenses = entityPM.OldEntityPM.TenantManagementLicenses;
        }

        entityPM.TenantManagementLicenses = new Array<TenantManagementLicensePM>();
        for (var item in jsonPM.TenantManagementLicenses) {
            var jItem = jsonPM.TenantManagementLicenses[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTenantManagementLicensePM: TenantManagementLicensePM;
	  
            if (mapParent) {
                newTenantManagementLicensePM = new TenantManagementLicensePM(entityPM);
            }
            else
            {
                newTenantManagementLicensePM = new TenantManagementLicensePM(null);
            }
                
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM" )|| pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTenantManagementLicensePM[pmProperty] = jItem[pmProperty];
            }
           
			 
            if (mapParent) {
                newTenantManagementLicensePM.UniqueKey = Guid.newGuid();
                newTenantManagementLicensePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newTenantManagementLicensePM.OldEntityPM = this.clone(newTenantManagementLicensePM);

				
            }
            else {
                if (newTenantManagementLicensePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newTenantManagementLicensePM.ChangeSetOp = "Update";
                }
                else {
                        newTenantManagementLicensePM.ChangeSetOp = "Insert";
                }
 
                newTenantManagementLicensePM.OldEntityPM = null;
                newTenantManagementLicensePM.EntityParentPM = null;
            }
			
			 newTenantManagementLicensePM.IsDirty = false;
            entityPM.TenantManagementLicenses.push(newTenantManagementLicensePM);
        }
        if (oldTenantManagementLicenses) {
            
            for (var itemKey in oldTenantManagementLicenses) {
                if (entityPM.TenantManagementLicenses.filter(p=> p.UniqueKey === oldTenantManagementLicenses[itemKey].UniqueKey).length === 0) {
				
                    if (oldTenantManagementLicenses[itemKey]) {
                        //oldTenantManagementLicenses[itemKey].ChangeSetOp = "Delete";
                        //entityPM.TenantManagementLicenses.push(oldTenantManagementLicenses[itemKey]);
						var oldItemJson = oldTenantManagementLicenses[itemKey];
                        var deletedPM: TenantManagementLicensePM = new TenantManagementLicensePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }

                      
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        
                        deletedPM.OldEntityPM = null;
                        entityPM.TenantManagementLicenses.push(deletedPM);
                    }
                }
            }
        }
    }
    MapAddOns(entityPM: TenantManagementPM, jsonPM: any, mapParent: boolean = true) {

        var oldAddOns: TenantAddOnPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldAddOns = entityPM.OldEntityPM.AddOns;
        }

        entityPM.AddOns = new Array<TenantAddOnPM>();
        for (var item in jsonPM.AddOns) {
            var jItem = jsonPM.AddOns[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTenantAddOnPM: TenantAddOnPM;
	  
            if (mapParent) {
                newTenantAddOnPM = new TenantAddOnPM(entityPM);
            }
            else
            {
                newTenantAddOnPM = new TenantAddOnPM(null);
            }
                
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM" )|| pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTenantAddOnPM[pmProperty] = jItem[pmProperty];
            }
           
			 
            if (mapParent) {
                newTenantAddOnPM.UniqueKey = Guid.newGuid();
                newTenantAddOnPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newTenantAddOnPM.OldEntityPM = this.clone(newTenantAddOnPM);

				
            }
            else {
                if (newTenantAddOnPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newTenantAddOnPM.ChangeSetOp = "Update";
                }
                else {
                        newTenantAddOnPM.ChangeSetOp = "Insert";
                }
 
                newTenantAddOnPM.OldEntityPM = null;
                newTenantAddOnPM.EntityParentPM = null;
            }
			
			 newTenantAddOnPM.IsDirty = false;
            entityPM.AddOns.push(newTenantAddOnPM);
        }
        if (oldAddOns) {
            
            for (var itemKey in oldAddOns) {
                if (entityPM.AddOns.filter(p=> p.UniqueKey === oldAddOns[itemKey].UniqueKey).length === 0) {
				
                    if (oldAddOns[itemKey]) {
                        //oldAddOns[itemKey].ChangeSetOp = "Delete";
                        //entityPM.AddOns.push(oldAddOns[itemKey]);
						var oldItemJson = oldAddOns[itemKey];
                        var deletedPM: TenantAddOnPM = new TenantAddOnPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }

                      
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        
                        deletedPM.OldEntityPM = null;
                        entityPM.AddOns.push(deletedPM);
                    }
                }
            }
        }
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
		    var entityPM: TenantManagementPM;
			entityPM = new TenantManagementPM();
			return entityPM;
    }
		 

}
