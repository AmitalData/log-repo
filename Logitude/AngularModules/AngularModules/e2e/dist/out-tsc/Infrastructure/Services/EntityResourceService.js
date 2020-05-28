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
require("rxjs/add/operator/map");
require("rxjs/add/operator/catch");
var Observable_1 = require("rxjs/Observable");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var web_worker_service_1 = require("../WebWorker/web-worker.service");
var IndexedDbService_1 = require("./IndexedDbService");
var LocalStorageManager_1 = require("../Utilities/LocalStorageManager");
var EntityResourceService = /** @class */ (function () {
    function EntityResourceService() {
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/EntityResource";
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this.DbService = new IndexedDbService_1.IndexedDbService(); //InfraSettings.IndexedDbService;//
        window.CurrentService = this;
    }
    EntityResourceService_1 = EntityResourceService;
    EntityResourceService.prototype.getEntityResourceByTableName = function (objectTableName, tenant) {
        var _this = this;
        if (tenant === void 0) { tenant = 0; }
        if (!SessionLocator_1.SessionLocator.UseCachedData) {
            return Observable_1.Observable.create(function (observer) {
                observer.next(1);
            });
        }
        if (EntityResourceService_1.ExisitsInCache(objectTableName)) {
            return Observable_1.Observable.create(function (observer) {
                observer.next(objectTableName);
            });
        }
        else {
            if (EntityResourceService_1.TablesLoadQueue[objectTableName]) {
                // if already exists in the load stack return the same obs
                return EntityResourceService_1.TablesLoadQueue[objectTableName].share();
            }
            var fieldsKey = objectTableName + "_ObjectFields.zip";
            var codesKey = objectTableName + "_TextCodes.zip";
            var entityFields = LocalStorageManager_1.LocalStorageManager.GetItem(fieldsKey);
            var entityCodes = LocalStorageManager_1.LocalStorageManager.GetItem(codesKey);
            var isMissingClosedTable = false;
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
            if (objectTable && objectTable.IsClosed == true) {
                var storagefileName = objectTableName + "_ClosedData.zip";
                var fileString = LocalStorageManager_1.LocalStorageManager.GetItem(storagefileName);
                if (fileString == null || fileString == undefined)
                    isMissingClosedTable = true;
            }
            // var r = -1;
            // return this.DbService.GetTableData("TextCodes", objectTableName).flatMap(r=> {
            if (!entityFields || !entityCodes || isMissingClosedTable == true) {
                //var observable = Observable.timer(1).flatMap(function test() {
                //    var authHeader = new Headers();
                //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
                //    return this._http.get(this._apiUrl + '?objectTableName=' + objectTableName + '&tenant=' + tenant, { headers: authHeader }).map(response => {
                //        var filejson = response.json();
                //        if (filejson) {
                //            var fileData = EntityResourceService.base64ToBufferConvertor(filejson);
                //            return this.UnZipFileAndAddToStorageUsingWebWorker(fileData, objectTableName);
                //        }
                //        else {
                //            console.error("couldn't find " + objectTableName + ".zip file in the server!");
                //            return [];
                //        }
                //    });
                //}).share();
                var observable = this.GetResourcesFile(objectTableName, tenant).flatMap(function (response) {
                    var filejson = response.json();
                    if (filejson) {
                        if (EntityResourceService_1.ServerTablesUnzipQueue[objectTableName]) {
                            // if already exists in the load stack return the same obs
                            return EntityResourceService_1.ServerTablesUnzipQueue[objectTableName].share();
                        }
                        var fileData = EntityResourceService_1.base64ToBufferConvertor(filejson);
                        var obs = _this.UnZipFileAndAddToStorageUsingWebWorker(fileData, objectTableName).share();
                        EntityResourceService_1.ServerTablesUnzipQueue[objectTableName] = obs;
                        return obs;
                    }
                    else {
                        console.error("couldn't find " + objectTableName + ".zip file in the server!");
                        return [];
                    }
                }).share();
                EntityResourceService_1.TablesLoadQueue[objectTableName] = observable;
                return observable.share();
            }
            else {
                EntityResourceService_1.ZipFilesDictionary[objectTableName] = [];
                if (entityFields) {
                    var zFieldsObject = new ZipFileDetails();
                    zFieldsObject.FileName = fieldsKey;
                    zFieldsObject.FileData = EntityResourceService_1.base64ToBufferConvertor(entityFields);
                    EntityResourceService_1.ZipFilesDictionary[objectTableName].push(zFieldsObject);
                }
                if (entityCodes) {
                    var zCodesObject = new ZipFileDetails();
                    zCodesObject.FileName = codesKey;
                    zCodesObject.FileData = EntityResourceService_1.base64ToBufferConvertor(entityCodes);
                    EntityResourceService_1.ZipFilesDictionary[objectTableName].push(zCodesObject);
                }
                var observable = this.UnZipFilesToMemoryUsingWebWorker(objectTableName);
                observable.share();
                EntityResourceService_1.TablesLoadQueue[objectTableName] = observable;
                return observable.share();
            }
        }
    };
    EntityResourceService.prototype.AddTableToCache = function (tableName) {
        //console.log("adding " + tableName + " table to cache");
        window.CachedTables.push(tableName);
        // if (window.CachedTables.length > 10) {
        //   window.CachedTables.splice(0, 10);
        //  }
    };
    EntityResourceService.prototype.GetResourcesFile = function (objectTableName, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?objectTableName=' + objectTableName + '&tenant=' + tenant, { headers: authHeader }).share();
    };
    EntityResourceService.base64ToBufferConvertor = function (str) {
        str = window.atob(str); // creates a ASCII string
        var buffer = new ArrayBuffer(str.length), view = new Uint8Array(buffer);
        for (var i = 0; i < str.length; i++) {
            view[i] = str.charCodeAt(i);
        }
        return buffer;
    };
    EntityResourceService.ExisitsInCache = function (objectTableName) {
        var table = objectTableName;
        if (objectTableName.indexOf("&") > -1) {
            table = objectTableName.split('&')[0];
        }
        var cachedTable = window.CachedTables.filter(function (d) { return (d == table); })[0];
        if (cachedTable) {
            var tableExists = false;
            var fieldsExists = false;
            var codesExists = false;
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
            fieldsExists = window.ObjectFields.some(function (d) { return d.ObjectTableId == objectTable.Id; });
            codesExists = window.TextCodes.some(function (d) { return d.ObjectTableId == objectTable.Id; });
            if ((fieldsExists == true && codesExists == true) || objectTableName == "General") {
                if (objectTable.IsClosed == true) {
                    var storagefileName = objectTableName + "_ClosedData.zip";
                    var fileString = LocalStorageManager_1.LocalStorageManager.GetItem(storagefileName);
                    if (fileString != null && fileString != undefined)
                        tableExists = true;
                    else
                        console.warn(objectTableName + " Closed Table Data doesn't exist in cache");
                }
                else {
                    tableExists = true;
                }
            }
            else {
                tableExists = false;
                console.warn(objectTableName + " fields or codes doesn't exist in cache");
            }
        }
        else {
            tableExists = false;
            //console.warn(objectTableName + " is not defined!!");
        }
        if (tableExists)
            console.log(objectTableName + " already exist in cache");
        // else
        return tableExists;
    };
    // =============================================================================
    EntityResourceService.prototype.UnZipFilesToMemoryUsingWebWorker = function (objectTableName) {
        return Observable_1.Observable.create(function (observer) {
            console.time("Unzipping (TextCodes & Fields) from local cache for: " + objectTableName);
            var insideCount = 0;
            for (var key in EntityResourceService_1.ZipFilesDictionary[objectTableName]) {
                var zFile = EntityResourceService_1.ZipFilesDictionary[objectTableName][key];
                var worker = new web_worker_service_1.WebWorkerService();
                var data = new ZipWorkerMessage();
                data.FileData = zFile.FileData;
                data.FileType = "string";
                data.FileName = objectTableName;
                //const promises = [];
                //promises.push(worker.runUrl('Infrastructure/WebWorker/JSZipWebWorker.js', data));
                var promise = worker.runUrl('Js/jszip-web-worker.js', data);
                promise.then(function (response) {
                    insideCount++;
                    var unzippedFiles = JSON.parse(response);
                    for (key in unzippedFiles) { // loop the unzipped files
                        var unzfile = unzippedFiles[key];
                        //var message = JSON.parse(response)[0];//it contains only one file
                        var nameArr = unzfile.FileName.split('_');
                        var entityName = nameArr[0];
                        var tableTypeName = nameArr[1].split('.')[0];
                        var list = JSON.parse(unzfile.FileData);
                        if (!EntityResourceService_1.ExisitsInCache(objectTableName)) {
                            var objectTable = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
                            switch (tableTypeName) {
                                case "Codes":
                                    var codesExists = false;
                                    //if (objectTable)
                                    //   codesExists = window.TextCodes.some(d => d.ObjectTableId == objectTable.Id);
                                    if (codesExists == false)
                                        window.TextCodes = window.TextCodes.concat(list);
                                    else
                                        console.log(objectTableName + " Text Codes already loaded in the memory");
                                    if (!list || (list && list.length <= 0)) {
                                        console.warn(objectTableName + " Unzipped local cache Text Codes are Empty!!");
                                    }
                                    if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                                        // window.TextCodes.filter(a => a.Code == "General.MH.Importers")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelName;
                                        //window.TextCodes.filter(a => a.Code == "General.MH.ActivationWizard")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Services"; 
                                    }
                                    break;
                                case "Fields":
                                    var fieldsExists = false;
                                    if (objectTable && objectTable.IsClosed == true)
                                        fieldsExists = window.ObjectFields.some(function (d) { return d.ObjectTableId == objectTable.Id; });
                                    if (fieldsExists == false)
                                        window.ObjectFields = window.ObjectFields.concat(list);
                                    else
                                        console.log(objectTableName + " Object Fields already loaded in the memory");
                                    if (!list || (list && list.length <= 0) && objectTableName != "General") {
                                        console.warn(objectTableName + " Unzipped local cache Object Fields are Empty!!");
                                    }
                                    break;
                            }
                        }
                        if (insideCount == EntityResourceService_1.ZipFilesDictionary[objectTableName].length) {
                            console.timeEnd("Unzipping (TextCodes & Fields) from local cache for: " + objectTableName);
                            var table = objectTableName;
                            if (objectTableName.indexOf("&") > -1) {
                                table = objectTableName.split('&')[0];
                            }
                            EntityResourceService_1.ZipFilesDictionary[objectTableName] = [];
                            EntityResourceService_1.TablesLoadQueue[objectTableName] = null;
                            window.CachedTables.push(table);
                            worker.terminate(promise);
                            observer.next(1);
                        }
                    }
                })
                    .catch(function (error) {
                    console.error(error);
                });
            }
        }).share(); //.publish().refCount();
    };
    EntityResourceService.prototype.UnZipFileAndAddToStorageUsingWebWorker = function (buffer, parentEntityName) {
        console.time("Unzipping Server File for: " + parentEntityName);
        EntityResourceService_1.ZipFilesDictionary[parentEntityName] = [];
        return Observable_1.Observable.create(function (observer) {
            var worker = new web_worker_service_1.WebWorkerService();
            var count = 0;
            var zipdata = new ZipWorkerMessage();
            zipdata.FileData = buffer;
            zipdata.FileType = "base64";
            zipdata.FileName = parentEntityName;
            var promise = worker.runUrl('Js/jszip-web-worker.js', zipdata);
            promise.then(function (response) {
                var unzippedFiles = JSON.parse(response);
                for (key in unzippedFiles) {
                    var unzfile = unzippedFiles[key]; //JSON.parse(response);
                    var datm = unzfile.FileData;
                    var fileCount = unzfile.FilesCount;
                    count++;
                    var entityName = unzfile.FileName;
                    var type = null;
                    if (entityName.indexOf("_") > -1) {
                        var a = entityName.split('_');
                        entityName = a[0];
                        type = a[1].split('.')[0];
                    }
                    if (entityName.indexOf("Customs.") == -1) {
                        if (entityName.indexOf(".") > -1) {
                            var a = entityName.split('.');
                            entityName = a[0];
                        }
                    }
                    var storagefileName = unzfile.FileName;
                    if (type == "ClosedData") {
                        storagefileName = entityName + "_ClosedData.zip";
                    }
                    if (LocalStorageManager_1.LocalStorageManager.SetItem(storagefileName, datm) == false) // write the file to the local storage
                     {
                        if (type == "ClosedData") {
                            if (!EntityResourceService_1.ClosedTablesDataZipFilesDictionary[storagefileName]) {
                                EntityResourceService_1.ClosedTablesDataZipFilesDictionary[storagefileName] = datm;
                                console.log(entityName + " Closed Table Data stored to memory");
                            }
                        }
                        console.warn(storagefileName + " Failed to be written on the storage!!!!!");
                    }
                    if (entityName == parentEntityName && type != "ClosedData") { // decompress the files for the requested entity to the memory
                        var add = true;
                        if (EntityResourceService_1.ZipFilesDictionary[parentEntityName] && EntityResourceService_1.ZipFilesDictionary[parentEntityName].length > 0 && EntityResourceService_1.ZipFilesDictionary[parentEntityName].filter(function (d) { return d.FileName == unzfile.FileName; }).length > 0) {
                            add = false;
                        }
                        if (add) {
                            var zObject = new ZipFileDetails();
                            zObject.FileName = unzfile.FileName;
                            zObject.FileData = EntityResourceService_1.base64ToBufferConvertor(datm);
                            EntityResourceService_1.ZipFilesDictionary[parentEntityName].push(zObject);
                        }
                    }
                    //------------------------------------------------------------------
                    if (count == fileCount) {
                        console.timeEnd("Unzipping Server File for: " + parentEntityName);
                        console.time("(TextCodes & Fields) (after server call) for table:" + parentEntityName);
                        var insideCount = 0;
                        for (var key in EntityResourceService_1.ZipFilesDictionary[parentEntityName]) {
                            var zFile = EntityResourceService_1.ZipFilesDictionary[parentEntityName][key];
                            var data = new ZipWorkerMessage();
                            data.FileData = zFile.FileData;
                            data.FileType = "string";
                            var promise = worker.runUrl('Js/jszip-web-worker.js', data);
                            promise.then(function (response) {
                                insideCount++;
                                var unzchildfile = JSON.parse(response)[0]; // take the only file in the result
                                var nameArr = unzchildfile.FileName.split('_');
                                var entityName = nameArr[0];
                                var tableTypeName = nameArr[1].split('.')[0];
                                var list = JSON.parse(unzchildfile.FileData);
                                if (!EntityResourceService_1.ExisitsInCache(entityName)) {
                                    var objectTable = window.ObjectTables.filter(function (d) { return d.Name == entityName; })[0];
                                    switch (tableTypeName) {
                                        case "Codes":
                                            var codesExists = false;
                                            //if (objectTable)
                                            //   codesExists = window.TextCodes.some(d => d.ObjectTableId == objectTable.Id);
                                            if (codesExists == false)
                                                window.TextCodes = window.TextCodes.concat(list);
                                            else
                                                console.log(entityName + " Text Codes already loaded in the memory");
                                            if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                                                // window.TextCodes.filter(a => a.Code == "General.MH.Importers")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelName;
                                                //window.TextCodes.filter(a => a.Code == "General.MH.ActivationWizard")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Services"; 
                                            }
                                            break;
                                        case "Fields":
                                            var fieldsExists = false;
                                            if (objectTable && objectTable.IsClosed == true)
                                                fieldsExists = window.ObjectFields.some(function (d) { return d.ObjectTableId == objectTable.Id; });
                                            if (fieldsExists == false)
                                                window.ObjectFields = window.ObjectFields.concat(list);
                                            else
                                                console.log(entityName + " Object Fields already loaded in the memory");
                                            break;
                                        default:
                                            console.log("this is a closed table data " + entityName);
                                            break;
                                    }
                                }
                                if (insideCount == EntityResourceService_1.ZipFilesDictionary[parentEntityName].length) {
                                    console.timeEnd("(TextCodes & Fields) (after server call) for table:" + parentEntityName);
                                    var table = entityName;
                                    if (entityName.indexOf("&") > -1) {
                                        table = entityName.split('&')[0];
                                    }
                                    EntityResourceService_1.ZipFilesDictionary[entityName] = [];
                                    EntityResourceService_1.TablesLoadQueue[entityName] = null;
                                    EntityResourceService_1.ServerTablesUnzipQueue[parentEntityName] = null;
                                    window.CachedTables.push(table);
                                    worker.terminate(promise);
                                    observer.next(1);
                                }
                            }).catch(function (error) {
                                console.error(error);
                            });
                        }
                    }
                }
            }).catch(function (error) {
                console.error(error);
            });
        }).share(); //.publish().refCount()
    };
    var EntityResourceService_1;
    EntityResourceService.ClosedTablesDataZipFilesDictionary = {};
    EntityResourceService.ZipFilesDictionary = {};
    EntityResourceService.TablesLoadQueue = {};
    EntityResourceService.ServerTablesUnzipQueue = {};
    EntityResourceService = EntityResourceService_1 = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], EntityResourceService);
    return EntityResourceService;
}());
exports.EntityResourceService = EntityResourceService;
var ZipFileDetails = /** @class */ (function () {
    function ZipFileDetails() {
    }
    return ZipFileDetails;
}());
exports.ZipFileDetails = ZipFileDetails;
var ZipWorkerMessage = /** @class */ (function () {
    function ZipWorkerMessage() {
    }
    return ZipWorkerMessage;
}());
exports.ZipWorkerMessage = ZipWorkerMessage;
//# sourceMappingURL=EntityResourceService.js.map