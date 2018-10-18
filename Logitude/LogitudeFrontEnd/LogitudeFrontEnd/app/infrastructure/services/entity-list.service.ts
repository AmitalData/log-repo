import {Injectable, provide, Injector} from 'angular2/core';
import {Http, HTTP_PROVIDERS} from 'angular2/http';
import {ServiceArgs} from '../data-contracts/service-args';
@Injectable()
export class EntityListService {

    constructor(public http: Http, public serviceArgs: ServiceArgs) {
        this.serviceArgs.http = this.http;
    }

    getAll(objectTableName:string) {

        var serviceType;
        var servicename = objectTableName + "Service";
        var servicelink = './app/shipment/services/' + objectTableName + '.service';

        return System.import(servicelink)
            .then(m => {

                serviceType = m[servicename];
                var service = Object.create(serviceType.prototype);
                service.setServiceArgs(this.serviceArgs);
                return service.getAll();
                
            })
    }

}