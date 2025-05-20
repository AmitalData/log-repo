import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer, of } from 'rxjs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { catchError, map } from 'rxjs/operators';
import { ChartingDataClass } from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { AdvancedQueryFilterPM } from 'Infrastructure/EntityPMs/AdvancedQueryFilterPM';
import { ClassLevelValidator } from 'Infrastructure/Validators/ClassLevelValidator';
import { EntityPMServiceResponse } from 'Infrastructure/DataContracts/EntityPMServiceResponse';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';

@Injectable()

export class DeclarationReferantDataWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationReferantDataWebService';
    }
    GetQueriesCounts(refId:string, depId:string, transportMode:string) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetQueriesCounts?refId=" + refId + "&depId=" + depId + "&transportMode=" + transportMode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    getadvancedqueryfiltersbytenantByQuery(tenant: number, userid: string, queryCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAdvancedQueryFilters?' + 'tenant=' + tenant + '&loggedcontactid=' + userid + '&queryCode=' + queryCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pms = response;

                return pms;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDeclarationReferandDateByDeclarationIdToDisplay(declarationId:string, tenant: number) {

        var url = this._apiUrl + '/GetDeclarationReferandDateByDeclarationIdToDisplay?' + 'declarationId=' +  declarationId +'&tenant=' + tenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }



    GetDeclarationReferantDataDashBoard(Tenant: number) {

        var url = this._apiUrl + '/GetDeclarationReferantDataDashBoard?tenant=' + Tenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: Array<ChartingDataClass> = new Array<ChartingDataClass>();
                for (var key in allLists) {
                    var entity: ChartingDataClass;
                    entity = this.MapJsonToEntityListChartingDataClass(allLists[key]);
                    myList.push(entity);
                }


                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myList;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insertDeclarationReferantFilters(entityPM: AdvancedQueryFilterPM) {

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
                return this._http.put( this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
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
    GetSingleByObjectFieldCodeAndTenant(objectFieldId: string, tenant: number,queryCode:string,loggedUserId:string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(ServiceHelper.GetLogitudeURL() +'api/AdvancedQueryFiltersExtended/GetSingleByObjectFieldCodeAndTenant?' + 'objectFieldId=' + objectFieldId + '&tenant=' + tenant+ '&queryCode=' + queryCode+ '&loggedUserId=' + loggedUserId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pms = response;

                return pms;
            }), catchError(ServiceHelper.HandleServiceError));
        });

    }
    MapJsonToEntityListChartingDataClass(jsonList: any) {

        var entityList: ChartingDataClass;
        entityList = new ChartingDataClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
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
