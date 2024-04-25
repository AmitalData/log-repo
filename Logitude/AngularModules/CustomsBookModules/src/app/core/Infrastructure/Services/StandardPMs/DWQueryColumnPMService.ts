
import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {DWQueryColumnPM} from '../../EntityPMs/DWQueryColumnPM';


@Injectable()
export class DWQueryColumnsPMService {

    private _apiUrl: string;
    private _http: HttpClient;

    constructor() {

        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DWQueryColumn';  
    }



    GetDWQueryColumnPMs(tenant: number, dWQueryid: string, dWObjecttableid: string, userid: string) {

        console.log('--------------------------------------> calling getSingleEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/getDWQuerycolumnpms?tenant=' + tenant + '&dwqueryid=' + dWQueryid + '&dwobjecttableid=' + dWObjecttableid + '&userid=' + userid, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pms = response.body;

                        var _mappedListsArray: Array<DWQueryColumnPM> = [];

                        for (var key in pms) {
                            var entity: DWQueryColumnPM;
                            entity = this.MapJsonToEntityPM(pms[key]);
                            _mappedListsArray.push(entity);
                        }

                        return _mappedListsArray;
                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: DWQueryColumnPM) {

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
                var mappedEntity: DWQueryColumnPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWQueryColumnPM;
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

    update(entityPM: DWQueryColumnPM) {

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
                var mappedEntity: DWQueryColumnPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWQueryColumnPM;
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

    delete(entityPM: DWQueryColumnPM) {

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
                var mappedEntity: DWQueryColumnPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.delete(this._apiUrl + "/delete?id=" + entityPM.Id + "&tenant=" + entityPM.Tenant, ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {
                                var mappedResult: DWQueryColumnPM;
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

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: DWQueryColumnPM = null) {


        if (!entityPM) {

            entityPM = new DWQueryColumnPM();
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
