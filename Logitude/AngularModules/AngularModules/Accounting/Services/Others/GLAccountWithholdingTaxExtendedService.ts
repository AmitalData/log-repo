import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { GLAccountWithholdingTaxPM } from '../../EntityPMs/GLAccountWithholdingTaxPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

export class GLAccountWithholdingTaxExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GLAccountingWithholdingTax';
    }

    GetDeductionPercentage(vendorId: string, date: Date) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Observable.defer(() => {
                return this._http.get(this._apiUrl + '/GetDeductionPercentage?vendorId=' + vendorId + '&registerDate=' + ServiceHelper.GetDateString(date), { headers: authHeader })
                    .map(response => {
                        var myResult = response.json();
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = myResult;
                        return serviceResponse;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });
    }
}

export class GLAccountingWithholdingItem {
    public Percentage: number;
    public IsDefault: boolean;
}
