
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Observable';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {ErrorLogPM} from '../../EntityPMs/ErrorLogPM';


@Injectable()
export class ErrorsLogPMService {
    private _http: Http;
    private _apiUrl: string;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/errorlogs';

    }

    //setServiceArgs(serviceArgs: ServiceArgs) {
    //    this._serviceArgs = serviceArgs;

    //}

    //get(code: string) {


    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

    //    return Observable.defer(() => {
    //        return this._http.get(this._apiUrl + '/getsingle?' + 'code=' + code, {
    //            headers: authHeader
    //        }).map(response => {
    //            var pm = response.json();


    //            var entity: ChargesGroupPM;
    //            if (pm) {
    //                entity = this.MapJsonToEntityPM(pm);
    //            }
    //            return entity;
    //        });
    //    }

    //    );




    //}

    insert(entityPM: ErrorLogPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];//validator.Validate("ErrorLog", entityPM);


            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ErrorLogPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: ErrorLogPM;
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

   


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ErrorLogPM = null) {


        if (!entityPM) {

            entityPM = new ErrorLogPM();
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

        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entityPM;
    }


    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

		  public GetNewEntityPM() {
              var entityPM: ErrorLogPM;
              entityPM = new ErrorLogPM();

        return entityPM;
    }


}
