
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import { AgentSharedDocumentPM} from '../../EntityPMs/AgentSharedDocumentPM';



@Injectable()
export class AgentSharedDocumentExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AgentSharedDocumentExtended';
    }

    PostSharedDocuments(shipmentShareDocumentsDataLists: any, entityId:string) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(this._apiUrl + '/PostSharedDocuments?entityId=' + entityId, JSON.stringify(shipmentShareDocumentsDataLists),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }
}

