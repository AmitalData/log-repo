import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

import {AccountingPeriodPM} from '../../EntityPMs/AccountingPeriodPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
//import {AccountingPeriodLinePM} from '../../EntityPMs/AccountingPeriodLinePM';
//import {AccountingPeriodValidator} from '../../Validators/AccountingPeriodValidator';

@Injectable()

export class AccountingPeriodExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/accountingPeriods';
    }


    createDefaultPeriods(year: number) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.post(this._apiUrl + "/PostCreatePeriodsForYear?year=" + year, null ,{ headers: authHeader }).map((res) => {

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });
    }


    //MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AccountingPeriodPM = null) {


    //    if (!entityPM) {

    //        entityPM = new AccountingPeriodPM();
    //    }

    //    var jsonPMKeys = Object.keys(jsonPM);

    //    for (var key in jsonPMKeys) {
    //        if (jsonPMKeys[key] === "UIProperties") {

    //            continue;
    //        }
    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];
    //    }
    //    return entityPM;
    //}

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