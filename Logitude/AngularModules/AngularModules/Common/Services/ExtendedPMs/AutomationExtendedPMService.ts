
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {AutomationPM} from '../../EntityPMs/AutomationPMExtended';

import {AutomationArgs} from '../../../Infrastructure/DataContracts/AutomationArgs';

@Injectable()
export class AutomationExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AutomationExtended';
    }


    getAutomationesByObjectTableId(objectTableId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getautomationesbyobjecttableid" + '?objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: AutomationPM;
            var automationPMLists: AutomationPM[];
            automationPMLists = new Array<AutomationPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM2(item);
                automationPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = automationPMLists;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }

    getAutomationBackupDataById(automationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getautomationbackupdatabyid" + '?automationId=' + automationId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
  
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }


    putAuomationList(items: AutomationArgs[]) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/putauomationlist', JSON.stringify(items),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


    update(entityPM: AutomationPM) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Automation", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AutomationPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this._http.put(this._apiUrl + '/put', JSON.stringify(mappedEntity),
           
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: AutomationPM;
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
        }

        );

    }


    insert(entityPM: AutomationPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Automation", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AutomationPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this._http.post(this._apiUrl + '/post', JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: AutomationPM;
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
        }

        );
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AutomationPM = null) {


        if (!entityPM) {

            entityPM = new AutomationPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }


    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }


    MapJsonToEntityPM2(jsonPM: any) {

        var entityPM: AutomationPM;
        entityPM = new AutomationPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }



}

