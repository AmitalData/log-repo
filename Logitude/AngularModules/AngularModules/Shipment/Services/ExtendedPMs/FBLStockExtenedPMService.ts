 
import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {FBLStockPM} from '../../EntityPMs/FBLStockPM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()

export class FBLStockExtenedPMService {
    private _httpClient: HttpClient
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FBLStockExtened';
    }

    GetAllFBLStockPMsByTenant(tenant: number) {

        var url = this._apiUrl + '/GetAllFBLStockPMsByTenant?tenant=' + tenant ;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<FBLStockPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: FBLStockPM = this.MapJsonToEntityPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetAllFBLStockPMsCountByTenant(tenant: number) {

        var url = this._apiUrl + '/GetAllFBLStockPMsCountByTenant?tenant=' + tenant;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

               
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetFBLStockPMsByTenant(tenant: number, pageSize: number, pageIndex: number) {

        var url = this._apiUrl + '/GetFBLStockPMsByTenant?tenant=' + tenant + '&pageSize=' + pageSize + '&pageIndex=' + pageIndex;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<FBLStockPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: FBLStockPM = this.MapJsonToEntityPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }



    CreateFBLStocksOperation( myStartNumber: number, myEndNumber: number) {

        var url = this._apiUrl + '/GetCreateFBLStocksOperation?myStartNumber=' + myStartNumber + '&myEndNumber=' + myEndNumber;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    DeleteFBLStocksOperation(myStackId: string, isDeletingSeries: boolean) {

        var url = this._apiUrl + '/GetDeleteFBLStocksOperation?myStackId=' + myStackId  + '&isDeletingSeries=' + isDeletingSeries;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAllAvailableStockSeries() {

        var url = this._apiUrl + '/GetAllAvailableStockSeries';

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: FBLStockPM = null) {


        if (!entityPM) {

            entityPM = new FBLStockPM();
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


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }


    public clone(jsonPM: any) {
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

    public GetNewEntityPM() {
        var entityPM: FBLStockPM;
        entityPM = new FBLStockPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }


}
