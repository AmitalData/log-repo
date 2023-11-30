import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {CounterPM} from '../EntityPMs/CounterPM';
import {CounterDefinitionPM} from '../EntityPMs/CounterDefinitionPM';
import {TenantSettingPM} from '../../Infrastructure/EntityPMs/TenantSettingPM';

@Injectable()

export class CountersDomainService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CountersDomain';
    }

    GetTenantCounters() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTenantCounters',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<CounterPM> = [];

                for (var key in listJason) {

                    var entity: CounterPM;
                    entity = this.MapJsonToCounterPM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCounterDefinitions(CounterId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCounterDefinitions?CounterId=' + CounterId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<CounterDefinitionPM> = [];

                for (var key in listJason) {

                    var entity: CounterDefinitionPM;
                    entity = this.MapJsonToCounterDefinitionPM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCounterAPIHelper(CounterId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCounterAPIHelper?CounterId=' + CounterId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;

                var mappedResult: CounterAPIHelper = this.MapJsonToCounterAPIHelper(myJsonResult, true);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCounterProperties(counterCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCounterProperties?counterCode=' + counterCode;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;


                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    Post(args: CounterAPIHelper) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: CounterAPIHelper = this.MapJsonToCounterAPIHelper(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: CounterAPIHelper = this.MapJsonToCounterAPIHelper(myJsonResult, true);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToCounterAPIHelper(jsonPM: any, getCallMap: boolean = true, entity: CounterAPIHelper = null) {
        if (!entity) {
            entity = new CounterAPIHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "CounterPM") {
                entity.CounterPM = this.MapJsonToCounterPM(jsonPM.CounterPM, getCallMap);
            }

            else if (property === "TenantSettings") {

                entity.TenantSettings = new Array<TenantSettingPM>();

                for (var item_Setting in jsonPM.TenantSettings) {
                    var jItem_Setting = jsonPM.TenantSettings[item_Setting];
                    var newItemPM_Setting: TenantSettingPM = this.MapJsonToTenantSettingPM(jItem_Setting, getCallMap);
                    entity.TenantSettings.push(newItemPM_Setting);
                }
            }

            else if (property === "CounterDefinitions") {

                entity.CounterDefinitions = new Array<CounterDefinitionPM>();

                for (var item_Definition in jsonPM.CounterDefinitions) {
                    var jItem_Definition = jsonPM.CounterDefinitions[item_Definition];
                    var newItemPM_Definition: CounterDefinitionPM = this.MapJsonToCounterDefinitionPM(jItem_Definition, getCallMap);
                    entity.CounterDefinitions.push(newItemPM_Definition);
                }
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
    MapJsonToCounterPM(jsonPM: any, mapParent: boolean = true, entityPM: CounterPM = null) {
        if (!entityPM) {
            entityPM = new CounterPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];

            entityPM[property] = jsonPM[property];

            entityPM.IsDirty = false;

            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
            }

            else {
                entityPM.OldEntityPM = null;
            }
        }

        return entityPM;
    }
    MapJsonToTenantSettingPM(jsonPM: any, mapParent: boolean = true, entityPM: TenantSettingPM = null) {
        if (!entityPM) {
            entityPM = new TenantSettingPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];

            entityPM[property] = jsonPM[property];

            entityPM.IsDirty = false;

            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
            }

            else {
                entityPM.OldEntityPM = null;
            }
        }

        return entityPM;
    }
    MapJsonToCounterDefinitionPM(jsonPM: any, mapParent: boolean = true, entityPM: CounterDefinitionPM = null) {
        if (!entityPM) {
            entityPM = new CounterDefinitionPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];

            entityPM[property] = jsonPM[property];

            entityPM.IsDirty = false;

            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
            }

            else {
                entityPM.OldEntityPM = null;
            }
        }

        return entityPM;
    }

    //GetLastValueCounterStatByCounterId(counterId: string) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

    //    return defer(() => {
    //        return this._http.get(this._apiUrl + '/GetLargestLastValueFromCounterStatByCounterId?counterId=' + counterId , ServiceHelper.GetHttpHeaders())
    //            .pipe(
    //                map(response => {
    //                    var myResult = response;
    //                    var serviceResponse: ServiceResponse;
    //                    serviceResponse = new ServiceResponse();
    //                    serviceResponse.Result = myResult;
    //                    return serviceResponse;
    //        }), catchError(ServiceHelper.HandleServiceError));
    //    });
    //}

    clone(jsonPM: any) {
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

export class CounterAPIHelper {
    CounterId: string;
    IsCounterUsed: boolean;
    LastDBValue: number;
    CounterPM: CounterPM;
    TenantSettings: TenantSettingPM[] = [];
    CounterDefinitions: CounterDefinitionPM[] = [];
    IsCustomized: boolean;
}
