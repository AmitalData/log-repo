
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

@Injectable()

export class ShipmentComputedFieldExtendedService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentComputedFieldExtended';
    }

 
    GetMarkCompleteDepositionRequest(id: string, directionId: string, forwardershipmentNumber: string, forwarderPartnerId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMarkCompleteDepositionRequest?id=' + id + "&directionId=" + directionId  + "&forwardershipmentNumber=" + forwardershipmentNumber + "&forwarderPartnerId=" + forwarderPartnerId ;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var result = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }




}
