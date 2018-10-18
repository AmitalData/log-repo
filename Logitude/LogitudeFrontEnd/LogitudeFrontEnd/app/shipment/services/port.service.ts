import {Injectable, Injector, provide, Inject} from 'angular2/core';
import {Http, Headers, ConnectionBackend, BaseRequestOptions} from 'angular2/http';
import Rx from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceArgs} from '../../infrastructure/data-contracts/service-args';


@Injectable()
export class PortService {

    private _apiUrl: string = "http://localhost:9996/api/ngMetaData";
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {
        console.warn("port Service instantiated");
       
    }

    getAll() {
        return this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&inActive=false&inland=true&air=true&ocean=true')
            .map(ports => { /*console.log(ports.json());*/ return ports.json(); });
    }
   

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
    }
}
