import { Injectable } from '@angular/core';
import { HttpClient, HttpEventType, HttpHeaders, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Observable, defer, of } from 'rxjs';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass'

import { SatisfactionSurveyPM } from '../../EntityPMs/SatisfactionSurveyPM';


@Injectable()

export class SatisfactionSurveyService {
	private _http: HttpClient;
	private _apiUrl: string;
	constructor() {
		this._http = ServiceHelper.HttpClient;
		this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SatisfactionSurveysWebService';
	}

	private GetHttpFullHeaders() {
		const httpOptions: { headers; observe; } = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
			observe: 'response'
		};

		return httpOptions;
	}

	// insert(entityPM: SatisfactionSurveyPM) {
	// 	var callTime = new Date();
	// 	return defer(() => {
	// 		var serviceResponse: ServiceResponse = new ServiceResponse();
	// 		var validator: ClassLevelValidator = new ClassLevelValidator();
	// 		var mappedEntity: SatisfactionSurveyPM = this.MapJsonToEntityPM(entityPM, false);
	// 		return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), this.GetHttpFullHeaders())
	// 			.pipe(
	// 				map((response: HttpResponse<any>) => {
	// 					var pm = response.body;
	// 					if (pm) {
	// 						var mappedResult: SatisfactionSurveyPM = this.MapJsonToEntityPM(pm, true, entityPM);
	// 						serviceResponse.Result = mappedResult;
	// 					}

	// 					var servertime = response.headers.get('ServerExecutionTime');
	// 					PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "SatisfactionSurvey", "SaveChanges", "");

	// 					return serviceResponse;
	// 				}),

	// 				catchError(ServiceHelper.HandleServiceError));
	// 	});
	// }



	insert(entityPM: SatisfactionSurveyPM) {
		var callTime = new Date();
		return defer(() => {
			var serviceResponse: ServiceResponse = new ServiceResponse();
			var validator: ClassLevelValidator = new ClassLevelValidator();
			var mappedEntity: SatisfactionSurveyPM = this.MapJsonToEntityPM(entityPM, false);
			return this._http.post<ServiceResponse>(this._apiUrl, JSON.stringify(mappedEntity), this.GetHttpFullHeaders())
				.pipe(
					map(event => {
						if (event.type === HttpEventType.Response) {
							return event.body; 
						}
						throw new Error('Unexpected event type');
					})
				);
		});
	}

	MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: SatisfactionSurveyPM = null) {
		if (!entityPM) {

			entityPM = new SatisfactionSurveyPM();
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

	public GetNewEntityPM() {
		var entityPM: SatisfactionSurveyPM;
		entityPM = new SatisfactionSurveyPM();
		entityPM.Tenant = InfraSettings.TenantPM.Id;
		return entityPM;
	}


}
