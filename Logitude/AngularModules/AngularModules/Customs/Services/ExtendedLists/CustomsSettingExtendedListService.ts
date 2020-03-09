import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomsSettingList} from '../../EntityLists/CustomsSettingList';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';


export class CustomsSettingExtendedListService {
   

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsSettingExtended';
    }


    GetSettingByTenant() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSettingByTenant/?', { headers: authHeader }).map(response => {
                var list = response.json();

                var entity: CustomsSettingList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSincroOption(tenant: number, SincroScreen: string): any {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSincroOption/?SincroScreen=' + SincroScreen.toString() + '&tenant=' + tenant.toString() , { headers: authHeader })
                .map(response => {
                    var obj = response.json();


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }
    PostSincroOption(genericRequestParams: GenericRequestParams) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/PostSincroOption/',
                JSON.stringify(genericRequestParams),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    GetAmitalRestrictOwnerModel(getFromCache: boolean) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAmitalRestrictOwnerModel/?getFromCache=' + getFromCache.toString(), { headers: authHeader }).map(response => {
                var list = response.json();

                var entity: CustomsSettingList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSkipAutoInsurance/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, { headers: authHeader })
                .map(response => {
                    var obj = response.json();

                    //var entity: CustomsSettingList;
                    //if (list) {
                    //    entity = this.MapJsonToEntityList(list);
                    //}

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetInsurancePercentDefault(customerCode: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetInsurancePercentDefault/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, { headers: authHeader })
                .map(response => {
                    var obj = response.json();

                    //var entity: CustomsSettingList;
                    //if (list) {
                    //    entity = this.MapJsonToEntityList(list);
                    //}

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetDefault(DISTRID: string, DEFID: string, BRANCHID: string, CARDID: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        DISTRID = DISTRID || "ISRAEL";
        BRANCHID = BRANCHID || "NON";
        CARDID = CARDID || "NON";
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDefault/?' +
                'DISTRID=' + DISTRID.toString() +
                '&DEFID=' + DEFID.toString() +
                '&BRANCHID=' + BRANCHID.toString() +
                '&CARDID=' + CARDID.toString() +
                '&tenant=' + tenant, { headers: authHeader })
                .map(response => {
                    var obj = response.json();

                    //var entity: CustomsSettingList;
                    //if (list) {
                    //    entity = this.MapJsonToEntityList(list);
                    //}

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = obj;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

}
