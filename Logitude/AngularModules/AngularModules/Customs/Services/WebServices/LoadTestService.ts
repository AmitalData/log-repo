import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo'; 

@Injectable()

export class LoadTestService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsLoadTest';

    }
    
    

    PostCourierBOLRequest(entity: any) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCourierBOLRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    
    

    GetNewCustomFile(tenant: number, ConsigneeId: string, CustomerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetNewCustomFile/?' + '&tenant=' + tenant + '&ConsigneeId=' + ConsigneeId + '&CustomerId=' + CustomerId,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDeclarationFromFileNo(tenant: number, fileNo: string, FilingCopy:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetDeclarationFromFileNo/?' + '&tenant=' + tenant + '&fileNo=' + fileNo + '&FilingCopy=' +  FilingCopy,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetTicket(tenant: number, declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetTicket/?' + '&tenant=' + tenant + '&declarationId=' + declarationId,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    

}
