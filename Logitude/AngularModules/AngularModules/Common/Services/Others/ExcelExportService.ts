import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()
export class ExcelExportService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
    }

    ExportRoleFeaturesToCSVFile() {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getexportrolefeaturestocsvfile'  , { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);

    }
    ExportFeaturesToCSVFile() {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExportFeatures';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetExportFeaturesToCSVFile', { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);

    }


    ImportFeaturePackages(parameter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExportFeatures';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postImportFeaturePackages', JSON.stringify(parameter), {
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

    ImportClockTimeData(parameter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ClockTimeGeneralDomain';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postImportingClockTimeData', JSON.stringify(parameter), {
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


    ImportRoleFeatures(parameter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {

            return this._http.post(this._apiUrl + '/postimportrolefeatures', JSON.stringify(parameter), {

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

