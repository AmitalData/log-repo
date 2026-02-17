import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo'; 

@Injectable()

export class LoadTestService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsLoadTest';

    }
    
    

    PostCourierBOLRequest(entity: any) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCourierBOLRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    
    

    GetNewCustomFile(tenant: number, ConsigneeId: string, CustomerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetNewCustomFile/?' + '&tenant=' + tenant + '&ConsigneeId=' + ConsigneeId + '&CustomerId=' + CustomerId,
                { headers: authHeader }).map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();

                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetDeclarationFromFileNo(tenant: number, fileNo: string, FilingCopy:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetDeclarationFromFileNo/?' + '&tenant=' + tenant + '&fileNo=' + fileNo + '&FilingCopy=' +  FilingCopy,
                { headers: authHeader }).map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();

                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetTicket(tenant: number, declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(this._apiUrl + '/GetTicket/?' + '&tenant=' + tenant + '&declarationId=' + declarationId,
                { headers: authHeader }).map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();

                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }
    

}
