import { Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()
export class CarrierAreaExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CarrierAreaExtended';
    }

    DownloadCarrierAreaPorts(carrierAreaId: string) {
        var url = this._apiUrl + '/GetDownloadCarrierAreaPorts?carrierAreaId=' + carrierAreaId;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    PostUploadCarrierAreaPortsExcelFile(filter: CarrierAreaParameters) {
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadCarrierAreaPortsExcelFile", JSON.stringify(filter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class CarrierAreaParameters {
    Tenant: number;
    FileData: string;    
    CarrierAreaId: string;
    FileName: string;
    FileExtension: string;
    TransportMode: string;
}
