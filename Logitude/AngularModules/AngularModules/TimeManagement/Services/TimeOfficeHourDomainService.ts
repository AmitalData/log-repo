import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {TMOfficeHourPM} from '../EntityPMs/TMOfficeHourPM';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';

@Injectable()

export class TimeOfficeHourDomainService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TimeOfficeHourDomain';
    }

    GetTimeOfficeClock(employeeUserId: string, FromDate: Date, ToDate: Date) {

        var url = this._apiUrl + '/GetTimeOfficeClock?employeeUserId=' + employeeUserId + "&FromDate=" + ServiceHelper.GetDateString(FromDate) + "&ToDate=" + ServiceHelper.GetDateString(ToDate);
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

               var list:any = response;                
                var entity: Array<TMOfficeHourPM>=[];
                if (list) {
                    list.forEach(p => {
                        entity.push(this.MapJsonToEntityPM(p));
                    });
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;                             
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateOfficeHourList(entityPMList: TMOfficeHourPM[]) {
        return defer(() => {

            var mappedEntities: TMOfficeHourPM[] = [];
            entityPMList.forEach((item) => {
                var mappedEntity: TMOfficeHourPM = this.MapJsonToEntityPM(item);
                mappedEntities.push(mappedEntity);
            });

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntities), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TMOfficeHourPM = null) {
        if (!entityPM) {

            entityPM = new TMOfficeHourPM();
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
        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
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
