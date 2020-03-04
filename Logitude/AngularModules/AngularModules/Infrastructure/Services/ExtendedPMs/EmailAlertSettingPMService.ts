import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { EmailAlertSettingPM } from '../../EntityPMs/EmailAlertSettingPM';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Token': ServiceHelper.GetLoggedUserToken()
    }),
};

@Injectable()
export class EmailAlertSettingPMService {
    private httpClient: HttpClient;
    private apiUrl: string;

    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EmailAlertSetting';
    }

    getAllEmailAlerts(tenant: number): Observable<ServiceResponse> {
        var url = this.apiUrl + '/GetEmailAlertSettingsByTenant?' + 'tenant=' + tenant;

        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        return this.httpClient.get(url, httpOptions).pipe(
            map(response => {

                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

    updateAllAlerts(allAlerts: EmailAlertSettingPM[], tenant: number): Observable<ServiceResponse> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Token': ServiceHelper.GetLoggedUserToken()
            }),
        };

        var url = this.apiUrl + "?tenant=" + tenant;
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        var env: EmailAlertSettingsEnvelope = new EmailAlertSettingsEnvelope();
        env.EmailAlerts = [];
        env.EmailAlerts = allAlerts;
        var postString: string;
        postString = JSON.stringify(env);
        console.log(postString);

        return this.httpClient.put(url, postString, httpOptions).pipe(
            map(response => {
                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
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
