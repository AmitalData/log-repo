import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import { defer, of } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomsSettingList} from '../../EntityLists/CustomsSettingList';


export class CustomsSettingExtendedListService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsSettingExtended';
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
            return this._http.get(this._apiUrl + '/GetSkipAutoInsurance/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, { headers: authHeader })
                .map(response => {
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
            return this._http.get(this._apiUrl + '/GetInsurancePercentDefault/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, { headers: authHeader })
                .map(response => {
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
                '&tenant=' + tenant, { headers: authHeader })
                .map(response => {
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