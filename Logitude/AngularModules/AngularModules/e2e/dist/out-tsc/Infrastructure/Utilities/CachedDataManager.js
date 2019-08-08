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
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var web_worker_service_1 = require("../WebWorker/web-worker.service");
var ApiQueryFilters_1 = require("../DataContracts/ApiQueryFilters");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var Rx_1 = require("rxjs/Rx");
var http_1 = require("@angular/http");
var MetaDataLastUpdateDates_1 = require("../Others/MetaDataLastUpdateDates");
var ObjectTableLastUpdatePM_1 = require("../Others/ObjectTableLastUpdatePM");
var LocalStorageManager_1 = require("./LocalStorageManager");
var core_1 = require("@angular/core");
var ObjectsLocator_1 = require("../Locators/ObjectsLocator");
var EntityResourceService_1 = require("../Services/EntityResourceService");
;
var CachedDataManagerServices_1 = require("./CachedDataManagerServices");
var CachedDataManager = /** @class */ (function () {
    function CachedDataManager() {
    }
    CachedDataManager.GetClosedTableData = function (objectTableName) {
        if (CachedDataManager.TablesLoadQueue[objectTableName]) {
            // if already exists in the load stack return the same obs
            return CachedDataManager.TablesLoadQueue[objectTableName].share();
        }
        var observable;
        console.time("Unzipping ClosedTable Data from local cache for: " + objectTableName);
        var storagefileName = objectTableName + "_ClosedData.zip";
        var fileString = LocalStorageManager_1.LocalStorageManager.GetItem(storagefileName);
        if (fileString == null || fileString == undefined)
            fileString = EntityResourceService_1.EntityResourceService.ClosedTablesDataZipFilesDictionary[storagefileName];
        if (fileString != null) {
            observable = Rx_1.Observable.create(function (observer) {
                if (fileString != null) {
                    var fileData = ServiceHelper_1.ServiceHelper.base64ToBufferConvertor(fileString);
                    var data = new ZipWorkerMessage();
                    data.FileData = fileData;
                    data.FileType = "string";
                    var worker = new web_worker_service_1.WebWorkerService();
                    //const promises = [];
                    //promises.push(worker.runUrl('Infrastructure/WebWorker/JSZipWebWorker.js', data));
                    var promise = worker.runUrl('Js/jszip-web-worker.js', data);
                    promise.then(function (unzippedFiles) {
                        var file = JSON.parse(unzippedFiles)[0]; // only one file in the result
                        var list = JSON.parse(file.FileData);
                        console.timeEnd("Unzipping ClosedTable Data from local cache for: " + objectTableName);
                        CachedDataManager.TablesLoadQueue[objectTableName] = null;
                        worker.terminate(promise);
                        observer.next(list);
                    })
                        .catch(function (error) {
                        console.error(error);
                    });
                }
                else {
                    CachedDataManager.TablesLoadQueue[objectTableName] = null;
                    console.error("couldn't find closed table data for " + objectTableName + ".zip file in the cache!");
                    observer.next(0);
                }
            }).share();
        }
        else {
            console.log("couldn't find closed table data for " + objectTableName + ".zip file in the cache!");
            observable = Rx_1.Observable.defer(function () {
                console.log("Calling Server For " + objectTableName + " Closed Data & MetaData");
                var entityResourceService = new EntityResourceService_1.EntityResourceService();
                EntityResourceService_1.EntityResourceService.TablesLoadQueue[objectTableName] = null;
                return entityResourceService.getEntityResourceByTableName(objectTableName, 0).flatMap(function (response) {
                    //return CachedDataManager.GetClosedTableData(objectTableName);
                    var storagefileName = objectTableName + "_ClosedData.zip";
                    var fileString = LocalStorageManager_1.LocalStorageManager.GetItem(storagefileName);
                    if (fileString == null || fileString == undefined)
                        fileString = EntityResourceService_1.EntityResourceService.ClosedTablesDataZipFilesDictionary[storagefileName];
                    if (fileString != null) {
                        return Rx_1.Observable.create(function (observer) {
                            if (fileString != null) {
                                var fileData = ServiceHelper_1.ServiceHelper.base64ToBufferConvertor(fileString);
                                var data = new ZipWorkerMessage();
                                data.FileData = fileData;
                                data.FileType = "string";
                                var worker = new web_worker_service_1.WebWorkerService();
                                //const promises = [];
                                //promises.push(worker.runUrl('Infrastructure/WebWorker/JSZipWebWorker.js', data));
                                var promise = worker.runUrl('Js/jszip-web-worker.js', data);
                                promise.then(function (unzippedFiles) {
                                    var file = JSON.parse(unzippedFiles)[0]; // only one file in the result
                                    var list = JSON.parse(file.FileData);
                                    console.timeEnd("Unzipping ClosedTable Data from local cache for: " + objectTableName);
                                    CachedDataManager.TablesLoadQueue[objectTableName] = null;
                                    worker.terminate(promise);
                                    observer.next(list);
                                })
                                    .catch(function (error) {
                                    console.error(error);
                                });
                            }
                            else {
                                CachedDataManager.TablesLoadQueue[objectTableName] = null;
                                console.error("couldn't find closed table data for " + objectTableName + ".zip file in the cache!");
                                observer.next(0);
                            }
                        });
                    }
                    else {
                        return Rx_1.Observable.of(0);
                    }
                });
            }).share();
        }
        /*

         
        */
        CachedDataManager.TablesLoadQueue[objectTableName] = observable;
        return observable;
    };
    CachedDataManager.GetCacheOnClientTablesData = function (entityListService) {
        for (var key in localStorage) {
            if (key.indexOf("_CachedData_") != -1) {
                var m = key.split('_');
                var storedTenant = +(m[m.length - 1]);
                if (storedTenant != SessionInfo_1.SessionInfo.LoggedUserTenant) {
                    LocalStorageManager_1.LocalStorageManager.RemoveItem(key);
                    console.log(key + " deleted from local storage");
                }
            }
            else {
                //console.log( key + " not deleted")
            }
        }
        //if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
        if (ObjectsLocator_1.ObjectsLocator != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
            CachedDataManager.AllCachedTables = window.ObjectTables.filter(function (d) { return d.CacheOnClient === true && d.ClientModuleName && d.IsClosed == false && d.Name.startsWith("Customs."); });
        }
        else {
            CachedDataManager.AllCachedTables = window.ObjectTables.filter(function (d) { return d.CacheOnClient === true && d.ClientModuleName && d.IsClosed == false && !d.Name.startsWith("Customs."); });
        }
        CachedDataManager.CachedTablesCount = CachedDataManager.AllCachedTables.length;
        CachedDataManager.CallsCount = 0;
        CachedDataManager.FinishedCallsCount = 0;
        var arr = CachedDataManager.AllCachedTables.splice(0, 5);
        CachedDataManager.CallCacheOnClientTablesData(entityListService, arr);
    };
    CachedDataManager.CallCacheOnClientTablesData = function (entityListService, tablesCalls) {
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filters.GetAll = true;
        var promises = [];
        //var chachedTables = [];//window.ObjectTables.filter(d => d.CacheOnClient === true && d.ClientModuleName && d.IsClosed == false);
        for (var k in tablesCalls) {
            if (tablesCalls[k].ClientModuleName) {
                //promises.push(entityListService.getAllFromCache(chachedTables[k].Name, filters)).then(res=> {
                try {
                    var myCachedDataManagerServices = new CachedDataManagerServices_1.CachedDataManagerServices();
                    myCachedDataManagerServices.getAllFromCache(tablesCalls[k].Name, filters).then(function (res) {
                        //entityListService.getAllFromCache(tablesCalls[k].Name, filters).then((res:any)=> {
                        res.subscribe(function (resp) {
                            if (resp.Data) {
                            }
                            else {
                            }
                            //console.log(resp.Data);
                            CachedDataManager.CheckFinishedCalls(entityListService);
                        }, function (error) {
                            console.error(error);
                            CachedDataManager.CheckFinishedCalls(entityListService);
                        }); //.catch(CachedDataManager.HandleServiceError);
                    }, function (err) {
                        console.error(err);
                        CachedDataManager.CheckFinishedCalls(entityListService);
                    }); //.catch(CachedDataManager.HandleServiceError);
                }
                catch (e) {
                    CachedDataManager.CheckFinishedCalls(entityListService);
                    console.error(e);
                }
            }
        }
    };
    CachedDataManager.CheckFinishedCalls = function (entityListService) {
        CachedDataManager.FinishedCallsCount++;
        CachedDataManager.CallsCount++;
        if (CachedDataManager.CallsCount == 5) {
            CachedDataManager.CallsCount = 0;
            var arr = CachedDataManager.AllCachedTables.splice(0, 5);
            CachedDataManager.CallCacheOnClientTablesData(entityListService, arr);
        }
        if (CachedDataManager.FinishedCallsCount == CachedDataManager.CachedTablesCount) {
            //"CachedTableLastUpdateDate" + tenant.Id
            var cacheKey = "CachedTableLastUpdateDate" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            var stored = LocalStorageManager_1.LocalStorageManager.GetItem(cacheKey);
            if (!stored) {
                var authHeader = new http_1.Headers();
                authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
                return ServiceHelper_1.ServiceHelper.Http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ObjectTableLastUpdate/GetLastTableUpdateDate/?' + 'tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant, {
                    headers: authHeader
                }).subscribe(function (resp) {
                    var lastdate = resp.json();
                    LocalStorageManager_1.LocalStorageManager.SetItem("CachedTableLastUpdateDate" + SessionInfo_1.SessionInfo.LoggedUserTenant, JSON.stringify(lastdate));
                    console.log(lastdate);
                });
            }
            console.log("loading cached on client tables finished! (" + CachedDataManager.CachedTablesCount + ")");
        }
    };
    CachedDataManager.HandleServiceError = function (error) {
    };
    CachedDataManager.CheckSystemMetadataLastUpdate = function () {
        // window.on("itemRemoved", function (e, args) {
        //     console.log(e, args);
        //     if (!localStorage.getItem(args)) {
        //         // do stuff
        //     }
        // })
        //let fn = localStorage.removeItem;
        // localStorage.removeItem = function () {
        //     var args = arguments;
        //     setTimeout(function () {
        //         window.trigger("itemRemoved", [args[0]])
        //     });
        //     return fn.call(localStorage, args[0]);
        // }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return ServiceHelper_1.ServiceHelper.Http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/SystemMetadataLastUpdate/GetSystemMetadataLastUpdates/?' + 'tenant=' + 0, {
                headers: authHeader
            }).flatMap(function (response) {
                return Rx_1.Observable.create(function (observer) {
                    var list = response.json();
                    var serverlastUpdates;
                    var clientlastUpdates;
                    serverlastUpdates = CachedDataManager.MapJsonToClassEntity(list, MetaDataLastUpdateDates_1.MetaDataLastUpdateDates);
                    var stored = LocalStorageManager_1.LocalStorageManager.GetItem("SystemMetadataLastUpdate");
                    if (stored) {
                        var result = JSON.parse(stored);
                        clientlastUpdates = CachedDataManager.MapJsonToClassEntity(result, MetaDataLastUpdateDates_1.MetaDataLastUpdateDates);
                        var m = clientlastUpdates.ObjectFieldsSystemUpdateDateGMT;
                        if (clientlastUpdates.ObjectFieldsSystemUpdateDateGMT != serverlastUpdates.ObjectFieldsSystemUpdateDateGMT
                            || clientlastUpdates.TranslationsSystemUpdateDateGMT != serverlastUpdates.TranslationsSystemUpdateDateGMT) {
                            for (var key in localStorage) {
                                if (key.indexOf("ObjectFields") != -1 || key.indexOf("TextCodes") != -1 || key.indexOf("ClosedData") != -1) {
                                    LocalStorageManager_1.LocalStorageManager.RemoveItem(key);
                                    console.log("%c" + key + " deleted from local storage (System Metadata Refresh)", 'background: #ffd966; color: brown');
                                }
                                else {
                                    //console.log( key + " not deleted")
                                }
                            }
                            //setTimeout(() => {
                            for (var key in localStorage) {
                                if (key.indexOf("ObjectFields") != -1 || key.indexOf("TextCodes") != -1 || key.indexOf("ClosedData") != -1) {
                                    console.warn(key + " is not deleted from local storage after System Metadata Refresh !!!");
                                }
                            }
                            LocalStorageManager_1.LocalStorageManager.SetItem("SystemMetadataLastUpdate", JSON.stringify(list));
                            console.log("%c -------------      SystemMetadataLastUpdate has been updated!      ---------------", 'background: #222; color: #bada55');
                            observer.next(list);
                            // }, 3000);
                        }
                        else {
                            observer.next(list);
                            //return list;
                        }
                    }
                    else {
                        LocalStorageManager_1.LocalStorageManager.SetItem("SystemMetadataLastUpdate", JSON.stringify(list));
                        observer.next(list);
                        //return list;
                    }
                    //return list;
                });
            }).catch(ServiceHelper_1.ServiceHelper.HandleTimerServiceError);
        });
    };
    CachedDataManager.CheckCachedTableLastUpdateDate = function () {
        CachedDataManager.needTobeUpdatedTablesList = [];
        CachedDataManager.updatedCachedTablesCount = 0;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            var cacheKey = "CachedTableLastUpdateDate" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            var storedDate = LocalStorageManager_1.LocalStorageManager.GetItem(cacheKey);
            if (storedDate) {
                return ServiceHelper_1.ServiceHelper.Http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ObjectTableLastUpdate/GetLastUpdatedTables/?' + 'tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant + '&sinceDate=' + JSON.parse(storedDate) + '&clientEmail=' + SessionInfo_1.SessionInfo.LoggedUserEmail, {
                    headers: authHeader
                }).map(function (response) {
                    var list = response.json();
                    //CachedDataManager.needTobeUpdatedTablesList = CachedDataManager.MapJsonToEntityList<ObjectTableLastUpdatePM>(list, ObjectTableLastUpdatePM);
                    //for (var key in cachedJson) {
                    //    var entity: IncotermList;
                    //    entity = this.MapJsonToEntityList(cachedJson[key]);
                    //    _mappedListsArray.push(entity);
                    //}
                    if (list) {
                        CachedDataManager.needTobeUpdatedTablesList = CachedDataManager.MapJsonToEntityList(list, ObjectTableLastUpdatePM_1.ObjectTableLastUpdatePM);
                        for (var key in CachedDataManager.needTobeUpdatedTablesList) {
                            CachedDataManager.RefreshTableData(list[key].ObjectTableName);
                        }
                    }
                    return list;
                }).catch(ServiceHelper_1.ServiceHelper.HandleTimerServiceError);
            }
        });
    };
    CachedDataManager.RefreshTableData = function (tableName, holdTimer) {
        if (holdTimer === void 0) { holdTimer = false; }
        console.log("calling refresh for table: " + tableName);
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filters.GetAll = true;
        filters.ForceCacheRefresh = true;
        //var entityListService = new EntityListService();
        //entityListService.getAllFromCache(tableName, filters).then((res:any)=> {
        var myCachedDataManagerServices = new CachedDataManagerServices_1.CachedDataManagerServices();
        myCachedDataManagerServices.getAllFromCache(tableName, filters).then(function (res) {
            res.subscribe(function (resp) {
                CachedDataManager.CheckUpdatedCachedTables(holdTimer);
                //CachedDataManager.CheckFinishedCalls(entityListService);
            }, function (error) {
                CachedDataManager.CheckUpdatedCachedTables(holdTimer);
                //console.error(error);
                //CachedDataManager.CheckFinishedCalls(entityListService);
            });
        }, function (err) {
            CachedDataManager.CheckUpdatedCachedTables(holdTimer);
            //CachedDataManager.CheckFinishedCalls(entityListService);
        });
    };
    CachedDataManager.CheckUpdatedCachedTables = function (holdTimer) {
        CachedDataManager.updatedCachedTablesCount++;
        if (CachedDataManager.updatedCachedTablesCount == CachedDataManager.needTobeUpdatedTablesList.length && !holdTimer) {
            LocalStorageManager_1.LocalStorageManager.SetItem("CachedTableLastUpdateDate" + SessionInfo_1.SessionInfo.LoggedUserTenant, JSON.stringify(CachedDataManager.needTobeUpdatedTablesList[0].LastUpdateDate));
            // CachedDataManager.needTobeUpdatedTablesList.sort(function (a, b) {
            // Turn your strings into dates, and then subtract them
            // to get a value that is either negative, positive, or zero.
            // return (b.LastUpdateDate - a.LastUpdateDate);
            // });
            CachedDataManager.updatedCachedTablesCount = 0;
            CachedDataManager.needTobeUpdatedTablesList = [];
        }
        CachedDataManager.RefreshCompleted.emit(true);
        holdTimer = false;
    };
    CachedDataManager.MapJsonToClassEntity = function (jsonList, classType) {
        var entityList;
        entityList = new classType();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CachedDataManager.MapJsonToEntityList = function (jsonArray, classType) {
        var mappedArray = [];
        for (var key in jsonArray) {
            var entity = jsonArray[key];
            mappedArray.push(CachedDataManager.MapJsonToClassEntity(entity, classType));
        }
        return mappedArray;
    };
    CachedDataManager.RefreshTenantTextCodes = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ngMetaData/GetTenantTextCodes?tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant;
        return Rx_1.Observable.defer(function () {
            return ServiceHelper_1.ServiceHelper.Http.get(url, { headers: authHeader }).map(function (response) {
                var newList = response.json();
                for (var k in newList) {
                    var item = newList[k];
                    var oldItem = window.TextCodes.filter(function (t) { return t.Id == item.Id; })[0];
                    if (oldItem) {
                        var index = window.TextCodes.indexOf(oldItem);
                        window.TextCodes.splice(index, 1);
                    }
                    window.TextCodes.push(item);
                    var oldItemCached = window.TextCodesCache.filter(function (t) { return t.Id == item.Id; })[0];
                    if (oldItemCached) {
                        var index = window.TextCodesCache.indexOf(oldItemCached);
                        window.TextCodesCache.splice(index, 1);
                    }
                    window.TextCodesCache.push(item);
                    //var index = window.TextCodes.indexOf(item);
                    //if (index > -1) {
                    //    window.TextCodes.splice(index, 1);
                    //}
                    //window.TextCodes.push(item);
                    //var oldItem = window.TextCodes.filter(t=> t.Id == item.Id)[0];
                    //if (oldItem) {
                    //    var a = [];
                    //    a.
                    //    //window.TextCodes.
                    //}
                    //else {
                    //}
                }
                return response.json();
            });
        });
    };
    CachedDataManager.RefreshTenantObjectFields = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ngMetaData/GetTenantObjectFields?loggedTenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant;
        return Rx_1.Observable.defer(function () {
            return ServiceHelper_1.ServiceHelper.Http.get(url, { headers: authHeader }).map(function (response) {
                var newList = response.json();
                for (var k in newList) {
                    var item = newList[k];
                    var oldItem = window.ObjectFields.filter(function (t) { return t.Id == item.Id; })[0];
                    if (oldItem) {
                        var index = window.ObjectFields.indexOf(oldItem);
                        window.ObjectFields.splice(index, 1);
                    }
                    window.ObjectFields.push(item);
                    var oldItemCached = window.TextCodesCache.filter(function (t) { return t.Id == item.Id; })[0];
                    if (oldItemCached) {
                        var index = window.ObjectFieldsCache.indexOf(oldItemCached);
                        window.ObjectFieldsCache.splice(index, 1);
                    }
                    window.ObjectFieldsCache.push(item);
                }
                return response.json();
            });
        });
    };
    CachedDataManager.TablesLoadQueue = {};
    CachedDataManager.FinishedTables = [];
    CachedDataManager.AllCachedTables = [];
    CachedDataManager.CallsCount = 0;
    CachedDataManager.FinishedCallsCount = 0;
    CachedDataManager.CachedTablesCount = 0;
    CachedDataManager.needTobeUpdatedTablesList = [];
    CachedDataManager.updatedCachedTablesCount = 0;
    CachedDataManager.RefreshCompleted = new core_1.EventEmitter();
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CachedDataManager, "RefreshCompleted", void 0);
    return CachedDataManager;
}());
exports.CachedDataManager = CachedDataManager;
var ZipWorkerMessage = /** @class */ (function () {
    function ZipWorkerMessage() {
    }
    return ZipWorkerMessage;
}());
exports.ZipWorkerMessage = ZipWorkerMessage;
//# sourceMappingURL=CachedDataManager.js.map