import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { AddressPM } from '../EntityPMs/AddressPM';
import { CustomFieldClass } from '../../Infrastructure/DataContracts/CustomFieldClass'

@Injectable()
export class PotentialAddressService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PotentialAddress';
    }

    AddAddress(entityPM: AddressPM) {
        var mappedEntity: AddressPM = this.MapJsonToAddressPM(entityPM, false);
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostAddress", JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: AddressPM = this.MapJsonToAddressPM(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    PutAddress(entityPM: AddressPM) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: AddressPM = this.MapJsonToAddressPM(entityPM, false);

            return this._http.put(this._apiUrl + "/PutAddress", JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;
                var mappedResult: AddressPM = this.MapJsonToAddressPM(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToAddressPM(jsonPM: any, mapParent: boolean = true, entityPM: AddressPM = null) {
        if (!entityPM) {

            entityPM = new AddressPM();
            entityPM.DisableMarkAsDirty = true;
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        entityPM.DisableMarkAsDirty = false;

        return entityPM;
    }
    private clone(jsonPM: any) {
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
