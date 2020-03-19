import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {TraceEventPM} from '../EntityPMs/TraceEventPM';
import {ExportToExcelArgs} from '../DataContracts/ExportToExcelArgs';
import { BIReportPM } from '../EntityPMs/BIReportPM';
import { BIReportXMLData } from '../Services/InfrastructureDomainService';


@Injectable()

export class WebFreightDomainService {
    private _apiUrl: string;
    private _http: Http
    constructor() {
        this._http = ServiceHelper.Http; 
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WebFreightDomain';
    }

    GetTraceEventsForEntity(objectTableId: string, entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = ServiceHelper.GetLogitudeURL() + 'api/TraceEventsDomain/GetTraceEventsForEntity?objectTableId=' + objectTableId + '&entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<TraceEventPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TraceEventPM = this.MapTraceEventPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    InsertTraceEvent(entityId: string, objectTableId: string, eventTypeId: string, eventDate: Date, notes: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        var args = new TraceEventsServiceArgs();
        args.EntityId = entityId;
        args.ObjectTableId = objectTableId;
        args.EventTypeId = eventTypeId;
        args.EventDate = eventDate;
        args.Notes = notes;

        var mappedArgs: TraceEventsServiceArgs = this.MapJsonTraceEventArgs(args);

        var url = ServiceHelper.GetLogitudeURL() + 'api/TraceEventsDomain';

        return Observable.defer(() => {
            return this._http.post(url, JSON.stringify(mappedArgs), { headers: authHeader }).map((response) => {
                var myJason = response.json();
                var myResult: NewTraceEventResult = this.MapNewTraceEventResult(myJason);

                var myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    DeleteTraceEvent(entityId: string, objectTableId: string, traceEventId: string, isExternal: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        var args = new TraceEventsServiceArgs();
        args.EntityId = entityId;
        args.ObjectTableId = objectTableId;
        args.TraceEventId = traceEventId;
        args.IsExternal = isExternal;

        var mappedArgs: TraceEventsServiceArgs = this.MapJsonTraceEventArgs(args);

        var url = ServiceHelper.GetLogitudeURL() + 'api/TraceEventsDomain';

        return Observable.defer(() => {
            return this._http.put(url + "/PutDeleteTraceEvent", JSON.stringify(mappedArgs), { headers: authHeader }).map((response) => {
                var myResult = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    private MapJsonTraceEventArgs(json: any, args: TraceEventsServiceArgs = null) {
        if (!args) {
            args = new TraceEventsServiceArgs();
        }

        var jsonPMKeys = Object.keys(json);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else {
                args[property] = json[property];
            }
        }

        return args;
    }
    private MapTraceEventPM(jsonList: any) {
        var entityPM: TraceEventPM;
        entityPM = new TraceEventPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonList[property];
        }

        return entityPM;
    }
    private MapNewTraceEventResult(jsonList: any) {
        var entityPM: NewTraceEventResult;
        entityPM = new NewTraceEventResult();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonList[property];
        }

        return entityPM;
    }
    getExcelData(filters: ApiQueryFilters, queryCode: string, tenant: number, userid: string, ObjectTableName: string) {
        if (filters == null) {
            filters.GetCount = true;
            filters.PageIndex = 0;
            filters.PageSize = 30;
            filters.SortBy = "";
            filters.SortDirection = ""; 
        }
        filters.queryCode = queryCode;
        filters.Tenant = tenant;
        filters.userid = userid;
        filters.ObjectTableName = ObjectTableName;
        var urlparameters = '/getquerytoexceldata?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var j = 1;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            //if (urlparameters != "?") {
            //    urlparameters = urlparameters.concat('&');
            //}
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));

            if (j < mykeys.length && !ignoreFilter) {
                urlparameters = urlparameters.concat('&');
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
            j++;

        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
       
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                if (response.ok == true) {
                    var viewResponse = response.json();
                    if (viewResponse != "Faild") {
                        return viewResponse;
                    }
                    else {
                        return "Faild";
                    }
                    
                }
                else {
                    return "Faild";
                }
            });
        }
        );
    }

    GetExportBIReportToExcel(QueryData: BIReportXMLData) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);
            var response: ServiceResponse;
            response = new ServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: BIReportPM;
                mappedEntity = this.MapJsonToEntityPM(QueryData.BIReportPM, false);
                var temp = this.deepClone(QueryData.DWQueryData);
                QueryData.BIReportPM = mappedEntity;
                QueryData.DWQueryData = temp;
                var temp2 = this.deepClone(QueryData);
                return this._http.put(this._apiUrl + "/PutExportBIReportToExcelByWR", JSON.stringify(QueryData),
                    { headers: authHeader }).map((res) => {
                        var entity = res.json();
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;
                        var servertime = res.headers.get('ServerExecutionTime');
                        return serviceResponse;
                    });
            }
            else {
                return null;
            }
        }
        );
    }
    GetBIReportLogStatus(reportId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBIReportLogStatus?reportId=' + reportId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var report = response.json();
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = report;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: BIReportPM = null) {

        if (!entityPM) {

            entityPM = new BIReportPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
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

    getHypridPartnerLogo(logoId: string) {
        var urlparameters = '/GetHypridPartnerLogo?LogoId=' + logoId;
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {
                
                    return response.json();
                    
            });
        }

        );

    }

    DownLoadAllFilesForShipments(ShipmentId: string, ObjectTableId: string, tenant: number) {
        
        var urlparameters = '/DownLoadAllFilesForShipments?ShipmentId=' + ShipmentId + "&&ObjectTableId=" + ObjectTableId + "&&Tenant=" + tenant;
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                return response.json();

            });
        }

        );
    }
}

export class TraceEventsServiceArgs {
    public EntityId: string;
    public ObjectTableId: string;
    public EventTypeId: string;
    public TraceEventId: string;
    public Notes: string;
    public EventDate: Date = null;
    public IsExternal: boolean = false;
}

export class NewTraceEventResult {
    public EntityId: string;
    public StatusId: string;
    public StatusName: string;
    public StatusLocation: string;
    public StatusDate: Date;
    public LastStatusLogDate: Date;
    public LogDateTime: Date;
    public StatusChanged: boolean;
    public LastSharedEventId: string;
    public LastSharedEventLocation: string;
    public LastSharedEventNotes: string;
    public LastSharedEventDate: Date;
}
