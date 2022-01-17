 
import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {AdvancedQueryFilterPM} from '../../EntityPMs/AdvancedQueryFilterPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';

@Injectable()
export class AdvancedQueryFiltersPMService {

 private _apiUrl: string;
 private _http: HttpClient;
 private _serviceArgs: ServiceArgs;
 constructor() {
        
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/advancedqueryfilters';
    }

    getadvancedqueryfiltersbytenant(tenant: number, userid: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/getadvancedqueryfiltersbytenant?' + 'tenant=' + tenant + '&loggedcontactid=' + userid, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pms = response;

                return pms;
            }), catchError(ServiceHelper.HandleServiceError));
        });

    }

    getadvancedqueryfiltersbytenantByQuery(tenant: number, userid: string, queryCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/getadvancedqueryfiltersbytenantandquery?' + 'tenant=' + tenant + '&loggedcontactid=' + userid + '&queryCode=' + queryCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pms = response;

                return pms;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    getuseradvancedqueryfilterbytenantobjecttablequery(tenant: number, objecttableCode: string, queryCode: string, userid: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/getadvancedqueryfiltersbytenantuserobjecttablequery?' + 'tenant=' + tenant + '&objecttableCode=' + objecttableCode + '&queryCode=' + queryCode + '&loggedcontactid=' + userid, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pms = response;

                return pms;
            }), catchError(ServiceHelper.HandleServiceError));
        });

    }


    insert(entityPM: AdvancedQueryFilterPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AdvancedQueryFilterPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: AdvancedQueryFilterPM;
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

    update(entityPM: AdvancedQueryFilterPM) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("AnalyzeQueue", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AdvancedQueryFilterPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return ServiceHelper.HttpClient.put(ServiceHelper.GetLogitudeURL() + 'api/advancedqueryfilters', JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {


                            var pm = response.body;
                            if (pm) {
                                var mappedResult: AdvancedQueryFilterPM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AnalyzeQueue", "SaveChanges", "");

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

    delete(entityPM: AdvancedQueryFilterPM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var serviceResponse: EntityPMServiceResponse;
            serviceResponse = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AdvancedQueryFilterPM;

                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: AdvancedQueryFilterPM;
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

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: AdvancedQueryFilterPM = null) {


        if (!entityPM) {

            entityPM = new AdvancedQueryFilterPM();
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
