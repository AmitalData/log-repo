import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ReferantExceptionPM } from '../../EntityPMs/ReferantExceptionPM';

@Injectable()

export class ClientItemExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ClientItemExtendedControllerExtended';
    }

    GetClientItemPM(itemCode: string, exporterCode: string, tenant: number) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return this._http.get(this._apiUrl + '/GetClientItem/?' + '&itemCode=' + itemCode + '&exporterCode=' + exporterCode + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetClientItemDescriptionPM(itemDescription: string, exporterCode: string, tenant: number) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return this._http.get(this._apiUrl + '/GetClientItemDescription/?' + '&itemDescription=' + itemDescription + '&exporterCode=' + exporterCode + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
