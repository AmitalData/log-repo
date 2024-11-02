
import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()

export class ShipmentComputedFieldExtendedService {
    private _httpClient: HttpClient
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentComputedFieldExtended';
    }

 
    GetMarkCompleteDepositionRequest(id: string, directionId: string, forwardershipmentNumber: string, forwarderPartnerId:string) {

        var url = this._apiUrl + '/GetMarkCompleteDepositionRequest?id=' + id + "&directionId=" + directionId  + "&forwardershipmentNumber=" + forwardershipmentNumber + "&forwarderPartnerId=" + forwarderPartnerId ;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }




}
