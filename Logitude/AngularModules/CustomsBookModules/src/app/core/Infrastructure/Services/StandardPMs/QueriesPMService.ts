
import {Injectable, Query} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import { defer, of } from 'rxjs';
import {QueryPM} from '../../EntityPMs/QueryPM';
import { SharedUserQueryPM } from '../../EntityPMs/SharedUserQueryPM';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';

@Injectable()
export class QueriesPMService {

    private _apiUrl: string;
    private _http: HttpClient;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/objectfields';    
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/queries';
    }

    get(UniqueCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'UniqueCode=' + UniqueCode, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;
                        var entity: QueryPM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: QueryPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: QueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: QueryPM;
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

    update(entityPM: QueryPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: QueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: QueryPM;
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

    delete(entityPM: QueryPM, userId: string) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = [];

            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: QueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.delete(this._apiUrl + '?UniqueCode=' + entityPM.UniqueCode + '&userId=' + userId, ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: QueryPM;
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

    public MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: QueryPM = null) {        
        if (!entityPM) {
            entityPM = new QueryPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapSharedUserQueies(entityPM, jsonPM, mapParent);

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.SharedUserQueries = [];
            for (var item in entityPM.SharedUserQueries) {
                var mySharedUserQueryPM = entityPM.SharedUserQueries[item];
                var newSharedUserQueryPM: SharedUserQueryPM = this.clone(mySharedUserQueryPM);
                entityPM.OldEntityPM.SharedUserQueries.push(newSharedUserQueryPM);
            }
        }

        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapSharedUserQueies(entityPM: QueryPM, jsonPM: any, mapParent: boolean = true) {
        var oldSharedUserQueries: SharedUserQueryPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSharedUserQueries = entityPM.OldEntityPM.SharedUserQueries;
        }

        entityPM.SharedUserQueries = new Array<SharedUserQueryPM>();
        for (var item in jsonPM.SharedUserQueries) {
            var jItem = jsonPM.SharedUserQueries[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSharedUserQueryPM: SharedUserQueryPM;

            if (mapParent) {
                newSharedUserQueryPM = new SharedUserQueryPM(entityPM);
            }
            else {
                newSharedUserQueryPM = new SharedUserQueryPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSharedUserQueryPM[pmProperty] = jItem[pmProperty];
            }
            newSharedUserQueryPM.IsDirty = false;

            if (mapParent) {
                newSharedUserQueryPM.UniqueKey = Guid.newGuid();
                newSharedUserQueryPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSharedUserQueryPM.OldEntityPM = this.clone(newSharedUserQueryPM);
            }
            else {
                if (newSharedUserQueryPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSharedUserQueryPM.ChangeSetOp = "Update";
                }
                else {
                    newSharedUserQueryPM.ChangeSetOp = "Insert";
                }

                newSharedUserQueryPM.OldEntityPM = null;
                newSharedUserQueryPM.EntityParentPM = null;
            }

            newSharedUserQueryPM.IsDirty = false;

            entityPM.SharedUserQueries.push(newSharedUserQueryPM);
        }

        if (oldSharedUserQueries) {
            for (var itemKey in oldSharedUserQueries) {
                if (entityPM.SharedUserQueries.filter(p => p.UniqueKey === oldSharedUserQueries[itemKey].UniqueKey).length === 0) {

                    if (oldSharedUserQueries[itemKey]) {
                        var oldItemJson = oldSharedUserQueries[itemKey];
                        var deletedPM: SharedUserQueryPM = new SharedUserQueryPM(null);
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
                        entityPM.SharedUserQueries.push(deletedPM);
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
}
