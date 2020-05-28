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
var InfraSettings_1 = require("../../Infrastructure/Utilities/InfraSettings");
//import Rx from 'rxjs/Rx';
var Observable_1 = require("rxjs/Observable");
var IndexedDbService = /** @class */ (function () {
    function IndexedDbService() {
        // In the following line, you should include the prefixes of implementations you want to test.
        //window.indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
        // DON'T use "var indexedDB = ..." if you're not in a function.
        // Moreover, you may need references to some window.IDB* objects:
        //window.IDBTransaction = window.IDBTransaction || window.webkitIDBTransaction || window.msIDBTransaction || { READ_WRITE: "readwrite" }; // This line should only be needed if it is needed to support the object's constants for older browsers
        //window.IDBKeyRange = window.IDBKeyRange || window.webkitIDBKeyRange || window.msIDBKeyRange;
        // (Mozilla has never prefixed these objects, so we don't need window.mozIDB*)
        //if (InfraSettings.LogitudeIndexedDB)
        //{
        //    this.dbTransaction = InfraSettings.LogitudeIndexedDB.transaction("TextCodes", "readwrite");//"ObjectFields", "ClosedTables"]
        //    this.objectStore = this.dbTransaction.objectStore("TextCodes");
        //}
    }
    IndexedDbService.prototype.InitializeIndexedDB = function () {
        // window.indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
        return Observable_1.Observable.create(function (observer) {
            //prefixes of window.IDB objects
            window.IDBTransaction = window.IDBTransaction || window.webkitIDBTransaction || window.msIDBTransaction;
            window.IDBKeyRange = window.IDBKeyRange || window.webkitIDBKeyRange || window.msIDBKeyRange;
            //if (!window.indexedDB) {
            //    window.alert("Your browser doesn't support a stable version of IndexedDB.")
            //}
            //var db;
            //var request = window.indexedDB.open("newDatabase", 1);
            //request.onerror = function (event) {
            //    console.log("error: ");
            //};
            //request.onsuccess = function (event) {
            //    db = request.result;
            //    var objectStore = db.createObjectStore("TextCodes", { keyPath: "id" });
            //    var objectStore2 = db.createObjectStore("ObjectFields", { keyPath: "id" });
            //};
            if (!window.indexedDB) {
                window.alert("Your browser doesn't support a stable version of IndexedDB.");
            }
            var request = window.indexedDB.open("LogitudeDatabase", 1);
            //return request;
            request.onerror = function (event) {
                //console.log("error: ");
                return observer.next("Error");
            };
            request.onsuccess = function (event) {
                InfraSettings_1.InfraSettings.LogitudeIndexedDB = request.result;
                //console.log("indexeddb initiated successfully: " + InfraSettings.LogitudeIndexedDB);
                //this.dbTransaction = InfraSettings.LogitudeIndexedDB.transaction("TextCodes", "readwrite");//"ObjectFields", "ClosedTables"]
                //this.objectStore = this.dbTransaction.objectStore("TextCodes");
                return observer.next("Ok");
                //InfraSettings.LogitudeIndexedDB = request.result;
                //viewmodel._entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(res => {
                //    //var buffer = viewmodel.base64ToBuffer(res);
                //    //viewmodel.handleFile(buffer);
                //});
                //this.db =
            };
            request.onupgradeneeded = function (event) {
                var db = event.target.result;
                var objectStore = db.createObjectStore("TextCodes", { keyPath: 'EntityName' }); //, 'ResourcesTableName']
                var objectStore2 = db.createObjectStore("ObjectFields", { keyPath: 'EntityName' });
                var objectStore3 = db.createObjectStore("ClosedTables", { keyPath: 'EntityName' });
                var objectStore3 = db.createObjectStore("AdvancedQueryFilters", { keyPath: 'EntityName' });
                var objectStore3 = db.createObjectStore("Queries", { keyPath: 'EntityName' });
                // objectStore.createIndex("EntityName", "EntityName", { unique: false });
                //objectStore2.createIndex("EntityName", "EntityName", { unique: false });
                //objectStore3.createIndex("EntityName", "EntityName", { unique: false });
                // objectStore.createIndex('EntityName, TableName', ['EntityName', 'TableName'], { unique: true });
                //objectStore2.createIndex('EntityName, TableName', ['EntityName', 'TableName'], { unique: true });
                // objectStore3.createIndex('EntityName, TableName', ['EntityName', 'TableName'], { unique: true });
                InfraSettings_1.InfraSettings.LogitudeIndexedDB = db;
            };
        });
    };
    IndexedDbService.prototype.Add = function (dbtablename, entityName, data) {
        // var objectStore = InfraSettings.LogitudeIndexedDB.createObjectStore(dbtablename, { keyPath: "id" });
        var request = InfraSettings_1.InfraSettings.LogitudeIndexedDB.transaction([dbtablename], "readwrite")
            .objectStore(dbtablename)
            .add({ EntityName: entityName, value: data }); //ResourcesTableName: resorcesTableName,
        //request.onsuccess = function (event) {
        //    console.log(dbtablename + "data has been added to your database.", data);
        //};
        //request.onerror = function (event) {
        //    //alert("Unable to add data\r\nKenny is aready exist in your database! ");
        //}
        return request;
    };
    IndexedDbService.prototype.GetTableDataTest = function (dbtablename, entityName) {
        var start = new Date();
        // var transaction = InfraSettings.LogitudeIndexedDB.transaction([dbtablename]);
        //var objectStore = this.dbTransaction.objectStore(dbtablename);
        var request = this.objectStore.get(entityName); //[entityName, resorcesTableName]
        var request1 = this.objectStore.get(entityName);
        var request2 = this.objectStore.get(entityName);
        var request3 = this.objectStore.get(entityName);
        var request4 = this.objectStore.get(entityName);
        this.dbTransaction.oncomplete = function (event) {
            var end = new Date().getTime();
            var time = end - start;
            //console.log("All " + dbtablename + " " + entityName +  + " Resources are loaded!" + ' Execution time: ' + time, "==============================================================================");
        };
        //return request;
        request.onerror = function (event) {
            //console.log("Unable to retrieve daa from database!");
        };
        request.onsuccess = function (event) {
            //var end = new Date();
            // var time = end - start;
            //console.log("transaction " + dbtablename + " " + entityName + " success! " + ' Execution time: ' + time);
            // Do something with the request.result!
            if (request.result) {
                //request.result);
                //console.log("Name: " + request.result.name + ", Age: " + request.result.age + ", Email: " + request.result.email);
            }
            else {
                //console.log(dbtablename + " " + entityName + " record couldn't be found in your database!");
            }
        };
    };
    IndexedDbService.prototype.GetTableData = function (dbtablename, entityName) {
        return Observable_1.Observable.create(function (observer) {
            // console.clear();
            //console.time("get table " + dbtablename + " " + entityName + " data fom db");
            var count = 0;
            // var start = new Date();
            var transaction = InfraSettings_1.InfraSettings.LogitudeIndexedDB.transaction([dbtablename], "readonly");
            // transaction.oncomplete = function (event) {
            //    var end = new Date();
            //     var time = end - start;
            //     console.log("reading " + dbtablename + " " + entityName + " success! " + ' Execution time: ' + time , "==============================================================================");
            // }
            var objectStore = transaction.objectStore(dbtablename);
            // while (count < 1000) {
            var request = objectStore.get(entityName); //[entityName, resorcesTableName]
            //  count++;
            //}
            //return request;
            request.onerror = function (event) {
                //console.log("Unable to retrieve data from database!");
                observer.next("error");
            };
            request.onsuccess = function (event) {
                //console.timeEnd("get table " + dbtablename + " " + entityName + " data fom db");
                //var end = new Date();
                // var time = end - start;
                // console.log("transaction " + dbtablename + " " + entityName+ " success! " + ' Execution time: ' + time);
                // Do something with the request.result!
                if (request.result) {
                    // console.log(request.result);
                    // if (fillData) {
                    if (dbtablename == "TextCodes") {
                        //window.TextCodes.concat(request.result.value);
                        for (var index in request.result.value) {
                            var item = request.result.value[index];
                            if (item) {
                                window.TextCodes.push(request.result.value[index]);
                            }
                        }
                    }
                    if (dbtablename == "ObjectFields") {
                        //window.TextCodes.concat(request.result);
                        for (var index in request.result.value) {
                            var item = request.result.value[index];
                            if (item) {
                                window.ObjectFields.push(request.result.value[index]);
                            }
                        }
                    }
                    if (dbtablename == "AdvancedQueryFilters") {
                        //window.TextCodes.concat(request.result);
                        for (var index in request.result.value) {
                            var item = request.result.value[index];
                            if (item) {
                                window.AdvancedQueryFilters.push(request.result.value[index]);
                            }
                        }
                    }
                    if (dbtablename == "Queries") {
                        //window.TextCodes.concat(request.result);
                        for (var index in request.result.value) {
                            var item = request.result.value[index];
                            if (item) {
                                window.Queries.push(request.result.value[index]);
                            }
                        }
                    }
                    // }
                    return observer.next("ok"); //request.result);
                    //console.log("Name: " + request.result.name + ", Age: " + request.result.age + ", Email: " + request.result.email);
                }
                else {
                    //console.log(dbtablename + " " + entityName + " record couldn't be found in your database!");
                    observer.next(-1);
                }
            };
        });
    };
    IndexedDbService.prototype.GetClosedTableData = function (tableName) {
        return Observable_1.Observable.create(function (observer) {
            var dbtablename = "ClosedTables";
            //console.time("geting closed table " + tableName + " data fom db");
            var count = 0;
            var transaction = InfraSettings_1.InfraSettings.LogitudeIndexedDB.transaction([dbtablename], "readonly");
            var objectStore = transaction.objectStore(dbtablename);
            var request = objectStore.get(tableName);
            request.onerror = function (event) {
                //console.log("Unable to retrieve data from database!");
                observer.next("error");
            };
            request.onsuccess = function (event) {
                //console.timeEnd("geting closed table " + tableName + " data fom db");
                if (request.result) {
                    //if (dbtablename == "TextCodes") {
                    //    //window.TextCodes.concat(request.result.value);
                    //    for (var index in request.result.value) {
                    //        var item = request.result.value[index];
                    //        if (item) {
                    //            window.TextCodes.push(request.result.value[index]);
                    //        }
                    //    }
                    //}
                    return observer.next(request.result.value);
                }
                else {
                    //console.log(dbtablename + " " + tableName + " record couldn't be found in your database!");
                    observer.next(-1);
                }
            };
        });
    };
    IndexedDbService.prototype.GetByIndex = function (dbtablename, entityName) {
        return Observable_1.Observable.create(function (observer) {
            var count = 0;
            var start = new Date();
            var transaction = InfraSettings_1.InfraSettings.LogitudeIndexedDB.transaction([dbtablename]);
            transaction.oncomplete = function (event) {
                var end = new Date().getTime();
                var time = end - start;
                //console.log("reading all " + dbtablename + " " + entityName + " completed!" + ' Execution time: ' + time + ' count:' + count , "==============================================================================");
            };
            var objectStore = transaction.objectStore(dbtablename);
            var index = objectStore.index("EntityName");
            var singleKeyRange = IDBKeyRange.only(entityName);
            index.openCursor(singleKeyRange).onsuccess = function (event) {
                var cursor = event.target.result;
                if (cursor) {
                    count++;
                    // Do something with the matches.
                    //console.log("dbName " + dbtablename+ " Name: " + cursor.key + ", Entity: " + cursor.value.ResourcesTableName + ", Data: " + cursor.value);
                    if (dbtablename == "TextCodes") {
                        window.TextCodes.concat(cursor.value.value);
                        for (var index in cursor.value.value) {
                            window.TextCodes.push(cursor.value.value[index]);
                        }
                    }
                    if (dbtablename == "ObjectFields") {
                        window.ObjectFields.concat(cursor.value.value);
                        for (var index in cursor.value.value) {
                            window.ObjectFields.push(cursor.value.value[index]);
                        }
                    }
                    if (dbtablename == "AdvancedQueryFilters") {
                        window.AdvancedQueryFilters.concat(cursor.value.value);
                        for (var index in cursor.value.value) {
                            window.AdvancedQueryFilters.push(cursor.value.value[index]);
                        }
                    }
                    if (dbtablename == "Queries") {
                        window.Queries.concat(cursor.value.value);
                        for (var index in cursor.value.value) {
                            window.Queries.push(cursor.value.value[index]);
                        }
                    }
                    cursor.continue();
                }
                else {
                    return observer.next("ok"); //window.TextCodes
                }
            };
        });
        // var index = objectStore.index(['EntityName', 'TableName']);
        //  index.get("Donna,").onsuccess = function (event) {
        //      alert("Donna's SSN is " + event.target.result.ssn);
        //  };
    };
    //GetTableData(dbtablename: string) {
    //    var objectStore = InfraSettings.LogitudeIndexedDB.transaction(dbtablename).objectStore(dbtablename);
    //objectStore.openCursor().onsuccess = function (event) {
    //    var cursor = event.target.result;
    //    if (cursor) {
    //        alert("Name for id " + cursor.key + " is " + cursor.value.name + ", Age: " + cursor.value.age + ", Email: " + cursor.value.email);
    //        cursor.continue();
    //    }
    //    else {
    //        alert("No more entries!");
    //    }
    //};
    //}
    IndexedDbService.prototype.DeleteEntity = function (dbtablename, entityName) {
        var request = InfraSettings_1.InfraSettings.LogitudeIndexedDB.transaction([dbtablename], "readwrite")
            .objectStore(dbtablename)
            .delete(entityName);
        return request;
        //request.onsuccess = function (event) {
        //    console.log("entity has been removed from your database.");
        //};
    };
    IndexedDbService.prototype.getObjectStore = function (store_name, mode) {
        var tx = InfraSettings_1.InfraSettings.LogitudeIndexedDB.transaction(store_name, mode);
        return tx.objectStore(store_name);
    };
    IndexedDbService.prototype.clearObjectStore = function (store_name) {
        var store = this.getObjectStore(store_name, 'readwrite');
        var req = store.clear();
        return req;
        //req.onsuccess = function (evt) {
        //    //displayActionSuccess("Store cleared");
        //    //displayPubList(store);
        //};
        //req.onerror = function (evt) {
        //    console.error("clearObjectStore:", evt.target.errorCode);
        //    // displayActionFailure(this.error);
        //};
    };
    IndexedDbService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], IndexedDbService);
    return IndexedDbService;
}());
exports.IndexedDbService = IndexedDbService;
//# sourceMappingURL=IndexedDbService.js.map