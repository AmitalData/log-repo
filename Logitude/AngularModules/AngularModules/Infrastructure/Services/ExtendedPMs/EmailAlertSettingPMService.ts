
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Observable';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';
import {EntityPMServiceResponse} from '../../DataContracts/EntityPMServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import { EmailAlertSettingPM } from '../../EntityPMs/EmailAlertSettingPM';


@Injectable()
export class EmailAlertSettingPMService {
    private _http: Http;
    private _apiUrl: string;
    private _serviceArgs: ServiceArgs;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EmailAlertSetting';

    }


    getAllEmailAlerts(tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetEmailAlertSettingsByTenant?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(response => {
                var mappedAlerts: Array<EmailAlertSettingPM> = [];
                var alerts = response.json();
                for (var k in alerts)
                {

                    var entity: EmailAlertSettingPM;
                    entity = this.MapJsonToEntityPM(alerts[k]);
                    mappedAlerts.push(entity);
                }

                serviceResponse.Result = mappedAlerts;

                return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }); 




    }


    updateAllAlerts(allAlerts :EmailAlertSettingPM[], tenant:number ) {


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
                var mappedAlerts: Array<EmailAlertSettingPM> = [];
                
                for (var k in allAlerts) {

                    var entity: EmailAlertSettingPM;
                    entity = this.MapJsonToEntityPM(allAlerts[k], false);
                    mappedAlerts.push(entity);
                }

                var env: EmailAlertSettingsEnvelope = new EmailAlertSettingsEnvelope();
                env.EmailAlerts = [];
                env.EmailAlerts = mappedAlerts;
                var postString: string;
                postString = JSON.stringify(env);
                console.log(postString);
                return this._http.put(this._apiUrl + "?tenant="+ tenant, postString,
                    { headers: authHeader }).map((response) => {
                        var mappedAlerts: Array<EmailAlertSettingPM> = [];
                        var env: EmailAlertSettingsEnvelope = response.json();
                        for (var k in env.EmailAlerts) {

                            var entity: EmailAlertSettingPM;
                            entity = this.MapJsonToEntityPM(env.EmailAlerts[k]);
                            mappedAlerts.push(entity);
                        }

                        
                        serviceResponse.Result = mappedAlerts;


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

    //insert(entityPM: EmailAlertSettingPM) {

    //    return Observable.defer(() => {

    //        var authHeader = new Headers();
    //        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //        authHeader.append('Content-Type', 'application/json');

    //        var validator: ClassLevelValidator;

    //        validator = new ClassLevelValidator();

    //        var errorsArray = [];


    //        var response: EntityPMServiceResponse;
    //        response = new EntityPMServiceResponse();
    //        if (errorsArray.length == 0) {
    //            var mappedEntity: EmailAlertSettingPM;
    //            mappedEntity = this.MapJsonToEntityPM(entityPM, false);

    //            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
    //                { headers: authHeader }).map((res) => {
    //                    var pm = res.json();
    //                    if (pm) {
    //                        var mappedResult: EmailAlertSettingPM;
    //                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
    //                        response.Result = mappedResult;
    //                    }

    //                    return response;

    //                }).catch(ServiceHelper.HandleServiceError);
           
    //        }
    //        else {

    //            response.HasError = true;
    //            response.ErrorsArray = errorsArray;

    //            return Observable.of(response);

    //        }
    //    }

    //    );
    //}


    //update(entityPM: EmailAlertSettingPM) {


    //    return Observable.defer(() => {

    //        var authHeader = new Headers();
    //        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //        authHeader.append('Content-Type', 'application/json');

    //        var validator: ClassLevelValidator;

    //        validator = new ClassLevelValidator();

    //        var errorsArray = [];


    //        var serviceResponse: ServiceResponse;
    //        serviceResponse = new ServiceResponse();
    //        if (errorsArray.length == 0) {
    //            var mappedEntity: EmailAlertSettingPM;
    //            mappedEntity = this.MapJsonToEntityPM(entityPM, false);

    //            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
    //                { headers: authHeader }).map((res) => {
    //                    var pm = res.json();
    //                    if (pm) {
    //                        var mappedResult: EmailAlertSettingPM;
    //                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
    //                        serviceResponse.Result = mappedResult;
    //                    }


    //                    return serviceResponse;

    //                }).catch(ServiceHelper.HandleServiceError);
    //        }
    //        else {

    //            serviceResponse.HasError = true;
    //            serviceResponse.ErrorsArray = errorsArray;

    //            return Observable.of(serviceResponse);

    //        }
    //    }

    //    );

    //}





    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: EmailAlertSettingPM = null) {


        if (!entityPM) {

            entityPM = new EmailAlertSettingPM();
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
        var entityPM: EmailAlertSettingPM;
        entityPM = new EmailAlertSettingPM();

        return entityPM;
    }



}

export class EmailAlertSettingsEnvelope {
    public EmailAlerts: EmailAlertSettingPM[];
}
