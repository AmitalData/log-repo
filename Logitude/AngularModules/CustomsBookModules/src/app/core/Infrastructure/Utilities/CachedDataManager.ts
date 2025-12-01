// declare var System: any;
// declare var window: any;
// declare var JSZip: any;
// import {ServiceHelper} from '../Utilities/ServiceHelper';
// import {WebWorkerService} from '../WebWorker/web-worker.service';
// import {EntityListService} from '../Services/EntityListService';
// import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
// import {SessionInfo} from '../Utilities/SessionInfo';
// import {ServiceResponse} from '../DataContracts/ServiceResponse';
// import {MetaDataLastUpdateDates} from '../Others/MetaDataLastUpdateDates';
// import { ObjectTableLastUpdatePM } from '../Others/ObjectTableLastUpdatePM';
// import { LocalStorageManager } from './LocalStorageManager';
// import {SessionLocator} from './SessionLocator';
// import {Output, EventEmitter} from '@angular/core';
// import {ObjectsLocator} from '../Locators/ObjectsLocator';
// import {EntityResourceService} from '../Services/EntityResourceService';;
// import {CachedDataManagerServices} from './CachedDataManagerServices';
// import { GeneralDomainService } from '../Services/GeneralDomainService';
// import { Observable, defer, of}     from 'rxjs';
// import { map, catchError, share, flatMap } from 'rxjs/operators';

// export class CachedDataManager {

//     public static TablesLoadQueue: { [TableName: string]: any; } = {};

//     public static GetClosedTableData(objectTableName: string) {

//         if (CachedDataManager.TablesLoadQueue[objectTableName]) {

//             // if already exists in the load stack return the same obs
//             return CachedDataManager.TablesLoadQueue[objectTableName].pipe(share());
//         }
//         var observable: any;
//         console.time("Unzipping ClosedTable Data from local cache for: " + objectTableName);
//         var storagefileName: string = objectTableName + "_ClosedData.zip";
//         var fileString = LocalStorageManager.GetItem(storagefileName);
//         if (fileString == null || fileString == undefined)
//             fileString = EntityResourceService.ClosedTablesDataZipFilesDictionary[storagefileName];
//       if (fileString != null) {
//         observable = Observable.create(observer => {
//           if (fileString != null) {
//             var fileData = ServiceHelper.base64ToBufferConvertor(fileString);

//             var data = new ZipWorkerMessage();
//             data.FileData = fileData;
//             data.FileType = "string";

//             var worker = new WebWorkerService();
//             //const promises = [];
//             //promises.push(worker.runUrl('Infrastructure/WebWorker/JSZipWebWorker.js', data));
//             var promise = worker.runUrl('Js/jszip-web-worker.js', data);
//             promise.then(unzippedFiles => {

//               var file = JSON.parse(unzippedFiles)[0];// only one file in the result
//               var list = JSON.parse(file.FileData);

//               console.timeEnd("Unzipping ClosedTable Data from local cache for: " + objectTableName);

//               CachedDataManager.TablesLoadQueue[objectTableName] = null;

//               worker.terminate(promise);
//               observer.next(list);
//             }
//             )
//               .catch(error => {
//                 console.error(error);
//               });
//           }
//           else {

//             CachedDataManager.TablesLoadQueue[objectTableName] = null;
//             console.error("couldn't find closed table data for " + objectTableName + ".zip file in the cache!");
//             observer.next(0);
//           }

//         }).pipe(share());
//       } else {
//         console.log("couldn't find closed table data for " + objectTableName + ".zip file in the cache!");
//         //observable = defer(() => {
//           console.log("Calling Server For " + objectTableName + " Closed Data & MetaData");
//           var entityResourceService: EntityResourceService = new EntityResourceService();
//           EntityResourceService.TablesLoadQueue[objectTableName] = null;
//           return entityResourceService.getEntityResourceByTableName(objectTableName, 0).pipe(flatMap((response: any) => {
//             //return CachedDataManager.GetClosedTableData(objectTableName);
//             var storagefileName: string = objectTableName + "_ClosedData.zip";
//             var fileString = LocalStorageManager.GetItem(storagefileName);
//             if (fileString == null || fileString == undefined)
//               fileString = EntityResourceService.ClosedTablesDataZipFilesDictionary[storagefileName];
//             if (fileString != null) {
//               return Observable.create(observer => {
//                 if (fileString != null) {
//                   var fileData = ServiceHelper.base64ToBufferConvertor(fileString);

//                   var data = new ZipWorkerMessage();
//                   data.FileData = fileData;
//                   data.FileType = "string";

//                   var worker = new WebWorkerService();
//                   //const promises = [];
//                   //promises.push(worker.runUrl('Infrastructure/WebWorker/JSZipWebWorker.js', data));
//                   var promise = worker.runUrl('Js/jszip-web-worker.js', data);
//                   promise.then(unzippedFiles => {

//                     var file = JSON.parse(unzippedFiles)[0];// only one file in the result
//                     var list = JSON.parse(file.FileData);

//                     console.timeEnd("Unzipping ClosedTable Data from local cache for: " + objectTableName);

//                     CachedDataManager.TablesLoadQueue[objectTableName] = null;

//                     worker.terminate(promise);
//                     observer.next(list);
//                   }
//                   )
//                     .catch(error => {
//                       console.error(error);
//                     });
//                 }
//                 else {

//                   CachedDataManager.TablesLoadQueue[objectTableName] = null;
//                   console.error("couldn't find closed table data for " + objectTableName + ".zip file in the cache!");
//                   observer.next(0);
//                 }

//               });
//             }
//             else {
//               return of(0);
//             }
//           }), share());
//        // }).pipe(share());
//       }

//         /*

//         */
//         CachedDataManager.TablesLoadQueue[objectTableName] = observable;

//         return observable;
//     }

//     public static CuurentEntityListService: EntityListService;
//     static FinishedTables = [];
//     static AllCachedTables = [];
//     public static GetCacheOnClientTablesData(entityListService: EntityListService) {

//         for (var key in localStorage) {
//             if (key.indexOf("_CachedData_") != -1) {
//                 var m: Array<string> = key.split('_');
//                 var storedTenant: number = +(m[m.length - 1]);
//                 if (storedTenant != SessionInfo.LoggedUserTenant) {
//                     LocalStorageManager.RemoveItem(key);
//                     console.log(key + " deleted from local storage");
//                 }
//             }
//             else {
//                 //console.log( key + " not deleted")
//             }
//         }
//         //if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
//         if (ObjectsLocator != null && ObjectsLocator.GlobalSetting != null && ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
//             CachedDataManager.AllCachedTables = window.ObjectTables.filter(d => d.CacheOnClient === true && d.ClientModuleName && d.IsClosed == false && d.Name.startsWith("Customs."));

//         }
//         else {
//             CachedDataManager.AllCachedTables = window.ObjectTables.filter(d => d.CacheOnClient === true && d.ClientModuleName && d.IsClosed == false && !d.Name.startsWith("Customs."));
//         }

//         CachedDataManager.CachedTablesCount = CachedDataManager.AllCachedTables.length;
//         CachedDataManager.CallsCount = 0;
//         CachedDataManager.FinishedCallsCount = 0;

//         var arr = CachedDataManager.AllCachedTables.splice(0, 5);
//         CachedDataManager.CallCacheOnClientTablesData(entityListService, arr);

//     }

//     static CallsCount = 0;
//     static FinishedCallsCount = 0;
//     static CachedTablesCount = 0;
//     public static CallCacheOnClientTablesData(entityListService: EntityListService, tablesCalls: Array<any>) {

//         var filters = new ApiQueryFilters();
//         filters.Tenant = SessionInfo.LoggedUserTenant;
//         filters.GetAll = true;

//         const promises = [];
//         //var chachedTables = [];//window.ObjectTables.filter(d => d.CacheOnClient === true && d.ClientModuleName && d.IsClosed == false);
//         for (var k in tablesCalls) {
//             if (tablesCalls[k].ClientModuleName) {
//                 //promises.push(entityListService.getAllFromCache(chachedTables[k].Name, filters)).then(res=> {
//                 try {
//                     if (tablesCalls[k].Name == "VatType")
//                         filters.ForceCacheRefresh = true;
//                     else
//                         filters.ForceCacheRefresh = false;

//                     var myCachedDataManagerServices = new CachedDataManagerServices();
//                     myCachedDataManagerServices.getAllFromCache(tablesCalls[k].Name, filters).then((res: any) => {
//                     //entityListService.getAllFromCache(tablesCalls[k].Name, filters).then((res:any)=> {
//                         res.subscribe(resp=> {

//                             if (resp.Data) {
//                             }
//                             else {
//                             }
//                             //console.log(resp.Data);
//                             CachedDataManager.CheckFinishedCalls(entityListService);

//                         }, error => {

//                             console.error(error);
//                             CachedDataManager.CheckFinishedCalls(entityListService);

//                         });//.catch(CachedDataManager.HandleServiceError);
//                     }, err => {

//                         console.error(err);
//                         CachedDataManager.CheckFinishedCalls(entityListService);

//                     });//.catch(CachedDataManager.HandleServiceError);
//                 }
//                 catch (e) {

//                     CachedDataManager.CheckFinishedCalls(entityListService);
//                     console.error(e);
//                 }
//             }
//         }

//     }

//     public static CheckFinishedCalls(entityListService: any) {
//         CachedDataManager.FinishedCallsCount++;
//         CachedDataManager.CallsCount++;
//         if (CachedDataManager.CallsCount == 5) {
//             CachedDataManager.CallsCount = 0;

//             var arr = CachedDataManager.AllCachedTables.splice(0, 5);
//             CachedDataManager.CallCacheOnClientTablesData(entityListService, arr);
//         }

//         if (CachedDataManager.FinishedCallsCount == CachedDataManager.CachedTablesCount) {

//             //"CachedTableLastUpdateDate" + tenant.Id
//             var cacheKey = "CachedTableLastUpdateDate" + SessionInfo.LoggedUserTenant;
//             var stored = LocalStorageManager.GetItem(cacheKey);
//             if (!stored) {

//                 var authHeader = new Headers();
//                 authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

//                 return ServiceHelper.HttpClient.get(ServiceHelper.GetLogitudeURL() + 'api/ObjectTableLastUpdate/GetLastTableUpdateDate/?' + 'tenant=' + SessionInfo.LoggedUserTenant, ServiceHelper.GetHttpHeaders()).subscribe((response: any) => {

//                     var lastdate = response;
//                     LocalStorageManager.SetItem("CachedTableLastUpdateDate" + SessionInfo.LoggedUserTenant, JSON.stringify(lastdate));
//                     console.log(lastdate);
//                 });
//             }

//             console.log("loading cached on client tables finished! (" + CachedDataManager.CachedTablesCount+ ")");
//         }
//     }
//     public static HandleServiceError(error: any) {

//     }

//     public static CheckSystemMetadataLastUpdate() {

//        // window.on("itemRemoved", function (e, args) {
//        //     console.log(e, args);
//        //     if (!localStorage.getItem(args)) {
//        //         // do stuff
//        //     }
//        // })

//        //let fn = localStorage.removeItem;
//        // localStorage.removeItem = function () {
//        //     var args = arguments;
//        //     setTimeout(function () {
//        //         window.trigger("itemRemoved", [args[0]])
//        //     });
//        //     return fn.call(localStorage, args[0]);
//        // }

//         var authHeader = new Headers();
//         authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
//         return defer(() => {
//             return ServiceHelper.HttpClient.get(ServiceHelper.GetLogitudeURL() + 'api/SystemMetadataLastUpdate/GetSystemMetadataLastUpdates/?' + 'tenant=' + 0, ServiceHelper.GetHttpHeaders()).pipe(flatMap(response => {
//                 return Observable.create(observer => {

//                     var list = response;

//                     var serverlastUpdates: MetaDataLastUpdateDates;
//                     var clientlastUpdates: MetaDataLastUpdateDates;

//                     serverlastUpdates = CachedDataManager.MapJsonToClassEntity<MetaDataLastUpdateDates>(list, MetaDataLastUpdateDates);
//                     var stored = LocalStorageManager.GetItem("SystemMetadataLastUpdate");
//                     if (stored) {
//                         var result = JSON.parse(stored);
//                         clientlastUpdates = CachedDataManager.MapJsonToClassEntity<MetaDataLastUpdateDates>(result, MetaDataLastUpdateDates);
//                         var m = clientlastUpdates.ObjectFieldsSystemUpdateDateGMT;
//                         if (clientlastUpdates.ObjectFieldsSystemUpdateDateGMT != serverlastUpdates.ObjectFieldsSystemUpdateDateGMT
//                             || clientlastUpdates.TranslationsSystemUpdateDateGMT != serverlastUpdates.TranslationsSystemUpdateDateGMT) {
//                             for (var key in localStorage) {
//                                 if (key.indexOf("ObjectFields") != -1 || key.indexOf("TextCodes") != -1 || key.indexOf("ClosedData") != -1) {
//                                     LocalStorageManager.RemoveItem(key);
//                                     console.log("%c" + key + " deleted from local storage (System Metadata Refresh)", 'background: #ffd966; color: brown')
//                                 }
//                                 else {
//                                     //console.log( key + " not deleted")
//                                 }
//                             }

//                             //setTimeout(() => {
//                                 for (var key in localStorage) {
//                                     if (key.indexOf("ObjectFields") != -1 || key.indexOf("TextCodes") != -1 || key.indexOf("ClosedData") != -1) {

//                                         console.warn(key + " is not deleted from local storage after System Metadata Refresh !!!");
//                                     }
//                                 }

//                                 LocalStorageManager.SetItem("SystemMetadataLastUpdate", JSON.stringify(list));

//                                 console.log("%c -------------      SystemMetadataLastUpdate has been updated!      ---------------", 'background: #222; color: #bada55');
//                                 observer.next(list);

//                            // }, 3000);

//                         }
//                         else {
//                             observer.next(list);
//                             //return list;
//                         }
//                     }
//                     else {

//                         LocalStorageManager.SetItem("SystemMetadataLastUpdate", JSON.stringify(list));
//                         observer.next(list);
//                         //return list;
//                     }

//                     //return list;
//                 });
//             }), catchError(ServiceHelper.HandleServiceError));
//         });

//     }

//     static needTobeUpdatedTablesList: Array<ObjectTableLastUpdatePM> = []
//     static updatedCachedTablesCount = 0;
//     @Output() static RefreshCompleted: EventEmitter<any> = new EventEmitter();
//     public static CheckCachedTableLastUpdateDate() {

//         CachedDataManager.needTobeUpdatedTablesList = [];
//         CachedDataManager.updatedCachedTablesCount = 0;

//         var authHeader = new Headers();
//         authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
//         return defer(() => {
//             var cacheKey = "CachedTableLastUpdateDate" + SessionInfo.LoggedUserTenant;
//             var storedDate = LocalStorageManager.GetItem(cacheKey);
//             if (storedDate) {

//                 return ServiceHelper.HttpClient.get(ServiceHelper.GetLogitudeURL() + 'api/ObjectTableLastUpdate/GetLastUpdatedTables/?' + 'tenant=' + SessionInfo.LoggedUserTenant + '&sinceDate=' + JSON.parse(storedDate) + '&clientEmail=' + SessionInfo.LoggedUserEmail, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
//                     var list = response;
//                     //CachedDataManager.needTobeUpdatedTablesList = CachedDataManager.MapJsonToEntityList<ObjectTableLastUpdatePM>(list, ObjectTableLastUpdatePM);
//                     //for (var key in cachedJson) {

//                     //    var entity: IncotermList;
//                     //    entity = this.MapJsonToEntityList(cachedJson[key]);
//                     //    _mappedListsArray.push(entity);
//                     //}
//                     if (list) {

//                         CachedDataManager.needTobeUpdatedTablesList = CachedDataManager.MapJsonToEntityList<ObjectTableLastUpdatePM>(list, ObjectTableLastUpdatePM);

//                         for (var key in CachedDataManager.needTobeUpdatedTablesList) {

//                             CachedDataManager.RefreshTableData(list[key].ObjectTableName);

//                         }
//                     }

//                     return list;

//                 }), catchError(ServiceHelper.HandleServiceError));
//             }
//         });

//     }

//     static RefreshTableData(tableName: string, holdTimer:boolean = false) {
//         console.log("calling refresh for table: " + tableName);
//         var filters = new ApiQueryFilters();
//         filters.Tenant = SessionInfo.LoggedUserTenant;
//         filters.GetAll = true;
//         filters.ForceCacheRefresh = true;

//         //var entityListService = new EntityListService();
//         //entityListService.getAllFromCache(tableName, filters).then((res:any)=> {

//         var myCachedDataManagerServices = new CachedDataManagerServices();
//         myCachedDataManagerServices.getAllFromCache(tableName, filters).then((res: any) => {
//             res.subscribe(resp=> {

//                 CachedDataManager.CheckUpdatedCachedTables(holdTimer);
//                 //CachedDataManager.CheckFinishedCalls(entityListService);

//             }, error => {
//                 CachedDataManager.CheckUpdatedCachedTables(holdTimer);
//                 //console.error(error);

//                 //CachedDataManager.CheckFinishedCalls(entityListService);

//             });
//         }, err => {
//             CachedDataManager.CheckUpdatedCachedTables(holdTimer);
//             //CachedDataManager.CheckFinishedCalls(entityListService);
//         });
//     }

//     static CheckUpdatedCachedTables(holdTimer:boolean) {
//         CachedDataManager.updatedCachedTablesCount++;
//         if (CachedDataManager.updatedCachedTablesCount == CachedDataManager.needTobeUpdatedTablesList.length && !holdTimer) {
//             LocalStorageManager.SetItem("CachedTableLastUpdateDate" + SessionInfo.LoggedUserTenant, JSON.stringify(CachedDataManager.needTobeUpdatedTablesList[0].LastUpdateDate));
//            // CachedDataManager.needTobeUpdatedTablesList.sort(function (a, b) {
//                 // Turn your strings into dates, and then subtract them
//                 // to get a value that is either negative, positive, or zero.
//                // return (b.LastUpdateDate - a.LastUpdateDate);
//            // });

//             CachedDataManager.updatedCachedTablesCount = 0;
//             CachedDataManager.needTobeUpdatedTablesList = [];
//         }

//         CachedDataManager.RefreshCompleted.emit(true);
//         holdTimer = false;

//     }

//     static MapJsonToClassEntity<T>(jsonList: any, classType: { new (): T; }) {

//         var entityList: T;
//         entityList = new classType();
//         var jsonListKeys = Object.keys(jsonList);

//         for (var key in jsonListKeys) {
//             var property = jsonListKeys[key];
//             entityList[property] = jsonList[property];
//         }

//         return entityList;
//     }

//     static MapJsonToEntityList<T>(jsonArray: any, classType: { new (): T; }) {

//         var mappedArray: Array<T> = [];
//         for (var key in jsonArray) {

//             var entity = jsonArray[key];
//             mappedArray.push(CachedDataManager.MapJsonToClassEntity<T>(entity, classType));
//         }

//         return mappedArray;
//     }

//     public static RefreshTenantTextCodes() {

//         var authHeader = new Headers();
//         authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
//         var url = ServiceHelper.GetLogitudeURL() + 'api/ngMetaData/GetTenantTextCodes?tenant=' + SessionInfo.LoggedUserTenant;
//         return defer(() => {
//             return ServiceHelper.HttpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
//                 var newList = response;
//                 for (var k in newList) {
//                     var item = newList[k];

//                     var oldItem = window.TextCodes.filter(t=> t.Id == item.Id)[0];
//                     if (oldItem) {
//                         var index = window.TextCodes.indexOf(oldItem);
//                         window.TextCodes.splice(index, 1);
//                     }
//                     window.TextCodes.push(item);

//                     var oldItemCached = window.TextCodesCache.filter(t => t.Id == item.Id)[0];
//                     if (oldItemCached) {
//                         var index = window.TextCodesCache.indexOf(oldItemCached);
//                         window.TextCodesCache.splice(index, 1);
//                     }
//                     window.TextCodesCache.push(item);

//                     //var index = window.TextCodes.indexOf(item);
//                     //if (index > -1) {
//                     //    window.TextCodes.splice(index, 1);
//                     //}

//                     //window.TextCodes.push(item);
//                     //var oldItem = window.TextCodes.filter(t=> t.Id == item.Id)[0];
//                     //if (oldItem) {
//                     //    var a = [];
//                     //    a.
//                     //    //window.TextCodes.
//                     //}
//                     //else {
//                     //}
//                 }

//                 return response;
//             }), catchError(ServiceHelper.HandleServiceError));
//         });

//     }

//     public static RefreshTenantObjectFields() {

//         var authHeader = new Headers();
//         authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
//         var url = ServiceHelper.GetLogitudeURL() + 'api/ngMetaData/GetTenantObjectFields?loggedTenant=' + SessionInfo.LoggedUserTenant;
//         return defer(() => {
//             return ServiceHelper.HttpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
//                 var newList = response;
//                 for (var k in newList) {
//                     var item = newList[k];

//                     var oldItem = window.ObjectFields.filter(t => t.Id == item.Id)[0];
//                     if (oldItem) {
//                         var index = window.ObjectFields.indexOf(oldItem);
//                         window.ObjectFields.splice(index, 1);
//                     }
//                     window.ObjectFields.push(item);

//                     var oldItemCached = window.TextCodesCache.filter(t => t.Id == item.Id)[0];
//                     if (oldItemCached) {
//                         var index = window.ObjectFieldsCache.indexOf(oldItemCached);
//                         window.ObjectFieldsCache.splice(index, 1);
//                     }
//                     window.ObjectFieldsCache.push(item);

//                 }

//                 return response;
//             }), catchError(ServiceHelper.HandleServiceError));
//         });

//     }

//     public static RefreshObjectFieldsModifications() {
//         console.log("calling refresh for object fields modifications");
//         var generalDomainService: GeneralDomainService = new GeneralDomainService();
//         generalDomainService.GetObjectFieldModificationForLoggedTenant().subscribe((response: ServiceResponse) => {

//             if (!response.HasError) {
//                 window.ObjectFieldModifications = response.Result;

//                 EntityResourceService.FillObjectFieldsModifications();

//                 console.log("refresh for object fields modification completed!");
//             }
//             else
//                 console.warn("refresh for object fields modification failed!");
//         });
//     }
//     //static testM(message) {

//     //    var objectTableName = "Incoterm";
//     //    var serviceType;
//     //    //var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
//     //    var moduleName = "Common";//table.ClientModuleName;
//     //    var servicename = objectTableName + "ListService";
//     //    var servicelink = './' + moduleName + '/Services/StandardLists/' + -;

//     //    return 44444System.import(servicelink)
//     //        .then(m => {
//     //            serviceType = m[servicename];
//     //            var service = Object.create(serviceType.prototype);
//     //            service.setServiceArgs(null);
//     //            return service.getAllFromCache(null);
//     //        });

//     //    //CachedDataManager.CuurentEntityListService.getAllFromCache("Incoterm", null);
//     //    //return message;
//     //}

//     //runWebWorker() {
//     //    var worker = new WebWorkerService();

//     //    var promise = worker.run(this.testM, "hello!!!");
//     //    promise.then(response => {

//     //        console.log(response);
//     //        worker.terminate(promise);

//     //    }
//     //    ).catch(error => {
//     //        console.error(error);
//     //    }
//     //        );
//     //}

//     //testM(message) {
//     //    return message;
//     //}

// }

// export class ZipWorkerMessage {
//     public FileType: string;
//     public FileData: any;
// }
