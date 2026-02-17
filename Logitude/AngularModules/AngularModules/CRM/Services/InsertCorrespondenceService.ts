import {Injectable} from '@angular/core';
import {Http, Headers, URLSearchParams} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceArgs} from '../../Infrastructure/DataContracts/ServiceArgs';
import {CorrespondencePM} from '../EntityPMs/CorrespondencePM';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {EntityPMServiceResponse} from '../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';


@Injectable()

export class InsertCorrespondenceService {
    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {

    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InsertCorrespondence';
    }

    insert(entityPM: CorrespondencePM) {

        console.log('--------------------------------------> calling insert CorrespondencePM:');
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Correspondence", entityPM);

            var response: EntityPMServiceResponse;
            response = new EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: CorrespondencePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: CorrespondencePM;
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
        });
    }

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: CorrespondencePM = null) {

        if (!entityPM) {

            entityPM = new CorrespondencePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        if (getCallMap) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }

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
}