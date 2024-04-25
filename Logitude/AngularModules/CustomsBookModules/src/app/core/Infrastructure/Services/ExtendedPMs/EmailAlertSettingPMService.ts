import { EmailAlertSettingPM } from '../../EntityPMs/EmailAlertSettingPM';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class EmailAlertSettingPMService {
    private httpClient: HttpClient;
    private apiUrl: string;

    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EmailAlertSetting';
    }

    getAllEmailAlerts(tenant: number) {
        var url = this.apiUrl + '/GetEmailAlertSettingsByTenant?' + 'tenant=' + tenant;
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    updateAllAlerts(allAlerts: EmailAlertSettingPM[], tenant: number) {
        var url = this.apiUrl + "?tenant=" + tenant;
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        var env: EmailAlertSettingsEnvelope = new EmailAlertSettingsEnvelope();

        env.EmailAlerts = [];
        env.EmailAlerts = allAlerts;
        var postString: string;
        postString = JSON.stringify(env);
        console.log(postString);

        return this.httpClient.put(url, postString, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
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
