import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ServiceHelper } from "../../../Infrastructure/Utilities/ServiceHelper";
import { defer, Observable } from 'rxjs';
import { ServiceResponse } from "../../../Infrastructure/DataContracts/ServiceResponse";
import { SessionInfo } from "../../../Infrastructure/Utilities/SessionInfo";
import { catchError, map } from "rxjs/operators";

@Injectable()

export class NewQuoteOPWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NewQuoteOPWebService';
    }
    /*example :-  var service = new DeclarationWebService();  PTERMID=code
    service.GetETBPAYTRtemList(this.PTERMID , this.costSearchText, 30, true).subscribe((res: ServiceResponse) => {
    });*/
    GetETBPAYTRitemList(PTERMID: string, search: string, top: number, isSearchNULLVendor: boolean):Observable<ServiceResponse> {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetETBPAYTRitemList/?PTERMID=" + PTERMID
                + "&search=" + search
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCarriersItemsList(DIRECTIONID: string, TRANSPORTMODEID: string, search: string, top: number, isSearchNULLVendor: boolean) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCarriersItemsList/?DIRECTIONID=" + DIRECTIONID
                + "&TRANSPORTMODEID=" + TRANSPORTMODEID
                + "&search=" + search
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSpecialServiceItemsList(DIRECTIONID: string, TRANSPORTMODEID: string, search: string, top: number, isSearchNULLVendor: boolean) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSpecialServiceItemsList/?DIRECTIONID=" + DIRECTIONID
                + "&TRANSPORTMODEID=" + TRANSPORTMODEID
                + "&search=" + search
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
