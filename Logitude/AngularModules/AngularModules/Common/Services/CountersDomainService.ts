import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {CounterPM} from '../EntityPMs/CounterPM';
import {CounterDefinitionPM} from '../EntityPMs/CounterDefinitionPM';
import {TenantSettingPM} from '../../Infrastructure/EntityPMs/TenantSettingPM';

@Injectable()

export class CountersDomainService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CountersDomain';
    }

    GetTenantCounters() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTenantCounters', { headers: authHeader }).map(response => {
                var listJason = response.json();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCounterDefinitions(CounterId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCounterDefinitions?CounterId=' + CounterId, { headers: authHeader }).map(response => {
                var listJason = response.json();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCounterAPIHelper(CounterId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCounterAPIHelper?CounterId=' + CounterId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var mappedResult: CounterAPIHelper = this.MapJsonToCounterAPIHelper(myJsonResult, true);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCounterProperties(counterCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCounterProperties?counterCode=' + counterCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();


                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    Post(args: CounterAPIHelper) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: CounterAPIHelper = this.MapJsonToCounterAPIHelper(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: CounterAPIHelper = this.MapJsonToCounterAPIHelper(myJsonResult, true);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
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
}
