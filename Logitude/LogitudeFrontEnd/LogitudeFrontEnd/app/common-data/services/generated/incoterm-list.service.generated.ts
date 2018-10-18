import {Injectable} from 'angular2/core';
import {Http, Headers} from 'angular2/http';
import 'rxjs/add/operator/map';
import Rx from 'rxjs/Rx';
import {ServiceArgs} from '../../../infrastructure/data-contracts/service-args';
import {ApiQueryFilters} from '../../../infrastructure/data-contracts/api-query-filters';
import {IncotermList} from '../../entity-lists/generated/IncotermList.generated';
@Injectable()
export class IncotermListService {

    private _apiUrl: string = 'http://localhost:9996/api/incotermviews';
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {
        
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
    }

    getSingle(id: string) {
	    console.log('--------------------------------------> calling getSingle:');
        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');

        return Rx.Observable.defer(() => {
            return this._http.get(this._apiUrl+'?'+'id=' + id, {
                headers: authHeader
            }).map(response => {
                var list = response.json();
                    
                //var entity: IncotermList;
                // entity = this.MapJsonToEntityList(list);
                    
                return list;
            });
        }

        );
    }

    getAll() {
        console.log('--------------------------------------> calling getAllEntityListsFromServer:');
        return this._http.get(this._apiUrl)
            .map(response => {
                var dataList = response.json();
                console.log(response.json());
                return Promise.resolve(dataList);
            });
    }

	
    getByFilters(filters: ApiQueryFilters) {
        console.log('--------------------------------------> calling the server with filters:');
        var urlparameters = '?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("AdditionalFilters=").concat(JSON.stringify(propValue));
        }

        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
        var callUrl = this._apiUrl.concat(urlparameters);//
        console.log("Calling Url:" + callUrl);

        this._http.get(callUrl, {
            headers: authHeader
        }).subscribe(response => {
            console.log('--------------------------------------> server Response:');
            console.log(response.json());
            console.log('TotalCount:' + response.headers.get('TotalCount'))
        });
    }
}

