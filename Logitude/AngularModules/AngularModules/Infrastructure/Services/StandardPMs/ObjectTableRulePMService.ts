import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ObjectTableRulePM} from '../../EntityPMs/ObjectTableRulePM';
import {RuleConditionFieldPM} from '../../EntityPMs/RuleConditionFieldPM';
 import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
@Injectable()
export class ObjectTableRulePMService {
   
   
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ObjectTableRules';
    }

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                var pm = response.body;


                var entity: ObjectTableRulePM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }); 
    }

    insert(entityPM: ObjectTableRulePM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("ObjectTableRule", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ObjectTableRulePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: ObjectTableRulePM;
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

    update(entityPM: ObjectTableRulePM) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("ObjectTableRule", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ObjectTableRulePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                        var pm = response.body;
                        if (pm) {
                            var mappedResult: ObjectTableRulePM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }


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


    getAllByTenant(tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetObjectTableRulePMsByTenant?' + 'tenant=' + tenant, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                var result = response.body;

                
                var mappedResult: Array<ObjectTableRulePM> = [];
                if (result) {
                    for (var k in result) {
                        var mappedPM: ObjectTableRulePM;
                        mappedPM = this.MapJsonToEntityPM(result[k]);
                        mappedResult.push(mappedPM);
                    }
                    //mappedResult = ServiceHelper.MapJsonToArrayofEntities(result, ObjectTableRulePM);
                
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    restoreDefaultRule(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetRestoredDefaultRule?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;

                        var entity: ObjectTableRulePM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;
                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ObjectTableRulePM = null) {


        if (!entityPM) {

            entityPM = new ObjectTableRulePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldRuleConditionFields: RuleConditionFieldPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldRuleConditionFields = entityPM.OldEntityPM.RuleConditionFields;
        }


        entityPM.RuleConditionFields = new Array<RuleConditionFieldPM>();
        for (var item in jsonPM.RuleConditionFields) {

            var jItem = jsonPM.RuleConditionFields[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newRuleConditionFieldPM: RuleConditionFieldPM;
            if (mapParent) {
                newRuleConditionFieldPM = new RuleConditionFieldPM(entityPM);
            }
            else {
                newRuleConditionFieldPM = new RuleConditionFieldPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newRuleConditionFieldPM[pmProperty] = jItem[pmProperty];
            }
            newRuleConditionFieldPM.IsDirty = false;
            if (mapParent) {
                newRuleConditionFieldPM.OldEntityPM = this.clone(newRuleConditionFieldPM);
                newRuleConditionFieldPM.UniqueKey = Guid.newGuid();
                newRuleConditionFieldPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newRuleConditionFieldPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newRuleConditionFieldPM.ChangeSetOp = "Update";
                }
                else {
                    newRuleConditionFieldPM.ChangeSetOp = "Insert";
                }

                newRuleConditionFieldPM.OldEntityPM = null;
                newRuleConditionFieldPM.EntityParentPM = null;
            }


            entityPM.RuleConditionFields.push(newRuleConditionFieldPM);
        }

        if (oldRuleConditionFields) {

            for (var itemKey in oldRuleConditionFields) {
                if (entityPM.RuleConditionFields.filter(p=> p.UniqueKey === oldRuleConditionFields[itemKey].UniqueKey).length === 0) {

                    if (oldRuleConditionFields[itemKey]) {
                        oldRuleConditionFields[itemKey].ChangeSetOp = "Delete";
                        entityPM.RuleConditionFields.push(oldRuleConditionFields[itemKey]);
                    }
                }
            }
        }



        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.RuleConditionFields = [];
            for (var m in entityPM.RuleConditionFields) {
                entityPM.OldEntityPM.RuleConditionFields.push(this.clone(entityPM.RuleConditionFields[m]));
            }
          

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
        var entityPM: ObjectTableRulePM;
        entityPM = new ObjectTableRulePM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;

        return entityPM;
    }

}
