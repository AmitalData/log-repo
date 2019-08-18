"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../DataContracts/ServiceResponse");
var FeatureLocator_1 = require("../Utilities/FeatureLocator");
var FeaturePM_1 = require("../EntityPMs/FeaturePM");
var FeatureList_1 = require("../EntityLists/FeatureList");
var PackagePMService_1 = require("../../Common/Services/StandardPMs/PackagePMService");
var BusinessHourPM_1 = require("../EntityPMs/BusinessHourPM");
var BusinessHoursHolidayPM_1 = require("../EntityPMs/BusinessHoursHolidayPM");
var Guid_1 = require("../Utilities/Guid");
var CustomFieldClass_1 = require("../DataContracts/CustomFieldClass");
var TasksSchedulerPM_1 = require("../EntityPMs/TasksSchedulerPM");
var BIReportPM_1 = require("../EntityPMs/BIReportPM");
var ClassLevelValidator_1 = require("../Validators/ClassLevelValidator");
var FeatureToggleList_1 = require("../EntityLists/FeatureToggleList");
var InfrastructureDomainService = /** @class */ (function () {
    function InfrastructureDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InfrastructureDomain';
    }
    InfrastructureDomainService.prototype.UpdateLastFilter = function (myControlName, myFilterName, myFilterValue) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateLastFilter?myControlName=' + myControlName + '&myFilterName=' + myFilterName + '&myFilterValue=' + myFilterValue;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        }); //.share();
    };
    InfrastructureDomainService.prototype.GetMainMenuFollowups = function (objectTableName) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMainMenuFollowups?objectTableName=' + objectTableName;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetSelectedAndUnselectedRoleFeatures = function (RoleId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSelectedAndUnselectedRoleFeatures?RoleId=' + RoleId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                for (var key in listJason) {
                    var entity;
                    entity = _this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetSelectedAndUnselectedPackageFeatures = function (PackageCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSelectedAndUnselectedPackageFeatures?PackageCode=' + PackageCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                for (var key in listJason) {
                    var entity;
                    entity = _this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetAllowedFeaturesForLoggedUser = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllowedFeaturesForLoggedUser';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                for (var key in listJason) {
                    var entity;
                    entity = _this.MapJsonToFeaturePM(listJason[key]);
                    _mappedArray.push(entity);
                }
                FeatureLocator_1.FeatureLocator.Features = _mappedArray;
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetNewFeaturesList = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNewFeaturesList';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                for (var key in listJason) {
                    var entity;
                    entity = _this.MapJsonToFeatureList(listJason[key]);
                    _mappedArray.push(entity);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetPackagesBMs = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetPackagesBMs';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                var myPackagePMService = new PackagePMService_1.PackagePMService();
                for (var key in listJason) {
                    var entity = myPackagePMService.MapJsonToEntityPM(listJason[key]);
                    _mappedArray.push(entity);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = _mappedArray;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.SendEntityToAirlineTenant = function (entityId, objectTableName, airlineCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSendEntityToAirlineTenant';
        var url = this._apiUrl + '/GetSendEntityToAirlineTenant?entityId=' + entityId + '&objectTableName=' + objectTableName + '&airlineCode=' + airlineCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.UpdateFeatures = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToFeaturesUpdateHelper(entityPM, false);
            return _this._http.put(_this._apiUrl + "/PutFeatures", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToFeaturesUpdateHelper(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetBusinessHourBM = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBusinessHourBM';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToBusinessHourEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetBatchServicesLogs = function (serviceCode, dateFilterCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBatchServicesLogs?serviceCode=' + serviceCode + '&dateFilterCode=' + dateFilterCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetRoleFeaturesChanges = function (RoleId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRoleFeaturesChanges?RoleId=' + RoleId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetPackageFeaturesChanges = function (PackageCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetPackageFeaturesChanges?PackageCode=' + PackageCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetDataCountForTenant = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDataCountForTenant?entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new BusinessRecordsSummary();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.DeleteDataForTenant = function (entityId, type) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDeleteDataForTenant?entityId=' + entityId + '&type=' + type;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.ResetCountersForTenant = function (entityId, code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetResetCountersForTenant?entityId=' + entityId + '&code=' + code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.UpdateTenantSettings = function (DocumentFilingByEmailEnabled) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetPutTenantSettings?DocumentFilingByEmailEnabled=' + DocumentFilingByEmailEnabled;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.ResendAnalyzeQueue = function (AnalyzeQueueId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetResendAnalyzeQueue?AnalyzeQueueId=' + AnalyzeQueueId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var iResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = iResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.getDWObjectFieldsWithChildrenByDWTableId = function (DWOTId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var MyApi = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/dwobjectfields';
        return this._http.get(MyApi + "/getDWObjectFieldsWithChildrenByDWTableId" + '?DWOTId=' + DWOTId, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DWObjectFieldPMLists;
            DWObjectFieldPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DWObjectFieldPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DWObjectFieldPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    InfrastructureDomainService.prototype.MapJsonToBusinessHourEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new BusinessHourPM_1.BusinessHourPM();
        }
        var customFields = [];
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
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
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
                var newBusinessHoursHolidayPM = this.clone(myBusinessHoursHolidayPM);
                entityPM.OldEntityPM.BusinessHoursHolidays.push(newBusinessHoursHolidayPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    InfrastructureDomainService.prototype.MapBusinessHoursHolidays = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldBusinessHoursHolidays = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldBusinessHoursHolidays = entityPM.OldEntityPM.BusinessHoursHolidays;
        }
        entityPM.BusinessHoursHolidays = new Array();
        for (var item in jsonPM.BusinessHoursHolidays) {
            var jItem = jsonPM.BusinessHoursHolidays[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newBusinessHoursHolidayPM;
            if (mapParent) {
                newBusinessHoursHolidayPM = new BusinessHoursHolidayPM_1.BusinessHoursHolidayPM(entityPM);
            }
            else {
                newBusinessHoursHolidayPM = new BusinessHoursHolidayPM_1.BusinessHoursHolidayPM(null);
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
                newBusinessHoursHolidayPM.UniqueKey = Guid_1.Guid.newGuid();
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
                if (entityPM.BusinessHoursHolidays.filter(function (p) { return p.UniqueKey === oldBusinessHoursHolidays[itemKey].UniqueKey; }).length === 0) {
                    if (oldBusinessHoursHolidays[itemKey]) {
                        //oldBusinessHoursHolidays[itemKey].ChangeSetOp = "Delete";
                        //entityPM.BusinessHoursHolidays.push(oldBusinessHoursHolidays[itemKey]);
                        var oldItemJson = oldBusinessHoursHolidays[itemKey];
                        var deletedPM = new BusinessHoursHolidayPM_1.BusinessHoursHolidayPM(null);
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
    };
    InfrastructureDomainService.prototype.MapJsonToFeaturesUpdateHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
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
                entityPM.Items = new Array();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];
                    var newItemPM;
                    newItemPM = this.MapJsonToFeaturePM(jItem, getCallMap);
                    entityPM.Items.push(newItemPM);
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    InfrastructureDomainService.prototype.MapJsonToFeaturePM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new FeaturePM_1.FeaturePM();
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
    };
    //private MapJsonToPackagePM(jsonItem: any) {
    //    var entityPM: PackagePM = new PackagePM();
    //    var jsonItemKeys = Object.keys(jsonItem);
    //    for (var key in jsonItemKeys) {
    //        var property = jsonItemKeys[key];
    //        entityPM[property] = jsonItem[property];
    //    }
    //    return entityPM;
    //}
    InfrastructureDomainService.prototype.MapJsonToFeatureList = function (jsonItem) {
        var entityList = new FeatureList_1.FeatureList();
        var jsonItemKeys = Object.keys(jsonItem);
        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entityList[property] = jsonItem[property];
        }
        return entityList;
    };
    InfrastructureDomainService.prototype.clone = function (jsonPM) {
        var entityPM;
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
    };
    InfrastructureDomainService.prototype.GetAllTasksSchedulerPMs = function (schedulerType) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllTasksSchedulerPMs?schedulerType=' + schedulerType;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapTasksSchedulerPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.MapTasksSchedulerPM = function (jsonList) {
        var entityList;
        entityList = new TasksSchedulerPM_1.TasksSchedulerPM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    InfrastructureDomainService.prototype.GetTaskSchedulerHistory = function (taskId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTaskSchedulerHistory?taskId=' + taskId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.GetByBIReportId = function (Queryid, DWQueryId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetByBIReportId?' + 'Id=' + Queryid + '&dWQueryId=' + DWQueryId, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity = new BIReportXMLData();
                if (pm) {
                    entity.BIReportId = pm.BIReportId;
                    entity.BIReportPM = pm.BIReportPM;
                    entity.DWQueryData = pm.DWQueryData;
                    entity.BITabularViewSettings = pm.BITabularViewSettings;
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                var servertime = response.headers.get('ServerExecutionTime');
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.UpdateBIReportXMLData = function (QueryData) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var errorsArray = []; //validator.Validate("AdvancedQueryFilter", entityPM);
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(QueryData.BIReportPM, false);
                var temp = _this.deepClone(QueryData.DWQueryData);
                QueryData.BIReportPM = mappedEntity;
                QueryData.DWQueryData = temp;
                var temp2 = _this.deepClone(QueryData);
                /////////////////////////////////////////////////////
                return _this._http.put(_this._apiUrl + "/PutBIReport", JSON.stringify(temp2), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    var entity = new BIReportXMLData();
                    if (pm) {
                        entity = pm;
                    }
                    var serviceResponse;
                    serviceResponse = new ServiceResponse_1.ServiceResponse();
                    serviceResponse.Result = entity;
                    var servertime = res.headers.get('ServerExecutionTime');
                    return serviceResponse;
                });
            }
            else {
                return null;
            }
        });
    };
    InfrastructureDomainService.prototype.DeleteBIReport = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDeleteBIReport?' + 'Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var servertime = response.headers.get('ServerExecutionTime');
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.DeleteFolder = function (Id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDeleteFolder?' + 'Id=' + Id, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var servertime = response.headers.get('ServerExecutionTime');
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new BIReportPM_1.BIReportPM();
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
    };
    InfrastructureDomainService.prototype.deepClone = function (obj, hash) {
        var _this = this;
        if (hash === void 0) { hash = new WeakMap(); }
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
            Array.from(obj, function (_a) {
                var key = _a[0], val = _a[1];
                return result.set(_this.deepClone(key, hash), _this.deepClone(val, hash));
            });
        }
        else if (obj instanceof Set) {
            Array.from(obj, function (key) { return result.add(_this.deepClone(key, hash)); });
        }
        // Register in hash    
        hash.set(obj, result);
        // Clone and assign enumerable own properties recursively
        return Object.assign.apply(Object, [result].concat(Object.keys(obj).map(function (key) {
            var _a;
            return (_a = {},
                _a[key] = key != "UIProperties" && key != "MyParentClass" && key != "ShowSampleDateCommand" && key != "Items" && key != "TooltipId" && key != "TooltipContentId" && key != "CurrentSession" ? _this.deepClone(obj[key], hash) : true,
                _a);
        })));
    };
    InfrastructureDomainService.prototype.GetFeatureToggles = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetFeatureToggles';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToFeatureToggleList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    InfrastructureDomainService.prototype.MapJsonToFeatureToggleList = function (jsonItem) {
        var entityList = new FeatureToggleList_1.FeatureToggleList();
        var jsonItemKeys = Object.keys(jsonItem);
        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entityList[property] = jsonItem[property];
        }
        return entityList;
    };
    InfrastructureDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], InfrastructureDomainService);
    return InfrastructureDomainService;
}());
exports.InfrastructureDomainService = InfrastructureDomainService;
var FeaturesUpdateHelper = /** @class */ (function () {
    function FeaturesUpdateHelper() {
        this.Items = [];
    }
    return FeaturesUpdateHelper;
}());
exports.FeaturesUpdateHelper = FeaturesUpdateHelper;
var BusinessRecordsSummary = /** @class */ (function () {
    function BusinessRecordsSummary() {
    }
    return BusinessRecordsSummary;
}());
exports.BusinessRecordsSummary = BusinessRecordsSummary;
var BIReportXMLData = /** @class */ (function () {
    function BIReportXMLData() {
    }
    return BIReportXMLData;
}());
exports.BIReportXMLData = BIReportXMLData;
var BITabularViewSettings = /** @class */ (function () {
    function BITabularViewSettings() {
    }
    return BITabularViewSettings;
}());
exports.BITabularViewSettings = BITabularViewSettings;
var Column = /** @class */ (function () {
    function Column() {
    }
    return Column;
}());
exports.Column = Column;
//# sourceMappingURL=InfrastructureDomainService.js.map