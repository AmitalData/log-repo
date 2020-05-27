
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InfraGenericFilter } from '../../../Infrastructure/Utilities/InfraGenericFilter';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';


@Injectable()

export class SignStationExtendedListService {
    private _http: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<SignStationList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        //CustomsRequestsSheetViewsController
        //CustomsSettingExtended
        //CustomsRequestsSheetViews
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SignStationExtended';
    }

    getSingle(declarationid: string, invoicecounterkey: number, lineNumber: number) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'declarationid=' + declarationid + '&' + 'invoicecounterkey=' + invoicecounterkey + '&' + 'lineNumber=' + lineNumber, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: SignStationList;
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

    getAll() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<SignStationList> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: SignStationList;
                        entity = this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

   

    GetSignStationGroupByStatus(searchfields: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(
                this._apiUrl + '/GetSignStationGroupByStatus?' + "&searchfields=" + searchfields ,
                ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = response;
                    //var _mappedListsArray: Array<SignStationGroup> = [];
                    

                    //if (serviceResponse.Result) {
                    //    _mappedListsArray = serviceResponse.Result;
                    //}
                    //serviceResponse.Result = _mappedListsArray;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getByFilters//(filters: ApiQueryFilters) {
        (skip, take, sortingCol, sortingDir, getCount: boolean, searchfields: string, FilterByStatus:string ){

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(
                this._apiUrl + '/GetSignStations?' + "&skip=" + skip.toString() + "&take=" + take.toString() + "&sortingCol=" + sortingCol.toString() + "&sortingDir=" + sortingDir.toString() + "&searchfields=" + searchfields + "&FilterByStatus=" + FilterByStatus.toString(),
                ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = response;
                    var _mappedListsArray: Array<SignStationList> = [];
                    //if (serviceResponse.Result) {
                    //    for (var key in serviceResponse.Result) {

                    //        var entity: SignStationList;
                    //        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                    //        _mappedListsArray.push(entity);

                    //    }
                    //}

                    if (serviceResponse.Result) {
                        _mappedListsArray = serviceResponse.Result;
                    }
                    serviceResponse.Result = _mappedListsArray;
                    return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {

        return jsonList;
        //var entityList: SignStationList;
        //entityList = new SignStationList();
        //var jsonListKeys = Object.keys(jsonList);

        //for (var key in jsonListKeys) {
        //    var property = jsonListKeys[key];
        //    entityList[property] = jsonList[property];
        //}


        //return entityList;
    }

}

export interface SignStationList {
    PersonId: string;
    SignerName: string;
    CustomsAgentId: string;
    MachineName: string;
    MachineUser: string;




    IsPersonalSignOn: boolean;
    IsCompanySignOn: boolean;

    Status: string;
    LastSignAt?: Date;
    IsOk?: boolean;
    VersionByFeatures: string;
    //IsOk: boolean | null;
}
export interface SignStationGroup{ Key: string, Total: number }

//export class SignStationList {
//    public PersonId: string;
//    public SignerName: string;
//    public CustomsAgentId: string;
//    public MachineName: string;
//    public MachineUser: string;


    

//    public IsPersonalSignOn: boolean;
//    public IsCompanySignOn : boolean;

//    public Status: string;
//    public LastSignAt: Date;
//    public IsOk?: boolean;
//}