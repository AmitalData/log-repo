import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { DWQueryData } from '../../../Common/DataContracts/DWQueryData';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { DWObjectFieldPM } from '../../EntityPMs/DWObjectFieldPM';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { DWObjectFieldsDetails } from '../../../InfrastructureModules/InfrastructureBIReport/Components/Workspaces/DWQueryBuilderBaseComponent';

@Injectable()
export class DWQueryBuilderService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/dwquerybuilder';
    }

    GetDWQueryData(sql: string, table: string) {
        var url = this._apiUrl + "/GetDWQueryData" + '?SQL=' + MySql + '&Table=' + table;
        var MySql = encodeURIComponent(sql);
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = result;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetDWDataForDimTabel(Tabel: string, Field: string, SearchData: string) {
        var url = this._apiUrl + "/GetDWDataForDimTabel" + '?Tabel=' + Tabel + '&Field=' + Field + '&SearchData=' + SearchData;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = result;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    getByFilters(filters: ApiQueryFilters) {
        var callTime = new Date();
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;

        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl.concat(urlparameters);


        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse = response.body;
                    serviceResponse.CallTime = callTime;

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetNewDWQueryData(entityPM: DWQueryData) {

        return defer(() => {
            var errorsArray = [];
            if (errorsArray.length == 0) {
                var temp = this.deepClone(entityPM);
                var url = this._apiUrl + '/PostGetDWQueryData';
                return this._http.post(url, JSON.stringify(temp), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {

                    var result = response;
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = result;

                    return pmresponse;
                }), catchError(ServiceHelper.HandleServiceError));;
            }
            else {
                return null;
            }
        });
    }

    GetDateFilterSample(entityPM: DWObjectFieldsDetails) {

        return defer(() => {
            var errorsArray = [];
            if (errorsArray.length == 0) {
                var temp = this.deepClone(entityPM);
                var url = this._apiUrl + '/PostGetDateFilterSample';

                return this._http.post(url, JSON.stringify(temp), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {

                    var result = response;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = result;
                    return serviceResponse;

                }), catchError(ServiceHelper.HandleServiceError));;
            }
            else {
                return null;
            }
        });
    }

    insert(entityPM: DWQueryData) {

        return defer(() => {
            var errorsArray = [];
            if (errorsArray.length == 0) {
                var temp = this.deepClone(entityPM);
                var url = this._apiUrl + '/PostQueryData';

                return this._http.post(url, JSON.stringify(temp), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                    var pm = response;
                    if (pm) {}
                    return null;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                return null;
            }
        });
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
            entityPM.OldEntityPM = this.deepClone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;

        return entityPM;
    }

    public deepClone(obj, hash = new WeakMap()) {
        // Do not try to clone primitives or functions
        if (Object(obj) !== obj || obj instanceof Function) {
            return obj;
        }

        if (hash.has(obj)) {
            //return hash.get(obj); // Cyclic reference
            return;
        }

        try { // Try to run constructor (without arguments, as we don't know them)
            var result = new obj.constructor();
        }
        catch (e) { // Constructor failed, create object without running the constructor
            result = Object.create(Object.getPrototypeOf(obj));
        }

        // Optional: support for some standard constructors (extend as desired)
        if (obj instanceof Map) {
            Array.from(obj, ([key, val]) => result.set(this.deepClone(key, hash),
                this.deepClone(val, hash)));
        }
        else if (obj instanceof Set) {
            Array.from(obj, (key) => result.add(this.deepClone(key, hash)));
        }

        // Register in hash    
        hash.set(obj, result);

        // Clone and assign enumerable own properties recursively
        return Object.assign(result, ...Object.keys(obj).map(
            key => ({
                [key]:

                    key != "UIProperties" && key != "MyParentClass" && key != "ShowSampleDateCommand" && key != "Items" && key != "TooltipId" && key != "TooltipContentId" && key != "CurrentSession" ? this.deepClone(obj[key], hash) : true

            })));
    }

    CustomMapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entities: DWQueryData = null) {
        if (!entities) {
            entities = new DWQueryData();
            entities.Columns = [];
            //entities.Filters = 
        }

        ///////////

        //if (!entities) {

        //    entities = new PickListGeneralEntitiesArgs();
        //    entities.CustomPickListPMs = [];
        //    entities.RemovedCustomPickListPMs = [];
        //}

        //var jsonPMKeys = Object.keys(jsonPM);

        //for (var key in jsonPMKeys) {
        //    if (jsonPMKeys[key] === "UIProperties") {

        //        continue;
        //    }

        //    var property = jsonPMKeys[key];
        //    entities[property] = jsonPM[property];
        //}

        ///////////
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }

            var property = jsonPMKeys[key];
            entities[property] = jsonPM[property];
        }


        //entityPM.IsDirty = false;

        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entities;
    }

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "MyParentClass" || jsonPMKeys[key] === "ShowSampleDateCommand" || jsonPMKeys[key] === "Items" || jsonPMKeys[key] === "TooltipId" || jsonPMKeys[key] === "TooltipContentId" || jsonPMKeys[key] === "CurrentSession") {
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
