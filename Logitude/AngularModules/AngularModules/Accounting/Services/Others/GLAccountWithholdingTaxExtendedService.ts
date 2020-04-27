import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { GLAccountWithholdingTaxPM } from '../../EntityPMs/GLAccountWithholdingTaxPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

export class GLAccountWithholdingTaxExtendedPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GLAccountingWithholdingTax';
    }

    GetDeductionPercentage(vendorId: string, date: Date) {
        return defer(() => {
             
            return defer(() => {
                return this._http.get(this._apiUrl + '/GetDeductionPercentage?vendorId=' + vendorId + '&registerDate=' + ServiceHelper.GetDateString(date), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                        var myResult = response;
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = myResult;
                        return serviceResponse;
                    }),catchError(ServiceHelper.HandleServiceError));
            });
        });
    }
}

export class GLAccountingWithholdingItem {
    public Percentage: number;
    public IsDefault: boolean;
}
