declare var JSZip: any;
declare var window: any;
import { LocalStorageManager } from '../Utilities/LocalStorageManager';
import { WebWorkerService } from '../WebWorker/web-worker.service';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { SessionLocator } from '../Utilities/SessionLocator';
import { ObjectsLocator } from '../Locators/ObjectsLocator';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { IndexedDbService } from './IndexedDbService';
import { HttpClient } from '@angular/common/http';
import { share, flatMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class EntityResourceService {
  private _apiUrl: string;
  public DbService: IndexedDbService;
  public static ClosedTablesDataZipFilesDictionary: { [FileName: string]: any; } = {};
  public static ZipFilesDictionary: { [TableName: string]: Array<ZipFileDetails>; } = {};
  public static TablesLoadQueue: { [TableName: string]: any; } = {};
  public static ServerTablesUnzipQueue: { [TableName: string]: any; } = {};
  private _http: HttpClient;
  constructor() {
    this._apiUrl = ServiceHelper.GetLogitudeURL() + "api/EntityResource";
    this._http = ServiceHelper.HttpClient;
    this.DbService = new IndexedDbService();//InfraSettings.IndexedDbService;//
    window.CurrentService = this;
  }

  public getEntityResourceByTableName(objectTableName: string, tenant: number = 0) {

    try {
      //if (objectTableName == "Shipment") {
      //    var m;
      //    let test = m.text;
      //}
      if (!SessionLocator.UseCachedData) {
        return Observable.create(observer => {
          observer.next(1);
        });
      }

      if (EntityResourceService.ExisitsInCache(objectTableName)) {
        return Observable.create(observer => {
          observer.next(objectTableName);
        });
      }

      else {
        if (EntityResourceService.TablesLoadQueue[objectTableName]) {
          // if already exists in the load stack return the same obs
          return EntityResourceService.TablesLoadQueue[objectTableName].pipe(share());
        }

        var fieldsKey = objectTableName + "_ObjectFields.zip";
        var codesKey = objectTableName + "_TextCodes.zip";

        var entityFields = LocalStorageManager.GetItem(fieldsKey);
        var entityCodes = LocalStorageManager.GetItem(codesKey);

        var isMissingClosedTable: boolean = false;
        var objectTable: any = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
        if (objectTable && objectTable.IsClosed == true) {
          var storagefileName: string = objectTableName + "_ClosedData.zip";
          var fileString = LocalStorageManager.GetItem(storagefileName);
          if (fileString == null || fileString == undefined)
            isMissingClosedTable = true;
        }
        // var r = -1;
        // return this.DbService.GetTableData("TextCodes", objectTableName).flatMap(r=> {
        if (!entityFields || !entityCodes || isMissingClosedTable == true) {

          //var observable = Observable.timer(1).flatMap(function test() {
          //    var authHeader = new Headers();
          //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
          //    return this._http.get(this._apiUrl + '?objectTableName=' + objectTableName + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


          //        var filejson = response;
          //        if (filejson) {
          //            var fileData = EntityResourceService.base64ToBufferConvertor(filejson);

          //            return this.UnZipFileAndAddToStorageUsingWebWorker(fileData, objectTableName);
          //        }
          //        else {

          //            console.error("couldn't find " + objectTableName + ".zip file in the server!");
          //            return [];
          //        }
          //    });
          //}).pipe(share());

          var observable = this.GetResourcesFile(objectTableName, tenant).pipe(flatMap((response: any) => {
            var filejson = response;
            if (filejson) {
              if (EntityResourceService.ServerTablesUnzipQueue[objectTableName]) {

                // if already exists in the load stack return the same obs
                return EntityResourceService.ServerTablesUnzipQueue[objectTableName].pipe(share());
              }

              var fileData = EntityResourceService.base64ToBufferConvertor(filejson);
              var obs = this.UnZipFileAndAddToStorageUsingWebWorker(fileData, objectTableName).pipe(share());

              EntityResourceService.ServerTablesUnzipQueue[objectTableName] = obs;
              return obs;
            }

            else {

              console.error("couldn't find " + objectTableName + ".zip file in the server!");
              return [];
            }

          })).pipe(share());

          EntityResourceService.TablesLoadQueue[objectTableName] = observable;
          return observable.pipe(share());

        }
        else {

          EntityResourceService.ZipFilesDictionary[objectTableName] = [];
          if (entityFields) {
            var zFieldsObject = new ZipFileDetails();
            zFieldsObject.FileName = fieldsKey;
            zFieldsObject.FileData = EntityResourceService.base64ToBufferConvertor(entityFields);
            EntityResourceService.ZipFilesDictionary[objectTableName].push(zFieldsObject);
          }

          if (entityCodes) {
            var zCodesObject = new ZipFileDetails();
            zCodesObject.FileName = codesKey;
            zCodesObject.FileData = EntityResourceService.base64ToBufferConvertor(entityCodes);
            EntityResourceService.ZipFilesDictionary[objectTableName].push(zCodesObject);
          }

          let observable: Observable<{}> = this.UnZipFilesToMemoryUsingWebWorker(objectTableName);
          observable.pipe(share());

          EntityResourceService.TablesLoadQueue[objectTableName] = observable;

          return observable.pipe(share());
        }
      }
    }

    catch (error) {
      this.HandleError(error, objectTableName);
      if (ObjectsLocator.GlobalSetting && (ObjectsLocator.GlobalSetting.DeploymentStage == "Dev" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2")) {
        alert(error);
      }

      return Observable.create(observer => {
        observer.error(error);
      });
    }

  }

  private HandleError(error, objectTableName: string) {
    console.error("couldn't load entity resources for  " + objectTableName + " " + error);
  }

  private AddTableToCache(tableName: string) {

    //console.log("adding " + tableName + " table to cache");

    window.CachedTables.push(tableName)

    // if (window.CachedTables.length > 10) {
    //   window.CachedTables.splice(0, 10);
    //  }
  }

  private GetResourcesFile(objectTableName: string, tenant: number) {
    var url = this._apiUrl + '?objectTableName=' + objectTableName + '&tenant=' + tenant;

    return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(share());
  }

  public static base64ToBufferConvertor(str: string) {
    str = window.atob(str); // creates a ASCII string
    var buffer = new ArrayBuffer(str.length),
      view = new Uint8Array(buffer);
    for (var i = 0; i < str.length; i++) {
      view[i] = str.charCodeAt(i);
    }

    return buffer;

  }

  public static ExisitsInCache(objectTableName: string) {

    var table = objectTableName;
    if (objectTableName.indexOf("&") > -1) {
      table = objectTableName.split('&')[0];
    }

    var cachedTable = window.CachedTables.filter(d => (d == table))[0];

    if (cachedTable) {

      var tableExists: boolean = false;
      var fieldsExists: boolean = false;
      var codesExists: boolean = false;
      var objectTable: any = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
      fieldsExists = window.ObjectFields.some(d => d.ObjectTableId == objectTable.Id);
      codesExists = window.TextCodes.some(d => d.ObjectTableId == objectTable.Id);
      if ((fieldsExists == true && codesExists == true) || objectTableName == "General") {

        if (objectTable.IsClosed == true) {
          var storagefileName: string = objectTableName + "_ClosedData.zip";
          var fileString = LocalStorageManager.GetItem(storagefileName);
          if (fileString != null && fileString != undefined)
            tableExists = true;
          else
            console.warn(objectTableName + " Closed Table Data doesn't exist in cache");
        } else {
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
  }

  // =============================================================================

  public UnZipFilesToMemoryUsingWebWorker(objectTableName: string) {
    return Observable.create(observer => {

      console.time("Unzipping (TextCodes & Fields) from local cache for: " + objectTableName);
      var insideCount = 0;
      for (var key in EntityResourceService.ZipFilesDictionary[objectTableName]) {
        var zFile = EntityResourceService.ZipFilesDictionary[objectTableName][key];

        var worker = new WebWorkerService();
        var data = new ZipWorkerMessage();
        data.FileData = zFile.FileData;
        data.FileType = "string";
        data.FileName = objectTableName;

        //const promises = [];
        //promises.push(worker.runUrl('Infrastructure/WebWorker/JSZipWebWorker.js', data));
        var promise = worker.runUrl('Js/jszip-web-worker.js', data);
        promise.then(response => {

          insideCount++;
          var unzippedFiles = JSON.parse(response);

          for (key in unzippedFiles) {// loop the unzipped files


            var unzfile = unzippedFiles[key];
            //var message = JSON.parse(response)[0];//it contains only one file
            var nameArr = unzfile.FileName.split('_');
            var entityName = nameArr[0];
            var tableTypeName = nameArr[1].split('.')[0];

            var list = JSON.parse(unzfile.FileData);

            if (!EntityResourceService.ExisitsInCache(objectTableName)) {
              var objectTable: any = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
              switch (tableTypeName) {
                case "Codes":
                  var codesExists: boolean = false;
                  //if (objectTable)
                  //   codesExists = window.TextCodes.some(d => d.ObjectTableId == objectTable.Id);

                  if (codesExists == false)
                    window.TextCodes = window.TextCodes.concat(list);
                  else
                    console.log(objectTableName + " Text Codes already loaded in the memory");

                  if (!list || (list && list.length <= 0)) {
                    console.warn(objectTableName + " Unzipped local cache Text Codes are Empty!!")
                  }
                  if (SessionLocator.PrivateLableSettings) {
                    // window.TextCodes.filter(a => a.Code == "General.MH.Importers")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelName;
                    //window.TextCodes.filter(a => a.Code == "General.MH.ActivationWizard")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Services"; 
                  }
                  break;
                case "Fields":
                  var fieldsExists: boolean = false;
                  if (objectTable && objectTable.IsClosed == true)
                    fieldsExists = window.ObjectFields.some(d => d.ObjectTableId == objectTable.Id);

                  if (fieldsExists == false) {
                    window.ObjectFields = window.ObjectFields.concat(list);
                    EntityResourceService.FillObjectFieldsModifications();
                  }
                  else
                    console.log(objectTableName + " Object Fields already loaded in the memory");

                  if (!list || (list && list.length <= 0) && objectTableName != "General") {
                    console.warn(objectTableName + " Unzipped local cache Object Fields are Empty!!")
                  }
                  break;
              }

            }

            if (insideCount == EntityResourceService.ZipFilesDictionary[objectTableName].length) {

              console.timeEnd("Unzipping (TextCodes & Fields) from local cache for: " + objectTableName);

              var table = objectTableName;
              if (objectTableName.indexOf("&") > -1) {
                table = objectTableName.split('&')[0];
              }

              EntityResourceService.ZipFilesDictionary[objectTableName] = [];
              EntityResourceService.TablesLoadQueue[objectTableName] = null;
              window.CachedTables.push(table);

              worker.terminate(promise);
              observer.next(1);
            }


          }
        }
        )
          .catch(error => {
            this.HandleError(error, objectTableName);
          }
          );
      }


    }).pipe(share()); //.publish().refCount();


  }

  public UnZipFileAndAddToStorageUsingWebWorker(buffer: any, parentEntityName: string) {

    console.time("Unzipping Server File for: " + parentEntityName);

    EntityResourceService.ZipFilesDictionary[parentEntityName] = [];

    return Observable.create(observer => {

      var worker = new WebWorkerService();


      var count = 0;

      var zipdata = new ZipWorkerMessage();
      zipdata.FileData = buffer;
      zipdata.FileType = "base64";
      zipdata.FileName = parentEntityName;

      var promise = worker.runUrl('Js/jszip-web-worker.js', zipdata);
      promise.then(response => {

        var unzippedFiles = JSON.parse(response);

        for (key in unzippedFiles) {


          var unzfile = unzippedFiles[key];//JSON.parse(response);
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

          if (LocalStorageManager.SetItem(storagefileName, datm) == false) // write the file to the local storage
          {
            if (type == "ClosedData") {
              if (!EntityResourceService.ClosedTablesDataZipFilesDictionary[storagefileName]) {
                EntityResourceService.ClosedTablesDataZipFilesDictionary[storagefileName] = datm;
                console.log(entityName + " Closed Table Data stored to memory");
              }
            }
            console.warn(storagefileName + " Failed to be written on the storage!!!!!");
          }
          if (entityName == parentEntityName && type != "ClosedData") {  // decompress the files for the requested entity to the memory

            var add = true;
            if (EntityResourceService.ZipFilesDictionary[parentEntityName] && EntityResourceService.ZipFilesDictionary[parentEntityName].length > 0 && EntityResourceService.ZipFilesDictionary[parentEntityName].filter(d => d.FileName == unzfile.FileName).length > 0) {
              add = false;
            }

            if (add) {
              var zObject = new ZipFileDetails();
              zObject.FileName = unzfile.FileName;
              zObject.FileData = EntityResourceService.base64ToBufferConvertor(datm);
              EntityResourceService.ZipFilesDictionary[parentEntityName].push(zObject);
            }
          }
          //------------------------------------------------------------------
          if (count == fileCount) {

            console.timeEnd("Unzipping Server File for: " + parentEntityName);

            console.time("(TextCodes & Fields) (after server call) for table:" + parentEntityName);
            var insideCount = 0;
            for (var key in EntityResourceService.ZipFilesDictionary[parentEntityName]) {
              var zFile = EntityResourceService.ZipFilesDictionary[parentEntityName][key];


              var data = new ZipWorkerMessage();
              data.FileData = zFile.FileData;
              data.FileType = "string";

              var promise = worker.runUrl('Js/jszip-web-worker.js', data);
              promise.then(response => {

                insideCount++;

                var unzchildfile = JSON.parse(response)[0];// take the only file in the result
                var nameArr = unzchildfile.FileName.split('_');
                var entityName = nameArr[0];
                var tableTypeName = nameArr[1].split('.')[0];

                var list = JSON.parse(unzchildfile.FileData);

                if (!EntityResourceService.ExisitsInCache(entityName)) {
                  var objectTable: any = window.ObjectTables.filter(d => d.Name == entityName)[0];

                  switch (tableTypeName) {
                    case "Codes":
                      var codesExists: boolean = false;
                      //if (objectTable)
                      //   codesExists = window.TextCodes.some(d => d.ObjectTableId == objectTable.Id);

                      if (codesExists == false)
                        window.TextCodes = window.TextCodes.concat(list);
                      else
                        console.log(entityName + " Text Codes already loaded in the memory");


                      if (SessionLocator.PrivateLableSettings) {
                        // window.TextCodes.filter(a => a.Code == "General.MH.Importers")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelName;
                        //window.TextCodes.filter(a => a.Code == "General.MH.ActivationWizard")[0].DefaultText = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Services"; 
                      }
                      break;
                    case "Fields":
                      var fieldsExists: boolean = false;
                      if (objectTable && objectTable.IsClosed == true)
                        fieldsExists = window.ObjectFields.some(d => d.ObjectTableId == objectTable.Id);

                      if (fieldsExists == false) {
                        window.ObjectFields = window.ObjectFields.concat(list);
                        EntityResourceService.FillObjectFieldsModifications();
                      }
                      else
                        console.log(entityName + " Object Fields already loaded in the memory");
                      break;
                    default:
                      console.log("this is a closed table data " + entityName);
                      break;

                  }

                }

                if (insideCount == EntityResourceService.ZipFilesDictionary[parentEntityName].length) {

                  console.timeEnd("(TextCodes & Fields) (after server call) for table:" + parentEntityName);

                  var table = entityName;
                  if (entityName.indexOf("&") > -1) {
                    table = entityName.split('&')[0];
                  }

                  EntityResourceService.ZipFilesDictionary[entityName] = [];
                  EntityResourceService.TablesLoadQueue[entityName] = null;
                  EntityResourceService.ServerTablesUnzipQueue[parentEntityName] = null;

                  window.CachedTables.push(table);

                  worker.terminate(promise);
                  observer.next(1);
                }


              }
              ).catch(error => {
                this.HandleError(error, parentEntityName);
              }
              );



            }

          }

        }
      }).catch(error => {
        this.HandleError(error, parentEntityName);
      }
      );

    }).pipe(share()); //.publish().refCount()

  }

  public static FillObjectFieldsModifications() {

    if (window.ObjectFieldModifications != undefined && window.ObjectFieldModifications != null) {
      window.ObjectFieldModifications.forEach(ofMod => {
        var objectField = window.ObjectFields.filter(d => d.FieldCode == ofMod.ObjectFieldCode)[0];
        if (objectField) {
          objectField.IsRequired = objectField.IsRequiered = ofMod.IsRequired;
          objectField.MaxLength = ofMod.MaxLength;
          objectField.MinLength = ofMod.MinLength;

          window.ObjectFields.splice(window.ObjectFields.indexOf(objectField), 1, objectField);
          var objectField = window.ObjectFields.filter(d => d.FieldCode == ofMod.ObjectFieldCode)[0];
        }
      });
    }


  }
  //================================================================================
}

export class ZipFileDetails {
    public FileName: string;
    public FileData: any;
}

export class ZipWorkerMessage {
    public FileName: string;
    public FileType: string;
    public FileData: any;
}
