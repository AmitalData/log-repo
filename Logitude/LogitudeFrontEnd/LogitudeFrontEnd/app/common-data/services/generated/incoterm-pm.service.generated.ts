import {Injectable} from 'angular2/core';
import {Http, Headers} from 'angular2/http';
import 'rxjs/add/operator/map';
import Rx from 'rxjs/Rx';
import {ServiceArgs} from '../../../infrastructure/data-contracts/service-args';

import {IncotermPM} from '../../entity-pms/generated/IncotermPM.generated';


@Injectable()
export class IncotermPMService {

 private _apiUrl: string = 'http://localhost:9996/api/incoterms';
 private _http: Http;
 private _serviceArgs: ServiceArgs;
 constructor() {
        
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
    }

 get(id: string) {
         
        console.log('--------------------------------------> calling getSingleEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
		
		 return Rx.Observable.defer(() => {
                return this._http.get(this._apiUrl+'?'+'id=' + id, {
                    headers: authHeader
                }).map(response => {
                    var pm = response.json();
                    
					
                    var entity: IncotermPM;
                    entity = this.MapJsonToEntityPM(pm);
                    
					return entity;
                });
            }

            );
       

     
         
    }

	 insert(entityPM: IncotermPM) {
        console.log('--------------------------------------> calling updateEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
        authHeader.append('Content-Type', 'application/json');
        console.log('server call ---------');
        return Rx.Observable.defer(() => {
            return this._http.post(this._apiUrl, JSON.stringify(entityPM), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();

                return result;
            });
        }

        );
    }

    update(entityPM: IncotermPM) {

        console.log('--------------------------------------> calling updateEntityPM:');
        var authHeader = new Headers();
        authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
        authHeader.append('Content-Type', 'application/json');
        console.log('server call ---------');
        return Rx.Observable.defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(entityPM),{
                headers: authHeader,
                
            }).map(response => {
                var result = response.json();
                
                return result;
            });
        }

        );

    }

   

	    MapJsonToEntityPM(jsonPM: any) {
       
            var entityPM: IncotermPM;
            entityPM = new IncotermPM();
            var jsonPMKeys = Object.keys(jsonPM);

            for (var key in jsonPMKeys) {
                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];
            }
			
					 
            entityPM.IsDirty = false;

        return entityPM;
    }


}
