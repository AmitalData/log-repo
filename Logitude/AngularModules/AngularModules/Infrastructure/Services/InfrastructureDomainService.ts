import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {FeatureLocator} from '../Utilities/FeatureLocator';
import {FeaturePM} from '../EntityPMs/FeaturePM';
import {FeatureList} from '../EntityLists/FeatureList';
import {PackagePM} from '../../Common/EntityPMs/PackagePM';
import {PackagePMService} from '../../Common/Services/StandardPMs/PackagePMService';
import {BusinessHourPM } from '../EntityPMs/BusinessHourPM';
import {BusinessHoursHolidayPM} from '../EntityPMs/BusinessHoursHolidayPM';
import {Guid} from '../Utilities/Guid';
import {CustomFieldClass} from '../DataContracts/CustomFieldClass'; 
import {TasksSchedulerPM} from '../EntityPMs/TasksSchedulerPM';

@Injectable()

export class InfrastructureDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InfrastructureDomain';
    }

    UpdateLastFilter(myControlName: string, myFilterName: string, myFilterValue: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpdateLastFilter?myControlName=' + myControlName + '&myFilterName=' + myFilterName + '&myFilterValue=' + myFilterValue;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myResult = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }); //.share();
    }
    GetMainMenuFollowups(objectTableName: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMainMenuFollowups?objectTableName=' + objectTableName;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetSelectedAndUnselectedRoleFeatures(RoleId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSelectedAndUnselectedRoleFeatures?RoleId=' + RoleId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var _mappedArray: Array<FeaturePM> = [];

                for (var key in listJason) {

                    var entity: FeaturePM;
                    entity = this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSelectedAndUnselectedPackageFeatures(PackageCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSelectedAndUnselectedPackageFeatures?PackageCode=' + PackageCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var _mappedArray: Array<FeaturePM> = [];

                for (var key in listJason) {

                    var entity: FeaturePM;
                    entity = this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAllowedFeaturesForLoggedUser() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllowedFeaturesForLoggedUser';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetNewFeaturesList() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetNewFeaturesList';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var _mappedArray: Array<FeatureList> = [];

                for (var key in listJason) {

                    var entity: FeatureList;
                    entity = this.MapJsonToFeatureList(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetPackagesBMs() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetPackagesBMs';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var _mappedArray: Array<PackagePM> = [];

                var myPackagePMService = new PackagePMService();

                for (var key in listJason) {

                    var entity: PackagePM = myPackagePMService.MapJsonToEntityPM(listJason[key]);
                    _mappedArray.push(entity);
                }

                var myResponse = new ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    SendEntityToAirlineTenant(entityId: string, objectTableName: string, airlineCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSendEntityToAirlineTenant';
        var url = this._apiUrl + '/GetSendEntityToAirlineTenant?entityId=' + entityId + '&objectTableName=' + objectTableName + '&airlineCode=' + airlineCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    UpdateFeatures(entityPM: FeaturesUpdateHelper) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: FeaturesUpdateHelper = this.MapJsonToFeaturesUpdateHelper(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: FeaturesUpdateHelper = this.MapJsonToFeaturesUpdateHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetBusinessHourBM() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBusinessHourBM';
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var pm = response.json();
                var entity: BusinessHourPM;
                if (pm) {
                    entity = this.MapJsonToBusinessHourEntityPM(pm);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetBatchServicesLogs(serviceCode: string, dateFilterCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetBatchServicesLogs?serviceCode=' + serviceCode + '&dateFilterCode=' + dateFilterCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetRoleFeaturesChanges(RoleId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRoleFeaturesChanges?RoleId=' + RoleId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetPackageFeaturesChanges(PackageCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetPackageFeaturesChanges?PackageCode=' + PackageCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetDataCountForTenant(entityId: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDataCountForTenant?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
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

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    DeleteDataForTenant(entityId: number, type: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        
        var url = this._apiUrl + '/GetDeleteDataForTenant?entityId=' + entityId + '&type=' + type;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    ResetCountersForTenant(entityId: number, code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetResetCountersForTenant?entityId=' + entityId + '&code=' + code;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    UpdateTenantSettings(DocumentFilingByEmailEnabled: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetPutTenantSettings?DocumentFilingByEmailEnabled=' + DocumentFilingByEmailEnabled;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    ResendAnalyzeQueue(AnalyzeQueueId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetResendAnalyzeQueue?AnalyzeQueueId=' + AnalyzeQueueId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var iResult = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = iResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
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

    GetAllTasksSchedulerPMs() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllTasksSchedulerPMs';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<TasksSchedulerPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TasksSchedulerPM = this.MapTasksSchedulerPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTaskSchedulerHistory?taskId=' + taskId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
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
