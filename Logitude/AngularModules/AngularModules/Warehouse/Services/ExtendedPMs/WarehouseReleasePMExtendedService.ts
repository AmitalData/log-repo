
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {WarehouseReleasePM} from '../../EntityPMs/WarehouseReleasePM';

import {WarehouseReleasePackagePM} from '../../EntityPMs/WarehouseReleasePackagePM';


@Injectable()
export class WarehouseReleasePMExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseReleaseExtended';
    }




    Insert(entityPM: WarehouseReleasePM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("WarehouseRelease", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: WarehouseReleasePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl + '/postwarehousereleasepm', JSON.stringify(mappedEntity),

                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: WarehouseReleasePM;
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

    CancelRelease(entityPM: WarehouseReleasePM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("WarehouseRelease", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: WarehouseReleasePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl + '/PutCancelWarehouseReleasePM', JSON.stringify(mappedEntity),

                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: WarehouseReleasePM;
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
    
    GetCrossDockWorkspaceSummary(transportModeId: string, directionId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetCrossDockWorkspaceSummary?' + 'transportModeId=' + transportModeId + '&directionId=' + directionId, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: WarehouseReleasePM = null) {


        if (!entityPM) {

            entityPM = new WarehouseReleasePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldWarehouseReleasePackages: WarehouseReleasePackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldWarehouseReleasePackages = entityPM.OldEntityPM.WarehouseReleasePackages;
        }


        entityPM.WarehouseReleasePackages = new Array<WarehouseReleasePackagePM>();
        for (var item in jsonPM.WarehouseReleasePackages) {

            var jItem = jsonPM.WarehouseReleasePackages[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newWarehouseReleasePackagePM: WarehouseReleasePackagePM;
            if (mapParent) {
                newWarehouseReleasePackagePM = new WarehouseReleasePackagePM(entityPM);
            }
            else {
                newWarehouseReleasePackagePM = new WarehouseReleasePackagePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newWarehouseReleasePackagePM[pmProperty] = jItem[pmProperty];
            }
            newWarehouseReleasePackagePM.IsDirty = false;
            if (mapParent) {
                newWarehouseReleasePackagePM.OldEntityPM = this.clone(newWarehouseReleasePackagePM);
                newWarehouseReleasePackagePM.UniqueKey = Guid.newGuid();
                newWarehouseReleasePackagePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                 
            }
            else {

                if (newWarehouseReleasePackagePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newWarehouseReleasePackagePM.ChangeSetOp = "Update";
                }
                else {
                    newWarehouseReleasePackagePM.ChangeSetOp = "Insert";
                }

                newWarehouseReleasePackagePM.OldEntityPM = null;
                newWarehouseReleasePackagePM.EntityParentPM = null;
            }


            entityPM.WarehouseReleasePackages.push(newWarehouseReleasePackagePM);
        }

        if (oldWarehouseReleasePackages) {

            for (var itemKey in oldWarehouseReleasePackages) {
                if (entityPM.WarehouseReleasePackages.filter(p=> p.UniqueKey === oldWarehouseReleasePackages[itemKey].UniqueKey).length === 0) {

                    if (oldWarehouseReleasePackages[itemKey]) {
                        oldWarehouseReleasePackages[itemKey].ChangeSetOp = "Delete";
                        entityPM.WarehouseReleasePackages.push(oldWarehouseReleasePackages[itemKey]);
                    }
                }
            }
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.WarehouseReleasePackages = [];
            for (var m in entityPM.WarehouseReleasePackages) {
                entityPM.OldEntityPM.WarehouseReleasePackages.push(this.clone(entityPM.WarehouseReleasePackages[m]));
            }

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


}

