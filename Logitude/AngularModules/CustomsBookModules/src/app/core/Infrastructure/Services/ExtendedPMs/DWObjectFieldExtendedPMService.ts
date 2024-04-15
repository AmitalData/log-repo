import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { DWObjectFieldPM } from '../../EntityPMs/DWObjectFieldPM';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class DWObjectFieldExtendedPMService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/dwobjectfields';
    }

    getDWObjectFieldsByDWTableId(DWOTId: string) {
        var url = this._apiUrl + "/GetDWObjectFieldsByDWTableId" + '?DWOTId=' + DWOTId;

        return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
            var entity: DWObjectFieldPM;
            var DWObjectFieldPMLists: DWObjectFieldPM[];
            DWObjectFieldPMLists = new Array<DWObjectFieldPM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DWObjectFieldPMLists.push(entity);
            });

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = DWObjectFieldPMLists;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetDWObjectFieldsByDWTableIdGroupedByCategory(DWOTId: string) {
        var url = this._apiUrl + "/GetDWObjectFieldsByDWTableIdGroupedByCategory" + '?DWOTId=' + DWOTId;

        return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    getDWObjectFieldsWithChildrenByDWTableId(DWOTId: string)  {
        var url = this._apiUrl + "/getDWObjectFieldsWithChildrenByDWTableId" + '?DWOTId=' + DWOTId;
        return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
            var entity: DWObjectFieldPM;
            var DWObjectFieldPMLists: DWObjectFieldPM[];
            DWObjectFieldPMLists = new Array<DWObjectFieldPM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DWObjectFieldPMLists.push(entity);
            });

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DWObjectFieldPMLists;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: DWObjectFieldPM = null) {
        if (!entityPM) {
            entityPM = new DWObjectFieldPM();
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

        return entityPM;
    }

    public clone(jsonPM: any) {
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

    public GetNewEntityPM() {
        var entityPM: DWObjectFieldPM;
        entityPM = new DWObjectFieldPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }
}
