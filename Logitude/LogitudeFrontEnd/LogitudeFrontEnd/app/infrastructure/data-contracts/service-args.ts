import {Injectable} from 'angular2/core';
import {Http, Headers} from 'angular2/http';
 
@Injectable()
export class ServiceArgs {
    public http: Http;
    public objectTableName: string;
}
