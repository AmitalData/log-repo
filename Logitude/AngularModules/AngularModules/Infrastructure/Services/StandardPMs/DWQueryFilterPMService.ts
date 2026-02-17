
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {DWQueryFilterPM} from '../../EntityPMs/DWQueryFilterPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()
export class DWQueryFilterPMService {

    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {

    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DWQueryFilter';
    }

    getDWqueryfiltersbytenant(tenant: number, userid: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getDWqueryfiltersbytenant?' + 'tenant=' + tenant + '&loggedcontactid=' + userid, {
                headers: authHeader
            }).map(response => {
                var pms = response.json();

                return pms;
            });
        }

        );

    }

    getuserDWqueryfilterbytenantobjecttablequery(tenant: number, dwobjecttableid: string, dwqueryid: string, userid: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getDWqueryfiltersbytenantuserobjecttablequery?' + 'tenant=' + tenant + '&dwobjecttableid=' + dwobjecttableid + '&dwqueryid=' + dwqueryid + '&loggedcontactid=' + userid, {
                headers: authHeader
            }).map(response => {
                var pms = response.json();

                return pms;
            });
        }

        );

    }

    insert(entityPM: DWQueryFilterPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("DWQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWQueryFilterPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DWQueryFilterPM;
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

    delete(entityPM: DWQueryFilterPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("DWQueryFilter", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DWQueryFilterPM;

                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DWQueryFilterPM;
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

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: DWQueryFilterPM = null) {


        if (!entityPM) {

            entityPM = new DWQueryFilterPM();
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
