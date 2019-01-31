
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {Observable}     from 'rxjs/Rx';

import {QueryPM} from '../../EntityPMs/QueryPM';


@Injectable()
export class QueriesPMService {

    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/objectfields';    
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/queries';
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

    insert(entityPM: QueryPM) {

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
                var mappedEntity: QueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: QueryPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
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

    update(entityPM: QueryPM) {

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
                var mappedEntity: QueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: QueryPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
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

    delete(entityPM: QueryPM) {

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
                var mappedEntity: QueryPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.delete(this._apiUrl + '?id=' + entityPM.Id + '&tenant=' + entityPM.Tenant,
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: QueryPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
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

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: QueryPM = null) {


        if (!entityPM) {

            entityPM = new QueryPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        //entityPM.IsDirty = false;

        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entityPM;
    }

}
