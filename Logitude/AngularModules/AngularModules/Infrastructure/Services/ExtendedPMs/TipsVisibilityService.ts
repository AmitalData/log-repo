
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Observable';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {TipsVisibilityPM} from '../../EntityPMs/TipsVisibilityPM';


@Injectable()
export class TipsVisibilityService {
    private _http: Http;
    private _apiUrl: string;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TipsVisibility';

    }



    insert(entityPM: TipsVisibilityPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];


            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TipsVisibilityPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: TipsVisibilityPM;
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


    update(entityPM: TipsVisibilityPM) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = [];


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: TipsVisibilityPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: TipsVisibilityPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }


                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);

            }
        }

        );

    }

    



    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TipsVisibilityPM = null) {


        if (!entityPM) {

            entityPM = new TipsVisibilityPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
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

		  public GetNewEntityPM() {
        var entityPM: TipsVisibilityPM;
        entityPM = new TipsVisibilityPM();

        return entityPM;
    }


}
