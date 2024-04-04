import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomsSettingList} from '../../EntityLists/CustomsSettingList';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';
import { CustomsClosedTablePM } from '../../EntityPMs/CustomsClosedTablePM';


export class CustomsSettingExtendedListService {
    

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsSettingExtended';
    }

    GetCustomsClosedTablePMByObjectTableId(objectTableId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsClosedTablePMByObjectTableId/?objectTableId='+objectTableId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSettingByTenant() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSettingByTenant/?', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: CustomsSettingList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetLastRunningDCAWS() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetLastRunningDCAWS/?', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
              
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    private static _AppSetting: KeyValue[] = [];
    GetAppSettingByCode(key: string): any {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            let res = CustomsSettingExtendedListService._AppSetting.filter(r => r.Key == key);
            if (res.length == 1) {
                let val = res[0].Value;
                let serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = { val: val };

                return of(serviceResponse);
            }
            return this._http.get(this._apiUrl + '/GetAppSettingByCode/?key=' + key.toString() , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var obj = response;
                let val: string = (obj as any).val;
                CustomsSettingExtendedListService._AppSetting.push(new KeyValue(key,val))
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = obj;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSincroOption(tenant: number, SincroScreen: string,objectTable:string): any {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSincroOption/?SincroScreen=' + SincroScreen.toString() + '&tenant=' + tenant.toString()+'&objectTable=' + objectTable , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var obj = response;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    PostSincroOption(genericRequestParams: GenericRequestParams) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/PostSincroOption/',
                JSON.stringify(genericRequestParams),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetAmitalRestrictOwnerModel(getFromCache: boolean) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAmitalRestrictOwnerModel/?getFromCache=' + getFromCache.toString(), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: CustomsSettingList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapJsonToEntityList(jsonList: any) {

        var entityList: CustomsSettingList;
        entityList = new CustomsSettingList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
    GetSkipAutoInsurancePromise(customerCode: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSkipAutoInsurance/?customerCode=' + customerCode + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var obj = response;

                    //var entity: CustomsSettingList;
                    //if (list) {
                    //    entity = this.MapJsonToEntityList(list);
                    //}

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetInsurancePercentDefault(customerCode: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetInsurancePercentDefault/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var obj = response;

                    //var entity: CustomsSettingList;
                    //if (list) {
                    //    entity = this.MapJsonToEntityList(list);
                    //}

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDefault(DISTRID: string, DEFID: string, BRANCHID: string, CARDID: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        DISTRID = DISTRID || "ISRAEL";
        BRANCHID = BRANCHID || "NON";
        CARDID = CARDID || "NON";
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDefault/?' +
                'DISTRID=' + DISTRID.toString() +
                '&DEFID=' + DEFID.toString() +
                '&BRANCHID=' + BRANCHID.toString() +
                '&CARDID=' + CARDID.toString() +
                '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var obj = response;

                    //var entity: CustomsSettingList;
                    //if (list) {
                    //    entity = this.MapJsonToEntityList(list);
                    //}

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }


}
class KeyValue {
    constructor(public Key: string, public Value) { }
}
