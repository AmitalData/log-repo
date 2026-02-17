import {Injectable, Injector, Inject} from '@angular/core';
import {Http, Headers, ConnectionBackend, BaseRequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {PortList} from '../../EntityLists/PortList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

@Injectable()

export class PortService {
    private _apiUrl: string;
    private _http: Http;   
    constructor() {
        this._http = ServiceHelper.Http;
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
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getportcopytocurrenttenant/?' + 'id=' + zeroPortId, {
                headers: authHeader
            }).map(response => {
                var list = response.json();

                var entity: PortList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                return entity;
            });
        }

        );
    }

    getAll() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(ServiceHelper.GetLogitudeURL() + 'api/ngMetaData?tenant=' + SessionInfo.LoggedUserTenant +'&inActive=false&inland=true&air=true&ocean=true', { headers: authHeader })
            .map(ports => { /*console.log(ports.json());*/ return ports.json(); });
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
