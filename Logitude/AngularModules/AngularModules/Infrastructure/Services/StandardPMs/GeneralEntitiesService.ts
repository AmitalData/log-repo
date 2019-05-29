
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
//import Rx from 'rxjs/Rx';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {GeneralEntitiesArgs} from '../../DataContracts/GeneralEntitiesArgs';


@Injectable()
export class GeneralEntitiesService {

    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {

    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/webfreightdomain';
    }

    //getadvancedqueryfiltersbytenant(tenant: number, userid: string) {


    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

    //    return Rx.Observable.defer(() => {
    //        return this._http.get(this._apiUrl + '/getadvancedqueryfiltersbytenant?' + 'tenant=' + tenant + '&loggedcontactid=' + userid, {
    //            headers: authHeader
    //        }).map(response => {
    //            var pms = response.json();

    //            return pms;
    //        });
    //    }

    //    );

    //}

    insert(entities: GeneralEntitiesArgs) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: GeneralEntitiesArgs;
                mappedEntity = this.MapJsonToEntityPM(entities, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: GeneralEntitiesArgs;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entities);
                            response.Result = mappedResult;
                        }



                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }

    update(entities: GeneralEntitiesArgs) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("AdvancedQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: GeneralEntitiesArgs;
                mappedEntity = this.MapJsonToEntityPM(entities, false);

                return this._http.put(this._apiUrl + "/PutGeneralEntitiesArgs", JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: GeneralEntitiesArgs;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entities);
                            response.Result = mappedResult;
                        }



                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }
     
    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entities: GeneralEntitiesArgs = null) {


        if (!entities) {

            entities = new GeneralEntitiesArgs();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entities[property] = jsonPM[property];
        }


        //entityPM.IsDirty = false;

        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entities;
    }

}
