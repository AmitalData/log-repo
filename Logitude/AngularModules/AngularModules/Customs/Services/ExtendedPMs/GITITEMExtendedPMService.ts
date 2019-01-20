import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { GITITEMDto } from '../../EntityPMs/Extended/GITITEMDto';

export class GITITEMExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GITITEM';
    }

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();


                var entity: GITITEMDto;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    insert546(entityPM: GITITEMDto) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            //if (errorsArray.length == 0) {
            //var mappedEntity: GITITEMDto;
            //    mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return this._http.post(this._apiUrl, JSON.stringify(entityPM),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: GITITEMDto;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }

                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            //}
            //else {

            //    serviceResponse.HasError = true;
            //    serviceResponse.ErrorsArray = errorsArray;

            //    return Observable.of(serviceResponse);
            //}
        });
    }

    //insert(entityPM: GITITEMDto) {

    //    return Observable.defer(() => {

    //        var authHeader = new Headers();
    //        authHeader.append('Token', SessionInfo.Token);
    //        authHeader.append('Content-Type', 'application/json');
           

    //        var serviceResponse: ServiceResponse;
    //        serviceResponse = new ServiceResponse();
            
    //        return this._http
    //            .post(
    //            this._apiUrl + '/PostGITITEMPM',
    //            JSON.stringify(entityPM),
    //            { headers: authHeader })
    //            .map((res) => {
    //                var pm = res.json();
    //                if (pm) {
    //                    var mappedResult: GITITEMDto;
    //                    mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
    //                    serviceResponse.Result = mappedResult;
    //                }

    //                return serviceResponse;

    //            }).catch(ServiceHelper.HandleServiceError);

    //    });
    //}

    insert(GITITEMDtoList: GITITEMDto[]) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http
                .post(
                    this._apiUrl + '/PostGITITEMPMList',
                    JSON.stringify(GITITEMDtoList),
                    { headers: authHeader })
                .map((res) => {
                    //var pm = res.json();
                    //if (pm) {
                    //    var mappedResult: GITITEMDto[];
                    //    mappedResult = this.MapJsonToEntityPM(pm, true, GITITEMDtoList);
                    //    serviceResponse.Result = mappedResult;
                    //}

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        });
    }

    update(entityPM: GITITEMDto) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Customs.GITITEM", entityPM);
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: GITITEMDto;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: GITITEMDto;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);
            }
        });

    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: GITITEMDto = null) {


        if (!entityPM) {

            entityPM = new GITITEMDto();
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

        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entityPM;
    }


    //public clone(jsonPM: any) {
    //    var entityPM: any;
    //    entityPM = {};

    //    var jsonPMKeys = Object.keys(jsonPM);
    //    for (var key in jsonPMKeys) {

    //        if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
    //            continue;
    //        }

    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];

    //    }
    //    return entityPM;
    //}

}
