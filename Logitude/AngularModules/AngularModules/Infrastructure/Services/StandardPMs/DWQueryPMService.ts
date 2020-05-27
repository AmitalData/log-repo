
import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import { defer, of } from 'rxjs'; 
import {DWQueryPM} from '../../EntityPMs/DWQueryPM';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';


@Injectable()
export class DWQueryPMService {

    private _apiUrl: string;
    private _http: HttpClient;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DWQuery';     
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



                        var entity: DWQueryPM = new DWQueryPM();
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        //var servertime = response.headers.get('ServerExecutionTime');
                        //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "DWObjectField", "GetSinglePM", 'id=' + id);

                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: DWQueryPM) {

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
                var mappedEntity: DWQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWQueryPM;
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

    update(entityPM: DWQueryPM) {

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
                var mappedEntity: DWQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWQueryPM;
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

    delete(entityPM: DWQueryPM) {

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
                var mappedEntity: DWQueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.delete(this._apiUrl + '?id=' + entityPM.Id + '&tenant=' + entityPM.Tenant, ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWQueryPM;
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

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: DWQueryPM = null) {


        if (!entityPM) {

            entityPM = new DWQueryPM();
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
