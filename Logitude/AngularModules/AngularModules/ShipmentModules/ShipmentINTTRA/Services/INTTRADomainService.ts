import {Injectable} from '@angular/core';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {BranchPM} from '../../../Common/EntityPMs/BranchPM';
import {INTTRASettingPM} from '../../../Common/EntityPMs/INTTRASettingPM';
import {BranchPMService} from '../../../Common/Services/StandardPMs/BranchPMService';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';

@Injectable()

export class INTTRADomainService {
    private _apiUrl: string;
  private _http: HttpClient;
    constructor() {
      this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/INTTRADomain';
    }

    GetINTTRASettings() {

        var url = this._apiUrl + '/GetINTTRASettings';

        return defer(() => {
          return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var itemJSON = response;
                var itemMapped: INTTRASettingsHelper = this.MapINTTRASettingsHelper(itemJSON);

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;

          }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    UpdateINTTRASettings(entityPM: INTTRASettingsHelper) {
        return defer(() => {

            var mappedEntity: INTTRASettingsHelper = this.MapINTTRASettingsHelper(entityPM, false);

          return this._http.put(this._apiUrl + '/PutINTTRASettings', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: INTTRASettingsHelper = this.MapINTTRASettingsHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

          }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapINTTRASettingsHelper(jsonPM: any, getCallMap: boolean = true, entity: INTTRASettingsHelper = null) {
        if (!entity) {
            entity = new INTTRASettingsHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        var myBranchPMService = new BranchPMService();

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "INTTRASetting") {
                if (jsonPM[property]) {
                    entity[property] = this.MapJsonToINTTRASettingPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Branches") {

                entity.Branches = new Array<BranchPM>();

                for (var item in jsonPM.Branches) {
                    var jItem = jsonPM.Branches[item];

                    var newBranchPM: BranchPM = myBranchPMService.MapJsonToEntityPM(jItem, getCallMap);

                    entity.Branches.push(newBranchPM);
                }
            }
            else if (property === "Items") {

                entity.Items = new Array<INTTRASettingsHelperItem>();

                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];

                    var newItemPM: INTTRASettingsHelperItem = this.MapJsonToINTTRASettingsHelperItem(jItem, getCallMap);

                    entity.Items.push(newItemPM);
                }
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
    MapJsonToINTTRASettingPM(jsonPM: any, mapParent: boolean = true, entityPM: INTTRASettingPM = null) {
        if (!entityPM) {

            entityPM = new INTTRASettingPM();
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
    MapJsonToINTTRASettingsHelperItem(jsonPM: any, mapParent: boolean = true, entityPM: INTTRASettingsHelperItem = null) {

        if (!entityPM) {

            entityPM = new INTTRASettingsHelperItem();
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

    GetINTTRACommunicationSettings() {

        var url = this._apiUrl + '/GetINTTRACommunicationSettings';

        return defer(() => {
          return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var itemJSON = response;
                var itemMapped: INTTRACommunicationSettingsHelper = this.MapINTTRACommunicationSettingsHelper(itemJSON);

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;

          }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    UpdateINTTRACommunicationSettings(entityPM: INTTRACommunicationSettingsHelper) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: INTTRACommunicationSettingsHelper = this.MapINTTRACommunicationSettingsHelper(entityPM, false);

          return this._http.put(this._apiUrl + '/PutINTTRACommunicationSettings', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: INTTRACommunicationSettingsHelper = this.MapINTTRACommunicationSettingsHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

          }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapINTTRACommunicationSettingsHelper(jsonPM: any, getCallMap: boolean = true, entityPM: INTTRACommunicationSettingsHelper = null) {
        if (!entityPM) {
            entityPM = new INTTRACommunicationSettingsHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }
}

export class INTTRASettingsHelper {
    Id: string;
    INTTRASetting: INTTRASettingPM;
    Branches: BranchPM[];
    Items: INTTRASettingsHelperItem[];
}
export class INTTRASettingsHelperItem {
    CompinedId: string;
    Code: string;
    Name: string;
    Notes: string;
    IsLineItem: boolean;
    BranchId: string;
    RegisteredCarrierId: string;
    IsRegistered: boolean;
    IsRegistered_Old: boolean;
    Tenant: number;
    IsFromTenantZero: boolean;
    ShippingLineId: string;
    UpdatesShipmentsDates: boolean;
    UpdatesShipmentsDates_Old: boolean;
}
export class INTTRACommunicationSettingsHelper {
    Id: string;
    INTTRAProdFTPHost: string;
    INTTRATestFTPHost: string;
}
