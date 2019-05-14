import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {JournalList} from '../../EntityLists/JournalList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {JournalLinePM} from '../../EntityPMs/JournalLinePM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';

@Injectable()

export class AccountingOpService {
    

    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingOp';//AccountingOpController
    }
    Generate1000(email: string): any {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetGenerate1000?email=' + email;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var result = response.json();
                var entity: JournalPM;
                if (result) {
                    //entity = this.MapJsonToEntityPM(result);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }
    
    PutSystem1000File(fileUploadParamerter: ImageParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutSystem1000File', JSON.stringify(fileUploadParamerter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


}
