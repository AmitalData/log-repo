/// <reference path="../../../infrastructure/datacontracts/serviceresponse.ts" />

import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
//import 'rxjs/add/operator/map';
//import Rx from 'rxjs/Rx';
import {Observable}     from 'rxjs/Rx';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ViewResponse} from '../../../Infrastructure/DataContracts/ViewResponse';
import {HybridPartnerList} from '../../EntityLists/HybridPartnerList';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
@Injectable()
export class HybridPartnerExtendedListService {

    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
	private CachedData: Array<HybridPartnerList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/HybridPartnerExtendedList';  
    }

  //  setServiceArgs(serviceArgs: ServiceArgs) {
  //      this._serviceArgs = serviceArgs;
  //      this._http = serviceArgs.http;
  //      this._apiUrl = logitude_url + 'api/HybridPartnerExtendedList';
		//this.CachedData = [];
  //  }
    

    GetHybridPartnerLists(tenant : number) {
	   var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
       return Observable.defer(() => {
           return this._http.get(this._apiUrl + '/GetHybridPartnerLists/?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(response => {

              var allLists = response.json();
              var _mappedListsArray: Array<HybridPartnerList> = [];
		      if(allLists)
			  {
				for (var key in  allLists) {
				
				   var entity: HybridPartnerList;
                   entity = this.MapJsonToEntityList(allLists[key]);
				   _mappedListsArray.push(entity);

				 }
               }
                return _mappedListsArray;
            });
        }

        );
    }

    GetHybridPartnerListWithNoRequest(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetHybridPartnerListWithNoRequest/?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<HybridPartnerList> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: HybridPartnerList;
                        entity = this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }
                return _mappedListsArray;
            });
        }

        );
    }




    GetAllowdHybridPartnerLists(hybridPartnerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAllowdHybridPartnerLists/?' + 'hybridPartnerId=' + hybridPartnerId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = allLists;
                return pmresponse;
            });
        }

        );
    }

    GetAllowingHybridPartnerLists(hybridPartnerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAllowingHybridPartnerLists/?' + 'hybridPartnerId=' + hybridPartnerId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = allLists;




                return pmresponse;
            });
        }

        );
    }

	MapJsonToEntityList(jsonList: any) {
       
            var entityList: HybridPartnerList;
            entityList = new HybridPartnerList();
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityList[property] = jsonList[property];
            }
			

        return entityList;
    }

}

