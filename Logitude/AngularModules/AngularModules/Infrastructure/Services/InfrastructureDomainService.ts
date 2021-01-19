import { PackagePMService } from '../../Common/Services/StandardPMs/PackagePMService';
import { BusinessHoursHolidayPM } from '../EntityPMs/BusinessHoursHolidayPM';
import { ClassLevelValidator } from '../Validators/ClassLevelValidator';
import { DWQueryData } from '../../Common/DataContracts/DWQueryData';
import { CustomFieldClass } from '../DataContracts/CustomFieldClass'; 
import { FeatureToggleList } from '../EntityLists/FeatureToggleList';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { TasksSchedulerPM } from '../EntityPMs/TasksSchedulerPM';
import { FeatureLocator } from '../Utilities/FeatureLocator';
import { BusinessHourPM } from '../EntityPMs/BusinessHourPM';
import { PackagePM } from '../../Common/EntityPMs/PackagePM';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { FeatureList } from '../EntityLists/FeatureList';
import { BIReportPM } from '../EntityPMs/BIReportPM';
import { FeaturePM } from '../EntityPMs/FeaturePM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Guid } from '../Utilities/Guid';
import { defer, of } from 'rxjs';

@Injectable()
export class InfrastructureDomainService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InfrastructureDomain';
    }

    UpdateLastFilter(myControlName: string, myFilterName: string, myFilterValue: string) {
        var url = this._apiUrl + '/GetUpdateLastFilter?myControlName=' + myControlName + '&myFilterName=' + myFilterName + '&myFilterValue=' + myFilterValue;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetMainMenuFollowups(objectTableName: string) {
        var url = this._apiUrl + '/GetMainMenuFollowups?objectTableName=' + objectTableName;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSelectedAndUnselectedRoleFeatures(RoleId: string) {
        var url = this._apiUrl + '/GetSelectedAndUnselectedRoleFeatures?RoleId=' + RoleId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<FeaturePM> = [];

                for (var key in listJason) {

                    var entity: FeaturePM;
                    entity = this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSelectedAndUnselectedPackageFeatures(PackageCode: string) {
        var url = this._apiUrl + '/GetSelectedAndUnselectedPackageFeatures?PackageCode=' + PackageCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<FeaturePM> = [];

                for (var key in listJason) {

                    var entity: FeaturePM;
                    entity = this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAllowedFeaturesForLoggedUser() {
        var url = this._apiUrl + '/GetAllowedFeaturesForLoggedUser';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<FeaturePM> = [];

                for (var key in listJason) {

                    var entity: FeaturePM;
                    entity = this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }

                FeatureLocator.Features = _mappedArray;

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetNewFeaturesList() {
        var url = this._apiUrl + '/GetNewFeaturesList';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<FeatureList> = [];

                for (var key in listJason) {

                    var entity: FeatureList;
                    entity = this.MapJsonToFeatureList(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetPackagesBMs() {
        var url = this._apiUrl + '/GetPackagesBMs';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var _mappedArray: Array<PackagePM> = [];

                var myPackagePMService = new PackagePMService();

                for (var key in listJason) {

                    var entity: PackagePM = myPackagePMService.MapJsonToEntityPM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    SendEntityToAirlineTenant(entityId: string, objectTableName: string, airlineCode: string) {
        var url = this._apiUrl + '/GetSendEntityToAirlineTenant?entityId=' + entityId + '&objectTableName=' + objectTableName + '&airlineCode=' + airlineCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateFeatures(entityPM: FeaturesUpdateHelper) {
        return defer(() => {
            var mappedEntity: FeaturesUpdateHelper = this.MapJsonToFeaturesUpdateHelper(entityPM, false);

            return this._http.put(this._apiUrl + "/PutFeatures", JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var myJsonResult = response;

                var mappedResult: FeaturesUpdateHelper = this.MapJsonToFeaturesUpdateHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetBusinessHourBM() {
        var url = this._apiUrl + '/GetBusinessHourBM';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;
                var entity: BusinessHourPM;
                if (pm) {
                    entity = this.MapJsonToBusinessHourEntityPM(pm);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetBatchServicesLogs(serviceCode: string, dateFilterCode: string) {
        var url = this._apiUrl + '/GetBatchServicesLogs?serviceCode=' + serviceCode + '&dateFilterCode=' + dateFilterCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetRoleFeaturesChanges(RoleId: string) {
        var url = this._apiUrl + '/GetRoleFeaturesChanges?RoleId=' + RoleId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetPackageFeaturesChanges(PackageCode: string) {
        var url = this._apiUrl + '/GetPackageFeaturesChanges?PackageCode=' + PackageCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDataCountForTenant(entityId: number) {
        var url = this._apiUrl + '/GetDataCountForTenant?entityId=' + entityId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;
                var myResult = new BusinessRecordsSummary();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DeleteDataForTenant(entityId: number, type: string) {
        var url = this._apiUrl + '/GetDeleteDataForTenant?entityId=' + entityId + '&type=' + type;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    ResetCountersForTenant(entityId: number, code: string) {
        var url = this._apiUrl + '/GetResetCountersForTenant?entityId=' + entityId + '&code=' + code;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateTenantSettings(DocumentFilingByEmailEnabled: boolean) {
        var url = this._apiUrl + '/GetPutTenantSettings?DocumentFilingByEmailEnabled=' + DocumentFilingByEmailEnabled;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    ResendAnalyzeQueue(AnalyzeQueueId: string) {
        var url = this._apiUrl + '/GetResendAnalyzeQueue?AnalyzeQueueId=' + AnalyzeQueueId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var iResult = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = iResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getDWObjectFieldsWithChildren() {
        var myUrl = ServiceHelper.GetLogitudeURL() + 'api/dwobjectfields/getDWObjectFieldsWithChildren';

        return this._http.get(myUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result: any = response;
            var entity: any;
            var DWObjectFieldPMLists: any[];
            DWObjectFieldPMLists = new Array<any>();

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

    getDWObjectFieldsWithChildrenByDWTableId(DWOTId: string) {
        var myUrl = ServiceHelper.GetLogitudeURL() + 'api/dwobjectfields/getDWObjectFieldsWithChildrenByDWTableId' + '?DWOTId=' + DWOTId;

        return this._http.get(myUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result: any = response;
            var entity: any;
            var DWObjectFieldPMLists: any[];
            DWObjectFieldPMLists = new Array<any>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DWObjectFieldPMLists.push(entity);
            });

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DWObjectFieldPMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    private MapJsonToBusinessHourEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: BusinessHourPM = null) {
        if (!entityPM) {
            entityPM = new BusinessHourPM();
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

        this.MapBusinessHoursHolidays(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.BusinessHoursHolidays = [];
            for (var item in entityPM.BusinessHoursHolidays) {
                var myBusinessHoursHolidayPM = entityPM.BusinessHoursHolidays[item];
                var newBusinessHoursHolidayPM: BusinessHoursHolidayPM = this.clone(myBusinessHoursHolidayPM);


                entityPM.OldEntityPM.BusinessHoursHolidays.push(newBusinessHoursHolidayPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    private MapBusinessHoursHolidays(entityPM: BusinessHourPM, jsonPM: any, mapParent: boolean = true) {

        var oldBusinessHoursHolidays: BusinessHoursHolidayPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldBusinessHoursHolidays = entityPM.OldEntityPM.BusinessHoursHolidays;
        }

        entityPM.BusinessHoursHolidays = new Array<BusinessHoursHolidayPM>();
        for (var item in jsonPM.BusinessHoursHolidays) {
            var jItem = jsonPM.BusinessHoursHolidays[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newBusinessHoursHolidayPM: BusinessHoursHolidayPM;

            if (mapParent) {
                newBusinessHoursHolidayPM = new BusinessHoursHolidayPM(entityPM);
            }
            else {
                newBusinessHoursHolidayPM = new BusinessHoursHolidayPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newBusinessHoursHolidayPM[pmProperty] = jItem[pmProperty];
            }
            newBusinessHoursHolidayPM.IsDirty = false;

            if (mapParent) {
                newBusinessHoursHolidayPM.UniqueKey = Guid.newGuid();
                newBusinessHoursHolidayPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newBusinessHoursHolidayPM.OldEntityPM = this.clone(newBusinessHoursHolidayPM);


            }
            else {
                if (newBusinessHoursHolidayPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newBusinessHoursHolidayPM.ChangeSetOp = "Update";
                }
                else {
                    newBusinessHoursHolidayPM.ChangeSetOp = "Insert";
                }

                newBusinessHoursHolidayPM.OldEntityPM = null;
                newBusinessHoursHolidayPM.EntityParentPM = null;
            }


            entityPM.BusinessHoursHolidays.push(newBusinessHoursHolidayPM);
        }
        if (oldBusinessHoursHolidays) {

            for (var itemKey in oldBusinessHoursHolidays) {
                if (entityPM.BusinessHoursHolidays.filter(p => p.UniqueKey === oldBusinessHoursHolidays[itemKey].UniqueKey).length === 0) {

                    if (oldBusinessHoursHolidays[itemKey]) {
                        //oldBusinessHoursHolidays[itemKey].ChangeSetOp = "Delete";
                        //entityPM.BusinessHoursHolidays.push(oldBusinessHoursHolidays[itemKey]);
                        var oldItemJson = oldBusinessHoursHolidays[itemKey];
                        var deletedPM: BusinessHoursHolidayPM = new BusinessHoursHolidayPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.BusinessHoursHolidays.push(deletedPM);
                    }
                }
            }
        }
    }

    private MapJsonToFeaturesUpdateHelper(jsonPM: any, getCallMap: boolean = true, entityPM: FeaturesUpdateHelper = null) {
        if (!entityPM) {
            entityPM = new FeaturesUpdateHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "Items") {

                entityPM.Items = new Array<FeaturePM>();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];

                    var newItemPM: FeaturePM;
                    newItemPM = this.MapJsonToFeaturePM(jItem, getCallMap);
                    entityPM.Items.push(newItemPM);
                }
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }

    public MapJsonToFeaturePM(jsonPM: any, mapParent: boolean = true, entityPM: FeaturePM = null) {
        if (!entityPM) {
            entityPM = new FeaturePM();
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

    //private MapJsonToPackagePM(jsonItem: any) {
    //    var entityPM: PackagePM = new PackagePM();
    //    var jsonItemKeys = Object.keys(jsonItem);

    //    for (var key in jsonItemKeys) {
    //        var property = jsonItemKeys[key];
    //        entityPM[property] = jsonItem[property];
    //    }

    //    return entityPM;
    //}

    private MapJsonToFeatureList(jsonItem: any) {
        var entityList: FeatureList = new FeatureList();
        var jsonItemKeys = Object.keys(jsonItem);

        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entityList[property] = jsonItem[property];
        }

        return entityList;
    }

    private clone(jsonPM: any) {
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

    GetAllTasksSchedulerPMs(schedulerType:string) {
        var url = this._apiUrl + '/GetAllTasksSchedulerPMs?schedulerType=' + schedulerType;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<TasksSchedulerPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TasksSchedulerPM = this.MapTasksSchedulerPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapTasksSchedulerPM(jsonList: any) {
        var entityList: TasksSchedulerPM;
        entityList = new TasksSchedulerPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    GetTaskSchedulerHistory(taskId: string) {
        var url = this._apiUrl + '/GetTaskSchedulerHistory?taskId=' + taskId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetByBIReportId(Queryid: string, DWQueryId : string ) {
        var url = this._apiUrl + '/GetByBIReportId?' + 'Id=' + Queryid + '&dWQueryId=' + DWQueryId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm : any = response;
                var entity: BIReportXMLData = new BIReportXMLData();
                if (pm) {
                    entity.BIReportId = pm.BIReportId;
                    entity.BIReportPM = pm.BIReportPM;
                    entity.DWQueryData = pm.DWQueryData;
                    entity.BITabularViewSettings = pm.BITabularViewSettings;
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateBIReportXMLData(QueryData: BIReportXMLData) {
        return defer(() => {
            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);
            var response: ServiceResponse;
            response = new ServiceResponse();

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();

            if (errorsArray.length == 0) {
                var mappedEntity: BIReportPM;
                mappedEntity = this.MapJsonToEntityPM(QueryData.BIReportPM, false);
                var temp = this.deepClone(QueryData.DWQueryData);
                QueryData.BIReportPM = mappedEntity;
                QueryData.DWQueryData = temp;
                var temp2 = this.deepClone(QueryData);
                var url = this._apiUrl + "/PutBIReport";
                /////////////////////////////////////////////////////
                return this._http.put(url, JSON.stringify(temp2), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                        var pm : any = response;
                        var entity: BIReportXMLData = new BIReportXMLData();
                        if (pm) {
                            entity = pm;
                        }
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;
                        return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                return null;
            }
        });
    }

    DeleteBIReport(Id: string) {
        var url = this._apiUrl + '/GetDeleteBIReport?' + 'Id=' + Id;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DeleteFolder(Id: string) {
        var url = this._apiUrl + '/GetDeleteFolder?' + 'Id=' + Id;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: any = null) {

        if (!entityPM) {

            entityPM = new BIReportPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

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

    GetFeatureToggles() {
        var url = this._apiUrl + '/GetFeatureToggles';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<FeatureToggleList> = [];

                for (var key in allLists) {
                    var entity: FeatureToggleList;
                    entity = this.MapJsonToFeatureToggleList(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapJsonToFeatureToggleList(jsonItem: any) {
        var entityList: FeatureToggleList = new FeatureToggleList();
        var jsonItemKeys = Object.keys(jsonItem);

        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entityList[property] = jsonItem[property];
        }

        return entityList;
    }
}

export class FeaturesUpdateHelper {
    public Tenant: number;
    public RoleId: string;
    public PackageCode: string;
    public Items: FeaturePM[] = [];
}

export class BusinessRecordsSummary {
    public Id: number;
    public ShipmentsCount: number;
    public QuotesCount: number;
    public ARInvoicesCount: number;
    public APInvoicesCount: number;
    public ARPaymentsCount: number;
    public APPaymentsCount: number;
    public CustomersCount: number;
    public TicketsCount: number;
    public ActivitiesCount: number;
    public OpportunitiesCount: number;
}

export class BIReportXMLData {
    public BIReportId: string;
    public DWQueryData: DWQueryData; 
    public BIReportPM: BIReportPM;
    public BITabularViewSettings: BITabularViewSettings;
    public UserId: string;
    public BIReportKey: string;
    public IncludeTotals: boolean;
    public ExportDataType: string;
    public BIReportsExecutionLogId: string;

}

export class BITabularViewSettings {
    public Columns: Column[];  
}

export class Column {
    public Code: string;
    public Name: string;
    public SortDirction: string;
    public SortOrder: number;
    public Width: number;
    public Index: number;
    public IsChecked: boolean;
    public DataTypeCode: string; 
    public FieldCode: string; 

}
