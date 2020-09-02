import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {CashBookPM} from '../../EntityPMs/CashBookPM';
import {CashBookLinePM} from '../../EntityPMs/CashBookLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 

@Injectable()
export class CargoTrackingExtendedPMService {
 
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
   
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CargoTrackingBuildTables';
    }

    PostCargoTrackingBuilder( Args:CargoTrackingArgs) {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
      return this.httpClient.post(this._apiUrl +'/PostCargoTrackingBuilder', Args ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    
    }
 
   

}
export class CargoTrackingArgs {

    public FromDate:Date;
    public ToDate:Date;
    public Tenant:number;

}