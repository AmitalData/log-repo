import {Injectable, Injector, Inject} from '@angular/core';
import { ConnectionBackend, BaseRequestOptions} from '@angular/http';
import { defer, of } from 'rxjs';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {PortList} from '../../EntityLists/PortList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
@Injectable()

export class PortService {
    private _apiUrl: string;
    private _http: HttpClient;   
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/portlistviewsexteded';
        //this.CachedData = [];
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        //this._serviceArgs = serviceArgs;
        //this._http = serviceArgs.http;
        //this._apiUrl = logitude_url + 'api/portlistviewsexteded';
        
    }

    GetPortCopyToCurrentTenant(zeroPortId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/getportcopytocurrenttenant/?' + 'id=' + zeroPortId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: PortList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                return entity;
            }));
        }

        );
    }

    getAll() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(ServiceHelper.GetLogitudeURL() + 'api/ngMetaData?tenant=' + SessionInfo.LoggedUserTenant + '&inActive=false&inland=true&air=true&ocean=true', ServiceHelper.GetHttpHeaders()).pipe(map(ports => {  return ports; }));
    }

    
    MapJsonToEntityList(jsonList: any) {

        var entityList: PortList;
        entityList = new PortList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

   
}
