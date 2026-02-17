import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 


@Injectable()
export class BackUpService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BackUp';
    }




  BackUpForClientData(tenant: number) {

       var s = true;
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&s=' + s
            , {
                headers: authHeader,

            }).map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }




  SetDatabaseDataBackupNotReady(tenant: number) {

      var authHeader = new Headers();
      authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
      authHeader.append('Content-Type', 'application/json');
      return this._http.get(this._apiUrl + '?id=' + tenant
          , {
              headers: authHeader,

          }).map(response => {
              var pmresponse: ServiceResponse;
              pmresponse = new ServiceResponse();

              pmresponse.Result = response.json();
              return pmresponse;
          }).catch(ServiceHelper.HandleServiceError);
  }



  CheckIfDatabaseBackupIsBuilt(tenant: number) {

      var authHeader = new Headers();
      authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
      authHeader.append('Content-Type', 'application/json');
      return this._http.get(this._apiUrl + '?a=' + '&tenant=' + tenant
          , {
              headers: authHeader,

          }).map(response => {
              var pmresponse: ServiceResponse;
              pmresponse = new ServiceResponse();

              pmresponse.Result = response.json();
              return pmresponse;
          }).catch(ServiceHelper.HandleServiceError);
  }





}

