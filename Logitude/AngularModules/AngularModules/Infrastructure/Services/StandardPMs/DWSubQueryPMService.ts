
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {Observable}     from 'rxjs/Rx';

import {DWSubQueryPM} from '../../EntityPMs/DWSubQueryPM';
import { DWQueryData } from '../../../Common/DataContracts/DWQueryData';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';


@Injectable()
export class DWSubQueryPMService {

    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DWSubQuery';     
    }

    insertDWQueryData(entityPM: DWQueryData) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                //var mappedEntity: QueryColumnPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                //////////////////////////////////////////////////////
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM.SubQueryData, false);
                entityPM.SubQueryData = mappedEntity;
                var temp = this.deepClone(entityPM);

                /////////////////////////////////////////////////////
                return this._http.post(this._apiUrl, JSON.stringify(temp),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            //var mappedResult: QueryColumnPM;
                            //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = pm;
                        }



                        return response;//response;

                    });
            }
            else {

                return null;//Observable.of(response);

            }
        }

        );
    }
    UpdateDWQueryData(entityPM: DWQueryData) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                //var mappedEntity: QueryColumnPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                //////////////////////////////////////////////////////
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM.SubQueryData, false);
                entityPM.SubQueryData = mappedEntity;
                var temp = this.deepClone(entityPM);

                /////////////////////////////////////////////////////
                return this._http.put(this._apiUrl, JSON.stringify(temp),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            //var mappedResult: QueryColumnPM;
                            //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = pm;
                        }

                        return response;//response;

                    });
            }
            else {

                return null;//Observable.of(response);

            }
        }

        );
    }
    public deepClone(obj, hash = new WeakMap()) {
        // Do not try to clone primitives or functions
        if (Object(obj) !== obj || obj instanceof Function) {
            return obj;
        }

        if (hash.has(obj)) {
            //return hash.get(obj); // Cyclic reference
            return;
        }

        try { // Try to run constructor (without arguments, as we don't know them)
            var result = new obj.constructor();
        }
        catch (e) { // Constructor failed, create object without running the constructor
            result = Object.create(Object.getPrototypeOf(obj));
        }

        // Optional: support for some standard constructors (extend as desired)
        if (obj instanceof Map) {
            Array.from(obj, ([key, val]) => result.set(this.deepClone(key, hash),
                this.deepClone(val, hash)));
        }
        else if (obj instanceof Set) {
            Array.from(obj, (key) => result.add(this.deepClone(key, hash)));
        }

        // Register in hash    
        hash.set(obj, result);

        // Clone and assign enumerable own properties recursively
        return Object.assign(result, ...Object.keys(obj).map(
            key => ({
                [key]:

                    key != "UIProperties" && key != "MyParentClass" && key != "Items" ? this.deepClone(obj[key], hash) : true

            })));
    }

    insert(entityPM: DWSubQueryPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedDWQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DWSubQueryPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = mappedResult;
                        }



                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }

    update(entityPM: DWSubQueryPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedDWQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DWSubQueryPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = mappedResult;
                        }



                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }

    delete(entityPM: DWSubQueryPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedDWQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.delete(this._apiUrl + '?id=' + entityPM.Id + '&tenant=' + entityPM.Tenant,
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DWSubQueryPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = mappedResult;
                        }



                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                var entity: DWQueryData = new DWQueryData();
                if (pm) {
                    entity.SubQueryData = pm.SubQueryData;//this.MapJsonToEntityPM(pm);
                    entity.Columns = pm.Columns;
                    entity.Filters = pm.Filters;
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "DWObjectField", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getByQueryId(Queryid: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getByQueryId?' + 'id=' + Queryid, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                var entity: DWQueryData = new DWQueryData();
                if (pm) {
                    entity.SubQueryData = pm.SubQueryData;//this.MapJsonToEntityPM(pm);
                    entity.Columns = pm.Columns;
                    entity.Filters = pm.Filters;
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "DWObjectField", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: DWSubQueryPM = null) {


        if (!entityPM) {

            entityPM = new DWSubQueryPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        //entityPM.IsDirty = false;

        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entityPM;
    }

}
