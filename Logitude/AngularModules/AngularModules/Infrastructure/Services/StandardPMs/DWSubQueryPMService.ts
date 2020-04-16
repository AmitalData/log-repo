
import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import { defer, of } from 'rxjs';

import {DWSubQueryPM} from '../../EntityPMs/DWSubQueryPM';
import { DWQueryData } from '../../../Common/DataContracts/DWQueryData';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';


@Injectable()
export class DWSubQueryPMService {

    private _apiUrl: string;
    private _http: HttpClient;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DWSubQuery';     
    }

    insertDWQueryData(entityPM: DWQueryData) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                //var mappedEntity: QueryColumnPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                //////////////////////////////////////////////////////
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM.SubQueryData, false);
                entityPM.SubQueryData = mappedEntity;
                var temp = this.deepClone(entityPM);

                /////////////////////////////////////////////////////
                return this._http.post(this._apiUrl, JSON.stringify(temp), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                        var pm = response.body;
                        if (pm) {
                            //var mappedResult: QueryColumnPM;
                            //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = pm;
                        }



                        return serviceResponse;//response;

                        }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                return null;//of(response);

            }
        });
    }

    UpdateDWQueryData(entityPM: DWQueryData) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                //var mappedEntity: QueryColumnPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                //////////////////////////////////////////////////////
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM.SubQueryData, false);
                entityPM.SubQueryData = mappedEntity;
                var temp = this.deepClone(entityPM);

                /////////////////////////////////////////////////////
                return this._http.put(this._apiUrl, JSON.stringify(temp), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                        var pm = response.body;
                        if (pm) {
                            //var mappedResult: QueryColumnPM;
                            //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = pm;
                        }

                            return serviceResponse;//response;

                        }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                return null;//of(response);

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

                     key != "UIProperties" && key != "MyParentClass" && key != "ShowSampleDateCommand" && key != "Items" && key != "TooltipId" && key != "TooltipContentId" && key != "CurrentSession" ? this.deepClone(obj[key], hash) : true

            })));
    }
    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "MyParentClass" || jsonPMKeys[key] === "ShowSampleDateCommand" || jsonPMKeys[key] === "Items" || jsonPMKeys[key] === "TooltipId" || jsonPMKeys[key] === "TooltipContentId" || jsonPMKeys[key] === "CurrentSession") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }
    insert(entityPM: DWSubQueryPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedDWQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWSubQueryPM;
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

    update(entityPM: DWSubQueryPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedDWQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWSubQueryPM;
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

    delete(entityPM: DWSubQueryPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedDWQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWSubQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.delete(this._apiUrl + '?id=' + entityPM.Id + '&tenant=' + entityPM.Tenant, ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWSubQueryPM;
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

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



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

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getByQueryId(Queryid: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getByQueryId?' + 'id=' + Queryid, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



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

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getFromTenant(id: string, copyFromTenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingleFromTenant?' + 'id=' + id + '&copyFromTenant=' + copyFromTenant, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



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

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getByQueryIdFromTenant(Queryid: string, copyFromTenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getByQueryIdFromTenant?' + 'id=' + Queryid + '&copyFromTenant=' + copyFromTenant, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



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

                    }), catchError(ServiceHelper.HandleServiceError));
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
