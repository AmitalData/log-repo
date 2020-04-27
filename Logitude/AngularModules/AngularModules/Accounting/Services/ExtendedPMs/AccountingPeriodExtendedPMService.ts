import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import {AccountingPeriodPM} from '../../EntityPMs/AccountingPeriodPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

 

@Injectable()

export class AccountingPeriodExtendedPMService {
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/accountingPeriods';
    }


    createDefaultPeriods(year: number) {

        return defer(() => {

      

            var serviceResponse: ServiceResponse = new ServiceResponse();
            return this.httpClient.post(this._apiUrl + "/PostCreatePeriodsForYear?year=" + year, null ,  ServiceHelper.GetHttpHeaders()).pipe(
                map(res => {
               
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
     
        });
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
