import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {ObjectTableRuleFieldPM} from '../../EntityPMs/ObjectTableRuleFieldPM';


@Injectable()
export class ObjectTableRuleFieldPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ObjectTableRuleFields';
    }

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                var pm = response.body;


                var entity: ObjectTableRuleFieldPM;
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

    insert(entityPM: ObjectTableRuleFieldPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("ObjectTableRuleField", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ObjectTableRuleFieldPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: ObjectTableRuleFieldPM;
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

    update(entityPM: ObjectTableRuleFieldPM) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("ObjectTableRuleField", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ObjectTableRuleFieldPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: ObjectTableRuleFieldPM;
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

    updateRuleFieldsList(ruleFieldsPMList: ObjectTableRuleFieldPM[]) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedList: ObjectTableRuleFieldPM[] = [];
            for (var k in ruleFieldsPMList) {
                var field = ruleFieldsPMList[k];
                var mappedEntity: ObjectTableRuleFieldPM;
                mappedEntity = this.MapJsonToEntityPM(field, false);
                mappedList.push(mappedEntity);
            }

            return this._http.put(this._apiUrl + '/PutRuleFieldsList', JSON.stringify(mappedList), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                    var pm = response.body;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });

    }


    getAllByTenant(tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetObjectTableRuleFieldPMsByTenant?' + 'tenant=' + tenant, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var result = response.body;


                        var mappedResult: Array<ObjectTableRuleFieldPM> = [];
                        if (result) {
                            mappedResult = ServiceHelper.MapJsonToArrayofEntities(result, ObjectTableRuleFieldPM);

                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = mappedResult;
                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ObjectTableRuleFieldPM = null) {


        if (!entityPM) {

            entityPM = new ObjectTableRuleFieldPM();
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
        var entityPM: ObjectTableRuleFieldPM;
        entityPM = new ObjectTableRuleFieldPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }


}
