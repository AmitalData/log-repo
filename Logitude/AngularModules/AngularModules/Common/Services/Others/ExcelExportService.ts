import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable}     from 'rxjs/Rx';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()
export class ExcelExportService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
    }

    ExportRoleFeaturesToCSVFile() {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getexportrolefeaturestocsvfile'  ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }
    ExportFeaturesToCSVFile() {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExportFeatures';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetExportFeaturesToCSVFile',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }


    ImportFeaturePackages(parameter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExportFeatures';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postImportFeaturePackages', JSON.stringify(parameter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }

    ImportClockTimeData(parameter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ClockTimeGeneralDomain';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postImportingClockTimeData', JSON.stringify(parameter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


    ImportRoleFeatures(parameter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {

            return this._http.post(this._apiUrl + '/postimportrolefeatures', JSON.stringify(parameter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }
}

