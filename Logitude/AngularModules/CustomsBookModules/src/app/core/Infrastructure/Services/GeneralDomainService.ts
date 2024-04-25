import { PickListGeneralEntitiesArgs } from '../DataContracts/PickListGeneralEntitiesArgs';
import { EntityPMServiceResponse } from '../DataContracts/EntityPMServiceResponse';
import { ObjectFieldValidationPM } from '../EntityPMs/ObjectFieldValidationPM';
import { PropertyChangedArgs } from '../EventEmitterArgs/PropertyChangedArgs';
import { ScreenLayoutArgs } from '../DataContracts/ScreenLayoutArgs';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { Injectable, Output, EventEmitter } from '@angular/core';
import { CustomPickListPM } from '../EntityPMs/CustomPickListPM';
import { ObjectFieldPM } from '../EntityPMs/ObjectFieldPM';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Guid } from '../Utilities/Guid';
import { defer, of } from 'rxjs';

@Injectable()
export class GeneralDomainService {
	private _apiUrl: string;
	private _http: HttpClient;
	constructor() {
		this._http = ServiceHelper.HttpClient;
		this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GeneralDomain';
	}

	GetTranslationsByParam(typeCode: string, tableId: string, translationLanguageCode: string) {
		var url =
			this._apiUrl +
			'/GetTranslationsByParam?typeCode=' +
			typeCode +
			'&tableId=' +
			tableId +
			'&translationLanguageCode=' +
			translationLanguageCode;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					var listMapped: Array<FieldsTranslations> = [];

					for (var itemJeson in listJason) {
						var itemMapped: FieldsTranslations = this.MapFieldsTranslations(listJason[itemJeson]);
						listMapped.push(itemMapped);
					}

					var serviceResponse = new ServiceResponse();
					serviceResponse.Result = listMapped;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetStandardFieldsByTableId(tableId: string) {
		var url = this._apiUrl + '/GetStandardFieldsByTableId?tableId=' + tableId;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					//var listMapped: Array<ObjectFieldPM> = [];

					//for (var itemJeson in listJason) {
					//    var itemMapped: ObjectFieldPM = this.MapJsonToObjectFieldPM(listJason[itemJeson]);
					//    listMapped.push(itemMapped);
					//}

					var serviceResponse = new ServiceResponse();
					serviceResponse.Result = listJason;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetCustomFieldsByTableId(tableId: string) {
		var url = this._apiUrl + '/GetCustomFieldsByTableId?tableId=' + tableId;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;

					var myResponse = new ServiceResponse();
					myResponse.Result = listJason;
					return myResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetFieldDataTypes() {
		var url = this._apiUrl + '/GetFieldDataTypes';

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;

					var myResponse = new ServiceResponse();
					myResponse.Result = listJason;
					return myResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetTranslationsList(typeCode: string) {
		var url = this._apiUrl + '/GetTranslationsList?typeCode=' + typeCode;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					var listMapped: Array<FieldsTranslations> = [];

					for (var itemJeson in listJason) {
						var itemMapped: FieldsTranslations = this.MapFieldsTranslations(listJason[itemJeson]);
						listMapped.push(itemMapped);
					}

					var serviceResponse = new ServiceResponse();
					serviceResponse.Result = listMapped;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	UpdateFieldsTranslations(entity: FieldsUpdateHelper) {
		return defer(() => {
			var mappedEntity: FieldsUpdateHelper = this.MapJsonToFieldsUpdateHelper(entity, false);

			return this._http
				.put(this._apiUrl + '/PutFieldsTranslations', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders())
				.pipe(
					map((response) => {
						var myJsonResult = response;

						var mappedResult: FieldsUpdateHelper = this.MapJsonToFieldsUpdateHelper(myJsonResult, true, entity);

						var myResponse = new ServiceResponse();
						myResponse.Result = mappedResult;
						return myResponse;
					}),
					catchError(ServiceHelper.HandleServiceError),
				);
		});
	}

	GetTextCodeTypes() {
		var url = this._apiUrl + '/GetTextCodeTypes';

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					var listMapped: Array<TextCodeType> = [];

					for (var itemJeson in listJason) {
						var itemMapped: TextCodeType = this.MapTextCodeType(listJason[itemJeson]);
						listMapped.push(itemMapped);
					}

					var serviceResponse = new ServiceResponse();
					serviceResponse.Result = listMapped;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	LoadAllFieldsTranslations(translationLanguageCode: string, objectTableId: string, textCodeType: string) {
		var url =
			this._apiUrl +
			'/GetAllFieldsTranslations?language=' +
			translationLanguageCode +
			'&objectTableId=' +
			objectTableId +
			'&textCodeType=' +
			textCodeType;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					var listMapped: Array<FieldsTranslations> = [];

					for (var itemJeson in listJason) {
						var itemMapped: FieldsTranslations = this.MapFieldsTranslations(listJason[itemJeson]);
						listMapped.push(itemMapped);
					}

					var serviceResponse = new ServiceResponse();
					serviceResponse.Result = listMapped;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetCustomPickListsByCode(Code: string) {
		var url = this._apiUrl + '/GetCustomPickListsByCode?code=' + Code;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					var listMapped: Array<CustomPickListPM> = [];

					for (var itemJeson in listJason) {
						var itemMapped: CustomPickListPM = this.MapFieldsPickList(listJason[itemJeson]);
						listMapped.push(itemMapped);
					}

					//var serviceResponse = new ServiceResponse();
					//serviceResponse.Result = listMapped;
					var myResponse = new ServiceResponse();
					myResponse.Result = listMapped;
					return myResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	insertPickListGeneralEntities(entities: PickListGeneralEntitiesArgs) {
		return defer(() => {
			var entityPMServiceResponse: EntityPMServiceResponse;
			entityPMServiceResponse = new EntityPMServiceResponse();

			var mappedEntity: PickListGeneralEntitiesArgs;
			mappedEntity = this.MapJsonToEntityPM(entities, false);

			return this._http
				.post(this._apiUrl + '/PostPickList', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders())
				.pipe(
					map((response) => {
						var pm = response;
						if (pm) {
							//var mappedResult: GeneralEntitiesArgs;
							//mappedResult = this.MapJsonToEntityPM(pm, true, entities);
							entityPMServiceResponse.Result = pm;
						}
						return entityPMServiceResponse;
					}),
					catchError(ServiceHelper.HandleServiceError),
				);
		});
	}

	updatePickListGeneralEntities(entities: PickListGeneralEntitiesArgs) {
		return defer(() => {
			var entityPMServiceResponse: EntityPMServiceResponse;
			entityPMServiceResponse = new EntityPMServiceResponse();

			var mappedEntity: PickListGeneralEntitiesArgs;
			mappedEntity = this.MapJsonToEntityPM(entities, false);

			return this._http
				.put(this._apiUrl + '/PutPickList', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders())
				.pipe(
					map((response) => {
						var pm = response;
						if (pm) {
							//var mappedResult: PickListGeneralEntitiesArgs;
							//mappedResult = this.MapJsonToEntityPM(pm, true, entities);
							entityPMServiceResponse.Result = pm;
						}

						return entityPMServiceResponse;
					}),
					catchError(ServiceHelper.HandleServiceError),
				);
		});
	}

	updateScreenFields(entities: ScreenLayoutArgs) {
		return defer(() => {
			var entityPMServiceResponse: EntityPMServiceResponse;
			entityPMServiceResponse = new EntityPMServiceResponse();

			var mappedEntity: ScreenLayoutArgs;
			mappedEntity = this.MapJsonToScreenFieldsPM(entities, false);

			return this._http
				.put(this._apiUrl + '/PutScreenFields', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders())
				.pipe(
					map((response) => {
						var pm = response;
						if (pm) {
							//var mappedResult: PickListGeneralEntitiesArgs;
							//mappedResult = this.MapJsonToEntityPM(pm, true, entities);
							entityPMServiceResponse.Result = pm;
						}

						return entityPMServiceResponse;
					}),
					catchError(ServiceHelper.HandleServiceError),
				);
		});
	}

	GetScreenModificationByScreenCode(ScreenCode: string) {
		var url = this._apiUrl + '/GetScreenModificationByScreenCode?ScreenCode=' + ScreenCode;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;
					var myResponse = new ServiceResponse();
					myResponse.Result = listJason;

					return myResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetSingleObjectFieldFromZeroTenant(id: string) {
		var url = this._apiUrl + '/GetSingleObjectFieldFromZeroTenant?' + 'id=' + id;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var pm = response;

					var entity: ObjectFieldPM;
					if (pm) {
						entity = this.MapJsonToObjectFieldPM(pm);
					}

					var serviceResponse: ServiceResponse;
					serviceResponse = new ServiceResponse();
					serviceResponse.Result = entity;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetSingleObjectFieldByFieldCodeFromZeroTenant(fieldCode: string) {
		var url = this._apiUrl + '/GetSingleObjectFieldByFieldCodeFromZeroTenant?' + 'fieldCode=' + fieldCode;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var pm = response;

					var entity: ObjectFieldPM;
					if (pm) {
						entity = this.MapJsonToObjectFieldPM(pm);
					}

					var serviceResponse: ServiceResponse;
					serviceResponse = new ServiceResponse();
					serviceResponse.Result = entity;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetSingleObjectFieldByFieldNameAndTableId(fieldName: string, tableId: string) {
		var url =
			this._apiUrl + '/GetSingleObjectFieldByFieldNameAndTableId?fieldName=' + fieldName + '&tableId=' + tableId;

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var pm = response;

					var entity: ObjectFieldPM;
					if (pm) {
						entity = this.MapJsonToObjectFieldPM(pm);
					}

					var serviceResponse: ServiceResponse;
					serviceResponse = new ServiceResponse();
					serviceResponse.Result = entity;

					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	GetObjectFieldModificationForLoggedTenant() {
		var url = this._apiUrl + '/GetObjectFieldModificationForLoggedTenant';

		return defer(() => {
			return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
				map((response) => {
					var listJason = response;

					var serviceResponse = new ServiceResponse();
					serviceResponse.Result = listJason;
					return serviceResponse;
				}),
				catchError(ServiceHelper.HandleServiceError),
			);
		});
	}

	MapFieldsTranslations(jsonPM: any, mapParent: boolean = true, entityPM: FieldsTranslations = null) {
		if (!entityPM) {
			entityPM = new FieldsTranslations();
		}

		var jsonPMKeys = Object.keys(jsonPM);

		for (var key in jsonPMKeys) {
			if (jsonPMKeys[key] === 'UIProperties') {
				continue;
			}
			var property = jsonPMKeys[key];
			entityPM[property] = jsonPM[property];
		}

		entityPM.IsDirty = false;

		if (mapParent) {
			entityPM.OldEntityPM = this.clone(entityPM);
		} else {
			entityPM.OldEntityPM = null;
		}

		return entityPM;
	}

	MapFieldsPickList(jsonList: any) {
		var entityList: CustomPickListPM;
		entityList = new CustomPickListPM();
		var jsonListKeys = Object.keys(jsonList);

		for (var key in jsonListKeys) {
			var property = jsonListKeys[key];
			entityList[property] = jsonList[property];
		}

		return entityList;
	}

	MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entities: PickListGeneralEntitiesArgs = null) {
		if (!entities) {
			entities = new PickListGeneralEntitiesArgs();
			entities.CustomPickListPMs = [];
			entities.RemovedCustomPickListPMs = [];
		}

		var jsonPMKeys = Object.keys(jsonPM);

		for (var key in jsonPMKeys) {
			if (jsonPMKeys[key] === 'UIProperties') {
				continue;
			}

			var property = jsonPMKeys[key];
			entities[property] = jsonPM[property];
		}

		//entityPM.IsDirty = false;

		//if (getCallMap) {
		//    entityPM.OldEntityPM = this.clone(entityPM);

		//}
		//else {

		//    entityPM.OldEntityPM = null;
		//}

		return entities;
	}

	MapJsonToScreenFieldsPM(jsonPM: any, getCallMap: boolean = true, entities: ScreenLayoutArgs = null) {
		if (!entities) {
			entities = new ScreenLayoutArgs();
			entities.ScreenFields = [];
			entities.RemovedScreenFields = [];
		}

		var jsonPMKeys = Object.keys(jsonPM);

		for (var key in jsonPMKeys) {
			if (jsonPMKeys[key] === 'UIProperties') {
				continue;
			}

			var property = jsonPMKeys[key];
			entities[property] = jsonPM[property];
		}

		return entities;
	}

	MapTextCodeType(jsonList: any) {
		var entityList: TextCodeType;
		entityList = new TextCodeType();
		var jsonListKeys = Object.keys(jsonList);

		for (var key in jsonListKeys) {
			var property = jsonListKeys[key];
			entityList[property] = jsonList[property];
		}

		return entityList;
	}

	MapJsonToFieldsUpdateHelper(jsonPM: any, getCallMap: boolean = true, entityPM: FieldsUpdateHelper = null) {
		if (!entityPM) {
			entityPM = new FieldsUpdateHelper();
		}

		var jsonPMKeys = Object.keys(jsonPM);

		for (var key in jsonPMKeys) {
			var property = jsonPMKeys[key];

			if (property === 'UIProperties') {
				continue;
			} else if (property === 'Items') {
				entityPM.Items = new Array<FieldsTranslations>();
				for (var item in jsonPM.Items) {
					var jItem = jsonPM.Items[item];

					var newItemPM: FieldsTranslations;
					newItemPM = this.MapFieldsTranslations(jItem);
					entityPM.Items.push(newItemPM);
				}
			} else {
				entityPM[property] = jsonPM[property];
			}
		}

		return entityPM;
	}

	MapJsonToObjectFieldPM(jsonPM: any, mapParent: boolean = true, entityPM: ObjectFieldPM = null) {
		if (!entityPM) {
			entityPM = new ObjectFieldPM();
		}

		var jsonPMKeys = Object.keys(jsonPM);

		for (var key in jsonPMKeys) {
			if (jsonPMKeys[key] === 'UIProperties') {
				continue;
			}
			var property = jsonPMKeys[key];
			entityPM[property] = jsonPM[property];
		}

		this.MapObjectFieldValidations(entityPM, jsonPM, mapParent);

		entityPM.IsDirty = false;

		if (mapParent) {
			entityPM.OldEntityPM = this.clone(entityPM);

			entityPM.OldEntityPM.ObjectFieldValidations = [];
			for (var item in entityPM.ObjectFieldValidations) {
				var myObjectFieldValidationPM = entityPM.ObjectFieldValidations[item];
				var newObjectFieldValidationPM: ObjectFieldValidationPM = this.clone(myObjectFieldValidationPM);

				entityPM.OldEntityPM.ObjectFieldValidations.push(newObjectFieldValidationPM);
			}
		} else {
			entityPM.OldEntityPM = null;
		}

		return entityPM;
	}

	MapObjectFieldValidations(entityPM: ObjectFieldPM, jsonPM: any, mapParent: boolean = true) {
		var oldObjectFieldValidations: ObjectFieldValidationPM[] = [];
		if (entityPM.OldEntityPM && !mapParent) {
			oldObjectFieldValidations = entityPM.OldEntityPM.ObjectFieldValidations;
		}

		entityPM.ObjectFieldValidations = new Array<ObjectFieldValidationPM>();
		for (var item in jsonPM.ObjectFieldValidations) {
			var jItem = jsonPM.ObjectFieldValidations[item];
			if (mapParent && (jItem.ChangeSetOp == 'Delete' || jItem.ChangeSetOp == 3)) {
				continue;
			}
			var newObjectFieldValidationPM: ObjectFieldValidationPM;

			if (mapParent) {
				newObjectFieldValidationPM = new ObjectFieldValidationPM(entityPM);
			} else {
				newObjectFieldValidationPM = new ObjectFieldValidationPM(null);
			}

			var pmKeysArray = Object.keys(jItem);
			for (var pmKey in pmKeysArray) {
				if ((!mapParent && pmKeysArray[pmKey] === 'entityParentPM') || pmKeysArray[pmKey] === 'UIProperties') {
					continue;
				}
				var pmProperty = pmKeysArray[pmKey];
				newObjectFieldValidationPM[pmProperty] = jItem[pmProperty];
			}
			newObjectFieldValidationPM.IsDirty = false;

			if (mapParent) {
				newObjectFieldValidationPM.UniqueKey = Guid.newGuid();
				newObjectFieldValidationPM.ChangeSetOp = 'None';
				jItem.ChangeSetOp = 'None';
				newObjectFieldValidationPM.OldEntityPM = this.clone(newObjectFieldValidationPM);
			} else {
				if (newObjectFieldValidationPM.UniqueKey) {
					if (jItem.IsDirty) newObjectFieldValidationPM.ChangeSetOp = 'Update';
				} else {
					newObjectFieldValidationPM.ChangeSetOp = 'Insert';
				}

				newObjectFieldValidationPM.OldEntityPM = null;
				newObjectFieldValidationPM.EntityParentPM = null;
			}

			entityPM.ObjectFieldValidations.push(newObjectFieldValidationPM);
		}

		if (oldObjectFieldValidations) {
			for (var itemKey in oldObjectFieldValidations) {
				if (
					entityPM.ObjectFieldValidations.filter((p) => p.UniqueKey === oldObjectFieldValidations[itemKey].UniqueKey)
						.length === 0
				) {
					if (oldObjectFieldValidations[itemKey]) {
						var oldItemJson = oldObjectFieldValidations[itemKey];
						var deletedPM: ObjectFieldValidationPM = new ObjectFieldValidationPM(null);
						var pmKeys = Object.keys(oldItemJson);
						for (var key in pmKeys) {
							if (
								(!mapParent && pmKeys[key] === 'entityParentPM') ||
								pmKeys[key] === 'UIProperties' ||
								pmKeys[key] === 'OldEntityPM'
							) {
								continue;
							}

							var property = pmKeys[key];
							deletedPM[property] = oldItemJson[property];
						}

						deletedPM.IsDirty = false;
						deletedPM.ChangeSetOp = 'Delete';

						deletedPM.OldEntityPM = null;
						entityPM.ObjectFieldValidations.push(deletedPM);
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
			if (
				jsonPMKeys[key] === 'entityParentPM' ||
				jsonPMKeys[key] === 'UIProperties' ||
				jsonPMKeys[key] === 'OldEntityPM' ||
				jsonPMKeys[key] === 'PropertyChanged'
			) {
				continue;
			}

			var property = jsonPMKeys[key];
			entityPM[property] = jsonPM[property];
		}
		return entityPM;
	}
}

export class FieldsTranslations {
	constructor() {
		this.IsDirty = false;
	}

	public TextCodeId: string;
	public TextCodeCode: string;
	public Code: string;

	private translatedText: string;
	public get TranslatedText() {
		return this.translatedText;
	}
	public set TranslatedText(newValue: string) {
		if (this.translatedText != newValue) {
			this.translatedText = newValue;
			this.MarkAsDirty('TranslatedText');
		}
	}

	private translatedTextPlural: string;
	public get TranslatedTextPlural() {
		return this.translatedTextPlural;
	}
	public set TranslatedTextPlural(newValue: string) {
		if (this.translatedTextPlural != newValue) {
			this.translatedTextPlural = newValue;
			this.MarkAsDirty('TranslatedTextPlural');
		}
	}

	public DefaultText: string;
	public DefaultTextPlural: string;
	public Tenant: number;
	public TypeCode: string;
	public ObjectTableID: string;
	public TranslationTenent: number;
	public ObjectTableName: string;
	public ObjectTableTypeCode: string;
	public TranslationLanguageCode: string;
	public TranslateDate: Date;
	public TranslatedByUserId: string;
	public IsTranslated: boolean;
	public OldEntityPM: FieldsTranslations;
	public IsDirty: boolean;

	MarkAsDirty(propertyName: string = null) {
		this.IsDirty = true;

		if (propertyName != null) {
		}
	}
}

export class TranslationArgs {
	public FieldsTranslations: FieldsTranslations[];
	public CountAll: number;
}

export class TextCodeType {
	public Name: string;
	public Code: string;
}

export class FieldsUpdateHelper {
	public Tenant: number;
	public Items: FieldsTranslations[] = [];
}
